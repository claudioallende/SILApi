using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceServer.Models.DataAccess
{
    interface ICentroStore
    {
        void Save(Centro c);
        void Update(string Id, Centro c);
        void Delete(string Id);
        Centro FindById(string Id);
        IList<Centro> FindAll(ISession session = null);
        IList<Centro> FindAllCenter(ISession session = null);
        IList<Centro> FindByCentro(string centro);
        IList<Centro> FindByCentro(string centro, ISession session);
        IList<Centro> FindByNombre(string nombre);
    }
}
