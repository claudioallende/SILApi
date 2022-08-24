using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceServer.Models.DataAccess
{
    interface ICuposAgrupadosPorVendedorStore
    {
        IList<CuposAgrupadosPorVendedor> FindAllByVendedor(Int64 VENDCTA, ISession session);
        IList<CuposAgrupadosPorVendedor> FindInforma2StopByVendedor(Int64 VENDCTA, ISession session);
        IList<CuposAgrupadosPorVendedor> FindNoInforma2StopByVendedor(Int64 VENDCTA, ISession session);
        IList<CuposAgrupadosPorVendedor> FindAll(ISession session);
    }
}
