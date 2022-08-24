using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceServer.Models.DataAccess
{
    interface ICuposAgrupadosStore
    {
        void Save(CuposAgrupados c);
        void Update(int Id, CuposAgrupados c);
        void Delete(int Id);
        CuposAgrupados FindById(int Id);
        IList<CuposAgrupados> FindByFecha(string fecha);
        IList<CuposAgrupadosCyo> FindOnlyCyO();
        IList<CuposAgrupados> FindAll();
        IList<CuposAgrupados> FindByFechaAndGrano(string f, int g);
        IList<CuposAgrupados> FindByCentro(string Centro);
        IList<CuposAgrupados> FindByCentroAndGrano(string Centro, int Grano);
        IList<ICuposAgrupados> FindByFiltro(IndexCupoViewModel Filtro);
        IList<CuposAgrupadosCyo> FindByFechaCyO(string fecha);
        IList<CuposAgrupadosCyo> FindByFechaAndGranoCyO(string fecha, int grano);
        IList<CuposAgrupadosCyo> FindByCentroCyo(string Centro);
        IList<CuposAgrupadosCyo> FindByCentroAndGranoCyo(string Centro, int Grano);
        IList<ICuposAgrupados> FindByFiltroCyo(IndexCupoViewModel Filtro);
        IList<ICuposAgrupadosDetalle> FindByPuertoAndCompradorAndGranoGroupByPuertoAndComprador(long puerto, long comprador, int grano);
        IList<ICuposAgrupadosDetalle> FindByPuertoAndCompradorAndGranoAndCentroAndCentrodistGroupByPuertoAndComprador(long puerto, long comprador, int grano, string centro, string centrodist);
        IList<ICuposAgrupadosDetalle> FindByPuertoAndCompradorAndGranoAndVendedorGroupByPuertoAndComprador(long comprador, long vendedor, long puerto, int grano);
        IList<ICuposAgrupadosDetalle> FindByPuertoAndCompradorAndGranoGroupByPuertoAndCompradorAndCyO(long comprador, long puerto, int grano);
        IList<ICuposAgrupadosDetalle> FindByPuertoAndCompradorAndGranoAndCentroAndCentrodistGroupByPuertoAndCompradorAndCyO(long comprador, long puerto, int grano, string centro, string centrodist);
        IList<ICuposAgrupados> FindByFiltro(IndexCupoViewModel Filtro, string Vendedor);
        IList<ICuposAgrupados> FindByFiltroCyo(IndexCupoViewModel Filtro, string Vendedor);
    }
}
