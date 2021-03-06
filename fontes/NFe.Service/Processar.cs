﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Windows.Forms;
using System.ComponentModel;

using NFe.Components;
using NFe.Settings;
using NFe.Exceptions;
using NFe.ConvertTxt;
using NFe.Validate;
using NFe.Certificado;
using NFe.Components.Info;

namespace NFe.Service
{
    public class Processar
    {
        #region Métodos gerais

        #region ProcessaArquivo()
        public void ProcessaArquivo(int emp, string arquivo)
        {
            try
            {
                Servicos servico = Servicos.Nulo;
                try
                {
                    ValidarExtensao(arquivo);

                    servico = DefinirTipoServico(emp, arquivo);

                    if (servico == Servicos.Nulo)
                        throw new Exception("Não pode identificar o tipo de serviço baseado no arquivo " + arquivo);

                    switch (servico)
                    {
                        #region NFS-e
                        case Servicos.NFSeCancelar:
                            this.DirecionarArquivo(emp, true, true, arquivo, new NFSe.TaskNFSeCancelar());
                            break;

                        case Servicos.NFSeConsultar:
                            this.DirecionarArquivo(emp, true, true, arquivo, new NFSe.TaskNFSeConsultar());
                            break;

                        case Servicos.NFSeConsultarLoteRps:
                            this.DirecionarArquivo(emp, true, true, arquivo, new NFSe.TaskNFSeConsultarLoteRps());
                            break;

                        case Servicos.NFSeConsultarPorRps:
                            this.DirecionarArquivo(emp, true, true, arquivo, new NFSe.TaskNFSeConsultarPorRps());
                            break;

                        case Servicos.NFSeConsultarSituacaoLoteRps:
                            this.DirecionarArquivo(emp, true, true, arquivo, new NFSe.TaskNFSeConsultaSituacaoLoteRps());
                            break;

                        case Servicos.NFSeConsultarURL:
                            this.DirecionarArquivo(emp, true, true, arquivo, new NFSe.TaskNFSeConsultarURL());
                            break;

                        case Servicos.NFSeConsultarURLSerie:
                            this.DirecionarArquivo(emp, true, true, arquivo, new NFSe.TaskNFSeConsultarURLSerie());
                            break;

                        case Servicos.NFSeRecepcionarLoteRps:
                            this.DirecionarArquivo(emp, true, true, arquivo, new NFSe.TaskNFSeRecepcionarLoteRps());
                            break;

                        case Servicos.NFSeConsultarNFSePNG:
                            this.DirecionarArquivo(emp, true, true, arquivo, new NFSe.TaskConsultarNfsePNG());
                            break;

                        case Servicos.NFSeInutilizarNFSe:
                            this.DirecionarArquivo(emp, true, true, arquivo, new NFSe.TaskInutilizarNfse());
                            break;

                        case Servicos.NFSeConsultarNFSePDF:
                            this.DirecionarArquivo(emp, true, true, arquivo, new NFSe.TaskConsultarNfsePDF());
                            break;

                        #endregion

                        #region NFe
                        case Servicos.NFeAssinarValidarEnvioEmLote:
                            CertVencido(emp);
                            AssinarValidarNFe(arquivo, Empresas.Configuracoes[emp].PastaXmlEmLote);
                            break;

                        case Servicos.ConsultaCadastroContribuinte:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskCadastroContribuinte());
                            break;

                        case Servicos.EventoRecepcao:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskNFeEventos());
                            break;

                        case Servicos.NFeConsultaNFDest:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskNFeConsultaNFDest());
                            break;

                        case Servicos.NFeConsultaStatusServico:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskNFeConsultaStatus());
                            break;

                        case Servicos.NFeConverterTXTparaXML:
                            ConverterTXTparaXML(arquivo);
                            break;

                        case Servicos.NFeDownload:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskNFeDownload());
                            break;

                        case Servicos.NFeEnviarLote:
                            DirecionarArquivo(emp, false, false, arquivo, new TaskNFeRecepcao());
                            break;

                        case Servicos.NFeGerarChave:
                            GerarChaveNFe(arquivo);
                            break;

                        case Servicos.NFeInutilizarNumeros:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskNFeInutilizacao());
                            break;

                        case Servicos.NFePedidoSituacaoLote:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskNFeRetRecepcao());
                            break;

                        case Servicos.NFeMontarLoteUma:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskNFeMontarLoteUmaNFe());
                            break;

                        case Servicos.NFeMontarLoteVarias:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskNFeMontarLoteVarias());
                            break;

                        case Servicos.NFePedidoConsultaSituacao:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskNFeConsultaSituacao());
                            break;

#if nao
                        case Servicos.RegistroDeSaida:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskRegistroDeSaida());
                            break;

                        case Servicos.RegistroDeSaidaCancelamento:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskRegistroDeSaidaCancelamento());
                            break;
