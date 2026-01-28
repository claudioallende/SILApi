using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.DataAccess
{
  public class CondicionGranoStore : ICondicionGranoStore
  {
    private string mapping = "CondicionGrano.mpg.xml";

    public void Delete(long Id)
    {
      throw new NotImplementedException();
    }

    public IList<CondicionGrano> FindAll()
    {
      using (ISession session = HibernateUtil.OpenSession(mapping))
      {
        var count = session.CreateQuery("select count(*) from CondicionGrano")
                   .UniqueResult<long>();

        IList<CondicionGrano> condicionesGrano = session.Query<CondicionGrano>()
            .ToList();
        return condicionesGrano;
      }
    }

    public IList<CondicionGrano> FindAll(ISession Session)
    {
      IList<CondicionGrano> condicionesGrano = Session.Query<CondicionGrano>()
        .ToList();
      return condicionesGrano;
    }

    public CondicionGrano FindById(long Id)
    {
      CondicionGrano condicionGrano = HibernateUtil.OpenSession(mapping).Query<CondicionGrano>()
          .Where(c => c.Id == Id)
          .FirstOrDefault();
        HibernateUtil.Dispose();
        return condicionGrano;
    }

    public CondicionGrano FindById(long Id, ISession Session)
    {
      CondicionGrano condicionGrano = Session.Query<CondicionGrano>()
        .Where(c => c.Id == Id)
        .FirstOrDefault();
      return condicionGrano;
    }

    public IList<CondicionGrano> FindByNombre(string nombre)
    {
      IList<CondicionGrano> granos = HibernateUtil.OpenSession(mapping).Query<CondicionGrano>()
        .Where(c => c.Nombre == nombre)
        .ToList();
      HibernateUtil.Dispose();
      return granos;
    }

    public void Save(CondicionGrano c)
    {
      throw new NotImplementedException();
    }

    public void Update(long Id, CondicionGrano c)
    {
      throw new NotImplementedException();
    }
  }
}