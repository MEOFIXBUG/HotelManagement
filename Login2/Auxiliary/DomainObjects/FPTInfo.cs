using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login2.Auxiliary.DomainObjects
{
    public class Address_entities
    {
        public string province { get; set; }
        public string district { get; set; }
        public string ward { get; set; }
        public string street { get; set; }

    }
    public class Data
    {
        public string id { get; set; }
        public string id_prob { get; set; }
        public string name { get; set; }
        public string name_prob { get; set; }
        public string dob { get; set; }
        public string dob_prob { get; set; }
        public string sex { get; set; }
        public string sex_prob { get; set; }
        public string nationality { get; set; }
        public string nationality_prob { get; set; }
        public string home { get; set; }
        public string home_prob { get; set; }
        public string address { get; set; }
        public string address_prob { get; set; }
        public Address_entities address_entities { get; set; }
        public string doe { get; set; }
        public string doe_prob { get; set; }
        public string type { get; set; }

    }
    public class InfoCMT
    {
        public int errorCode { get; set; }
        public string errorMessage { get; set; }
        public IList<Data> data { get; set; }

    }
}
