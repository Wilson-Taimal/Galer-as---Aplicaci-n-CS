using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galerias.Model
{
    public class Vigas
    {
        // -------------------------------------------------------------------------------------------------------------------------------
        // DATOS DE ENTRADA //

        /// <summary>
        /// Ancho de la viga _ [cm]
        /// </summary>
        public double b;

        /// <summary>
        /// Altura de la viga _ [cm]
        /// </summary>
        public double h;

        /// <summary>
        /// Recubrimiento del refuerzo _ [cm]
        /// </summary>
        public double r;

        /// <summary>
        /// Altura efectiva de la vigas _ [cm]
        /// </summary>
        public double d;

        /// <summary>
        /// Cuantia mínima de refuerzo en vigas
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
        /// Lista o vector de momentos de diseño {Mu(+), Mu(-)} _ [kN.cm]
        /// </summary>
        public double[] LMu;

        /// <summary>
        /// Fuerzas cortante maxíma en viga _ [kN]
        /// </summary>
        public double Vu;


        // -------------------------------------------------------------------------------------------------------------------------------
        // METODOS //

        public string ChequeoCortante(double Vu, double Vn)
        {
            string Opcion1 = "Cumple";
            string Opcion2 = "No cumple";

            if (Vn > Vu)
            { return Opcion1; }
            else
            { return Opcion2; }

        }

        public double SepEstribos(double d, double db, double dbe)
        {
            double Sa = Math.Min(30 , d/4);
            double Sb = Math.Min(8 * db, 24 * dbe);
            double S1 = Math.Min(Sa , Sb);

            return Math.Round(S1, 2);
        }

    }
}
