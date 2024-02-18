using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venta.Domain.Models;

namespace Venta.Domain.Repositories
{
    public  interface IClienteRepository: IRepository
    {
        Task<bool> Adicionar(Cliente entity);

        Task<bool> Modificar(Cliente entity);

        Task<bool> Eliminar(Cliente entity);

        Task<Cliente> Consultar(int id);

        Task<IEnumerable<Cliente>> Consultar(string nombre);


    }
}
