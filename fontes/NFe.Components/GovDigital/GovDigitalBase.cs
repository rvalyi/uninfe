﻿using NFe.Components.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web.Services.Protocols;

namespace NFe.Components.GovDigital
{
    public abstract class GovDigitalBase : EmiteNFSeBase
    {
        #region locais/ protegidos
        object govDigitalService;
        private int CodigoMun = 0;
        private string ProxyUser = null;
        private string ProxyPass = null;
        private string ProxyServer = null;


        protected object GovDigitalService
        {
            get
            {
                if (govDigitalService == null)
                {
                    if (tpAmb == TipoAmbiente.taHomologacao)
                    {
                        switch (CodigoMun)
                        {
                            case 3122306: // Divinópolis-MG
                                govDigitalService = new br.com.govdigital.homolog1.NfseServiceImplDivService();
                                break;

                            case 3151800: //Pocos de caldas-MG
                                govDigitalService = new br.com.govdigital.homolog.pocos.h.NfseServiceImplPocosService();
                                break;

                            case 3147006: //Paracatu-MG
                                govDigitalService = new br.com.govdigital.homolog.paracatu.h.NfseServiceImplPctuService();
                                break;

                            case 3138203: //Lavras-MG
                                govDigitalService = new br.com.govdigital.homolog.lavras.h.NfseServiceImplLavrService();
                                break;

                            case 3152808: //Prata-MG
                                govDigitalService = new br.com.govdigital.homolog.prata.h.NfseServiceImplPrataService();
                                break;

                            case 3162955: //São José da Lapa-MG
                                govDigitalService = new br.com.govdigital.homolog.saojosedalapa.h.NfseServiceImplSjlService();
                                break;

                            case 3149309: //Pedro Leopoldo-MG
                                govDigitalService = new br.com.govdigital.homolog.pedroleopoldo.h.NfseServiceImplPlService();
                                break;

                            default:
                                break;
                        }
                    }

                    else
                    {
                        switch (CodigoMun)
                        {
                            case 3122306: // Divinópolis-MG
                                govDigitalService = new br.com.govdigital.ws.NfseServiceImplDivService();
                                break;

                            case 3151800: //Pocos de caldas-MG
                                govDigitalService = new br.com.govdigital.ws.pocos.p.NfseServiceImplPocosService();
                                break;

                            case 3147006: //Paracatu-MG
                                govDigitalService = new br.com.govdigital.ws.paracatu.p.NfseServiceImplPctuService();
                                break;

                            case 3138203: //Lavras-MG
                                govDigitalService = new br.com.govdigital.ws.lavras.p.NfseServiceImplLavrService();
                                break;

                            case 3152808: //Prata-MG
                                govDigitalService = new br.com.govdigital.ws.prata.p.NfseServiceImplPrataService();
                                break;

                            case 3162955: //São José da Lapa-MG
                                govDigitalService = new br.com.govdigital.ws.saojosedalapa.p.NfseServiceImplSjlService();
                                break;

                            case 3149309: //Pedro Leopoldo-MG
                                govDigitalService = new br.com.govdigital.ws.pedroleopoldo.p.NfseServiceImplPlService();
                                break;

                            default:
                                break;
                        }
                    }

                    AddClientCertificates();
                    AddProxyUser();
                }
                return govDigitalService;
            }
        }

        private void AddClientCertificates()
        {
            X509CertificateCollection certificates = null;
            Type t = govDigitalService.GetType();
            PropertyInfo pi = t.GetProperty("ClientCertificates");
            certificates = pi.GetValue(govDigitalService, null) as X509CertificateCollection;
            certificates.Add(Certificate);
        }

        private void AddProxyUser()
        {
            if (!String.IsNullOrEmpty(ProxyUser))
            {
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(ProxyUser, ProxyPass, ProxyServer);
                System.Net.WebRequest.DefaultWebProxy.Credentials = credentials;

                WebServiceProxy wsp = new WebServiceProxy(Certificate as X509Certificate2);

                wsp.SetProp(govDigitalService, "Proxy", Proxy.DefinirProxy(ProxyServer, ProxyUser, ProxyPass, 8080));
            }
        }
        #endregion

        #region propriedades
        public X509Certificate Certificate { get; private set; }
        private string NfseCabecMsg = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><ns2:cabecalho xmlns=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:ns2=\"http://www.abrasf.org.br/nfse.xsd\" versao=\"2.00\"><ns2:versaoDados>2.00</ns2:versaoDados></ns2:cabecalho>";
        #endregion

        #region Construtores
        public GovDigitalBase(TipoAmbiente tpAmb, string pastaRetorno, X509Certificate certificate, int codMun, string proxyUser, string proxyPass, string proxyServer)
            : base(tpAmb, pastaRetorno)
        {
            Certificate = certificate;
            CodigoMun = codMun;
            ProxyUser = proxyUser;
            ProxyPass = proxyPass;
            ProxyServer = proxyServer;

        }
        #endregion

        #region Métodos
        public override void EmiteNF(string file)
        {
            string strResult = Invoke("GerarNfse", new[] { NfseCabecMsg, ReaderXML(file) });
            GerarRetorno(file, strResult, Propriedade.ExtEnvio.EnvLoteRps, Propriedade.ExtRetorno.RetLoteRps);
        }

        public override void CancelarNfse(string file)
        {
            string strResult = Invoke("CancelarNfse", new[] { NfseCabecMsg, ReaderXML(file) });
            GerarRetorno(file, strResult, Propriedade.ExtEnvio.PedCanNfse, Propriedade.ExtRetorno.CanNfse);
        }

        public override void ConsultarLoteRps(string file)
        {
            string strResult = Invoke("ConsultarLoteRps", new[] { NfseCabecMsg, ReaderXML(file) });
            GerarRetorno(file, strResult, Propriedade.ExtEnvio.PedLoteRps, Propriedade.ExtRetorno.LoteRps);
        }

        public override void ConsultarSituacaoLoteRps(string file)
        {
            throw new Exceptions.ServicoInexistenteException();
        }

        public override void ConsultarNfse(string file)
        {
            string strResult = Invoke("ConsultarNfseServicoPrestado", new[] { NfseCabecMsg, ReaderXML(file) });
            GerarRetorno(file, strResult, Propriedade.ExtEnvio.PedSitNfse, Propriedade.ExtRetorno.SitNfse);
        }

        public override void ConsultarNfsePorRps(string file)
        {
            string strResult = Invoke("ConsultarNfsePorRps", new[] { NfseCabecMsg, ReaderXML(file) });
            GerarRetorno(file, strResult, Propriedade.ExtEnvio.PedSitNfseRps, Propriedade.ExtRetorno.SitNfseRps);
        }
        #endregion

        #region invoke
        string Invoke(string methodName, params object[] _params)
        {
            object result = "";
            ServicePointManager.Expect100Continue = false;
            Type t = GovDigitalService.GetType();
            MethodInfo mi = t.GetMethod(methodName);
            result = mi.Invoke(GovDigitalService, _params);
            return result.ToString();
        }
        #endregion

        #region ReaderXML
        private string ReaderXML(string file)
        {
            string result = "";

            using (StreamReader reader = new StreamReader(file))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    result += line;
                }
            }
            return result;
        }
        #endregion
    }
    
}
