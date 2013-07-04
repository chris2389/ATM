using System;

namespace ATM
{

    public class Checking : Account
    {
        private double minBalance;
        private double charge;

        public Checking(Person p, double minAmount, double charge)
            : base(minAmount, p)
        {
            minBalance = minAmount;
            this.charge = charge;
        }

        public double ProcessCheck(double amount)
        {
            double result;
            if (GetBalance() >= minBalance)
                result = base.Withdraw(amount);
            else
                result = base.Withdraw(amount + charge);
            return result;

        }

        //public static double Withdraw(double amount)
        //{
        //    return ProcessCheck(amount);
        //}
    }


}