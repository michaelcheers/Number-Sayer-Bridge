using System;
using Bridge.Html5;
using Bridge.Linq;
using System.Linq;

namespace Number_Sayer_Bridge
{
    internal class Sound
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

        public void Play()
        {
            if (sound.Length > 0)
            Play(0);
        }
        
        void Play (int index)
        {
            var audio = sound[index];
            var audioActual = audio.audio;
            if (sound.Length != ++index)
                audioActual.OnEnded = v => 
                {
                    v.Target.OnEnded = v2 => { };
                    Play(index);
                };
            audioActual.Play();
        }

        public void AppendThis (Sound sound)
        {
            this.sound = Append(sound).sound;
        }

        public Sound Append(Sound sound)
        {
            var result = new Audio[this.sound.Length + sound.sound.Length];
            this.sound.CopyTo(result, 0);
            sound.sound.CopyTo(result, this.sound.Length);
            return new Sound(result);
        }
    }

    public class Audio
    {
        public Random rnd;
        public AudioElement[] value;
        public string name;

        public Audio(AudioElement[] value, string name = "", Random rnd = null)
        {
            this.value = value;
            this.rnd = rnd == null ? new Random() : rnd;
            this.name = name;
        }

        public AudioElement audio
        {
            get
            {
                return value[rnd.Next(value.Length)];
            }
        }
    }
}