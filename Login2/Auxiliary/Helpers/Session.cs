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
        private int _roleID;

        public Session()
        {
        }
        public Session(int AccountID, int RoleID)
        {
            _accountID = AccountID;
            _roleID = RoleID;
        }
        public void SetData(int AccountID, int RoleID)
        {
            _accountID = AccountID;
            _roleID = RoleID;
        }
        public int getAccountID()
        {
            return _accountID;
        }
        public int getRoleID()
        {
            return _roleID;
        }
    }
}
