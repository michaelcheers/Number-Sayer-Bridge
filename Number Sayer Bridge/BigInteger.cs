using Bridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenNum = Bridge.Any<int, System.Numerics.BigInteger>;
namespace System.Numerics
{
    [External]
    [Name("bigInt")]
    [Constructor("bigInt")]
#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
    public struct BigInteger
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
    {
        public static extern BigInteger One
        {
            [Template("bigInt(1)")]
            get;
        }

        [Template("bigInt({0})")]
        public static extern implicit operator BigInteger(int value);

        [Template("{value}.toJSNumber()")]
        public static extern explicit operator int(BigInteger value);

        [Template("{0}.pow({1})")]
        public static extern BigInteger Pow(BigInteger a, GenNum b);

        [Template("bigInt({0}).pow({1})")]
        public static extern BigInteger Pow(int a, GenNum b);

#pragma warning disable CS0626 // Method, operator, or accessor is marked external and has no attributes on it
#pragma warning disable CS0824 // Method, operator, or accessor is marked external and has no attributes on it

        [Template("bigInt({value}, {radix})")]
        public static extern BigInteger Parse(string value, int radix = 10);

        public extern BigInteger(int value = 0);

        public static extern BigInteger Zero
        {
            [Template("bigInt.zero")]
            get;
        }

        public extern bool IsOne
        {
            [Template("eq(1)")]
            get;
        }

        public override extern string ToString();
#pragma warning restore CS0626 // Method, operator, or accessor is marked external and has no attributes on it
#pragma warning restore CS0824 // Method, operator, or accessor is marked external

        [Template("{0}.and({1})")]
        public static extern BigInteger operator &(BigInteger a, GenNum b);

        [Template("{0}.or({1})")]
        public static extern BigInteger operator |(BigInteger a, GenNum b);

        [Template("{0}.eq({1})")]
        public static extern bool operator ==(BigInteger a, GenNum b);

        [Template("{0}.neq({1})")]
        public static extern bool operator !=(BigInteger a, GenNum b);

        [Template("{0}.gt({1})")]
        public static extern bool operator >(BigInteger a, GenNum b);

        [Template("{value}.abs()")]
        public static extern BigInteger Abs(BigInteger value);

        [Template("bigInt.gcd({0}, {1})")]
        public static extern BigInteger GreatestCommonDivisor(BigInteger numerator, BigInteger denominator);

        [Template("{0}.greaterOrEquals({1})")]
        public static extern bool operator >=(BigInteger a, GenNum b);

        [Template("{0}.lesserOrEquals({1})")]
        public static extern bool operator <=(BigInteger a, GenNum b);

        [Template("{0}.lt({1})")]
        public static extern bool operator <(BigInteger a, GenNum b);

        [Template("{0}.shiftLeft({1})")]
        public static extern BigInteger operator <<(BigInteger a, int b);

        [Template("{0}.shiftRight({1})")]
        public static extern BigInteger operator >>(BigInteger a, int b);

        [Template("{0}.negate()")]
        public static extern BigInteger operator - (BigInteger value);

        [Template("{0}.minus({1})")]
        public static extern BigInteger operator -(BigInteger a, GenNum b);

        [Template("{0}.times({1})")]
        public static extern BigInteger operator *(BigInteger a, GenNum b);

        [Template("{0}.over({1})")]
        public static extern BigInteger operator /(BigInteger a, GenNum b);

        [Template("{0}.add({1})")]
        public static extern BigInteger operator +(BigInteger a, GenNum b);

        [Template("{0}.mod({1})")]
        public static extern BigInteger operator %(BigInteger a, GenNum b);
    }
}