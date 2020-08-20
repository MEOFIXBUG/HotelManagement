using Login2.Auxiliary.Enums;
using Login2.Models;
using Login2.ViewModels;

using Login2.ViewModels.HumanResources;
using Login2.ViewModels.Receptionist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login2.Auxiliary.Helpers
{
    public static class ExtraFunction
    {
        public static List<string> featureOfRole(Roles role)
        {
            List<string> items = new List<string>();
            switch (role)
            {
                case Roles.HumanResources:
                    // code block
                    items= ConvertEnumToList.GetListOfDescription<HumanResourcesFeatures>();
                    break;
                case Roles.Sales:
                    // code block
                    items = ConvertEnumToList.GetListOfDescription<SalesFeatures>();
                    break;
                case Roles.Accountings:
                    items = ConvertEnumToList.GetListOfDescription<AccountantFeatures>();
                    break;
                case Roles.Receiptions:
                    items = ConvertEnumToList.GetListOfDescription<ReceptionistFeatures>();
                    break;
                default:
                    // code block
                    break;
            }
            return items;
        }

        public static MyBaseViewModel getUserControl(int index, Roles role)
        {
            MyBaseViewModel item=new MyBaseViewModel();
            switch (role)
            {
                case Roles.HumanResources:
                    {
                        HumanResourcesFeatures feateture = (HumanResourcesFeatures)Enum.ToObject(typeof(HumanResourcesFeatures), index);
                        switch (feateture)
                        {
                            case HumanResourcesFeatures.staffList:
                                // code block
                                item = new StaffListViewModel();
                                break;
                            case HumanResourcesFeatures.addAccount:
                                // code block
                                item = new InsertStaffViewModel();
                                break;
                            default:
                                // code block
                                item = new StaffListViewModel();
                                break;
                        }
                        break;
                    }

                case Roles.Accountings:
                    break;
                case Roles.Receiptions:
                    {
                        ReceptionistFeatures feateture = (ReceptionistFeatures)Enum.ToObject(typeof(ReceptionistFeatures), index);
                        switch (feateture)
                        {
                            case ReceptionistFeatures.roomList:
                                item = new RoomViewModel();
                                break;
                            case ReceptionistFeatures.customList:
                                item = new CustomerViewModel();
                                break;
                        }
                        break;
                    }
                    
                case Roles.Sales:
                    break;
                default:
                    // code block
                    break;
            }

            return item;
        }

        public static string generateUserName(staff p)
        {
            StringBuilder res = new StringBuilder();
            string[] words = p.Name.Split(' ');
            for (int i = 0; i < words.Length-1; i++)
            {
                res.Append(words[i][0]);
            }
            res.Append(words[words.Length - 1]);
            res.Append(p.DOB.Year);
            return res.ToString();
        }
    }
}
