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
    public interface IEntidadeViewModel : IViewModelBase
    {
        ObservableCollection<EntidadeFinanceira> EntidadeFinanceiraCollection { get; }
        EntidadeFinanceira EntidadeFinanceiraSelecionada { get; }

        bool ExibeBotoesCrud { get; }
        bool ExibeBotoesConfirmacao { get; }
        bool HabilitaBotaoEditarExcluir { get; }
        string Nome { get; set; }
        bool HabilitarNome { get; }
        int SelecionarIndice { get; }


        void CarregarColecoes();
        void DefinirTipoOperacao(eTipoOperacaoCrud tipoOperacao);
        void DefinirItemSelecionado(EntidadeFinanceira entidadeFinanceira);
        void Salvar();
    }
}
