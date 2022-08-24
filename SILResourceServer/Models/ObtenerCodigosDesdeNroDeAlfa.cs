using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class ObtenerCodigosDesdeNroDeAlfa: IObtenerCodigosAlfanumericos
    {
        private string NroDeAlfa = "";
        private DateTime dia = new DateTime();

        public ObtenerCodigosDesdeNroDeAlfa(string nro, DateTime elDia) 
        {         
            this.NroDeAlfa = nro;
            this.dia = elDia;
        }

        public string[] obtenerCodigosAlfanumericos()
        {
            //ROS + Valor aleatorio (4 dígitos) + fecha formato DDMMYYYY
            Random rnd = new Random();
            int CantidadAlfa = int.Parse(this.NroDeAlfa);
            string elDia = this.dia.ToString("dd'/'MM'/'yyyy").Replace("/", "");
            IList<string> result = new List<string>();
            //Cuando hace result.Distinct(), si tiene mucha cantidad de cupos, puede que genere menos cantidad 
            //debido a que se repiten. 
            //Con el while nos aseguramos de que si la cantidad que se generan sea igual a la que se pide.
            while (result.Count() < CantidadAlfa)
            {
                for (int i = result.Count(); i < CantidadAlfa; i++)
                {
                    result.Add("ROS" + rnd.Next(65535).ToString("X") + elDia);
                }
                result = result.Distinct().ToList();
            }
            return result.ToArray();
        }
    }
}