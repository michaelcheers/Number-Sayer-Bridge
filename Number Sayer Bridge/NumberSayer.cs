using Bridge;
using Bridge.Html5;
using System;
using System.Collections.Generic;

namespace Number_Sayer_Bridge
{
    using Number = BigInteger;
    class NumberSayer
    {
        public Language language;
        public Sound[] smalls;
        public string Voice;

        public NumberSayer(string voice = "Michael")
        {
            Voice = voice;
            smalls = new Sound[]{
            LoadSound("0"),
            LoadSound("1"),
            LoadSound("2"),
            LoadSound("3"),
            LoadSound("4"),
            LoadSound("5"),
            LoadSound("6"),
            LoadSound("7"),
            LoadSound("8"),
            LoadSound("9"),
            LoadSound("10"),
            LoadSound("11"),
            LoadSound("12")
        };
            thir = LoadSound("thir");
            fif = LoadSound("fif");
            hundred = LoadSound("hundred");
            and = LoadSound("and");
            ty = LoadSound("ty");
        }

        public readonly Sound thir;
        public readonly Sound fif;
        public readonly Sound hundred;
        public readonly Sound and;
        public readonly Sound ty;

        public Dictionary<string, Sound> alreadyDone = new Dictionary<string, Sound>();

        public Sound LoadSound(string value)
        {
            if (alreadyDone.ContainsKey(value))
                return alreadyDone[value];
            return (alreadyDone[value] = new Sound(new AudioElement("Sounds/" + Voice + "/" + value + ".wav")));
        }

        public Sound GetThirFifSound(Number value)
        {
            switch ((int)value)
            {
                case 1:
                    return LoadSound("1");
                case 2:
                    return LoadSound("2");
                case 3:
                    return thir;
                case 4:
                    return LoadSound("4");
                case 5:
                    return fif;
                case 6:
                    return LoadSound("6");
                case 7:
                    return LoadSound("7");
                case 8:
                    return LoadSound("8");
                case 9:
                    return LoadSound("9");
            }
            throw new ArgumentException(value + " should only be 1 digit.");
        }

        public Sound Say(Number value)
        {
            Sound result = new Sound();
            if (value < 1000)
            {
                if (value < 100)
                {
                    if (value < 30)
                    {
                        if (value < 20)
                        {
                            if (value < 13)
                            {
                                result.AppendThis(smalls[(int)value]);
                                return result;
                            }
                            result.AppendThis(GetThirFifSound(value % 10));
                            result.AppendThis(LoadSound("teen"));
                            return result;
                        }
                        result.AppendThis(LoadSound("20"));
                        var dig22 = value % 10;
                        if (dig22 != 0)
                            result.AppendThis(Say(dig22));
                        return result;
                    }
                    var dig1 = value / 10;
                    var dig2 = value % 10;
                    result.AppendThis(GetThirFifSound(dig1));
                    result.AppendThis(ty);
                    if (dig2 != 0)
                        result.AppendThis(Say(dig2));
                    return result;
                }

                var hundred = value / 100;
                var remainder = value % 100;
                result.AppendThis(Say(hundred));
                result.AppendThis(this.hundred);
                if (remainder != 0)
                {
                    result.AppendThis(and);
                    result.AppendThis(Say(remainder));
                }
                return result;
            }
            Number current = 1;
            int n = 0;
            for (; value >= current; n++, current *= 1000) ;
            n -= 2;
            current /= 1000;
            while (true)
            {
                var condition = n == -1;
                var currentVal = (value / current) % 1000;
                if (currentVal != 0)
                {
                    if (currentVal < 100 && condition)
                        result.AppendThis(and);
                    result.AppendThis(Say(currentVal));
                    if (!condition)
                        result.AppendThis(LoadSound(placeValues[n]));
                }
                current /= 1000;
                n--;
                if (current == 0)
                    return result;
            }
        }

        static string[] placeValues = new string[] { "thousand", "million", "billion", "trillion", "quadrillion", "quintillion", "sextillion", "septillion", "octillion", "nonillion", "decillion", "undecillion", "duodecillion", "tredecillion", "quattuordecillion", "quindecillion", "sedecillion" };
        public enum Language
        {
            English
        }
    }
}
