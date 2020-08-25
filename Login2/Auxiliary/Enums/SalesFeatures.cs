﻿using System;
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
        SF_roomList=0,
        [Description("Tạo Phòng")]
        SF_addRoom=1,
        [Description("Danh Sách Khách Hàng")]
        SF_customerList=2,

    }
}
