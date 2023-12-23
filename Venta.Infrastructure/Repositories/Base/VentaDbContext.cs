using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venta.Domain.Models;

namespace Venta.Infrastructure.Repositories.Base
{
    public class VentaDbContext: DbContext
    {
       /// <summary>
       /// Configurando el aceso a la base de datos
       /// </summary>
       /// <param name="options"></param>
        public VentaDbContext(DbContextOptions<VentaDbContext>  options)
            :base (options) 
        {

        }

        public virtual DbSet<Producto> Productos { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Producto>(
                p =>
                {
                    p.ToTable("Producto");
                    p.HasKey(c => c.IdProducto);
                    p.Property(c => c.PrecioUnitario).HasPrecision(2);
                }                
                );
        }

    }
}
