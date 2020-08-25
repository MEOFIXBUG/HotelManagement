using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login2.Auxiliary.Enums
{
    public enum ReceptionistFeatures
    {
        [Description("Danh sách phòng")]
        RF_roomList,
        [Description("Danh sách khách hàng")]
        RF_customerList
    }
}
