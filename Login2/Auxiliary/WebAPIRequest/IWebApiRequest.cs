using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login2.Auxiliary.WebAPIRequest
{
    public interface IWebApiRequest
    {
        string Get(string uri, ParamObject param = null, string jwtToken = null);
        string Post(string uri, object param = null, string jwtToken = null);
    }
}
