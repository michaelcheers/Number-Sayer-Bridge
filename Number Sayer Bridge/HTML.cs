using Bridge.Html5;
﻿using Bridge;
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
        private static extern HTMLInputElement Number { [Template("document.getElementById(\"number\")")] get; }

        private static extern HTMLSelectElement Voice { [Template("document.getElementById(\"voice\")")] get; }

        private static extern HTMLSelectElement Language { [Template("document.getElementById(\"language\")")] get; }

        private static extern NumberSayer.Language CurrentLanguage { [Template("document.getElementById(\"language\").selectedIndex")] get; }

        private static extern string CurrentVoice { [Template("document.getElementById(\"voice\").value")] get; }

        private static extern HTMLParagraphElement Said { [Template("document.getElementById(\"said\")")] get; }

        [Ready]
        private static void Start()
        {
            Number.OnKeyDown = (ev) =>
            {
                if (ev.IsKeyboardEvent() && ev.As<KeyboardEvent>().KeyCode == 13)
                {
                    Submit(null, () => { });
                }
            };
            new[] { Document.GetElementById("from"), Document.GetElementById("to") }.ForEach(item => item.OnKeyDown = ev =>
            {
                if (ev.IsKeyboardEvent() && ev.As<KeyboardEvent>().KeyCode == 13)
                    Count(null);
            });

            Language.OnChange = e => Update();

            Document.GetElementById<HTMLButtonElement>("submit").OnClick = (e) => Submit(e, () => { });
            Document.GetElementById<HTMLButtonElement>("count") .OnClick = Count;

            Language.InnerHTML = "";
            foreach (NumberSayer.Language item in Enum.GetValues(typeof(NumberSayer.Language)))
                Language.AppendChild(new HTMLOptionElement
                {
                    Value = item.ToString(),
                    InnerHTML = item.ToString().Replace('_', ' ')
                });
            Language.SelectedIndex = 0;

            Update();
        }

        private static void Count(MouseEvent<HTMLButtonElement> arg)
        {
            var number = Document.GetElementById<HTMLInputElement>("number");
            BigInteger to = BigInteger.Parse(Document.GetElementById<HTMLInputElement>("to").Value);
            BigInteger n = BigInteger.Parse(Document.GetElementById<HTMLInputElement>("from").Value);
                number.Value = n.ToString();
                Submit(null, () =>
                {
                    if (BigInteger.Parse(number.Value) == to)
                        return;
                    Document.GetElementById<HTMLInputElement>("from").Value = (BigInteger.Parse(number.Value) + 1).ToString();
                    Count(null);
                });
        }

        private static Dictionary<string, NumberSayer> sayers = new Dictionary<string, NumberSayer>();

        [Name(false)]
        private static NumberSayer NumberSayer
        {
            get
            { 
                string key = CurrentVoice + CurrentLanguage.ToString();

                return sayers.ContainsKey(key) ? sayers[key] : (sayers[key] = new NumberSayer(CurrentLanguage, CurrentVoice));
            }
        }

        private static void Submit(MouseEvent<HTMLButtonElement> arg, Action Callback)
        {
            Sound sound = NumberSayer.Say(BigDecimal.Parse(Document.GetElementById<HTMLInputElement>("number").Value));
            Said.InnerHTML = "";
            int bumped = 0;
            bool with = false;
            for (int n = 0; n < sound.sound.Length; n++)
            {
                var name = sound.sound[n].name;
                switch (name)
                {
                    case "es":
                    case "ty":
                    case "teen":
                        break;
                    case "with":
                        with = true;
                        break;
                    case "line":
                    case "lines":
                        with = false;
                        break;
                    default:
                        if (CurrentLanguage != NumberSayer.Language.Roman_Numerals || !name.StartsWith("d_"))
                            Said.AppendChild(new HTMLSpanElement { InnerHTML = " " });
                        break;
                }
                bool bump = false;
                if (CurrentLanguage == NumberSayer.Language.Roman_Numerals)
                {
                    if (with || name.StartsWith("line"))
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
                    Said.AppendChild(span);
                }
            }
            var indexBump = 0;
            sound.OnEnded += Callback;
            sound.Play(index => 
            {
                switch (sound.sound[index].name)
                {
                    case "with":
                        currentSayingWith = true;
                        break;
                    case "line":
                    case "lines":
                        currentSayingWith = false;
                        break;
                }
                if (currentSayingWith || sound.sound[index].name.StartsWith("line"))
                    indexBump++;
                else
                    Document.GetElementById("s" + (index - indexBump)).Style.Color = HTMLColor.Red;
            });
        }

        static bool currentSayingWith = false;

        private static void Update()
        {
            Voice.InnerHTML = "";
            string[] currentKnownVoices = NumberSayer.knownVoices[CurrentLanguage];

            foreach (string item in currentKnownVoices)
                Voice.AppendChild(new HTMLOptionElement
                {
                    Value = item.ToString(),
                    InnerHTML = item.ToString()
                });

            Voice.AppendChild(new HTMLOptionElement
            {
                Value = "mixed",
                InnerHTML = "Mixed"
            });
        }
    }
}
