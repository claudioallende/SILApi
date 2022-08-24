using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceServer.Models.DataAccess
{
    interface ICompradorStore
    {
        void Save(Comprador c);
        void Update(int Id, Comprador c);
        void Delete(int Id);
        Comprador FindById(long Id);
        Comprador FindById(long Id, ISession Session);
        IList<Comprador> FindAll();
        IList<Comprador> FindByCuenta(long Cuenta);
        Comprador FindByCuenta(long cuenta, ISession session);
        IList<Comprador> FindByCuit(string Cuit, ISession Session);
        IList<Comprador> FindByDomicilio(string Domicilio);
        IList<Comprador> FindByLocalidad(string Localidad);
        IList<Comprador> FindByNombre(string Nombre);
        IList<Comprador> FindByProvincia(string Provincia);
        IList<Comprador> FindByStipprovee(int Stipprovee);
        IList<Comprador> FindByTipodecuenta(string Tipodecuenta);
    }
}
