using ResourceServer.Models.Configuracion;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceServer.Models.DataAccess
{
    interface IDTablaStore
    {
        DTabla findByUvalue(long id);
        IList<DTabla> findClaveValorByEntidadAndOrden(string entidad, string orden);
        DTabla findByEntidadAndOrdenAndClave(string entidad, string orden, string clave);
        IList<string> FindEmailByCuit(string Cuit);
        IList<string> GetEmailsByClave(string Clave);
        IList<string> GetEmailsByClave(string Clave, ISession Session);
        void SaveEmails(string Cuenta, string Nombre, string Emails1, string Emails2, string Emails3, string Emails4, string Emails5);
        IList<DTabla> FindObjectEmailByCuit(string Cuit);
        IList<DTabla> FindEmailByClaveCuenta(string Cuenta);
        void UpdateEmails(DTabla Emails1, DTabla Emails2, DTabla Emails3, DTabla Emails4, DTabla Emails5);
        string SaveCuentaCYO(CuentaCYOViewModel Cuenta);
        string SaveOrUpdateCuentaCYO(CuentaCYOViewModel Cuenta);
        DTabla FindByEntidadAndClaveAndOrdenAndValor(string Entidad, string Clave, string Orden, string Valor);
        DTabla FindByEntidadAndClave(string Entidad, string Clave, ISession Session = null);
        DTabla FindByEntidadAndOrdenAndValor(string Entidad, string Orden, string Valor);
        DTabla FindByEntidadAndOrdenAndValor(string Entidad, string Orden, string Valor, ISession Session);
        IList<DTabla> FindByEntidadAndOrdenAndValor(string Entidad, string Orden, IList<string> Valor, ISession Session);
        string BuscarYBorrarCuentaCYO(CuentaCYOViewModel Cuenta);
        void DeleteCuentaCYO(IList<DTabla> Entidades, ISession session);
    }
}
