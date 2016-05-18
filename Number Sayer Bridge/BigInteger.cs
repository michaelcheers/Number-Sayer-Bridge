using Bridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenNum = Bridge.Any<int, BigInteger>;

[External]
[Name("bigInt")]
#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
public class BigInteger
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
{
    [Template("bigInt({0})")]
    public static extern implicit operator BigInteger (int value);

    [Template("{value}.toJSNumber()")]
    public static extern explicit operator int (BigInteger value);

    [Template("{0}.pow({1})")]
    public static extern BigInteger Pow(BigInteger a, GenNum b);

    [Template("bigInt({0}).pow({1})")]
    public static extern BigInteger Pow(GenNum a, GenNum b);

#pragma warning disable CS0626 // Method, operator, or accessor is marked external and has no attributes on it
#pragma warning disable CS0824 // Method, operator, or accessor is marked external and has no attributes on it
    public extern BigInteger(string value, int radix = 10);
    
    public extern BigInteger(int value = 0);

    public static extern BigInteger Zero
    {
        [Template("bigInt.zero")]
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

    [Template("{0}.greaterOrEquals({1})")]
    public static extern bool operator >= (BigInteger a, GenNum b);

    [Template("{0}.lesserOrEquals({1})")]
    public static extern bool operator <=(BigInteger a, GenNum b);

    [Template("{0}.lt({1})")]
    public static extern bool operator <(BigInteger a, GenNum b);

    [Template("{0}.shiftLeft({1})")]
    public static extern BigInteger operator <<(BigInteger a, int b);

    [Template("{0}.shiftRight({1})")]
    public static extern BigInteger operator >>(BigInteger a, int b);

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