using ResourceServer.Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Pdf
{
    public class ServicioPdfDistribucion
    {
        private IPuertoStore PuertoStore { get; set; }
        private IGranoStore GranoStore { get; set; }
        private IList<Cupos> Cupos { get; set; }

        public ServicioPdfDistribucion(IList<Cupos> cupos)
        {
            PuertoStore = new PuertoStore();
            GranoStore = new GranoStore();
            Cupos = cupos;
        }
        public Cupos GetCupo()
        {
            return Cupos.ElementAt(0);
        }
        public IList<string> GetAlfanumericosAInformar()
        {
            return Cupos.Select(x => x.Nrocupo).ToList();
        }
        public Puerto GetPuerto(long cuenta)
        {
            return PuertoStore.FindById(cuenta);
        }
        public string GetNroPlanta(long cuentapuerto)
        {
            return PuertoStore.FindNroPlantaByCuentaPuerto(cuentapuerto);
        }
        public Grano GetGrano(int codigo)
        {
            return GranoStore.FindById(codigo.ToString());
        }
    }
}