using NetParty.Core.Servers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetParty.Core.APIs
{

    /// <summary>
    /// Class to keep list of servers returned from API
    /// </summary>
    /// <seealso cref="System.Collections.Generic.List{NetParty.Core.Servers.Server}" />
    /// <seealso cref="NetParty.Core.APIs.Response" />
    public class GetServersResponse : List<Server> ,Response
    {

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GetServersResponse"/> class.
        /// </summary>
        public GetServersResponse()
        {
        }

        #endregion Constructors

        #region Properties

        private string message;
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        [JsonProperty("message")]
        public string Message { get => message; set => message = value; }

        #endregion Properties
    }
}
