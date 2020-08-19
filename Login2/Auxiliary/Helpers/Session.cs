using Login2.Auxiliary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login2.Auxiliary.Helpers
{
    public class Session
    {
        private int _accountID;
        private Roles _role;

        public Session()
        {
        }
        public Session(int AccountID, Roles Role)
        {
            _accountID = AccountID;
            _role = Role;
        }
        public void SetData(int AccountID, Roles Role)
        {
            _accountID = AccountID;
            _role = Role;
        }
        public int getAccountID()
        {
            return _accountID;
        }
        public Roles getRole()
        {
            return _role;
        }
        public void clear()
        {
            _accountID = -1;
        }
    }
}
