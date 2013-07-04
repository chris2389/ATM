using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;

namespace ATM
{
    public class AtmScreen : Form

    {
        private Button start = new Button();
        private Button finish = new Button();
        private Button enter = new Button();
        private TextBox dataEntry = new TextBox();
        private RadioButton savings = new RadioButton();
        private RadioButton checking = new RadioButton();
        private ComboBox transaction = new ComboBox();
        private String message = "Welcome to Art's Bank";
        private Bank bank = new Bank();
        private Person[] person = Database.PersonData();
        private Teller teller;
        private Font font = new Font(FontFamily.GenericSerif, 18, FontStyle.Bold);
        
        public AtmScreen()
        {
            Text = "Atm Screen";
            start.Text = "Start";
            finish.Text = "Finish";
            savings.Text = "Savings";
            checking.Text = "Checking";
            enter.Text = "Enter Name";

            //Set sizes
            Size = new Size(400, 200);
            enter.Width = 100;

            //Set locations
            int w = 20;
            dataEntry.Location = new Point(w, 5);
            enter.Location = new Point(w, 35);
            savings.Location = new Point(w += 10 + dataEntry.Width, 5);
            checking.Location = new Point(w, 35);
            transaction.Location = new Point(w += 10 + savings.Width, 15);
            start.Location = new Point(120, 120);
            finish.Location = new Point(130 + start.Width, 120);

            //Add transaction types
            transaction.Items.Add("Deposit");
            transaction.Items.Add("Withdraw");
            transaction.Items.Add("Balance");

            //Add controls to form
            Controls.Add(start);
            Controls.Add(finish);
            Controls.Add(dataEntry);
            Controls.Add(savings);
            Controls.Add(checking);
            Controls.Add(transaction);
            Controls.Add(enter);

            AcceptButton = enter;

            //Register event handles
            enter.Click += new EventHandler(Enter_Click);
            start.Click += new EventHandler(Start_Click);
            finish.Click += new EventHandler(Finish_Click);
            savings.CheckedChanged += new EventHandler(Savings_Changed);
            checking.CheckedChanged += new EventHandler(Checking_Changed);
            transaction.SelectedIndexChanged +=new EventHandler(Selected_Index);
            Clear();
            InitializeComponent();
        }

        private void Start_Click(object sender, EventArgs e)
        {
            dataEntry.Enabled = true;
            dataEntry.Focus();
            enter.Enabled = true;
            start.Enabled = false;
            Invalidate();
        }

        private void Enter_Click(object sender, EventArgs e)
        {
            String s = enter.Text;
            if (s == "Enter Name")
            {
                String name = dataEntry.Text;
                if (name == "John Venn")
                    teller.AcceptCard(person[0]);
                else if (name == "Mabel Venn")
                    teller.AcceptCard(person[1]);
                else
                {
                    message = "Enter John Venn or Mabel Venn";
                    dataEntry.Text = "";
                    Invalidate();
                }
            }
            else if (s == "Enter PIN")
                teller.AcceptPIN(dataEntry.Text);
            else if (s == "Enter Amount")
                teller.AcceptAmount(double.Parse(dataEntry.Text));
        }

        private void Finish_Click(object sender, EventArgs e)
        {
            Clear();
        }

        protected void Selected_Index(Object sender, EventArgs e)
        {
            teller.AcceptTransaction(transaction.SelectedIndex);
        }

        protected void Savings_Changed(Object sender, EventArgs e)
        {
            teller.AcceptType(Bank.SAVINGS);
        }

        protected void Checking_Changed(Object sender, EventArgs e)
        {
            teller.AcceptType(Bank.CHECKING);
        }

        public void EnterPIN()
        {
            dataEntry.Text = "";
            enter.Text = "Enter PIN";
            message = "Enter your PIN number";
            Invalidate();
        }

        public void SelectTransaction()
        {
            dataEntry.Text = "";
            dataEntry.Enabled = false;
            enter.Enabled = false;
            transaction.Enabled = true;
            message = "Select your transaction";
            Invalidate();
        }

        public void SelectType()
        {
            savings.Enabled = true;
            checking.Enabled = true;
            transaction.Enabled = false;
            message = "Select your account type";
            Invalidate();
        }

        public void SpecifyAmount()
        {
            dataEntry.Enabled = true;
            dataEntry.Focus();
            enter.Enabled = true;
            enter.Text = "Enter Amount";
            savings.Enabled = false;
            checking.Enabled = false;
            message = "Specify the amount";
            Invalidate();
        }

