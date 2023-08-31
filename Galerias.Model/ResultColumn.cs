using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galerias.Model
{
    public class ResultColumn
    {
        /// <summary>
        /// Ag: Área bruta de la columna _ [cm²]
        /// </summary>
        public double Ag { get; set; }

        /// <summary>
        /// Asmin: Área de acero mínimo de la sección _ [cm²]
        /// </summary>
        public double Asmin { get; set; }

        /// <summary>
        /// Ast: Área de acero total en la columna _ [cm²]
        /// </summary>
        public double Ast { get; set; }

        /// <summary>
        /// Astf: Distribucion final de refuerzo longitudinal en la columna _ [cm²]
        /// </summary>
        public double Astf { get; set; }

        /// <summary>
        /// Asb: Área de la sección transversal de la barra selecionada (refuerzo longitudinal) _ [cm²]
        /// </summary>
        public double Asb { get; set; }

        /// <summary>
        /// Av: Área de la sección transversal de la barra selecionada (Estribos) _ [cm²]
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
        /// S1: Separación de estribos en zona confinada para una columna DMO _ [cm]
        /// </summary>        
        public double S1 { get; set; }

        /// <summary>
        /// S2: Separación de estribos en zona no confinada para una columna DMO _ [cm]
        /// </summary>        
        public double S2 { get; set; }

        /// <summary>
        /// Cantidad de barras refuerzo longitudinal
        /// </summary>
        public double Cant { get; set; }

        /// <summary>
        /// Pu: Carga axial de diseño _ [kN]
        /// </summary>        
        public double Pu { get; set; }

        /// <summary>
        /// Pn: Resistencia nominal a carga axial _ [kN]
        /// </summary>        
        public double Pn { get; set; }

        /// <summary>
        /// Chequeo resistencia del consreto al cortante (øVc > Vu)
        /// </summary>
        public string ChequeoCargaAxial { get; set; }

    }
}
