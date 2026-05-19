using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace MyAIAgent.DTOs
{
    public abstract class BaseDTO
    {
        public int Id { get; set; }
        public DateTime GeneratedAt { get; set; } = DateTime.Now;

        public abstract override string ToString();
    }
}
