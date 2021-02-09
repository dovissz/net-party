using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetParty.Core.APIs
{
    /// <summary>
    /// Class to keep authorization response information
    /// </summary>
    /// <seealso cref="NetParty.Core.APIs.Response" />
    public class AuthResponse : Response
    {
        #region Properties

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        public string Token { get; set; }

        private string message;
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get => message; set => message = value; }

        #endregion Properties
    }
}
