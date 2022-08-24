using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class CuposAgrupadosPorVendedor
    {
        [Display(Name = "Grano")]
        public virtual int GRANO { get; set; }	
        [Display(Name = "NombreGrano")]
        public virtual string NOMGRANO { get; set; }	
        [Display(Name = "Comprador")]
        public virtual Int64 COMPCTA { get; set; }
        public virtual string COMPRADOR { get; set; }
        [Display(Name = "Puerto")]
        public virtual Int64 PUERTOCTA { get; set; }
        public virtual string PUERTO { get; set; }
        [Display(Name = "Vendedor")]
        public virtual Int64 VENDCTA { get; set; }
        public virtual string VENDEDOR { get; set; }
        [Display(Name = "InformadoStop")]
        public virtual int INFORMADOSTOP { get; set; }
        [Display(Name = "Cupos Otorgados")]
        public virtual int D0CO { get; set; }
        [Display(Name = "Cupos Cumplido")]
        public virtual int D0CC { get; set; }
        [Display(Name = "Cupos Otorgados")]
        public virtual int D1CO { get; set; }
        [Display(Name = "Cupos Cumplido")]
        public virtual int D1CC { get; set; }
        [Display(Name = "Cupos Otorgados")]
        public virtual int D2CO { get; set; }
        [Display(Name = "Cupos Cumplido")]
        public virtual int D2CC { get; set; }
        [Display(Name = "Cupos Otorgados")]
        public virtual int D3CO { get; set; }
        [Display(Name = "Cupos Cumplido")]
        public virtual int D3CC { get; set; }
        [Display(Name = "Cupos Otorgados")]
        public virtual int D4CO { get; set; }
        [Display(Name = "Cupos Cumplido")]
        public virtual int D4CC { get; set; }
        [Display(Name = "Cupos Otorgados")]
        public virtual int D5CO { get; set; }
        [Display(Name = "Cupos Cumplido")]
        public virtual int D5CC { get; set; }
        [Display(Name = "Cupos Otorgados")]
        public virtual int D6CO { get; set; }
        [Display(Name = "Cupos Cumplido")]
        public virtual int D6CC { get; set; }
        [Display(Name = "Cupos Otorgados")]
        public virtual int D7CO { get; set; }
        [Display(Name = "Cupos Cumplido")]
        public virtual int D7CC { get; set; }
        [Display(Name = "Cupos Otorgados")]
        public virtual int D8CO { get; set; }
        [Display(Name = "Cupos Cumplido")]
        public virtual int D8CC { get; set; }
        [Display(Name = "Cupos Otorgados")]
        public virtual int D9CO { get; set; }
        [Display(Name = "Cupos Cumplido")]
        public virtual int D9CC { get; set; }
        [Display(Name = "Cupos Otorgados")]
        public virtual int D10CO { get; set; }
        [Display(Name = "Cupos Cumplido")]
        public virtual int D10CC { get; set; }
        [Display(Name = "Cupos Otorgados")]
        public virtual int D11CO { get; set; }
        [Display(Name = "Cupos Cumplido")]
        public virtual int D11CC { get; set; }
        [Display(Name = "Cupos Otorgados")]
        public virtual int D12CO { get; set; }
        [Display(Name = "Cupos Cumplido")]
        public virtual int D12CC { get; set; }
        [Display(Name = "Cupos Otorgados")]
        public virtual int D13CO { get; set; }
        [Display(Name = "Cupos Cumplido")]
        public virtual int D13CC { get; set; }
        [Display(Name = "Cupos Otorgados")]
        public virtual int D14CO { get; set; }
        [Display(Name = "Cupos Cumplido")]
        public virtual int D14CC { get; set; }
        [Display(Name = "Cupos Otorgados")]
        public virtual int D15CO { get; set; }
        [Display(Name = "Cupos Cumplido")]
        public virtual int D15CC { get; set; }
        [Display(Name = "Cupos Otorgados")]
        public virtual int D16CO { get; set; }
        [Display(Name = "Cupos Cumplido")]
        public virtual int D16CC { get; set; }
        [Display(Name = "Cupos Otorgados")]
        public virtual int D17CO { get; set; }
        [Display(Name = "Cupos Cumplido")]
        public virtual int D17CC { get; set; }
        [Display(Name = "Cupos Otorgados")]
        public virtual int D18CO { get; set; }
        [Display(Name = "Cupos Cumplido")]
        public virtual int D18CC { get; set; }
        [Display(Name = "Cupos Otorgados")]
        public virtual int D19CO { get; set; }
        [Display(Name = "Cupos Cumplido")]
        public virtual int D19CC { get; set; }
        [Display(Name = "Cupos Otorgados")]
        public virtual int D20CO { get; set; }
        [Display(Name = "Cupos Cumplido")]
        public virtual int D20CC { get; set; }    
        public virtual string HOY { get; set; }
        public virtual string DIA1 { get; set; }
        public virtual string DIA2 { get; set; }
        public virtual string DIA3 { get; set; }
        public virtual string DIA4 { get; set; }
        public virtual string DIA5 { get; set; }
        public virtual string DIA6 { get; set; }
        public virtual string DIA7 { get; set; }
        public virtual string DIA8 { get; set; }
        public virtual string DIA9 { get; set; }
        public virtual string DIA10 { get; set; }
        public virtual string DIA11 { get; set; }
        public virtual string DIA12 { get; set; }
        public virtual string DIA13 { get; set; }
        public virtual string DIA14 { get; set; }
        public virtual string DIA15 { get; set; }
        public virtual string DIA16 { get; set; }
        public virtual string DIA17 { get; set; }
        public virtual string DIA18 { get; set; }
        public virtual string DIA19 { get; set; }
        public virtual string DIA20 { get; set; }
        [Display(Name = "Solicitante")]
        public virtual string CUITSOLICITANTE { get; set; }
        [Display(Name = "Nombre")]
        public virtual string NOMSOLICITANTE { get; set; }
        [Display(Name = "Intermediario")]
        public virtual string CUITINTERMEDIARIO { get; set; }
        [Display(Name = "Nombre")]
        public virtual string NOMINTERMEDIARIO { get; set; }
        [Display(Name = "Comercial")]
        public virtual string CUITRTECOMERCIAL { get; set; }
        [Display(Name = "Nombre")]
        public virtual string NOMRTECOMERCIAL { get; set; }
        [Display(Name = "Corredor Comprador")]
        public virtual string CUITCORRCOMP { get; set; }
        [Display(Name = "Nombre")]
        public virtual string NOMCORRCOMP { get; set; }
        [Display(Name = "Mercado a Termino")]
        public virtual string CUITMAT { get; set; }
        [Display(Name = "Nombre")]
        public virtual string NOMMAT { get; set; }
        [Display(Name = "Corredor Vendedor")]
        public virtual string CUITCORRVTA { get; set; }
        [Display(Name = "Nombre")]
        public virtual string NOMCORRVTA { get; set; }
        [Display(Name = "Entregador")]
        public virtual string CUITRTEENT { get; set; }
        [Display(Name = "Nombre")]
        public virtual string NOMRTEENT { get; set; }
        [Display(Name = "Destinatario")]
        public virtual string CUITDESTINATARIO { get; set; }
        [Display(Name = "Nombre")]
        public virtual string NOMDESTINATARIO { get; set; }
        [Display(Name = "Remitente Comercial Productor")]
        public virtual string CuitRteComercialProductor { get; set; }
        [Display(Name = "Nombre")]
        public virtual string NomRteComercialProductor { get; set; }
        [Display(Name = "Nombre")]
        public virtual string NomRteComercialVentaPrimaria { get; set; }
        [Display(Name = "Remitente Comercial Venta Primaria")]
        public virtual string CuitRteComercialVentaPrimaria { get; set; }
    public virtual int CYO { get; set; }

        public override bool Equals(object obj)
        {
            CuposAgrupadosPorVendedor recievedObject = (CuposAgrupadosPorVendedor)obj;

            if (ReferenceEquals(recievedObject, null)) return false;
            if (ReferenceEquals(recievedObject, this)) return true;
            if ((this.GRANO == recievedObject.GRANO) &&
                (this.COMPCTA == recievedObject.COMPCTA) &&
                (this.PUERTOCTA == recievedObject.PUERTOCTA) &&
                (this.VENDCTA == recievedObject.VENDCTA) &&
                (this.INFORMADOSTOP == recievedObject.INFORMADOSTOP)
                )
            {
                return (true);
            }
            return (false);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}