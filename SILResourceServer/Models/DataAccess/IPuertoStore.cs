using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceServer.Models.DataAccess
{
    interface IPuertoStore
    {
        void Save(Puerto c);
        void Update(int Id, Puerto c);
        void Delete(int Id);
        Puerto FindById(long Id);
        Puerto FindById(long Id, ISession Session);
        Puerto FindByIdTerminal(long IdTerminal, ISession session);
        IList<Puerto> FindAll(ISession Session = null);
        IList<Puerto> FindByCuenta(long Cuenta);
        IList<Puerto> FindByCuenta(long Cuenta, ISession Session);
        IList<Puerto> FindPuertoStartsWithCuentaLimit(long cuenta, ISession session, int limit);
        IList<Puerto> FindByCuit(string Cuit, ISession session);
        IList<Puerto> FindByDomicilio(string Domicilio);
        IList<Puerto> FindByLocalidad(string Localidad);
        IList<Puerto> FindByNombre(string Nombre);
        IList<Puerto> FindPuertoLikeIgnoreCaseNombreLimit(string nombre, ISession session, int limit);
        IList<Puerto> FindByProvincia(string Provincia);
        IList<Puerto> FindByStipprovee(int Stipprovee);
        IList<Puerto> FindByTipodecuenta(string Tipodecuenta);
        string FindNroPlantaByCuentaPuerto(long CuentaPuerto);
        string FindNroPlantaByCuentaPuerto(long CuentaPuerto, ISession Session);
    }
}
