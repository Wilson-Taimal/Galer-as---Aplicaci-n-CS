using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galerias.Model
{
    public class Column
    {
        // -------------------------------------------------------------------------------------------------------------------------------
        // DATOS DE ENTRADA //

        /// <summary>
        /// Ancho de la Columna _ [cm]
        /// </summary>
        public double b;

        /// <summary>
        /// Altura de la columna _ [cm]
        /// </summary>
        public double h;

        /// <summary>
        /// Cuantía mínima de refuerzo en columnas
        /// </summary>
        public double pmin;

        /// <summary>
        /// Número de la barra a emplear refuerzo longitudinal
        /// </summary>
        public int Nb;

        /// <summary>
        /// Número de la barra a emplear refuerzo transversal (estribos)
        /// </summary>
        public int Nbe;

        /// <summary>
        /// Lista o vector de momentos para diagrama de interación M2 _ [kN.cm]
        /// </summary>
        public double[] LM2;

        /// <summary>
        /// Lista o vector de momentos para diagrama de interación M3 _ [kN.cm]
        /// </summary>
        public double[] LM3;

        /// <summary>
        /// Lista o vector de carga axial para diagrama de interación M3 _ [kN.cm]
        /// </summary>
        public double[] LP;

        /// <summary>
        /// Carga axial maxíma en la columna _ [kN]
        /// </summary>
        public double Pu;

        /// <summary>
        /// Cantidad final de barras de refuerzo en la columna
        /// </summary>
        public int CantReal;


        // -------------------------------------------------------------------------------------------------------------------------------
        // METODOS //

        public double SepEstribos(double h, double b, double db, double dbe)
        {
            double Sa = Math.Min(Math.Min(h,b)/3 , 8*db);
            double Sb = Math.Min(16 * dbe , 15);
            double S1 = Math.Min(Sa, Sb);

            return Math.Round(S1, 2);
        }

        public string ChequeoCargaAxial(double Pu, double Pn)
        {
            string Opcion1 = "Cumple";
            string Opcion2 = "No cumple";

            if (Pn > Pu)
            { return Opcion1; }
            else
            { return Opcion2; }

        }
    }
}
