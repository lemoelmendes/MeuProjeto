using System.ComponentModel.DataAnnotations;

namespace MeuProjeto.ViewModels
{
    public class UsuarioCreateViewModel
    {
        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string Senha { get; set; } = string.Empty;
    }
}