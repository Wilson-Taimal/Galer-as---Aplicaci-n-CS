using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Galerias.Core;
using Galerias.Model;

namespace Galerias.View
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            prueba();
        }

        public void prueba()
        {
            Desarrollo desarrollo = new Desarrollo();

            // -------------------------------------------------------------------------------------------------------------------------------
            // GEOMETRÍA //
            GeomEstructura GeomEst = new GeomEstructura();
            GeomEst.el = 0.25;
            GeomEst.h2 = 1.50;
            GeomEst.h3 = 1.00;
            GeomEst.a = 1.10;
            GeomEst.b = 0.80;
            GeomEst.em = 0.25;
            GeomEst.et = 0.25;
            GeomEst.L = 7.00;
            GeomEst.HT = Math.Round (GeomEst.h2 + GeomEst.el , 2);
            GeomEst.Di = Math.Round (GeomEst.a + GeomEst.b + GeomEst.et , 2);
            GeomEst.Bc = Math.Round ((2*GeomEst.em) + GeomEst.Di , 2);


            // -------------------------------------------------------------------------------------------------------------------------------
            // MATERIALES //
            Materiales materiales = new Materiales();
            materiales.fc = 28;
            materiales.fy = 420;
            materiales.fiv = 0.75;
            materiales.rc = 24;
            materiales.Ecuacion = 0;
            materiales.Ec = materiales.ElasticidadConcreto(materiales.Ecuacion, materiales.fc);


            // -------------------------------------------------------------------------------------------------------------------------------
            // SUELO //
            Suelo suelo = new Suelo();            
            suelo.Qadm = 110;
            suelo.rs = 18;
            suelo.fis = 25;
            suelo.rsat = 19;
            suelo.rw = 10;
            suelo.H1 = 0.20;


            // -------------------------------------------------------------------------------------------------------------------------------
            // CARGAS //
            Cargas cargas = new Cargas();
            cargas.DC = 284.46;
            cargas.DG = 8.0;
            cargas.DT = 0.60;
            cargas.SentidoMuro = 0;
            List<ResultCargas> resulCargas = desarrollo.CalculoEmpujes(GeomEst, suelo, cargas);


            // -------------------------------------------------------------------------------------------------------------------------------
            // ESTABILIDAD //
            Estabilidad estabilidad = new Estabilidad();
            estabilidad.Qs = 25.20;
            estabilidad.Si = 0.21;
            estabilidad.Sadm = 2.54;
            List<ResultEstabilidad> resultEstabilidad = desarrollo.CalculoEstabiliada(estabilidad, suelo);


            // -------------------------------------------------------------------------------------------------------------------------------
            // DISEÑO LOSA //
            DiseñoElementos diseñoElementos = new DiseñoElementos();
            Refuerzo refuerzo = new Refuerzo();
            Losas losas = new Losas();
            losas.b = 100;
            losas.rs = 5;
            losas.ri = 7.5;
            losas.pmin = 0.0018;
            losas.Nbs = 5;
            losas.Nbi = 6;
            losas.h = GeomEst.el * 100;
            losas.ds = losas.h - losas.rs;
            losas.di = losas.h - losas.ri;
            losas.d =  Math.Min ( losas.di , losas.ds);

            // Solicitaciones sobre la losa
            double[] Mun = { 750, 1500 };
            double[] Mup = { 2000, 7000};
            double[] Vu = { 30, 115 };
            double[] Msn = { 600, 1200 };
            double[] Msp = { 1500, 5500 };
            losas.LMun = Mun;
            losas.LMup = Mup;
            losas.LVu = Vu;
            losas.LMsn = Msn;
            losas.LMsp = Msp;

            // Canidad de barras a aumentar para cumplir fisuración
            losas.Cantt_ps = 7;
            losas.Cantt_pi = 7;

            List<ResultLosas> resultLosas = desarrollo.CalculoLosas(materiales, losas, diseñoElementos, refuerzo);


            // -------------------------------------------------------------------------------------------------------------------------------
            // DISEÑO MUROS //            
            Muros muros = new Muros();
            muros.b = 100;
            muros.r = 5;
            muros.pminv = 0.0015;
            muros.pminh = 0.0025;
            muros.Nbv = 5;
            muros.Nbh = 5;
            muros.h = GeomEst.em * 100;
            muros.d = muros.h - muros.r;

            // Solicitaciones sobre el muro
            double[] Muv = { 1500, 7000 };
            double[] Muh = { 3000, 7000 };
            double[] _Vu = { 112, 130 };
            double[] Msv = { 1200, 6000 };
            double[] Msh = { 2500, 6000 };
            muros.LMuy = Muv;
            muros.LMux = Muh;
            muros.LVu = _Vu;
            muros.LMsy = Msv;
            muros.LMsx = Msh;

            // Canidad de barras a aumentar para cumplir fisuración
            muros.Cantt_pv = 7;
            muros.Cantt_ph = 7;

            List<ResultMuros> resultMuros = desarrollo.CaculoMuros(materiales, muros, diseñoElementos, refuerzo);


            // -------------------------------------------------------------------------------------------------------------------------------
            // DISEÑO VIGAS //           
            Vigas vigas = new Vigas();
            vigas.b = 25;
            vigas.r = 5;
            vigas.pmin = 0.0033;
            vigas.Nb = 4;
            vigas.Nbe = 3;
            vigas.h = 30;
            vigas.d = vigas.h - vigas.r;
            
            // Solicitaciones sobre la viga
            double[] Mu = { 2059, 3801 };
            vigas.LMu = Mu;
            vigas.Vu = 56.67;           

            List<ResultVigas> resultVigas = desarrollo.CaculoVigas(materiales, vigas, diseñoElementos, refuerzo);
            
            
            // -------------------------------------------------------------------------------------------------------------------------------
            // DISEÑO COLUMNAS //

            Column column = new Column();
            column.b = 25;
            column.h = 30;
            column.pmin = 0.005;
            column.Nb = 4;
            column.Nbe = 3;
            column.CantReal = 4;

            // Solicitaciones sobre la columna          
            column.Pu = 56.67;
            
            List<ResultColumn> resultColumn = desarrollo.CaculoColumn(materiales, column, diseñoElementos, refuerzo);
        }

    }
}
