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
    public interface IOperacaoFinanceiraViewModel : IViewModelBase
    {
        ObservableCollection<OperacaoFinanceiraGrid> OperacaoFinanceiraCollection { get; }
        OperacaoFinanceiraGrid OperacaoFinanceiraSelecionada { get; }

        ObservableCollection<TipoOperacao> TipoOperacaoCollection { get; }
        int PK_TipoOperacaoSelecionada { get; set; }

        ObservableCollection<Categoria> CategoriaCollection { get; }
        int PK_CategoriaSelecionada { get; set; }

        ObservableCollection<EntidadeFinanceira> EntidadeFinanceiraCollection { get; }
        int PK_EntidadeFinanceiraSelecionada { get; set; }

        decimal Valor { get; set; }

        DateTime? DataVencimento { get; set; }

        bool ExibeBotoesCrud { get; }
        bool ExibeBotoesConfirmacao { get; }
        bool HabilitaBotaoEditarExcluir { get; }
        int SelecionarIndice { get; }


        void CarregarColecoes();
        void DefinirTipoOperacao(eTipoOperacao tipoOperacao);
        void DefinirItemSelecionado(OperacaoFinanceiraGrid operacaoFinanceiraGrid);
        void Salvar();
    }
}
