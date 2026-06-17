using System.ComponentModel.DataAnnotations;

namespace MeuProjeto.Models
{
    public class Perfil
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nome { get; set; } = string.Empty;

        public ICollection<Usuario> Usuarios { get; set; }
            = new List<Usuario>();
    }
}