using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.DataAccess
{
    public interface ICupoInStopAndNotSILStore
    {
        void Save(CupoInStopAndNotSIL c);
        void Save(CupoInStopAndNotSIL c, ISession session);
        void Save(CupoInStopAndNotSIL c, ISession session, ITransaction transaccion);
        void Update(Int64 Id, CupoInStopAndNotSIL c, ISession session);
        void Delete(Int64 Id);
        void Delete(CupoInStopAndNotSIL cupo, ISession session);
        void Delete(CupoInStopAndNotSIL cupo, ISession session, ITransaction transaccion);
        IList<CupoInStopAndNotSIL> FindByGranoPuertoCompradorVendedor(CupoInStopAndNotSIL compPuertoGrano, ISession session);
        IList<CupoInStopAndNotSIL> FindByFiltro(CupoInStopAndNotSIL compPuertoGrano, ISession session);
        IList<CupoInStopAndNotSIL> FindByFiltroAndACACorredorCORDestinatarioORCorredorV(CupoInStopAndNotSIL filtro, ISession session);
        CupoInStopAndNotSIL FindByAlfa(string alfa, ISession session);
        IList<CupoInStopAndNotSIL> FindByIds(IList<long> Ids, ISession session);
        IList<CupoInStopAndNotSIL> FindAll(ISession session);
        IList<CupoInStopAndNotSIL> FindAllWithACACorredorCORDestinatarioORCorredorV(ISession Session);
        
    }
}
