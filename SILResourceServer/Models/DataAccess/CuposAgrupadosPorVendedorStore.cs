using NHibernate;
using ResourceServer.Models.Filtro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ResourceServer.Models.DataAccess
{
    public class CuposAgrupadosPorVendedorStore : ICuposAgrupadosPorVendedorStore
    {
        public IList<CuposAgrupadosPorVendedor> FindAllByVendedor(long VENDCTA, ISession session)
        {
            IList<CuposAgrupadosPorVendedor> cupos = session.Query<CuposAgrupadosPorVendedor>()
                    .Where(x => x.VENDCTA == VENDCTA)
                    .ToList();            
            return cupos;
        }

        public IList<CuposAgrupadosPorVendedor> FindAll(ISession session)
        {
            List<CuposAgrupadosPorVendedor> Result = new List<CuposAgrupadosPorVendedor>();
            Result = session.Query<CuposAgrupadosPorVendedor>()                    
                    .ToList();
            return Result;
        }

        public IList<CuposAgrupadosPorVendedor> FindInforma2StopByVendedor(long VENDCTA, ISession session)
        {
            IList<CuposAgrupadosPorVendedor> cupos = session.Query<CuposAgrupadosPorVendedor>()
                   .Where(x => x.VENDCTA == VENDCTA
                          && x.INFORMADOSTOP==1)
                   .ToList();
            return cupos;
        }

        public IList<CuposAgrupadosPorVendedor> FindNoInforma2StopByVendedor(long VENDCTA, ISession session)
        {
            IList<CuposAgrupadosPorVendedor> cupos = session.Query<CuposAgrupadosPorVendedor>()
                   .Where(x => x.VENDCTA == VENDCTA
                          && x.INFORMADOSTOP == 0)
                   .ToList();
            return cupos;
        }

        public IList<CuposAgrupadosPorVendedor> FindByCuitOrCuentaAndInformadoStop(IList<long> CuentasVendedor, bool InformadoStop, ISession session)
        {
            var query = session.Query<CuposAgrupadosPorVendedor>()
                   .Where(x => CuentasVendedor.Contains(x.VENDCTA));
            if (InformadoStop) query = query.Where(x => x.INFORMADOSTOP == 1);
            else query = query.Where(x => x.INFORMADOSTOP == 0);
            IList<CuposAgrupadosPorVendedor> cupos = query.ToList();
            return cupos;
        }

        public IList<CuposAgrupadosPorVendedor> FindByCuitOrCuentaAndInformadoStopAndCyo(IList<long> CuentasVendedor, bool InformadoStop, bool EsCyo, ISession session, GranoCompradorPuerto filtro = null)
        {
            var query = session.Query<CuposAgrupadosPorVendedor>()
                   .Where(x => CuentasVendedor.Contains(x.VENDCTA) &&
                   x.INFORMADOSTOP == (InformadoStop ? 1 : 0) &&
                   x.CYO == (EsCyo ? 1 : 0));
            return query.ToList();
        }
    }
}