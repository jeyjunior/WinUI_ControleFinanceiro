using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CF.Domain.Entidades;
using CF.Domain.Enumeradores;
using CF.Domain.Extensoes;
using CF.Domain.Interfaces.Repository;
using CF.Domain.Interfaces.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Media;

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
           
        }
   
        public void Pesquisar(DateTime dataInicial, DateTime dataFinal)
        {
            if (OperacaoFinanceiraCollection == null)
                OperacaoFinanceiraCollection = new ObservableCollection<OperacaoFinanceiraGrid>();

            if (OperacaoFinanceiraCollection.Any())
                OperacaoFinanceiraCollection.Clear();

            string _inicial = dataInicial.ToString("yyyy-MM-dd");
            string _final = dataFinal.AddDays(1).ToString("yyyy-MM-dd");

            var operacaoFinanceira = _operacaoFinanceiraRepository.ObterListaGrid("OperacaoFinanceira.DataVencimento >= @DataInicial AND OperacaoFinanceira.DataVencimento < @DataFinal", new {DataInicial = _inicial, DataFinal = _final});

            foreach (var item in operacaoFinanceira)
            {
                item.Valor = Convert.ToDecimal(item.Valor).ToString("N2");

                DefinirStatusOperacao(item);
                DefinirStatusPagamento(item);

                OperacaoFinanceiraCollection.Add(item);
            }

            PropriedadeAlterada(nameof(OperacaoFinanceiraCollection));
        }
        private void DefinirStatusOperacao(OperacaoFinanceiraGrid operacao)
        {
            operacao.DataVencimentoFormatado = Convert.ToDateTime(operacao.DataVencimento).ToShortDateString();
            operacao.DataTransacaoFormatado = operacao.DataTransacao != null ? Convert.ToDateTime(operacao.DataTransacao).ToShortDateString() : "";
            
            if (operacao.FK_TipoOperacao == 1)
            {
                operacao.StatusOperacaoCor = eCor.Verde2.ObterCor();
                operacao.StatusOperacaoIcone = "\xEB11";
            }
            else
            {
                operacao.StatusOperacaoCor = eCor.Vermelho2.ObterCor();
                operacao.StatusOperacaoIcone = "\xEB0F";
            }
        }

        public void DefinirStatusPagamento(OperacaoFinanceiraGrid operacao)
        {
            if (operacao.DataTransacao != null)
            {
                operacao.StatusPagamentoIcone = eStatusPagamento.Pago.ObterCodigoGlyph();
                operacao.StatusPagamentoCor = eCor.Verde1.ObterCor();
                operacao.DataVencimentoCor = eCor.Verde1.ObterCor();
                return;
            }

            DateTime hoje = DateTime.Now.Date;
            DateTime vencimento = operacao.DataVencimento.Date;

            if (vencimento < hoje)
            {
                operacao.StatusPagamentoIcone = eStatusPagamento.Vencido.ObterCodigoGlyph();
                operacao.StatusPagamentoCor = eCor.Vermelho1.ObterCor();
                operacao.DataVencimentoCor = eCor.Vermelho1.ObterCor();
            }
            else if (vencimento == hoje)
            {
                operacao.StatusPagamentoIcone = eStatusPagamento.EmAberto.ObterCodigoGlyph();
                operacao.StatusPagamentoCor = eCor.Laranja.ObterCor();
                operacao.DataVencimentoCor = eCor.Laranja.ObterCor();
            }
            else if (vencimento == hoje.AddDays(1))
            {
                operacao.StatusPagamentoIcone = eStatusPagamento.EmAberto.ObterCodigoGlyph();
                operacao.StatusPagamentoCor = eCor.Amarelo.ObterCor();
                operacao.DataVencimentoCor = eCor.Amarelo.ObterCor();
            }
            else
            {
                operacao.StatusPagamentoIcone = eStatusPagamento.EmAberto.ObterCodigoGlyph();
                operacao.StatusPagamentoCor = eCor.Cinza1.ObterCor();
                operacao.DataVencimentoCor = eCor.Cinza1.ObterCor();
            }
        }
    }
}
