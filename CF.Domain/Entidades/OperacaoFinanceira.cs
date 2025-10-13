using CF.Domain.Atributos;
using CF.Domain.Enumeradores;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Domain.Entidades
{
    [Entidade("OperacaoFinanceira")]
    public class OperacaoFinanceira
    {
        [ChavePrimaria]
        public int PK_OperacaoFinanceira { get; set; }
        
        [Obrigatorio, Relacionamento("TipoOperacao", "PK_TipoOperacao")]
        public int FK_TipoOperacao { get; set; }

        [Obrigatorio, Relacionamento("EntidadeFinanceira", "PK_EntidadeFinanceira")]
        public int FK_EntidadeFinanceira { get; set; }

        [Obrigatorio, Relacionamento("Categoria", "PK_Categoria")]
        public int FK_Categoria { get; set; }
        
        [Obrigatorio, TamanhoDecimal(18, 2)]
        public decimal Valor { get; set; }
        public DateTime? DataTransacao { get; set; }
        
        [Obrigatorio]
        public DateTime DataVencimento { get; set; } 

        [TamanhoString(200)]
        public string Anotacao { get; set; }

        [Relacionamento("Usuario", "PK_Usuario")]
        public int? FK_Usuario { get; set; }

        [Editavel(false)]
        public eStatusPagamento StatusPagamento { get; set; }

        [Editavel(false)]
        public TipoOperacao TipoOperacao { get; set; }

        [Editavel(false)]
        public EntidadeFinanceira EntidadeFinanceira { get; set; }

        [Editavel(false)]
        public Categoria Categoria { get; set; }
    }

    public class OperacaoFinanceiraGrid
    {
        [ChavePrimaria]
        public int PK_OperacaoFinanceira { get; set; }
        public int FK_TipoOperacao { get; set; }
        public string Valor { get; set; }
        public string DataTransacaoFormatado { get; set; }
        public DateTime? DataTransacao { get; set; }
        public string DataVencimentoFormatado { get; set; }
        public DateTime DataVencimento { get; set; }
        public string Anotacao { get; set; }
        public string TipoOperacao { get; set; }
        public string EntidadeFinanceira { get; set; }
        public string Categoria { get; set; }
        public string StatusPagamentoIcone { get; set; }
        public SolidColorBrush StatusOperacaoCor { get; set; }
        public SolidColorBrush StatusPagamentoCor { get; set; }
        public SolidColorBrush DataVencimentoCor { get; set; }  
        public string StatusOperacaoIcone { get; set; }
    }
}
