using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.DataAccess
{
    public class CompradorStore : ICompradorStore, ICuentaStore
    {
        private string mapping = "Comprador.mpg.xml";

        public void Save(Comprador c)
        {
            throw new NotImplementedException();
        }

        public void Update(int Id, Comprador c)
        {
            throw new NotImplementedException();
        }

        public void Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public Comprador FindById(long Id)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                Comprador comprador = session.Query<Comprador>()
                    .Where(x => x.Id == Id)
                    .FirstOrDefault();
                HibernateUtil.Dispose();
                return comprador;
            }
        }

        public IList<Comprador> FindByCuenta(long Cuenta)
        {
            throw new NotImplementedException();
        }

        public IList<Comprador> FindByCuit(string Cuit, ISession Session)
        {
            IList<Comprador> comprador = Session.Query<Comprador>()
                   .Where(x => x.Cuit == Cuit)
                   .ToList();
            return comprador;
        }

        public IList<Comprador> FindByDomicilio(string Domicilio)
        {
            throw new NotImplementedException();
        }

        public IList<Comprador> FindByLocalidad(string Localidad)
        {
            throw new NotImplementedException();
        }

        public IList<Comprador> FindByNombre(string Nombre)
        {
            throw new NotImplementedException();
        }

        public IList<Comprador> FindByProvincia(string Provincia)
        {
            throw new NotImplementedException();
        }

        public IList<Comprador> FindByStipprovee(int Stipprovee)
        {
            throw new NotImplementedException();
        }

        public IList<Comprador> FindByTipodecuenta(string Tipodecuenta)
        {
            throw new NotImplementedException();
        }


        public IList<Comprador> FindAll()
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<Comprador> compradores = session.Query<Comprador>()
                    .ToList();
                HibernateUtil.Dispose();
                return compradores;
            }
        }

        public IList<Comprador> FindLikeCuentaLimit(int cuenta, int limit)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<Comprador> mycuenta = session.Query<Comprador>()
                     .Where(c => c.Cuenta.ToString().StartsWith(cuenta.ToString()))
                     .Take(limit)
                     .ToList();
                HibernateUtil.Dispose();
                return mycuenta;
            }
        }

        public IList<Comprador> FindLikeNombreLimit(string nombre, int limit)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<Comprador> mycuenta = session.Query<Comprador>()
                     .Where(c => c.Nombre.StartsWith(nombre))
                     .Take(limit)
                     .ToList();
                HibernateUtil.Dispose();
                return mycuenta;
            }
        }

        IList<ICuenta> ICuentaStore.FindStartsWithCuentaLimit(long cuenta, int limit)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<ICuenta> mycuenta = session.Query<Comprador>()
                     .Where(c => c.Cuenta.ToString().StartsWith(cuenta.ToString()))
                     .Take(limit)
                     .ToList<ICuenta>();
                HibernateUtil.Dispose();
                return mycuenta;
            }
        }

        IList<ICuenta> ICuentaStore.FindStartsWithIgnoreCaseNombreLimit(string nombre, int limit)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<ICuenta> mycuenta = session.Query<Comprador>()
                     .Where(c => c.Nombre.ToUpper().StartsWith(nombre.ToUpper()))
                     .Take(limit)
                     .ToList<ICuenta>();
                HibernateUtil.Dispose();
                return mycuenta;
            }
        }


        IList<ICuenta> ICuentaStore.FindLikeCuentaLimit(long cuenta, int limit)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<ICuenta> mycuenta = session.Query<Comprador>()
                     .Where(c => c.Cuenta.ToString().Contains(cuenta.ToString()))
                     .Take(limit)
                     .ToList<ICuenta>();
                HibernateUtil.Dispose();
                return mycuenta;
            }
        }

        public IList<ICuenta> FindLikeIgnoreCaseNombreLimit(string nombre, int limit)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<ICuenta> mycuenta = session.Query<Comprador>()
                     .Where(c => c.Nombre.ToUpper().Contains(nombre.ToUpper()))
                     .Take(limit)
                     .ToList<ICuenta>();
                HibernateUtil.Dispose();
                return mycuenta;
            }
        }

        public ICuenta FindNombreAndCuitByCuenta(long cuenta)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                ICuenta mycuenta = session.Query<Comprador>()
                     .Where(c => c.Cuenta == cuenta)
                     .FirstOrDefault<ICuenta>();
                HibernateUtil.Dispose();
                return mycuenta;
            }
        }

        public Comprador FindByCuenta(long cuenta, ISession session)
        {
            Comprador mycuenta = session.Query<Comprador>()
                    .Where(c => c.Cuenta == cuenta)
                    .FirstOrDefault<Comprador>();
            return mycuenta;
        }

        public IList<string> FindNombreLikeNombre(string value)
        {
            throw new NotImplementedException();
        }

        IList<ICuenta> ICuentaStore.FindLikeNombreLimit(string value, int limit)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<ICuenta> mycuenta = session.Query<Comprador>()
                     .Where(c => c.Nombre.StartsWith(value))
                     .Take(limit)
                     .ToList<ICuenta>();
                HibernateUtil.Dispose();
                return mycuenta;
            }
        }


        public IList<ICuenta> FindByCuitLimit(string cuit, int limit)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<ICuenta> mycuenta = session.Query<Comprador>()
                     .Where(c => c.Cuit.StartsWith(cuit))
                     .Take(limit)
                     .ToList<ICuenta>();
                HibernateUtil.Dispose();
                return mycuenta;
            }
        }


        public IList<ICuenta> FindStartsWithCuentaLimit(long cuenta, ISession session, int limit)
        {
            IList<ICuenta> mycuenta = session.Query<Comprador>()
                     .Where(c => c.Cuenta.ToString().StartsWith(cuenta.ToString()))
                     .Take(limit)
                     .ToList<ICuenta>();
            return mycuenta;
        }

        public Comprador FindById(long Id, ISession Session)
        {
            Comprador comprador = Session.Query<Comprador>()
                    .Where(x => x.Id == Id)
                    .FirstOrDefault();
            return comprador;
        }

        public IList<ICuenta> FindByCuit(string cuit)
        {
            throw new NotImplementedException();
        }

        public IList<ICuenta> GetAll()
        {
            throw new NotImplementedException();
        }

    public IList<ICuenta> FindByCuits(IList<string> cuits)
    {
      throw new NotImplementedException();
    }

    public IList<ICuenta> FindStartsWithCuentaLimitCC(long cuenta, int limit)
    {
      throw new NotImplementedException();
    }

    public IList<ICuenta> FindLikeIgnoreCaseNombreLimitCC(string nombre, int limit)
    {
      throw new NotImplementedException();
    }
  }
}