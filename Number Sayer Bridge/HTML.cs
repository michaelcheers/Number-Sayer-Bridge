using Bridge;
using Bridge.Html5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bridge.Linq;

namespace Number_Sayer_Bridge
{
    [FileName("html.js")]
    static class HTML
    {
        static SelectElement voice { get { return Document.GetElementById<SelectElement>("voice"); } }
        static SelectElement language { get { return Document.GetElementById<SelectElement>("language"); } }
        static NumberSayer.Language currentLanguage { get{ return (NumberSayer.Language)language.SelectedIndex; } }
        static string currentVoice { get { return voice.Value; } }
        static ParagraphElement said { get { return Document.GetElementById<ParagraphElement>("said"); } }

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
            var sound = sayer.Say(new BigInteger(Document.GetElementById<InputElement>("number").Value));
            sound.Play();
            List<string> saidString = new List<string>();
            sound.sound.ForEach(v => saidString.Add(v.name));
            said.InnerHTML = saidString.Join(" ").Replace(" es", "es").Replace(" ty", "ty").Replace(" teen", "teen");
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
