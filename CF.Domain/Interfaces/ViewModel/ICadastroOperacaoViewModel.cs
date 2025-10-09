using CF.Domain.Entidades;
using CF.Domain.Enumeradores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Domain.Interfaces.ViewModel
{
    public interface ICadastroOperacaoViewModel : IViewModelBase
    {
        OperacaoFinanceira OperacaoFinanceiraSelecionada { get; }

        ObservableCollection<TipoOperacao> TipoOperacaoCollection { get; }
        int PK_TipoOperacaoSelecionada { get; set; }

        ObservableCollection<Categoria> CategoriaCollection { get; }
        int PK_CategoriaSelecionada { get; set; }

        ObservableCollection<EntidadeFinanceira> EntidadeFinanceiraCollection { get; }
        int PK_EntidadeFinanceiraSelecionada { get; set; }

        string Valor { get; set; }

        DateTimeOffset? DataVencimento { get; set; }
        string DataVencimentoFormatada { get; set; }
        DateTimeOffset? DataTransacao { get; set; }
        string DataTransacaoFormatada { get; set; }

        string Anotacao { get; set; }

        void Salvar();
        void Limpar();
        void DefinirOperacao(eTipoOperacao _tipoOperacao, int PK_OperacaoFinanceira);
    }
}
