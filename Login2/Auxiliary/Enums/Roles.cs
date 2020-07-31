using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login2.Auxiliary.Enums
{
    public enum Roles
    {
        [Description("Nhân Sự")]
        HumanResources =1,
        [Description("Kế Toán")]
        Accountings =2,
        [Description("Lễ Tân")]
        Receiptions =3,
        [Description("Kinh Doanh")]
        Sales =4
    }
}
