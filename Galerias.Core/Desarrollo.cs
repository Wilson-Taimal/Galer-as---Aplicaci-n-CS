using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galerias.Model;

namespace Galerias.Core
{
    public class Desarrollo
    {
        // CÁLCULO DE CARGAS Y EMPUJES //
        public List<ResultCargas> CalculoEmpujes(GeomEstructura GeomEst, Suelo suelo, Cargas cargas)
        {
            List<ResultCargas> listaresultado = new List<ResultCargas>();
            ResultCargas resulCargas = new ResultCargas();

            if (suelo.H1 == 0)
            {
                resulCargas.EH = cargas.EmpujeHorizontal(suelo.fis, suelo.rs, GeomEst.HT);
            }
            else
            {
                resulCargas.WA0 = cargas.PresionAgua0(suelo.fis, suelo.rs);
                resulCargas.WA1 = cargas.PresionAgua1(suelo.fis, suelo.rs, suelo.H1);
                resulCargas.WA2 = cargas.PresionAgua2(GeomEst.HT, suelo.H1, suelo.fis, suelo.rs, suelo.rsat, suelo.rw);
            }

            if (cargas.SentidoMuro == 0)
            {
                resulCargas.LSs_per = cargas.SobrecargaVivaS_per(suelo.fis, suelo.rs);
                resulCargas.LSi_per = cargas.SobrecargaVivaI_per(GeomEst.HT, suelo.fis, suelo.rs);
            }
            else
            {
                resulCargas.LSs_par = cargas.SobrecargaVivaS_par(suelo.fis, suelo.rs);
                resulCargas.LSi_par = cargas.SobrecargaVivaI_par(GeomEst.HT, suelo.fis, suelo.rs);
            }

            listaresultado.Add(resulCargas);
            return listaresultado;
        }
        // ------------------------------------------------------------------------------------------------------------------------------- //



        // ===== ANALISIS DE ESTABILIDAD ===== //

        public List<ResultEstabilidad> CalculoEstabiliada(Estabilidad estabilidad, Suelo suelo)
        {
            List<ResultEstabilidad> listaresultado = new List<ResultEstabilidad>();
            ResultEstabilidad resultEstabilidad = new ResultEstabilidad();
            resultEstabilidad.Esfuerzo = estabilidad.Esfuerzo(estabilidad.Qs, suelo.Qadm);
            resultEstabilidad.Asentamiento = estabilidad.Asentamiento(estabilidad.Si, estabilidad.Sadm);

            listaresultado.Add(resultEstabilidad);
            return listaresultado;
        }
        // ------------------------------------------------------------------------------------------------------------------------------- //



        //  ===== DISEÑO LOSAS =====  //

