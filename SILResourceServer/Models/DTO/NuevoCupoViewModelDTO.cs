using ResourceServer.Models.AtributosValidacion;
using ResourceServer.Models.Configuracion;
using ResourceServer.Models.DataAccess;
using ResourceServer.Models.Identity;
using NHibernate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ResourceServer.Models.Error.Exceptions;

namespace ResourceServer.Models.DTO
{
    public class NuevoCupoViewModelDTO
    {
        public string Error { get; set; }
        [Display(Name = "Comprador")]
        public string Compcta { get; set; }
        public string CompradorNombre { get; set; }
        [Display(Name = "Vendedor")]
        public string Vendcta { get; set; }
        public string VendedorNombre { get; set; }
        public string Puerto { get; set; }
        public string PuertoNombre { get; set; }
        public int Producto { get; set; }
        public string ProductoNombre { get; set; }
        public readonly int CantidadDias = 20;
        [Display(Name = "Centro")]
        public virtual string Centro { get; set; }
        public virtual bool VendcyoBoolValue { get; set; }
        [Display(Name = "Solicitante")]
        public virtual string Cuitsolicitante { get; set; }
        [Display(Name = "Nombre")]
        public virtual string Nomsolicitante { get; set; }
        [Display(Name = "Intermediario")]
        public virtual string Cuitintermediario { get; set; }
        [Display(Name = "Nombre")]
        public virtual string Nomintermediario { get; set; }
        [Display(Name = "Comercial")]
        public virtual string Cuitrtecomercial { get; set; }
        [Display(Name = "Nombre")]
        public virtual string Nomrtecomercial { get; set; }
        [Display(Name = "Corredor Comprador")]
        public virtual string Cuitcorrcomp { get; set; }
        [Display(Name = "Nombre")]
        public virtual string Nomcorrcomp { get; set; }
        [Display(Name = "Mercado a Termino")]
        public virtual string Cuitmat { get; set; }
        [Display(Name = "Nombre")]
        public virtual string Nommat { get; set; }
        [Display(Name = "Corredor Vendedor")]
        public virtual string Cuitcorrvta { get; set; }
        [Display(Name = "Nombre")]
        public virtual string Nomcorrvta { get; set; }
        [Display(Name = "Entregador")]
        public virtual string Cuitrteent { get; set; }
        [Display(Name = "Nombre")]
        public virtual string Nomrteent { get; set; }
        [Display(Name = "Destinatario")]
        [Required]
        public virtual string Cuitdestinatario { get; set; }
        [Display(Name = "Nombre")]
        [Required]
        public virtual string Nomdestinatario { get; set; }
        [Display(Name = "Remitente Comercial Productor")]
        public virtual string CuitRteComercialProductor { get; set; }
        [Display(Name = "Nombre")]
        public virtual string NomRteComercialProductor { get; set; }
        [Display(Name = "Remitente Comercial Venta Primaria")]
        public virtual string CuitRteComercialVentaPrimaria { get; set; }
        [Display(Name = "Nombre")]
        public virtual string NomRteComercialVentaPrimaria { get; set; }

        public virtual IList<CuposPorDiaDTO> CodigosDias { get; set; }
        public virtual string Observaciones { get; set; }
        public virtual string DetalleCupoCNRT { get; set; }
        public virtual int EstadoCupoCNRT { get; set; }
        public virtual bool EmparejoTurnos{ get; set; }
        public virtual string CentroAnterior { get; set; }
        public virtual string Caratula { get; set; }
        public virtual string ContactoComercial { get; set; }
        public NuevoCupoViewModelDTO() { }

        public NuevoCupoViewModelDTO(NuevoCupoViewModel cupoVM)
        {
            this.Producto = cupoVM.ProductoSeleccionado;
            this.Puerto = cupoVM.Puerto;
            this.Compcta = cupoVM.Compcta;
            this.Centro = cupoVM.Centro;
            this.VendcyoBoolValue = cupoVM.VendcyoBoolValue;
            this.Cuitdestinatario = cupoVM.Cuitdestinatario;
            this.Nomdestinatario = cupoVM.Nomdestinatario;
            this.Cuitsolicitante = cupoVM.Cuitsolicitante;
            this.Nomsolicitante = cupoVM.Nomsolicitante;
            this.Cuitintermediario = cupoVM.Cuitintermediario;
            this.Nomintermediario = cupoVM.Nomintermediario;
            this.Cuitmat = cupoVM.Cuitmat;
            this.Nommat = cupoVM.Nommat;
            this.Cuitrtecomercial = cupoVM.Cuitrtecomercial;
            this.Nomrtecomercial = cupoVM.Nomrtecomercial;
            this.Cuitcorrvta = cupoVM.Cuitcorrvta;
            this.Nomcorrvta = cupoVM.Nomcorrvta;
            this.Cuitcorrcomp = cupoVM.Cuitcorrcomp;
            this.Nomcorrcomp = cupoVM.Nomcorrcomp;
            this.Cuitrteent = cupoVM.Cuitrteent;
            this.Nomrteent = cupoVM.Nomrteent;
            this.CuitRteComercialProductor = cupoVM.CuitRteComercialProductor;
            this.NomRteComercialProductor = cupoVM.NomRteComercialProductor;
            this.CuitRteComercialVentaPrimaria = cupoVM.CuitRteComercialVentaPrimaria;
            this.NomRteComercialVentaPrimaria = cupoVM.NomRteComercialVentaPrimaria;
            this.DetalleCupoCNRT = cupoVM.DetalleCupoCNRT;
            this.EstadoCupoCNRT = cupoVM.EstadoCupoCNRT;
            this.CodigosDias = new List<CuposPorDiaDTO>();
            IList<CodigosAlfanumericos> listaDeAlfas = cupoVM.CodigosDias;
            var DiasDeLosAlfa = listaDeAlfas.GroupBy(x => (DateTime)x.Dia).ToList();
            foreach (var dia in DiasDeLosAlfa)
            {
                CuposPorDiaDTO cuposxDia = new CuposPorDiaDTO();
                cuposxDia.Fecha = dia.Key.Date;
                cuposxDia.Turnos = listaDeAlfas.Where(x => (DateTime)x.Dia == dia.Key.Date).Select(y => y.Alfanumerico).ToList();
                this.CodigosDias.Add(cuposxDia);
            }           
        }

