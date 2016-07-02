using Bridge;
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

            language.OnChange = (ev) => Update();

            Document.GetElementById<HTMLButtonElement>("submit").OnClick = Submit;

            language.InnerHTML = "";
            foreach (NumberSayer.Language item in Enum.GetValues(typeof(NumberSayer.Language)))
                language.AppendChild(new HTMLOptionElement
                {
                    Value = item.ToString(),
                    InnerHTML = item.ToString()
                });
            language.SelectedIndex = 0;

            Update();
        }

        private static Dictionary<string, NumberSayer> sayers = new Dictionary<string, NumberSayer>();

        private static void Submit(MouseEvent<HTMLButtonElement> arg)
        {
            string key = currentVoice + currentLanguage.ToString();
            NumberSayer sayer;
            
            sayer = sayers.ContainsKey(key) ? sayers[key] : (sayers[key] = new NumberSayer(currentLanguage, currentVoice));

            Sound sound = sayer.Say(BigInteger.Parse(Document.GetElementById<HTMLInputElement>("number").Value));
            said.InnerHTML = "";
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
                        said.AppendChild(new HTMLSpanElement { InnerHTML = " " });
                        break;
                }
                said.AppendChild(new HTMLSpanElement { Id = "s" + n, InnerHTML = name });
            }
            sound.Play(index => Document.GetElementById("s" + index).Style.Color = HTMLColor.Red);
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