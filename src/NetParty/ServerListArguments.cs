using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using NetParty.Core.Servers;

namespace NetParty
{
    [Verb("server_list", HelpText = "Used to get list of servers.")]
    public class ServerListArguments
    {
        #region Properties

        [Option("local", HelpText = "Servers which are saved localy.")]
        public bool Local { get; set; }

        public ServerDataLocation DataLocation => Local ? ServerDataLocation.Local : ServerDataLocation.Database;

        #endregion Properties
    }
}
