using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetParty.Core.Database
{
    /// <summary>
    /// Sqlite database commands
    /// </summary>
    public class SqliteDatabase : IDatabase
    {
        #region Constructors

        /// <summary>
        /// Sqlite database constructor
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="databaseFileName"></param>
        public SqliteDatabase(string connectionString, string databaseFileName)
        {
            this.databaseConnectionString = connectionString;
            this.databaseFileName = databaseFileName;
            InitializeDatabase();
        }

        #endregion Constructors

        #region Properties

        private readonly string databaseConnectionString;
        private readonly string databaseFileName;

        #endregion Properties

        #region Methods

        private void InitializeDatabase()
        {
            if (!File.Exists(databaseFileName))
                SQLiteConnection.CreateFile(databaseFileName);
            using (IDbConnection conn = new SQLiteConnection(databaseConnectionString))
            {
                conn.Execute("CREATE TABLE IF NOT EXISTS \"Credentials\" ( \"Id\" INTEGER, \"username\" TEXT, \"password\" TEXT, PRIMARY KEY(\"Id\") )");
                conn.Execute("CREATE TABLE IF NOT EXISTS \"Servers\"( \"Name\"  TEXT, \"Distance\" INTEGER, PRIMARY KEY(\"Name\") )");
            }
        }

        /// <summary>
        /// Queries database asynchroniously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Task<IEnumerable<T>> QueryAsync<T>(string sql) where T : class
        {
            using (IDbConnection conn = new SQLiteConnection(databaseConnectionString))
                return conn.QueryAsync<T>(sql);
        }

        /// <summary>
        /// Executes async query
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Task ExecuteAsync(string sql)
        {
            using (IDbConnection conn = new SQLiteConnection(databaseConnectionString))
                return conn.ExecuteAsync(sql);
        }

        #endregion Methods

    }
}
