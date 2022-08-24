using NHibernate;
using ResourceServer.Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
  public class ServicioPuertoStop
  {

    private CuposPuertoSTOPStore CPStore { get; set; }
    private ISession Session;

    public ServicioPuertoStop()
    {
      this.CPStore = new CuposPuertoSTOPStore();
      this.Session = HibernateUtil.OpenSession("");
    }

    public List<CuposPuertoSTOP> GetPuertos(long nroPuerto = 0)
    {
      List<CuposPuertoSTOP> listResult = new List<CuposPuertoSTOP>();
      listResult.AddRange((nroPuerto != 0) ? new List<CuposPuertoSTOP>() { this.CPStore.FindByNroPuerto(nroPuerto, Session) } : this.CPStore.FindAll(Session));
      return listResult;
    }

    public CuposPuertoSTOP AddPuerto(CuposPuertoSTOP puertoIn)
    {
      if (puertoIn != null)
      {
        return this.CPStore.Save(puertoIn, Session);
      }
      return null;
    }

    public CuposPuertoSTOP UpdatePuerto(CuposPuertoSTOP puertoIn)
    {
      if (puertoIn != null)
      {
        return this.CPStore.Update(puertoIn, Session);
      }
      return null;
    }

    public bool EliminarPuerto(CuposPuertoSTOP puertoIn)
    {
      bool result = false;
      if (puertoIn != null)
      {
        ITransaction tx = Session.BeginTransaction();
        try
        {
          RelacionPuertoSILPuertoSTOPStore relationStore = new RelacionPuertoSILPuertoSTOPStore();
          if (relationStore.DeleteAllRelationOfPuertoSTOP(puertoIn.NroPuerto, Session, tx))
          {
            result = this.CPStore.Delete(puertoIn, Session, tx);
          }
        }
        catch (Exception e)
        {
          tx.Rollback();
          throw e;
        }
        return result;
      }
      return false;
    }



  }
}