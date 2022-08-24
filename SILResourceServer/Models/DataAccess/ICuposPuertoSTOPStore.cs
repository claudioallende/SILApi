using NHibernate;
using System.Collections.Generic;

namespace ResourceServer.Models.DataAccess
{
  interface ICuposPuertoSTOPStore
  {
    CuposPuertoSTOP Save(CuposPuertoSTOP c, ISession session);
    CuposPuertoSTOP Update(CuposPuertoSTOP c, ISession session);
    bool Delete(CuposPuertoSTOP GranoSTOP, ISession session, ITransaction tx1 = null);
    IList<CuposPuertoSTOP> FindAll(ISession session = null);
    CuposPuertoSTOP FindByNroPuerto(long NroGrano, ISession session);
  }
}
