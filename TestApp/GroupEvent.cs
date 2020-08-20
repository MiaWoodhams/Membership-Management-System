using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    class GroupEvent
    {
        public GroupEvent(int id, DateTime date, string name, string details) //Creating a group event without earnings
        {
            this.id = id;//The event's id
            this.date = date;//When the event takes place (date & time)
            this.name = name;//The name of the event
            this.details = details;//Additional details about the event
            this.earnings = 0;//Sets earnings as 0 initially
        }
        public GroupEvent(int id, DateTime date, string name, string details, double earnings) //creating a group event with earnings
        {
            this.id = id;//The event's id
            this.date = date;//When the event takes place (date & time)
            this.name = name;//The name of the event
            this.details = details;//Additional details about the event
            this.earnings = earnings;//How much was earned from the event
        }
        public int id { get; set; }
        public DateTime date { get; set; }
        public string name { get; set; }
        public string details { get; set; }
        public double earnings { get; set; }
    }
}
