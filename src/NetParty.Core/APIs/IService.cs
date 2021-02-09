using NetParty.Core.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetParty.Core.APIs
{
    public interface IService
    {
        #region Methods

        /// <summary>
        /// Saves the credentials.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <returns></returns>
        Task<Credentials> SaveCredentials(Credentials credentials);

        /// <summary>
        /// Gets the token.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <returns></returns>
        Task<AuthResponse> GetToken(Credentials credentials);

        /// <summary>
        /// Gets the servers.
        /// </summary>
        /// <param name="dataLocation">The data location.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task<GetServersResponse> GetServers(ServerDataLocation dataLocation, string token);

        #endregion Methods
    }
}
