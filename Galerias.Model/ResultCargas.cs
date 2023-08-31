using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galerias.Model
{
    public class ResultCargas
    {
        /// <summary>
        /// Empuje horizontal del suelo _ [kN/m]
        /// </summary>
        public double EH { get; set; }

        /// <summary>
        /// Carga de presión de agua, nivel cero _ [kN/m]
        /// </summary>
        public double WA0 { get; set; }

        /// <summary>
        /// Carga de presión de agua, nivel igual a H1 _ [kN/m]
        /// </summary>
        public double WA1 { get; set; }

        /// <summary>
        /// Carga de presión de agua, nivel igual a H2 _ [kN/m]
        /// </summary>
        public double WA2 { get; set; }

        /// <summary>
        /// SobrEcarga de carga viva superior sobre muros perpendiculares al trafico _ [kN/m]
        /// </summary>
        public double LSs_per { get; set; }

        /// <summary>
        /// Sobrcarga de carga viva inferior sobre muros perpendiculares al trafico _ [kN/m]
        /// </summary>
        public double LSi_per { get; set; }

        /// <summary>
        /// Sobrcarga de carga viva superior sobre muros paralelos al trafico _ [kN/m]
        /// </summary>
        public double LSs_par { get; set; }

        /// <summary>
        /// Sobrcarga de carga viva inferior sobre muros paralelos al trafico _ [kN/m]
        /// </summary>
        public double LSi_par { get; set; }

    }
}
