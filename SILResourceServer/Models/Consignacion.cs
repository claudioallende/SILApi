using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class Consignacion
    {
        [Display(Name = "Solicitante")]
        public string Cuitsolicitante { get; set; }
        [Display(Name = "Nombre")]
        public string Nomsolicitante { get; set; }
        [Display(Name = "Intermediario")]
        public string Cuitintermediario { get; set; }
        [Display(Name = "Nombre")]
        public string Nomintermediario { get; set; }
        [Display(Name = "Rte. Comercial")]
        public string Cuitrtecomercial { get; set; }
        [Display(Name = "Nombre")]
        public string Nomrtecomercial { get; set; }
        [Display(Name = "Corredor Comprador")]
        public string Cuitcorrcomp { get; set; }
        [Display(Name = "Nombre")]
        public string Nomcorrcomp { get; set; }
        [Display(Name = "Mercado a Término")]
        public string Cuitmat { get; set; }
        [Display(Name = "Nombre")]
        public string Nommat { get; set; }
        [Display(Name = "Corredor Vendedor")]
        public string Cuitcorrvta { get; set; }
        [Display(Name = "Nombre")]
        public string Nomcorrvta { get; set; }
        [Display(Name = "Representante/Entregador")]
        public string Cuitrteent { get; set; }
        [Display(Name = "Nombre")]
        public string Nomrteent { get; set; }
        [Display(Name = "Destinatario")]
        public string Cuitdestinatario { get; set; }
        [Display(Name = "Nombre")]
        public string Nomdestinatario { get; set; }

        [Display(Name = "Remitente Comercial Productor")]
        public virtual string CuitRteComercialProductor { get; set; }
        [Display(Name = "Nombre")]
        public virtual string NomRteComercialProductor { get; set; }
        [Display(Name = "Remitente Comercial Venta Primaria")]
        public virtual string CuitRteComercialVentaPrimaria { get; set; }
        [Display(Name = "Nombre")]
        public virtual string NomRteComercialVentaPrimaria { get; set; }
        public string Observacion { get; set; }
        [Display(Name = "Caratula")]
        public virtual string Caratula { get; set; }
        [Display(Name = "Contacto Comercial")]
        public virtual string ContactoComercial { get; set; }
    public Consignacion()
        {

        }

        public Consignacion(Cupos cupo)
        {
            Cuitsolicitante = cupo.Cuitsolicitante.Trim();
            Nomsolicitante = cupo.Nomsolicitante.Trim();
            Cuitintermediario = cupo.Cuitintermediario.Trim();
            Nomintermediario = cupo.Nomintermediario.Trim();
            Cuitrtecomercial = cupo.Cuitrtecomercial.Trim();
            Nomrtecomercial = cupo.Nomrtecomercial.Trim();
            Cuitcorrcomp = cupo.Cuitcorrcomp.Trim();
            Nomcorrcomp = cupo.Nomcorrcomp.Trim();
            Cuitmat = cupo.Cuitmat.Trim();
            Nommat = cupo.Nommat.Trim();
            Cuitcorrvta = cupo.Cuitcorrvta.Trim();
            Nomcorrvta = cupo.Nomcorrvta.Trim();
            Cuitrteent = cupo.Cuitrteent.Trim();
            Nomrteent = cupo.Nomrteent.Trim();
            Cuitdestinatario = cupo.Cuitdestinatario.Trim();
            Nomdestinatario = cupo.Nomdestinatario.Trim();
            CuitRteComercialProductor = cupo.CuitRteComercialProductor.Trim();
            NomRteComercialProductor = cupo.NomRteComercialProductor.Trim();
            CuitRteComercialVentaPrimaria = cupo.NomRteComercialVentaPrimaria.Trim();
            NomRteComercialVentaPrimaria = cupo.NomRteComercialVentaPrimaria.Trim();
        }

        public void SetObservacion(string Observacion)
        {
            this.Observacion = Observacion;
        }

        public string GetObservacion()
        {
            return this.Observacion;
        }

        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Consignacion c = (Consignacion)obj;
                return (Cuitsolicitante == c.Cuitsolicitante) && (Nomsolicitante == c.Nomsolicitante) && 
                    (Cuitintermediario == c.Cuitintermediario) && Nomintermediario == c.Nomintermediario &&
                    (Cuitrtecomercial == c.Cuitrtecomercial) && Nomrtecomercial == c.Nomrtecomercial &&
                    (Cuitcorrcomp == c.Cuitcorrcomp) && Nomcorrcomp == c.Nomcorrcomp &&
                    (Cuitmat == c.Cuitmat) && Nommat == c.Nommat &&
                    (Cuitcorrvta == c.Cuitcorrvta) && Nomcorrvta == c.Nomcorrvta &&
                    (Cuitrteent == c.Cuitrteent) && Nomrteent == c.Nomrteent &&
                    (Cuitdestinatario == c.Cuitdestinatario) && Nomdestinatario == c.Nomdestinatario &&
                    (CuitRteComercialProductor == c.CuitRteComercialProductor) && NomRteComercialProductor == c.NomRteComercialProductor &&
                    (CuitRteComercialVentaPrimaria == c.CuitRteComercialVentaPrimaria) && NomRteComercialVentaPrimaria == c.NomRteComercialVentaPrimaria;
            }
        }

        public override int GetHashCode()
        {
            return (string.IsNullOrEmpty(this.Cuitsolicitante) ? 0 : this.Cuitsolicitante.GetHashCode()) ^ 
                (string.IsNullOrEmpty(this.Nomsolicitante) ? 0 : this.Nomsolicitante.GetHashCode()) ^
                (string.IsNullOrEmpty(this.Cuitintermediario) ? 0 : this.Cuitintermediario.GetHashCode()) ^ 
                (string.IsNullOrEmpty(this.Nomintermediario) ? 0 : this.Nomintermediario.GetHashCode()) ^
                (string.IsNullOrEmpty(this.Cuitrtecomercial) ? 0 : this.Cuitrtecomercial.GetHashCode()) ^ 
                (string.IsNullOrEmpty(this.Nomrtecomercial) ? 0 : this.Nomrtecomercial.GetHashCode()) ^
                (string.IsNullOrEmpty(this.Cuitcorrcomp) ? 0 : this.Cuitcorrcomp.GetHashCode()) ^ 
                (string.IsNullOrEmpty(this.Nomcorrcomp) ? 0 : this.Nomcorrcomp.GetHashCode()) ^
                (string.IsNullOrEmpty(this.Cuitmat) ? 0 : this.Cuitmat.GetHashCode()) ^ 
                (string.IsNullOrEmpty(this.Nommat) ? 0 : this.Nommat.GetHashCode()) ^
                (string.IsNullOrEmpty(this.Cuitcorrvta) ? 0 : this.Cuitcorrvta.GetHashCode()) ^ 
                (string.IsNullOrEmpty(this.Nomcorrvta) ? 0 : this.Nomcorrvta.GetHashCode()) ^
                (string.IsNullOrEmpty(this.Cuitrteent) ? 0 : this.Cuitrteent.GetHashCode()) ^ 
                (string.IsNullOrEmpty(this.Nomrteent) ? 0 : this.Nomrteent.GetHashCode()) ^
                (string.IsNullOrEmpty(this.Cuitdestinatario) ? 0 : this.Cuitdestinatario.GetHashCode()) ^ 
                (string.IsNullOrEmpty(this.Nomdestinatario) ? 0 : this.Nomdestinatario.GetHashCode()) ^ 
                (string.IsNullOrEmpty(this.CuitRteComercialProductor) ? 0 : this.CuitRteComercialProductor.GetHashCode()) ^
                (string.IsNullOrEmpty(this.NomRteComercialProductor) ? 0 : this.NomRteComercialProductor.GetHashCode()) ^
                (string.IsNullOrEmpty(this.CuitRteComercialVentaPrimaria) ? 0 : this.CuitRteComercialVentaPrimaria.GetHashCode()) ^
                (string.IsNullOrEmpty(this.NomRteComercialVentaPrimaria) ? 0 : this.NomRteComercialVentaPrimaria.GetHashCode());
        }

        public Cupos GetConsignacion()
        {
            return new Cupos
            {
                Cuitsolicitante = this.Cuitsolicitante,
                Nomsolicitante = this.Nomsolicitante,
                Cuitintermediario = this.Cuitintermediario,
                Nomintermediario = this.Nomintermediario,
                Cuitrtecomercial = this.Cuitrtecomercial,
                Nomrtecomercial = this.Nomrtecomercial,
                Cuitcorrcomp = this.Cuitcorrcomp,
                Nomcorrcomp = this.Nomcorrcomp,
                Cuitmat = this.Cuitmat,
                Nommat = this.Nommat,
                Cuitcorrvta = this.Cuitcorrvta,
                Nomcorrvta = this.Nomcorrvta,
                Cuitrteent = this.Cuitrteent,
                Nomrteent = this.Nomrteent,
                Cuitdestinatario = this.Cuitdestinatario,
                Nomdestinatario = this.Nomdestinatario,
                CuitRteComercialProductor = this.CuitRteComercialProductor,
                NomRteComercialProductor = this.NomRteComercialProductor,
                CuitRteComercialVentaPrimaria = this.CuitRteComercialVentaPrimaria,
                NomRteComercialVentaPrimaria = this.NomRteComercialVentaPrimaria
            };
        }

        /// <summary>
        /// Solo por CUIT
        /// </summary>
        /// <param name="cupos"></param>
        /// <returns></returns>
        public IQueryable<Cupos> FiltroConsignacion(IQueryable<Cupos> cupos)
        {
            if (this.Cuitsolicitante == null || this.Cuitsolicitante.Trim() == "")
            {
                cupos = cupos.Where(x => x.Cuitsolicitante == null || x.Cuitsolicitante.Trim() == "");
            }
            else
            {
                cupos = cupos.Where(x => this.Cuitsolicitante == x.Cuitsolicitante);
            }
            if (this.Cuitintermediario == null || this.Cuitintermediario.Trim() == "")
            {
                cupos = cupos.Where(x => x.Cuitintermediario == null || x.Cuitintermediario.Trim() == "");
            }
            else
            {
                cupos = cupos.Where(x => this.Cuitintermediario == x.Cuitintermediario);
            }
            if (this.Cuitrtecomercial == null || this.Cuitrtecomercial.Trim() == "")
            {
                cupos = cupos.Where(x => x.Cuitrtecomercial == null || x.Cuitrtecomercial.Trim() == "");
            }
            else
            {
                cupos = cupos.Where(x => this.Cuitrtecomercial == x.Cuitrtecomercial);
            }
            if (this.Cuitcorrcomp == null || this.Cuitcorrcomp.Trim() == "")
            {
                cupos = cupos.Where(x => x.Cuitcorrcomp == null || x.Cuitcorrcomp.Trim() == "");
            }
            else
            {
                cupos = cupos.Where(x => this.Cuitcorrcomp == x.Cuitcorrcomp);
            }
            if (this.Cuitmat == null || this.Cuitmat.Trim() == "")
            {
                cupos = cupos.Where(x => x.Cuitmat == null || x.Cuitmat.Trim() == "");
            }
            else
            {
                cupos = cupos.Where(x => this.Cuitmat == x.Cuitmat);
            }
            if (this.Cuitcorrvta == null || this.Cuitcorrvta.Trim() == "")
            {
                cupos = cupos.Where(x => x.Cuitcorrvta == null || x.Cuitcorrvta.Trim() == "");
            }
            else
            {
                cupos = cupos.Where(x => this.Cuitcorrvta == x.Cuitcorrvta);
            }
            if (this.Cuitrteent == null || this.Cuitrteent.Trim() == "")
            {
                cupos = cupos.Where(x => x.Cuitrteent == null || x.Cuitrteent.Trim() == "");
            }
            else
            {
                cupos = cupos.Where(x => this.Cuitrteent == x.Cuitrteent);
            }
            if (this.Cuitdestinatario == null || this.Cuitdestinatario.Trim() == "")
            {
                cupos = cupos.Where(x => x.Cuitdestinatario == null || x.Cuitdestinatario.Trim() == "");
            }
            else
            {
                cupos = cupos.Where(x => this.Cuitdestinatario == x.Cuitdestinatario);
            }

            if (this.CuitRteComercialProductor == null || this.CuitRteComercialProductor.Trim() == "")
            {
                cupos = cupos.Where(x => x.CuitRteComercialProductor == null || x.CuitRteComercialProductor.Trim() == "");
            }
            else 
            {
                cupos = cupos.Where(x => this.CuitRteComercialProductor == x.CuitRteComercialProductor);
            }
            if (this.CuitRteComercialVentaPrimaria == null || this.CuitRteComercialVentaPrimaria.Trim() == "")
            {
              cupos = cupos.Where(x => x.CuitRteComercialVentaPrimaria == null || x.CuitRteComercialVentaPrimaria.Trim() == "");
            }
            else
            {
              cupos = cupos.Where(x => this.CuitRteComercialVentaPrimaria == x.CuitRteComercialVentaPrimaria);
            }
            if (this.Caratula == null || this.Caratula.Trim() == "")
            {
              cupos = cupos.Where(x => x.Caratula== null || x.Caratula.Trim() == "");
            }
            else
            {
              cupos = cupos.Where(x => this.Caratula == x.Caratula);
            }
        return cupos;
        }

        public IQueryable<Cupos> FiltroConsignacionIgnoreIfIsNull(IQueryable<Cupos> cupos)
        {
            if (!string.IsNullOrEmpty(Cuitsolicitante))
            {
                cupos = cupos.Where(x => this.Cuitsolicitante == x.Cuitsolicitante);
            }
            if (!string.IsNullOrEmpty(Cuitintermediario))
            {
                cupos = cupos.Where(x => this.Cuitintermediario == x.Cuitintermediario);
            }
            if (!string.IsNullOrEmpty(Cuitrtecomercial))
            {
                cupos = cupos.Where(x => this.Cuitrtecomercial == x.Cuitrtecomercial);
            }
            if (!string.IsNullOrEmpty(Cuitcorrcomp))
            {
                cupos = cupos.Where(x => this.Cuitcorrcomp == x.Cuitcorrcomp);
            }
            if (!string.IsNullOrEmpty(Cuitmat))
            {
                cupos = cupos.Where(x => this.Cuitmat == x.Cuitmat);
            }
            if (!string.IsNullOrEmpty(Cuitcorrvta))
            {
                cupos = cupos.Where(x => this.Cuitcorrvta == x.Cuitcorrvta);
            }
            if (!string.IsNullOrEmpty(Cuitrteent))
            {
                cupos = cupos.Where(x => this.Cuitrteent == x.Cuitrteent);
            }
            if (!string.IsNullOrEmpty(Cuitdestinatario))
            {
                cupos = cupos.Where(x => this.Cuitdestinatario == x.Cuitdestinatario);
            }

            if (!string.IsNullOrEmpty(CuitRteComercialProductor))
            {
                cupos = cupos.Where(x => this.CuitRteComercialProductor == x.CuitRteComercialProductor);
            }
            if (!string.IsNullOrEmpty(CuitRteComercialVentaPrimaria))
            {
                cupos = cupos.Where(x => this.CuitRteComercialVentaPrimaria == x.CuitRteComercialVentaPrimaria);
            }
            return cupos;
        }
        public IQueryable<VistaCuposDistribuidos> FiltroConsignacion(IQueryable<VistaCuposDistribuidos> cupos)
        {
            if (this.Cuitsolicitante == null || this.Cuitsolicitante.Trim() == "")
            {
                cupos = cupos.Where(x => x.Cuitsolicitante == null || x.Cuitsolicitante.Trim() == "");
            }
            else
            {
                cupos = cupos.Where(x => this.Cuitsolicitante == x.Cuitsolicitante);
            }
            if (this.Cuitintermediario == null || this.Cuitintermediario.Trim() == "")
            {
                cupos = cupos.Where(x => x.Cuitintermediario == null || x.Cuitintermediario.Trim() == "");
            }
            else
            {
                cupos = cupos.Where(x => this.Cuitintermediario == x.Cuitintermediario);
            }
            if (this.Cuitrtecomercial == null || this.Cuitrtecomercial.Trim() == "")
            {
                cupos = cupos.Where(x => x.Cuitrtecomercial == null || x.Cuitrtecomercial.Trim() == "");
            }
            else
            {
                cupos = cupos.Where(x => this.Cuitrtecomercial == x.Cuitrtecomercial);
            }
            if (this.Cuitcorrcomp == null || this.Cuitcorrcomp.Trim() == "")
            {
                cupos = cupos.Where(x => x.Cuitcorrcomp == null || x.Cuitcorrcomp.Trim() == "");
            }
            else
            {
                cupos = cupos.Where(x => this.Cuitcorrcomp == x.Cuitcorrcomp);
            }
            if (this.Cuitmat == null || this.Cuitmat.Trim() == "")
            {
                cupos = cupos.Where(x => x.Cuitmat == null || x.Cuitmat.Trim() == "");
            }
            else
            {
                cupos = cupos.Where(x => this.Cuitmat == x.Cuitmat);
            }
            if (this.Cuitcorrvta == null || this.Cuitcorrvta.Trim() == "")
            {
                cupos = cupos.Where(x => x.Cuitcorrvta == null || x.Cuitcorrvta.Trim() == "");
            }
            else
            {
                cupos = cupos.Where(x => this.Cuitcorrvta == x.Cuitcorrvta);
            }
            if (this.Cuitrteent == null || this.Cuitrteent.Trim() == "")
            {
                cupos = cupos.Where(x => x.Cuitrteent == null || x.Cuitrteent.Trim() == "");
            }
            else
            {
                cupos = cupos.Where(x => this.Cuitrteent == x.Cuitrteent);
            }
            if (this.Cuitdestinatario == null || this.Cuitdestinatario.Trim() == "")
            {
                cupos = cupos.Where(x => x.Cuitdestinatario == null || x.Cuitdestinatario.Trim() == "");
            }
            else
            {
                cupos = cupos.Where(x => this.Cuitdestinatario == x.Cuitdestinatario);
            }

            if (this.CuitRteComercialProductor == null || this.CuitRteComercialProductor.Trim() == "")
            {
              cupos = cupos.Where(x => x.CuitRteComercialProductor == null || x.CuitRteComercialProductor.Trim() == "");
            }
            else
            {
              cupos = cupos.Where(x => this.CuitRteComercialProductor == x.CuitRteComercialProductor);
            }
            if (this.CuitRteComercialVentaPrimaria == null || this.CuitRteComercialVentaPrimaria.Trim() == "")
            {
              cupos = cupos.Where(x => x.CuitRteComercialVentaPrimaria == null || x.CuitRteComercialVentaPrimaria.Trim() == "");
            }
            else
            {
              cupos = cupos.Where(x => this.CuitRteComercialVentaPrimaria == x.CuitRteComercialVentaPrimaria);
            }
            return cupos;
        }

        public IQueryable<CuposDist> FiltroConsignacion(IQueryable<CuposDist> cupos)
        {
            if (this.Cuitsolicitante == null || this.Cuitsolicitante.Trim() == "")
            {
                cupos = cupos.Where(x => x.Cuitsolicitante == null || x.Cuitsolicitante.Trim() == "");
            }
            else
            {
                cupos = cupos.Where(x => this.Cuitsolicitante == x.Cuitsolicitante);
            }
            if (this.Cuitintermediario == null || this.Cuitintermediario.Trim() == "")
            {
                cupos = cupos.Where(x => x.Cuitintermediario == null || x.Cuitintermediario.Trim() == "");
            }
            else
            {
                cupos = cupos.Where(x => this.Cuitintermediario == x.Cuitintermediario);
            }
            if (this.Cuitrtecomercial == null || this.Cuitrtecomercial.Trim() == "")
            {
                cupos = cupos.Where(x => x.Cuitrtecomercial == null || x.Cuitrtecomercial.Trim() == "");
            }
            else
            {
                cupos = cupos.Where(x => this.Cuitrtecomercial == x.Cuitrtecomercial);
            }
            if (this.Cuitcorrcomp == null || this.Cuitcorrcomp.Trim() == "")
            {
                cupos = cupos.Where(x => x.Cuitcorrcomp == null || x.Cuitcorrcomp.Trim() == "");
            }
            else
            {
                cupos = cupos.Where(x => this.Cuitcorrcomp == x.Cuitcorrcomp);
            }
            if (this.Cuitmat == null || this.Cuitmat.Trim() == "")
            {
                cupos = cupos.Where(x => x.Cuitmat == null || x.Cuitmat.Trim() == "");
            }
            else
            {
                cupos = cupos.Where(x => this.Cuitmat == x.Cuitmat);
            }
            if (this.Cuitcorrvta == null || this.Cuitcorrvta.Trim() == "")
            {
                cupos = cupos.Where(x => x.Cuitcorrvta == null || x.Cuitcorrvta.Trim() == "");
            }
            else
            {
                cupos = cupos.Where(x => this.Cuitcorrvta == x.Cuitcorrvta);
            }
            if (this.Cuitrteent == null || this.Cuitrteent.Trim() == "")
            {
                cupos = cupos.Where(x => x.Cuitrteent == null || x.Cuitrteent.Trim() == "");
            }
            else
            {
                cupos = cupos.Where(x => this.Cuitrteent == x.Cuitrteent);
            }
            if (this.Cuitdestinatario == null || this.Cuitdestinatario.Trim() == "")
            {
                cupos = cupos.Where(x => x.Cuitdestinatario == null || x.Cuitdestinatario.Trim() == "");
            }
            else
            {
                cupos = cupos.Where(x => this.Cuitdestinatario == x.Cuitdestinatario);
            }

            if (this.CuitRteComercialProductor == null || this.CuitRteComercialProductor.Trim() == "")
            {
                cupos = cupos.Where(x => x.CuitRteComercialProductor == null || x.CuitRteComercialProductor.Trim() == "");
            }
            else
            {
                cupos = cupos.Where(x => this.CuitRteComercialProductor == x.CuitRteComercialProductor);
            }
            if (this.CuitRteComercialVentaPrimaria == null || this.CuitRteComercialVentaPrimaria.Trim() == "")
            {
                cupos = cupos.Where(x => x.CuitRteComercialVentaPrimaria == null || x.CuitRteComercialVentaPrimaria.Trim() == "");
            }
            else
            {
                cupos = cupos.Where(x => this.CuitRteComercialVentaPrimaria == x.CuitRteComercialVentaPrimaria);
            }
            return cupos;
        }

        public bool IsNullOrEmpty()
        {
            return (string.IsNullOrEmpty(this.Cuitsolicitante) &&
                string.IsNullOrEmpty(this.Nomsolicitante) &&
                string.IsNullOrEmpty(this.Cuitintermediario) &&
                string.IsNullOrEmpty(this.Nomintermediario) &&
                string.IsNullOrEmpty(this.Cuitrtecomercial) &&
                string.IsNullOrEmpty(this.Nomrtecomercial) &&
                string.IsNullOrEmpty(this.Cuitcorrcomp) &&
                string.IsNullOrEmpty(this.Nomcorrcomp) &&
                string.IsNullOrEmpty(this.Cuitmat) &&
                string.IsNullOrEmpty(this.Nommat) &&
                string.IsNullOrEmpty(this.Cuitcorrvta) &&
                string.IsNullOrEmpty(this.Nomcorrvta) &&
                string.IsNullOrEmpty(this.Cuitrteent) &&
                string.IsNullOrEmpty(this.Nomrteent) &&
                string.IsNullOrEmpty(this.Cuitdestinatario) &&
                string.IsNullOrEmpty(this.Nomdestinatario) &&
                string.IsNullOrEmpty(this.CuitRteComercialProductor) &&
                string.IsNullOrEmpty(this.NomRteComercialProductor) &&
                string.IsNullOrEmpty(this.CuitRteComercialVentaPrimaria) &&
                string.IsNullOrEmpty(this.NomRteComercialVentaPrimaria)
            );
        }
    }
}