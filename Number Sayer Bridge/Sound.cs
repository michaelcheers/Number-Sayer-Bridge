using System;
using Bridge.Html5;
using Bridge.Linq;
using System.Linq;

namespace Number_Sayer_Bridge
{
    public class Sound
    {
        internal Audio[] sound;

        public Sound(Audio value)
        {
            sound = new Audio[] { value };
        }

        public Sound (Audio[] value)
        {
            sound = value;
        }

        public Sound()
        {
            sound = new Audio[] { };
        }

        public void Play ()
        {
            Play(v => { });
        }

        public void Play(Action<int> callStart)
        {
            if (sound.Length > 0) Play(0, callStart);
        }
        
        void Play (int index, Action<int> callStart)
        {
            callStart(index);
            Audio audio = sound[index];
            HTMLAudioElement audioActual = audio.audio;
            if (sound.Length != ++index)
                audioActual.OnEnded = v => 
                {
                    v.Target.OnEnded = v2 => { };
                    Play(index, callStart);
                };
            audioActual.Play();
        }

        public void AppendThis (Sound sound)
        {
            this.sound = Append(sound).sound;
        }

        public Sound Append(Sound sound)
        {
            Audio[] result = new Audio[this.sound.Length + sound.sound.Length];
            this.sound.CopyTo(result, 0);
            sound.sound.CopyTo(result, this.sound.Length);
            return new Sound(result);
        }
    }

    public class Audio
    {
        public Random rnd;
        public HTMLAudioElement[] value;
        public string name;

        public Audio(HTMLAudioElement[] value, string name = "", Random rnd = null)
        {
            this.value = value;
            this.rnd = rnd ?? new Random();
            this.name = name;
        }

        public HTMLAudioElement audio
        {
            get
            {
                if (value.Length == 0)
                {
                    string error = "No valid audio for "+name+".";
                    Global.Alert(error);
                    throw new Exception(error);
                }
                return value[rnd.Next(value.Length)];
            }
        }
    }
}