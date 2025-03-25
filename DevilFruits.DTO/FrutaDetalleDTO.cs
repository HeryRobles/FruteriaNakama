namespace DevilFruits.DTO
{
    public class FrutaDetalleDTO : FrutaDTO
    {
        public List<ResenaDTO> Resenas { get; set; }
        public int PuntajePromedio { get; set; }
    }
}
