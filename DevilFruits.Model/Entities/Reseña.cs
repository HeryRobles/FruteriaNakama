using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevilFruits.Model.Entities
{
    public class Reseña
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public int DevilFruitId {  get; set; }
        public string Comentario { get; set; }

        [Range(1, 5)]
        public int Puntaje { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

    }
}