        public List<ResultLosas> CalculoLosas(Materiales materiales, Losas losas, DiseñoElementos diseñoElementos, Refuerzo refuerzo)
        {
            List<ResultLosas> listaresultado = new List<ResultLosas>();

            // Parrilla superior
            for (int i = 0; i < losas.LMun.Length; i++)
            {
                ResultLosas resultLosas = new ResultLosas();
                resultLosas.Mun = losas.LMun[i];
                resultLosas.Msn = losas.LMsn[i];
                resultLosas.Asmin = diseñoElementos.AceroMinimo(losas.pmin, losas.b, losas.h);
                resultLosas.preq = diseñoElementos.Cuantia(materiales.fc, materiales.fy, losas.LMun[i], losas.b, losas.ds);
                resultLosas.Asreq = diseñoElementos.AceroRequerido(materiales.fc, materiales.fy, losas.LMun[i], losas.b, losas.ds);
                resultLosas.As = diseñoElementos.Acero(resultLosas.Asreq, resultLosas.Asmin);
                resultLosas.Asb = refuerzo.AreaBarra(losas.Nbs);
                resultLosas.Cant = diseñoElementos.CantBarras(resultLosas.As, resultLosas.Asb);
                resultLosas.Sep = diseñoElementos.SepBarras(losas.b, resultLosas.As, resultLosas.Asb);

                // Chqueo fisuracion
                resultLosas.fs = diseñoElementos.EsfuerzoAcero(materiales.Ec, resultLosas.preq, losas.LMsn[i], resultLosas.As, losas.ds);
                resultLosas.db = refuerzo.DiametroBarra(losas.Nbs);
                resultLosas.fsadm = diseñoElementos.EsfuerzoAdmisible(losas.h, resultLosas.db, resultLosas.Sep);

                if (resultLosas.fsadm >= 170 && resultLosas.fsadm <= 250 && resultLosas.fs <= resultLosas.fsadm)
                {
                    resultLosas.ChequeoFisuracion = diseñoElementos.ChequeoFisuracion(resultLosas.fs, resultLosas.fsadm);
                }
                else
                {
                    resultLosas.Msn = losas.LMsn[i];
                    resultLosas.Cant = losas.Cantt_ps;
                    resultLosas.Ascol = diseñoElementos.AceroColocado(resultLosas.Cant, resultLosas.Asb);
                    resultLosas.Sep = diseñoElementos.SepBarras(losas.b, resultLosas.Ascol, resultLosas.Asb);
                    resultLosas.preq = diseñoElementos.N_Cuantia(resultLosas.Ascol, losas.b, losas.ds);

                    // Chqueo fisuracion
                    resultLosas.fs = diseñoElementos.EsfuerzoAcero(materiales.Ec, resultLosas.preq, losas.LMsn[i], resultLosas.Ascol, losas.ds);
                    resultLosas.db = refuerzo.DiametroBarra(losas.Nbs);
                    resultLosas.fsadm = diseñoElementos.EsfuerzoAdmisible(losas.h, resultLosas.db, resultLosas.Sep);
                    resultLosas.ChequeoFisuracion = diseñoElementos.ChequeoFisuracion(resultLosas.fs, resultLosas.fsadm);
                }

                // Diseño por durabilidad
                resultLosas.Mureq = diseñoElementos.MomtUltimoRequ(resultLosas.Mun, resultLosas.Msn, materiales.fy, resultLosas.fs);
                resultLosas.preq = diseñoElementos.Cuantia(materiales.fc, materiales.fy, resultLosas.Mureq, losas.b, losas.ds);
                resultLosas.Asreq = diseñoElementos.AceroRequerido(materiales.fc, materiales.fy, resultLosas.Mureq, losas.b, losas.ds);
                resultLosas.Asmin = diseñoElementos.AceroMinimo(losas.pmin, losas.b, losas.h);
                resultLosas.As = diseñoElementos.Acero(resultLosas.Asreq, resultLosas.Asmin);
                resultLosas.Asb = refuerzo.AreaBarra(losas.Nbs);
                resultLosas.Cant = losas.Cantt_ps;
                resultLosas.Sep = diseñoElementos.SepBarras(losas.b, resultLosas.As, resultLosas.Asb);
                resultLosas.Ascol = diseñoElementos.AceroColocado(resultLosas.Cant, resultLosas.Asb);

                // Chequeos
                resultLosas.Ascol = diseñoElementos.AceroColocado(resultLosas.Cant, resultLosas.Asb);
                resultLosas.Mn = diseñoElementos.MomNominal(materiales.fc, materiales.fy, resultLosas.Ascol, losas.b, losas.ds);
                resultLosas.ChequeoMomentoNominal = losas.ChequeoMomentoNominal(resultLosas.Mureq, resultLosas.Mn);

                listaresultado.Add(resultLosas);
            }
            

            // Parilla inferior
            for (int i = 0; i < losas.LMup.Length; i++)
            {
                ResultLosas resultLosas = new ResultLosas();
                resultLosas.Mup = losas.LMup[i];
                resultLosas.Msp = losas.LMsp[i];
                resultLosas.Asmin = diseñoElementos.AceroMinimo(losas.pmin, losas.b, losas.h);
                resultLosas.preq = diseñoElementos.Cuantia(materiales.fc, materiales.fy, losas.LMup[i], losas.b, losas.di);
                resultLosas.Asreq = diseñoElementos.AceroRequerido(materiales.fc, materiales.fy, losas.LMup[i], losas.b, losas.di);
                resultLosas.As = diseñoElementos.Acero(resultLosas.Asreq, resultLosas.Asmin);
                resultLosas.Asb = refuerzo.AreaBarra(losas.Nbi);
                resultLosas.Cant = diseñoElementos.CantBarras(resultLosas.As, resultLosas.Asb);
                resultLosas.Sep = diseñoElementos.SepBarras(losas.b, resultLosas.As, resultLosas.Asb);

                // ChequeoFisuracion
                resultLosas.fs = diseñoElementos.EsfuerzoAcero(materiales.Ec, resultLosas.preq, losas.LMsp[i], resultLosas.As, losas.di);
                resultLosas.db = refuerzo.DiametroBarra(losas.Nbi);
                resultLosas.fsadm = diseñoElementos.EsfuerzoAdmisible(losas.h, resultLosas.db, resultLosas.Sep);

                if (resultLosas.fsadm >= 170 && resultLosas.fsadm <= 250 && resultLosas.fs <= resultLosas.fsadm)
                {
                    resultLosas.ChequeoFisuracion = diseñoElementos.ChequeoFisuracion(resultLosas.fs, resultLosas.fsadm);
                }
                else
                {
                    resultLosas.Msp = losas.LMsp[i];
                    resultLosas.Cant = losas.Cantt_pi;
                    resultLosas.Ascol = diseñoElementos.AceroColocado(resultLosas.Cant, resultLosas.Asb);
                    resultLosas.Sep = diseñoElementos.SepBarras(losas.b, resultLosas.Ascol, resultLosas.Asb);
                    resultLosas.preq = diseñoElementos.N_Cuantia(resultLosas.Ascol, losas.b, losas.di);

                    // Chqueo fisuracion
                    resultLosas.fs = diseñoElementos.EsfuerzoAcero(materiales.Ec, resultLosas.preq, losas.LMsp[i], resultLosas.Ascol, losas.di);
                    resultLosas.db = refuerzo.DiametroBarra(losas.Nbi);
                    resultLosas.fsadm = diseñoElementos.EsfuerzoAdmisible(losas.h, resultLosas.db, resultLosas.Sep);
                    resultLosas.ChequeoFisuracion = diseñoElementos.ChequeoFisuracion(resultLosas.fs, resultLosas.fsadm);
                }

                // Diseño por durabilidad
                resultLosas.Mureq = diseñoElementos.MomtUltimoRequ(resultLosas.Mup, resultLosas.Msp, materiales.fy, resultLosas.fs);
                resultLosas.preq = diseñoElementos.Cuantia(materiales.fc, materiales.fy, resultLosas.Mureq, losas.b, losas.di);
                resultLosas.Asreq = diseñoElementos.AceroRequerido(materiales.fc, materiales.fy, resultLosas.Mureq, losas.b, losas.di);
                resultLosas.Asmin = diseñoElementos.AceroMinimo(losas.pmin, losas.b, losas.h);
                resultLosas.As = diseñoElementos.Acero(resultLosas.Asreq, resultLosas.Asmin);
                resultLosas.Asb = refuerzo.AreaBarra(losas.Nbi);
                resultLosas.Cant = losas.Cantt_pi;
                resultLosas.Sep = diseñoElementos.SepBarras(losas.b, resultLosas.As, resultLosas.Asb);
                resultLosas.Ascol = diseñoElementos.AceroColocado(resultLosas.Cant, resultLosas.Asb);

                // Chequeos
                resultLosas.Ascol = diseñoElementos.AceroColocado(resultLosas.Cant, resultLosas.Asb);
                resultLosas.Mn = diseñoElementos.MomNominal(materiales.fc, materiales.fy, resultLosas.Ascol, losas.b, losas.di);
                resultLosas.ChequeoMomentoNominal = losas.ChequeoMomentoNominal(resultLosas.Mureq, resultLosas.Mn);

                listaresultado.Add(resultLosas);
            }

            // Chequeo Cortante
            for (int i = 0; i < losas.LVu.Length; i++)
            {
                ResultLosas resultLosas = new ResultLosas();
                resultLosas.Vu = losas.LVu[i];
                resultLosas.Vc = diseñoElementos.CortConcreto(materiales.fiv, materiales.fc, losas.b, losas.d);
                resultLosas.ChequeoCortante = losas.ChequeoCortante(resultLosas.Vu, resultLosas.Vc);

                listaresultado.Add(resultLosas);
            }

            return listaresultado;
        }
        // ------------------------------------------------------------------------------------------------------------------------------- //



