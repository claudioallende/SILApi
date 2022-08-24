using NHibernate;
using ResourceServer.Models.DataAccess;
using ResourceServer.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
  public class ServicioRelacionPuertoSILPuertoSTOP
  {
    private ISession Session;
    private RelacionPuertoSILPuertoSTOPStore RelationStore { get; set; }

    public ServicioRelacionPuertoSILPuertoSTOP()
    {
      this.RelationStore = new RelacionPuertoSILPuertoSTOPStore();
      this.Session = HibernateUtil.OpenSession("");
    }

    public List<RelacionPuertoSILPuertoSTOP> GetRelaciones(RelacionPuertoSILPuertoSTOPDTO relacion = null)
    {
      List<RelacionPuertoSILPuertoSTOP> listResult = new List<RelacionPuertoSILPuertoSTOP>();

      if (relacion != null)
      {
        if (relacion.NroPuertoSIL > 0 && relacion.NroPuertoSTOP > 0)
        {
          listResult.Add(this.RelationStore.FindCompleteRelationByPuertoSILAndPuertoSTOP(relacion.NroPuertoSIL, relacion.NroPuertoSTOP, Session));
        }
        else if (relacion.NroPuertoSIL > 0)
        {
          listResult.AddRange(this.RelationStore.FindCompleteRelationByPuertoSIL(relacion.NroPuertoSIL, Session));
        } if (relacion.NroPuertoSTOP > 0)
        {                                       
          listResult.AddRange(this.RelationStore.FindCompleteRelationByPuertoSTOP(relacion.NroPuertoSTOP, Session));
        }
      }
      else
      {
        listResult.AddRange(this.RelationStore.FindAllRelations(Session));
      }
      return listResult;
    }

    public RelacionPuertoSILPuertoSTOP AgregarRelacion(RelacionPuertoSILPuertoSTOPDTO nuevaRelacion)
    {

      if (nuevaRelacion != null)
      {
        return this.RelationStore.Save(nuevaRelacion.ToRelacionPuertoSILPuertoSTOP(), Session);
      }
      return null;
    }

    public RelacionPuertoSILPuertoSTOP ModificarRelacion(RelacionPuertoSILPuertoSTOPDTO relacion)
    {
      if (relacion != null)
      {
        return RelationStore.UpdateRelacion(relacion.ToRelacionPuertoSILPuertoSTOP(), Session);
      }
      return null;
    }

    public bool EliminarRelacion(RelacionPuertoSILPuertoSTOPDTO relacion)
    {
      if (relacion != null)
      {
        return this.RelationStore.Delete(relacion.ToRelacionPuertoSILPuertoSTOP(), Session);
      }
      return false;
    }

  }
}