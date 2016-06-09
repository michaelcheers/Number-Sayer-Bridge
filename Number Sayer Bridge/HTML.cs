using Bridge;
using Bridge.Html5;
using Bridge.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var key = currentVoice + currentLanguage.ToString();
            NumberSayer sayer;
            
            sayer = sayers.ContainsKey(key) ? sayers[key] : (sayers[key] = new NumberSayer(currentLanguage, currentVoice));

            var sound = sayer.Say(new BigInteger(Document.GetElementById<HTMLInputElement>("number").Value));
            sound.Play();
            said.InnerHTML = Array.ConvertAll(sound.sound, v => v.name).Join(" ").Replace(" es", "es").Replace(" ty", "ty").Replace(" teen", "teen");
        }

        private static void Update()
        {
            voice.InnerHTML = "";
            var currentKnownVoices = NumberSayer.knownVoices[currentLanguage];

            foreach (var item in currentKnownVoices)
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