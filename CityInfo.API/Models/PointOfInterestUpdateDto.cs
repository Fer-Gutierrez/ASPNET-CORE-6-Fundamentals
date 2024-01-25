using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Models
{
    public class PointOfInterestUpdateDto
    {
        [Required(ErrorMessage = "La propiedad Name es obligatoria.")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200, ErrorMessage = "La propiedad Description debe tener un maximo de 200 carateres.")]
        public string? Description { get; set; }
    }
}
