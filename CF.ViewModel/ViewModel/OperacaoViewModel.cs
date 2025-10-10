using CF.Domain.Entidades;
using CF.Domain.Enumeradores;
using CF.Domain.Interfaces.Repository;
using CF.Domain.Interfaces.ViewModel;
using Microsoft.Extensions.DependencyInjection;
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
                item.Valor = "R$ " + Convert.ToDecimal(item.Valor).ToString("N2");
                item.DataVencimento = item.DataVencimento != null ? Convert.ToDateTime(item.DataVencimento).ToShortDateString() : "-";
                item.DataTransacao = item.DataTransacao != null ? Convert.ToDateTime(item.DataTransacao).ToShortDateString() : "-";

                OperacaoFinanceiraCollection.Add(item);
            }

            PropriedadeAlterada(nameof(OperacaoFinanceiraCollection));
        }
    }
}
