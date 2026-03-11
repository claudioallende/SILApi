using ResourceServer.Models.Cupo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceServer.Models
{
    public interface ICupo
    {
         long Id { get; set; }
         int Grano { get; set; }
         Int64 Compcta { get; set; }
         Int64 Puerto { get; set; }
         Int64 Vendcta { get; set; }
         Int64 Vendcyo { get; set; }
         string Cuitsolicitante { get; set; }
         string Nomsolicitante { get; set; }
         string Cuitintermediario { get; set; }
         string Nomintermediario { get; set; }
         string Cuitrtecomercial { get; set; }
         string Nomrtecomercial { get; set; }
         string Cuitcorrcomp { get; set; }
         string Nomcorrcomp { get; set; }
         string Cuitmat { get; set; }
         string Nommat   { get; set; }
         string Cuitcorrvta { get; set; }
         string Nomcorrvta  { get; set; }
         string Cuitrteent  { get; set; }
         string Nomrteent   { get; set; }
         string Cuitdestinatario { get; set; }
         string Nomdestinatario { get; set; }
         string Alfadia0 { get; set; }
         string Alfadia1 { get; set; }
         string Alfadia2 { get; set; }
         string Alfadia3 { get; set; }
         string Alfadia4 { get; set; }
         string Alfadia5 { get; set; }
         string Centro { get; set; }
         string Centrodist { get; set; }
         string Contrato { get; set; }
         long Corrcta { get; set; }
         int Cuposotorgados { get; set; }
         int Cupospedidos { get; set; }
         int Cuposrecibidos { get; set; }
         DateTime Dia0 { get; set; }
         DateTime Dia1 { get; set; }
         DateTime Dia2 { get; set; }
         DateTime Dia3 { get; set; }
         DateTime Dia4 { get; set; }
         DateTime Dia5 { get; set; }
         DateTime Dia6 { get; set; }
         DateTime Dia7 { get; set; }
         DateTime Dia8 { get; set; }
         DateTime Dia9 { get; set; }
         DateTime Dia10 { get; set; }
         DateTime Dia11 { get; set; }
         DateTime Dia12 { get; set; }
         DateTime Dia13 { get; set; }
         DateTime Dia14 { get; set; }
         DateTime Dia15 { get; set; }
         DateTime Dia16 { get; set; }
         DateTime Dia17 { get; set; }
         DateTime Dia18 { get; set; }
         DateTime Dia19 { get; set; }
         DateTime Dia20 { get; set; }
         DateTime Fecha { get; set; }
         string Nrocupo { get; set; }
         int Status { get; set; }
         int Tipo { get; set; }
         Int64 Idorigen { get; set; }
         long Uvcupodist { get; set; }
         string Motbaja { get; set; }
         string Observa { get; set; }
         int Pdf { get; set; }
         EstadoCupo Estado { get; set; }
         int EstadoCupoCNRT { get; set; }
         string Usuario { get; set; }
         string DetalleCupoCNRT { get; set; }
         DateTime? FechaYHoraInformado { get; set; }

         int Otorgados { get; set; }
         int Cumplidos { get; set; }
         int Perdidos { get; set; }
         decimal PorcentajeDePerdida { get; set; }

         bool VendcyoBoolValue { get; set; }
         object Clone();
         Consignacion GetConsignacion();
         Consignacion GetConsignacionTrim();
         bool EsCuentaYOrden();
         bool EsCuentaYOrdenPadre();
         EstadoCupo GetEstado();
         void SetConsignacion(Consignacion consignacion);
         bool PuedeOperar();
         Cupos NuevoCupoHijo();
         ClaveCupo GetClave();
         string CondicionGrano { get; set; }
  }
}
