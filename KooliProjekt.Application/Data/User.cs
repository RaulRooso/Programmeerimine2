using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = "";
        public string? Email { get; set; }

        public List<BatchLog> BatchLogs { get; set; } = new();
        public List<TasteLog> TasteLogs { get; set; } = new();
    }
}
