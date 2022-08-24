using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceServer.Models.DataAccess
{
    interface IDistribucionStore
    {
        void Delete(string Id);
        int AgregarDistribucion(CuposDist cuposDist, Cupos cupoCuerpo, Cupos cupoEncabezado, Cupos nuevoCuerpoCYO, Cupos editoCuerpoCYOOrigen);
        int AnularDistribucion(CuposDist cuposDist, Cupos cupoCuerpo, Cupos cupoEncabezado);
        void AnularDistribucionCYO(CuposDist cupoDistAnulado, Cupos cupo, Cupos CupoCYORelacionado, Cupos encabezado);
        void Update(CuposDist Distribucion, Cupos CupoCuerpo, Cupos CupoEncabezado);
        void Update(CuposDist Distribucion, Cupos CupoPadreCyO, Cupos CupoHijoCyO, Cupos CupoEncabezado);
        void NuevaDistribucion(CuposDist Distribucion, Cupos CupoCuerpo, Cupos CupoEncabezado);
        void NuevaDistribucion(CuposDist Distribucion, Cupos CupoCuerpo);
        void NuevaDistribucion(long Distribucion, Cupos Cupo, ISession Session);
        //Borra al CupoHijoCyO
        void AnularCupoCyO(CuposDist Distribucion, Cupos CupoPadreCyO, Cupos CupoHijoCyO, Cupos CupoEncabezado);
        //void DistribuirCupo(Cupos CupoEncabezado, Cupos CupoCuerpo, CuposDist Distribucion);
        //void DistribuirCupoCyO(Cupos CupoEncabezado, Cupos CupoCuerpo, CuposDist Distribucion);
    }
}
