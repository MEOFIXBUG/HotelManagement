using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login2.Auxiliary.DomainObjects
{
    public class RePass
    {
        [Required]
        public string curpass { get; set; }
        [Required]
        public string newpass { get; set; }
        [Required]
        public string repass { get; set; }
        //public RePass(string str)
        //{
        //    var p = str.Split(';');
        //    curpass = p[0];
        //    newpass = p[1];
        //    repass = p[2];
        //}
        public RePass()
        {
            curpass = "";
            newpass = "";
            repass = "";
        }
        public bool validate()
        {
            if(String.IsNullOrEmpty(curpass)|| String.IsNullOrEmpty(newpass)|| String.IsNullOrEmpty(repass))
            {
                return false;
            }
            return true;
        }
        public void clear()
        {
            curpass = "";
            newpass = "";
            repass = "";
        }
    }
}
