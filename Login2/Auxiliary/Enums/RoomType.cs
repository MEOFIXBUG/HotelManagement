using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login2.Auxiliary.Enums
{
    public enum RoomType
    {
        [Description("Phòng đơn")]
        singleRoom,
        [Description("Phòng đôi")]
        doubleRoom,
        [Description("Tất cả")]
        all=-1,
    }
}
