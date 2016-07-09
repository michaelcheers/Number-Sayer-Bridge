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
                return BigInteger.Abs(value % BigInteger.Pow(10, pow10Div));
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
                    switch (item)
                    {
                        case '0':
                            n0s++;
                            break;
                        case '-':
                            break;
                        default:
                            return n0s;
                        }
                }
                throw new Exception("Something bad happenned.");
            }
        }

        public override string ToString()
        {
            bool negative = value < BigInteger.Zero;
            string vString = value.ToString();
            if (negative)
                vString = vString.Substring(1);
            if (pow10Div == 0)
                return vString;
            int insertLoc = vString.Length - pow10Div;
            if (insertLoc < 0)
            {
                string leftPiece = "";
                for (int n = 0; n > insertLoc; n--) leftPiece += "0";
                vString = leftPiece + vString;
                insertLoc = 0;
            }
            return vString.Insert(insertLoc, "." + (negative ? "-" : ""));
        }
    }
}
