using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Number_Sayer_Bridge
{
    class RomanNumeralsAudio : Audio
    {
        public int lineNumbers;
        
        public RomanNumeralsAudio (Audio value, int lineNumbers) : base(value.value, value.name, value.rnd)
        {
            this.lineNumbers = lineNumbers;
        }
    }
}
