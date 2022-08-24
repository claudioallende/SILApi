using ResourceServer.Models.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class CupoInStopAndNotSIL
    {
        [Display(Name = "Id")]
        public virtual long Id { get; set; }
        [Display(Name = "Fecha")]
        public virtual DateTime Fecha { get; set; }
        [Display(Name = "Id Cupo STOP")]
        public virtual long Idcupo { get; set; }
        [Display(Name = "Cupo Nro.")]
        public virtual string Nrocupo { get; set; }
        [Display(Name = "Id Terminal")]
        public virtual int Idterminal { get; set; }
        [Display(Name = "Origen")]
        public virtual string Cuitorigen { get; set; }
        [Display(Name = "Intermediario")]
        public virtual string Cuitintermediario { get; set; }
        [Display(Name = "Mercado a Termino")]
        public virtual string Cuitmercadoatermino { get; set; }
        [Display(Name = "Remitente Comercial")]
        public virtual string Cuitremcomercial { get; set; }
        [Display(Name = "Corredor Vendedor")]
        public virtual string Cuitcorredorv { get; set; }
        [Display(Name = "Corredor Comprador")]
        public virtual string Cuitcorredorc { get; set; }
        [Display(Name = "Representante Entregador")]
        public virtual string Cuitrepresentanteentregador { get; set; }
        [Display(Name = "Destino")]
        public virtual string Cuitdestino { get; set; }
        [Display(Name = "Destinatario")]
        public virtual string Cuitdestinatario { get; set; }
        [Display(Name = "Intermediario Flete")]
        public virtual string Cuitintermediarioflete { get; set; }
        [Display(Name = "Transportista")]
        public virtual string Cuittransportista { get; set; }
        [Display(Name = "Chofer")]
        public virtual string Cuitchofer { get; set; }
        [Display(Name = "Detalle Cupo")]
        public virtual string Detallecupo { get; set; }
        [Display(Name = "Codigo Estado Cupo")]
        public virtual string CodigoEstadoCupo { get; set; }
        [Display(Name = "Grano")]
        public virtual int Grano { get; set; }
        [Display(Name = "Centro")]
        public virtual string Centro { get; set; }

        //public virtual ICupo toCupo() 
        //{
        //    Cupos thisInCupo = new Cupos();
        //    thisInCupo.Grano = 0;//verqueponemosaca
        //    thisInCupo.Cuitsolicitante = this.Cuitorigen; // hay que ver el caso que hablamos con valvecchia. cotagro cotagro
        //    thisInCupo.Cuitintermediario = this.Cuitintermediario;
        //    thisInCupo.Cuitrtecomercial = this.Cuitremcomercial;
        //    thisInCupo.Cuitcorrcomp = this.Cuitcorredorc;
        //    thisInCupo.Cuitcorrvta = this.Cuitcorredorv;
        //    thisInCupo.Cuitmat = this.Cuitmercadoatermino;
        //    thisInCupo.Cuitrteent = this.Cuitrepresentanteentregador;
        //    thisInCupo.Cuitdestinatario = this.Cuitdestinatario;
        //    return thisInCupo;
        //}

        public virtual Consignacion GetCuitConsignacion() 
        {
            Consignacion consignacion = new Consignacion();
            consignacion.Cuitsolicitante = this.Cuitorigen;
            consignacion.Cuitintermediario = this.Cuitintermediario;
            consignacion.Cuitrtecomercial = this.Cuitremcomercial;
            consignacion.Cuitcorrcomp = this.Cuitcorredorc;
            consignacion.Cuitmat = this.Cuitmercadoatermino;
            consignacion.Cuitcorrvta = this.Cuitcorredorv;
            consignacion.Cuitrteent = this.Cuitrepresentanteentregador;
            consignacion.Cuitdestinatario = this.Cuitdestinatario;
            return consignacion;     
        }
        public virtual void SetCuitConsignacion(Consignacion consignacion)
        {
            this.Cuitorigen = consignacion.Cuitsolicitante;
            this.Cuitintermediario = consignacion.Cuitintermediario;
            this.Cuitremcomercial = consignacion.Cuitrtecomercial;
            this.Cuitcorredorc = consignacion.Cuitcorrcomp;
            this.Cuitmercadoatermino = consignacion.Cuitmat;
            this.Cuitcorredorv = consignacion.Cuitcorrvta;
            this.Cuitrepresentanteentregador = consignacion.Cuitrteent;
            this.Cuitdestinatario = consignacion.Cuitdestinatario;
        }
    }
}