#endif
                        #endregion

                        #region MDFe
                        case Servicos.MDFeAssinarValidarEnvioEmLote:
                            CertVencido(emp);
                            AssinarValidarMDFe(arquivo, Empresas.Configuracoes[emp].PastaXmlEmLote);
                            break;

                        case Servicos.MDFeConsultaNaoEncerrado:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskMDFeConsNaoEncerrado());
                            break;

                        case Servicos.MDFeConsultaStatusServico:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskMDFeConsultaStatus());
                            break;

                        case Servicos.MDFeEnviarLote:
                            DirecionarArquivo(emp, false, false, arquivo, new TaskMDFeRecepcao());
                            break;

                        case Servicos.MDFeMontarLoteUm:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskMDFeMontarLoteUm());
                            break;

                        case Servicos.MDFeMontarLoteVarios:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskMDFeMontarLoteVarias());
                            break;

                        case Servicos.MDFePedidoSituacaoLote:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskMDFeRetRecepcao());
                            break;

                        case Servicos.MDFePedidoConsultaSituacao:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskMDFeConsultaSituacao());
                            break;

                        case Servicos.MDFeRecepcaoEvento:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskMDFeEventos());
                            break;
                        #endregion

                        #region CTe
                        case Servicos.CTeAssinarValidarEnvioEmLote:
                            CertVencido(emp);
                            AssinarValidarCTe(arquivo, Empresas.Configuracoes[emp].PastaXmlEmLote);
                            break;

                        case Servicos.CTeConsultaStatusServico:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskCTeConsultaStatus());
                            break;

                        case Servicos.CTeEnviarLote:
                            DirecionarArquivo(emp, false, false, arquivo, new TaskCTeRecepcao());
                            break;

                        case Servicos.CTeInutilizarNumeros:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskCTeInutilizacao());
                            break;

                        case Servicos.CTeMontarLoteUm:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskCTeMontarLoteUm());
                            break;

                        case Servicos.CTeMontarLoteVarios:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskCTeMontarLoteVarias());
                            break;

                        case Servicos.CTePedidoConsultaSituacao:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskCTeConsultaSituacao());
                            break;

                        case Servicos.CTePedidoSituacaoLote:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskCTeRetRecepcao());
                            break;

                        case Servicos.CTeRecepcaoEvento:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskCTeEventos());
                            break;
                        #endregion

                        #region DFe
                        case Servicos.DFeEnviar:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskDFeRecepcao());
                            break;
                        #endregion

                        #region LMC
                        case Servicos.LMCAutorizacao:
                            DirecionarArquivo(emp, true, true, arquivo, new TaskLMCAutorizacao());
                            break;
                        #endregion
                    }

                    #region Serviços em comum
                    switch (servico)
                    {
                        case Servicos.AssinarValidar:
                            //Não vou verificar a validade do certificado neste ponto, pois as vezes quero somente validar o XML mesmo com um certificado vencido, e não consigo. Como é somente validação, isso vai facilitar para o desenvolvedor. Wandrey 21/11/2014
                            //CertVencido(emp);
                            AssinarValidar(arquivo);
                            break;

                        case Servicos.UniNFeAlterarConfiguracoes:
                            AlterarConfiguracoesUniNFe(arquivo);
                            break;

                        case Servicos.UniNFeConsultaGeral:
                            ConsultarGeral(arquivo);
                            break;

                        case Servicos.UniNFeConsultaInformacoes:
                            ConsultaInformacoesUniNFe(arquivo);
                            break;

                        case Servicos.WSExiste:
                            DirecionarArquivo(emp, false, false, arquivo, new TaskWSExiste());
                            break;

                        case Servicos.DANFEImpressao:
                            DirecionarArquivo(emp, false, false, arquivo, new TaskDanfe());
                            break;

                        case Servicos.DANFEImpressao_Contingencia:
                            DirecionarArquivo(emp, false, false, arquivo, new TaskDanfeContingencia());
                            break;

                        case Servicos.DANFERelatorio:
                            DirecionarArquivo(emp, false, false, arquivo, new TaskDanfeReport());
                            break;
                    }
                    #endregion
                }
                catch (ExceptionSemInternet ex)
                {
                    GravaErroERP(arquivo, servico, ex, ex.ErrorCode);
                }
                catch (ExceptionCertificadoDigital ex)
                {
                    GravaErroERP(arquivo, servico, ex, ex.ErrorCode);
                }
                catch (Exception ex)
                {
                    if (servico == Servicos.Nulo || servico == Servicos.NFeConsultaStatusServico)
                    {
                        /// 7/2012 <<< danasa
                        ///o erp nao precisa esperar pelo tempo excedido, então retornamos um arquivo .err
                        ///
                        GravaErroERP(arquivo, servico, ex, ErroPadrao.ErroNaoDetectado);
                    }
                }
            }
            catch { }
        }

        private void ValidarExtensao(string arquivo)
        {
            var extOk = false;
            string extensoes = "";
            foreach (var pS in typeof(Propriedade.ExtEnvio).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (extensoes != "") extensoes += ", ";
                extensoes += pS.GetValue(null).ToString();
                if (arquivo.EndsWith(pS.GetValue(null).ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    extOk = true;
                    break;
                }
            }
            if (!extOk)
            {
                throw new Exception("Não pode identificar o tipo de arquivo baseado no arquivo '" + arquivo + "'\r\nExtensões permitidas: " + extensoes);
            }
        }
        #endregion

        private Servicos DefinirTipoServico(int empresa, string fullPath)
        {
            Servicos tipoServico = Servicos.Nulo;

            string arq = fullPath.ToLower().Trim();

            #region Serviços que funcionam tanto na pasta Geral como na pasta da Empresa
            if (arq.IndexOf(Propriedade.ExtEnvio.ConsCertificado) >= 0)
            {
                tipoServico = Servicos.UniNFeConsultaGeral;
            }
            else if (arq.IndexOf(Propriedade.ExtEnvio.AltCon_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.AltCon_TXT) >= 0)
            {
                tipoServico = Servicos.UniNFeAlterarConfiguracoes;
            }
            else if (arq.IndexOf(Propriedade.ExtEnvio.EnvWSExiste_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.EnvWSExiste_TXT) >= 0)
            {
                tipoServico = Servicos.WSExiste;
            }
            else if (arq.IndexOf(Propriedade.ExtEnvio.EnvImpressaoDanfe_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.EnvImpressaoDanfe_TXT) >= 0)
            {
                tipoServico = Servicos.DANFEImpressao;
            }
            else if (arq.IndexOf(Propriedade.ExtEnvio.EnvDanfeReport_XML) >= 0 || arq.IndexOf(Propriedade.ExtEnvio.EnvDanfeReport_TXT) >= 0)
            {
                tipoServico = Servicos.DANFERelatorio;
            }
            #endregion
            else
            {
                if (arq.IndexOf(Empresas.Configuracoes[empresa].PastaContingencia.ToLower()) >= 0)
                {
                    tipoServico = Servicos.DANFEImpressao_Contingencia;
                }
                else
                    if (arq.IndexOf(Empresas.Configuracoes[empresa].PastaValidar.ToLower()) >= 0)
                    {
                        tipoServico = Servicos.AssinarValidar;
                    }
                    else
                    {
                        FileInfo infArq = new FileInfo(arq);
                        string pastaArq = ConfiguracaoApp.RemoveEndSlash(infArq.DirectoryName).ToLower().Trim();
                        string pastaLote = ConfiguracaoApp.RemoveEndSlash(Empresas.Configuracoes[empresa].PastaXmlEmLote).ToLower().Trim();
                        string pastaEnvio = ConfiguracaoApp.RemoveEndSlash(Empresas.Configuracoes[empresa].PastaXmlEnvio).ToLower().Trim();
                        if (pastaArq.EndsWith("\\temp"))
                            pastaArq = Path.GetDirectoryName(pastaArq);

                        #region Arquivos com extensão txt (Somente NFe tem TXT)
                        if (fullPath.ToLower().EndsWith(".txt"))
                        {
                            if (arq.IndexOf(Propriedade.ExtEnvio.PedSit_TXT) >= 0)
                            {
                                tipoServico = Servicos.NFePedidoConsultaSituacao;
                            }
                            else if (arq.IndexOf(Propriedade.ExtEnvio.PedSta_TXT) >= 0)
                            {
                                tipoServico = Servicos.NFeConsultaStatusServico;
                            }
                            else if (arq.IndexOf(Propriedade.ExtEnvio.ConsCad_TXT) >= 0)
                            {
                                tipoServico = Servicos.ConsultaCadastroContribuinte;
                            }
                            else if (arq.IndexOf(Propriedade.ExtEnvio.PedInu_TXT) >= 0)
                            {
                                tipoServico = Servicos.NFeInutilizarNumeros;
                            }
                            else if (arq.IndexOf(Propriedade.ExtEnvio.Nfe_TXT) >= 0)
                            {
                                tipoServico = Servicos.NFeConverterTXTparaXML;
                            }
                            else if (arq.IndexOf(Propriedade.ExtEnvio.GerarChaveNFe_TXT) >= 0)
                            {
                                tipoServico = Servicos.NFeGerarChave;
                            }
                            else if (arq.IndexOf(Propriedade.ExtEnvio.EnvWSExiste_TXT) >= 0)
                            {
                                tipoServico = Servicos.WSExiste;
                            }
                            else if (arq.IndexOf(Propriedade.ExtEnvio.ConsInf_TXT) >= 0)
                            {
                                tipoServico = Servicos.UniNFeConsultaInformacoes;
                            }
                            else if (arq.IndexOf(Propriedade.ExtEnvio.EnvCCe_TXT) >= 0 ||
                                    arq.IndexOf(Propriedade.ExtEnvio.PedEve_TXT) >= 0 ||
                                    arq.IndexOf(Propriedade.ExtEnvio.EnvCancelamento_TXT) >= 0 ||
                                    arq.IndexOf(Propriedade.ExtEnvio.EnvManifestacao_TXT) >= 0)
                            {
                                tipoServico = Servicos.EventoRecepcao;
                            }
                            else if (arq.IndexOf(Propriedade.ExtEnvio.ConsNFeDest_TXT) >= 0)
                            {
                                tipoServico = Servicos.NFeConsultaNFDest;
                            }
                            else if (arq.IndexOf(Propriedade.ExtEnvio.EnvDownload_TXT) >= 0)
                            {
                                tipoServico = Servicos.NFeDownload;
                            }
                            else if (arq.IndexOf(Propriedade.ExtEnvio.EnvDFe_TXT) >= 0)
                            {
                                tipoServico = Servicos.DFeEnviar;
                            }
#if nao
                        else if (arq.IndexOf(Propriedade.ExtEnvio.EnvRegistroDeSaida_TXT) >= 0)
                        {
                            tipoServico = Servicos.RegistroDeSaida;
                        }
                        else if (arq.IndexOf(Propriedade.ExtEnvio.EnvCancRegistroDeSaida_TXT) >= 0)
                        {
                            tipoServico = Servicos.RegistroDeSaidaCancelamento;
                        }
#endif
                            else if (arq.IndexOf(Propriedade.ExtEnvio.MontarLote_TXT) >= 0)
                            {
                                if (arq.IndexOf(Empresas.Configuracoes[empresa].PastaXmlEmLote.ToLower().Trim()) >= 0)
                                {
                                    tipoServico = Servicos.NFeMontarLoteVarias;
                                }
                            }
                        }
                        #endregion
                        else
                        #region Arquivos com extensão XML
                        {
                            XmlDocument doc = new XmlDocument();
                            doc.Load(fullPath);

                            switch (doc.DocumentElement.Name)
                            {
                                #region DFe
                                case "distDFeInt":
                                    tipoServico = Servicos.DFeEnviar;
                                    break;
                                #endregion

                                #region MDFe
                                case "consMDFeNaoEnc":
                                    tipoServico = Servicos.MDFeConsultaNaoEncerrado;
                                    break;

                                case "consStatServMDFe":
                                    tipoServico = Servicos.MDFeConsultaStatusServico;
                                    break;

                                case "MDFe":
                                    if (pastaArq == pastaLote)
                                        tipoServico = Servicos.MDFeAssinarValidarEnvioEmLote;
                                    else if (pastaArq == pastaEnvio)
                                        tipoServico = Servicos.MDFeMontarLoteUm;
                                    break;

                                case "enviMDFe":
                                    tipoServico = Servicos.MDFeEnviarLote;
                                    break;

                                case "consReciMDFe":
                                    tipoServico = Servicos.MDFePedidoSituacaoLote;
                                    break;

                                case "consSitMDFe":
                                    tipoServico = Servicos.MDFePedidoConsultaSituacao;
                                    break;

                                case "MontarLoteMDFe":
                                    if (arq.IndexOf(Empresas.Configuracoes[empresa].PastaXmlEmLote.ToLower().Trim()) >= 0)
                                    {
                                        tipoServico = Servicos.MDFeMontarLoteVarios;
                                    }
                                    break;

                                case "eventoMDFe":
                                    tipoServico = Servicos.MDFeRecepcaoEvento;
                                    break;
                                #endregion

                                #region CTe
                                case "consStatServCte":
                                    tipoServico = Servicos.CTeConsultaStatusServico;
                                    break;

                                case "CTe":
                                    if (pastaArq == pastaLote)
                                        tipoServico = Servicos.CTeAssinarValidarEnvioEmLote;
                                    else if (pastaArq == pastaEnvio)
                                        tipoServico = Servicos.CTeMontarLoteUm;
                                    break;

                                case "enviCTe":
                                    tipoServico = Servicos.CTeEnviarLote;
                                    break;

                                case "consReciCTe":
                                    tipoServico = Servicos.CTePedidoSituacaoLote;
                                    break;

                                case "consSitCTe":
                                    tipoServico = Servicos.CTePedidoConsultaSituacao;
                                    break;

                                case "inutCTe":
                                    tipoServico = Servicos.CTeInutilizarNumeros;
                                    break;

                                case "eventoCTe":
                                    tipoServico = Servicos.CTeRecepcaoEvento;
                                    break;

                                case "MontarLoteCTe":
                                    if (arq.IndexOf(Empresas.Configuracoes[empresa].PastaXmlEmLote.ToLower().Trim()) >= 0)
                                    {
                                        tipoServico = Servicos.CTeMontarLoteVarios;
                                    }
                                    break;
                                #endregion

                                #region NFe
                                case "consStatServ":
                                    tipoServico = Servicos.NFeConsultaStatusServico;
                                    break;

                                case "NFe":
                                    if (pastaArq == pastaLote)
                                        tipoServico = Servicos.NFeAssinarValidarEnvioEmLote;
                                    else if (pastaArq == pastaEnvio)
                                        tipoServico = Servicos.NFeMontarLoteUma;
                                    break;

                                case "enviNFe":
                                    tipoServico = Servicos.NFeEnviarLote;
                                    break;

                                case "consReciNFe":
                                    tipoServico = Servicos.NFePedidoSituacaoLote;
                                    break;

                                case "consSitNFe":
                                    tipoServico = Servicos.NFePedidoConsultaSituacao;
                                    break;

                                case "inutNFe":
                                    tipoServico = Servicos.NFeInutilizarNumeros;
                                    break;

                                case "envEvento":
                                    tipoServico = Servicos.EventoRecepcao;
                                    break;

                                case "ConsCad":
                                    tipoServico = Servicos.ConsultaCadastroContribuinte;
                                    break;

                                case "MontarLoteNFe":
                                    if (arq.IndexOf(Empresas.Configuracoes[empresa].PastaXmlEmLote.ToLower().Trim()) >= 0)
                                    {
                                        tipoServico = Servicos.NFeMontarLoteVarias;
                                    }
                                    break;

                                case "gerarChave":
                                    tipoServico = Servicos.NFeGerarChave;
                                    break;

                                case "consNFeDest":
                                    tipoServico = Servicos.NFeConsultaNFDest;
                                    break;

                                case "downloadNFe":
                                    tipoServico = Servicos.NFeDownload;
                                    break;
                                #endregion

                                #region LMC
                                case "autorizacao":
                                    tipoServico = Servicos.LMCAutorizacao;
                                    break;
                                #endregion

                                #region Geral
                                case "ConsInf":
                                    tipoServico = Servicos.UniNFeConsultaInformacoes;
                                    break;
                                #endregion

                                default:
                                    #region NFS-e
                                    if (arq.IndexOf(Propriedade.ExtEnvio.PedLoteRps) >= 0)
                                    {
                                        tipoServico = Servicos.NFSeConsultarLoteRps;
                                    }
                                    else if (arq.IndexOf(Propriedade.ExtEnvio.PedCanNfse) >= 0)
                                    {
                                        tipoServico = Servicos.NFSeCancelar;
                                    }
                                    else if (arq.IndexOf(Propriedade.ExtEnvio.PedSitLoteRps) >= 0)
                                    {
                                        tipoServico = Servicos.NFSeConsultarSituacaoLoteRps;
                                    }
                                    else if (arq.IndexOf(Propriedade.ExtEnvio.EnvLoteRps) >= 0)
                                    {
                                        tipoServico = Servicos.NFSeRecepcionarLoteRps;
                                    }
                                    else if (arq.IndexOf(Propriedade.ExtEnvio.PedSitNfse) >= 0)
                                    {
                                        tipoServico = Servicos.NFSeConsultar;
                                    }
                                    else if (arq.IndexOf(Propriedade.ExtEnvio.PedSitNfseRps) >= 0)
                                    {
                                        tipoServico = Servicos.NFSeConsultarPorRps;
                                    }
                                    else if (arq.IndexOf(Propriedade.ExtEnvio.PedURLNfse) >= 0)
                                    {
                                        tipoServico = Servicos.NFSeConsultarURL;
                                    }
                                    else if (arq.IndexOf(Propriedade.ExtEnvio.PedURLNfseSerie) >= 0)
                                    {
                                        tipoServico = Servicos.NFSeConsultarURLSerie;
                                    }
                                    else if (arq.IndexOf(Propriedade.ExtEnvio.PedNFSePNG) >= 0)
                                    {
                                        tipoServico = Servicos.NFSeConsultarNFSePNG;
                                    }
                                    else if (arq.IndexOf(Propriedade.ExtEnvio.PedInuNfse) >= 0)
                                    {
                                        tipoServico = Servicos.NFSeInutilizarNFSe;
                                    }
                                    else if (arq.IndexOf(Propriedade.ExtEnvio.PedNFSePDF) >= 0)
                                    {
                                        tipoServico = Servicos.NFSeConsultarNFSePDF;
                                    }
                                    #endregion

                                    break;
                            }
                        }
                        #endregion
                    }
            }

            return tipoServico;
        }

        #region AssinarValidarNFe()
        /// <summary>
        /// Assinar e Validar todos os arquivos XML de notas fiscais encontrados na pasta informada por parâmetro
        /// </summary>
        /// <param name="nfe">Objeto da classe ServicoNFe</param>
        /// <param name="arquivo">Arquivo a ser validado e assinado</param>
        protected void AssinarValidarNFe(string arquivo, string pasta)
        {
            TaskNFeAssinarValidar nfe = new TaskNFeAssinarValidar();
            nfe.NomeArquivoXML = arquivo;
            nfe.AssinarValidarXMLNFe(pasta);
        }
        #endregion

        #region AssinarValidarCTe()
        /// <summary>
        /// Assinar e Validar todos os arquivos XML de notas fiscais encontrados na pasta informada por parâmetro
        /// </summary>
        /// <param name="nfe">Objeto da classe ServicoNFe</param>
        /// <param name="arquivo">Arquivo a ser validado e assinado</param>
        protected void AssinarValidarCTe(string arquivo, string pasta)
        {
            TaskCTeAssinarValidar nfe = new TaskCTeAssinarValidar();
            nfe.NomeArquivoXML = arquivo;
            nfe.AssinarValidarXMLNFe(pasta);
        }
        #endregion

        #region AssinarValidarMDFe()
        /// <summary>
        /// Assinar e Validar todos os arquivos XML de notas fiscais encontrados na pasta informada por parâmetro
        /// </summary>
        /// <param name="nfe">Objeto da classe ServicoNFe</param>
        /// <param name="arquivo">Arquivo a ser validado e assinado</param>
        protected void AssinarValidarMDFe(string arquivo, string pasta)
        {
            TaskMDFeAssinarValidar nfe = new TaskMDFeAssinarValidar();
            nfe.NomeArquivoXML = arquivo;
            nfe.AssinarValidarXMLNFe(pasta);
        }
        #endregion

        #region AlterarConfiguracoesUniNFe()
        /// <summary>
        /// Executa as tarefas pertinentes a consulta das informações do UniNFe
        /// </summary>
        /// <param name="arquivo">Arquivo a ser tratado/param>
        protected void AlterarConfiguracoesUniNFe(string arquivo)
        {
            try
            {
                ReconfigurarUniNFe(arquivo);
            }
            catch
            {
            }
        }
        #endregion

        #region AssinarValidar()
        /// <summary>
        /// Executa as tarefas pertinentes ao processo de somente assinar e validar os arquivos
        /// </summary>
        /// <param name="arquivo">Arquivo a ser assinado e validado</param>
        protected void AssinarValidar(string arquivo)
        {
            try
            {
                int emp = Empresas.FindEmpresaByThread();

                Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaValidado, Path.GetFileName(Path.ChangeExtension(arquivo, ".xml"))));
                Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlErro, Path.GetFileName(Path.ChangeExtension(arquivo, ".xml"))));
                Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlErro, Path.GetFileName(arquivo)));

                if (arquivo.EndsWith(".txt", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (arquivo.EndsWith(Propriedade.ExtEnvio.EnvDFe_TXT, StringComparison.InvariantCultureIgnoreCase))
                    {
                        #region DFe
                        Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, Functions.ExtrairNomeArq(arquivo, Propriedade.ExtEnvio.EnvDFe_TXT) + Propriedade.ExtRetorno.retEnvDFe_ERR));
                        Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, Functions.ExtrairNomeArq(arquivo, Propriedade.ExtEnvio.EnvDFe_TXT) + Propriedade.ExtRetorno.retEnvDFe_XML));
                        DirecionarArquivo(emp, false, false, arquivo, new TaskDFeRecepcao());
                        #endregion
                    }

                    if (arquivo.EndsWith(Propriedade.ExtEnvio.ConsCad_TXT, StringComparison.InvariantCultureIgnoreCase))
                    {
                        #region Consulta ao cadastro de contribuinte
                        Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, Functions.ExtrairNomeArq(arquivo, Propriedade.ExtEnvio.ConsCad_TXT) + Propriedade.ExtRetorno.ConsCad_ERR));
                        Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, Functions.ExtrairNomeArq(arquivo, Propriedade.ExtEnvio.ConsCad_TXT) + Propriedade.ExtRetorno.ConsCad_XML));
                        DirecionarArquivo(emp, false, false, arquivo, new TaskCadastroContribuinte());
                        #endregion
                    }

                    if (arquivo.EndsWith(Propriedade.ExtEnvio.Nfe_TXT, StringComparison.InvariantCultureIgnoreCase))
                    {
                        new ConverterTXT(arquivo);
                    }

                    if (arquivo.EndsWith(Propriedade.ExtEnvio.EnvCCe_TXT, StringComparison.InvariantCultureIgnoreCase) ||
                        arquivo.EndsWith(Propriedade.ExtEnvio.EnvCancelamento_TXT, StringComparison.InvariantCultureIgnoreCase) ||
                        arquivo.EndsWith(Propriedade.ExtEnvio.EnvManifestacao_TXT, StringComparison.InvariantCultureIgnoreCase) ||
                        arquivo.EndsWith(Propriedade.ExtEnvio.PedEve_TXT, StringComparison.InvariantCultureIgnoreCase))
                    {
                        #region Eventos
                        Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, Functions.ExtrairNomeArq(arquivo, Propriedade.ExtEnvio.EnvCCe_TXT) + Propriedade.ExtRetorno.retEnvCCe_ERR));
                        Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, Functions.ExtrairNomeArq(arquivo, Propriedade.ExtEnvio.EnvCancelamento_TXT) + Propriedade.ExtRetorno.retCancelamento_ERR));
                        Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, Functions.ExtrairNomeArq(arquivo, Propriedade.ExtEnvio.EnvManifestacao_TXT) + Propriedade.ExtRetorno.retManifestacao_ERR));
                        Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, Functions.ExtrairNomeArq(arquivo, Propriedade.ExtEnvio.PedEve_TXT) + Propriedade.ExtRetorno.Eve_ERR));
                        DirecionarArquivo(emp, false, false, arquivo, new TaskNFeEventos());
                        #endregion
                    }

                    if (arquivo.EndsWith(Propriedade.ExtEnvio.PedInu_TXT, StringComparison.InvariantCultureIgnoreCase))
                    {
                        #region Inutilizacao
                        Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, Functions.ExtrairNomeArq(arquivo, Propriedade.ExtEnvio.PedInu_TXT) + Propriedade.ExtRetorno.Inu_ERR));
                        Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, Functions.ExtrairNomeArq(arquivo, Propriedade.ExtEnvio.PedInu_TXT) + "-ped-inu-ret.xml"));
                        DirecionarArquivo(emp, false, false, arquivo, new TaskNFeInutilizacao());
                        #endregion
                    }

                    if (arquivo.EndsWith(Propriedade.ExtEnvio.PedSta_TXT, StringComparison.InvariantCultureIgnoreCase))
                    {
                        Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, Functions.ExtrairNomeArq(arquivo, Propriedade.ExtEnvio.PedSta_TXT) + Propriedade.ExtRetorno.Sta_ERR));
                        DirecionarArquivo(emp, false, false, arquivo, new TaskNFeConsultaStatus());
                    }

                    if (arquivo.IndexOf(Propriedade.ExtEnvio.ConsNFeDest_TXT) >= 0)
                    {
                        Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, Functions.ExtrairNomeArq(arquivo, Propriedade.ExtEnvio.ConsNFeDest_TXT) + Propriedade.ExtRetorno.retConsNFeDest_ERR));
                        DirecionarArquivo(emp, false, false, arquivo, new TaskNFeConsultaNFDest());
                    }

                    if (arquivo.IndexOf(Propriedade.ExtEnvio.PedSit_TXT) >= 0)
                    {
                        Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, Functions.ExtrairNomeArq(arquivo, Propriedade.ExtEnvio.PedSit_TXT) + Propriedade.ExtRetorno.Sit_ERR));
                        DirecionarArquivo(emp, false, false, arquivo, new TaskNFeConsultaSituacao());
                    }
                }
                else
                {
                    if (arquivo.EndsWith(Propriedade.ExtEnvio.EnvCCe_XML, StringComparison.InvariantCultureIgnoreCase) ||
                        arquivo.EndsWith(Propriedade.ExtEnvio.EnvCancelamento_XML, StringComparison.InvariantCultureIgnoreCase) ||
                        arquivo.EndsWith(Propriedade.ExtEnvio.EnvManifestacao_XML, StringComparison.InvariantCultureIgnoreCase) ||
                        arquivo.EndsWith(Propriedade.ExtEnvio.PedEve, StringComparison.InvariantCultureIgnoreCase) ||
                        arquivo.EndsWith(Propriedade.ExtEnvio.PedSit_XML, StringComparison.InvariantCultureIgnoreCase) ||
                        arquivo.EndsWith(Propriedade.ExtEnvio.PedSta_XML, StringComparison.InvariantCultureIgnoreCase))
                    {
                        DirecionarArquivo(arquivo);
                    }

                    ValidarXML validar = new ValidarXML(arquivo, Empresas.Configuracoes[emp].UnidadeFederativaCodigo);
                    validar.ValidarAssinarXML(arquivo);
                }
            }
            catch
            {
            }
        }
        #endregion

        #region ConsultaInformacoesUniNFe()
        /// <summary>
        /// Executa as tarefas pertinentes a consulta das informações do UniNFe
        /// </summary>
        /// <param name="arquivo">Arquivo a ser tratado/param>
        protected void ConsultaInformacoesUniNFe(string arquivo)
        {
            try
            {
                GravarXMLDadosCertificado(arquivo);
            }
            catch
            {
            }
        }
        #endregion

        #region ConverterTXTparaXML()
        /// <summary>
        /// Executa as tarefas pertinentes da conversão de NF-e em TXT para XML 
        /// </summary>
        /// <param name="arquivo">Nome do arquivo a ser convertido</param>
        protected void ConverterTXTparaXML(string arquivo)
        {
            try
            {
                new ConverterTXT(arquivo);
            }
            catch
            {
            }
        }
        #endregion

        #region LimpezaTemporario()
        /// <summary>
        /// Executar as tarefas pertinentes a limpeza de arquivos temporários
        /// </summary>
        public void LimpezaTemporario()
        {
            while (true)
            {
                ExecutaLimpeza();

                Thread.Sleep(new TimeSpan(1, 0, 0, 0));
            }
        }
        #endregion

        #region EmProcessamento()
        /// <summary>
        /// Executar as tarefas pertinentes a analise das notas em processamento a mais de 5 minutos
        /// </summary>
        public void EmProcessamento()
        {
            bool hasAll = false;

            while (true)
            {
                for (int i = 0; i < Empresas.Configuracoes.Count; i++)
                {
                    if (Empresas.Configuracoes[i].Servico == TipoAplicativo.Nfse)
                        continue;

                    NFeEmProcessamento nfe = new NFeEmProcessamento();
                    nfe.Analisar(i);

                    hasAll = true;
                }
                if (hasAll)
                    Thread.Sleep(720000); //Dorme por 12 minutos, para atender o problema do consumo indevido da SEFAZ
                else
                    break;
            }
        }
        #endregion

        #region GerarChaveNFe()
        /// <summary>
        /// Executa tarefas pertinentes a geração da Chave da NFe solicitado pelo ERP
        /// </summary>
        /// <param name="arquivo">Arquivo a ser tratado</param>
        protected void GerarChaveNFe(string arquivo)
        {
            Auxiliar oAux = new Auxiliar();

            FileInfo fi = new FileInfo(arquivo);
            // processa arquivos XML
            if (fi.Extension.ToLower() == ".xml")
            {
                new NFeW().GerarChaveNFe(arquivo, true);
            }
            // processa arquivos TXT
            else
            {
                new NFeW().GerarChaveNFe(arquivo, false);
            }
        }
        #endregion

        #region GerarXMLPedRec()
        /// <summary>
        /// Executa as tarefas pertinentes a geração dos pedidos de consulta situação de lote da NFe
        /// </summary>
        /// <param name="nfe">Objeto da classe servico NFe</param>
        public void GerarXMLPedRec(object nfe)
        {
            while (true)
            {
                for (int i = 0; i < Empresas.Configuracoes.Count; i++)
                {
                    Empresa empresa = Empresas.Configuracoes[i];

                    if (!string.IsNullOrEmpty(empresa.PastaEmpresa) && empresa.Servico != TipoAplicativo.Nfse)
                        GerarXMLPedRec(i, nfe);
                }

                Thread.Sleep(2000);
            }
        }
        #endregion

        #endregion

        #region DirecionarArquivo()
        /// <summary>
        /// Direcionar os arquivos encontrados na pasta de envio corretamente
        /// </summary>
        /// <param name="arquivos">Lista de arquivos</param>
        /// <param name="metodo">Método a ser executado do serviço da NFe</param>
        /// <param name="nfe">Objeto do serviço da NFe a ser executado</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 18/04/2011
        /// </remarks>
        /*private void DirecionarArquivo(List<string> arquivos, object nfe, string metodo)
        {
            for (int i = 0; i < arquivos.Count; i++)
            {
                DirecionarArquivo(arquivos[i], nfe, metodo);
            }
        }*/
        #endregion

        #region DirecionarArquivo()
        /// <summary>
        /// Direcionar o arquivo
        /// </summary>
        /// <param name="arquivos">Arquivo</param>
        /// <param name="metodo">Método a ser executado do serviço da NFe</param>
        /// <param name="nfe">Objeto do serviço da NFe a ser executado</param>
        /// <remarks>
        /// Autor: Wandrey Mundin Ferreira
        /// Data: 18/04/2011
        /// </remarks>
        private void DirecionarArquivo(int emp, bool veCertificado, bool veConexao, string arquivo, object taskClass)
        {
            try
            {
                if (veCertificado)
                    CertVencido(emp);
                if (veConexao)
                    IsConnectedToInternet();

                //Processa ou envia o XML
                EnviarArquivo(arquivo, taskClass, "Execute");
            }
            catch
            {
                //Não pode ser tratado nenhum erro aqui, visto que já estão sendo tratados e devidamente retornados
                //para o ERP no ponto da execução dos serviços. Foi muito bem testado e analisado. Wandrey 09/03/2010
                if (taskClass is TaskNFeConsultaStatus)
                    throw;
            }
        }

        private void DirecionarArquivo(string arquivo)
        {
            //Processa ou envia o XML
            var valclass = new TaskValidar();
            //Definir o tipo do serviço
            Type tipoServico = valclass.GetType();
            //Definir o arquivo XML para a classe UniNfeClass
            tipoServico.InvokeMember("NomeArquivoXML", System.Reflection.BindingFlags.SetProperty, null, valclass, new object[] { arquivo });
            tipoServico.InvokeMember("Execute", System.Reflection.BindingFlags.InvokeMethod, null, valclass, null);
        }
        #endregion

        #region EnviarArquivo()
        /// <summary>
        /// Analisa o tipo do XML que está na pasta de envio e executa a operação necessária. Exemplo: Envia ao SEFAZ, reconfigura o UniNFE, etc... 
        /// </summary>
        /// <param name="cArquivo">Nome do arquivo XML a ser enviado ou analisado</param>
        /// <param name="oNfe">Objeto da classe UniNfeClass a ser utilizado nas operações</param>
        private void EnviarArquivo(string arquivo, Object nfe, string metodo)
        {
            int emp = Empresas.FindEmpresaByThread();

            //Definir o tipo do serviço
            Type tipoServico = nfe.GetType();

            //Definir o arquivo XML para a classe UniNfeClass
            tipoServico.InvokeMember("NomeArquivoXML", System.Reflection.BindingFlags.SetProperty, null, nfe, new object[] { arquivo });

            bool doExecute = arquivo.IndexOf(Empresas.Configuracoes[emp].PastaValidar, StringComparison.InvariantCultureIgnoreCase) >= 0;
            if (!doExecute)
            {
                if (Empresas.Configuracoes[emp].tpEmis != (int)NFe.Components.TipoEmissao.teFS &&
                    Empresas.Configuracoes[emp].tpEmis != (int)NFe.Components.TipoEmissao.teFSDA &&
                    Empresas.Configuracoes[emp].tpEmis != (int)NFe.Components.TipoEmissao.teOffLine &&
                    Empresas.Configuracoes[emp].tpEmis != (int)NFe.Components.TipoEmissao.teEPEC) //Confingência em formulário de segurança e EPEC não envia na hora, tem que aguardar voltar para normal.
                {
                    doExecute = true;
                }
                else
                {
                    if (nfe is TaskDFeRecepcao ||
                        nfe is TaskNFeRetRecepcao ||
                        nfe is TaskNFeConsultaStatus ||
                        nfe is TaskNFeConsultaSituacao ||
                        nfe is TaskDanfeContingencia ||
                        nfe is TaskDanfe ||
                        nfe is TaskCadastroContribuinte ||
                        nfe is TaskCTeRetRecepcao ||
                        nfe is TaskCTeConsultaStatus ||
                        nfe is TaskCTeConsultaSituacao ||
                        nfe is TaskMDFeRetRecepcao ||
                        nfe is TaskMDFeConsultaStatus ||
                        nfe is TaskMDFeConsultaSituacao ||
                        nfe is TaskMDFeConsNaoEncerrado ||
                        nfe is TaskLMCAutorizacao ||
                        (nfe is TaskNFeEventos && Empresas.Configuracoes[emp].tpEmis == (int)NFe.Components.TipoEmissao.teEPEC)||
                        (nfe is TaskCTeEventos && Empresas.Configuracoes[emp].tpEmis == (int)NFe.Components.TipoEmissao.teEPEC))
                    {
                        doExecute = true;
                    }
                }
            }
            if (doExecute)
                tipoServico.InvokeMember(metodo, System.Reflection.BindingFlags.InvokeMethod, null, nfe, null);
        }
        #endregion

        #region GravarXMLDadosCertificado()
        /// <summary>
        /// Gravar o XML de retorno com as informações do UniNFe para o aplicativo de ERP
        /// </summary>
        /// <param name="oNfe">Objeto da classe UniNfeClass para conseguir pegar algumas informações para gravar o XML</param>
        /// <by>Wandrey Mundin Ferreira</by>
        /// <date>29/01/2009</date>
        private void GravarXMLDadosCertificado(string ArquivoXml)
        {
            int emp = Empresas.FindEmpresaByThread();
            string sArqRetorno = string.Empty;

            Auxiliar oAux = new Auxiliar();

            if (Path.GetExtension(ArquivoXml).ToLower() == ".txt")
                sArqRetorno = Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" +
                              Functions.ExtrairNomeArq(ArquivoXml, Propriedade.ExtEnvio.ConsInf_TXT) + "-ret-cons-inf.txt";
            else
                sArqRetorno = Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" +
                              Functions.ExtrairNomeArq(ArquivoXml, Propriedade.ExtEnvio.ConsInf_XML) + "-ret-cons-inf.xml";

            try
            {
                Aplicacao app = new Aplicacao();

                //Deletar o arquivo de solicitação do serviço
                FileInfo oArquivo = new FileInfo(ArquivoXml);
                oArquivo.Delete();

                oArquivo = new FileInfo(sArqRetorno);
                if (oArquivo.Exists)
                    oArquivo.Delete();

                app.GravarXMLInformacoes(sArqRetorno);
            }
            catch (Exception ex)
            {
                try
                {
                    oAux.GravarArqErroERP(Path.GetFileNameWithoutExtension(sArqRetorno) + ".err", ex.Message);
                }
                catch
                {
                    //Se também falhou gravar o arquivo de retorno para o ERP, infelizmente não posso fazer mais nada. Deve estar com algum problema na rede, HD, permissão de acesso as pastas, etc... Wandrey 09/03/2010
                }
            }
        }
        #endregion

        #region ReconfigurarUniNFe()
        /// <summary>
        /// Reconfigura o UniNFe, gravando as novas informações na tela de configuração
        /// </summary>
        /// <param name="cArquivo">Nome do arquivo XML contendo as novas configurações</param>
        protected void ReconfigurarUniNFe(string cArquivo)
        {
            try
            {
                ConfiguracaoApp oConfig = new ConfiguracaoApp();
                oConfig.ReconfigurarUniNFe(cArquivo);
            }
            catch
            {
            }
        }
        #endregion

        #region GerarXMLPedRec()
        /// <summary>
        /// Gera o XML de consulta do recibo do lote de notas enviadas
        /// </summary>
        /// <param name="empresa">Index da empresa que é para gerar os pedidos de consulta do recibo do lote da nfe</param>
        /// <param name="nfe">Objeto da classe ServicoNfe</param>
        /// <by>Wandrey Mundin Ferreira</by>
        private void GerarXMLPedRec(int empresa, object nfe)
        {
            //Criar a lista dos recibos a serem consultados no SEFAZ
            List<ReciboCons> recibos = new List<ReciboCons>();

            FluxoNfe fluxoNfe = new FluxoNfe(empresa);

            try
            {
                recibos = fluxoNfe.CriarListaRec();
            }
            catch
            {
                //Não precisa fazer nada se não conseguiu criar a lista, somente con
            }

            Type tipoServico = nfe.GetType();

            for (int i = 0; i < recibos.Count; i++)
            {
                ReciboCons reciboCons = recibos[i];
                var tempoConsulta = reciboCons.tMed;

                if (tempoConsulta > 15)
                    tempoConsulta = 15; //Tempo previsto no manual da SEFAZ, isso foi feito pq o ambiente SVAN está retornando na consulta recibo, tempo superior a 160, mas não está com problema, é erro no calculo deste tempo. Wandrey

                if (tempoConsulta < Empresas.Configuracoes[empresa].TempoConsulta)
                    tempoConsulta = Empresas.Configuracoes[empresa].TempoConsulta;

                //Vou dar no mínimo 3 segundos para efetuar a consulta do recibo. Wandrey 21/11/2014
                if (tempoConsulta < 3)
                    tempoConsulta = 3;

                if (DateTime.Now.Subtract(reciboCons.dPedRec).Seconds >= tempoConsulta)
                {
                    //Atualizar a tag da data e hora da ultima consulta do recibo aumentando 10 segundos
                    fluxoNfe.AtualizarDPedRec(reciboCons.nRec, DateTime.Now.AddSeconds(10));
                    tipoServico.InvokeMember("XmlPedRec", System.Reflection.BindingFlags.InvokeMethod, null, nfe, new object[] { empresa, reciboCons.nRec, reciboCons.versao, reciboCons.mod });
                }
            }
        }
        #endregion

        #region Executa Limpeza
        /// <summary>
        /// executa a limpeza das pastas temp e retorno
        /// </summary>
        /// <by>http://desenvolvedores.net/marcelo</by>
        private void ExecutaLimpeza()
        {
            lock (this)
            {
                //Limpar conteúdo da pasta de LOG, mas manter 60 dias de informação
                Limpar(-1, Propriedade.PastaLog, "", 60);

                for (int i = 0; i < Empresas.Configuracoes.Count; i++)
                {
                    //Limpar conteúdo da pasta temp que fica dentro da pasta de envio de cada empresa a cada 10 dias
                    Limpar(i, Empresas.Configuracoes[i].PastaXmlEnvio, "temp", 10);
                    Limpar(i, Empresas.Configuracoes[i].PastaValidar, "temp", 10);   //danasa 12/8/2011
                    Limpar(i, Empresas.Configuracoes[i].PastaXmlEmLote, "temp", 10);   //Wandrey 05/10/2011

                    if (Empresas.Configuracoes[i].DiasLimpeza == 0)
                        continue;

                    #region temporario
                    Limpar(i, Empresas.Configuracoes[i].PastaXmlErro, "", Empresas.Configuracoes[i].DiasLimpeza);
                    #endregion

                    #region retorno
                    Limpar(i, Empresas.Configuracoes[i].PastaXmlRetorno, "", Empresas.Configuracoes[i].DiasLimpeza);
                    #endregion
                }
            }
        }

        private void Limpar(int empresa, string diretorio, string subdir, int diasLimpeza)
        {
            // danasa 27-2-2011
            if (diasLimpeza == 0 || string.IsNullOrEmpty(diretorio)) return;

            if (!Directory.Exists(diretorio)) return;   //danasa 12/8/2011

            if (!string.IsNullOrEmpty(subdir))
                diretorio += "\\" + subdir;

            if (!Directory.Exists(diretorio)) return;   //danasa 12/8/2011

            try
            {
                //recupera os arquivos da pasta temporario
                string[] files = Directory.GetFiles(diretorio, "*.*", SearchOption.AllDirectories);
                DateTime UltimaData = DateTime.Today.AddDays(-diasLimpeza);

                foreach (string file in files)
                {
                    FileInfo fi = new FileInfo(file);
                    //usar a última data de acesso, e não a data de criação
                    if (fi.LastWriteTime <= UltimaData)
                    {
                        try
                        {
                            fi.Delete();
                        }
                        catch
                        {
                            //td bem... nao deu para excluir. fica pra próxima
                        }
                    }
                    Application.DoEvents();
                }

            }
            catch (Exception ex)
            {
                if (empresa >= 0)
                    Functions.WriteLog(Empresas.Configuracoes[empresa].Nome + "\r\n" + ex.Message, false, true, Empresas.Configuracoes[empresa].CNPJ);
                else
                    Functions.WriteLog("Geral:\r\n" + ex.Message, false, true, "");
            }
        }
        #endregion

        #region CertVencido
        /// <summary>
        /// Verificar se o certificado digital está vencido
        /// </summary>
        /// <param name="emp">Empresa que é para ser verificado o certificado digital</param>
        /// <remarks>
        /// Retorna uma exceção ExceptionCertificadoDigital caso o certificado esteja vencido
        /// </remarks>
        protected void CertVencido(int emp)
        {
            //#if !DEBUG
            CertificadoDigital CertDig = new CertificadoDigital();
            if (CertDig.Vencido(emp))
            {
                throw new ExceptionCertificadoDigital(ErroPadrao.CertificadoVencido, "(" + CertDig.dValidadeInicial.ToString() + " a " + CertDig.dValidadeFinal.ToString() + ")");
            }
            //#endif
        }
        #endregion

        #region IsConnectedToInternet()
        /// <summary>
        /// Verifica se a conexão com a internet está OK
        /// </summary>
        /// <remarks>
        /// Retorna uma exceção ExceptionSemInternet caso a internet não esteja OK
        /// </remarks>
        protected void IsConnectedToInternet()
        {
            //Verificar antes se tem conexão com a internet, se não tiver já gera uma exceção no padrão já esperado pelo ERP
            if (ConfiguracaoApp.ChecarConexaoInternet)
                if (!Functions.IsConnectedToInternet())
                {
                    throw new ExceptionSemInternet(ErroPadrao.FalhaInternet);
                }
        }
        #endregion

        #region GravaErroERP()
        /// <summary>
        /// Gravar o erro ocorrido para o ERP
        /// </summary>
        /// <param name="arquivo">Nome do arquivo que seria processado</param>
        /// <param name="extRet">Extensão do arquivo de erro a ser gravado</param>
        /// <param name="servico">Serviço que está sendo executado</param>
        /// <param name="ex">Exception gerada</param>
        protected void GravaErroERP(string arquivo, Servicos servico, Exception ex, ErroPadrao erroPadrao)
        {
            string extRetERR = string.Empty;
            string extRet = string.Empty;

            switch (servico)
            {
                #region NFe / CTe / MDFe
                case Servicos.CTeInutilizarNumeros:
                case Servicos.NFeInutilizarNumeros:
                    extRet = Propriedade.ExtEnvio.PedInu_XML;
                    extRetERR = Propriedade.ExtRetorno.Inu_ERR;
                    break;

                case Servicos.CTePedidoConsultaSituacao:
                case Servicos.NFePedidoConsultaSituacao:
                case Servicos.MDFePedidoConsultaSituacao:
                    extRet = Propriedade.ExtEnvio.PedSit_XML;
                    extRetERR = Propriedade.ExtRetorno.Sit_ERR;
                    break;

                case Servicos.CTeConsultaStatusServico:
                case Servicos.NFeConsultaStatusServico:
                case Servicos.MDFeConsultaStatusServico:
                    extRet = Propriedade.ExtEnvio.PedSta_XML;
                    extRetERR = Propriedade.ExtRetorno.Sta_ERR;
                    break;

                case Servicos.CTePedidoSituacaoLote:
                case Servicos.MDFePedidoSituacaoLote:
                case Servicos.NFePedidoSituacaoLote:
                    extRet = Propriedade.ExtEnvio.PedRec_XML;
                    extRetERR = Propriedade.ExtRetorno.ProRec_ERR;
                    break;

                case Servicos.ConsultaCadastroContribuinte:
                    extRet = Propriedade.ExtEnvio.ConsCad_XML;
                    extRetERR = Propriedade.ExtRetorno.ConsCad_ERR;
                    break;

                case Servicos.CTeMontarLoteUm:
                    extRet = Propriedade.ExtEnvio.Cte;
                    extRetERR = Propriedade.ExtRetorno.Cte_ERR;
                    break;

                case Servicos.NFeMontarLoteUma:
                    extRet = Propriedade.ExtEnvio.Nfe;
                    extRetERR = Propriedade.ExtRetorno.Nfe_ERR;
                    break;

                case Servicos.MDFeMontarLoteUm:
                    extRet = Propriedade.ExtEnvio.MDFe;
                    extRetERR = Propriedade.ExtRetorno.MDFe_ERR;
                    break;

                case Servicos.CTeMontarLoteVarios:
                case Servicos.NFeMontarLoteVarias:
                case Servicos.MDFeMontarLoteVarios:
                    extRet = Propriedade.ExtEnvio.MontarLote;
                    extRetERR = Propriedade.ExtRetorno.MontarLote_ERR;
                    break;

                case Servicos.CTeEnviarLote:
                case Servicos.NFeEnviarLote:
                case Servicos.NFeEnviarLote2:
                case Servicos.MDFeEnviarLote:
                    extRet = Propriedade.ExtEnvio.EnvLot;
                    extRetERR = Propriedade.ExtRetorno.Rec_ERR;
                    break;

                case Servicos.CTeAssinarValidarEnvioEmLote:
                    extRet = Propriedade.ExtEnvio.Cte;
                    extRetERR = Propriedade.ExtRetorno.Cte_ERR;
                    break;

                case Servicos.MDFeAssinarValidarEnvioEmLote:
                    extRet = Propriedade.ExtEnvio.MDFe;
                    extRetERR = Propriedade.ExtRetorno.MDFe_ERR;
                    break;

                case Servicos.NFeAssinarValidarEnvioEmLote:
                    extRet = Propriedade.ExtEnvio.Nfe;
                    extRetERR = Propriedade.ExtRetorno.Nfe_ERR;
                    break;

                case Servicos.EventoRecepcao:
                case Servicos.CTeRecepcaoEvento:
                case Servicos.MDFeRecepcaoEvento:
                case Servicos.EventoEPEC:
                    extRet = Propriedade.ExtEnvio.PedEve;
                    extRetERR = Propriedade.ExtRetorno.Eve_ERR;
                    break;

                case Servicos.EventoCCe:
                    extRet = Propriedade.ExtEnvio.EnvCCe_XML;
                    extRetERR = Propriedade.ExtRetorno.retEnvCCe_ERR;
                    break;

                case Servicos.EventoManifestacaoDest:
                    extRet = Propriedade.ExtEnvio.EnvManifestacao_XML;
                    extRetERR = Propriedade.ExtRetorno.retManifestacao_ERR;
                    break;

                case Servicos.EventoCancelamento:
                    extRet = Propriedade.ExtEnvio.EnvCancelamento_XML;
                    extRetERR = Propriedade.ExtRetorno.retCancelamento_ERR;
                    break;

                case Servicos.NFeDownload:
                    extRet = Propriedade.ExtEnvio.EnvDownload_XML;
                    extRetERR = Propriedade.ExtRetorno.retDownload_ERR;
                    break;

                case Servicos.NFeConsultaNFDest:
                    extRet = Propriedade.ExtEnvio.ConsNFeDest_XML;
                    extRetERR = Propriedade.ExtRetorno.retConsNFeDest_ERR;
                    break;

                #endregion

                #region NFSe
                case Servicos.NFSeRecepcionarLoteRps:
                    extRet = Propriedade.ExtEnvio.EnvLoteRps;
                    extRetERR = Propriedade.ExtRetorno.RetLoteRps_ERR;
                    break;

                case Servicos.NFSeConsultarSituacaoLoteRps:
                    extRet = Propriedade.ExtEnvio.PedSitLoteRps;
                    extRetERR = Propriedade.ExtRetorno.SitLoteRps_ERR;
                    break;

                case Servicos.NFSeConsultarPorRps:
                    extRet = Propriedade.ExtEnvio.PedSitNfseRps;
                    extRetERR = Propriedade.ExtRetorno.SitNfseRps_ERR;
                    break;

                case Servicos.NFSeConsultar:
                    extRet = Propriedade.ExtEnvio.PedSitNfse;
                    extRetERR = Propriedade.ExtRetorno.SitNfse_ERR;
                    break;

                case Servicos.NFSeConsultarLoteRps:
                    extRet = Propriedade.ExtEnvio.PedLoteRps;
                    extRetERR = Propriedade.ExtRetorno.LoteRps_ERR;
                    break;

                case Servicos.NFSeCancelar:
                    extRet = Propriedade.ExtEnvio.PedCanNfse;
                    extRetERR = Propriedade.ExtRetorno.CanNfse_ERR;
                    break;

                #endregion

                #region Diversos
                case Servicos.UniNFeAlterarConfiguracoes:
                case Servicos.AssinarValidar:
                case Servicos.UniNFeConsultaInformacoes:
                case Servicos.NFeConverterTXTparaXML:
                case Servicos.EmProcessamento:
                case Servicos.NFeGerarChave:
                case Servicos.UniNFeLimpezaTemporario:
                    //Não tem definição pois não gera arquivo .ERR
                    break;
                #endregion

                default:
                    if (arquivo.EndsWith(Propriedade.ExtEnvio.PedSit_XML))
                    {
                        extRet = Propriedade.ExtEnvio.PedSit_XML;
                        extRetERR = Propriedade.ExtRetorno.Sit_ERR;
                    }
                    else
                    {
                        //Como não foi possível identificar o tipo do servico vou mudar somente a extensão para .err pois isso pode acontecer caso exista erro na estrutura do XML.
                        //Renan - 05/03/2014 
                        extRet = ".xml";
                        extRetERR = ".err";
                    }
                    break;
            }
            if (!string.IsNullOrEmpty(extRet))
                try
                {
                    //Gravar o arquivo de erro de retorno para o ERP, caso ocorra
                    TFunctions.GravarArqErroServico(arquivo, extRet, extRetERR, ex, erroPadrao, true);
                }
                catch
                {
                    //Se falhou algo na hora de gravar o retorno .ERR (de erro) para o ERP, infelizmente não posso fazer mais nada.
                    //Wandrey 02/06/2011
                }
        }
        #endregion

        #region ConsultaGeral()
        protected void ConsultarGeral(string arquivo)
        {
            string arq = arquivo.ToLower().Trim();

            if (arq.EndsWith(Propriedade.ExtEnvio.ConsCertificado))
            {
                ConsultaCertificados(arquivo);
            }
        }

        protected void ConsultaCertificados(string arquivo)
        {
            ConfiguracaoApp oConfig = new ConfiguracaoApp();
            oConfig.CertificadosInstalados(arquivo);
        }

        #endregion
    }
}