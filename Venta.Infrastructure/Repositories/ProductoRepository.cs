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
    public class ProductoRepository : IProductoRepository
    {
        private readonly VentaDbContext _context;
        public ProductoRepository(VentaDbContext context)
        {
            _context = context;
        }


        public Task<bool> Adicionar(Producto entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Producto> Consultar(int id)
        {
            return await _context.Productos.FindAsync(id);
        }

        public async Task<IEnumerable<Producto>> Consultar(string nombre)
        {
            return await _context.Productos.Include(p=>p.Categoria).ToListAsync();
        }

        public Task<bool> Eliminar(Producto entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Modificar(Producto entity)
        {
            throw new NotImplementedException();
        }
    }
}
