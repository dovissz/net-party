using CommandLine;
using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetParty.Core
{
    [Verb("config", HelpText = "API login configuration.")]
    public class Credentials
    {
        #region Properties
        
        [Option("username", Required = true, HelpText = "Login username.")]
        public string Username { get; set; }

        [Option("password", Required = true, HelpText = "Login password.")]
        public string Password { get; set; }

        #endregion Properties

        #region Methods

        public async Task<Credentials> SaveToDatabase(IDbConnection _database)
        {
            using (_database)
                _database.Execute(string.Format("INSERT OR REPLACE INTO Credentials (UserName, Password) values ({0},{1},{2})", Username, Password));
            using (_database)
                await _database.ExecuteAsync(string.Format("INSERT OR REPLACE INTO Credentials (Id, UserName, Password) values ({0},{1},{2})", Username, Password));
            return this;
        }

        #endregion Methods
    }
}
