using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.DataAccess
{
    public class CentroStore : ICentroStore
    {

        public void Save(Centro c)
        {
            throw new NotImplementedException();
        }

        public void Update(string Id, Centro c)
        {
            throw new NotImplementedException();
        }

        public void Delete(string Id)
        {
            throw new NotImplementedException();
        }

        public Centro FindById(string Id)
        {
            Centro centro = HibernateUtil.OpenSession("Centro.mpg.xml").Query<Centro>()
                .Where(c => c.Id == Id)
                .FirstOrDefault();
            HibernateUtil.Dispose();
            return centro;
        }

        public IList<Centro> FindByIds(IList<string> Ids)
        {
            IList<Centro> centros = HibernateUtil.OpenSession().Query<Centro>()
                .Where(c => Ids.Contains(c.Id))
                .ToList();
            HibernateUtil.Dispose();
            return centros;
        }

        public IList<Centro> FindByCentro(string centro)
        {
            IList<Centro> centros = HibernateUtil.OpenSession("Centro.mpg.xml").Query<Centro>()
                .Where(c => c.CodigoCentro == centro)
                .ToList();
            HibernateUtil.Dispose();
            return centros;
        }

        public IList<Centro> FindByCentro(string centro, ISession session)
        {
            IList<Centro> centros = session.Query<Centro>()
                .Where(c => c.CodigoCentro == centro)
                .ToList();
            return centros;
        }

        public IList<Centro> FindByNombre(string nombre)
        {
            IList<Centro> centros = HibernateUtil.OpenSession("Centro.mpg.xml").Query<Centro>()
                .Where(c => c.Nombre == nombre)
                .ToList();
            HibernateUtil.Dispose();
            return centros;
        }
        
        public IList<Centro> FindAll(ISession session = null)
        {
            IList<Centro> centros = new List<Centro>();
            if (session == null)
            {
                centros = HibernateUtil.OpenSession("Centro.mpg.xml").Query<Centro>()
                    .Where(x => ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(x.CodigoCentro))
                    .ToList();
                HibernateUtil.Dispose();
            }
            else {
                centros = session.Query<Centro>()
                    .Where(x => ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(x.CodigoCentro))
                    .ToList();
            }
            return centros;
        }

        public IList<Centro> FindAllCenter(ISession session = null)
        {
            IList<Centro> centros = new List<Centro>();
            if (session == null)
            {
                centros = HibernateUtil.OpenSession("Centro.mpg.xml").Query<Centro>()
                    .ToList();
                HibernateUtil.Dispose();
            }
            else
            {
                centros = session.Query<Centro>()
                    .ToList();
            }
            return centros;
        }
    }
}