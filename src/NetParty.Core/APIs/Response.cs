using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetParty.Core.APIs
{
    /// <summary>
    /// Response interface with properties for every response to have.
    /// </summary>
    public interface Response
    {
        #region Properties

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        string Message { get; set; }

        #endregion Properties
    }
}
