Audio and ﻿using Bridge;
using Bridge.Html5;
using Bridge.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Number_Sayer_Bridge
{
    [FileName("html.js")]
    internal static class HTML
    {
        private static extern HTMLInputElement number { [Template("document.getElementById(\"number\")")] get; }

        private static extern HTMLSelectElement voice { [Template("document.getElementById(\"voice\")")] get; }

        private static extern HTMLSelectElement language { [Template("document.getElementById(\"language\")")] get; }

        private static extern NumberSayer.Language currentLanguage { [Template("document.getElementById(\"language\").selectedIndex")] get; }

        private static extern string currentVoice { [Template("document.getElementById(\"voice\").value")] get; }

        private static extern HTMLParagraphElement said { [Template("document.getElementById(\"said\")")] get; }

        [Ready]
        private static void Start()
        {
            number.OnKeyDown = (ev) =>
            {
                if (ev.IsKeyboardEvent() && ev.As<KeyboardEvent>().KeyCode == 13)
                {
                    Submit(null);
                }
            };
            new[] { Document.GetElementById("from"), Document.GetElementById("to") }.ForEach(item => item.OnKeyDown = ev =>
            {
                if (ev.IsKeyboardEvent() && ev.As<KeyboardEvent>().KeyCode == 13)
                    Count(null);
            });

            language.OnChange = e => Update();

            Document.GetElementById<HTMLButtonElement>("submit").OnClick = Submit;
            Document.GetElementById<HTMLButtonElement>("count") .OnClick = Count;

            language.InnerHTML = "";
            foreach (NumberSayer.Language item in Enum.GetValues(typeof(NumberSayer.Language)))
                language.AppendChild(new HTMLOptionElement
                {
                    Value = item.ToString(),
                    InnerHTML = item.ToString().Replace('_', ' ')
                });
            language.SelectedIndex = 0;

            Update();
        }

        private static void Count(MouseEvent<HTMLButtonElement> arg)
        {
            var number = Document.GetElementById<HTMLInputElement>("number");
            BigInteger to = BigInteger.Parse(Document.GetElementById<HTMLInputElement>("to").Value);
            for (BigInteger n = BigInteger.Parse(Document.GetElementById<HTMLInputElement>("from").Value; n <= to; n++)
            {
                number.Value = n;
                Submit(null);
            }
        }

        private static Dictionary<string, NumberSayer> sayers = new Dictionary<string, NumberSayer>();

        [Name(false)]
        private static NumberSayer NumberSayer
        {
            get
            {Audio and 
                string key = currentVoice + currentLanguage.ToString();

                return sayers.ContainsKey(key) ? sayers[key] : (sayers[key] = new NumberSayer(currentLanguage, currentVoice));
            }
        }

        private static void Submit(MouseEvent<HTMLButtonElement> arg)
        {
            Sound sound = NumberSayer.Say(BigDecimal.Parse(Document.GetElementById<HTMLInputElement>("number").Value));
            said.InnerHTML = "";
            int bumped = 0;
            for (int n = 0; n < sound.sound.Length; n++)
            {
                var name = sound.sound[n].name;
                switch (name)
                {
                    case "es":
                    case "ty":
                    case "teen":
                        break;
                    default:
                        if (currentLanguage != NumberSayer.Language.Roman_Numerals || !name.StartsWith("d_"))
                            said.AppendChild(new HTMLSpanElement { InnerHTML = " " });
                        break;
                }
                bool bump = false;
                if (currentLanguage == NumberSayer.Language.Roman_Numerals)
                {
                    if (!(sound.sound[n] is RomanNumeralsAudio))
                        bump = true;
                    else
                        name = name.Replace("d_0_0", "I").Replace("d_0_1", "V").Replace("d_1_0", "X").Replace("d_1_1", "L").Replace("d_2_0", "C").Replace("d_2_1", "D").Replace("d_3_0", "M");
                }
                if (bump)
                    bumped++;
                else
                {
                    var span = new HTMLSpanElement { InnerHTML = name };
                    if (sound.sound[n] is RomanNumeralsAudio)
                        for (int idx = 0; idx < sound.sound[n].As<RomanNumeralsAudio>().lineNumbers; idx++)
                        {
                            var oldSpan = span;
                            span = new HTMLSpanElement();
                            span.Style.BorderTop = "1px solid black";
                            span.Style.MarginTop = "1px";
                            span.Style.Display = Display.InlineBlock;
                            span.AppendChild(oldSpan);
                        }
                    span.Id = "s" + (n - bumped);
                    said.AppendChild(span);
                }
            }
            var indexBump = 0;
            sound.Play(index => 
            {
                if (sound.sound[index] is RomanNumeralsAudio || currentLanguage != NumberSayer.Language.Roman_Numerals)
                    Document.GetElementById("s" + (index - indexBump)).Style.Color = HTMLColor.Red;
                else
                    indexBump++;
            });
        }

        private static void Update()
        {
            voice.InnerHTML = "";
            string[] currentKnownVoices = NumberSayer.knownVoices[currentLanguage];

            foreach (string item in currentKnownVoices)
                voice.AppendChild(new HTMLOptionElement
                {
                    Value = item.ToString(),
                    InnerHTML = item.ToString()
                });

            voice.AppendChild(new HTMLOptionElement
            {
                Value = "mixed",
                InnerHTML = "Mixed"
            });
        }
    }
}
