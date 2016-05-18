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
        private static InputElement number
        { get { return Document.GetElementById<InputElement>("number"); } }

        private static SelectElement voice
        { get { return Document.GetElementById<SelectElement>("voice"); } }

        private static SelectElement language
        { get { return Document.GetElementById<SelectElement>("language"); } }

        private static NumberSayer.Language currentLanguage
        { get { return (NumberSayer.Language)language.SelectedIndex; } }

        private static string currentVoice
        { get { return voice.Value; } }

        private static ParagraphElement said
        { get { return Document.GetElementById<ParagraphElement>("said"); } }

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

            language.OnChange = (ev) =>
            {
                Update();
            };

            Document.GetElementById<ButtonElement>("submit").OnClick = Submit;

            language.InnerHTML = "";
            foreach (NumberSayer.Language item in Enum.GetValues(typeof(NumberSayer.Language)))
                language.AppendChild(new OptionElement
                {
                    Value = item.ToString(),
                    InnerHTML = item.ToString()
                });
            language.SelectedIndex = 0;

            Update();
        }

        private static Dictionary<string, NumberSayer> sayers = new Dictionary<string, NumberSayer>();

        private static void Submit(MouseEvent<ButtonElement> arg)
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

        private static void Update()
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