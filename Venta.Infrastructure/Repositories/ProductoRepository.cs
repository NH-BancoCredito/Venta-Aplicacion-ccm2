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


        public async Task<bool> Adicionar(Producto entity)
        {
            try
            {
                _context.Productos.Add(entity);
                _context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;

            }
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

        public async Task<bool> Modificar(Producto entity)
        {
            try
            {
                Producto p = await _context.Productos.FindAsync(entity.IdProducto);

                p.Nombre = entity.Nombre;
                p.Stock = entity.Stock;
                p.StockMinimo = entity.StockMinimo;
                p.PrecioUnitario = entity.PrecioUnitario;
                p.IdCategoria = entity.IdCategoria;

                _context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;

            }

        }
    }
}
