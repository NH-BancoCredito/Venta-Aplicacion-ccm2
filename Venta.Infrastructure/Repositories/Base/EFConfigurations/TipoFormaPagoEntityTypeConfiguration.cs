using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Venta.Domain.Models;

namespace Venta.Infrastructure.Repositories.Base.EFConfigurations
{
    public class TipoFormaPagoEntityTypeConfiguration
        : IEntityTypeConfiguration<TipoFormaPago>
    {
        public void Configure(EntityTypeBuilder<TipoFormaPago> builder)
        {
            builder.ToTable("TipoFormaPago");
            builder.HasKey(c => c.IdTipoFormaPago);
            
        }
    }
}
