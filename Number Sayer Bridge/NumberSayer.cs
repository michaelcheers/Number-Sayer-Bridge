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

        public NumberSayer(Language language = Language.English, string voice = "Michael")
        {
            this.language = language;
            Voice = voice;
            switch (language)
            {
                case Language.Esperanto:
                    {
                        smalls = new Sound[]
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
                        smalls = new Sound[]
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
                        smalls = new Sound[]
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
                        smalls = new Sound[]
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
                default:
                    break;
            }
        }

        public Sound thir { get { return LoadSound("thir"); } }
        public Sound fif { get { return LoadSound("fif"); } }
        public Sound and { get { return LoadSound("and"); } }
        public Sound ty { get { return LoadSound("ty"); } }
        public readonly Random rnd = new Random();
        public static readonly Dictionary<Language, string[]> knownVoices = new Dictionary<Language, string[]>
        {
            {Language.English, new[] {"Ally", "Ben", "Jeff", "Laurie", "Melissa", "Michael", "Seamus"} },
            {Language.Spanish, new[] {"Ana", "Sylvia"} },
            {Language.French,  new[] {"Ben"} },
            {Language.Esperanto, new[] {"Michael"} }
        };

        public static readonly Dictionary<Language, int> irregularStarters = new Dictionary<Language, int>
        {
            {Language.English, 13 },
            {Language.Spanish, 16 },
            {Language.French, 17 },
            {Language.Esperanto, 10 }
        };

        [InlineConst]
        public const int shortNumberScale = 1000   ;
        [InlineConst]
        public const int  longNumberScale = 1000000;

        public static readonly Dictionary<Language, int> numberScale = new Dictionary<Language, int>
        {
            {Language.English, shortNumberScale },
            {Language.French,  shortNumberScale },
            {Language.Spanish,  longNumberScale },
            {Language.Esperanto, shortNumberScale }
        };

        public Dictionary<string, Sound> alreadyDone = new Dictionary<string, Sound>();

        public Sound LoadSound(string value)
        {
            if (alreadyDone.ContainsKey(value))
                return alreadyDone[value];
            AudioElement[] mixedResult = new AudioElement[] { };
            string format = "Sounds/" + language.ToString() + "/{0}/{1}.wav";
            try
            {
                if (Voice == "mixed")
                    foreach (var item in knownVoices[language])
                        mixedResult.Push(new AudioElement(string.Format(format, item, value)));
                else
                    mixedResult.Push(new AudioElement(string.Format(format, Voice, value)));
            }
            catch (KeyNotFoundException e)
            {
                mixedResult.Push(new AudioElement(string.Format(format, "", "")));
            }
            return (alreadyDone[value] = new Sound(new Audio(mixedResult, value, rnd)));
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
                                    {
                                        result.AppendThis(GetThirFifSound(value % 10));
                                        result.AppendThis(LoadSound("teen"));
                                        return result;
                                    }
                                default:
                                    break;
                            }
                        }
                        var dig1 = value / 10;
                        var dig2 = value % 10;
                        switch (language)
                        {
                            case Language.English:
                                {
                                    if (dig1 == 2)
                                        result.AppendThis(LoadSound("20"));
                                    else
                                    {
                                        result.AppendThis(GetThirFifSound(dig1));
                                        result.AppendThis(ty);
                                    }
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
                                    var dig220 = value % 20;
                                    switch (dig120)
                                    {
                                        case 3:
                                        case 4:
                                            {
                                                result.AppendThis(LoadSound(dig120 * 2 + "0"));
                                                if (dig120 == 3 && dig220 == 1)
                                                    result.AppendThis(and);
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
                            default:
                                break;
                        }
                        if (dig2 != 0)
                            result.AppendThis(Say(dig2));
                        return result;
                    }

                    int hundred = (int)(value / 100);
                    var remainder = value % 100;
                    switch (language)
                    {
                        case Language.English:
                            {
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
                                            if (remainder == 0)
                                                result.AppendThis(LoadSound("100"));
                                            else
                                                result.AppendThis(LoadSound("ciento"));
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
                        default:
                            break;
                    }
                    if (remainder != 0)
                    {
                        if (language == Language.English)
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
                        {
                            var part1 = value / 1000;
                            var part2 = value % 1000;
                            if (part1 != 1)
                                result.AppendThis(Say(part1));
                            result.AppendThis(LoadSound("thousand"));
                            if (part2 != 0)
                                result.AppendThis(Say(part2));
                            return result;
                        }
                    default:
                        break;
                }
            }
            Number current = 1;
            int n = 0;
            Number languageNumberScale = numberScale[language];
            for (; value >= current; n++, current *= languageNumberScale) ;
            n -= 2;
            current /= languageNumberScale;
            while (true)
            {
                var condition = n == -1;
                var currentVal = (value / current) % languageNumberScale;
                if (currentVal != 0)
                {
                    if (currentVal < 100 && condition && language == Language.English)
                        result.AppendThis(and);
                    if (currentVal != 1 || language == Language.English)
                        result.AppendThis((currentVal == 1 && language == Language.Spanish) ? LoadSound("one"): Say(currentVal));
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
                                    result.AppendThis(LoadSound(placeValues[(n + 1) / 2]).Append(((n + 1) % 2) == 1 ? LoadSound("ard") : LoadSound("on")));
                                    break;
                            default:
                                break;
                        }
                }
                current /= languageNumberScale;
                if (current == 1000 && language != Language.English)
                    return result.Append(Say(value % 1000000));
                n--;
                if (current == 0)
                    return result;
            }
        }

        static string[] placeValues = new string[] { "thousand", "million", "billion", "trillion", "quadrillion", "quintillion", "sextillion", "septillion", "octillion", "nonillion", "decillion", "undecillion", "duodecillion", "tredecillion", "quattuordecillion", "quindecillion", "sedecillion", "septendecillion", "octodecillion", "novendecillion", "vigintillion" };
        public enum Language
        {
            English,
            Spanish,
            French,
            Esperanto
        }
    }
}
