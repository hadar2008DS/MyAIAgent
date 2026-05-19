using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using MyAIAgent.DTOs;
using ClientSide;

namespace MyAIAgent
{
    public class MatcherService
    {
        private ApiService api = new ApiService();
        private readonly DatabasePlugin _db;

        public MatcherService(DatabasePlugin db)
        {
            _db = db;
        }

        // Top-N matches for a single musician (prototype uses aggregated DTO lists)
        public async Task<List<MatchResultDTO>> GetMatchesForMusician(int musicianId, int topN = 5)
        {
            var musicians = await api.GetMusicians(); // Ensure correct type
            Musician musician = musicians.FirstOrDefault(m => m.Id == musicianId);
            if (musician == null) return new List<MatchResultDTO>();


            var producers = await api.GetProducers(); // prototype method

            // For a robust solution we need per-producer apps and per-musician multiple instruments/segments.
            // This prototype assumes DTOs contain at least one instrument, genre, bpm and producer primary app.

            var results = new List<MatchResultDTO>();

            foreach (var pr in producers) //was done in _db.GetProducers(); .Where(p => p.IsActive)
            {
                if (!musician.IsActive) break; // or continue depending on rule

                double score = 0;
                var matched = new List<string>();

                // Instrument match - weight 30 (prototype: exact instrument name match if available)
                // FIX: Use a property that exists on Producer, e.g., Username or another available property.
                if (!string.IsNullOrWhiteSpace(musician.Username) &&
                    !string.IsNullOrWhiteSpace(pr.Username) && // Changed from pr.PrimaryApp to pr.Username
                    musician.Username.Equals(pr.Username, StringComparison.OrdinalIgnoreCase)) // Changed from pr.PrimaryApp to pr.Username
                {
                    score += 30;
                    matched.Add("Instrument");
                }

                if (score <= 0) continue;

                results.Add(new MatchResultDTO
                {
                    MusicianId = musician.Id,
                    /*ProducerId = pr.TotalProjects,*/ //Calc count of project using linked groups
                    MusicianName = musician.Username,
                    ProducerName = pr.Username,
                    Score = score,
                    MatchedCriteria = matched,
                });
            }

            return results.OrderByDescending(r => r.Score).Take(topN).ToList();
        }
    }
}