        //  ===== DISEÑO MUROS =====  //

        public List<ResultMuros> CaculoMuros(Materiales materiales, Muros muros, DiseñoElementos diseñoElementos, Refuerzo refuerzo)
        {
            List<ResultMuros> listaresultado = new List<ResultMuros>();

            // Refuerzo Vertical
            for (int i = 0; i < muros.LMuy.Length; i++)
            {
                ResultMuros resultMuros = new ResultMuros();
                resultMuros.Muy = muros.LMuy[i];
                resultMuros.Msy = muros.LMsy[i];
                resultMuros.Asminv = diseñoElementos.AceroMinimo(muros.pminv, muros.b, muros.h);
                resultMuros.preq = diseñoElementos.Cuantia(materiales.fc, materiales.fy, muros.LMuy[i], muros.b, muros.d);
                resultMuros.Asreq = diseñoElementos.AceroRequerido(materiales.fc, materiales.fy, muros.LMuy[i], muros.b, muros.d);
                resultMuros.As = diseñoElementos.Acero(resultMuros.Asreq, resultMuros.Asminv);
                resultMuros.Asb = refuerzo.AreaBarra(muros.Nbv);
                resultMuros.Cant = diseñoElementos.CantBarras(resultMuros.As, resultMuros.Asb);
                resultMuros.Sep = diseñoElementos.SepBarras(muros.b, resultMuros.As, resultMuros.Asb);

                // Chequeo fisuración
                resultMuros.fs = diseñoElementos.EsfuerzoAcero(materiales.Ec, resultMuros.preq, muros.LMsy[i], resultMuros.As, muros.d);
                resultMuros.db = refuerzo.DiametroBarra(muros.Nbv);
                resultMuros.fsadm = diseñoElementos.EsfuerzoAdmisible(muros.h, resultMuros.db, resultMuros.Sep);

                if (resultMuros.fsadm >= 170 && resultMuros.fsadm <= 250 && resultMuros.fs <= resultMuros.fsadm)
                {
                    resultMuros.ChequeoFisuracion = diseñoElementos.ChequeoFisuracion(resultMuros.fs, resultMuros.fsadm);
                }
                else 
                {
                    resultMuros.Msy = muros.LMsy[i];
                    resultMuros.Cant = muros.Cantt_pv;
                    resultMuros.Ascol = diseñoElementos.AceroColocado(resultMuros.Cant, resultMuros.Asb);
                    resultMuros.Sep = diseñoElementos.SepBarras(muros.b, resultMuros.Ascol, resultMuros.Asb);
                    resultMuros.preq = diseñoElementos.N_Cuantia(resultMuros.Ascol, muros.b, muros.d);

                    // Cheqquo fisuración
                    resultMuros.fs = diseñoElementos.EsfuerzoAcero(materiales.Ec, resultMuros.preq, muros.LMsy[i], resultMuros.Ascol, muros.d);
                    resultMuros.db = refuerzo.DiametroBarra(muros.Nbv);
                    resultMuros.fsadm = diseñoElementos.EsfuerzoAdmisible(muros.h, resultMuros.db, resultMuros.Sep);
                    resultMuros.ChequeoFisuracion = diseñoElementos.ChequeoFisuracion(resultMuros.fs, resultMuros.fsadm);
                }

                // Diseño por durabilidad
                resultMuros.Mureq = diseñoElementos.MomtUltimoRequ(resultMuros.Muy, resultMuros.Msy, materiales.fy, resultMuros.fs);
                resultMuros.preq = diseñoElementos.Cuantia(materiales.fc, materiales.fy, resultMuros.Mureq, muros.b, muros.d);
                resultMuros.Asreq = diseñoElementos.AceroRequerido(materiales.fc, materiales.fy, resultMuros.Mureq, muros.b, muros.d);
                resultMuros.Asminv = diseñoElementos.AceroMinimo(muros.pminv, muros.b, muros.h);
                resultMuros.As = diseñoElementos.Acero(resultMuros.Asreq, resultMuros.Asminv);
                resultMuros.Asb = refuerzo.AreaBarra(muros.Nbv);
                resultMuros.Cant = muros.Cantt_pv;
                resultMuros.Sep = diseñoElementos.SepBarras(muros.b, resultMuros.As, resultMuros.Asb);
                resultMuros.Ascol = diseñoElementos.AceroColocado(resultMuros.Cant, resultMuros.Asb);

                // Chequeos
                resultMuros.Ascol = diseñoElementos.AceroColocado(resultMuros.Cant, resultMuros.Asb);
                resultMuros.Mn = diseñoElementos.MomNominal(materiales.fc, materiales.fy, resultMuros.Ascol, muros.b, muros.d);
                resultMuros.ChequeoMomentoNominal = muros.ChequeoMomentoNominal(resultMuros.Mureq, resultMuros.Mn);

                listaresultado.Add(resultMuros);
            }

            // Refuerzo Horizontal
            for (int i = 0; i < muros.LMux.Length; i++)
            {
                ResultMuros resultMuros = new ResultMuros();
                resultMuros.Mux = muros.LMux[i];
                resultMuros.Msx = muros.LMsx[i];
                resultMuros.Asminh = diseñoElementos.AceroMinimo(muros.pminh, muros.b, muros.h);
                resultMuros.preq = diseñoElementos.Cuantia(materiales.fc, materiales.fy, muros.LMux[i], muros.b, muros.d);
                resultMuros.Asreq = diseñoElementos.AceroRequerido(materiales.fc, materiales.fy, muros.LMux[i], muros.b, muros.d);
                resultMuros.As = diseñoElementos.Acero(resultMuros.Asreq, resultMuros.Asminh);
                resultMuros.Asb = refuerzo.AreaBarra(muros.Nbh);
                resultMuros.Cant = diseñoElementos.CantBarras(resultMuros.As, resultMuros.Asb);
                resultMuros.Sep = diseñoElementos.SepBarras(muros.b, resultMuros.As, resultMuros.Asb);

                // Chequeo fisuración
                resultMuros.fs = diseñoElementos.EsfuerzoAcero(materiales.Ec, resultMuros.preq, muros.LMsx[i], resultMuros.As, muros.d);
                resultMuros.db = refuerzo.DiametroBarra(muros.Nbh);
                resultMuros.fsadm = diseñoElementos.EsfuerzoAdmisible(muros.h, resultMuros.db, resultMuros.Sep);

                if (resultMuros.fsadm >= 170 && resultMuros.fsadm <= 250 && resultMuros.fs <= resultMuros.fsadm)
                {
                    resultMuros.ChequeoFisuracion = diseñoElementos.ChequeoFisuracion(resultMuros.fs, resultMuros.fsadm);
                }
                else
                {
                    resultMuros.Msx = muros.LMsx[i];
                    resultMuros.Cant = muros.Cantt_ph;
                    resultMuros.Ascol = diseñoElementos.AceroColocado(resultMuros.Cant, resultMuros.Asb);
                    resultMuros.Sep = diseñoElementos.SepBarras(muros.b, resultMuros.Ascol, resultMuros.Asb);
                    resultMuros.preq = diseñoElementos.N_Cuantia(resultMuros.Ascol, muros.b, muros.d);

                    // Cheqquo fisuración
                    resultMuros.fs = diseñoElementos.EsfuerzoAcero(materiales.Ec, resultMuros.preq, muros.LMsx[i], resultMuros.Ascol, muros.d);
                    resultMuros.db = refuerzo.DiametroBarra(muros.Nbh);
                    resultMuros.fsadm = diseñoElementos.EsfuerzoAdmisible(muros.h, resultMuros.db, resultMuros.Sep);
                    resultMuros.ChequeoFisuracion = diseñoElementos.ChequeoFisuracion(resultMuros.fs, resultMuros.fsadm);
                }

                // Diseño por durabilidad
                resultMuros.Mureq = diseñoElementos.MomtUltimoRequ(resultMuros.Mux, resultMuros.Msx, materiales.fy, resultMuros.fs);
                resultMuros.preq = diseñoElementos.Cuantia(materiales.fc, materiales.fy, resultMuros.Mureq, muros.b, muros.d);
                resultMuros.Asreq = diseñoElementos.AceroRequerido(materiales.fc, materiales.fy, resultMuros.Mureq, muros.b, muros.d);
                resultMuros.Asminh = diseñoElementos.AceroMinimo(muros.pminv, muros.b, muros.h);
                resultMuros.As = diseñoElementos.Acero(resultMuros.Asreq, resultMuros.Asminh);
                resultMuros.Asb = refuerzo.AreaBarra(muros.Nbh);
                resultMuros.Cant = muros.Cantt_ph;
                resultMuros.Sep = diseñoElementos.SepBarras(muros.b, resultMuros.As, resultMuros.Asb);
                resultMuros.Ascol = diseñoElementos.AceroColocado(resultMuros.Cant, resultMuros.Asb);

                // Chequeos
                resultMuros.Ascol = diseñoElementos.AceroColocado(resultMuros.Cant, resultMuros.Asb);
                resultMuros.Mn = diseñoElementos.MomNominal(materiales.fc, materiales.fy, resultMuros.Ascol, muros.b, muros.d);
                resultMuros.ChequeoMomentoNominal = muros.ChequeoMomentoNominal(resultMuros.Mureq, resultMuros.Mn);

                listaresultado.Add(resultMuros);
            }

            // Chequeo Cortante
            for (int i = 0; i < muros.LVu.Length; i++)
            {
                ResultMuros resultMuros = new ResultMuros();
                resultMuros.Vu = muros.LVu[i];
                resultMuros.Vc = diseñoElementos.CortConcreto(materiales.fiv, materiales.fc, muros.b, muros.d);
                resultMuros.ChequeoCortante = muros.ChequeoCortante(resultMuros.Vu, resultMuros.Vc);

                listaresultado.Add(resultMuros);
            }

            return listaresultado;

        }
        // ------------------------------------------------------------------------------------------------------------------------------- //



