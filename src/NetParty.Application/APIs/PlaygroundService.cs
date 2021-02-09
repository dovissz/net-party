using System;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dapper;
using log4net;
using NetParty.Core;
using NetParty.Core.APIs;
using NetParty.Core.Servers;
using Newtonsoft.Json;
using RestSharp;

namespace NetParty.Application.APIs
{
    /// <summary>
    /// This class describes Playground service actions
    /// </summary>
    /// <seealso cref="NetParty.Core.APIs.IService" />
    public class PlaygroundService : IService
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaygroundService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public PlaygroundService(ILog logger)
        {
            this._logger = logger;
            databaseConnectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            linkToServices = ConfigurationManager.AppSettings["PlaygroundServiceAddress"];
            InitializeDatabase(ConfigurationManager.AppSettings["DatabaseFileName"]);
        }

        #endregion Constructors

        #region Properties 

        private readonly ILog _logger;
        readonly string databaseConnectionString;
        readonly string linkToServices;

        #endregion Properties

        #region Methods 

        /// <summary>
        /// Saves the credentials.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Credentials not provided.</exception>
        async Task<Credentials> IService.SaveCredentials(Credentials credentials)
        {
            if (credentials == null)
                throw new Exception("Credentials not provided.");
            _logger.Info("Saving credentials.");
            var result = await credentials.SaveToDatabase(databaseConnectionString);
            _logger.Info("Credentials were saved.");
            return result;
        }

        /// <summary>
        /// Gets the servers.
        /// </summary>
        /// <param name="dataLocation">The data location.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        async Task<GetServersResponse> IService.GetServers(ServerDataLocation dataLocation, string token)
        {
            if (dataLocation == ServerDataLocation.Local)
            {
                using (IDbConnection conn = new SQLiteConnection(databaseConnectionString))
                {
                    _logger.Info("Reading local servers list.");
                    var servers = conn.Query<Server>("select * from Servers");
                    var localResult = new GetServersResponse();
                    localResult.AddRange(servers);
                    return localResult;
                }
            }
            _logger.Info("Downloading server list from server.");
            var client = new RestClient(linkToServices);
            var request = new RestRequest("servers", DataFormat.Json);
            request.AddHeader("authorization", "Bearer " + token);

            var requestResult = await client.ExecuteAsync<GetServersResponse>(request, Method.GET);
            var result = requestResult.StatusCode == HttpStatusCode.OK ? requestResult.Data : new GetServersResponse() { Message = requestResult.ErrorMessage };
            result.ForEach(async server => await server.SaveToDatabase(databaseConnectionString));
            return result;
        }

        /// <summary>
        /// Gets the token.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <returns></returns>
        async Task<AuthResponse> IService.GetToken(Credentials credentials)
        {
            if (credentials == null)
            {
                _logger.Info("Reading credentials from database.");
                using (IDbConnection conn = new SQLiteConnection(databaseConnectionString))
                    credentials = conn.Query<Credentials>("select * from Credentials").FirstOrDefault();
            }
            _logger.Info("Requesting token from server.");
            var client = new RestClient(linkToServices);
            var request = new RestRequest("tokens", Method.POST, DataFormat.Json);
            request.AddJsonBody(JsonConvert.SerializeObject(credentials));
            var result = await client.PostAsync<AuthResponse>(request);
            return result;
        }

        private void InitializeDatabase(string databaseFileName)
        {
            if (!File.Exists(databaseFileName))
                SQLiteConnection.CreateFile(databaseFileName);
            using (IDbConnection conn = new SQLiteConnection(databaseConnectionString))
            {
                conn.Execute("CREATE TABLE IF NOT EXISTS \"Credentials\" ( \"Id\" INTEGER, \"username\" TEXT, \"password\" TEXT, PRIMARY KEY(\"Id\") )");
                conn.Execute("CREATE TABLE IF NOT EXISTS \"Servers\"( \"Name\"  TEXT, \"Distance\" INTEGER, PRIMARY KEY(\"Name\") )");
            }
        }

        #endregion Methods

    }
}
