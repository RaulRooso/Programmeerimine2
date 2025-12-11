using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Application.Data
{
    public class User : Entity
    {
        [Required, StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [EmailAddress, StringLength(100)]
        public string? Email { get; set; }

        public List<BatchLog> BatchLogs { get; set; } = new();
        public List<TasteLog> TasteLogs { get; set; } = new();
    }
}
