using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galerias.Model
{
    public class Cargas
    {
        // -------------------------------------------------------------------------------------------------------------------------------
        // CARGAS VERTICALES //

        /// <summary>
        /// Peso propio de la estructura _ [kN]
        /// </summary>
        public double DC;

        /// <summary>
        /// Peso propio de los gabinetes _ [kN]
        /// </summary>
        public double DG;

        /// <summary>
        /// Peso propio tapas de concreto _ [kN/m²]
        /// </summary>
        public double DT;


        // -------------------------------------------------------------------------------------------------------------------------------
        // EMPUJES //

        /// <summary>
        /// Empuje horizontal del suelo _ [kN/m]
        /// </summary>
        public double EH;

        /// <summary>
        /// Carga de presión de agua, nivel 0 _ [kN/m]
        /// </summary>
        public double WA0;

        /// <summary>
        /// Carga de presión de agua, nivel igual a H1 _ [kN/m]
        /// </summary>
        public double WA1;

        /// <summary>
        /// Carga de presión de agua, nivel igual a H2 _ [kN/m]
        /// </summary>
        public double WA2;

        /// <summary>
        /// Sobrecarga de carga viva _ [kN/m]
        /// </summary>
        public double LS;

        /// <summary>
        /// Ubicación o sentido del muro con relación al tráfico. 
        /// Muros perpendiculares al tráfico = 0
        /// Muros paralelos al trafico = 1
        /// </summary>
        public int SentidoMuro;


        // -------------------------------------------------------------------------------------------------------------------------------
        // METODOS // 

        public double EmpujeHorizontal(double fis, double rs, double HT)
        {
            double Ko = 1 - Math.Sin(fis * Math.PI / 180);
            double EH = Ko * rs * HT;
            return Math.Round(EH, 2);
        }

        public double PresionAgua0(double fis, double rs)
        {
            double Ko = 1 - Math.Sin(fis * Math.PI / 180);
            double WA1 = Ko * rs * 0;
            return Math.Round(WA1, 2);
        }

            public double PresionAgua1(double fis, double rs, double H1)
        {
            double Ko = 1 - Math.Sin(fis * Math.PI / 180);
            double WA1 = Ko * rs * H1;
            return Math.Round(WA1, 2);           
        }

        public double PresionAgua2(double HT, double H1, double fis, double rs, double rsat, double rw)
        {
            double H2 = HT - H1;
            double refe = rsat - rw;
            double Ko = 1 - Math.Sin(fis * Math.PI / 180);
            double WA2 = (Ko * (rs*H1 + refe*H2)) + rw*H2 ;
            return Math.Round(WA2, 2);
        }

        public double SobrecargaVivaS_per(double fis, double rs)
        {
            double Heqs = 1.20;
            double Ko = 1 - Math.Sin(fis * Math.PI / 180);
            double LSs_per = Ko * rs * Heqs;
            return Math.Round(LSs_per, 2);
        }

        public double SobrecargaVivaI_per(double HT, double fis, double rs)
        {
            if (HT < 1.50)
            { 
                double Heqi = 1.20; 
                double Ko = 1 - Math.Sin(fis * Math.PI / 180);
                double LSi_per = Ko * rs * Heqi;
                return Math.Round(LSi_per, 2);
            }
            else
            { 
                double Heqi = -0.433* Math.Log(HT) + 1.3755; 
                double Ko = 1 - Math.Sin(fis * Math.PI / 180);
                double LSi_per = Ko * rs * Heqi;
                return Math.Round(LSi_per, 2);
            }      
            
        }

        public double SobrecargaVivaS_par(double fis, double rs)
        {
            double Heqs = 1.50;
            double Ko = 1 - Math.Sin(fis * Math.PI / 180);
            double LSs_par = Ko * rs * Heqs;
            return Math.Round(LSs_par, 2);
        }

        public double SobrecargaVivaI_par(double HT, double fis, double rs)
        {
            if (HT < 1.50)
            {
                double Heqi = 1.20;
                double Ko = 1 - Math.Sin(fis * Math.PI / 180);
                double LSi_par = Ko * rs * Heqi;
                return Math.Round(LSi_par, 2);
            }
            else
            {
                double Heqi = -0.649 * Math.Log(HT) + 1.7466;
                double Ko = 1 - Math.Sin(fis * Math.PI / 180);
                double LSi_par = Ko * rs * Heqi;
                return Math.Round(LSi_par, 2);
            }

        }

    }
}
