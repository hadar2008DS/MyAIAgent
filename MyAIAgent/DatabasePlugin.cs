using Microsoft.SemanticKernel;
using MyAIAgent.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAIAgent
{
    public class DatabasePlugin
    {
        private readonly string _connectionString;

        public DatabasePlugin(string connectionString)
        {
            _connectionString = connectionString;
        }

        [KernelFunction("get_all_musician_profiles")]
        [Description("שולף פרופילים מלאים של נגנים כולל כלי נגינה ופרטי סגמנטים מוזיקליים")]
        public List<MusicianProfileDTO> GetAllMusicianProfiles()
        {
            var profiles = new List<MusicianProfileDTO>();

            using (OleDbConnection conn = new OleDbConnection(_connectionString))
            {
                string sql = @"
                    SELECT p.UserName, m.IsActive, i.InstrumentName, s.Genre, s.Mood, s.BPM
                    FROM (((Person AS p 
                    INNER JOIN Musician AS m ON p.Id = m.Id)
                    INNER JOIN MusicianInstruments AS mi ON m.Id = mi.Id_musician)
                    INNER JOIN Instruments AS i ON mi.Id_Instrument = i.Id)
                    LEFT JOIN MusicalSegments AS s ON m.Id = s.Id_musician";

                OleDbCommand cmd = new OleDbCommand(sql, conn);
                conn.Open();

                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        profiles.Add(new MusicianProfileDTO
                        {
                            UserName = reader["UserName"].ToString(),
                            IsActive = (bool)reader["IsActive"],
                            MainInstrument = reader["InstrumentName"].ToString(),
                            Genre = reader["Genre"]?.ToString() ?? "Unknown",
                            Mood = reader["Mood"]?.ToString() ?? "Unknown",
                            BPM = reader["BPM"] != DBNull.Value ? Convert.ToInt32(reader["BPM"]) : 0
                        });
                    }
                }
            }
            return profiles;
        }

        [KernelFunction("get_producers_with_apps")]
        [Description("שולף רשימת מפיקים והאפליקציות שהם משתמשים בהן")]
        public List<ProducerCollabDTO> GetProducers()
        {
            var producers = new List<ProducerCollabDTO>();

            using (OleDbConnection conn = new OleDbConnection(_connectionString))
            {
                string sql = @"
                    SELECT p.UserName, a.Appname, pr.IsActive
                    FROM ((Person AS p
                    INNER JOIN Producer AS pr ON p.Id = pr.Id)
                    INNER JOIN ProducerApps AS pa ON pr.Id = pa.Id_producer)
                    INNER JOIN Apps AS a ON pa.Id_app = a.Id";

                OleDbCommand cmd = new OleDbCommand(sql, conn);
                conn.Open();

                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        producers.Add(new ProducerCollabDTO
                        {
                            ProducerName = reader["UserName"].ToString(),
                            PrimaryApp = reader["Appname"].ToString(),
                            IsActive = (bool)reader["IsActive"]
                        });
                    }
                }
            }
            return producers;
        }
    }
}

