using Dapper;
using NetParty.Core.Database;
using NetParty.Core.Servers;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetParty.UnitTests.Core
{
    public class ServerUnitTest : TestSuite
    {
        #region Properties

        private string connectionString;
        private string pathToDatabaseFile;

        #endregion Properties

        #region Methods

        [SetUp]
        public void Setup()
        {
            pathToDatabaseFile = Path.Combine(binPath, "Party.db");
            connectionString = string.Format("Data Source={0};Version=3;", pathToDatabaseFile);
        }

        [Test]
        public async Task SaveAndLoadCredentialsTest()
        {
            using (IDbConnection conn = new SQLiteConnection(connectionString))
                Assert.AreEqual(0, conn.Query<Server>("select * from Servers").Count());

            var database = Substitute.For<SqliteDatabase>(connectionString, "Party.db");

            var server = Substitute.For<Server>();
            server.Name = "testServer";
            server.Distance = 9;
            await server.SaveToDatabase(database);

            Server loadedServer = null;
            using (IDbConnection conn = new SQLiteConnection(connectionString))
                loadedServer = conn.Query<Server>("select * from Servers").FirstOrDefault();
            Assert.AreEqual(server.Name, loadedServer.Name);
            Assert.AreEqual(server.Distance, loadedServer.Distance);
        }

        #endregion Methods

    }
}
