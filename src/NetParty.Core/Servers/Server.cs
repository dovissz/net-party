using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetParty.Core.Servers
{
    [JsonObject]
    public class Server
    {
        #region Properties

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("distance")]
        public int Distance { get; set; }

        #endregion Properties

        #region Methods

        public async Task<Server> SaveToDatabase(string databaseConnectionString)
        {
            using (IDbConnection conn = new SQLiteConnection(databaseConnectionString))
                await conn.ExecuteAsync(string.Format("INSERT OR REPLACE INTO Servers (Name, Distance) values ('{0}','{1}')", Name, Distance));
            return this;
        }

        #endregion Methods
    }
}
