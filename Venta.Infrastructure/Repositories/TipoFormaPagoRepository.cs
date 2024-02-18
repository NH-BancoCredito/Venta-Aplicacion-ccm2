using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venta.Domain.Models;
using Venta.Domain.Repositories;
using Venta.Infrastructure.Repositories.Base;

namespace Venta.Infrastructure.Repositories
{
    public class TipoFormaPagoRepository : ITipoFormaPagoRepository
    {
        private readonly VentaDbContext _context;
        public TipoFormaPagoRepository(VentaDbContext context)
        {
            _context = context;
        }


        public async Task<bool> Adicionar(TipoFormaPago entity)
        {
            try
            {
                _context.TipoFormasPago.Add(entity);
                _context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;

            }
        }

        public async Task<TipoFormaPago> Consultar(int id)
        {
            return await _context.TipoFormasPago.FindAsync(id);
        }

        public async Task<IEnumerable<TipoFormaPago>> Consultar(string nombre)
        {
            return await _context.TipoFormasPago.ToListAsync();
        }

        public Task<bool> Eliminar(TipoFormaPago entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Modificar(TipoFormaPago entity)
        {
            throw new NotImplementedException();
        }
    }
}