        //  ===== DISEÑO VIGAS =====  //
        public List<ResultVigas> CaculoVigas(Materiales materiales, Vigas vigas, DiseñoElementos diseñoElementos, Refuerzo refuerzo)
        {
            List<ResultVigas> listaresultado = new List<ResultVigas>();

            // Refuerzo Longuitudinal
            for (int i = 0; i < vigas.LMu.Length; i++)
            {
                ResultVigas resultVigas = new ResultVigas();
                resultVigas.Mu = vigas.LMu[i];
                resultVigas.Asmin = diseñoElementos.AceroMinimo(vigas.pmin, vigas.b, vigas.h);
                resultVigas.preq = diseñoElementos.Cuantia(materiales.fc, materiales.fy, vigas.LMu[i], vigas.b, vigas.d);
                resultVigas.Asreq = diseñoElementos.AceroRequerido(materiales.fc, materiales.fy, vigas.LMu[i], vigas.b, vigas.d);
                resultVigas.As = diseñoElementos.Acero(resultVigas.Asreq, resultVigas.Asmin);
                resultVigas.Asb = refuerzo.AreaBarra(vigas.Nb);
                resultVigas.Cant = diseñoElementos.CantBarras(resultVigas.As, resultVigas.Asb);

                listaresultado.Add(resultVigas);
            }

            // Refuerzo Transversal
            {
                ResultVigas resultVigas = new ResultVigas();
                resultVigas.db = refuerzo.DiametroBarra(vigas.Nb);
                resultVigas.dbe = refuerzo.DiametroBarra(vigas.Nbe);
                resultVigas.Av = refuerzo.AreaBarra(vigas.Nbe);
                resultVigas.Vu = vigas.Vu;

                //chequeo en zona confinada
                resultVigas.S1 = vigas.SepEstribos(vigas.d, resultVigas.db, resultVigas.dbe);
                resultVigas.Vc = diseñoElementos.CortConcreto(materiales.fiv, materiales.fc, vigas.b, vigas.d);
                resultVigas.Vs = diseñoElementos.CortAcero(materiales.fiv, resultVigas.Av, materiales.fy, vigas.d, resultVigas.S1);
                resultVigas.Vn = diseñoElementos.CortNominal(resultVigas.Vc, resultVigas.Vs);
                resultVigas.ChequeoCortante = vigas.ChequeoCortante(resultVigas.Vu, resultVigas.Vn);

                // Chequeo zono no confinada
                resultVigas.S2 = vigas.d / 2;

                listaresultado.Add(resultVigas);
            }

                return listaresultado;
        }
        // ------------------------------------------------------------------------------------------------------------------------------- //



