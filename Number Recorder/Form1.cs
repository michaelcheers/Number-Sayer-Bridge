using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
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
            @this = Assembly.GetExecutingAssembly();
            Stream voiceDataStream = @this.GetManifestResourceStream("Number_Recorder.voice data.txt");
            StreamReader reader = new StreamReader(voiceDataStream);
            voiceData = reader.ReadToEnd().Split('\n');
            reader.Close();
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
            { Language.English, "Sylvia" },
            { Language.Spanish, "Ana" },
            { Language.French, "Ben" },
            { Language.German, "Laurie" },
            { Language.Esperanto, "Michael" }
        };

        Dictionary<Language, string> cultures = new Dictionary<Language, string>
        {
            { Language.English,   "en-US" },
            { Language.Spanish,   "es-ES" },
            { Language.French,    "fr-FR" },
            { Language.German,    "de-DE" },
            { Language.Esperanto, "eo"    }
        };
        string[] needToBeRecorded;
        string[] actualNames;
        SpeechRecognitionEngine recognizer;
        string[] voiceData;
        Assembly @this;
        int nRecord = -1;

        private void language_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!started)
            {
                started = true;
                currentLanguage = (Language)language.SelectedIndex;
                actualNames = Array.ConvertAll(Directory.GetFiles(currentLanguage + "/" + exampleVoices[currentLanguage]), v => Path.GetFileNameWithoutExtension(v));
                needToBeRecorded = Array.ConvertAll(actualNames, v =>
                {
                    bool current = false;
                    foreach (var line in voiceData)
                    {
                        if (current)
                        {
                            if (line.EndsWith(";"))
                                return v;
                            var split = line.Split('|');
                            if (split[0] == v)
                                return split[1];
                        }
                        else if (line.EndsWith(";") && line.Substring(0, line.Length - 1) == currentLanguage.ToString())
                            current = true;
                    }
                    return v;
                });
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
                Stream stream = File.Open("Voice/" + actualNames[nRecord] + ".wav", FileMode.Create);
                e.Result.Audio.WriteToWaveStream(stream);
                stream.Close();
                Next();
            }
        }
    }
}
