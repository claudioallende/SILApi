using NHibernate;
using System.Collections.Generic;

namespace ResourceServer.Models.DataAccess
{
  interface IRelacionPuertoSILPuertoSTOPStore
  {
    RelacionPuertoSILPuertoSTOP Save(RelacionPuertoSILPuertoSTOP nuevaRelacion, ISession session);
    RelacionPuertoSILPuertoSTOP UpdateRelacion(RelacionPuertoSILPuertoSTOP relacion, ISession session, ITransaction transaccion = null);
    void Update(RelacionPuertoSILPuertoSTOP relacion, ISession session, ITransaction transaccion = null);
    bool Delete(RelacionPuertoSILPuertoSTOP relacion, ISession session);
    bool DeleteAllRelationOfPuertoSTOP(long NroPuertoSTOP, ISession session, ITransaction tx1 = null);
    IList<RelacionPuertoSILPuertoSTOP> FindAllRelations(ISession session);
    IList<RelacionPuertoSILPuertoSTOP> FindCompleteRelationByPuertoSIL(long NroPuerto, ISession session);
    IList<RelacionPuertoSILPuertoSTOP> FindCompleteRelationByPuertoSTOP(long NroPuerto, ISession session);
    RelacionPuertoSILPuertoSTOP FindCompleteRelationByPuertoSILAndPuertoSTOP(long NroPuertoSIL, long nroPuertoSTOP, ISession session);
    RelacionPuertoSILPuertoSTOP FindCompleteRelationById(int id, ISession session);
  }
}
