using CF.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Domain.Interfaces.ViewModel
{
    public interface IOperacaoViewModel
    {
        ObservableCollection<OperacaoFinanceiraGrid> OperacaoFinanceiraCollection { get; }
        OperacaoFinanceiraResumo OperacaoFinanceiraResumo { get; }
        int PK_OperacaoFinanceiraSelecionada { get; set; }
        bool HabilitaBotaoEditarExcluir { get; }
        bool HabilitarBotaoPesquisa { get; }
        bool DataInicialSelecionada { get; set; }
        bool DataFinalSelecionada { get; set; }
        string Total { get; }
        void CarregarColecoes();
        void Pesquisar(DateTime dataInicial, DateTime dataFinal);
    }
}
