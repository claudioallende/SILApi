using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.DataAccess
{
    public class GranoStore : IGranoStore
    {
        private string mapping = "Grano.mpg.xml";

        public void Save(Grano c)
        {
            throw new NotImplementedException();
        }

        public void Update(string Id, Grano c)
        {
            throw new NotImplementedException();
        }

        public void Delete(string Id)
        {
            throw new NotImplementedException();
        }

        public Grano FindById(string Id)
        {
            Grano grano = HibernateUtil.OpenSession(mapping).Query<Grano>()
                .Where(c => c.Id == Id)
                .FirstOrDefault();
            HibernateUtil.Dispose();
            return grano;
        }

        public IList<Grano> FindByIds(IList<string> Ids)
        {
            IList<Grano> granos = HibernateUtil.OpenSession(mapping).Query<Grano>()
                .Where(c => Ids.Contains(c.Id))
                .ToList();
            HibernateUtil.Dispose();
            return granos;
        }

        public IList<Grano> FindByGrano(int grano)
        {
            IList<Grano> granos = HibernateUtil.OpenSession(mapping).Query<Grano>()
                .Where(c => c.CodigoGrano == grano)
                .ToList();
            HibernateUtil.Dispose();
            return granos;
        }

        public IList<Grano> FindByNombre(string nombre)
        {
            IList<Grano> granos = HibernateUtil.OpenSession(mapping).Query<Grano>()
                .Where(c => c.Nombre == nombre)
                .ToList();
            HibernateUtil.Dispose();
            return granos;
        }


        public IList<Grano> FindAll()
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<Grano> granos = session.Query<Grano>()
                    .ToList();
                HibernateUtil.Dispose();
                return granos;
            }
        }

        public IList<Grano> FindAll(ISession Session)
        {
            IList<Grano> granos = Session.Query<Grano>()
                .ToList();
            return granos;
        }

        public Grano FindById(string Id, ISession Session)
        {
            Grano grano = Session.Query<Grano>()
                .Where(c => c.Id == Id)
                .FirstOrDefault();
            return grano;
        }
    }
}