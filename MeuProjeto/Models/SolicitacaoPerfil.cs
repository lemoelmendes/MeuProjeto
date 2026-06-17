using System.ComponentModel.DataAnnotations;

namespace MeuProjeto.Models
{
    public class SolicitacaoPerfil
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public Usuario? Usuario { get; set; }

        [Required]
        [StringLength(50)]
        public string PerfilSolicitado { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pendente";

        public DateTime DataSolicitacao { get; set; }
            = DateTime.Now;
    }
}