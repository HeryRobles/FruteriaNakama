using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevilFruits.Model.Entities
{
    public class Favorito
    {
        public int UsuarioId { get; set; }
        public Usuario Usuario {  get; set; }

        public int DevilFruitId { get; set; }
    }
}
