using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAIAgent.DTOs
{
    public class MusicianProfileDTO
    {
        // Simple data from Person and Musician
        public string UserName { get; set; }
        public bool IsActive { get; set; }

        // constracted data from Instruments and MusicalSegments
        public string MainInstrument { get; set; }
        public string Genre { get; set; }
        public string Mood { get; set; }
        public int BPM { get; set; }
    }
}
