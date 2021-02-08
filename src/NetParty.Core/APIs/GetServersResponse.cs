using NetParty.Core.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetParty.Core.APIs
{
    public class GetServersResponse : Response
    {
        #region Properties

        public List<Server> ServersList { get; set; }

        #endregion Properties
    }
}
