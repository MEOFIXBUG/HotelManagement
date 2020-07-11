using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login2.ViewModels
{
    public class ProfileViewModel:MyBaseViewModel
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the Main_ViewModel class.
        /// </summary>
        /// 
        public ProfileViewModel()
        {

            /*
			 * Not much point in this case, but for the record, you can have 
			 * different data depending on if you're in design or runtime like this:
			 */


            // This will register our method with the Messenger class for incoming 
            // messages of type RefreshPeople.
            //Messenger.Default.Register<RefreshPeople>(this, (msg) => Execute_RefreshPeople(msg.PeopleToFetch));
        }
        #endregion

        #region Properties
      

        #endregion

        
    }
}
