using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using NetParty.Core.Servers;

namespace NetParty
{
    /// <summary>
    /// Class to keep arguments of server_list command
    /// </summary>
    [Verb("server_list", HelpText = "Used to get list of servers.")]
    public class ServerListArguments
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ServerListArguments"/> is local.
        /// </summary>
        /// <value>
        ///   <c>true</c> if local; otherwise, <c>false</c>.
        /// </value>
        [Option("local", HelpText = "Servers which are saved localy.")]
        public bool Local { get; set; }

        /// <summary>
        /// Gets the data location.
        /// </summary>
        /// <value>
        /// The data location.
        /// </value>
        public ServerDataLocation DataLocation => Local ? ServerDataLocation.Local : ServerDataLocation.Database;

        #endregion Properties
    }
}
