using System;
using Bridge.Html5;
using Bridge.Linq;
using System.Linq;

namespace Number_Sayer_Bridge
{
    internal class Sound
    {
        private AudioElement[] sound;

        public Sound(AudioElement value)
        {
            sound = new AudioElement[] { value };
        }

        public Sound (AudioElement[] value)
        {
            sound = value;
        }

        public Sound()
        {
            sound = new AudioElement[] { };
        }

        public void Play()
        {
            if (sound.Length > 0)
            Play(0);
        }
        
        void Play (int index)
        {
            var audio = sound[index];
            if (sound.Length != ++index)
                audio.OnEnded = v => 
                {
                    v.Target.OnEnded = v2 => { };
                    Play(index);
                };
            audio.Play();
        }

        public void AppendThis (Sound sound)
        {
            this.sound = Append(sound).sound;
        }

        public Sound Append(Sound sound)
        {
            var result = new AudioElement[this.sound.Length + sound.sound.Length];
            this.sound.CopyTo(result, 0);
            sound.sound.CopyTo(result, this.sound.Length);
            return new Sound(result);
        }
    }
}