﻿// <auto-generated />
using CityInfo.API.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CityInfo.API.Migrations
{
    [DbContext(typeof(CityInfoContext))]
    partial class CityInfoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("CityInfo.API.Entities.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Cities");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "La perla del oeste",
                            Name = "Rafaela"
                        },
                        new
                        {
                            Id = 2,
                            Description = "La capital del cooperativismo",
                            Name = "Sunchales"
                        },
                        new
                        {
                            Id = 3,
                            Description = "El pueblo que crece",
                            Name = "Susana"
                        });
                });

            modelBuilder.Entity("CityInfo.API.Entities.PointOfInterest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CityId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("PointsOfInterest");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CityId = 1,
                            Description = "Autodromo en forma de ovalo donde se corre el TC",
                            Name = "Autodromo de Rafaela"
                        },
                        new
                        {
                            Id = 2,
                            CityId = 1,
                            Description = "Estadio del equipo Atletico de Rafaela que juega el Nacional B",
                            Name = "Estadio Nuevo Monumental AR"
                        },
                        new
                        {
                            Id = 3,
                            CityId = 2,
                            Description = "Club que juego el TNA de Basquet",
                            Name = "Club Atletico Libertad de Sunchales"
                        },
                        new
                        {
                            Id = 4,
                            CityId = 2,
                            Description = "Edificio sobresaliente y deslumbrante donde se encuentra la aseguradora numero uno del país",
                            Name = "Edificio Corporativo de Sancor Seguros"
                        },
                        new
                        {
                            Id = 5,
                            CityId = 2,
                            Description = "Monumento a la primera cosechadora fabricada en el pais.",
                            Name = "Monumento a la cosechadora"
                        },
                        new
                        {
                            Id = 6,
                            CityId = 3,
                            Description = "Plaza principal donde se realizan paseos y festivales.",
                            Name = "Plaza principal"
                        });
                });

            modelBuilder.Entity("CityInfo.API.Entities.PointOfInterest", b =>
                {
                    b.HasOne("CityInfo.API.Entities.City", "City")
                        .WithMany("PointsOfInterest")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("CityInfo.API.Entities.City", b =>
                {
                    b.Navigation("PointsOfInterest");
                });
#pragma warning restore 612, 618
        }
    }
}