        public void Display(String s)
        {
            dataEntry.Text = "";
            enter.Enabled = false;
            savings.Enabled = true;
            checking.Enabled = false;
            message = s;
            Invalidate();
        }

        public void Clear()
        {
            start.Enabled = true;
            savings.Enabled = false;
            checking.Enabled = false;
            transaction.Enabled = false;
            enter.Text = "Enter Name";
            enter.Enabled = false;
            dataEntry.Text = "";
            dataEntry.Enabled = false;
            message = "Welcome to Art's bank";
            Invalidate();
            teller = new Teller(bank, this);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            int w = (int)g.MeasureString(message, font).Width;
            g.DrawString(message, font, Brushes.Blue, (Width - w) / 2, 80);
        }

     //Teller class
        class Teller
        {
            public const int DEPOSIT = 0;
            public const int WITHDRAW = 1;
            public const int BALANCE = 2;
            private String id;
            private int transType;
            private int acctType;
            private Person user;
            private Bank bank;
            private Account account;
            private AtmScreen screen;

            public Teller(Bank b, AtmScreen s)
            {
                bank = b;
                screen = s;
            }

            public void AcceptCard(Person p)
            {
                user = p;
                screen.EnterPIN();
            }

            public void AcceptPIN(String s)
            {
                id = s;
                screen.SelectTransaction();
            }

            public void AcceptTransaction(int trans)
            {
                transType = trans;
                screen.SelectType();
            }
            public void AcceptType(int type)
            {
                acctType = type;
                bank.Find(id, acctType, this);
            }
            public void AcceptAccount(Account a)
            {
                account = a;
                if (account != null)
                    if (transType == BALANCE)
                    {
                        screen.Display("The balance is {0:C}" + account.GetBalance());
                    }
                    else
                    {
                        if (transType == DEPOSIT || transType == WITHDRAW)
                        {
                            screen.SpecifyAmount();
                        }
                    }
                else
                    screen.Display("No such account -- session terminated");

            }

            public void AcceptAmount(double amount)
            {
                switch (transType)
                {
                    case DEPOSIT:
                        account.Deposit(amount);
                        screen.Display("Deposit of " + amount.ToString("C"));
                        break;
                    case WITHDRAW:
                        double taken = account.Withdraw(amount);
                        if (taken >= 0)
                            screen.Display("Withdrawal of " + taken.ToString("C"));
                        else
                            screen.Display("Insufficient funds");
                        break;
                }
            }
        }

     //Database Class
        class Database
        {
            public static Person[] PersonData()
            {
                Name n1 = new Name("John", "Venn");
                Address a1 = new Address("123 Main St.", "Tyler", "WY", "45654");
                Person p1 = new Person("123123123", n1, a1);
                Name n2 = new Name("Mabel", "Venn");
                Person p2 = new Person("456456456", n2, a1);
                Person[] p = { p1, p2 };
                return p;
            }

            public static Account[] AccountData()
            {
                Person[] p = PersonData();
                Account p1Savings = new Savings(1500.00, p[0], 4.0);
                Account p1Checking = new Checking(p[0], 2500.00, .50);
                Account p2Savings = new Savings(1000.00, p[1], 3.5);
                Account[] a = { p1Savings, p1Checking, p2Savings };
                return a;
            }


        }

     //Bank Class
        class Bank
        {
            public const int SAVINGS = 1;
            public const int CHECKING = 2;
            private Account[] accounts = Database.AccountData();

            public void Find(String id, int acctType, Teller teller)
            {
                for (int i = 0; i < accounts.Length; i++)
                {
                    Account acct = accounts[i];
                    if (acct.GetId() == id)
                    {
                        switch (acctType)
                        {
                            case SAVINGS:
                                if (acct is Savings)
                                {
                                    teller.AcceptAccount(acct);
                                    return;
                                }
                                break;
                            case CHECKING:
                                if (acct is Checking)
                                {
                                    teller.AcceptAccount(acct);
                                    return;
                                }
                                break;
                        }
                    }
                    teller.AcceptAccount(null);
                }
            }

        }

     

      public static void Main()
      {
          Application.Run(new AtmScreen());
      }

        

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // AtmScreen
            // 
            this.ClientSize = new System.Drawing.Size(284, 262);
           //ATM.AtmScreen.Name = "AtmScreen";
            
            this.ResumeLayout(false);

        }

        

       
     
       

       
        

        
        
    }
}
