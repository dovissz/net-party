using Autofac;
using CommandLine;
using log4net;
using NetParty.Application.APIs;
using NetParty.Core;
using NetParty.Core.APIs;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetParty
{
    class Program
    {
        #region Properties

        private static ContainerBuilder _builder;
        private static ILog logger;

        #endregion Properties

        #region Methods

        static void Main(string[] args)
        {
            try
            {
                //Console.BackgroundColor = ConsoleColor.Green;
                logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                
                _builder = new ContainerBuilder();
                //_builder.Register(it => new SQLiteConnection(ConfigurationManager.ConnectionStrings["Default"].ConnectionString)).As<IDbConnection>();
                //_builder.Register(it => new RestClient(ConfigurationManager.AppSettings["PlaygroundServiceAddress"])).As<IRestClient>();
                
                _builder.Register(it => LogManager.GetLogger(typeof(Object))).As<ILog>();
                _builder.RegisterType<PlaygroundService>().As<IService>();

                //ILog logger = _builder.Build().Resolve<ILog>();
                //logger.Info("log4net OK");

                var parser = new Parser()
                    .ParseArguments(args, new Type[] { typeof(ServerListArguments), typeof(Credentials) })
                    .WithParsed(ExecuteCmdRequest)
                    .WithNotParsed(errs => throw new Exception("Failed to parse parameters."));
            }
            catch (Exception ex)
            {
                logger.Error("Program failed to execute.", ex);
            }
            finally
            {
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        private static void ExecuteCmdRequest(object obj)
        {
            var container = _builder.Build();
            if (obj is Credentials)
            {
                var scope = container.BeginLifetimeScope();
                var plugin = scope.Resolve<IService>();
                plugin.SaveCredentials(obj as Credentials);
            }
            else if (obj is ServerListArguments)
            {
                ServerListArguments serverArguments = obj as ServerListArguments;
                using (var scope = container.BeginLifetimeScope())
                {
                    var plugin = scope.Resolve<IService>();
                    var token = serverArguments.Local ? "" : plugin.GetToken(null).Result.Token;
                    //token.Wait();
                    var servers = plugin.GetServers(serverArguments.DataLocation, token);
                    if (string.IsNullOrEmpty(servers.Result.Message))
                    {
                        logger.Info("===== Servers list =====");
                        servers.Result.ForEach(it => logger.Info(it.Name));
                        logger.Info(string.Format("Total servers count: {0}", servers.Result.Count));
                    }
                    else
                        logger.Error(servers.Result.Message);
                }
            }
        }

        #endregion Methods

    }
}
