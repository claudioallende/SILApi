using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Email
{
    public class InfoDeCupoPendDeCTG
    {
        private string _Cupo;
        private DateTime _FechaInformado;
        private string _Informado;

       
        public virtual string Cupo
        {
            get { return _Cupo; }
            set
            {
                if (value == null)
                    _Cupo = "Sin Informacion de Alfa";
                else
                    _Cupo = value;
            }
        }

        public virtual DateTime FechaInformado
        {
            get { return _FechaInformado; }
            set
            {
                if (value == null)
                    _FechaInformado = default(DateTime);
                else
                    _FechaInformado = value;
            }
        }

        public virtual string Informado
        {
            get {
                if (_FechaInformado == default(DateTime))
                    return  "&nbsp;&nbsp;<font  color='blue'>Sin Información</font>";
                else
                    return "&nbsp;&nbsp;<font  color='blue'>" + ((DateTime)_FechaInformado).ToString("dddd, dd MMMM yyyy HH:mm:ss", new CultureInfo("es-ES")) + " Hs.</font>";                    
            }
            set
            {
                    if (_FechaInformado == default(DateTime))
                        _Informado = "&nbsp;&nbsp;<font  color='blue'>Sin Información</font>";
                    else
                        _Informado = "&nbsp;&nbsp;<font  color='blue'>" + ((DateTime)_FechaInformado).ToString("dddd, dd MMMM yyyy HH:mm:ss", new CultureInfo("es-ES")) + " Hs.</font>";                    
            }
        }
    }
}