        public virtual void SetConsignacion(Consignacion consignacion)
        {
            this.Cuitdestinatario = consignacion.Cuitdestinatario;
            this.Nomdestinatario = consignacion.Nomdestinatario;
            this.Cuitsolicitante = consignacion.Cuitsolicitante;
            this.Nomsolicitante = consignacion.Nomsolicitante;
            this.Cuitintermediario = consignacion.Cuitintermediario;
            this.Nomintermediario = consignacion.Nomintermediario;
            this.Cuitmat = consignacion.Cuitmat;
            this.Nommat = consignacion.Nommat;
            this.Cuitrtecomercial = consignacion.Cuitrtecomercial;
            this.Nomrtecomercial = consignacion.Nomrtecomercial;
            this.Cuitcorrvta = consignacion.Cuitcorrvta;
            this.Nomcorrvta = consignacion.Nomcorrvta;
            this.Cuitcorrcomp = consignacion.Cuitcorrcomp;
            this.Nomcorrcomp = consignacion.Nomcorrcomp;
            this.Cuitrteent = consignacion.Cuitrteent;
            this.Nomrteent = consignacion.Nomrteent;
            this.CuitRteComercialProductor = consignacion.CuitRteComercialProductor;
            this.NomRteComercialProductor = consignacion.NomRteComercialProductor;
            this.CuitRteComercialVentaPrimaria = consignacion.CuitRteComercialVentaPrimaria;
            this.NomRteComercialVentaPrimaria = consignacion.NomRteComercialVentaPrimaria;
        }

        public virtual Consignacion GetConsignacion()
        {
            Consignacion consignacion = new Consignacion();
            consignacion.Cuitdestinatario = (!string.IsNullOrEmpty(this.Cuitdestinatario)) ? this.Cuitdestinatario.Replace("-", "") : this.Cuitdestinatario;
            consignacion.Nomdestinatario = this.Nomdestinatario;
            consignacion.Cuitsolicitante = (!string.IsNullOrEmpty(this.Cuitsolicitante)) ? this.Cuitsolicitante.Replace("-", "") : this.Cuitsolicitante;
            consignacion.Nomsolicitante = this.Nomsolicitante;
            consignacion.Cuitintermediario = (!string.IsNullOrEmpty(this.Cuitintermediario)) ? this.Cuitintermediario.Replace("-", "") : this.Cuitintermediario;
            consignacion.Nomintermediario = this.Nomintermediario;
            consignacion.Cuitmat = (!string.IsNullOrEmpty(this.Cuitmat)) ? this.Cuitmat.Replace("-", "") : this.Cuitmat;
            consignacion.Nommat = this.Nommat;
            consignacion.Cuitrtecomercial = (!string.IsNullOrEmpty(this.Cuitrtecomercial)) ? this.Cuitrtecomercial.Replace("-", "") : this.Cuitrtecomercial;
            consignacion.Nomrtecomercial = this.Nomrtecomercial;
            consignacion.Cuitcorrvta = (!string.IsNullOrEmpty(this.Cuitcorrvta) && this.Cuitcorrvta != "0") ? this.Cuitcorrvta.Replace("-", "") : null;
            consignacion.Nomcorrvta = this.Nomcorrvta;
            consignacion.Cuitcorrcomp = (!string.IsNullOrEmpty(this.Cuitcorrcomp) && this.Cuitcorrcomp != "0") ? this.Cuitcorrcomp.Replace("-", "") : null;
            consignacion.Nomcorrcomp = this.Nomcorrcomp;
            consignacion.Cuitrteent = (!string.IsNullOrEmpty(this.Cuitrteent)) ? this.Cuitrteent.Replace("-", "") : Cuitrteent;
            consignacion.Nomrteent = this.Nomrteent;
            consignacion.CuitRteComercialProductor = (!string.IsNullOrEmpty(this.CuitRteComercialProductor)) ? this.CuitRteComercialProductor.Replace("-", "") : CuitRteComercialProductor;
            consignacion.NomRteComercialProductor = this.NomRteComercialProductor;
            consignacion.CuitRteComercialVentaPrimaria = (!string.IsNullOrEmpty(this.CuitRteComercialVentaPrimaria)) ? this.CuitRteComercialVentaPrimaria.Replace("-", "") : CuitRteComercialVentaPrimaria;
            consignacion.NomRteComercialVentaPrimaria = this.NomRteComercialVentaPrimaria;
            return consignacion;
        }

        public virtual DateTime GetFechaDeAlfa(string CodigoAlfanumerico)
        {
            if (!string.IsNullOrEmpty(CodigoAlfanumerico)) 
            {
                foreach (CuposPorDiaDTO CPD in this.CodigosDias) 
                {
                    if (CPD.Turnos.Contains(CodigoAlfanumerico)) return CPD.Fecha;
                }
            }
            return default(DateTime);
        }

        public List<string> GetAlfas() 
        {
            
            List<string> salids= new List<string>();
            foreach (CuposPorDiaDTO alfas in this.CodigosDias)
            {
                salids.AddRange(alfas.Turnos);
            }
            return salids;
        }
    }
}