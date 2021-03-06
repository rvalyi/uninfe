﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace NFe.ConvertTxt
{
    public static class versoes
    {
        #region NFe
        public static string VersaoXMLStatusServico = "3.10";
        public static string VersaoXMLNFe = "3.10";
        public static string VersaoXMLPedSit = "3.10";
        public static string VersaoXMLInut = "3.10";
        public static string VersaoXMLConsCad = "2.00";
        public static string VersaoXMLEvento = "1.00";
        public static string VersaoXMLEnvConsultaNFeDest = "1.01";
        public static string VersaoXMLEnvDownload = "1.00";
        public static string VersaoXMLEnvDFe = "1.00";
        #endregion

        #region MDF-e
        public static string VersaoXMLMDFeCabecMsg = "1.00";
        public static string VersaoXMLMDFeStatusServico = "1.00";
        public static string VersaoXMLMDFe = "1.00";
        public static string VersaoXMLMDFePedRec = "1.00";
        public static string VersaoXMLMDFePedSit = "1.00";
        public static string VersaoXMLMDFeEvento = "1.00";
        public static string VersaoXMLMDFeConsNaoEnc = "1.00";
        #endregion

        #region CT-e
        public static string VersaoXMLCTeCabecMsg = "2.00";
        public static string VersaoXMLCTeStatusServico = "2.00";
        public static string VersaoXMLCTe = "2.00";
        public static string VersaoXMLCTePedRec = "2.00";
        public static string VersaoXMLCTePedSit = "2.00";
        public static string VersaoXMLCTeInut = "2.00";
        public static string VersaoXMLCTeEvento = "2.00";
        #endregion
    }
    ///
    /// NFC-e
    /// 
    public enum TpcnDestinoOperacao {
        doInterna = 1, 
        doInterestadual = 2, 
        doExterior = 3
    }
    public enum TpcnConsumidorFinal {
        cfNao = 0, 
        cfConsumidorFinal = 1
    }
    public enum TpcnPresencaComprador {
        pcNao=0, 
        pcPresencial=1, 
        pcInternet=2, 
        pcTeleatendimento=3, 
        pcOutros=9
    }
    public enum TpcnFormaPagamento {
        fpDinheiro=1, 
        fpCheque=2, 
        fpCartaoCredito=3, 
        fpCartaoDebito=4, 
        fpCreditoLoja=5,
        fpValeAlimentacao=10, 
        fpValeRefeicao=11, 
        fpValePresente=12, 
        fpValeCombustivel=13,
        fpOutro=99
    }
    public enum TpcnBandeiraCartao
    {
        bcVisa=1, 
        bcMasterCard=2, 
        bcAmericanExpress=3, 
        bcSorocred=4, 
        bcOutros=99
    }

    public enum TpcnProcessoEmissao
    {
        peAplicativoContribuinte = 0,
        peAvulsaFisco = 1,
        peAvulsaContribuinte = 2,
        peContribuinteAplicativoFisco = 3
    }
    public enum TpcnModalidadeFrete 
    { 
        mfContaEmitente = 0, 
        mfContaDestinatario = 1, 
        mfContaTerceiros = 2, 
        mfSemFrete = 9
    }
    public enum TpcnDeterminacaoBaseIcms 
    { 
        dbiMargemValorAgregado, 
        dbiPauta, 
        dbiPrecoTabelado, 
        dbiValorOperacao 
    }
    public enum TpcnDeterminacaoBaseIcmsST 
    { 
        dbisPrecoTabelado = 0, 
        dbisListaNegativa = 1, 
        dbisListaPositiva = 2, 
        dbisListaNeutra = 3, 
        dbisMargemValorAgregado = 4, 
        dbisPauta = 5
    }
    public enum TpcnOrigemMercadoria
    {
        oeNacional = 0,
        oeEstrangeiraImportacaoDireta = 1,
        oeEstrangeiraAdquiridaBrasil = 2,
		oeNacional_Mercadoria_ou_bem_com_Conteúdo_de_Importação_superior_a_40 = 3,
        oeNacional_Cuja_produção_tenha_sido_feita_em_conformidade_com_o_PPB = 4,
        oeNacional_Mercadoria_com_bem_ou_conteúdo_de_importação_inferior_a_40 = 5,
        oeEstrangeira_Importação_direta_sem_similar_nacional = 6,
        oeEstrangeira_Adquirida_no_mercado_interno_com_similar_nacional=7
    }
    public enum TpcnTipoArma 
    { 
        taUsoPermitido = 0, 
        taUsoRestrito = 1
    }
    public enum TpcnCondicaoVeiculo 
    { 
        cvAcabado = 1, 
        cvInacabado = 2, 
        cvSemiAcabado = 3
    }
    public enum TpcnTipoOperacao 
    { 
        toVendaConcessionaria = 1, 
        toFaturamentoDireto = 2, 
        toVendaDireta = 3, 
        toOutros = 0
    }
    public enum TpcnIndicadorTotal 
    { 
        itNaoSomaTotalNFe = 0, 
        itSomaTotalNFe = 1 
    }
    public enum TpcnCRT 
    { 
        crtSimplesNacional = 1, 
        crtSimplesExcessoReceita = 2,
        crtRegimeNormal = 3
    }
    public enum TpcnTipoCampo 
    {
        tcDec2 = 2, tcDec3 = 3, tcDec4 = 4, tcDec5 = 5, tcDec6 = 6, tcDec7 = 7, tcDec8 = 8, tcDec9 = 9, tcDec10 = 10,
        tcStr, tcInt, tcDatYYYY_MM_DD, tcDatYYYYMMDD, tcHor, tcDatHor
    }

    public enum TpcnIndicadorPagamento 
    { 
        ipVista = 0, 
        ipPrazo = 1, 
        ipOutras = 2 
    }
    public enum TpcnTipoNFe 
    { 
        tnEntrada = 0, 
        tnSaida = 1
    }
    public enum TpcnTipoImpressao 
    { 
        tiNao = 0,
        tiRetrato = 1, 
        tiPaisagem = 2,
        tiDANFESimplificado = 3,
        tiDANFENFCe = 4,
        tiDANFENFCe_em_mensagem_eletrônica = 5
    }
    public enum TpcnFinalidadeNFe
    {
        fnNormal = 1,
        fnComplementar = 2,
        fnAjuste = 3,
        fnDevolucao = 4
    }

    public enum TpcnECFModRef 
    {
        ECFModRefVazio, 
        ECFModRef2B,
        ECFModRef2C,
        ECFModRef2D /*'', '2B', '2C','2D'*/
    }

    public enum tpEventos
    {
        [Description("Carta de Correcao")]
        tpEvCCe = 110110,
        [Description("Cancelamento")]
        tpEvCancelamentoNFe = 110111,
        [Description("Confirmacao da Operacao")]
        tpEvConfirmacaoOperacao = 210200,
        [Description("Ciencia da Operacao")]
        tpEvCienciaOperacao = 210210,
        [Description("Desconhecimento da Operacao")]
        tpEvDesconhecimentoOperacao = 210220,
        [Description("EPEC")]
        tpEvEPEC = 110140,
        [Description("Operação nao Realizada")]
        tpEvOperacaoNaoRealizada = 210240,
        [Description("Encerramento MDFe")]
        tpEvEncerramentoMDFe = 110112,
        [Description("Inclusao de condutor")]
        tpEvInclusaoCondutor = 110114,
        [Description("Registro de passagem")]
        tpEvRegistroPassagem = 310620,
        [Description("Registro de passagem-BRid")]
        tpEvRegistroPassagemBRid = 510620,
        [Description("Registro Multimodal")]
        tpevRegMultimodal = 110160
    }

    public enum TpcnTipoAutor
    {
        taEmpresaEmitente = 1,
        taEmpresaDestinataria = 2,
        taEmpresa = 3,
        taFisco = 5,
        taRFB = 6,
        taOutros = 9
    }

    public enum TpcnCSTIcms
    {
        cst00,
        cst10,
        cst20,
        cst30,
        cst40,
        cst41,
        cst45,
        cst50,
        cst51,
        cst60,
        cst70,
        cst80,
        cst81,
        cst90,
        cstPart10,
        cstPart90,
        cstRep41,
        cstVazio,
        cstICMSOutraUF,
        cstICMSSN
    } //80 e 81 apenas para CTe

    internal enum ObOp
    {
        Obrigatorio,
        Opcional
    }

    public enum TpcnindIEDest {
        inContribuinte=1, 
        inIsento=2, 
        inNaoContribuinte=9
    }

    public enum TpcnMod
    {
        modNFe = 55,
        modNFCe = 65,
        modCTe = 57,
        modMDFe = 58,
        modIntefinido = 99
    }

    public enum TpcnTipoViaTransp
    {
        [Description("Marítima")]
        tvMaritima = 1,
        [Description("Fluvial")]
        tvFluvial = 2,
        [Description("Lacustre")]
        tvLacustre = 3,
        [Description("Aerea")]
        tvAerea = 4,
        [Description("Postal")]
        tvPostal = 5,
        [Description("Ferroviária")]
        tvFerroviaria = 6,
        [Description("Rodoviária")]
        tvRodoviaria = 7,
        [Description("Conduto/Rede Transmissão")]
        tvConduto = 8,
        [Description("Meios próprios")]
        tvMeiosProprios = 9,
        [Description("Entrada/Saida ficta")]
        tvEntradaSaidaFicta = 10
    }

    public enum TpcnTipoIntermedio
    {
        [Description("Importação por conta própria")]
        tiContaPropria = 1,
        [Description("Importação por conta e ordem")]
        tiContaOrdem = 2,
        [Description("Importação por encomenda")]
        tiEncomenda = 3
    }

    public enum TpcnindISS
    {
        iiExigivel = 1, 
        iiNaoIncidencia = 2, 
        iiIsencao = 3, 
        iiExportacao = 4,
        iiImunidade = 5, 
        iiExigSuspDecisaoJudicial = 6, 
        iiExigSuspProcessoAdm = 7
    }

    public enum TpcnRegimeTributario
    {
        Nenhum = 0,
        Microempresa_Municipal = 1,
        Estimativa = 2,
        Sociedade_de_Profissionais = 3,
        Cooperativa = 4,
        Microempresário_Individual__MEI = 5,
        Microempresário_e_Empresa_de_Pequeno_Porte__ME_EPP = 6
    }
}
