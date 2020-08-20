using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login2.Auxiliary.Enums
{
    public enum RoomStatus
    {

        [Description("Phòng trống")]
        available,
        [Description("Có khách")]
        rented,
        [Description("Đang dọn dẹp")]
        cleaning,
        [Description("Đang sửa chữa")]
        fixing,
        [Description("Tất cả")]
        all = -1,
    }
}
