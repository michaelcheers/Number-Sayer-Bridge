using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Number_Recorder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Directory.CreateDirectory("Voice");
            language.Items.AddRange(Enum.GetNames(typeof(Language)));
        }
        

        public void Next ()
        {
            if (++nRecord == needToBeRecorded.Length)
            {
                MessageBox.Show("Recording complete.");
                Environment.Exit(0);
            }
            toSay.Text = needToBeRecorded[nRecord];
        }

        public enum Language
        {
            English,
            Spanish,
            French,
            Esperanto,
            German
        }

        bool started;
        Language currentLanguage;
        Dictionary<Language, string> exampleVoices = new Dictionary<Language, string>
        {
            { Language.English, "Seamus" },
            { Language.Spanish, "Ana" },
            { Language.French, "Ben" },
            { Language.German, "Laurie" },
            { Language.Esperanto, "Michael" }
        };

        Dictionary<Language, int> cultures = new Dictionary<Language, int>
        {
            { Language.English,   0x0409 },
            { Language.Spanish,   0x0C0A },
            { Language.French,    0x040C },
            { Language.German,    0x0407 },
            { Language.Esperanto, 4096 }
        };
        string[] needToBeRecorded;
        SpeechRecognitionEngine recognizer;
        int nRecord = -1;

        private void language_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!started)
            {
                started = true;
                currentLanguage = (Language)language.SelectedIndex;
                needToBeRecorded = Array.ConvertAll(Directory.GetFiles(currentLanguage + "/" + exampleVoices[currentLanguage]), v => Path.GetFileNameWithoutExtension(v));
                GrammarBuilder gb = new GrammarBuilder();
                gb.Append(new Choices(needToBeRecorded));
                Grammar grammar = new Grammar(gb);
                recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo(cultures[currentLanguage]));
                recognizer.SetInputToDefaultAudioDevice();
                recognizer.LoadGrammar(grammar);
                recognizer.SpeechRecognized += SpeechRecognized;
                recognizer.RecognizeAsync(RecognizeMode.Multiple);
                Next();
            }
            else
            {
                MessageBox.Show("Invalid!");
                language.SelectedIndex = (int)currentLanguage;
            }
        }

        private void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text == needToBeRecorded[nRecord])
            {
                Stream stream = File.Open("Voice/" + needToBeRecorded[nRecord] + ".wav", FileMode.Create);
                e.Result.Audio.WriteToWaveStream(stream);
                stream.Close();
                Next();
            }
        }
    }
}
