using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.DbContexts
{
    public class CityInfoContext : DbContext
    {
        public DbSet<City> Cities { get; set; } = null!;

        public DbSet<PointOfInterest> PointsOfInterest { get; set; } = null!;

        public CityInfoContext(DbContextOptions<CityInfoContext> options) : base(options)
        {

        }

        //con el metodo OnModelCreating podemos agregar datos a la base de datos creada por firstcode.
        //solo debemos utilizar el modelBuilder.Entity<>().HasData() e insertarle los datos.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().HasData(
                 new City("Rafaela")
                 {
                     Id = 1,
                     Description = "La perla del oeste",
                 },
                new City("Sunchales")
                {
                    Id = 2,
                    Description = "La capital del cooperativismo",
                },
                new City("Susana")
                {
                    Id = 3,
                    Description = "El pueblo que crece",
                }
            );

            modelBuilder.Entity<PointOfInterest>().HasData(
                 new PointOfInterest("Autodromo de Rafaela")
                 {
                     Id = 1,
                     Description = "Autodromo en forma de ovalo donde se corre el TC",
                     CityId = 1,
                 },
                new PointOfInterest("Estadio Nuevo Monumental AR")
                {
                    Id = 2,
                    Description = "Estadio del equipo Atletico de Rafaela que juega el Nacional B",
                    CityId = 1,
                },
                 new PointOfInterest("Club Atletico Libertad de Sunchales")
                 {
                     Id = 3,
                     Description = "Club que juego el TNA de Basquet",
                     CityId = 2,
                 },
                new PointOfInterest("Edificio Corporativo de Sancor Seguros")
                {
                    Id = 4,
                    Description = "Edificio sobresaliente y deslumbrante donde se encuentra la aseguradora numero uno del país",
                    CityId = 2,
                },
                new PointOfInterest("Monumento a la cosechadora")
                {
                    Id = 5,
                    Description = "Monumento a la primera cosechadora fabricada en el pais.",
                    CityId = 2,
                },
                new PointOfInterest("Plaza principal")
                {
                    Id = 6,
                    Description = "Plaza principal donde se realizan paseos y festivales.",
                    CityId = 3,
                }
            );

            base.OnModelCreating(modelBuilder);
        }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("ConnectionString");
        //    base.OnConfiguring(optionsBuilder);
        //}
    }
}
