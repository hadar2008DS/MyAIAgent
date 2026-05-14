using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAIAgent.DTOs
{
    public class ProducerCollabDTO
    {
        public string ProducerName { get; set; }
        public string PrimaryApp { get; set; } 
        public bool IsActive { get; set; }
        public int TotalProjects { get; set; } 
    }
}
