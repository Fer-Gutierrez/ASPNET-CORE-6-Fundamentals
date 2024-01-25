using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityInfo.API.Entities
{
    public class PointOfInterest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        [ForeignKey("CityId")]  //Indico que la propiedad de navegacion utiliza CityId como FK
        public City? City { get; set; } //propiedad de navegacion  en EF
        public int CityId { get; set; } //campo entero que se realacionará con City

        public PointOfInterest(string name)
        {
            Name = name;
        }
    }
}
