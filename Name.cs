using System;

namespace ATM
{

    public class Name
    {
        private String first;
        private char initial;
        private String last;

        public Name(String f, String l)
        {
            first = f;
            last = l;
        }
        public Name(String f, char i, String l)
            : this(f, l)
        {
            initial = i;
        }
        public override string ToString()
        {
            if (initial == '\u0000')
                return first + "  " + last;
            else
                return first + " " + initial + "" + last;
        }
    }

}