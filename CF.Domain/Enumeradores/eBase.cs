using CF.Domain.Atributos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Domain.Enumeradores
{
    public enum eTipoBancoDados
    {
        SQLite = 1,
        SQLServer = 2,
        MySQL = 3,
    }

    public enum eTipoOperacaoCrud
    {
        Adicionar = 1,
        Editar = 2,
        Excluir = 3,
        Salvar = 4,
        Cancelar = 5
    }
    public enum eHabilitarEdicao
    {
        Sim = 1,
        Nao = 2
    }
    public enum eStatusPagamento
    {
        Nenhum = 0,
        [CodigoGlyph("\xE73E")]
        Pago = 1,
        [CodigoGlyph("\xE823")]
        EmAberto = 2,
        [CodigoGlyph("\xE783")]
        Vencido = 3
    }
    public enum eTipoOperacao
    {
        Nenhum = 0,
        [CodigoGlyph("\xEB11")]
        Receita = 1,
        [CodigoGlyph("\xEB0F")]
        Despesa = 2
    }

    public enum eCor
    {
        Verde1,
        Verde2,
        Verde3,
        Verde4,

        Azul1,
        Azul2,
        Azul3,
        Azul4,
        Azul5,

        Vermelho1,
        Vermelho2,
        Vermelho3,
        Vermelho4,
        Vermelho5,
        Vermelho6,
        Vermelho7,

        Vermelho5Transp10,

        Amarelo,
        Laranja,
        Laranja1,
        Roxo,
        Rosa,
        Violeta,

        Cinza1,
        Cinza2,
        Cinza3,
        Cinza4,
        Cinza5,
        Cinza6,
        Cinza7,
        Cinza8,
        Cinza9,
        Cinza10,
        Cinza11,
        Cinza12,
        Cinza13,

        Cinza7Transp10,
        Cinza7Transp20,
        Cinza7Transp30,
        Cinza7Transp40,
        Cinza7Transp50,

        Cinza8Transp10,
        Cinza8Transp20,
        Cinza8Transp30,
        Cinza8Transp40,
        Cinza8Transp50,

        Cinza9Transp10,
        Cinza9Transp20,
        Cinza9Transp30,
        Cinza9Transp40,
        Cinza9Transp50,

        Cinza10Transp10,
        Cinza10Transp20,
        Cinza10Transp30,
        Cinza10Transp40,
        Cinza10Transp50,

        Cinza11Transp10,
        Cinza11Transp20,
        Cinza11Transp30,
        Cinza11Transp40,
        Cinza11Transp50,

        Cinza12Transp10,
        Cinza12Transp20,
        Cinza12Transp30,
        Cinza12Transp40,
        Cinza12Transp50,

        Cinza13Transp10,
        Cinza13Transp20,
        Cinza13Transp30,
        Cinza13Transp40,
        Cinza13Transp50,

        Nenhuma,

        Branco,
        Preto
    }

    public enum eNotificacao
    {
        Informacao = 1,
        Sucesso = 2,
        Aviso = 3,
        Erro = 4
    }

    public enum eMensagem
    {
        Informacao = 1,
        Pergunta = 2,
        Confirmacao = 3,
        Erro = 4
    }

    public enum eBotoesMensagem
    {
        OK = 1,
        SimNao = 2,
        OKCancelar = 3
    }
    public enum eMensagemResultado
    {
        Nenhuma = 0,
        OK = 1,
        Cancelar = 2,
        Sim = 3,
        Nao = 4
    }
}
