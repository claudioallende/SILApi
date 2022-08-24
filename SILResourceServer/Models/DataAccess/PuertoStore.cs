using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.DataAccess
{
    public class PuertoStore : IPuertoStore, ICuentaStore
    {
        private string mapping = "Puerto.mpg.xml";

        public void Save(Puerto c)
        {
            throw new NotImplementedException();
        }

        public void Update(int Id, Puerto c)
        {
            throw new NotImplementedException();
        }

        public void Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public Puerto FindById(long Id)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                Puerto puerto = session.Query<Puerto>()
                    .Where(x => x.Id == Id)
                    .FirstOrDefault();
                HibernateUtil.Dispose();
                return puerto;
            }
        }

        public IList<Puerto> FindByCuenta(long Cuenta)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// consulta los puertos para el cuit pasado como parametros.
        /// retorna una lista porque por ej. el caso de ACA tiene varios puertos.
        /// </summary>
        /// <param name="Cuit"></param>
        /// <returns></returns>
        public IList<Puerto> FindByCuit(string Cuit, ISession session)
        {
            IList<Puerto> puertos = session.Query<Puerto>()
                    .Where(c => c.Cuit == Cuit)
                    .ToList<Puerto>();
            return puertos;
        }

        public IList<Puerto> FindByDomicilio(string Domicilio)
        {
            throw new NotImplementedException();
        }

        public IList<Puerto> FindByLocalidad(string Localidad)
        {
            throw new NotImplementedException();
        }

        public IList<Puerto> FindByNombre(string Nombre)
        {
            throw new NotImplementedException();
        }

        public IList<Puerto> FindByProvincia(string Provincia)
        {
            throw new NotImplementedException();
        }

        public IList<Puerto> FindByStipprovee(int Stipprovee)
        {
            throw new NotImplementedException();
        }

        public IList<Puerto> FindByTipodecuenta(string Tipodecuenta)
        {
            throw new NotImplementedException();
        }


        public IList<Puerto> FindAll(ISession Session = null)
        {
            ISession session = (Session == null) ? HibernateUtil.OpenSession(mapping) : Session;
            IList<Puerto> puertos = session.Query<Puerto>()
                .ToList();
            if (Session == null) HibernateUtil.Dispose();
            return puertos;
        }

        public IList<Puerto> FindLikeCuentaLimit(int cuenta, int limit)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<Puerto> mycuenta = session.Query<Puerto>()
                     .Where(c => c.Cuenta.ToString().StartsWith(cuenta.ToString()))
                     .Take(limit)
                     .ToList();
                HibernateUtil.Dispose();
                return mycuenta;
            }
        }

        public IList<Puerto> FindLikeNombreLimit(string nombre, int limit)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<Puerto> mycuenta = session.Query<Puerto>()
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
                IList<ICuenta> mycuenta = session.Query<Puerto>()
                     .Where(c => 
                            c.Cuenta.ToString().StartsWith(cuenta.ToString()))
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
                IList<ICuenta> mycuenta = session.Query<Puerto>()
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
                IList<ICuenta> mycuenta = session.Query<Puerto>()
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
                IList<ICuenta> mycuenta = session.Query<Puerto>()
                     .Where(c => c.Nombre.ToUpper().Contains(nombre.ToUpper()))
                     .Take(limit)
                     .ToList<ICuenta>();
                HibernateUtil.Dispose();
                return mycuenta;
            }
        }

        public IList<Puerto> FindPuertoLikeIgnoreCaseNombreLimit(string nombre, ISession session, int limit)
        {
            IList<Puerto> mypuerto = session.Query<Puerto>()
                    .Where(c => c.Nombre.ToUpper().Contains(nombre.ToUpper()))
                    .Take(limit)
                    .ToList<Puerto>();
            return mypuerto;
        }

        public ICuenta FindNombreAndCuitByCuenta(long cuenta)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                ICuenta mycuenta = session.Query<Puerto>()
                     .Where(c => c.Cuenta == cuenta)
                     .FirstOrDefault();
                HibernateUtil.Dispose();
                return mycuenta;
            }
        }

        IList<ICuenta> ICuentaStore.FindLikeNombreLimit(string value, int limit)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<ICuenta> mycuenta = session.Query<Puerto>()
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
                IList<ICuenta> mycuenta = session.Query<Puerto>()
                     .Where(c => c.Cuit.StartsWith(cuit))
                     .Take(limit)
                     .ToList<ICuenta>();
                HibernateUtil.Dispose();
                return mycuenta;
            }
        }

        public IList<string> FindNombreLikeNombre(string value)
        {
            throw new NotImplementedException();
        }


        public string FindNroPlantaByCuentaPuerto(long CuentaPuerto)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                string NroPlanta = session.Query<Puerto>()
                    .Where(c => c.Cuenta == CuentaPuerto)
                    .FirstOrDefault()
                    .Ruca;
                HibernateUtil.Dispose();
                return NroPlanta;
            }
        }

        public IList<ICuenta> FindStartsWithCuentaLimit(long cuenta, ISession session, int limit)
        {
            IList<ICuenta> mycuenta = session.Query<Puerto>()
                     .Where(c => c.Cuenta.ToString().StartsWith(cuenta.ToString()))
                     .Take(limit)
                     .ToList<ICuenta>();
            return mycuenta;
        }

        public IList<Puerto> FindPuertoStartsWithCuentaLimit(long cuenta, ISession session, int limit)
        {
            IList<Puerto> mycuenta = session.Query<Puerto>()
                     .Where(c => c.Cuenta.ToString().StartsWith(cuenta.ToString()))
                     .Take(limit)
                     .ToList<Puerto>();
            return mycuenta;
        }
        /// <summary>
        /// Retorna el nombre del puerto segun su IdTerminal
        /// segun German siempre habrá solo uno
        /// </summary>
        /// <param name="IdTerminal"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public Puerto FindByIdTerminalLimit(long IdTerminal, int limit)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                Puerto puerto = session.Query<Puerto>()
                    .Where(c => c.IdTerminal.Trim() == IdTerminal.ToString())
                    .Take(limit)
                    .FirstOrDefault();
                HibernateUtil.Dispose();
                return puerto;
            }
        }

        public Puerto FindById(long Id, ISession Session)
        {
            Puerto puerto = Session.Query<Puerto>()
                    .Where(x => x.Id == Id)
                    .FirstOrDefault();
            return puerto;
        }
        
        public IList<Puerto> FindByCuenta(long Cuenta, ISession Session)
        {
            IList<Puerto> puertos = Session.Query<Puerto>()
                     .Where(c => c.Cuenta == Cuenta)
                     .ToList<Puerto>();
            return puertos;
        }

        /// <summary>
        /// Dado el IdTerminal de STOP obtengo el puerto
        /// segun lo especificado por German siempre habrá uno por puerto
        /// </summary>
        /// <param name="IdTerminal"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public Puerto FindByIdTerminal(long IdTerminal, ISession session)
        {
            if (IdTerminal != 0) 
            {
                Puerto puerto = session.Query<Puerto>()
                        .Where(c => c.IdTerminal.Trim() == IdTerminal.ToString())
                        .FirstOrDefault<Puerto>();
                return puerto;
            }
            return null;
        }

        public string FindNroPlantaByCuentaPuerto(long CuentaPuerto, ISession Session)
        {
            string NroPlanta = Session.Query<Puerto>()
                    .Where(c => c.Cuenta == CuentaPuerto)
                    .FirstOrDefault()
                    .Ruca;
            return NroPlanta;
        }


        public ICuenta FindByCuit(long Cuit)
        {
            throw new NotImplementedException();
        }

        public IList<ICuenta> FindByCuit(string cuit)
        {
            throw new NotImplementedException();
        }
    }
}