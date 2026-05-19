using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAIAgent.DTOs
{
    public class MusicianProfileDTO : BaseDTO 
    {
        // Simple data from Person and Musician
        public string UserName { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        // constracted data from Instruments and MusicalSegments
        public string MainInstrument { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string Mood { get; set; }
        public int BPM { get; set; }

        public override string ToString()
        {
            return $"[DTO] ID: {Id} | Name: {UserName} | Active: {IsActive} | Instrument: {MainInstrument} | Genre: {Genre} | BPM: {BPM}";
        }
    }
}
