﻿using System;
using System.Collections.Generic;
using System.Linq;
using Bridge.Linq;
using Bridge;
using Bridge.Html5;
using System.Numerics;

namespace Number_Sayer_Bridge
{
    using WholeNumber = BigInteger;
    using Number = BigDecimal;
    
    [Namespace(false)]
    public class NumberSayer
    {
        public Language language;
        private readonly Sound[] smalls;
        public string voice;

        public NumberSayer(Language language = Language.English, string voice = "Michael")
        {
            if (!Enum.IsDefined(typeof(Language), language))
                throw new ArgumentOutOfRangeException("language", "Value should be defined in the Language enum.");
            this.language = language;
            this.voice = voice;
            switch (language)
            {
                case Language.Esperanto:
                    {
                        smalls = new[]
                        {
                            LoadSound("0"),
                            LoadSound("1"),
                            LoadSound("2"),
                            LoadSound("3"),
                            LoadSound("4"),
                            LoadSound("5"),
                            LoadSound("6"),
                            LoadSound("7"),
                            LoadSound("8"),
                            LoadSound("9")
                        };
                        break;
                    }
                case Language.English:
                    {
                        smalls = new[]
                        {
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
                        break;
                    }
                case Language.Spanish:
                    {
                        smalls = new[]
                        {
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
                            LoadSound("12"),
                            LoadSound("13"),
                            LoadSound("14"),
                            LoadSound("15")
                        };
                        break;
                    }
                case Language.French:
                    {
                        smalls = new[]
                        {
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
                            LoadSound("12"),
                            LoadSound("13"),
                            LoadSound("14"),
                            LoadSound("15"),
                            LoadSound("16")
                        };
                        break;
                    }
                case Language.German:
                    {
                        smalls = new[]
                        {
                            LoadSound("0"),
                            LoadSound("eins"),
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
                        break;
                    }
                case Language.Dutch:
                    {
                        smalls = new[]
                        {
                            LoadSound("0"),
                            LoadSound("eins"),
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
                            LoadSound("12"),
                            LoadSound("13")
                        };
                        break;
                    }
            }
        }

        Sound thir { get { return LoadSound("thir"); } }
        Sound fif { get { return LoadSound("fif"); } }
        Sound and { get { return LoadSound("and"); } }
        Sound ty { get { return LoadSound("ty"); } }
        readonly Random rnd = new Random();

        public static readonly Dictionary<Language, string[]> knownVoices = new Dictionary<Language, string[]>
        {
            {Language.English,        new[] {"Ally", "Ally (New)", "Ben (Silly)", "Erlantz", "Jeff", "Laurie", "Melissa", "Michael", "Pedro", "Seamus", "Sylvia" } },
            {Language.Spanish,        new[] {"Ana", "Sylvia"} },
            {Language.French,         new[] {"Ben", "Melissa"} },
            {Language.Esperanto,      new[] {"Michael"} },
            {Language.German,         new[] {"Ally", "Laurie", "Leire"} },
            {Language.Roman_Numerals, new[] {"Michael"} },
            {Language.Binary_Short,   new[] {"Michael"} },
            {Language.Dutch,    new string[]{ } }
        };

        private static readonly Dictionary<Language, int> irregularStarters = new Dictionary<Language, int>
        {
            {Language.English, 13 },
            {Language.Dutch, 14 },
            {Language.German, 13 },
            {Language.Spanish, 16 },
            {Language.French, 17 },
            {Language.Esperanto, 10 },
        };

        [InlineConst]
        const int shortNumberScale = 1000   ;
        [InlineConst]
        const int longNumberScale = 1000000 ;
        [InlineConst]
        const int chineseNumberScale = 10000;

        public static readonly Dictionary<Language, int> numberScale = new Dictionary<Language, int>
        {
            {Language.English, shortNumberScale },
            {Language.French,  shortNumberScale },
            {Language.German, shortNumberScale },
            {Language.Dutch, shortNumberScale },
            {Language.Spanish,  longNumberScale },
            {Language.Esperanto, shortNumberScale },
            {Language.Roman_Numerals, 10 }
        };

        public Dictionary<string, Sound> alreadyDone = new Dictionary<string, Sound>();

        public Sound LoadSound(string value)
        {
            if (alreadyDone.ContainsKey(value))
                return alreadyDone[value];
            HTMLAudioElement[] mixedResult = { };
            string format = "Sounds/" + language.ToString() + "/{0}/{1}.wav";
            try
            {
                switch (voice)
                {
                    case "mixed":
                        foreach (var item in knownVoices[language])
                            mixedResult.Push(CreateAudio(string.Format(format, item, value), mixedResult));
                        break;
                    default:
                        mixedResult.Push(CreateAudio(string.Format(format, voice, value), mixedResult));
                        break;
                }
            }
            catch (KeyNotFoundException)
            {
                mixedResult.Push(new HTMLAudioElement(string.Format(format, "", "")));
            }
            return (alreadyDone[value] = new Sound(new Audio(mixedResult, value, rnd)));
        }

        private HTMLAudioElement CreateAudio(string value, HTMLAudioElement[] mixedResult)
        {
            HTMLAudioElement result = new HTMLAudioElement();
            result.OnError = (message, url, lineNumber, columnNumber, error) =>
            {
                var index = mixedResult.IndexOf(result);
                if (index > -1)
                    mixedResult.Splice(index, 1);
                return Global.Undefined.As<bool>();
            };
            HTMLSourceElement source = new HTMLSourceElement
            {
                Src = value,
                OnError = result.OnError
            };
            result.AppendChild(source);
            return result;
        }

        public Sound GetThirFifSound(WholeNumber value)
        {
            switch ((int)value)
            {
                case 1:
                    return LoadSound("1");
                case 2:
                    return LoadSound("2");
                case 3:
                    return language == Language.English ? thir : LoadSound("3");
                case 4:
                    return LoadSound("4");
                case 5:
                    return language == Language.English ? fif : LoadSound("5");
                case 6:
                    return LoadSound("6");
                case 7:
                    return LoadSound(language != Language.German ? "7" : "sieb");
                case 8:
                    return LoadSound(language != Language.German ? "eigh" : "8");
                case 9:
                    return LoadSound("9");
            }
            throw new ArgumentException(value + " should only be 1 digit.");
        }

        public Sound Say (WholeNumber value)
        {
            Sound result = new Sound();
            if (language == Language.Binary_Short)
            {
                WholeNumber num = 1;
                int bit = 0;
                for (; num < value; num<<=1, bit++) ;
                for (WholeNumber currentNum = num; currentNum != 0; currentNum >>= 1, bit--)
                    if ((value & currentNum) == currentNum)
                        result.AppendThis(SayBit(bit));
                return result;
            }
            if (language != Language.Roman_Numerals)
            {
                if (value < 0)
                {
                    return LoadSound("minus").Append(Say(-value));
                }
                if (value < 1000000)
                {
                    if (value < 1000)
                    {
                        if (value < 100)
                        {
                            if (value < 20)
                            {
                                if (value < irregularStarters[language])
                                {
                                    result.AppendThis(smalls[(int)value]);
                                    return result;
                                }
                                switch (language)
                                {
                                    case Language.English:
                                    case Language.German:
                                    case Language.Dutch:
                                        {
                                            result.AppendThis(GetThirFifSound(value % 10));
                                            result.AppendThis(LoadSound(language != Language.English ? "10" : "teen"));
                                            return result;
                                        }
                                }
                            }
                            int dig1 = (int)(value / 10);
                            int dig2 = (int)(value % 10);
                            switch (language)
                            {
                                case Language.English:
                                case Language.German:
                                case Language.Dutch:
                                    {
                                        if (language != Language.English && dig2 != 0)
                                        {
                                            result.AppendThis(GetEinSound(dig2));
                                            result.AppendThis(and);
                                        }
                                        if (dig1 == 2)
                                            result.AppendThis(LoadSound("20"));
                                        else
                                        {
                                            result.AppendThis(GetThirFifSound(dig1));
                                            result.AppendThis(ty);
                                        }
                                        if (language != Language.English)
                                            return result;
                                        break;
                                    }
                                case Language.Esperanto:
                                    {
                                        if (dig1 != 1)
                                            result.AppendThis(Say(dig1));
                                        result.AppendThis(LoadSound("10"));
                                        break;
                                    }
                                case Language.Spanish:
                                    {
                                        result.AppendThis(LoadSound(dig1 + "0"));
                                        if (dig2 != 0)
                                            result.AppendThis(and);
                                        break;
                                    }
                                case Language.French:
                                    {
                                        int dig120 = (int)value / 20;
                                        BigInteger dig220 = value % 20;
                                        switch (dig120)
                                        {
                                            case 3:
                                            case 4:
                                                {
                                                    result.AppendThis(LoadSound(dig120 * 2 + "0"));
                                                    if (dig120 == 3 && dig220 == 1)
                                                        result.AppendThis(and);
                                                    if (dig220 != 0)
                                                        result.AppendThis(Say(dig220));
                                                    return result;
                                                }
                                            default:
                                                {
                                                    result.AppendThis(LoadSound(dig1 + "0"));
                                                    if (dig2 == 1)
                                                        result.AppendThis(and);
                                                    break;
                                                }
                                        }
                                        break;
                                    }
                            }
                            if (dig2 != 0)
                                result.AppendThis(Say(dig2));
                            return result;
                        }

                        int hundred = (int)(value / 100);
                        int remainder = (int)(value % 100);
                        switch (language)
                        {
                            case Language.English:
                            case Language.German:
                            case Language.Dutch:
                                {
                                    if (language == Language.English || hundred != 1)
                                        result.AppendThis(Say(hundred));
                                    result.AppendThis(LoadSound("hundred"));
                                    break;
                                }
                            case Language.Spanish:
                                {
                                    switch (hundred)
                                    {
                                        case 1:
                                            {
                                                result.AppendThis(remainder == 0 ? LoadSound("100") : LoadSound("ciento"));
                                                break;
                                            }
                                        case 5:
                                            {
                                                result.AppendThis(LoadSound("500"));
                                                break;
                                            }
                                        case 7:
                                            {
                                                result.AppendThis(LoadSound("700"));
                                                break;
                                            }
                                        case 9:
                                            {
                                                result.AppendThis(LoadSound("900"));
                                                break;
                                            }
                                        default:
                                            {
                                                result.AppendThis(Say(hundred));
                                                result.AppendThis(LoadSound("hundred"));
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case Language.French:
                            case Language.Esperanto:
                                {
                                    switch (hundred)
                                    {
                                        case 1:
                                            {
                                                result.AppendThis(LoadSound("hundred"));
                                                if (remainder == 1 && language == Language.French)
                                                    result.AppendThis(LoadSound("and"));
                                                break;
                                            }
                                        default:
                                            {
                                                result.AppendThis(Say(hundred));
                                                result.AppendThis(LoadSound("hundred"));
                                                break;
                                            }
                                    }
                                    break;
                                }
                        }
                        if (remainder != 0)
                        {
                            if (language == Language.English || language == Language.German || language == Language.Dutch)
                                result.AppendThis(and);
                            result.AppendThis(Say(remainder));
                        };
                        return result;
                    }
                    switch (language)
                    {
                        case Language.Spanish:
                        case Language.French:
                        case Language.Esperanto:
                        case Language.German:
                        case Language.Dutch:
                            {
                                WholeNumber part1 = value / 1000;
                                WholeNumber part2 = value % 1000;
                                if (part1 != 1)
                                    result.AppendThis(Say(part1));

                                result.AppendThis(LoadSound("thousand"));
                                if (part2 != 0)
                                    result.AppendThis(Say(part2));
                                return result;
                            }
                    }
                }
            }
            else if (value < 10)
            {
                RomanNumeralization((int)value).ForEach(v => result.AppendThis(v == 2 ? LoadSound("d_1_0") : LoadSound("d_0_" + v)));
                return result;
            }
            WholeNumber current = 1;
            int n = 0;
            WholeNumber languageNumberScale = numberScale[language];
            for (; value >= current; n++, current *= languageNumberScale) ;
            n -= 2;
            current /= languageNumberScale;
            while (true)
            {
                bool condition = n == -1;
                WholeNumber currentVal = (value / current) % languageNumberScale;
                if (currentVal != 0)
                {
                    if (currentVal < 100 && condition && (language == Language.English || language == Language.Dutch || language == Language.German))
                        result.AppendThis(and);
                    int spanishAPart = (int)(currentVal / 1000);
                    int spanishBPart = (int)(currentVal % 1000);
                    if (language == Language.Roman_Numerals)
                    {
                        List<int> digits = RomanNumeralization((int)currentVal);
                        foreach (var item in digits)
                        {
                            int lineNumbers = (n + 1 + ((item == 2) ? 1 : 0)) / 3;
                            Sound append;
                            if (((n % 3) == 2 && item == 0) || (item == 2 && (n % 3) == 1))
                            {
                                lineNumbers--;
                                append = LoadSound("d_3_0");
                            }
                            else if (item == 2)
                                append = LoadSound("d_" + (n + 2) % 3 + "_0");
                            else
                                append = LoadSound("d_" + (n + 1) % 3 + "_" + item);
                            result.AppendThis(new Sound(new RomanNumeralsAudio(append.sound[0], lineNumbers)));
                            if (lineNumbers > 0)
                            {
                                result.AppendThis(LoadSound("with"));
                                result.AppendThis(Say(lineNumbers));
                                result.AppendThis(LoadSound(lineNumbers == 1 ? "line" : "lines"));
                            }
                        }
                    }
                    else
                        result.AppendThis((spanishBPart == 1 && !condition && language == Language.Spanish) ? (spanishAPart == 0 ? new Sound() : Say(new WholeNumber(spanishAPart * 1000))).Append(LoadSound("one")): Say(currentVal));
                    if (!condition)
                        switch (language)
                        {
                            case Language.English:
                                    result.AppendThis(LoadSound(placeValues[n]));
                                    break;
                            case Language.Spanish:
                                    result.AppendThis(LoadSound(placeValues[n + 1]));
                                    if (currentVal != 1)
                                        result.AppendThis(LoadSound("es"));
                                    break;
                            case Language.French:
                            case Language.Esperanto:
                            case Language.German:
                            case Language.Dutch:
                                result.AppendThis(LoadSound(placeValues[(n + 1) / 2]).Append(((n + 1) % 2) == 1 ? LoadSound("ard") : LoadSound("on")));
                                break;
                        }
                }
                current /= languageNumberScale;
                int valMod1000000;
                if (current == 1000 && (valMod1000000 = (int)(value % 1000000)) != 0 && language != Language.English && language != Language.Roman_Numerals)
                    return result.Append(Say(valMod1000000));
                n--;
                if (current == 0)
                    return result;
            }
        }

        private Sound SayBit(int bit)
        {
            var div = bit / 4;
            if (div == 0)
                return LoadSound("bit_" + bit);
            else
            {
                var mod = bit % 4;
                Sound sound = SayBit(mod);
                return SayBit(mod).Append(LoadSound("_start")).Append(Say(div).Append(LoadSound("_end")));
            }
        }

        /// <returns>The List: key|type, value|number of </returns>
        static List<int> RomanNumeralization (int value)
        {
            var result = new List<int>(4);
            if (value == 9)
            {
                value -= 9;
                result.Add(0);
                result.Add(2);
            }
            if (value >= 5)
            {
                value -= 5;
                result.Add(1);
            }
            if (value == 4)
            {
                result.Add(0);
                result.Add(1);
            }
            else if (value != 0)
                for (int n = 0; n < value; n++)
                    result.Add(0);
            return result;
        }

        public Sound Say (string value)
        {
            return Say(Number.Parse(value));
        }

        public Sound Say (Number value)
        {
            if (language == Language.Roman_Numerals || language == Language.Binary_Short)
            {
                if (value.pow10Div != 0)
                    throw new Exception("Decimals are invalid.");
                if (value.value <= 0)
                    throw new Exception("Negatives are invalid");
                return Say(value.value);
            }
            Sound s0s = new Sound();
            for (int n = 0; n < value.Decimal0sAtBeginningOfPartB; n++)
                s0s.AppendThis(LoadSound("0"));
            bool negative = value.value < 0 && value.PartA == 0;
            switch (language)
            {
                case Language.English:
                    {
                        WholeNumber partB = value.PartB;
                        return (negative ? LoadSound("minus") : new Sound()).Append(Say(value.PartA).Append(partB == 0 ? new Sound() : LoadSound("point").Append(s0s).Append(new Sound(Array.ConvertAll(partB.ToString().ToCharArray(), v => smalls[int.Parse(v.ToString())].sound[0])))));
                    }
                case Language.Spanish:
                case Language.French:
                case Language.German:
                case Language.Esperanto:
                case Language.Dutch:
                    {
                        WholeNumber partB = value.PartB;
                        return (negative ? LoadSound("minus") : new Sound()).Append(Say(value.PartA).Append(partB == 0 ? new Sound() : LoadSound("point").Append(s0s).Append(Say(partB))));
                    }
            }
            throw new NotImplementedException("Unhandled language: " + language.ToString());
        }

        private Sound GetEinSound(int dig2)
        {
            return dig2 == 1 ? LoadSound("1") : Say(dig2);
        }

        static readonly string[] placeValues = { "thousand", "million", "billion", "trillion", "quadrillion", "quintillion", "sextillion", "septillion", "octillion", "nonillion", "decillion", "undecillion", "duodecillion", "tredecillion", "quattuordecillion", "quindecillion", "sedecillion", "septendecillion", "octodecillion", "novendecillion", "vigintillion" };
        public enum Language
        {
            English,
            Spanish,
            French,
            Esperanto,
            German,
            Roman_Numerals,
            Binary_Short,
            Dutch
        }
    }
}
