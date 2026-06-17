using System.ComponentModel.DataAnnotations;

namespace MeuProjeto.ViewModels
{
    public class RegistroViewModel
    {
        [Required(ErrorMessage = "Informe o nome")]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe o e-mail")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe a senha")]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string Senha { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirme a senha")]
        [Compare("Senha", ErrorMessage = "As senhas não conferem")]
        [DataType(DataType.Password)]
        public string ConfirmarSenha { get; set; } = string.Empty;
    }
}