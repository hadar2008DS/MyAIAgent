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
        public string GetAllMusicianProfiles()
        {
            try
            {
                var db = new AILogicDB(_connectionString);
                var profiles = db.GetAllMusicianProfiles();

                if (profiles == null || profiles.Count == 0)
                    return "No musicians found in the database.";

                // הפיכת רשימת ה-DTOs למחרוזת טקסט אחת ארוכה שה-AI מבין בקלות!
                return string.Join("\n", profiles.Select(p => p.ToString()));
            }
            catch (Exception ex)
            {
                return $"Error retrieving musicians: {ex.Message}";
            }
        }

        [KernelFunction("get_producers_with_apps")]
        [Description("שולף רשימת מפיקים והאפליקציות שהם משתמשים בהן")]
        public string GetProducers()
        {
            try
            {
                var db = new AILogicDB(_connectionString);
                var producers = db.GetProducersWithApps();

                if (producers == null || producers.Count == 0)
                    return "No producers found in the database.";

                // המרה ידנית לפורמט טקסט ברור עבור ה-AI עבור ה-DTO שאין לו ToString מובנה
                var lines = producers.Select(p => $"Producer: {p.ProducerName} | App: {p.PrimaryApp} | Active: {p.IsActive}");
                return string.Join("\n", lines);
            }
            catch (Exception ex)
            {
                return $"Error retrieving producers: {ex.Message}";
            }
        }
    }
}


