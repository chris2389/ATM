using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATM
{
    class Savings : Account
    {
        private double interestRate;
        
        public Savings(double amount, Person p, double rate)
            : base(amount,p)
            {
                interestRate = rate;
            }
        public void PostInterest()
        {
            double balance = GetBalance();
            double interest = interestRate / 100 * balance;
            Deposit(interest);
        }

    }
}
