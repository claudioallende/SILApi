using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class Cuerpo : Cupos
    {
        [Display(Name = "Id")]
        public override long Id { get; set; }
        [Display(Name = "Centro")]
        public override string Centro { get; set; }
        [Display(Name = "Contrato")]
        public override string Contrato { get; set; }
        public override long Compcta { get; set; }
        [Display(Name = "Fecha")]
        public override DateTime Fecha { get; set; }
        [Display(Name = "Cupo Nro.")]
        public override string Nrocupo { get; set; }
        [Display(Name = "Status")]
        public override int Status { get; set; }
        [Display(Name = "Origen")]
        public override Int64 Idorigen { get; set; }
        [Display(Name = "Cuenta Vendedor")]
        public override Int64 Vendcta { get; set; }
        public override long Vendcyo { get; set; }
        [Display(Name = "Solicitante")]
        public override string Cuitsolicitante { get; set; }
        [Display(Name = "Nombre")]
        public override string Nomsolicitante { get; set; }
        [Display(Name = "Intermediario")]
        public override string Cuitintermediario { get; set; }
        [Display(Name = "Nombre")]
        public override string Nomintermediario { get; set; }
        [Display(Name = "Comercial")]
        public override string Cuitrtecomercial { get; set; }
        [Display(Name = "Nombre")]
        public override string Nomrtecomercial { get; set; }
        [Display(Name = "Corredor Comprador")]
        public override string Cuitcorrcomp { get; set; }
        [Display(Name = "Nombre")]
        public override string Nomcorrcomp { get; set; }
        [Display(Name = "Mercado a Termino")]
        public override string Cuitmat { get; set; }
        [Display(Name = "Nombre")]
        public override string Nommat { get; set; }
        [Display(Name = "Corredor Vendedor")]
        public override string Cuitcorrvta { get; set; }
        [Display(Name = "Nombre")]
        public override string Nomcorrvta { get; set; }
        [Display(Name = "Entregador")]
        public override string Cuitrteent { get; set; }
        [Display(Name = "Nombre")]
        public override string Nomrteent { get; set; }
        [Display(Name = "Destinatario")]
        public override string Cuitdestinatario { get; set; }
        [Display(Name = "Nombre")]
        public override string Nomdestinatario { get; set; }
        public override string Motbaja { get; set; }
    }
}