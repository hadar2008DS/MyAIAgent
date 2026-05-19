using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using MyAIAgent.DTOs;
using ViewModel;
using Model;
namespace MyAIAgent
{
    public class AILogicDB
    {
        private readonly string _connectionString;

        public AILogicDB(string connectionString)
        {
            _connectionString = connectionString;
        }

        // שליפת כל פרופילי הנגנים מהמסד ומעבר ל-DTO
        public List<MusicianProfileDTO> GetAllMusicianProfiles()
        {
            var profiles = new List<MusicianProfileDTO>();

            string sql = @"
                SELECT p.Id, p.UserName, m.IsActive, i.InstrumentName, s.Genre, s.Mood, s.BPM
                FROM (((Person AS p 
                INNER JOIN Musician AS m ON p.Id = m.Id)
                LEFT JOIN MusicianInstruments AS mi ON m.Id = mi.Id_musician)
                LEFT JOIN Instruments AS i ON mi.Id_Instrument = i.Id)
                LEFT JOIN MusicalSegments AS s ON m.Id = s.Id_musician";

            try
            {
                using (OleDbConnection conn = new OleDbConnection(_connectionString))
                {
                    using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                    {
                        conn.Open();
                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                profiles.Add(new MusicianProfileDTO
                                {
                                    Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0,
                                    UserName = reader["UserName"]?.ToString() ?? "Unknown",
                                    IsActive = reader["IsActive"] != DBNull.Value && Convert.ToBoolean(reader["IsActive"]),
                                    MainInstrument = reader["InstrumentName"]?.ToString() ?? "None",
                                    Genre = reader["Genre"]?.ToString() ?? "Unknown",
                                    Mood = reader["Mood"]?.ToString() ?? "Unknown",
                                    BPM = reader["BPM"] != DBNull.Value ? Convert.ToInt32(reader["BPM"]) : 0
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error in AILogicDB (Musicians): " + ex.Message);
            }

            return profiles;
        }

        // שליפת כל המפיקים מהמסד ומעבר ל-DTO
        public List<ProducerCollabDTO> GetProducersWithApps()
        {
            var producers = new List<ProducerCollabDTO>();

            string sql = @"
                SELECT p.UserName, a.Appname, pr.IsActive
                FROM ((Person AS p
                INNER JOIN Producer AS pr ON p.Id = pr.Id)
                INNER JOIN ProducerApps AS pa ON pr.Id = pa.Id_producer)
                INNER JOIN Apps AS a ON pa.Id_app = a.Id";

            try
            {
                using (OleDbConnection conn = new OleDbConnection(_connectionString))
                {
                    using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                    {
                        conn.Open();
                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                producers.Add(new ProducerCollabDTO
                                {
                                    ProducerName = reader["UserName"]?.ToString() ?? "Unknown",
                                    PrimaryApp = reader["Appname"]?.ToString() ?? "Unknown",
                                    IsActive = reader["IsActive"] != DBNull.Value && Convert.ToBoolean(reader["IsActive"]),
                                    TotalProjects = 0
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error in AILogicDB (Producers): " + ex.Message);
            }

            return producers;
        }
    }
}

