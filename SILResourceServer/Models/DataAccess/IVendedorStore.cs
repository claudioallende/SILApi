using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceServer.Models.DataAccess
{
    interface IVendedorStore
    {
        void Save(Vendedor c);
        void Update(int Id, Vendedor c);
        void Delete(int Id);
        Vendedor FindById(long Id);
        Vendedor FindById(long Id, ISession Session);
        Vendedor FindByNroCuenta(long Cuenta);
        Vendedor FindByNroCuenta(long Cuenta, ISession Session);
        IList<Vendedor> FindVendedorByCuit(string Cuit, ISession Session);
        IList<Vendedor> FindAll();
        IList<Vendedor> FindByCuenta(long Cuenta);
        IList<Vendedor> FindVendedorByCuit(string Cuit);
        IList<Vendedor> FindByDomicilio(string Domicilio);
        IList<Vendedor> FindByLocalidad(string Localidad);
        IList<Vendedor> FindByNombre(string Nombre);
        IList<Vendedor> FindByProvincia(string Provincia);
        IList<Vendedor> FindByStipprovee(int Stipprovee);
        IList<Vendedor> FindByTipodecuenta(string Tipodecuenta);
        bool IsInternal(long cuenta);
    }
}
