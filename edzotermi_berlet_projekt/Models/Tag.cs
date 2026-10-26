using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace edzotermi_berlet_projekt.Models
{
    public class Tag
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A név megadása kötelező!")]
        [StringLength(100)]
        public string Nev { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Érvénytelen e-mail cím!")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Érvénytelen telefonszám!")]
        public string? Telefon { get; set; }

        public DateTime RegisztracioDatum { get; set; } = DateTime.Today;

        [ValidateNever]
        public ICollection<Berlet> Berletek { get; set; } = new List<Berlet>();
    }
}
