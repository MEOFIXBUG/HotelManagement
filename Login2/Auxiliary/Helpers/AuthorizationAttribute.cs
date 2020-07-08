using Login2.Auxiliary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login2.Auxiliary.Helpers
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AuthorizationAttribute : Attribute
    {
        public AuthorizationAttribute()
        {

        }

        public AuthorizationAttribute(AuthorizationType authorizationType, string role)
        {
            AuthorizationType = authorizationType;
            Role = role;
        }

        public string Role { get; set; }
        public AuthorizationType AuthorizationType { get; set; }
    }
}
