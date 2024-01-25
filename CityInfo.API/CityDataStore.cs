using CityInfo.API.Models;

namespace CityInfo.API
{
    public class CityDataStore
    {
        public List<CityDto> Cities { get; set; }
        //public static CityDataStore Current { get;} = new CityDataStore(); // Esto es para utilizar Sigleton Pathern. Quitamos para usar inyeccion de dependencias

        public CityDataStore() 
        {
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id = 1,
                    Name = "Rafaela",
                    Description = "La perla del oeste",
                    PointsOfInterest = new List<PointsOfInterestDto>()
                    {
                        new PointsOfInterestDto()
                        {
                            Id= 1,
                            Name= "Autodromo de Rafaela",
                            Description = "Autodromo en forma de ovalo donde se corre el TC"
                        },
                        new PointsOfInterestDto()
                        {
                            Id= 2,
                            Name="Estadio Nuevo Monumental AR",
                            Description= "Estadio del equipo Atletico de Rafaela que juega el Nacional B"
                        }
                    }
                },
                new CityDto()
                {
                    Id = 2,
                    Name = "Sunchales",
                    Description = "La capital del cooperativismo",
                    PointsOfInterest = new List<PointsOfInterestDto>()
                    {
                        new PointsOfInterestDto()
                        {
                            Id= 3,
                            Name= "Club Atletico Libertad de Sunchales",
                            Description = "Club que juego el TNA de Basquet"
                        },
                        new PointsOfInterestDto()
                        {
                            Id= 4,
                            Name = "Edificio Corporativo de Sancor Seguros",
                            Description = "Edificio sobresaliente y deslumbrante donde se encuentra la aseguradora numero uno del país"
                        },
                        new PointsOfInterestDto()
                        {
                            Id= 5,
                            Name="Monumento a la cosechadora",
                            Description = "Monumento a la primera cosechadora fabricada en el pais."
                        }
                    }
                },
                new CityDto()
                {
                    Id = 3,
                    Name = "Susana",
                    Description = "El pueblo que crece",
                    PointsOfInterest = new List<PointsOfInterestDto>()
                    {
                        new PointsOfInterestDto()
                        {
                            Id= 6,
                            Name = "Plaza principal",
                            Description = "Plaza principal donde se realizan paseos y festivales."
                        }
                    }
                },
            };
        }
    }
}
