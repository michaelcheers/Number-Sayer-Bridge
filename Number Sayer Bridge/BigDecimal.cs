using Bridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Number_Sayer_Bridge
{
    public class BigDecimal
    {
        public BigInteger value;
        public int pow10Div;

        public BigDecimal (BigInteger value, int pow10Div)
        {
            this.value = value;
            this.pow10Div = pow10Div;
        }

        public static BigDecimal Parse (string value)
        {
            string[] decPoints = value.Split('.');
            switch (decPoints.Length)
            {
                case 1:
                    return new BigDecimal(BigInteger.Parse(value), 0);
                case 2:
                    string b = decPoints[1];
                    return new BigDecimal(BigInteger.Parse(decPoints[0] + b), decPoints[1].Length);
                default:
                    throw new FormatException();
            }
        }

        public BigInteger PartA
        {
            get
            {
                return value / BigInteger.Pow(10, pow10Div);
            }
        }

        public BigInteger PartB
        {
            get
            {
                return value % BigInteger.Pow(10, pow10Div);
            }
        }

        [Name("N0s")]
        public BigInteger Decimal0sAtBeginningOfPartB
        {
            get
            {
                if (pow10Div == 0 || PartB == 0)
                    return 0;
                int n0s = 0;
                foreach (var item in ToString().Split('.')[1])
                {
                    if (item == '0')
                        n0s++;
                    else
                        return n0s;
                }
                throw new Exception("Something bad happenned.");
            }
        }

        public override string ToString()
        {
            string vString = value.ToString();
            if (pow10Div == 0)
                return vString;
            return vString.Insert(vString.Length - pow10Div, ".");
        }
    }
}
