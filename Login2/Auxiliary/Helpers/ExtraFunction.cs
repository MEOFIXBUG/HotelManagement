using Login2.Auxiliary.Enums;
using Login2.ViewModels;
using Login2.ViewModels.HumanResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login2.Auxiliary.Helpers
{
    public static class ExtraFunction
    {
        public static List<string> featureOfRole(int roleID)
        {
            List<string> items = new List<string>();
            switch (roleID)
            {
                case (int)Roles.HumanResources:
                    // code block
                    items= ConvertEnumToList.GetListOfDescription<HumanResourcesFeatures>();
                    break;
                case (int)Roles.Sales:
                    // code block
                    items = ConvertEnumToList.GetListOfDescription<SalesFeatures>();
                    break;
                default:
                    // code block
                    break;
            }
            return items;
        }
        public static MyBaseViewModel getUserControl(int index, int roleID)
        {
            MyBaseViewModel item=new MyBaseViewModel();
            switch (roleID)
            {
                case 1:
                    switch (index)
                    {
                        case (int)0:
                            // code block
                            item = new StaffListViewModel();
                            break;
                        case (int)1:
                            // code block
                            item = new InsertStaffViewModel();
                            break;
                        default:
                            // code block
                            item = new StaffListViewModel();
                            break;
                    }
                    break;
                default:
                    // code block
                    break;
            }
           
            return item;
        }
    }
}
