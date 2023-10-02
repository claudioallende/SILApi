using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.DataAccess
{
  public class VendedorStore : IVendedorStore, ICuentaStore
  {
    private string mapping = "Vendedor.mpg.xml";

    public void Save(Vendedor c)
    {
      throw new NotImplementedException();
    }

    public void Update(int Id, Vendedor c)
    {
      throw new NotImplementedException();
    }

    public void Delete(int Id)
    {
      throw new NotImplementedException();
    }

    public Vendedor FindById(long Id)
    {
      using (ISession session = HibernateUtil.OpenSession(mapping))
      {
        Vendedor vendedor = session.Query<Vendedor>()
            .Where(x => x.Id == Id)
            .FirstOrDefault();
        HibernateUtil.Dispose();
        return vendedor;
      }
    }

    public IList<Vendedor> FindByCuenta(long Cuenta)
    {
      throw new NotImplementedException();
    }

    public Vendedor FindByNroCuenta(long Cuenta)
    {
      using (ISession session = HibernateUtil.OpenSession(mapping))
      {
        Vendedor vendedor = session.Query<Vendedor>()
            .Where(x => x.Cuenta == Cuenta)
            .FirstOrDefault();
        HibernateUtil.Dispose();
        return vendedor;
      }
    }

    public IList<Vendedor> FindVendedorByCuit(string Cuit)
    {
      IList<Vendedor> vendedores = HibernateUtil.OpenSession(mapping).Query<Vendedor>()
          .Where(c => c.Cuit.ToString() == Cuit.ToString())
          .ToList();
      HibernateUtil.Dispose();
      return vendedores;
    }

    public IList<Vendedor> FindByDomicilio(string Domicilio)
    {
      throw new NotImplementedException();
    }

    public IList<Vendedor> FindByLocalidad(string Localidad)
    {
      throw new NotImplementedException();
    }

    public IList<Vendedor> FindByNombre(string Nombre)
    {
      throw new NotImplementedException();
    }

    public IList<Vendedor> FindByProvincia(string Provincia)
    {
      throw new NotImplementedException();
    }

    public IList<Vendedor> FindByStipprovee(int Stipprovee)
    {
      throw new NotImplementedException();
    }

    public IList<Vendedor> FindByTipodecuenta(string Tipodecuenta)
    {
      throw new NotImplementedException();
    }


    public IList<Vendedor> FindAll()
    {
      using (ISession session = HibernateUtil.OpenSession(mapping))
      {
        IList<Vendedor> vendedores = session.Query<Vendedor>()
            .ToList();
        HibernateUtil.Dispose();
        return vendedores;
      }
    }

    public IList<Vendedor> FindLikeCuentaLimit(int cuenta, int limit)
    {
      using (ISession session = HibernateUtil.OpenSession(mapping))
      {
        IList<Vendedor> mycuenta = session.Query<Vendedor>()
             .Where(c => c.Cuenta.ToString().StartsWith(cuenta.ToString()))
             .Take(limit)
             .ToList();
        HibernateUtil.Dispose();
        return mycuenta;
      }
    }

    public IList<Vendedor> FindLikeNombreLimit(string nombre, int limit)
    {
      using (ISession session = HibernateUtil.OpenSession(mapping))
      {
        IList<Vendedor> mycuenta = session.Query<Vendedor>()
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
        IList<ICuenta> mycuenta = session.Query<Vendedor>()
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
        IList<ICuenta> mycuenta = session.Query<Vendedor>()
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
        IList<ICuenta> mycuenta = session.Query<Vendedor>()
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
        IList<ICuenta> mycuenta = session.Query<Vendedor>()
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
        ICuenta mycuenta = session.Query<Vendedor>()
             .Where(c => c.Cuenta == cuenta)
             .FirstOrDefault();
        HibernateUtil.Dispose();
        return mycuenta;
      }
    }


    public IList<string> FindNombreLikeNombre(string value)
    {
      throw new NotImplementedException();
    }

    IList<ICuenta> ICuentaStore.FindLikeNombreLimit(string value, int limit)
    {
      using (ISession session = HibernateUtil.OpenSession(mapping))
      {
        IList<ICuenta> mycuenta = session.Query<Vendedor>()
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
        IList<ICuenta> mycuenta = session.Query<Vendedor>()
             .Where(c => c.Cuit.StartsWith(cuit))
             .Take(limit)
             .ToList<ICuenta>();
        HibernateUtil.Dispose();
        return mycuenta;
      }
    }

    public IList<ICuenta> FindByCuit(string cuit)
    {
      using (ISession session = HibernateUtil.OpenSession(mapping))
      {
        IList<ICuenta> mycuenta = session.Query<Vendedor>()
             .Where(c => c.Cuit.StartsWith(cuit))
             .ToList<ICuenta>();
        HibernateUtil.Dispose();
        return mycuenta;
      }
    }

    public bool IsInternal(long cuenta)
    {
      using (ISession session = HibernateUtil.OpenSession(mapping))
      {
        Vendedor mycuenta = session.Query<Vendedor>()
            .Where(c => c.Cuenta == cuenta)
            .FirstOrDefault();
        HibernateUtil.Dispose();
        if (mycuenta != null)
        {
          return (mycuenta.Interexter == 1);
        }
        return false;
      }
    }


    public IList<ICuenta> FindStartsWithCuentaLimit(long cuenta, ISession session, int limit)
    {
      IList<ICuenta> mycuenta = session.Query<Vendedor>()
               .Where(c => c.Cuenta.ToString().StartsWith(cuenta.ToString()))
               .Take(limit)
               .ToList<ICuenta>();
      return mycuenta;
    }


    public Vendedor FindByNroCuenta(long Cuenta, ISession Session)
    {
      Vendedor vendedor = Session.Query<Vendedor>()
              .Where(x => x.Cuenta == Cuenta)
              .FirstOrDefault();
      return vendedor;
    }


    public Vendedor FindById(long Id, ISession Session)
    {
      Vendedor vendedor = Session.Query<Vendedor>()
              .Where(x => x.Id == Id)
              .FirstOrDefault();
      return vendedor;
    }

    public IList<Vendedor> FindVendedorByCuit(string Cuit, ISession Session)
    {
      IList<Vendedor> vendedores = Session.Query<Vendedor>()
          .Where(c => c.Cuit.ToString() == Cuit.ToString())
          .ToList();
      return vendedores;
    }

    public ICuenta FindByCuit(long Cuit)
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

    public IList<ICuenta> FindByCuentas(IList<string> cuentas)
    {
      throw new NotImplementedException();
    }
  }
}