using log4net;
using NetParty.Application.APIs;
using NetParty.Core;
using NetParty.Core.APIs;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetParty.UnitTests.Core
{
    public class ServiceUnitTest : TestSuite
    {
        #region Properties

        private string connectionString;

        #endregion Properties

        #region Methods

        [SetUp]
        public void Setup()
        {
            //connectionString = string.Format("Data Source={0};Version=3;", Path.Combine(binPath, "Party.db"));
        }

        

        #endregion Methods

    }
}
