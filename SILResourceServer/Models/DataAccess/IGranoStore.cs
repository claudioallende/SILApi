using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceServer.Models.DataAccess
{
    interface IGranoStore
    {
        void Save(Grano c);
        void Update(string Id, Grano c);
        void Delete(string Id);
        Grano FindById(string Id);
        Grano FindById(string Id, ISession Session);
        IList<Grano> FindAll();
        IList<Grano> FindAll(ISession Session);
        IList<Grano> FindByGrano(int grano);
        IList<Grano> FindByNombre(string nombre);
    }
}
