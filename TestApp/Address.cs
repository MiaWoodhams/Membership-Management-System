using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    class Address
    {
        public Address (string house, string street, string town, string postcode)
        {
            this.House = house;//The member's house name/number
            this.Street = street;//The member's street
            this.Town = town;//The member's town
            this.Postcode = postcode;//The member's postcode
        }
        public string House { get; set; }
        public string Street { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }
    }
}
