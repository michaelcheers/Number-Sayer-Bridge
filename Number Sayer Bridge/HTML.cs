using Bridge;
using Bridge.Html5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Number_Sayer_Bridge
{
    [FileName("html.js")]
    static class HTML
    {
        static SelectElement voice { get { return Document.GetElementById<SelectElement>("voice"); } }
        static SelectElement language { get { return Document.GetElementById<SelectElement>("language"); } }
        static NumberSayer.Language currentLanguage { get{ return (NumberSayer.Language)language.SelectedIndex; } }
        static string currentVoice { get { return voice.Value; } }

        [Ready]
        static void Start ()
        {
            Document.GetElementById<ButtonElement>("submit").OnClick = Submit;
            foreach (NumberSayer.Language item in Enum.GetValues(typeof(NumberSayer.Language)))
                language.AppendChild(new OptionElement
                {
                    Value = item.ToString(),
                    InnerHTML = item.ToString()
                });
            language.SelectedIndex = 0;
            Update();
        }

        static Dictionary<string, NumberSayer> sayers = new Dictionary<string, NumberSayer>();

        static void Submit(MouseEvent<ButtonElement> arg)
        {
            var key = currentVoice + currentLanguage.ToString();
            NumberSayer sayer;
            if (sayers.ContainsKey(key))
                sayer = sayers[key];
            else
                sayer = (sayers[key] = new NumberSayer(currentLanguage, currentVoice));
            sayer.Say(new BigInteger(Document.GetElementById<InputElement>("number").Value)).Play();
        }

        static void Update ()
        {
            voice.InnerHTML = "";
            var currentKnownVoices = NumberSayer.knownVoices[(NumberSayer.Language)language.SelectedIndex];
            foreach (var item in currentKnownVoices)
                voice.AppendChild(new OptionElement
                {
                    Value = item.ToString(),
                    InnerHTML = item.ToString()
                });
            voice.AppendChild(new OptionElement
            {
                Value = "mixed",
                InnerHTML = "Mixed"
            });
        }
    }
}
