using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceServer.Models.DataAccess
{
    interface ICuitStore
    {
        void Save(CuposCuit c);
        void Update(string Id, CuposCuit c);
        void Delete(string Id);
        CuposCuit FindByCuit(string cuit);
        IList<CuposCuit> FindByCuit(string cuit, ISession session);
        CuposCuit FindByCuenta(long cuenta);
        IList<CuposCuit> FindByNombre(string nombre);
        IList<CuposCuit> FindLikeNombre(string nombre);
        IList<CuposCuit> FindLikeNombreLimit(string nombre, int limit);
        IList<CuposCuit> FindLikeCuit(string cuit);
        IList<CuposCuit> FindLikeCuitLimit(string cuit, int limit);
    }
}
