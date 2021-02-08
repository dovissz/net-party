using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetParty.Core.Servers
{
    public class Server
    {
        #region Properties

        public string Name { get; set; }
        public int Distance { get; set; }

        #endregion Properties

        #region Methods

        public void SaveToDatabase(IDbConnection _database)
        {
            using (_database)
                _database.ExecuteAsync(string.Format("INSERT OR REPLACE INTO Servers (Name, Distance) values ({0},{1})", Name, Distance));
        }

        #endregion Methods
    }
}