        //  ===== DISEÑO COLUMNAS =====  //
        public List<ResultColumn> CaculoColumn(Materiales materiales, Column column, DiseñoElementos diseñoElementos, Refuerzo refuerzo)
        {
            List<ResultColumn> listaresultado = new List<ResultColumn>();

            {
                // Refuerzo Longuitudinal
                ResultColumn resultColumn = new ResultColumn();
                resultColumn.Asmin = diseñoElementos.AceroMinimo(column.pmin, column.b, column.h);
                resultColumn.Asb = refuerzo.AreaBarra(column.Nb);
                resultColumn.Cant = diseñoElementos.CantBarras(resultColumn.Asmin, resultColumn.Asb);

                // Refuerzo Transversal            
                resultColumn.db = refuerzo.DiametroBarra(column.Nb);
                resultColumn.dbe = refuerzo.DiametroBarra(column.Nbe);
                resultColumn.Av = refuerzo.AreaBarra(column.Nbe);

                //chequeo en zona confinada
                resultColumn.S1 = column.SepEstribos(column.h, column.b, resultColumn.db, resultColumn.dbe);

                // Chequeo zono no confinada
                resultColumn.S2 = 2 * resultColumn.S1;

                // Chequeo carga axial            
                resultColumn.Pu = column.Pu;
                resultColumn.Ag = column.b * column.h;
                resultColumn.Ast = resultColumn.Asb * resultColumn.Cant;
                resultColumn.Astf = resultColumn.Asb * column.CantReal;
                resultColumn.Pn = diseñoElementos.ResistCargaAxial(materiales.fc, resultColumn.Ag, resultColumn.Astf, materiales.fy);
                resultColumn.ChequeoCargaAxial = column.ChequeoCargaAxial(resultColumn.Pu, resultColumn.Pn);

                listaresultado.Add(resultColumn);
            }

            return listaresultado;
        }
        // ------------------------------------------------------------------------------------------------------------------------------- //

    }

}
