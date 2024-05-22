using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceServer.Models.DataAccess
{
  public interface ICuentaStore
  {
    IList<ICuenta> GetAll();
    IList<ICuenta> FindStartsWithCuentaLimit(long cuenta, int limit);
    IList<ICuenta> FindStartsWithCuentaLimitCC(long cuenta, int limit);
    IList<ICuenta> FindStartsWithCuentaLimit(long cuenta, ISession session, int limit);
    IList<ICuenta> FindStartsWithIgnoreCaseNombreLimit(string nombre, int limit);
    IList<ICuenta> FindLikeCuentaLimit(long cuenta, int limit);
    IList<ICuenta> FindLikeIgnoreCaseNombreLimit(string nombre, int limit);
    IList<ICuenta> FindLikeIgnoreCaseNombreLimitCC(string nombre, int limit);
    ICuenta FindNombreAndCuitByCuenta(long cuenta);
    IList<string> FindNombreLikeNombre(string value);
    IList<ICuenta> FindLikeNombreLimit(string value, int limit);
    IList<ICuenta> FindByCuitLimit(string cuit, int limit);
    IList<ICuenta> FindByCuit(string cuit);
    IList<ICuenta> FindByCuits(IList<string> cuits);
    IList<ICuenta> FindByCuentas(IList<string> cuentas);
  }
}
