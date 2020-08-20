using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    class Member
    {
        public Member(int id,string firstname, string lastname, string phonenum, string email, Address address, string type, Subscription subscription)
        {
            this.id = id;//The member's id
            this.Firstname = firstname;//The member's first name
            this.Lastname = lastname;//The member's last name
            this.PhoneNum = phonenum;//The member's phone number
            this.EmailAddress = email;//The member's email address
            this.Address = address;//The member's address details
            this.MemberType = type;//The member's membershiptype
            this.Subscription = subscription;//The member's subscription details
        }
        public int id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string PhoneNum { get; set; }
        public Address Address { get; set; }
        public string EmailAddress { get; set; }
        public string MemberType { get; set; }
        public Subscription Subscription { get; set; }
    }
}
