using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galerias.Model
{
    public class ResultVigas
    {
        /// <summary>
        /// preq: Cuantía de refuerzo requerida para un momento ultimo de diseño dado
        /// </summary>
        public double preq { get; set; }

        /// <summary>
        /// Asreq: Área de acero requerido para una cuantia requerida _ [cm²]
        /// </summary>        
        public double Asreq { get; set; }

        /// <summary>
        /// Asmin: Área de acero mínimo de la sección _ [cm²]
        /// </summary>
        public double Asmin { get; set; }

        /// <summary>
        /// As: Área de acero obtenido por diseño igual al maximo entre Asreq y Asmin _ [cm²]
        /// </summary>
        public double As { get; set; }

        /// <summary>
        /// Asb: Área de la sección transversal de la barra selecionada (refuerzo longitudinal) _ [cm²]
        /// </summary>
        public double Asb { get; set; }


        /// <summary>
        /// Asb: Área de la sección transversal de la barra selecionada (Estribos) _ [cm²]
        /// </summary>
        public double Av { get; set; }

        /// <summary>
        /// db: Diámetro de la sección transversal de la barra selecionada (refuerzo longitudinal) _ [cm²]
        /// </summary>
        public double db { get; set; }


        /// <summary>
        /// dbe: Diámetro de la sección transversal de la barra selecionada (estribos) _ [cm²]
        /// </summary>
        public double dbe { get; set; }

        /// <summary>
        /// S1: Separación de estribos en zona confinada para una viga DMO _ [cm]
        /// </summary>        
        public double S1 { get; set; }

        /// <summary>
        /// S2: Separación de estribos en zona no confinada para una viga DMO _ [cm]
        /// </summary>        
        public double S2 { get; set; }

        /// <summary>
        /// Cantidad de barras refuerzo longitudinal
        /// </summary>
        public double Cant { get; set; }

        /// <summary>
        /// Mup: Momento de diseño _ [kN.cm]
        /// </summary>        
        public double Mu { get; set; }

        /// <summary>
        /// Vu: Fuerza cortante de diseño _ [kN]
        /// </summary>        
        public double Vu { get; set; }

        /// <summary>
        /// Vc: Resistencia del concreto al corte _ [kN]
        /// </summary>        
        public double Vc { get; set; }

        /// <summary>
        /// Vs: Resistencia del acero al corte (estribos) _ [kN]
        /// </summary>        
        public double Vs { get; set; }

        /// <summary>
        /// Vn: Resistencia nominal de la sección al corte _ [kN]
        /// </summary>        
        public double Vn { get; set; }

        /// <summary>
        /// Chequeo resistencia del consreto al cortante (øVc > Vu)
        /// </summary>
        public string ChequeoCortante { get; set; }

    }
}
