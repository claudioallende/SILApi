using NHibernate;
using NHibernate.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.DataAccess
{
    public class SessionContextProvider : ISessionProvider
    {
        public ISession GetSession(ISessionFactory Factory)
        {
            try
            {
                ISession session;
                if (CurrentSessionContext.HasBind(Factory))
                {
                    session = Factory.GetCurrentSession();
                    if (!session.IsOpen)
                    {
                        HibernateUtil.Dispose();
                        session = Factory.OpenSession();
                        CurrentSessionContext.Bind(session);
                    }
                }
                else
                {
                    session = Factory.OpenSession();
                    CurrentSessionContext.Bind(session);
                }
                return session;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public void DisposeSession(ISessionFactory Factory)
        {
            if (Factory != null && CurrentSessionContext.HasBind(Factory))
            {
                var session = CurrentSessionContext.Unbind(Factory);
                session.Dispose();
            }
        }
    }
}