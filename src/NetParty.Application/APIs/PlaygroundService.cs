using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using NetParty.Core;
using NetParty.Core.APIs;
using NetParty.Core.Servers;
using Newtonsoft.Json;
using RestSharp;

namespace NetParty.Application.APIs
{
    public class PlaygroundService : IService
    {
        readonly IDbConnection _database;
        readonly string linkToServices = "http://playground.tesonet.lt/v1/";
        public PlaygroundService(IDbConnection database)
        {
            _database = database;
        }

        public string Token { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        async Task<Credentials> IService.SaveCredentials(Credentials credentials)
        {
            if (credentials != null)
                return await credentials.SaveToDatabase(_database);
            throw new NotImplementedException();
        }

        async Task<GetServersResponse> IService.GetServers(ServerDataLocation dataLocation, string token)
        {
            if (dataLocation == ServerDataLocation.Local)
            {
                using (_database)
                {
                    //var servers = await _database.QueryAsync<Server>("select * from Servers");    //ToDo: padaryt kad automatiskai susikurtu strukturos
                    using (IDbConnection conn = new SQLiteConnection(_database.ConnectionString))
                    {
                        var srvrs = conn.Query<Server>("select * from Servers");
                    }
                    var servers = _database.Query<Server>("select * from Servers");
                    var localResult = new GetServersResponse();
                    localResult.ServersList.AddRange(servers);
                    return localResult;
                }
            }
            var client = new RestClient(linkToServices);
            var request = new RestRequest("servers", DataFormat.Json);
            request.AddHeader("authorization", "Bearer " + token);
            var result = await client.GetAsync<GetServersResponse>(request);
            result.ServersList.ForEach(server => server.SaveToDatabase(_database));
            return result;
        }

        async Task<AuthResponse> IService.GetToken(Credentials credentials)
        {
            using (_database)
                credentials = _database.Query<Credentials>("select * from Credentials").FirstOrDefault();

            var client = new RestClient(linkToServices);
            var request = new RestRequest("tokens", Method.POST, DataFormat.Json);
            request.AddJsonBody(JsonConvert.SerializeObject(credentials));
            var result = await client.PostAsync<AuthResponse>(request);
            return result;
        }

    }
}
