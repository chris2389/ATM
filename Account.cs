using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATM
{
    //Account Class
    public abstract class Account
    {
        private double balance;
        private Person holder;

        public Account(Person p)
            : this(0, p)
        {

        }

        public Account(double initialAmount, Person p)
        {

            balance = initialAmount;
            holder = p;
        }

        public String GetId()
        {
            return holder.GetId();
        }

        public void Deposit(double amount)
        {
            balance += amount;
        }

        public virtual double Withdraw(double amount)
        {
            if (balance >= amount)
            {
                balance -= amount;
                return amount;
            }
            else
                return -1.0;

        }

        public double GetBalance()
        {
            return balance;
        }
    }
}
