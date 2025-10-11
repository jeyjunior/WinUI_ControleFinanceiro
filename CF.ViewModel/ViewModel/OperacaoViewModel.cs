using CF.Domain.Entidades;
using CF.Domain.Enumeradores;
using CF.Domain.Interfaces.Repository;
using CF.Domain.Interfaces.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.ViewModel.ViewModel
{
    public class OperacaoViewModel : ViewModelBase, IOperacaoViewModel
    {
        private readonly IOperacaoFinanceiraRepository _operacaoFinanceiraRepository;

        public OperacaoViewModel()
        {
            _operacaoFinanceiraRepository = Bootstrap.ServiceProvider.GetRequiredService<IOperacaoFinanceiraRepository>();
        }
        public ObservableCollection<OperacaoFinanceiraGrid> OperacaoFinanceiraCollection { get; set; } = new ObservableCollection<OperacaoFinanceiraGrid>();
        public void CarregarColecoes()
        {
            CarregarColecaoOperacaoFinanceira();
        }
        private void CarregarColecaoOperacaoFinanceira()
        {
            if (OperacaoFinanceiraCollection == null)
                OperacaoFinanceiraCollection = new ObservableCollection<OperacaoFinanceiraGrid>();

            if (OperacaoFinanceiraCollection.Any())
                OperacaoFinanceiraCollection.Clear();

            var operacaoFinanceira = _operacaoFinanceiraRepository.ObterListaGrid();

            foreach (var item in operacaoFinanceira)
            {
                item.Valor = Convert.ToDecimal(item.Valor).ToString("N2");

                DefinirStatusPagamento(item);
                DefinirStatusOperacao(item);

                OperacaoFinanceiraCollection.Add(item);
            }

            PropriedadeAlterada(nameof(OperacaoFinanceiraCollection));
        }
        private void DefinirStatusPagamento(OperacaoFinanceiraGrid operacao)
        {
            if (operacao.FK_StatusPagamento == 1)
            {
                operacao.StatusPagamentoCor = (SolidColorBrush)Microsoft.UI.Xaml.Application.Current.Resources["Verde1"];
                operacao.StatusPagamentoIcone = "\xEB11";
            }
            else
            {
                operacao.StatusPagamentoCor = (SolidColorBrush)Microsoft.UI.Xaml.Application.Current.Resources["Vermelho2"];
                operacao.StatusPagamentoIcone = "\xEB0F";
            }
        }
        private void DefinirStatusOperacao(OperacaoFinanceiraGrid operacao)
        {
            operacao.DataVencimentoFormatado = operacao.DataVencimento != null ? Convert.ToDateTime(operacao.DataVencimento).ToShortDateString() : "-";
            operacao.DataTransacaoFormatado = operacao.DataTransacao != null ? Convert.ToDateTime(operacao.DataTransacao).ToShortDateString() : "-";

            operacao.StatusOperacaoCor = null;
            if (operacao.DataTransacao != null && operacao.DataTransacao.Value <= DateTime.Now)
            {
                operacao.StatusOperacaoCor = (SolidColorBrush)Microsoft.UI.Xaml.Application.Current.Resources["Verde2"];
            }
            else if (operacao.DataTransacao == null && (operacao.DataVencimento != null)) 
            { 
                if (operacao.DataVencimento.Value <= DateTime.Now)
                {
                    operacao.StatusOperacaoCor = (SolidColorBrush)Microsoft.UI.Xaml.Application.Current.Resources["Vermelho2"];
                }
            }
            
            if (operacao.StatusOperacaoCor == null)
            {
                operacao.StatusOperacaoCor = (SolidColorBrush)Microsoft.UI.Xaml.Application.Current.Resources["Cinza2"];
            }
        }
    }
}
