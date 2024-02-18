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
    public class ClienteRepository : IClienteRepository
    {
        private readonly VentaDbContext _context;
        public ClienteRepository(VentaDbContext context)
        {
            _context = context;
        }


        public async Task<bool> Adicionar(Cliente entity)
        {
            try
            {
                _context.Clientes.Add(entity);
                _context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;

            }
        }

        public async Task<Cliente> Consultar(int id)
        {
            return await _context.Clientes.FindAsync(id);
        }

        public async Task<IEnumerable<Cliente>> Consultar(string nombre)
        {
            return await _context.Clientes.ToListAsync();
        }

        public Task<bool> Eliminar(Cliente entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Modificar(Cliente entity)
        {
            throw new NotImplementedException();
        }
    }
}
