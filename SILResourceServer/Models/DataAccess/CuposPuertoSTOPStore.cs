using NHibernate;
using ResourceServer.Models.Error.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.DataAccess
{
  public class CuposPuertoSTOPStore : ICuposPuertoSTOPStore, ICuentaStore
  {
    public bool Delete(CuposPuertoSTOP PuertoSTOP, ISession session, ITransaction tx1 = null)
    {
      if (PuertoSTOP != null)
      {
        ITransaction tx = (tx1 != null) ? tx1 : session.BeginTransaction();
        try
        {
          session.Delete(PuertoSTOP);
          tx.Commit();
        }
        catch (Exception e)
        {
          tx.Rollback();
          throw e;
        }
        return true;
      }
      return false;
    }

    public IList<CuposPuertoSTOP> FindAll(ISession session = null)
    {
      IList<CuposPuertoSTOP> puertos = new List<CuposPuertoSTOP>();
      if (session != null) 
      {
        puertos = session.Query<CuposPuertoSTOP>().ToList();
      }
      else
      {
        using (ISession sess = HibernateUtil.OpenSession())
        {
          puertos = sess.Query<CuposPuertoSTOP>().ToList();
          HibernateUtil.Dispose();
        }
      }
      return puertos;

    }

        public IList<ICuenta> FindByCuit(string cuit)
        {
            throw new NotImplementedException();
        }

        public IList<ICuenta> FindByCuitLimit(string cuit, int limit)
        {
            throw new NotImplementedException();
        }

        public CuposPuertoSTOP FindByNroPuerto(long nroPuerto, ISession session)
    {
      CuposPuertoSTOP puertosSTOP = session.Query<CuposPuertoSTOP>().Where(c => c.NroPuerto == nroPuerto).FirstOrDefault();
      return puertosSTOP;
    }

        public IList<ICuenta> FindLikeCuentaLimit(long cuenta, int limit)
        {
            throw new NotImplementedException();
        }

        public IList<ICuenta> FindLikeIgnoreCaseNombreLimit(string nombre, int limit)
        {
            using (ISession session = HibernateUtil.OpenSession())
            {
                IList<ICuenta> mycuenta = session.Query<CuposPuertoSTOP>()
                     .Where(c => c.NombrePuerto.ToUpper().Contains(nombre.ToUpper()))
                     .Take(limit)
                     .ToList<ICuenta>();
                HibernateUtil.Dispose();
                return mycuenta;
            }
        }

        public IList<ICuenta> FindLikeNombreLimit(string value, int limit)
        {
            throw new NotImplementedException();
        }

        public ICuenta FindNombreAndCuitByCuenta(long cuenta)
        {
            throw new NotImplementedException();
        }

        public IList<string> FindNombreLikeNombre(string value)
        {
            throw new NotImplementedException();
        }

        public IList<ICuenta> FindStartsWithCuentaLimit(long cuenta, int limit)
        {
            using (ISession session = HibernateUtil.OpenSession())
            {
                IList<ICuenta> mycuenta = session.Query<CuposPuertoSTOP>()
                     .Where(c =>
                            c.NroPuerto.ToString().StartsWith(cuenta.ToString()))
                     .Take(limit)
                     .ToList<ICuenta>();
                HibernateUtil.Dispose();
                return mycuenta;
            }
        }

        public IList<ICuenta> FindStartsWithCuentaLimit(long cuenta, ISession session, int limit)
        {
            throw new NotImplementedException();
        }

        public IList<ICuenta> FindStartsWithIgnoreCaseNombreLimit(string nombre, int limit)
        {
            throw new NotImplementedException();
        }

        public IList<ICuenta> GetAll()
        {
            throw new NotImplementedException();
        }

        public CuposPuertoSTOP Save(CuposPuertoSTOP c, ISession session)
    {
      if (c != null)
      {
        using (ITransaction tx = session.BeginTransaction())
        {
          try
          {
            session.Save(c);
            tx.Commit();
            return this.FindByNroPuerto(c.NroPuerto, session);
          }
          catch (NHibernate.Exceptions.GenericADOException e)
          {
            if (e.GetBaseException().Message.Contains("ORA-00001"))
            {
              tx.Rollback();
              HibernateUtil.Dispose();
              throw new RegistroDuplicadoException("Cupos Puerto STOP");
            }
            else
            {
              tx.Rollback();
              HibernateUtil.Dispose();
              throw e;
            }
          }
          catch (Exception e)
          {
            tx.Rollback();
            HibernateUtil.Dispose();
            throw e;
          }
        }
      }
      return null;
    }

    public CuposPuertoSTOP Update(CuposPuertoSTOP c, ISession session)
    {
      if (c != null)
      {
        using (ITransaction tx = session.BeginTransaction())
        {
          try
          {
            session.Update(c, c.NroPuerto);
            tx.Commit();
            return this.FindByNroPuerto(c.NroPuerto, session);
          }
          catch (NHibernate.Exceptions.GenericADOException e)
          {
            if (e.GetBaseException().Message.Contains("ORA-00001"))
            {
              tx.Rollback();
              HibernateUtil.Dispose();
              throw new RegistroDuplicadoException("Cupos Puerto STOP");
            }
            else
            {
              tx.Rollback();
              HibernateUtil.Dispose();
              throw e;
            }
          }
          catch (Exception e)
          {
            tx.Rollback();
            HibernateUtil.Dispose();
            throw e;
          }
        }
      }
      return null;
    }
  }
}