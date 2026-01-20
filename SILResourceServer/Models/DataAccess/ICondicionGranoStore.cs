using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceServer.Models.DataAccess
{
  public interface ICondicionGranoStore
  {
    void Save(CondicionGrano c);
    void Update(string Id, CondicionGrano c);
    void Delete(string Id);
    CondicionGrano FindById(string Id);
    CondicionGrano FindById(string Id, ISession Session);
    IList<CondicionGrano> FindAll();
    IList<CondicionGrano> FindAll(ISession Session);
    IList<CondicionGrano> FindByNombre(string nombre);
  }
}
