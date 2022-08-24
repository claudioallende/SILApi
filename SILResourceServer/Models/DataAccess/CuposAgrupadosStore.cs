using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.DataAccess
{
    public class CuposAgrupadosStore : ICuposAgrupadosStore
    {
        private string mapping = "CuposAgrupAll.mpg.xml";
        private string mappingCyO = "CuposAgrupCyO.mpg.xml";
        private string mappingCompPuerto = "CuposAgrupCompPuerto.mpg.xml";
        private string mappingCompPuertoCyO = "CuposAgrupCompPuertoCyO.mpg.xml";

        public void Save(CuposAgrupados c)
        {
            throw new NotImplementedException();
        }

        public void Update(int Id, CuposAgrupados c)
        {
            throw new NotImplementedException();
        }

        public void Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public CuposAgrupados FindById(int Id)
        {
            throw new NotImplementedException();
        }

        public IList<CuposAgrupados> FindAll()
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<CuposAgrupados> cuposAgrupAll = session.Query<CuposAgrupados>()
                    .ToList();
                HibernateUtil.Dispose();
                return cuposAgrupAll;
            }
        }

        public IList<CuposAgrupadosCyo> FindOnlyCyO()
        {
            using (ISession session = HibernateUtil.OpenSession(mappingCyO))
            {
                IList<CuposAgrupadosCyo> cuposAgrupAll = session.Query<CuposAgrupadosCyo>()
                    .ToList();
                HibernateUtil.Dispose();
                return cuposAgrupAll;
            }
        }


        public IList<CuposAgrupados> FindByFecha(string fecha)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<CuposAgrupados> cuposAgrupAll = session.Query<CuposAgrupados>()
                    .Where(f => f.Hoy == fecha)
                    .ToList();
                HibernateUtil.Dispose();
                return cuposAgrupAll;
            }
        }


        public IList<CuposAgrupados> FindByFechaAndGrano(string fecha, int grano)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<CuposAgrupados> cuposAgrupAll = session.Query<CuposAgrupados>()
                    .Where(f => f.Hoy == fecha && f.Grano == grano)
                    .ToList();
                HibernateUtil.Dispose();
                return cuposAgrupAll;
            }
        }


        public IList<CuposAgrupadosCyo> FindByFechaCyO(string fecha)
        {
            using (ISession session = HibernateUtil.OpenSession(mappingCyO))
            {
                IList<CuposAgrupadosCyo> cuposAgrupAll = session.Query<CuposAgrupadosCyo>()
                    .Where(f => f.Hoy == fecha)
                    .ToList();
                HibernateUtil.Dispose();
                return cuposAgrupAll;
            }
        }


        public IList<CuposAgrupadosCyo> FindByFechaAndGranoCyO(string fecha, int grano)
        {
            using (ISession session = HibernateUtil.OpenSession(mappingCyO))
            {
                IList<CuposAgrupadosCyo> cuposAgrupAll = session.Query<CuposAgrupadosCyo>()
                    .Where(f => f.Hoy == fecha && f.Grano == grano)
                    .ToList();
                HibernateUtil.Dispose();
                return cuposAgrupAll;
            }
        }

        public IList<ICuposAgrupadosDetalle> FindByPuertoAndCompradorAndGranoGroupByPuertoAndComprador(long puerto, long comprador, int grano)
        {
            using (ISession session = HibernateUtil.OpenSession(mappingCompPuerto))
            {
                IList<ICuposAgrupadosDetalle> cuposAgrupAll = session.Query<CuposAgrupadosDetalle>()
                    .Where(f => f.Compcta == comprador && f.PuertoCta == puerto && f.Grano == grano &&
                        (ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(f.CodCentroDist) ||
                        ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(f.CodCentro)))
                    .ToList<ICuposAgrupadosDetalle>();
                HibernateUtil.Dispose();
                return cuposAgrupAll;
            } 
        }

        public IList<ICuposAgrupadosDetalle> FindByPuertoAndCompradorAndGranoAndCentroAndCentrodistGroupByPuertoAndComprador(long puerto, long comprador, int grano, string centro, string centrodist)
        {
            using (ISession session = HibernateUtil.OpenSession(mappingCompPuerto))
            {
                IList<ICuposAgrupadosDetalle> cuposAgrupAll = session.Query<CuposAgrupadosDetalle>()
                    .Where(f => f.Compcta == comprador && f.PuertoCta == puerto && f.Grano == grano && centro == f.CodCentro && centrodist == f.CodCentroDist &&
                        (ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(f.CodCentroDist) ||
                        ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(f.CodCentro)))
                    .ToList<ICuposAgrupadosDetalle>();
                HibernateUtil.Dispose();
                return cuposAgrupAll;
            }
        }

        public IList<ICuposAgrupadosDetalle> FindByPuertoAndCompradorAndGranoGroupByPuertoAndCompradorAndCyO(long comprador, long puerto, int grano)
        {
            using (ISession session = HibernateUtil.OpenSession(mappingCompPuertoCyO))
            {
                IList<ICuposAgrupadosDetalle> cuposAgrupAll = session.Query<CuposAgrupadosDetalleCyo>()
                    .Where(f => f.Compcta == comprador && f.PuertoCta == puerto && f.Grano == grano &&
                        (ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(f.CodCentroDist) ||
                        ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(f.CodCentro)))
                    .ToList<ICuposAgrupadosDetalle>();
                HibernateUtil.Dispose();
                return cuposAgrupAll;
            }
        }

        public IList<ICuposAgrupadosDetalle> FindByPuertoAndCompradorAndGranoAndCentroAndCentrodistGroupByPuertoAndCompradorAndCyO(long comprador, long puerto, int grano, string centro, string centrodist)
        {
            using (ISession session = HibernateUtil.OpenSession(mappingCompPuertoCyO))
            {
                IList<ICuposAgrupadosDetalle> cuposAgrupAll = session.Query<CuposAgrupadosDetalleCyo>()
                    .Where(f => f.Compcta == comprador && f.PuertoCta == puerto && f.Grano == grano && centro == f.CodCentro && centrodist == f.CodCentroDist &&
                        (ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(f.CodCentroDist) ||
                        ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(f.CodCentro)))
                    .ToList<ICuposAgrupadosDetalle>();
                HibernateUtil.Dispose();
                return cuposAgrupAll;
            }
        }

        public IList<ICuposAgrupadosDetalle> FindByPuertoAndCompradorAndGranoAndVendedorGroupByPuertoAndComprador(long comprador, long vendedor, long puerto, int grano)
        {
            using (ISession session = HibernateUtil.OpenSession(mappingCompPuerto))
            {
                IList<ICuposAgrupadosDetalle> cuposAgrupAll = session.Query<CuposAgrupadosDetalle>()
                    .Where(f => f.Compcta == comprador && f.PuertoCta == puerto && f.Grano == grano && f.VendCta == vendedor &&
                        (ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(f.CodCentroDist) ||
                        ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(f.CodCentro)))
                    .ToList<ICuposAgrupadosDetalle>();
                HibernateUtil.Dispose();
                return cuposAgrupAll;
            }
        }

        public IList<CuposAgrupados> FindByCentro(string Centro)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<CuposAgrupados> cuposAgrupAll = session.Query<CuposAgrupados>()
                    .Where(f => f.CodCentro == Centro)
                    .ToList();
                HibernateUtil.Dispose();
                return cuposAgrupAll;
            }
        }

        public IList<CuposAgrupados> FindByCentroAndGrano(string Centro, int Grano)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<CuposAgrupados> cuposAgrupAll = session.Query<CuposAgrupados>()
                    .Where(f => f.CodCentro == Centro && f.Grano == Grano)
                    .ToList();
                HibernateUtil.Dispose();
                return cuposAgrupAll;
            }
        }

        public IList<CuposAgrupadosCyo> FindByCentroCyo(string Centro)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<CuposAgrupadosCyo> cuposAgrupAll = session.Query<CuposAgrupadosCyo>()
                    .Where(f => f.CodCentro == Centro)
                    .ToList();
                HibernateUtil.Dispose();
                return cuposAgrupAll;
            }
        }

        public IList<CuposAgrupadosCyo> FindByCentroAndGranoCyo(string Centro, int Grano)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<CuposAgrupadosCyo> cuposAgrupAll = session.Query<CuposAgrupadosCyo>()
                    .Where(f => f.CodCentro == Centro && f.Grano == Grano)
                    .ToList();
                HibernateUtil.Dispose();
                return cuposAgrupAll;
            }
        }

        //Este mismo pero cambiar session.Query<CuposAgrupados>() por session.Query<CuposAgrupadosDetalle>() para buscar por vendedor
        public IList<ICuposAgrupados> FindByFiltro(IndexCupoViewModel Filtro)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                var cupos = session.Query<CuposAgrupados>()
                    .Where(f =>
                        (Filtro.ProductoSeleccionado == 0 || f.Grano == Filtro.ProductoSeleccionado) &&
                        (ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(f.CodCentroDist) ||
                        ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(f.CodCentro))
                    );
                if (!(Filtro.CentroSeleccionado == null || Filtro.CentroSeleccionado.Count == 0))
                {
                    cupos = cupos.Where(f => Filtro.CentroSeleccionado.Contains(f.CodCentro) || Filtro.CentroSeleccionado.Contains(f.CodCentroDist));
                }
                if (!string.IsNullOrEmpty(Filtro.Comprador))
                {
                    cupos = cupos.Where(f => Filtro.Comprador == f.Compcta.ToString());
                }
                IList<ICuposAgrupados> cuposAgrupAll = cupos.ToList<ICuposAgrupados>();
                HibernateUtil.Dispose();
                return cuposAgrupAll;
            }
        }

        public IList<ICuposAgrupados> FindByFiltroCyo(IndexCupoViewModel Filtro)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                var cupos = session.Query<CuposAgrupadosCyo>()
                    .Where(f =>
                        (Filtro.ProductoSeleccionado == 0 || f.Grano == Filtro.ProductoSeleccionado) &&
                        (ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(f.CodCentroDist) ||
                        ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(f.CodCentro))
                    );
                if (!(Filtro.CentroSeleccionado == null || Filtro.CentroSeleccionado.Count == 0))
                {
                    cupos = cupos.Where(f => Filtro.CentroSeleccionado.Contains(f.CodCentro) || Filtro.CentroSeleccionado.Contains(f.CodCentroDist));
                }
                if (!string.IsNullOrEmpty(Filtro.Comprador))
                {
                    cupos = cupos.Where(f => Filtro.Comprador == f.Compcta.ToString());
                }
                IList<ICuposAgrupados> cuposAgrupAll = cupos.ToList<ICuposAgrupados>();
                HibernateUtil.Dispose();
                return cuposAgrupAll;
            }
        }

        public IList<ICuposAgrupados> FindByFiltro(IndexCupoViewModel Filtro, string Vendedor)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                var cupos = session.Query<CuposAgrupadosDetalle>()
                    .Where(f =>
                        (Filtro.ProductoSeleccionado == 0 || f.Grano == Filtro.ProductoSeleccionado) &&
                        (ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(f.CodCentroDist) ||
                        ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(f.CodCentro))
                    );
                if (!(Filtro.CentroSeleccionado == null || Filtro.CentroSeleccionado.Count == 0))
                {
                    cupos = cupos.Where(f => Filtro.CentroSeleccionado.Contains(f.CodCentro) || Filtro.CentroSeleccionado.Contains(f.CodCentroDist));
                }
                if (!string.IsNullOrEmpty(Filtro.Comprador))
                {
                    cupos = cupos.Where(f => Filtro.Comprador == f.Compcta.ToString());
                }
                if (!string.IsNullOrEmpty(Vendedor))
                {
                    long CuentaVendedor = 0;
                    Int64.TryParse(Vendedor, out CuentaVendedor);
                    cupos = cupos.Where(x => x.VendCta == CuentaVendedor);
                }
                var query = cupos.GroupBy(x => new
                {
                    x.Compcta,
                    x.Comprador,
                    x.PuertoCta,
                    x.Puerto,
                    x.Grano,
                    x.Nomgrano,
                    x.Centro,
                    x.CodCentro,
                    x.CentroDist,
                    x.CodCentroDist,
                    x.D0Cr,
                    x.D0Co,
                    x.D1Cr,
                    x.D1Co,
                    x.D2Cr,
                    x.D2Co,
                    x.D3Cr,
                    x.D3Co,
                    x.D4Cr,
                    x.D4Co,
                    x.D5Cr,
                    x.D5Co,
                    x.D6Cr,
                    x.D6Co,
                    x.D7Cr,
                    x.D7Co,
                    x.D8Cr,
                    x.D8Co,
                    x.D9Cr,
                    x.D9Co,
                    x.D10Cr,
                    x.D10Co,
                    x.D11Cr,
                    x.D11Co,
                    x.D12Cr,
                    x.D12Co,
                    x.D13Cr,
                    x.D13Co,
                    x.D14Cr,
                    x.D14Co,
                    x.D15Cr,
                    x.D15Co,
                    x.D16Cr,
                    x.D16Co,
                    x.D17Cr,
                    x.D17Co,
                    x.D18Cr,
                    x.D18Co,
                    x.D19Cr,
                    x.D19Co,
                    x.D20Cr,
                    x.D20Co
                }).Select(x => new CuposAgrupados
                {
                    Compcta = x.Key.Compcta,
                    Comprador = x.Key.Comprador,
                    PuertoCta = x.Key.PuertoCta,
                    Puerto = x.Key.Puerto,
                    Grano = x.Key.Grano,
                    Nomgrano = x.Key.Nomgrano,
                    Centro = x.Key.Centro,
                    CodCentro = x.Key.CodCentro,
                    CentroDist = x.Key.CentroDist,
                    CodCentroDist = x.Key.CodCentroDist,
                    D0Cr = x.Key.D0Cr,
                    D0Co = x.Key.D0Co,
                    D1Cr = x.Key.D1Cr,
                    D1Co = x.Key.D1Co,
                    D2Cr = x.Key.D2Cr,
                    D2Co = x.Key.D2Co,
                    D3Cr = x.Key.D3Cr,
                    D3Co = x.Key.D3Co,
                    D4Cr = x.Key.D4Cr,
                    D4Co = x.Key.D4Co,
                    D5Cr = x.Key.D5Cr,
                    D5Co = x.Key.D5Co,
                    D6Cr = x.Key.D6Cr,
                    D6Co = x.Key.D6Co,
                    D7Cr = x.Key.D7Cr,
                    D7Co = x.Key.D7Co,
                    D8Cr = x.Key.D8Cr,
                    D8Co = x.Key.D8Co,
                    D9Cr = x.Key.D9Cr,
                    D9Co = x.Key.D9Co,
                    D10Cr = x.Key.D10Cr,
                    D10Co = x.Key.D10Co,
                    D11Cr = x.Key.D11Cr,
                    D11Co = x.Key.D11Co,
                    D12Cr = x.Key.D12Cr,
                    D12Co = x.Key.D12Co,
                    D13Cr = x.Key.D13Cr,
                    D13Co = x.Key.D13Co,
                    D14Cr = x.Key.D14Cr,
                    D14Co = x.Key.D14Co,
                    D15Cr = x.Key.D15Cr,
                    D15Co = x.Key.D15Co,
                    D16Cr = x.Key.D16Cr,
                    D16Co = x.Key.D16Co,
                    D17Cr = x.Key.D17Cr,
                    D17Co = x.Key.D17Co,
                    D18Cr = x.Key.D18Cr,
                    D18Co = x.Key.D18Co,
                    D19Cr = x.Key.D19Cr,
                    D19Co = x.Key.D19Co,
                    D20Cr = x.Key.D20Cr,
                    D20Co = x.Key.D20Co
                });
                IList<ICuposAgrupados> cuposAgrupAll = query.ToList<ICuposAgrupados>();
                HibernateUtil.Dispose();
                return cuposAgrupAll;
            }
        }

        public IList<ICuposAgrupados> FindByFiltroCyo(IndexCupoViewModel Filtro, string Vendedor)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                var cupos = session.Query<CuposAgrupadosDetalleCyo>()
                    .Where(f =>
                        (Filtro.ProductoSeleccionado == 0 || f.Grano == Filtro.ProductoSeleccionado) &&
                        (ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(f.CodCentroDist) ||
                        ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(f.CodCentro))
                    );
                if (!(Filtro.CentroSeleccionado == null || Filtro.CentroSeleccionado.Count == 0))
                {
                    cupos = cupos.Where(f => Filtro.CentroSeleccionado.Contains(f.CodCentro) || Filtro.CentroSeleccionado.Contains(f.CodCentroDist));
                }
                if (!string.IsNullOrEmpty(Filtro.Comprador))
                {
                    cupos = cupos.Where(f => Filtro.Comprador == f.Compcta.ToString());
                }
                if (!string.IsNullOrEmpty(Vendedor))
                {
                    long CuentaVendedor = 0;
                    Int64.TryParse(Vendedor, out CuentaVendedor);
                    cupos = cupos.Where(x => x.VendCta == CuentaVendedor);
                }
                var query = cupos.GroupBy(x => new
                {
                    x.Compcta,
                    x.Comprador,
                    x.PuertoCta,
                    x.Puerto,
                    x.Grano,
                    x.Nomgrano,
                    x.Centro,
                    x.CodCentro,
                    x.CentroDist,
                    x.CodCentroDist,
                    x.D0Cr,
                    x.D0Co,
                    x.D1Cr,
                    x.D1Co,
                    x.D2Cr,
                    x.D2Co,
                    x.D3Cr,
                    x.D3Co,
                    x.D4Cr,
                    x.D4Co,
                    x.D5Cr,
                    x.D5Co,
                    x.D6Cr,
                    x.D6Co,
                    x.D7Cr,
                    x.D7Co,
                    x.D8Cr,
                    x.D8Co,
                    x.D9Cr,
                    x.D9Co,
                    x.D10Cr,
                    x.D10Co,
                    x.D11Cr,
                    x.D11Co,
                    x.D12Cr,
                    x.D12Co,
                    x.D13Cr,
                    x.D13Co,
                    x.D14Cr,
                    x.D14Co,
                    x.D15Cr,
                    x.D15Co,
                    x.D16Cr,
                    x.D16Co,
                    x.D17Cr,
                    x.D17Co,
                    x.D18Cr,
                    x.D18Co,
                    x.D19Cr,
                    x.D19Co,
                    x.D20Cr,
                    x.D20Co
                }).Select(x => new CuposAgrupados
                {
                    Compcta = x.Key.Compcta,
                    Comprador = x.Key.Comprador,
                    PuertoCta = x.Key.PuertoCta,
                    Puerto = x.Key.Puerto,
                    Grano = x.Key.Grano,
                    Nomgrano = x.Key.Nomgrano,
                    Centro = x.Key.Centro,
                    CodCentro = x.Key.CodCentro,
                    CentroDist = x.Key.CentroDist,
                    CodCentroDist = x.Key.CodCentroDist,
                    D0Cr = x.Key.D0Cr,
                    D0Co = x.Key.D0Co,
                    D1Cr = x.Key.D1Cr,
                    D1Co = x.Key.D1Co,
                    D2Cr = x.Key.D2Cr,
                    D2Co = x.Key.D2Co,
                    D3Cr = x.Key.D3Cr,
                    D3Co = x.Key.D3Co,
                    D4Cr = x.Key.D4Cr,
                    D4Co = x.Key.D4Co,
                    D5Cr = x.Key.D5Cr,
                    D5Co = x.Key.D5Co,
                    D6Cr = x.Key.D6Cr,
                    D6Co = x.Key.D6Co,
                    D7Cr = x.Key.D7Cr,
                    D7Co = x.Key.D7Co,
                    D8Cr = x.Key.D8Cr,
                    D8Co = x.Key.D8Co,
                    D9Cr = x.Key.D9Cr,
                    D9Co = x.Key.D9Co,
                    D10Cr = x.Key.D10Cr,
                    D10Co = x.Key.D10Co,
                    D11Cr = x.Key.D11Cr,
                    D11Co = x.Key.D11Co,
                    D12Cr = x.Key.D12Cr,
                    D12Co = x.Key.D12Co,
                    D13Cr = x.Key.D13Cr,
                    D13Co = x.Key.D13Co,
                    D14Cr = x.Key.D14Cr,
                    D14Co = x.Key.D14Co,
                    D15Cr = x.Key.D15Cr,
                    D15Co = x.Key.D15Co,
                    D16Cr = x.Key.D16Cr,
                    D16Co = x.Key.D16Co,
                    D17Cr = x.Key.D17Cr,
                    D17Co = x.Key.D17Co,
                    D18Cr = x.Key.D18Cr,
                    D18Co = x.Key.D18Co,
                    D19Cr = x.Key.D19Cr,
                    D19Co = x.Key.D19Co,
                    D20Cr = x.Key.D20Cr,
                    D20Co = x.Key.D20Co
                });
                IList<ICuposAgrupados> cuposAgrupAll = query.ToList<ICuposAgrupados>();
                HibernateUtil.Dispose();
                return cuposAgrupAll;
            }
        }
    }
}