using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevilFruits.ExternalServiceAPI.Model
{
    public class FrutaDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string RomanName { get; set; }
        public string Type { get; set; }
        public string ImagenUrl { get; set; }
    }
}
