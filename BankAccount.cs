using System;

namespace ATM
{
    public class BankAccount
    {

        private double balance;

        //Creates a Bank Account with a balance of 0 
        public BankAccount()
        {

        }

        //Creates a Bank Account with a blance of initial Amout
        public BankAccount(double initialAmount)
        {
            balance = initialAmount;
        }

        //Returns the balance
        public double GetBalance()
        {
            return balance;
        }

        //Increase balance by amount
        public void Deposit(double amount)
        {
            balance += amount;
        }

        //Reduces balance by amount, if possible
        public double Withdraw(double amount)
        {
            if (balance >= amount)
            {
                balance -= amount;
                return balance;
            }
            else
                return -1.0;
        }
    }

}
    

