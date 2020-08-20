using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    class Subscription
    {
        public Subscription(DateTime dateDue, double amountPaid)
        {
            this.datePaid = dateDue.AddYears(-1);//When a member paid their subscription
            this.dateDue = dateDue;//When a member's subscription is due
            this.amountPaid = amountPaid;//How much a member paid towards their subscription
            if (dateDue.CompareTo(DateTime.Today) >= 0)//The subscription isn't due
            { this.due = false; }//The subscription isn't due
            else
            { this.due = true; }//Otherwise it is due

        }
        public DateTime datePaid { get; set; }
        public DateTime dateDue { get; set; }
        public double amountPaid { get; set; }
        public bool due { get; set; }
    }
}
