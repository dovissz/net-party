using NetParty.Core.Database;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public async Task<Server> SaveToDatabase(IDatabase database)
        {
            await database.ExecuteAsync(string.Format("INSERT OR REPLACE INTO Servers (Name, Distance) values ('{0}','{1}')", Name, Distance));
            return this;
        }

        #endregion Methods
    }
}
