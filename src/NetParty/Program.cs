using Autofac;
using CommandLine;
using log4net;
using NetParty.Application.APIs;
using NetParty.Core;
using NetParty.Core.APIs;
using NetParty.Core.Database;
using NetParty.Core.Servers;
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
    /// <summary>
    /// Main class of the program
    /// </summary>
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
                string databaseConnectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
                string databaseFilename = ConfigurationManager.AppSettings["DatabaseFileName"];
                string linkToServices = ConfigurationManager.AppSettings["PlaygroundServiceAddress"];
                logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                _builder = InitializeBuilder(databaseConnectionString, databaseFilename, linkToServices);
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

        private static ContainerBuilder InitializeBuilder(string databaseConnectionString, string databaseFilename, string linkToServices)
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.Register(it => LogManager.GetLogger(typeof(Object)))
                .As<ILog>();
            builder.RegisterType<SqliteDatabase>()
                .As<IDatabase>()
                .WithParameter("connectionString", databaseConnectionString)
                .WithParameter("databaseFileName", databaseFilename);
            builder.RegisterType<PlaygroundService>()
                .WithParameter("linkToServices", linkToServices)
                .As<IService>();
            return builder;
        }

        private static void ExecuteCmdRequest(object obj)
        {
            var container = _builder.Build();
            if (obj is Credentials)
            {
                using (var scope = container.BeginLifetimeScope())
                {
                    var plugin = scope.Resolve<IService>();
                    plugin.SaveCredentials(obj as Credentials);
                }
            }
            else if (obj is ServerListArguments)
            {
                ServerListArguments serverArguments = obj as ServerListArguments;
                using (var scope = container.BeginLifetimeScope())
                {
                    var plugin = scope.Resolve<IService>();
                    ExecuteServersRequest(serverArguments, plugin);
                }
            }
        }

        private static void ExecuteServersRequest(ServerListArguments serverArguments, IService plugin)
        {
            string token = ResolveToken(serverArguments, plugin);
            var servers = plugin.GetServers(serverArguments.DataLocation, token);
            if (string.IsNullOrEmpty(servers.Result.Message))
            {
                logger.Info("===== Servers list =====");
                servers.Result.ForEach(it => logger.Info(it.Name));
                logger.Info(string.Format("Total servers count: {0}", servers.Result.Count));
            }
            else
                throw new Exception(servers.Result.Message);
        }

        private static string ResolveToken(ServerListArguments serverArguments, IService plugin)
        {
            string token = "";
            if (serverArguments.DataLocation == ServerDataLocation.Database)
            {
                var tokenResult = plugin.GetToken(null).Result;
                if (string.IsNullOrEmpty(tokenResult.Message))
                    token = tokenResult.Token;
                else
                    throw new Exception(tokenResult.Message);
            }
            return token;
        }

        #endregion Methods

    }
}
