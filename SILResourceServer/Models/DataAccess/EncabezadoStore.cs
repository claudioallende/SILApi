using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.DataAccess
{
    public class EncabezadoStore : IEncabezadoStore
    {
        private string mapping = "CuposcorreEncab.mpg.xml";

        public Encabezado FindById(long Id)
        {
            throw new NotImplementedException();
        }

        public IList<Encabezado> FindAll()
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<Encabezado> encabezados = session.Query<Encabezado>()
                    .ToList();
                session.Dispose();
                return encabezados;
            }
        }

        public IList<Encabezado> FindByGranoAndCompradorAndVendedorAndPuerto(int grano, long comp, long vend, long puerto)
        {
            IList<long> cupos;
            using (ISession session = HibernateUtil.OpenSession("Cupos.mpg.xml"))
            {
                cupos = session.Query<Cupos>()
                    .Where(x => x.Compcta == comp &&
                        (vend == 0 ? true : x.Vendcta == vend) &&
                        (x.Vendcyo == 0 || x.Vendcyo == vend) &&
                        x.Grano == grano &&
                        x.Puerto == puerto &&
                        x.Fecha.Date > DateTime.Now.Date.AddDays(-5)
                    )
                    .Select(x => x.Idorigen)
                    .ToList();
            }
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<Encabezado> encabezados = session.Query<Encabezado>()
                    .Where(c => 
                        cupos.Contains(c.Id)
                    )
                    .Select(a => new Encabezado{
                        Cuerpos = a.Cuerpos.Where(x => x.Compcta == comp && (vend == 0 ? true : x.Vendcta == vend)).ToList(),
                        Centro = a.Centro,
                        Contrato = a.Contrato,
                        Corrcta = a.Corrcta,
                        Cuitcorrcomp = a.Cuitcorrcomp,
                        Cuitcorrvta = a.Cuitcorrvta,
                        Cuitdestinatario = a.Cuitdestinatario,
                        Cuitintermediario = a.Cuitintermediario,
                        Cuitmat = a.Cuitmat,
                        Cuitrtecomercial = a.Cuitrtecomercial,
                        Cuitrteent = a.Cuitrteent,
                        Cuitsolicitante = a.Cuitsolicitante,
                        Cuposotorgados = a.Cuposotorgados,
                        Cupospedidos = a.Cupospedidos,
                        Cuposrecibidos = a.Cuposrecibidos,
                        Fecha = a.Fecha,
                        Grano = a.Grano,
                        Id = a.Id,
                        Nomcorrcomp = a.Nomcorrcomp,
                        Nomcorrvta = a.Nomcorrvta,
                        Nomdestinatario = a.Nomdestinatario,
                        Nomintermediario = a.Nomintermediario,
                        Nommat = a.Nommat,
                        Nomrtecomercial = a.Nomrtecomercial,
                        Nomrteent = a.Nomrteent,
                        Nomsolicitante = a.Nomsolicitante,
                        Puerto = a.Puerto,
                        Status = a.Status,
                        Vendcta = a.Vendcta,
                        Vendcyo = a.Vendcyo
                    })
                    .ToList();
                session.Dispose();
                IList<Encabezado> encabezadosIgualesAVend = encabezados.Where(x => x.Vendcta == vend).ToList();
                if (encabezadosIgualesAVend.Count > 0)
                {
                    encabezados = encabezadosIgualesAVend;
                }
                return encabezados;
            }
        }

        public IList<Encabezado> FindByGranoAndCompradorAndVendedorAndPuertoCyO(int grano, long comp, long vend, long puerto)
        {
            IList<long> cupos;
            using (ISession session = HibernateUtil.OpenSession("Cupos.mpg.xml"))
            {
                cupos = session.Query<Cupos>()
                    .Where(x => x.Compcta == comp &&
                        (vend == 0 ? true : x.Vendcta == vend) &&
                        x.Grano == grano &&
                        x.Puerto == puerto &&
                        x.Fecha.Date > DateTime.Now.Date.AddDays(-5)
                    )
                    .Select(x => x.Idorigen)
                    .ToList();
            }
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<Encabezado> encabezados = session.Query<Encabezado>()
                    .Where(c =>
                        cupos.Contains(c.Id)
                    )
                    .Select(a => new Encabezado
                    {
                        Cuerpos = a.Cuerpos.Where(x => x.Vendcta == vend && x.Vendcyo != 0).ToList(),
                        Centro = a.Centro,
                        Contrato = a.Contrato,
                        Corrcta = a.Corrcta,
                        Cuitcorrcomp = a.Cuitcorrcomp,
                        Cuitcorrvta = a.Cuitcorrvta,
                        Cuitdestinatario = a.Cuitdestinatario,
                        Cuitintermediario = a.Cuitintermediario,
                        Cuitmat = a.Cuitmat,
                        Cuitrtecomercial = a.Cuitrtecomercial,
                        Cuitrteent = a.Cuitrteent,
                        Cuitsolicitante = a.Cuitsolicitante,
                        Cuposotorgados = a.Cuposotorgados,
                        Cupospedidos = a.Cupospedidos,
                        Cuposrecibidos = a.Cuposrecibidos,
                        Fecha = a.Fecha,
                        Grano = a.Grano,
                        Id = a.Id,
                        Nomcorrcomp = a.Nomcorrcomp,
                        Nomcorrvta = a.Nomcorrvta,
                        Nomdestinatario = a.Nomdestinatario,
                        Nomintermediario = a.Nomintermediario,
                        Nommat = a.Nommat,
                        Nomrtecomercial = a.Nomrtecomercial,
                        Nomrteent = a.Nomrteent,
                        Nomsolicitante = a.Nomsolicitante,
                        Puerto = a.Puerto,
                        Status = a.Status,
                        Vendcta = a.Vendcta,
                        Vendcyo = a.Vendcyo
                    })
                    .ToList();
                session.Dispose();
                IList<Encabezado> encabezadosIgualesAVend = encabezados.Where(x => x.Vendcta == vend).ToList();
                if (encabezadosIgualesAVend.Count > 0)
                {
                    encabezados = encabezadosIgualesAVend;
                }
                return encabezados;
            }
        }

        //public IList<Encabezado> FindByGranoAndCompradorAndVendedorAndPuertoOrCuerpo(int grano, int comp, int vend, int puerto)
        //{
        //    long idEncabezado = 0;
        //    using (ISession session = HibernateUtil.OpenSession("Cupos.mpg.xml"))
        //    {
        //        IList<Cuerpo> cuerpos = session.Query<Cupos>()
        //            .Where(c => c.Grano == grano && c.Puerto == puerto && c.Compcta == comp && c.Vendcta == vend)
        //            .Select(c => new Cuerpo { 
        //                Id = c.Id,
        //                Centro = c.Centro,
        //                Contrato = c.Contrato,
        //                Fecha = c.Fecha,
        //                Nrocupo = c.Nrocupo,
        //                Status = c.Status,
        //                Idorigen = c.Idorigen
        //            })
        //            .ToList();
        //        if (cuerpos.Count > 0)
        //        {
        //            idEncabezado = cuerpos.ElementAt(0).Idorigen;
        //            IList<Encabezado> encabezados = session.Query<Encabezado>()
        //                .Where(c => c.Id == idEncabezado)
        //                .ToList();
        //            session.Dispose();
        //            return encabezados;
        //        }
        //        session.Dispose();
        //        return new List<Encabezado>();
        //    }
        //}
    }
}