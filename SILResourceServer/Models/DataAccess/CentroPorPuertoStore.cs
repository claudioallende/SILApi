using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.DataAccess
{
    public class CentroPorPuertoStore : ICentroPorPuertoStore
    {

        public long Save(CuposCentroPorPuerto c, ISession session)
        {
            if (c != null)
            {
                CuposCentroPorPuerto relacioEnBD = this.FindByPuertoAndCentro(c.IdTerminal, c.CodigoCentro, session).FirstOrDefault();
                if (relacioEnBD != null)
                {
                    return 0;
                }
                else
                {
                    try
                    {
                        session.Save(c);
                        return c.Id;
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
            return 0;
        }

        public CuposCentroPorPuerto Update(CuposCentroPorPuerto c, ISession session)
        {
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {
                    session.Update(c, c.Id);
                    tx.Commit();
                    return this.FindById(c.Id, session);
                }
                catch (Exception e)
                {
                    tx.Rollback();
                    throw e;
                }
            }
        }

        public void Delete(CuposCentroPorPuerto PuertoCentro , ISession session)
        {
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {
                    session.Delete(PuertoCentro);
                    tx.Commit();
                }
                catch (Exception e)
                {
                    tx.Rollback();
                    throw e;
                }
            }
        }

        public CuposCentroPorPuerto FindById(long Id, ISession session)
        {
            CuposCentroPorPuerto elCentro = session.Query<CuposCentroPorPuerto>()
                .Where(c => c.Id == Id)
                .FirstOrDefault();
            return elCentro;
        }

        public IList<CuposCentroPorPuerto> FindAll(ISession session)
        {
            IList<CuposCentroPorPuerto> elCentro = session.Query<CuposCentroPorPuerto>()
                    .OrderBy(d=> d.CodigoCentro)
                    .ToList();
            return elCentro;
        }

        public IList<CuposCentroPorPuerto> FindByCentro(string CodigoCentro, ISession session)
        {
            IList<CuposCentroPorPuerto> elCentro = session.Query<CuposCentroPorPuerto>()
                .Where(c => c.CodigoCentro== CodigoCentro)
                .ToList();
            return elCentro;
        }

        public IList<CuposCentroPorPuerto> FindByPuerto(long IdTerminal, ISession session)
        {
            IList<CuposCentroPorPuerto> elCentro = session.Query<CuposCentroPorPuerto>()
                .Where(c => c.IdTerminal == IdTerminal)
                .ToList();
            return elCentro;
        }

        public IList<CuposCentroPorPuerto> FindByPuertoAndCentro(long IdTerminal, string codigoCentro, ISession session)
        {
            IList<CuposCentroPorPuerto> elCentro = session.Query<CuposCentroPorPuerto>()
                .Where(c =>     
                    c.IdTerminal == IdTerminal
                    && c.CodigoCentro == codigoCentro)
                    .OrderBy(d=> d.CodigoCentro)
                .ToList();
            return elCentro;
        }
    }
}