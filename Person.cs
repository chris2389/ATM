using System;


namespace ATM
{
    public class Person
    {
        private String id;
        private Name name;
        private Address address;
        public Person(String i, Name n, Address a)
        {
            id = i;
            name = n;
            address = a;
        }
        public String GetId()
        {
            return id;
        }
        public override String ToString()
        {
            return name + "\n" + address;
        }
    }
}