﻿// <auto-generated />
using System;
using DatosGTMModelo.DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DatosGTMModelo.Migrations
{
    [DbContext(typeof(MyAppContext))]
    partial class MyAppContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DatosGTMModelo.DataModel.Roles", b =>
                {
                    b.Property<string>("Rol")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("VARCHAR(50)")
                        .HasDefaultValue("True")
                        .HasColumnOrder(2);

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INT")
                        .HasColumnOrder(1);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.HasKey("Rol");

                    b.ToTable("Roles", (string)null);

                    b.HasData(
                        new
                        {
                            Rol = "Administrador",
                            Id = 0
                        },
                        new
                        {
                            Rol = "Usuario",
                            Id = 0
                        },
                        new
                        {
                            Rol = "Cliente",
                            Id = 0
                        });
                });

            modelBuilder.Entity("DatosGTMModelo.DataModel.Tercero", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INT")
                        .HasColumnOrder(1);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("FechaInicio")
                        .IsRequired()
                        .HasColumnType("VARCHAR(20)")
                        .HasColumnOrder(5);

                    b.Property<DateTime>("FechaRegistro")
                        .HasColumnType("DATETIME")
                        .HasColumnOrder(6);

                    b.Property<Guid>("Identificador")
                        .HasColumnType("UNIQUEIDENTIFIER")
                        .HasColumnOrder(7);

                    b.Property<string>("Nit")
                        .IsRequired()
                        .HasColumnType("VARCHAR(50)")
                        .HasColumnOrder(4);

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("VARCHAR(500)")
                        .HasColumnOrder(3);

                    b.Property<string>("Numero")
                        .IsRequired()
                        .HasColumnType("VARCHAR(10)")
                        .HasColumnOrder(2);

                    b.HasKey("Id");

                    b.ToTable("Tercero");
                });

            modelBuilder.Entity("DatosGTMModelo.DataModel.Usuario", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("VARCHAR(50)")
                        .HasColumnOrder(5);

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasColumnType("VARCHAR(50)")
                        .HasColumnOrder(3);

                    b.Property<bool>("Estado")
                        .HasColumnType("BIT")
                        .HasColumnOrder(9);

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("DATETIME")
                        .HasColumnOrder(8);

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INT")
                        .HasColumnOrder(1);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("VARCHAR(50)")
                        .HasColumnOrder(2);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("VARCHAR(50)")
                        .HasColumnOrder(6);

                    b.Property<string>("Password2")
                        .IsRequired()
                        .HasColumnType("VARCHAR(50)")
                        .HasColumnOrder(7);

                    b.Property<int>("RolId")
                        .HasColumnType("INT")
                        .HasColumnOrder(10);

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("VARCHAR(50)")
                        .HasColumnOrder(4);

                    b.HasKey("Email");

                    b.ToTable("Usuario");
                });
#pragma warning restore 612, 618
        }
    }
}
