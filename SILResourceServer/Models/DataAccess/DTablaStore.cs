using ResourceServer.Models.Configuracion;
using ResourceServer.Models.Error;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.DataAccess
{
    public class DTablaStore : IDTablaStore
    {
        private string mapping = "DTabla.mpg.xml";
        public DTabla findByUvalue(long id)
        {
            throw new NotImplementedException();
        }

        public IList<DTabla> findClaveValorByEntidadAndOrden(string entidad, string orden)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<DTabla> list =  session.Query<DTabla>()
                    .Where(x => x.Entidad == entidad && x.Orden == orden)
                    .Select(x => new DTabla
                    {
                        Clave = x.Clave,
                        Valor = x.Valor
                    })
                    .ToList();
                HibernateUtil.Dispose();
                return list;
            }
        }


        public DTabla findByEntidadAndOrdenAndClave(string entidad, string orden, string clave)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                DTabla obj =  session.Query<DTabla>()
                    .Where(x => x.Entidad == entidad && x.Orden == orden && x.Clave == clave)
                    .FirstOrDefault();
                HibernateUtil.Dispose();
                return obj;
            }
        }

        public IList<string> FindEmailByCuit(string Cuit)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                var Ordenes = new string[] { "CPM03","CPM04","CPM05","CPM06","CPM07" };
                IList<string> list = session.Query<DTabla>()
                    .Where(x => x.Entidad == "CUPMAIL" && Ordenes.Contains(x.Orden) && x.Clave.Trim() == Cuit.Trim())
                    .Select(x => x.Valor)
                    .ToList();
                HibernateUtil.Dispose();
                return list;
            }
        }

        public IList<string> GetEmailsByClave(string Clave)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                var Ordenes = new string[] { "CPM03", "CPM04", "CPM05", "CPM06", "CPM07" };
                IList<string> list = session.Query<DTabla>()
                    .Where(x => x.Entidad == "CUPMAIL" && Ordenes.Contains(x.Orden) && x.Clave.Trim() == Clave.Trim())
                    .Select(x => x.Valor)
                    .ToList();
                HibernateUtil.Dispose();
                return list;
            }
        }

        public IList<DTabla> FindObjectEmailByCuit(string Cuit)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                var Ordenes = new string[] { "CPM03", "CPM04", "CPM05", "CPM06", "CPM07" };
                IList<DTabla> list = session.Query<DTabla>()
                    .Where(x => x.Entidad == "CUPMAIL" && Ordenes.Contains(x.Orden) && x.Clave.Trim() == Cuit.Trim())
                    .ToList();
                HibernateUtil.Dispose();
                return list;
            }
        }

        public void UpdateEmails(DTabla Emails1, DTabla Emails2, DTabla Emails3, DTabla Emails4, DTabla Emails5)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {
                    session.Update(Emails1);
                    session.Update(Emails2);
                    session.Update(Emails3);
                    session.Update(Emails4);
                    session.Update(Emails5);
                    tx.Commit(); 
                    HibernateUtil.Dispose();
                }
                catch (Exception e)
                {
                    tx.Rollback();
                    HibernateUtil.Dispose();
                    throw e;
                }
            }
        }

        public void SaveEmails(string Cuenta, string Nombre, string Emails1, string Emails2, string Emails3, string Emails4, string Emails5)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {
                    DTabla nuevo;
                    //Cuit
                    nuevo = GetMail(Cuenta, "CPM01", Cuenta);
                    session.Save(nuevo);
                    //Nombre
                    nuevo = new DTabla();
                    nuevo = GetMail(Cuenta, "CPM02", Nombre);
                    session.Save(nuevo);
                    SaveMails(session, Cuenta, Emails1, Emails2, Emails3, Emails4, Emails5);
                    tx.Commit();
                    HibernateUtil.Dispose();
                }
                catch (Exception e)
                {
                    tx.Rollback();
                    HibernateUtil.Dispose();
                    throw e;
                }
            }
        }

        private void SaveMails(ISession session, string Cuenta, string Emails1, string Emails2, string Emails3, string Emails4, string Emails5)
        {
            DTabla nuevo;
            nuevo = GetMail(Cuenta, "CPM03", Emails1);
            session.Save(nuevo);
            nuevo = GetMail(Cuenta, "CPM04", Emails2);
            session.Save(nuevo);
            nuevo = GetMail(Cuenta, "CPM05", Emails3);
            session.Save(nuevo);
            nuevo = GetMail(Cuenta, "CPM06", Emails4);
            session.Save(nuevo);
            nuevo = GetMail(Cuenta, "CPM07", Emails5);
            session.Save(nuevo);
        }

        private DTabla GetMail(string clave, string orden, string valor){
            DTabla nuevo;
            nuevo = new DTabla();
            nuevo.Entidad = "CUPMAIL";
            nuevo.Clave = clave;
            nuevo.Orden = orden;
            nuevo.Valor = valor;
            nuevo.Empresa = 0;
            nuevo.Nuevo1 = DateTime.Now;
            return nuevo;
        }

        public string SaveOrUpdateCuentaCYO(CuentaCYOViewModel Cuenta)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                DTabla CuentaABuscar = Cuenta.GetEntidadCuenta();
                IList<DTabla> busqueda = session.Query<DTabla>()
                    .Where(x => x.Entidad == CuentaABuscar.Entidad && x.Empresa == CuentaABuscar.Empresa
                        && x.Clave == CuentaABuscar.Clave)
                    .ToList();
                HibernateUtil.Dispose();
                if (busqueda.Count > 0)
                {
                    return UpdateCuentaCYO(busqueda, Cuenta);
                }
                else
                {
                    return SaveCuentaCYO(Cuenta);
                }
            }
        }

        public string UpdateCuentaCYO(IList<DTabla> Entidades, CuentaCYOViewModel Cuenta)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {
                    DTabla entidad;
                    entidad = Entidades.Where(x => x.Orden == "CYO01").FirstOrDefault();
                    entidad.Valor = Cuenta.NumeroCuenta;
                    session.Update(entidad);
                    entidad = Entidades.Where(x => x.Orden == "CYO02").FirstOrDefault();
                    entidad.Valor = Cuenta.NombreCuenta;
                    session.Update(entidad);
                    entidad = Entidades.Where(x => x.Orden == "CYO03").FirstOrDefault();
                    entidad.Valor = Cuenta.Cuit;
                    session.Update(entidad);
                    tx.Commit();
                    HibernateUtil.Dispose();
                    return "OK";
                }
                catch (Exception e)
                {
                    tx.Rollback();
                    HibernateUtil.Dispose();
                    throw e;
                }
            }
        }

        public string SaveCuentaCYO(CuentaCYOViewModel Cuenta)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {
                    session.Save(Cuenta.GetEntidadCuenta());
                    session.Save(Cuenta.GetEntidadDescripcionCuenta());
                    session.Save(Cuenta.GetEntidadCuit());
                    tx.Commit();
                    HibernateUtil.Dispose();
                    return "OK";
                }
                catch (Exception e)
                {
                    tx.Rollback();
                    HibernateUtil.Dispose();
                    throw e;
                }
            }
        }

        public string BuscarYBorrarCuentaCYO(CuentaCYOViewModel Cuenta)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                DTabla CuentaABuscar = Cuenta.GetEntidadCuenta();
                IList<DTabla> busqueda = session.Query<DTabla>()
                    .Where(x => x.Entidad == CuentaABuscar.Entidad && x.Empresa == CuentaABuscar.Empresa
                        && x.Clave == CuentaABuscar.Clave)
                    .ToList();
                if (busqueda.Count > 0)
                {
                    DeleteCuentaCYO(busqueda, session);
                }
                HibernateUtil.Dispose();
                return "OK";
            }
        }

        public void DeleteCuentaCYO(IList<DTabla> Entidades, ISession session)
        {
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {
                    foreach (DTabla entidad in Entidades)
                    {
                        session.Delete(entidad);
                    }
                    tx.Commit();
                }
                catch(Exception e)
                {
                    tx.Rollback();
                    throw e;
                }
            }
        }

        public DTabla FindByEntidadAndClaveAndOrdenAndValor(string Entidad, string Clave, string Orden, string Valor)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                DTabla obj = session.Query<DTabla>().Where(x => x.Entidad == Entidad &&
                    x.Clave == Clave && x.Orden == Orden && x.Valor == Valor).FirstOrDefault();
                HibernateUtil.Dispose();
                return obj;
            }
        }

        public DTabla FindByEntidadAndClave(string Entidad, string Clave, ISession Session = null)
        {
            ISession session = (Session == null) ? HibernateUtil.OpenSession(mapping) : Session;
            DTabla obj = session.Query<DTabla>().Where(x => x.Entidad == Entidad &&
                x.Clave == Clave).FirstOrDefault();
            if (Session == null) HibernateUtil.Dispose(); 
            return obj;
        }


        public DTabla FindByEntidadAndOrdenAndValor(string Entidad, string Orden, string Valor)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                DTabla obj = session.Query<DTabla>().Where(x => x.Entidad == Entidad &&
                    x.Orden == Orden && x.Valor == Valor).FirstOrDefault();
                HibernateUtil.Dispose();
                return obj;
            }
        }

        public IList<DTabla> FindEmailByClaveCuenta(string Cuenta)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                var Ordenes = new string[] { "CPM03", "CPM04", "CPM05", "CPM06", "CPM07" };
                IList<DTabla> list = session.Query<DTabla>()
                    .Where(x => x.Entidad == "CUPMAIL" && Ordenes.Contains(x.Orden) && x.Clave.Trim() == Cuenta.Trim())
                    .ToList();
                HibernateUtil.Dispose();
                return list;
            }
        }


        public DTabla FindByEntidadAndOrdenAndValor(string Entidad, string Orden, string Valor, ISession Session)
        {
            DTabla obj = Session.Query<DTabla>().Where(x => x.Entidad == Entidad &&
                    x.Orden == Orden && x.Valor == Valor).FirstOrDefault();
            return obj;
        }

        public IList<string> GetEmailsByClave(string Clave, ISession Session)
        {
            var Ordenes = new string[] { "CPM03", "CPM04", "CPM05", "CPM06", "CPM07" };
            IList<string> list = Session.Query<DTabla>()
                .Where(x => x.Entidad == "CUPMAIL" && Ordenes.Contains(x.Orden) && x.Clave.Trim() == Clave.Trim())
                .Select(x => x.Valor)
                .ToList();
            return list;
        }


        public IList<DTabla> FindByEntidadAndOrdenAndValor(string Entidad, string Orden, IList<string> Valor, ISession Session)
        {
            IList<DTabla> obj = Session.Query<DTabla>().Where(x => x.Entidad == Entidad &&
                    x.Orden == Orden && Valor.Contains(x.Valor)).ToList();
            return obj;
        }
    }
}