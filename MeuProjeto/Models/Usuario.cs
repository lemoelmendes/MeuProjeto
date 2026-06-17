using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MeuProjeto.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string SenhaHash { get; set; } = string.Empty;

        public bool Ativo { get; set; } = true;

        public DateTime DataCadastro { get; set; }
            = DateTime.Now;

        public int PerfilId { get; set; }

        public Perfil? Perfil { get; set; }

        public ICollection<SolicitacaoPerfil> Solicitacoes { get; set; }
            = new List<SolicitacaoPerfil>();
    }
}