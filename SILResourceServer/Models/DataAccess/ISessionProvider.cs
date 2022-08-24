using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceServer.Models.DataAccess
{
    public interface ISessionProvider
    {
        ISession GetSession(ISessionFactory Factory);
        void DisposeSession(ISessionFactory Factory);
    }
}
