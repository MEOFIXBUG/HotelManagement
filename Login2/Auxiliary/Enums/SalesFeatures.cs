using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login2.Auxiliary.Enums
{
    public enum SalesFeatures
    {
        [Description("Danh Sách Phòng")]
        roomList=0,
        [Description("Tạo Phòng")]
        addRoom=1,
        [Description("Danh Sách Khách Hàng")]
        customerList=2,

    }
}
