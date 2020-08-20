using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace TestApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //AddTestData();
            ClearMemberTextboxes();
            loadMembersFromFile();
            loadEventsFromFile();
        }
        string memberFileLocation =  "Members.csv"; //Stored in Testapp\bin\Debug
        string eventFileLocation =  "Events.csv";
        List<Member> memberslist = new List<Member>();
        List<Member> displayedmembers = new List<Member>();
        List<GroupEvent> eventlist = new List<GroupEvent>();
        /*public void AddTestData()
        {
            Address address = new Address("a", "b", "c", "dddddd");
            Subscription subscription = new Subscription(DateTime.Now.AddDays(1), 5);
            Member member = new Member("test", "1", 12345678901, "c", address, "Regular", subscription);
            memberslist.Add(member);
            memberListBox.Items.Add(member.Firstname + " " + member.Lastname);
            Address address2 = new Address("a", "b", "c", "dddddd");
            Subscription subscription2 = new Subscription(DateTime.Now.AddMonths(11).AddDays(15), 7);
            Member member2 = new Member("test", "2", 12345678901, "c", address2, "Regular", subscription2);
            memberslist.Add(member2);
            memberListBox.Items.Add(member2.Firstname + " " + member2.Lastname);
            Address address3 = new Address("a", "b", "c", "dddddd");
            Subscription subscription3 = new Subscription(DateTime.Now.AddMonths(-11), 11);
            Member member3 = new Member("test", "3", 12345678901, "c", address3, "Regular", subscription3);
            memberslist.Add(member3);
            memberListBox.Items.Add(member3.Firstname + " " + member3.Lastname);
        }*/
        private void CreateMemberBtn_Click(object sender, EventArgs e) //Executes when creatememberbtn is clicked
        {
            int id = 0; //ID initially set to 0
            for (int i = 0; i < memberslist.Count(); i++) //Selects all membres in memberslist
            {
                if (memberslist[i].id >= id) //If the current member's ID is more than/equal to the new id
                {
                    id = memberslist[i].id + 1; //Set the new ID to the current member's ID +1
                } //This ensures all IDs are unique
            }
            string firstname = firstnameTxt.Text; //Storing the data from the textboxes as strings for ease of use
            string phonenum = phonenumTxt.Text;
            string lastname = lastnameTxt.Text;
            string emailaddress = emailAddressTxt.Text;
            string house = housenumTxt.Text;
            string street = streetTxt.Text;
            string town = townTxt.Text;
            string postcode = postcodeTxt.Text;
            string membertype = membershipTypeTxt.Text;
            bool error = validate(); //Checks to make sure all data entered is valid/not empty
            if (SubscriptionPaidChkBx.Checked) //Validates the amountpaid only if the member has paid
            {
                try
                {
                    Convert.ToDouble(originalAmountPaid.Text);
                }
                catch (Exception)
                {
                    error = true;
                    amountPaidErrorLbl.Visible = true;
                }
            }

            if (error != true) //Only executes the following code if all data entered is valid
            {
                Address address = new Address(house, street, town, postcode);
                Subscription sub = new Subscription(DateTime.Now.AddYears(-1500), 0); 
                //If a subscription is not paid, it is given a year far enough back that it will never be found in actual data
                if (SubscriptionPaidChkBx.Checked)
                {
                    sub = new Subscription(originalDatePaid.Value, Convert.ToDouble(originalAmountPaid.Text)); 
                    //Changes the subscription data if the data has been entered by the user
                }
                Member member = new Member(id, firstname, lastname, phonenum, emailaddress, address, membertype, sub); 
                //Creates a new member from the data entered
                memberListBox.Items.Add(member.id + " :  " + member.Lastname + " " + member.Firstname); //Adds the member into the listbox
                memberslist.Add(member); //Adds the member to the memberlist
                ClearMemberTextboxes();
                addToMemberFile(member); //Adds the member to members.csv
                memberListBox.ClearSelected();
                DeleteMemberBtn.Visible = false; //Clears any other buttons being shown
                EditBtn.Visible = false;
                cancelButton.Visible = false;
                originalAmountPaidLbl.Visible = false;
                originalAmountPaid.Visible = false;
                originalDatePaid.Visible = false;
                originalDatePaidLbl.Visible = false;
            }
        }

        private void memberListBox_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                int i = 0;
                int endOfID = Convert.ToString(memberListBox.SelectedItem).IndexOf(":") - 1;  
                //Gets where the end of the member's ID is in the listbox
                string id = Convert.ToString(memberListBox.SelectedItem).Remove(endOfID); 
                //Gets the selected member's ID by removing all data after the end of the id
                for (int j = 0; j < memberslist.Count; j++)
                {
                    if (memberslist[j].id == Convert.ToInt32(id)) //Finds the index of the member with this ID in the Members list
                    {
                        i = j;
                    }
                }
                firstnameTxt.Text = memberslist[i].Firstname; //Loads the details of this member into the text boxes
                lastnameTxt.Text = memberslist[i].Lastname;
                phonenumTxt.Text = Convert.ToString(memberslist[i].PhoneNum);
                emailAddressTxt.Text = memberslist[i].EmailAddress;
                housenumTxt.Text = Convert.ToString(memberslist[i].Address.House);
                streetTxt.Text = memberslist[i].Address.Street;
                townTxt.Text = memberslist[i].Address.Town;
                postcodeTxt.Text = memberslist[i].Address.Postcode;
                membershipTypeTxt.Text = memberslist[i].MemberType;
                DeleteMemberBtn.Visible = true; //Shows necessary buttons
                EditBtn.Visible = true;
                cancelButton.Visible = true;
                originalAmountPaidLbl.Visible = false;
                originalAmountPaid.Visible = false;
                originalDatePaid.Visible = false;
                originalDatePaidLbl.Visible = false;
                SubscriptionPaidLbl.Visible = false;
                SubscriptionPaidChkBx.Visible = false;
            }
            catch (Exception)
            {
                MessageBox.Show("Error loading member data"); //Displays error message if the ID cannot be read from the listbox
            }
        }
        private void EditBtn_Click(object sender, EventArgs e) //Executes after the edit button is clicked
        {
            bool memberNotSelected = false;
            try
            {
                int endOfID = Convert.ToString(memberListBox.SelectedItem).IndexOf(":") - 1; 
                string id = Convert.ToString(memberListBox.SelectedItem).Remove(endOfID); 
                //Gets the member's ID using the listbox and removes all data after the ID
            }
            catch (Exception)
            {
                memberNotSelected = true;
                MessageBox.Show("No member selected"); //Displays an error if the user hasn't selected a member
            }
            if (memberNotSelected == false)
            {

            bool error = validate(); //Makes sure all data entered into the textboxes is valid
            if (error != true) //If all data is valid
            {
                int currentmember = 0;
                int endOfID = Convert.ToString(memberListBox.SelectedItem).IndexOf(":") - 1;
                string id = Convert.ToString(memberListBox.SelectedItem).Remove(endOfID); //Gets selected member's ID using the listbox
                for (int j = 0; j < memberslist.Count; j++)
                {
                    if (memberslist[j].id == Convert.ToInt32(id)) //Finds the index of the member with this ID
                    {
                        currentmember = j;
                    }
                }
                memberslist[currentmember].Firstname = firstnameTxt.Text;
                   //Changes all of this member's data to match the data entered into the textboxes
                memberslist[currentmember].Lastname = lastnameTxt.Text;
                memberslist[currentmember].PhoneNum = phonenumTxt.Text;
                memberslist[currentmember].EmailAddress = emailAddressTxt.Text;
                memberslist[currentmember].Address.House = housenumTxt.Text;
                memberslist[currentmember].Address.Street = streetTxt.Text;
                memberslist[currentmember].Address.Town = townTxt.Text;
                memberslist[currentmember].Address.Postcode = postcodeTxt.Text;
                memberslist[currentmember].MemberType = Convert.ToString(membershipTypeTxt.Text);
                memberListBox.Items[memberListBox.SelectedIndex] = memberslist[currentmember].id + " :  " + lastnameTxt.Text + " " + firstnameTxt.Text;
                    //Updates the member's details in the listbox
                ClearMemberTextboxes();//Clears data entered into the textboxes
                UpdateMemberFile();//Updates all members in the member file
                DeleteMemberBtn.Visible = false;//Hides unneeded buttons
                EditBtn.Visible = false;
                cancelButton.Visible = false;
                memberListBox.ClearSelected();
                originalAmountPaidLbl.Visible = false;
                originalAmountPaid.Visible = false;
                originalDatePaid.Visible = false;
                originalDatePaidLbl.Visible = false;
                SubscriptionPaidLbl.Visible = true;
                SubscriptionPaidChkBx.Visible = true;
            }

            }
        }
        private void SubscriptionConfirmButton_Click(object sender, EventArgs e) //Executes when the user confirms an update to a member's subscription
        {
            int i = 0;
            int endOfID = Convert.ToString(subscriptionListBox.SelectedItem).IndexOf(":") - 1; //Gets selected member's ID using the listbox
            string id = Convert.ToString(subscriptionListBox.SelectedItem).Remove(endOfID);
            for (int j = 0; j < memberslist.Count; j++)
            {
                if (memberslist[j].id == Convert.ToInt32(id)) //Finds the index of the member with this ID
                {
                    i = j;
                }
            }
            try
            {
                DateTime dateDue = datePaidTxt.Value.AddYears(1);
                //Gets the date the member's subscription is due by adding 1 year to the date they paid it
                double amountPaid = Convert.ToDouble(amountPaidTxt.Text);
                //Gets the amount paid from the textbox
                Subscription sub = new Subscription(dateDue, amountPaid);
                //Creates a Subscription using this data
                memberslist[i].Subscription = sub;
                //Updates the member's subsciption
                string dayPaid = Convert.ToString(memberslist[i].Subscription.dateDue.Day); 
                if (memberslist[i].Subscription.dateDue.Day < 10)
                {
                    dayPaid = "0" + memberslist[i].Subscription.dateDue.Day;
                    //Adds a "0" at the start of the day part of the date if it is a single digit to improve readability
                }
                string monthPaid = Convert.ToString(memberslist[i].Subscription.dateDue.Month);
                if (memberslist[i].Subscription.dateDue.Month < 10)
                {
                    monthPaid = "0" + memberslist[i].Subscription.dateDue.Month;
                    //Adds a "0" at the start of the month part of the date if it is a single digit to improve readability
                }

                subscriptionListBox.Items.Add(memberslist[i].id + " :  " + (memberslist[i].Firstname + "," +
                    memberslist[i].Lastname).PadRight(30) + "Due:" + dayPaid + "/" + monthPaid + "/" + (memberslist[i].Subscription.dateDue.Year)
                    + "    " + "Amount Paid: £" + memberslist[i].Subscription.amountPaid);
                //Updates member information in listbox
                subscriptionListBox.Items.RemoveAt(i); //Removes current member from the listbox
                showSubscriptionChkBx.Checked = false;
                UpdateMemberFile();//Updates all member information in the member.csv file
            }
            catch
            {
                MessageBox.Show("Invalid amount paid"); //Validates the "amount paid" field
            }

        }

        private void showSubscriptionChkBx_CheckedChanged(object sender, EventArgs e)
        {
            subscriptionListBox.ClearSelected();//Removes buttons and clears selection
            SubscriptionConfirmButton.Visible = false;
            SubscriptionCancelBtn.Visible = false;
            SelectedMemberLbl.Text = "Select Member from the list below";
            SelectedMemberTxt.Text = "";
            subscriptionListBox.Items.Clear();
            if (showSubscriptionChkBx.Checked)//If only members with subscriptions due are being shown
            {
                for (int i = 0; i < memberslist.Count; i++)
                {
                    if (memberslist[i].Subscription.due == true)//Only shows subscriptions which are due
                    {
                        if (memberslist[i].Subscription.datePaid > DateTime.Now.AddYears(-1000))
                            //Checks if the subscription is within 1000 years. If it isn't, the subscription was never paid
                        {
                            string dayPaid = Convert.ToString(memberslist[i].Subscription.dateDue.Day); 
                            if (memberslist[i].Subscription.dateDue.Day < 10) //Makes day & month more readable
                            {
                                dayPaid = "0" + memberslist[i].Subscription.dateDue.Day;
                            }
                            string monthPaid = Convert.ToString(memberslist[i].Subscription.dateDue.Month);
                            if (memberslist[i].Subscription.dateDue.Month < 10)
                            {
                                monthPaid = "0" + memberslist[i].Subscription.dateDue.Month;
                            }

                            subscriptionListBox.Items.Add(memberslist[i].id + " :  " + (memberslist[i].Firstname + "," + memberslist[i].Lastname).PadRight(30)
                                + "Due:" + dayPaid + "/" + monthPaid + "/" + (memberslist[i].Subscription.dateDue.Year)
                                +  "    " + "Amount Paid: £" + memberslist[i].Subscription.amountPaid);//Adds member back into the listbox
                        }
                        else
                        {
                            subscriptionListBox.Items.Add(memberslist[i].id + " :  " + (memberslist[i].Firstname + "," + memberslist[i].Lastname).PadRight(30) + "Due: Not Paid");
                            //Shows that the subscription was not paid
                        }
                    }
                }
            }
            else //If all members are being shown
            {
                for (int i = 0; i < memberslist.Count; i++)//Gets all members from memberslist
                {
                    if (memberslist[i].Subscription.datePaid > DateTime.Now.AddYears(-1000))
                        //Checks if the subscription is within 1000 years. If it isn't, the subscription was never paid
                    {
                        string dayPaid = Convert.ToString(memberslist[i].Subscription.dateDue.Day);
                        if (memberslist[i].Subscription.dateDue.Day < 10)//Makes day & month more readable
                        {
                            dayPaid = "0" + memberslist[i].Subscription.dateDue.Day;
                        }
                        string monthPaid = Convert.ToString(memberslist[i].Subscription.dateDue.Month);
                        if (memberslist[i].Subscription.dateDue.Month < 10)
                        {
                            monthPaid = "0" + memberslist[i].Subscription.dateDue.Month;
                        }

                        subscriptionListBox.Items.Add(memberslist[i].id + " :  " + (memberslist[i].Firstname + "," + memberslist[i].Lastname).PadRight(30) 
                            + "Due:" + dayPaid + "/" + monthPaid + "/" + (memberslist[i].Subscription.dateDue.Year) 
                            + "    " + "Amount Paid: £" + memberslist[i].Subscription.amountPaid);//Adds member back into the listbox
                    }
                    else
                    {
                        subscriptionListBox.Items.Add(memberslist[i].id + " :  " + (memberslist[i].Firstname + "," + memberslist[i].Lastname).PadRight(30) + "Due: Not Paid");
                        //Shows that the subscription was not paid
                    }
                }
            }
        }

        private void SearchBtn_Click(object sender, EventArgs e)
        {
            memberListBox.Items.Clear();//Remove all members from the listbox
            for (int i = 0; i < memberslist.Count; i++) //Repeated for all members
            {
                string fname = memberslist[i].Firstname; //Initially sets the name to the member's name
                if (firstnameTxt.Text != "") //If something is being searched for (field isnt empty)
                {
                    fname = firstnameTxt.Text; //Set the name to what is being searched for
                }
                string lname = memberslist[i].Lastname;
                if (lastnameTxt.Text != "")
                {
                    lname = lastnameTxt.Text;
                }
                string phonenum = memberslist[i].PhoneNum;
                if (phonenumTxt.Text != "")
                {
                    phonenum = phonenumTxt.Text;
                }
                string email = memberslist[i].EmailAddress;
                if (emailAddressTxt.Text != "")
                {
                    email = emailAddressTxt.Text;
                }
                string house = memberslist[i].Address.House;
                if (housenumTxt.Text != "")
                {
                    house = housenumTxt.Text;
                }
                string street = memberslist[i].Address.Street;
                if (streetTxt.Text != "")
                {
                    street = streetTxt.Text;
                }
                string town = memberslist[i].Address.Town;
                if (townTxt.Text != "")
                {
                    town = townTxt.Text;
                }
                string postcode = memberslist[i].Address.Postcode;
                if (postcodeTxt.Text != "")
                {
                    postcode = postcodeTxt.Text;
                }
                string type = memberslist[i].MemberType;
                if (membershipTypeTxt.Text != "[Select Type]" && membershipTypeTxt.Text != "")
                {
                    type = membershipTypeTxt.Text;
                }
                if (memberslist[i].Firstname.Contains(fname) && //If all of the member's details contains all of the above data
                    memberslist[i].Lastname.Contains(lname) &&
                    memberslist[i].PhoneNum.Contains(phonenum) &&
                    memberslist[i].EmailAddress.Contains(email) &&
                    memberslist[i].Address.House.Contains(house) &&
                    memberslist[i].Address.Street.Contains(street) &&
                    memberslist[i].Address.Town.Contains(town) &&
                    memberslist[i].Address.Postcode.Contains(postcode) &&
                    memberslist[i].MemberType == type)
                {
                    memberListBox.Items.Add(memberslist[i].id + "  " + memberslist[i].Firstname + " " + memberslist[i].Lastname);
                    //Re-add the member to the listbox
                }

            }
        }
        private void EnterFinancialTab(object sender, EventArgs e)//When the user opens the financial tab
        {
            double totalsubearnings = 0; //Initialises values
            double monthlysubearnings = 0;
            double yearlysubearnings = 0;
            double totaleventearnings = 0;
            double monthlyeventearnings = 0;
            double yearlyeventearnings = 0;
            //Total subscription earnings:
            for (int i = 0; i < memberslist.Count; i++)
            {
                totalsubearnings += memberslist[i].Subscription.amountPaid; //Adds the amount each member has paid in subscription fees to the total
            }
            //Total event earnings:
            for (int i = 0; i < eventlist.Count; i++)
            {
                totaleventearnings += eventlist[i].earnings; //Adds the amount earned from each events to the total
            }
            //Monthly subscription earnings:
            for (int i = 0; i < memberslist.Count; i++)
            {
                if (DateTime.Now.AddMonths(-1).CompareTo(memberslist[i].Subscription.datePaid) <= 0) //If the member's subscription was paid within 1 month
                {
                    monthlysubearnings += memberslist[i].Subscription.amountPaid; //Adds the amount the member paid to the monthly total
                }
            }
            //Monthly event earnings:
            for (int i = 0; i < eventlist.Count; i++)
            {
                if (DateTime.Now.AddMonths(-1).CompareTo(eventlist[i].date) <= 0) //If the event was held within the last month
                {
                    monthlyeventearnings += eventlist[i].earnings; //Adds the amount earned to the monthly total
                }
            }
            //Yearly subscription earnings:
            for (int i = 0; i < memberslist.Count; i++)
            {
                if (DateTime.Now.AddYears(-1).CompareTo(memberslist[i].Subscription.datePaid) <= 0) //If the member's subscription was paid within 1 year
                {
                    yearlysubearnings += memberslist[i].Subscription.amountPaid; //Adds the amount the member paid to the yearly total
                }
            }
            //Yearly event earnings:
            for (int i = 0; i < eventlist.Count; i++)
            {
                if (DateTime.Now.AddYears(-1).CompareTo(eventlist[i].date) <= 0)//If the event was held within the last year
                {
                    yearlyeventearnings += eventlist[i].earnings;//Adds the amount earned to the yearly total
                }
            }
            totalEarningsSubTxt.Text = "£" + totalsubearnings; //Displays all of this data in the textboxes, preceded by a £ symbol
            totalEarningsEventTxt.Text = "£" + totaleventearnings;
            monthlyEarningsSubTxt.Text = "£" + monthlysubearnings;
            monthlyEarningsEventTxt.Text = "£" + monthlyeventearnings;
            yearlyEarningsSubTxt.Text = "£" + yearlysubearnings;
            yearlyEarningsEventTxt.Text = "£" + yearlyeventearnings;
        }
        private void ClearErrorLabels() //Clears all of the error validation labels (red  *'s)
        {
            firstNameErrorLbl.Visible = false;
            lastNameErrorLbl.Visible = false;
            emailErrorLbl.Visible = false;
            houseErrorLbl.Visible = false;
            streetErrorLbl.Visible = false;
            townErrorLbl.Visible = false;
            phoneNumErrorLbl.Visible = false;
            postcodeErrorLbl.Visible = false;
            membershipErrorLbl.Visible = false;
            datePaidErrorLbl.Visible = false;
            amountPaidErrorLbl.Visible = false;
        }
        private void cancelButton_Click(object sender, EventArgs e)//When the cancel button is clicked while editing a member
        {
            ClearMemberTextboxes();  //Clears all textboxes
            DeleteMemberBtn.Visible = false; //Resets buttons
            EditBtn.Visible = false;
            cancelButton.Visible = false;
            memberListBox.ClearSelected();
            originalAmountPaidLbl.Visible = false;
            originalAmountPaid.Visible = false;
            originalDatePaid.Visible = false;
            originalDatePaidLbl.Visible = false;
            SubscriptionPaidLbl.Visible = true;
            SubscriptionPaidChkBx.Visible = true;
            ClearErrorLabels();

        }
        private void ClearMemberTextboxes()
        {
            firstnameTxt.Clear(); //Clears all textboxes
            lastnameTxt.Clear();
            phonenumTxt.Clear();
            emailAddressTxt.Clear();
            housenumTxt.Clear();
            streetTxt.Clear();
            townTxt.Clear();
            postcodeTxt.Clear();
            membershipTypeTxt.ResetText();
            ClearErrorLabels();
        }
        private bool validate()
        {
            ClearErrorLabels();
            bool error = false;//boolean value used to track whether any data is invalid
            if (String.IsNullOrWhiteSpace(firstnameTxt.Text)) //If the firstname textbox is not empty
            {
                firstNameErrorLbl.Visible = true; //Shows the red * label next to firstname
                error = true;
            }
            if (String.IsNullOrWhiteSpace(lastnameTxt.Text))
            {
                lastNameErrorLbl.Visible = true;
                error = true;
            }
            if (String.IsNullOrWhiteSpace(emailAddressTxt.Text))
            {
                emailErrorLbl.Visible = true;
                error = true;
            }
            if (String.IsNullOrWhiteSpace(housenumTxt.Text))
            {
                houseErrorLbl.Visible = true;
                error = true;
            }
            if (String.IsNullOrWhiteSpace(streetTxt.Text))
            {
                streetErrorLbl.Visible = true;
                error = true;
            }
            if (String.IsNullOrWhiteSpace(townTxt.Text))
            {
                townErrorLbl.Visible = true;
                error = true;
            }
            if (Convert.ToString(phonenumTxt.Text).Length != 11)//Checks if the length of the phone number is exactly 11 characters long
            {
                error = true;
                phoneNumErrorLbl.Visible = true;
            }
            if (validatepostcode(postcodeTxt.Text) != true) //Checks if the postcode is valid
            {
                error = true;
                postcodeErrorLbl.Visible = true;
            }
            if (membershipTypeTxt.Text != "Regular" && 
                membershipTypeTxt.Text != "Honorary" &&
                membershipTypeTxt.Text != "Committee" &&
                membershipTypeTxt.Text != "Lapsed")//Checks if the membership type is one of the 4 valid values
            {
                error = true;
                membershipErrorLbl.Visible = true;
            }
            if (emailAddressTxt.Text.Contains("@") == false || emailAddressTxt.Text.Contains(".") == false)
                //Checks if the email address contains an @ symbol and a .
            {
                error = true;
                emailErrorLbl.Visible = true;
            }
            if (SubscriptionPaidChkBx.Checked) //Only validates datepaid and amountpaid textboxes if they are being shown to the user
            {
                if (String.IsNullOrWhiteSpace(originalDatePaid.Text))
                {
                    datePaidErrorLbl.Visible = true;
                    error = true;
                }
                if (String.IsNullOrWhiteSpace(originalAmountPaid.Text))
                {
                    amountPaidErrorLbl.Visible = true;
                    error = true;
                }
            }
            return error; //Returns whether or not any data was invalid
        }
        private bool validatepostcode(string postcode)
        {
            string a = "0 1 2 3 4 5 6 7 8 9";
            string b = "Q W E  R T Y U I O P A S D F G H J K L Z X C V B N M";
            List<string> numbers = new List<string>(a.Split()); //String list containing all numbers 0-9
            List<string> letters = new List<string>(b.Split()); //String list containing all letters A-Z
            postcode = postcode.ToUpper(); //Converts postcode to uppercase
            bool valid = false; //Postcode is initially invalid
            for (int i = 0; i < postcode.Length; i++)
            {
                if (postcode[i] == Convert.ToChar(" "))
                {
                    postcode = postcode.Remove(i, 1);//Removes any spaces in the postcode
                }
            }
            if (postcode.Length == 6) //If the postcode is in LLN NLL format
            {
                if (letters.Contains(Convert.ToString(postcode[0])) &&  //Checks that each character of the postcode fits this format
                   letters.Contains(Convert.ToString(postcode[1])) &&
                   numbers.Contains(Convert.ToString(postcode[2])) &&
                   numbers.Contains(Convert.ToString(postcode[3])) &&
                   letters.Contains(Convert.ToString(postcode[4])) &&
                   letters.Contains(Convert.ToString(postcode[5])))
                {
                    valid = true; //If it fits this format, the postcode is valid
                }
            }
            else if (postcode.Length == 7) //If the postcode is in LLNN NLL format
            {
                if (letters.Contains(Convert.ToString(postcode[0])) &&  //Checks that each character of the postcode fits this format
                   letters.Contains(Convert.ToString(postcode[1])) &&
                   numbers.Contains(Convert.ToString(postcode[2])) &&
                   numbers.Contains(Convert.ToString(postcode[3])) &&
                   numbers.Contains(Convert.ToString(postcode[4])) &&
                   letters.Contains(Convert.ToString(postcode[5])) &&
                   letters.Contains(Convert.ToString(postcode[6])))
                {
                    valid = true; //If it fits this format, the postcode is valid
                }
            }
            return valid;
        }
        private void UpdateMemberFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(memberFileLocation, append: false)) //Writes to the member file with append = false
                {
                    int num = 0;
                    for (int i = 0; i < memberslist.Count; i++)
                    {
                        Member member = memberslist[i];
                        writer.WriteLine( //Writes the member data to the file for each member
                        member.id + "," +
                        member.Firstname + "," +
                        member.Lastname + "," +
                        member.PhoneNum.ToString() + "," +
                        member.EmailAddress + "," +
                        member.MemberType + "," +
                        member.Address.House + "," +
                        member.Address.Street + "," +
                        member.Address.Town + "," +
                        member.Address.Postcode + "," +
                        member.Subscription.dateDue + "," +
                        member.Subscription.amountPaid);
                        num = i + 1;
                    }
                    MessageBox.Show(Convert.ToString(num) + " Members updated"); //Shows how many members were updated
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Customer File Not Found " + e); //Shows error if customer file cannot be found
            }
        }
        private void addToMemberFile(Member member) //Writes a single member to the member file
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(memberFileLocation, append: true)) //Writes to the member file with append = true
                {
                    writer.WriteLine( //Writes the member data for this member, seperated by commas
                        member.id + "," +
                        member.Firstname + "," +
                        member.Lastname + "," +
                        member.PhoneNum.ToString() + "," +
                        member.EmailAddress + "," +
                        member.MemberType + "," +
                        member.Address.House + "," +
                        member.Address.Street + "," +
                        member.Address.Town + "," +
                        member.Address.Postcode + "," +
                        member.Subscription.dateDue + "," +
                        member.Subscription.amountPaid);
                    MessageBox.Show("Added 1 Member");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Customer File Not Found " + e);//Shows error if customer file cannot be found
            }
        }
        private void loadMembersFromFile() //Reads all data from file
        {
            try
            {
                using (StreamReader reader = new StreamReader(File.OpenRead(memberFileLocation))) //Opens file
                {
                    int i = 0;
                    while (!reader.EndOfStream) //While there is still data to be read
                    {
                        i += 1;
                        string line = reader.ReadLine(); //Gets the current line of the string
                        if (!String.IsNullOrWhiteSpace(line))
                        {
                            string[] values = line.Split(','); //Splits the line based on where the commas are, stores as an array
                            Address address = new Address(values[6], values[7], values[8], values[9]); //Creates user data from these values
                            Subscription sub = new Subscription(Convert.ToDateTime(values[10]), Convert.ToDouble(values[11]));
                            Member member = new Member(Convert.ToInt32(values[0]), values[1], values[2], (values[3]), values[4], address, values[5], sub);
                            memberslist.Add(member); //Adds member to the memberslist
                            memberListBox.Items.Add(member.id + " :  " + member.Firstname + "," + member.Lastname); //Adds member to listbox
                            /* values[0]  ID
                             * values[1]  First name
                             * values[2]  Last name
                             * values[3]  Phone num
                             * values[4]  Email Address
                             * values[5]  Member Type
                             * values[6]  House
                             * values[7]  Street
                             * values[8]  Town
                             * values[9]  Postcode
                             * values[10]  Date due
                             * values[11] Amount Paid
                             */
                        }
                    }
                    MessageBox.Show(Convert.ToString(i) + " Members loaded"); //Shows how many members were loaded
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Customer File Not Found " + e);
            }
        }
        private void UpdateEventFile() //Same as UpdateMemberFile but with event details
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(eventFileLocation, append: false))
                {
                    int num = 0;
                    for (int i = 0; i < eventlist.Count; i++)
                    {
                        GroupEvent ev = eventlist[i];
                        writer.WriteLine(
                        ev.id + "," +
                        ev.date + "," +
                        ev.name + "," +
                        ev.details + "," +
                        ev.earnings);
                        num = i + 1;
                    }
                    MessageBox.Show(Convert.ToString(num) + " Events updated");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Customer File Not Found " + e);
            }
        }
        private void addToEventFile(GroupEvent ev) //Same as addToMemberFile but with event details
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(eventFileLocation, append: true))
                {
                    writer.WriteLine(
                        ev.id + "," +
                        ev.date + "," +
                        ev.name + "," +
                        ev.details + "," +
                        ev.earnings);

                    MessageBox.Show("Added 1 Event");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Event File Not Found " + e);
            }
        }
        private void loadEventsFromFile()//Same as loadEventsFromFile but with event details
        {
            try
            {
                using (StreamReader reader = new StreamReader(File.OpenRead(eventFileLocation)))
                {
                    int i = 0;
                    while (!reader.EndOfStream)
                    {
                        i += 1;
                        string line = reader.ReadLine();
                        if (!String.IsNullOrWhiteSpace(line))
                        {
                            string[] values = line.Split(',');
                            GroupEvent ev = new GroupEvent(Convert.ToInt32(values[0]), Convert.ToDateTime(values[1]), values[2], values[3]);
                            if (String.IsNullOrWhiteSpace(values[4]) == false)
                            {
                                ev.earnings = Convert.ToDouble(values[4]);
                            }
                            else
                            {
                                ev.earnings = 0;
                            }
                            eventlist.Add(ev);
                            addEventToListbox(ev);
                            /* values[0]  ID
                             * values[1]  Event Time
                             * values[2]  Event Name
                             * values[3]  Event Details
                             * values[4]  Event Earnings
                             */
                        }
                    }
                    MessageBox.Show(Convert.ToString(i) + " Events loaded");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Event File Not Found " + e);
            }
        }
        private void SubscriptionPaidChkBx_CheckedChanged(object sender, EventArgs e) //Sets whether the subscriptionpaid and amountpaid textboxes are shown or not
        {
            originalAmountPaidLbl.Visible = SubscriptionPaidChkBx.Checked;
            originalAmountPaid.Visible = SubscriptionPaidChkBx.Checked;
            originalDatePaid.Visible = SubscriptionPaidChkBx.Checked;
            originalDatePaidLbl.Visible = SubscriptionPaidChkBx.Checked;
            if (SubscriptionPaidChkBx.Checked == false) //Removes error labels if the fields are hidden, as data does not need to be validated if not in use
            {
                datePaidErrorLbl.Visible = false;
                amountPaidErrorLbl.Visible = false;
            }
        }

        private void subscriptionListBox_SelectedIndexChanged(object sender, EventArgs e)//When the selected index in the subscription listbox is changed
        {
            if (subscriptionListBox.SelectedIndex != -1) //If selected index is valid
            {
                SubscriptionConfirmButton.Visible = true;//Show confirm and cancel buttons
                SubscriptionCancelBtn.Visible = true;
            }
            else
            {
                SubscriptionConfirmButton.Visible = false; //Otherwise, don't show them
                SubscriptionCancelBtn.Visible = false;
            }
        }

        private void SubscriptionTabEnter(object sender, EventArgs e)
        {
            for (int i = 0; i < memberslist.Count; i++)
            {
                if (memberslist[i].Subscription.dateDue.Year < 1000) //If the member's subscription was not paid within 1000 years
                {
                    subscriptionListBox.Items.Add(memberslist[i].id + " :  " + (memberslist[i].Firstname + "," + memberslist[i].Lastname).PadRight(30) +  "Due:" + "Not Paid");
                    //Show that the subscription was never paid
                }
                else
                {
                    string dayPaid = Convert.ToString(memberslist[i].Subscription.dateDue.Day); //Adds a "0" before day/month if they are a single digit
                    if (memberslist[i].Subscription.dateDue.Day < 10)
                    {
                        dayPaid = "0" + memberslist[i].Subscription.dateDue.Day;
                    }
                    string monthPaid = Convert.ToString(memberslist[i].Subscription.dateDue.Month);
                    if (memberslist[i].Subscription.dateDue.Month < 10)
                    {
                        monthPaid = "0" + memberslist[i].Subscription.dateDue.Month;
                    }

                    subscriptionListBox.Items.Add(memberslist[i].id + " :  " + (memberslist[i].Firstname + "," + memberslist[i].Lastname).PadRight(30)
                        + "Due:" + dayPaid + "/" + monthPaid + "/" + (memberslist[i].Subscription.dateDue.Year) + "    " +
                        "Amount Paid: £" + memberslist[i].Subscription.amountPaid); //Show member's subscription details in the listbox
                }
            }
            if (memberListBox.SelectedIndex != -1) //Checks if a member is currently selected in the memberlistbox
            {
                SubscriptionConfirmButton.Visible = true; //Shows subscription confirm & cancel button
                SubscriptionCancelBtn.Visible = true;
                SelectedMemberLbl.Text = "Selected Member:";
                int i = 0;
                int endOfID = Convert.ToString(memberListBox.SelectedItem).IndexOf(":") - 1;
                string id = Convert.ToString(memberListBox.SelectedItem).Remove(endOfID);
                for (int j = 0; j < memberslist.Count; j++)
                {
                    if (memberslist[j].id == Convert.ToInt32(id)) //Gets member's index in the memberlist using the member's id displayed in the listbox
                    {
                        i = j;
                    }
                }
                SelectedMemberTxt.Text = Convert.ToString(subscriptionListBox.Items[i]).Remove(25); //Shows the selected member's name when they are selected
                subscriptionListBox.SelectedIndex = i;//Sets the selected member in subscriptionlistbox to the selected member in memberlistbox
            }
            else
            {
                SubscriptionConfirmButton.Visible = false; //Hide confirm & cancel buttons as no member is selected for editing
                SubscriptionCancelBtn.Visible = false;
            }
        }
        private void tabPage2_Leave(object sender, EventArgs e) 
        {
            subscriptionListBox.Items.Clear(); //Clears subscription listbox when the subscription tab is closed
        }

        private void SubscriptionCancelBtn_Click(object sender, EventArgs e)
        {
            subscriptionListBox.ClearSelected(); //Resets selection and hides buttons
            SubscriptionCancelBtn.Visible = false;
            SubscriptionConfirmButton.Visible = false;
            SelectedMemberLbl.Text = "Select Member from the list below";
            SelectedMemberTxt.Text = "";
        }
        private void LoadIntoListBox(ListBox l) //Used to loop through the memberslist and load all members into a listbox
        {
            for (int i = 0; i < memberslist.Count; i++)
            {
                l.Items.Add(memberslist[i]);
            }
        }
        private void DeleteMemberBtn_Click(object sender, EventArgs e) //When the "Delete member" button is clicked
        {
            int i = memberListBox.SelectedIndex; 
            DialogResult confirm = MessageBox.Show("Are you sure you want to delete this member?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            //Displays a message warning the user that they are about to delete a member
            if (confirm == DialogResult.Yes) //If the user clicks "yes"
            {
                memberslist.RemoveAt(i);//Remove member from memberlist
                memberListBox.Items.Clear();//Remove all members from listbox
                for (int j = 0; j < memberslist.Count; j++)
                {
                    memberListBox.Items.Add(memberslist[j].id + " :  " + memberslist[j].Lastname + "," + memberslist[j].Firstname);//Re-load all members into listbox
                }
                UpdateMemberFile();//Updates all members in the members.csv file
            }
        }


        private void eventDate_DateChanged(object sender, DateRangeEventArgs e)
        {
            bool v = true;
            if (eventDate.SelectionEnd >= eventDate.TodayDate)
            {
                v = false;
            }
            eventEarningsBox.Visible = v;
        }

        private void CreateEventBtn_Click(object sender, EventArgs e)
        {
            DateTime date = eventDate.SelectionStart; //Sets the date to the date selected by the user
            date = date.AddHours(eventTime.Value.Hour); //Adds the selected amount of hours onto the date
            date = date.AddMinutes(eventTime.Value.Minute);//Adds the selected amount of minutes onto the date
            int id = 0;
            bool id_valid;
            do
            {
                id_valid = true;
                for (int i = 0; i < eventlist.Count(); i++)
                {
                    if (eventlist[i].id >= id)
                    {
                        id = eventlist[i].id + 1;  //Finds the next id availible
                        id_valid = false;
                    }
                }
            } while (id_valid == false);
            GroupEvent ev = new GroupEvent(id, date, eventNameTxt.Text, eventDetailsTxt.Text); //Creates a new groupevent with the data entered
            if (eventEarningsBox.Visible == true && EventEarningsTxt.Text != "")
            {
                ev.earnings = Convert.ToDouble(EventEarningsTxt.Text);//Sets the earnings to the data in the event earnings textbox if applicable
            }
            else
            {
                ev.earnings = 0; //Otherwise sets earnings to 0
            }
            addToEventFile(ev); //Adds the new event to the file
            eventlist.Add(ev); //Adds to eventlist
            addEventToListbox(ev);
            ClearEventTextboxes(); //Clears all textboxes
            EditEventBtn.Visible = false; //Resets buttons
            DeleteEventBtn.Visible = false;
            CancelEventBtn.Visible = false;
            eventEarningsBox.Visible = false;
            eventsListBox.ClearSelected();
        }

        private void eventsListBox_DoubleClick(object sender, EventArgs e)//When an event is selected
        {
            int i = eventsListBox.SelectedIndex;
            if (i != -1)//If the event that was double clicked is valid
            {
            eventNameTxt.Text = eventlist[i].name; //Loads member details into the textboxes
            eventDetailsTxt.Text = eventlist[i].details;
            eventTime.Value = eventlist[i].date;
            eventDate.SelectionStart = eventlist[i].date.AddHours(-eventlist[i].date.Hour).AddMinutes(-eventlist[i].date.Minute);
                //Selects the event's date in the date selector, removing the time from it as it is loaded into its own textbox
            eventDate.SelectionEnd = eventlist[i].date.AddHours(-eventlist[i].date.Hour).AddMinutes(-eventlist[i].date.Minute);
            EventEarningsTxt.Text = Convert.ToString(eventlist[i].earnings);
            EditEventBtn.Visible = true;
            DeleteEventBtn.Visible = true;
            CancelEventBtn.Visible = true;
            eventEarningsBox.Visible = true;
            }
        }

        private void EditEventBtn_Click(object sender, EventArgs e) //When an event is being edited
        {
            int i = eventsListBox.SelectedIndex;
            DateTime date = eventDate.SelectionStart;
            date = date.AddHours(eventTime.Value.Hour);
            date = date.AddMinutes(eventTime.Value.Minute);
            if (eventEarningsBox.Visible == true && EventEarningsTxt.Text != "") //Gets the earnings data if applicable
            {
                eventlist[i].earnings = Convert.ToDouble(EventEarningsTxt.Text);
            }
            else
            {
                eventlist[i].earnings = 0;
            }
            eventlist[i].date = date; //Changes event data in the eventlist
            eventlist[i].name = eventNameTxt.Text;
            eventlist[i].details = eventDetailsTxt.Text;
            EditEventBtn.Visible = false;
            DeleteEventBtn.Visible = false;
            CancelEventBtn.Visible = false;
            eventEarningsBox.Visible = false;
            ClearEventTextboxes();
            eventsListBox.ClearSelected();
            UpdateEventFile();//Updates all events in events.csv
            GroupEvent ev = eventlist[i];
            string ev_day = Convert.ToString(ev.date.Day); //Puts a "0" before the day & month if they are less than 10
            if (ev.date.Day < 10) 
            {
                ev_day = "0" + ev.date.Day;
            }
            string ev_month = Convert.ToString(ev.date.Month);
            if (ev.date.Month < 10)
            {
                ev_month = "0" + ev.date.Month;
            }
            eventsListBox.Items[i] = Convert.ToString(ev.id).PadRight(4) + "     " + ev.name.PadRight(20) + "     " 
                + ev_day + " / " + ev_month + " / " + ev.date.Year + "   " + ev.date.ToShortTimeString() + "          "
                + ev.details; //Adds event to the event listbox
        }
        private void ClearEventTextboxes()
        {
            eventNameTxt.Clear();
            eventDetailsTxt.Clear();
            eventTime.ResetText();
            EventEarningsTxt.Clear();
        }
        private void CancelEventBtn_Click(object sender, EventArgs e)
        {
            EditEventBtn.Visible = false; //Hides buttons
            DeleteEventBtn.Visible = false;
            CancelEventBtn.Visible = false;
            eventEarningsBox.Visible = false;
            ClearEventTextboxes();
            eventsListBox.ClearSelected();
        }

        private void DeleteEventBtn_Click(object sender, EventArgs e)
        {
            int i = eventsListBox.SelectedIndex;
            DialogResult confirm = MessageBox.Show("Are you sure you want to delete this Event?", "Confirm", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            //Delete confirmation messagebox
            if (confirm == DialogResult.Yes) //If the user clicks "yes"
            {
                eventlist.RemoveAt(i);//Removes the event from eventlist
                eventsListBox.Items.Clear();//Clears eventlistbox
                for (int j = 0; j < eventlist.Count; j++)//Repopulates eventlistbox using the new eventlist
                {
                    GroupEvent ev = eventlist[j];
                    addEventToListbox(ev);
                }
                UpdateEventFile();//Updates all events in the events file
                ClearEventTextboxes();//Clears any data entered
                eventsListBox.ClearSelected();
                EditEventBtn.Visible = false;
                DeleteEventBtn.Visible = false;
                CancelEventBtn.Visible = false;
                eventEarningsBox.Visible = false;
            }
        }

        private void SortByDateDesc_CheckedChanged(object sender, EventArgs e)
        {
            if (sortByDateDescChkBx.Checked) //If the data is being sorted in descending order
            {
                List<DateTime> dates = new List<DateTime>(); //Makes a list of dates
                List<int> dateindexes = new List<int>(); //Makes a list of dateindexes
                for (int i = 0; i < eventlist.Count; i++)
                {
                    dates.Add(eventlist[i].date); //Populates the lists using the dates of all events
                    dateindexes.Add(i);
                }
                int changes = 0; //Keeps track of number of changes made
                do
                {
                    changes = 0;
                    for (int i = 0; i < dates.Count; i++)//Performs the following for all dates in datelist
                    {
                        TimeSpan difference1 = dates[i] - DateTime.Now; //Gets the difference between now and dates[i]
                        if (i != dates.Count - 1)
                        {
                            TimeSpan difference2 = dates[i + 1] - DateTime.Now; //Gets the difference between now and the next index in dates[]
                            if (difference1 < difference2) //If date1 is closer to today than date2
                            {//Switch the date's places
                                DateTime temp = dates[i];
                                dates[i] = dates[i + 1];
                                dates[i + 1] = temp;
                                int temp2 = dateindexes[i];
                                dateindexes[i] = dateindexes[i + 1];
                                dateindexes[i + 1] = temp2;
                                changes++;
                            }
                        }
                    }
                } while (changes != 0);//Repeat while values are being changed
                List<GroupEvent> neweventlist = new List<GroupEvent>(); //Creates a new list of events
                for (int i = 0; i < eventlist.Count; i++)
                {
                    neweventlist.Add(eventlist[dateindexes[i]]);//Adds events to the new list based on the new sorted order
                }
                eventlist = neweventlist;
                eventsListBox.Items.Clear(); //Clears listbox
                for (int j = 0; j < eventlist.Count; j++)//Repopulates listbox
                {
                    GroupEvent ev = eventlist[j];
                    string ev_day = Convert.ToString(ev.date.Day);
                    if (ev.date.Day < 10)
                    {
                        ev_day = "0" + ev.date.Day;
                    }
                    string ev_month = Convert.ToString(ev.date.Month);
                    if (ev.date.Month < 10)
                    {
                        ev_month = "0" + ev.date.Month;
                    }
                    addEventToListbox(ev);
                }
            }
        }

        private void SortByID_CheckedChanged(object sender, EventArgs e)
        {
            if (sortByIDChkBx.Checked) //If the events are being sorted by ID
            {
                int changes = 0; //Keep track of number of changes
                do
                {
                    changes = 0;
                    for (int i = 0; i < eventlist.Count - 1; i++)
                    {
                        if (eventlist[i].id > eventlist[i + 1].id) //If the current ID is greater than the next ID
                        {
                            GroupEvent temp = eventlist[i];//Switch their places
                            eventlist[i] = eventlist[i + 1];
                            eventlist[i + 1] = temp;
                            changes++;
                        }
                    }
                } while (changes != 0);
                eventsListBox.Items.Clear();//Clear listbox
                for (int j = 0; j < eventlist.Count; j++)//Repopulate listbox
                {
                    GroupEvent ev = eventlist[j];
                    addEventToListbox(ev);
                }
            }
        }

        private void SortByDateAsc_CheckedChanged(object sender, EventArgs e)
        {
            if (sortByDateAscChkBx.Checked) //If the data is being sorted in descending order
            {
                List<DateTime> dates = new List<DateTime>(); //Makes a list of dates
                List<int> dateindexes = new List<int>(); //Makes a list of dateindexes
                for (int i = 0; i < eventlist.Count; i++)
                {
                    dates.Add(eventlist[i].date);//Populates the lists using the dates of all events
                    dateindexes.Add(i);
                }
                int changes = 0;//Keeps track of number of changes made
                do
                {
                    changes = 0;
                    for (int i = 0; i < dates.Count; i++)//Performs the following for all dates in datelist
                    {
                        TimeSpan difference1 = dates[i] - DateTime.Now;//Gets the difference between now and dates[i]
                        if (i != dates.Count - 1)
                        {
                            TimeSpan difference2 = dates[i + 1] - DateTime.Now;//Gets the difference between now and the next index in dates[]
                            if (difference1 > difference2)//If date1 is closer to today than date2
                            {//Switch the date's places
                                DateTime temp = dates[i];
                                dates[i] = dates[i + 1];
                                dates[i + 1] = temp;
                                int temp2 = dateindexes[i];
                                dateindexes[i] = dateindexes[i + 1];
                                dateindexes[i + 1] = temp2;
                                changes++;
                            }
                        }
                    }
                } while (changes != 0);//Repeat while values are being changed
                List<GroupEvent> neweventlist = new List<GroupEvent>();//Creates a new list of events
                for (int i = 0; i < eventlist.Count; i++)
                {
                    neweventlist.Add(eventlist[dateindexes[i]]);//Adds events to the new list based on the new sorted order
                }
                eventlist = neweventlist;
                eventsListBox.Items.Clear();//Clear listbox
                for (int j = 0; j < eventlist.Count; j++)//Repopulate listbox
                {
                    GroupEvent ev = eventlist[j];
                    addEventToListbox(ev);
                }
            }
        }
        private void addEventToListbox(GroupEvent ev)
        {
            string ev_day = Convert.ToString(ev.date.Day);//Converts the day into a string
            if (ev.date.Day < 10)
            {
                ev_day = "0" + ev.date.Day;//Adds a 0 in front if the day is less than 10 to improve readability
            }
            string ev_month = Convert.ToString(ev.date.Month); //Converts the month into a string
            if (ev.date.Month < 10)
            {
                ev_month = "0" + ev.date.Month;//Adds a 0 in front if the month is less than 10 to improve readability
            }
            eventsListBox.Items.Add(ev.id + "     " + ev.name + "     " + ev_day + " / " + ev_month + " / " + ev.date.Year + "   "
                + ev.date.ToShortTimeString() + "          " + ev.details);
        }   //Populates eventslistbox

        private void subscriptionListBox_Click(object sender, EventArgs e)
        {
            if (subscriptionListBox.SelectedIndex != -1) //If the selected member is valid
            {
            SelectedMemberLbl.Text = "Selected member:";
            SelectedMemberTxt.Text = Convert.ToString(subscriptionListBox.SelectedItem).Remove(25);//Displays the currently selected member
            SubscriptionConfirmButton.Visible = true;//Shows confirm & cancel buttons
            SubscriptionCancelBtn.Visible = true;
            }
        }

        private void yearEarningsSearchTxt_SelectedIndexChanged(object sender, EventArgs e)
        {
            findEarnings(); //Finds earnings for the selected year
        }

        private void monthEarningsSearchTxt_SelectedIndexChanged(object sender, EventArgs e)
        {
            findEarnings(); //Finds earnings for the selected month
        }
        private void findEarnings()
        {
            List<String> months = new List<String>(); //List containing all months (3 characters)
            months.Add("Jan"); months.Add("Feb"); months.Add("Mar"); months.Add("Apr"); months.Add("May"); months.Add("Jun");
            months.Add("Jul"); months.Add("Aug"); months.Add("Sep"); months.Add("Oct"); months.Add("Nov"); months.Add("Dec"); 
            double eventearnings = 0;//Initialising values
            double subearnings = 0;
            double earningsNow = 0;
            if (String.IsNullOrWhiteSpace(monthEarningsSearchTxt.Text) || monthEarningsSearchTxt.Text == "ALL") //If a month is not specified
            {
                //Find total event earnings in selected year:
                for (int i = 0; i < eventlist.Count; i++)
                {
                    if (eventlist[i].date.Year == Convert.ToInt32(yearEarningsSearchTxt.Text))
                    {
                        eventearnings += eventlist[i].earnings;
                    }
                }
                //Find total subscription earnings in selected year:
                for (int i = 0; i < memberslist.Count; i++)
                {
                    if (memberslist[i].Subscription.datePaid.Year == Convert.ToInt32(yearEarningsSearchTxt.Text))
                    {
                        subearnings += memberslist[i].Subscription.amountPaid;
                    }
                }
                subEarningsSearchLbl.Text = "£" + subearnings; //Display the subscription & event earnings
                eventEarningsSearchLbl.Text = "£" + eventearnings;
                double total = subearnings + eventearnings;//Display the total earnings for the selected year
                totalEarningsSearchLbl.Text = "£" + total;
                //Find total earnings for this month
                comparisonLbl.Text = "Compared to this year:";
                for (int i = 0; i < eventlist.Count; i++)
                {
                    if (eventlist[i].date.Year == DateTime.Today.Year)//Finds the event earnings for this year
                    {
                        earningsNow += eventlist[i].earnings; //Add up total
                    }
                }
                for (int i = 0; i < memberslist.Count; i++)
                {
                    if (memberslist[i].Subscription.datePaid.Year == DateTime.Today.Year) //Finds subscription earnings for this year
                    {
                        earningsNow += memberslist[i].Subscription.amountPaid;  //Add up total (including events)
                    }
                }
                double percentchange = ((total / earningsNow) - 1) * 100; //Calculate the % change in earnings from the selected year to this year
                totalComparisonEarningsSearchLbl.Text = percentchange + "%";//Display percentage change
            }
            else//If a month is specified
            {
                comparisonLbl.Text = "Compared to this month:";
                int monthNum = months.IndexOf(monthEarningsSearchTxt.Text) + 1;
                //Find event earnings in selected month & year
                for (int i = 0; i < eventlist.Count; i++)
                {
                    if (eventlist[i].date.Year == Convert.ToInt32(yearEarningsSearchTxt.Text) && eventlist[i].date.Month == monthNum)
                    {
                        eventearnings += eventlist[i].earnings;
                    }
                }
                //Find subscription earnings in selected month & year
                for (int i = 0; i < memberslist.Count; i++)
                {
                    if (memberslist[i].Subscription.datePaid.Year == Convert.ToInt32(yearEarningsSearchTxt.Text)
                        && memberslist[i].Subscription.datePaid.Month == monthNum)
                    {
                        subearnings += memberslist[i].Subscription.amountPaid;
                    }
                }
                subEarningsSearchLbl.Text = "£" + subearnings;//Display total subscription earnings for selected month & year
                eventEarningsSearchLbl.Text = "£" + eventearnings;//Display total event earnings for selected month & year
                double total = subearnings + eventearnings; //Calculate overall earnings for selected month & year
                totalEarningsSearchLbl.Text = "£" + total;//display overall earnings
                //Find total earnings for this month
                for (int i = 0; i < eventlist.Count; i++)
                {
                    if (eventlist[i].date.Year == DateTime.Today.Year && eventlist[i].date.Month == DateTime.Today.Month)//Finds the earnings for this month
                    {
                        earningsNow += eventlist[i].earnings;
                    }
                }
                for (int i = 0; i < memberslist.Count; i++)
                {
                    if (memberslist[i].Subscription.datePaid.Year == DateTime.Today.Year && memberslist[i].Subscription.datePaid.Month == DateTime.Today.Month)
                    {
                        earningsNow += memberslist[i].Subscription.amountPaid;
                    }
                }
                double percentchange = ((total / earningsNow)-1) * 100; //Calculates the percentage change from the selected month & year to this year
                totalComparisonEarningsSearchLbl.Text = percentchange + "%"; //Displays percentage change
            }
        }
    }
}