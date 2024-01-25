using CityInfo.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityInfo.API.Entities
{
    public class City
    {
        [Key] //Con esta DataAnnotations indicamos que es PK.
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //Con este atributo indicamos que es PK para la DB.
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } //= string.Empty;

        [MaxLength(200)]
        public string? Description { get; set; }

        public ICollection<PointOfInterest> PointsOfInterest { get; set; } = new List<PointOfInterest>();


        //Ctor para siempre asignarle un nombre a la City
        public City(string name)
        {
            Name = name;
        }
    }
}
