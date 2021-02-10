using Dapper;
using NetParty.Core;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NetParty.Core.Database;
using Autofac;
using log4net;
using NetParty.Application.APIs;
using NetParty.Core.APIs;

namespace NetParty.UnitTests.Core
{
    [TestFixture]
    public class CredentialsUnitTest : TestSuite
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
                Assert.AreEqual(0, conn.Query<Credentials>("select * from Credentials").Count());

            var database = Substitute.For<SqliteDatabase>(connectionString, pathToDatabaseFile);
            var credentials = Substitute.For<Credentials>();
            credentials.Username = "testUser";
            credentials.Password = "testPass";
            await credentials.SaveToDatabase(database);

            Credentials loadedCredentials = null;
            using (IDbConnection conn = new SQLiteConnection(connectionString))
                loadedCredentials = conn.Query<Credentials>("select * from Credentials").FirstOrDefault();
            Assert.AreEqual(credentials.Username, loadedCredentials.Username);
            Assert.AreEqual(credentials.Password, loadedCredentials.Password);
        }

        #endregion Methods

    }
}
