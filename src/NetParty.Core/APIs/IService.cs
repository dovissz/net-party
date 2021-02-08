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

        Task<Credentials> SaveCredentials(Credentials credentials);
        Task<AuthResponse> GetToken(Credentials credentials);
        Task<GetServersResponse> GetServers(ServerDataLocation dataLocation, string token);

        #endregion Methods
    }
}
