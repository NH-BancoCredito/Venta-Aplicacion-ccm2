using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venta.Domain.Models;

namespace Venta.Domain.Repositories
{
    public  interface ITipoFormaPagoRepository: IRepository
    {
        Task<bool> Adicionar(TipoFormaPago entity);

        Task<bool> Modificar(TipoFormaPago entity);

        Task<bool> Eliminar(TipoFormaPago entity);

        Task<TipoFormaPago> Consultar(int id);

        Task<IEnumerable<TipoFormaPago>> Consultar(string nombre);


    }
}
