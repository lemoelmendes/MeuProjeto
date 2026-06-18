using System.ComponentModel.DataAnnotations;

namespace MeuProjeto.ViewModels
{
    public class UsuarioEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public bool Ativo { get; set; }
    }
}