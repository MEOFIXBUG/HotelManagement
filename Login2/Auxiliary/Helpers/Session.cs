using Login2.Auxiliary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Login2.Auxiliary.Helpers
{
    public class Session
    {
        private int _accountID;
        private Roles _role;
        public int AccountID
        {
            get
            {
                return _accountID;
            }
            set
            {
                _accountID = value;
            }
        }

        public Roles Role
        {
            get
            {
                return _role;
            }
            set
            {
                _role = value;
            }
        }
        public Session()
        {
        }
        public static Session GetCurrentSingleton()
        {
            Session session;

            if (null == System.Windows.Application.Current.Properties["Session"])
            {
                //No current session object exists, use private constructor to 
                // create an instance, place it into the session
                session = new Session();
                System.Windows.Application.Current.Properties["Session"] = session;
            }
            else
            {
                //Retrieve the already instance that was already created
                session = (Session)System.Windows.Application.Current.Properties["Session"];
            }

            //Return the single instance of this class that was stored in the session
            return session;
        }
        public static void Dispose()
        {
            //Cleanup this object so that GC can reclaim space
            System.Windows.Application.Current.Properties["Session"]=null;
        }
    }
}
