using Autofac;
using CommandLine;
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

        static ContainerBuilder _builder;

        #endregion Properties

        #region Methods

        static void Main(string[] args)
        {
            try
            {
                _builder = new ContainerBuilder();
                _builder.Register(it => new SQLiteConnection(ConfigurationManager.ConnectionStrings["Default"].ConnectionString)).As<IDbConnection>();
                _builder.Register(it => new RestClient(ConfigurationManager.AppSettings["PlaygroundServiceAddress"])).As<IRestClient>();
                _builder.RegisterType<PlaygroundService>().As<IService>();
                var parser = new Parser()
                    .ParseArguments(args, new Type[] { typeof(ServerListArguments), typeof(Credentials) })
                    .WithParsed(ExecuteCmdRequest)
                    .WithNotParsed(errs => Environment.Exit(0)); //ToDo: uzlogint ir press any key to continue
            }
            catch (Exception ex)
            {
                //ToDo: log error
            }
        }

        private static void ExecuteCmdRequest(object obj)
        {
            var container = _builder.Build();
            if (obj is Credentials)
            {
                try
                {
                    var scope = container.BeginLifetimeScope();
                    var plugin = scope.Resolve<IService>();
                    plugin.SaveCredentials(obj as Credentials);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                //ToDo: exit and logging
            }
            else if (obj is ServerListArguments)
            {
                try
                {
                    using (var scope = container.BeginLifetimeScope())
                    {
                        var plugin = scope.Resolve<IService>();
                        var token = plugin.GetToken(obj as Credentials);
                        token.Wait();
                        var servers = plugin.GetServers((obj as ServerListArguments).DataLocation, token.Result.Token);
                        servers.Wait(); //ToDo: Wait?????
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }

        #endregion Methods

    }
}
