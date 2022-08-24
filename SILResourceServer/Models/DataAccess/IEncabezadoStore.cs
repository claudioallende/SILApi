using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceServer.Models.DataAccess
{
    interface IEncabezadoStore
    {
        Encabezado FindById(long Id);
        IList<Encabezado> FindAll();
        IList<Encabezado> FindByGranoAndCompradorAndVendedorAndPuerto(int grano, long comp, long vend, long puerto);
        IList<Encabezado> FindByGranoAndCompradorAndVendedorAndPuertoCyO(int grano, long comp, long vend, long puerto);
    }
}
