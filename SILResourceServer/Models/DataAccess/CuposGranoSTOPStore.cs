using NHibernate;
using ResourceServer.Models.Error.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.DataAccess
{
    public class CuposGranoSTOPStore : ICuposGranoSTOPStore
    {
        public CuposGranoSTOP Save(CuposGranoSTOP c, ISession session)
        {
            if (c != null)
            {
                using (ITransaction tx = session.BeginTransaction())
                {
                    try
                    {
                        session.Save(c);
                        tx.Commit();
                        return this.FindByNroGrano(c.NroGrano, session);                        
                    }
                    catch (NHibernate.Exceptions.GenericADOException e)
                    {
                        if (e.GetBaseException().Message.Contains("ORA-00001"))
                        {
                            tx.Rollback();
                            HibernateUtil.Dispose();
                            throw new RegistroDuplicadoException("El Grano");
                        }
                        else
                        {
                            tx.Rollback();
                            HibernateUtil.Dispose();
                            throw e;
                        }
                    }
                    catch (Exception e)
                    {
                        tx.Rollback();
                        HibernateUtil.Dispose();
                        throw e;
                    }
                }
            }
            return null;
        }

        public CuposGranoSTOP Update(CuposGranoSTOP c, ISession session)
        {
            if (c != null)
            {
                using (ITransaction tx = session.BeginTransaction())
                {
                    try
                    {
                        session.Update(c, c.NroGrano);
                        tx.Commit();
                        return this.FindByNroGrano(c.NroGrano,session);
                    }
                    catch (NHibernate.Exceptions.GenericADOException e)
                    {
                        if (e.GetBaseException().Message.Contains("ORA-00001"))
                        {
                            tx.Rollback();
                            HibernateUtil.Dispose();
                            throw new RegistroDuplicadoException("El Grano");
                        }
                        else
                        {
                            tx.Rollback();
                            HibernateUtil.Dispose();
                            throw e;
                        }
                    }
                    catch (Exception e)
                    {
                        tx.Rollback();
                        HibernateUtil.Dispose();
                        throw e;
                    }
                }
            }
            return null;
        }

        public bool Delete(CuposGranoSTOP GranoSTOP, ISession session, ITransaction tx1 = null)
        {
            if (GranoSTOP != null) 
            {
                ITransaction tx = (tx1 != null)? tx1 : session.BeginTransaction();
                try
                {
                    session.Delete(GranoSTOP);
                    if (tx1 == null) 
                    {
                        tx.Commit();
                    } 
                }
                catch (Exception e)
                {
                    tx.Rollback();
                    throw e;
                }
                return true;
            }
            return false;
        }

        public IList<CuposGranoSTOP> FindAll(ISession session)
        {
            IList<CuposGranoSTOP> todoLosGranosDeSTOP = session.Query<CuposGranoSTOP>()
                .ToList();
            return todoLosGranosDeSTOP;
        }

        public IList<CuposGranoSTOP> FindAll()
        {
            using (ISession session = HibernateUtil.OpenSession())
            {
                IList<CuposGranoSTOP> granos = session.Query<CuposGranoSTOP>()
                    .ToList();
                HibernateUtil.Dispose();
                return granos;
            }
        }
        

        public CuposGranoSTOP FindByNroGrano(long NroGrano, ISession session)
        {
            CuposGranoSTOP granoSTOP = session.Query<CuposGranoSTOP>()
               .Where(c => c.NroGrano == NroGrano)
               .FirstOrDefault();
            return granoSTOP;
        }
    }
}