using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venta.Domain.Models;

namespace Venta.Domain.Repositories
{
    public  interface IProductoRepository
    {
        bool Adicionar(Producto entity);
        bool Modificar(Producto entity);

        bool Eliminar(Producto entity);

        Producto Consultar(int id);

        IEnumerable<Producto> Consultar(string nombre);


    }
}
