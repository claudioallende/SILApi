using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.DataAccess
{
  public class CuitStore : ICuitStore, ICuentaStore
  {
    private string mapping = "CuposCuit.mpg.xml";

    public void Save(CuposCuit c)
    {
      throw new NotImplementedException();
    }

    public void Update(string Id, CuposCuit c)
    {
      throw new NotImplementedException();
    }

    public void Delete(string Id)
    {
      throw new NotImplementedException();
    }

    public CuposCuit FindByCuit(string cuit)
    {
      using (ISession session = HibernateUtil.OpenSession(mapping))
      {
        CuposCuit mycuit = session.Query<CuposCuit>()
             .Where(c => c.Cuit == cuit)
             .FirstOrDefault();
        HibernateUtil.Dispose();
        return mycuit;
      }
    }
    public IList<CuposCuit> FindByCuit(string cuit, ISession session)
    {
      List<CuposCuit> mycuit = session.Query<CuposCuit>()
              .Where(c => c.Cuit == cuit)
              .ToList();
      return mycuit;
    }
    public CuposCuit FindByCuenta(long cuenta)
    {
      using (ISession session = HibernateUtil.OpenSession(mapping))
      {
        CuposCuit mycuit = session.Query<CuposCuit>()
                 .Where(c => c.Cuenta == cuenta)
                 .FirstOrDefault();
        HibernateUtil.Dispose();
        return mycuit;
      }
    }
    public IList<CuposCuit> FindByNombre(string nombre)
    {
      throw new NotImplementedException();
    }
    public IList<CuposCuit> FindLikeCuit(string cuit)
    {
      using (ISession session = HibernateUtil.OpenSession(mapping))
      {
        IList<CuposCuit> mycuit = session.Query<CuposCuit>()
             .Where(c => c.Cuit.Contains(cuit))
             .ToList();
        HibernateUtil.Dispose();
        return mycuit;
      }
    }
    public IList<CuposCuit> FindLikeNombre(string nombre)
    {
      using (ISession session = HibernateUtil.OpenSession(mapping))
      {
        IList<CuposCuit> mycuit = session.Query<CuposCuit>()
             .Where(c => c.Nombre.Contains(nombre))
             .ToList();
        HibernateUtil.Dispose();
        return mycuit;
      }
    }


    public IList<CuposCuit> FindLikeNombreLimit(string nombre, int limit)
    {
      using (ISession session = HibernateUtil.OpenSession(mapping))
      {
        IList<CuposCuit> mycuit = session.Query<CuposCuit>()
             .Where(c => c.Nombre.Contains(nombre))
             .Take(limit)
             .ToList();
        HibernateUtil.Dispose();
        return mycuit;
      }
    }

    public IList<CuposCuit> FindLikeCuitLimit(string cuit, int limit)
    {
      using (ISession session = HibernateUtil.OpenSession(mapping))
      {
        IList<CuposCuit> mycuit = session.Query<CuposCuit>()
             .Where(c => c.Cuit.Contains(cuit))
             .Take(limit)
             .ToList();
        HibernateUtil.Dispose();
        return mycuit;
      }
    }

    public IList<ICuenta> FindStartsWithCuentaLimit(long cuenta, int limit)
    {
      using (ISession session = HibernateUtil.OpenSession(mapping))
      {
        IList<ICuenta> mycuenta = session.Query<CuposCuit>()
             .Where(c => c.Cuenta.ToString().StartsWith(cuenta.ToString()))
             .Take(limit)
             .ToList<ICuenta>();
        HibernateUtil.Dispose();
        return mycuenta;
      }
    }

    public IList<ICuenta> FindStartsWithIgnoreCaseNombreLimit(string nombre, int limit)
    {
      throw new NotImplementedException();
    }

    public IList<ICuenta> FindLikeCuentaLimit(long cuenta, int limit)
    {
      throw new NotImplementedException();
    }

    public IList<ICuenta> FindLikeIgnoreCaseNombreLimit(string nombre, int limit)
    {
      using (ISession session = HibernateUtil.OpenSession(mapping))
      {
        IList<ICuenta> cuenta = session.Query<CuposCuit>()
            .Where(x => x.Nombre.ToUpper().Trim().Contains(nombre.ToUpper().Trim()))
            .Take(limit)
            .ToList<ICuenta>();
        HibernateUtil.Dispose();
        return cuenta;
      }
    }

    public ICuenta FindNombreAndCuitByCuenta(long cuenta)
    {
      using (ISession session = HibernateUtil.OpenSession(mapping))
      {
        ICuenta mycuit = session.Query<CuposCuit>()
             .Where(c => c.Cuenta == cuenta)
             .FirstOrDefault<ICuenta>();
        HibernateUtil.Dispose();
        return mycuit;
      }
    }


    public IList<string> FindNombreLikeNombre(string value)
    {
      using (ISession session = HibernateUtil.OpenSession(mapping))
      {
        IList<string> mycuit = session.Query<CuposCuit>()
             .Where(c => c.Nombre.ToUpper().Contains(value.ToUpper()))
             .Select(x => x.Nombre)
             .ToList<string>();
        HibernateUtil.Dispose();
        return mycuit;
      }
    }

    IList<ICuenta> ICuentaStore.FindLikeNombreLimit(string value, int limit)
    {
      using (ISession session = HibernateUtil.OpenSession(mapping))
      {
        IList<ICuenta> mycuenta = session.Query<CuposCuit>()
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
        IList<ICuenta> mycuenta = session.Query<CuposCuit>()
             .Where(c => c.Cuit.StartsWith(cuit))
             .Take(limit)
             .ToList<ICuenta>();
        HibernateUtil.Dispose();
        return mycuenta;
      }
    }


    public IList<ICuenta> FindStartsWithCuentaLimit(long cuenta, ISession session, int limit)
    {
      IList<ICuenta> mycuenta = session.Query<CuposCuit>()
               .Where(c => c.Cuenta.ToString().StartsWith(cuenta.ToString()))
               .Take(limit)
               .ToList<ICuenta>();
      return mycuenta;
    }

    IList<ICuenta> ICuentaStore.FindByCuit(string cuit)
    {
      throw new NotImplementedException();
    }

    public IList<ICuenta> GetAll()
    {
      throw new NotImplementedException();
    }

    public IList<ICuenta> FindByCuits(IList<string> cuits)
    {
      using (ISession session = HibernateUtil.OpenSession(mapping))
      {
        IList<ICuenta> mycuenta = session.Query<CuposCuit>()
             .Where(c => cuits.Contains(c.Cuit))
             .ToList<ICuenta>();
        HibernateUtil.Dispose();
        return mycuenta;
      }
    }

    public IList<ICuenta> FindByCuentas(IList<string> cuentas)
    {
      using (ISession session = HibernateUtil.OpenSession(mapping))
      {
        IList<ICuenta> mycuenta = session.Query<CuposCuit>()
             .Where(c => cuentas.Contains(c.Cuenta.ToString()))
             .ToList<ICuenta>();
        HibernateUtil.Dispose();
        return mycuenta;
      }
    }
  }
}