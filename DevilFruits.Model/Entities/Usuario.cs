using System.ComponentModel.DataAnnotations;

namespace DevilFruits.Model.Entities
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(100)]
        public string Pass { get; set; }

        public string Rol { get; set; }

        public virtual ICollection<Favorito> Favoritos { get; set; }
        public virtual ICollection<Reseña> Reseñas { get; set; }

    }
}
