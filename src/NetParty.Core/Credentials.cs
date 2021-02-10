using CommandLine;
using NetParty.Core.Database;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace NetParty.Core
{
    /// <summary>
    /// Class for keeping credentials
    /// </summary>
    [Verb("config", HelpText = "API login configuration.")]
    public class Credentials
    {
        #region Properties

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [JsonIgnore]
        public int Id => 1; //Only 1 login can be saved

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        [JsonProperty("username")]
        [Option("username", Required = true, HelpText = "Login username.")]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [JsonProperty("password")]
        [Option("password", Required = true, HelpText = "Login password.")]
        public string Password { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Saves to database.
        /// </summary>
        /// <param name="databaseConnectionString">The database connection string.</param>
        /// <returns></returns>
        public async Task<Credentials> SaveToDatabase(IDatabase database)
        {
            await database.ExecuteAsync(string.Format("INSERT OR REPLACE INTO Credentials (Id, UserName, Password) values ('{0}','{1}','{2}')", Id, Username, Password));
            return this;
        }

        #endregion Methods
    }
}
