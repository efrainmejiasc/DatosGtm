using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatosGTMModelo.DataModel
{
    public class MyAppContext :DbContext
    {
         public MyAppContext(DbContextOptions<MyAppContext> options)
            : base(options)
         {
         }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Roles>()
                .ToTable("Roles");
            modelBuilder.Entity<Roles>()
                .Property(s => s.Id)
                .IsRequired(true);
            modelBuilder.Entity<Roles>()
                .Property(s => s.Rol)
                .HasDefaultValue(true);
            modelBuilder.Entity<Roles>()
                .HasData(
                    new Roles
                    {
                       Rol = "Administrador"
                    },
                    new Roles
                    {
                        Rol = "Usuario"
                    },
                    new Roles
                    {
                        Rol = "Cliente"
                    }
                );
        }

        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Tercero> Tercero { get; set; }
    }
}
