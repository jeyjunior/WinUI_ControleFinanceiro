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

        private int _pK_OperacaoFinanceiraSelecionada;
        public int PK_OperacaoFinanceiraSelecionada 
        { 
            get => _pK_OperacaoFinanceiraSelecionada;
            set 
            {
                _pK_OperacaoFinanceiraSelecionada = value;
                PropriedadeAlterada(nameof(HabilitaBotaoEditarExcluir));
            } 
        }

        public OperacaoViewModel()
        {
            _operacaoFinanceiraRepository = Bootstrap.ServiceProvider.GetRequiredService<IOperacaoFinanceiraRepository>();
        }
        public ObservableCollection<OperacaoFinanceiraGrid> OperacaoFinanceiraCollection { get; set; } = new ObservableCollection<OperacaoFinanceiraGrid>();
        public OperacaoFinanceiraResumo OperacaoFinanceiraResumo { get; set; } = new OperacaoFinanceiraResumo();

        public string Total { get => ("Operações financeiras: " + OperacaoFinanceiraCollection.Count.ToString("N0")); }
        public bool HabilitaBotaoEditarExcluir { get => OperacaoFinanceiraCollection.Count > 0 && PK_OperacaoFinanceiraSelecionada > 0; }

        private bool _dataFinalSelecionada = false;
        public bool DataFinalSelecionada 
        { 
            get => _dataFinalSelecionada; 
            set 
            { 
                _dataFinalSelecionada = value;
                PropriedadeAlterada(nameof(HabilitarBotaoPesquisa)); 
            } 
        }
        private bool _dataInicialSelecionada = false;
        public bool DataInicialSelecionada
        {
            get => _dataInicialSelecionada;
            set
            {
                _dataInicialSelecionada = value;
                PropriedadeAlterada(nameof(HabilitarBotaoPesquisa));
            }
        }
        public bool HabilitarBotaoPesquisa 
        { 
            get => _dataFinalSelecionada && _dataInicialSelecionada;
        }
        public void CarregarColecoes()
        {
           
        }
        public void Pesquisar(DateTime dataInicial, DateTime dataFinal)
        {
            if (OperacaoFinanceiraCollection == null)
                OperacaoFinanceiraCollection = new ObservableCollection<OperacaoFinanceiraGrid>();

            if (OperacaoFinanceiraCollection.Any())
                OperacaoFinanceiraCollection.Clear();

            DateTime _inicial = dataInicial;
            DateTime _final = dataFinal.AddDays(1);

            var operacaoFinanceira = _operacaoFinanceiraRepository.ObterListaGrid("OperacaoFinanceira.DataVencimento >= @DataInicial AND OperacaoFinanceira.DataVencimento < @DataFinal", new {DataInicial = _inicial, DataFinal = _final});

            foreach (var item in operacaoFinanceira)
            {
                DefinirStatusOperacao(item);
                DefinirStatusPagamento(item);

                OperacaoFinanceiraCollection.Add(item);
            }

            OperacaoFinanceiraResumo = _operacaoFinanceiraRepository.ObterResumoOperacao("DataVencimento >= @DataInicial AND DataVencimento < @DataFinal", new { DataInicial = _inicial, DataFinal = _final });
            FormatarDadosDoResumo(OperacaoFinanceiraResumo);

            PropriedadeAlterada(nameof(OperacaoFinanceiraCollection));
            PropriedadeAlterada(nameof(OperacaoFinanceiraResumo));
            PropriedadeAlterada(nameof(Total));
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
                operacao.OperacaoCor = eCor.Cinza1.ObterCor();
                return;
            }

            DateTime hoje = DateTime.Now.Date;
            DateTime vencimento = operacao.DataVencimento.Date;

            if (vencimento < hoje)
            {
                operacao.StatusPagamentoIcone = eStatusPagamento.Vencido.ObterCodigoGlyph();
                operacao.OperacaoCor = eCor.Vermelho3.ObterCor();
            }
            else if (vencimento >= hoje && vencimento <= hoje.AddDays(2))
            {
                operacao.StatusPagamentoIcone = eStatusPagamento.EmAberto.ObterCodigoGlyph();
                operacao.OperacaoCor = eCor.Amarelo.ObterCor();
            }
            else
            {
                operacao.StatusPagamentoIcone = eStatusPagamento.EmAberto.ObterCodigoGlyph();
                operacao.OperacaoCor = eCor.Cinza1.ObterCor();
            }
        }
        public void FormatarDadosDoResumo(OperacaoFinanceiraResumo resumo)
        {
            if (resumo == null)
                resumo = new OperacaoFinanceiraResumo();

            // RECEITA
            resumo.ReceitaProgressoAtual = Convert.ToInt64(resumo.TotalReceitaPaga);
            resumo.ReceitaProgressoMaximo = Convert.ToInt64(resumo.TotalReceita);
            resumo.TotalReceitaPagaFormatado = resumo.TotalReceitaPaga.ToString("N2");
            resumo.TotalReceitaFormatado = resumo.TotalReceita.ToString("N2");
            resumo.PercentualReceita = (resumo.TotalReceita == 0) ? "0%" : ((float)(resumo.TotalReceitaPaga / resumo.TotalReceita) * 100).ToString("N2") + "%";

            // DESPESA
            resumo.DespesaProgressoAtual = Convert.ToInt64(resumo.TotalDespesaPaga);
            resumo.DespesaProgressoMaximo = Convert.ToInt64(resumo.TotalDespesa);
            resumo.TotalDespesaPagaFormatado = resumo.TotalDespesaPaga.ToString("N2");
            resumo.TotalDespesaFormatado = resumo.TotalDespesa.ToString("N2");
            resumo.PercentualDespesa = (resumo.TotalDespesa == 0) ? "0%" : ((float)(resumo.TotalDespesaPaga / resumo.TotalDespesa) * 100).ToString("N2") + "%";

            // SALDO
            resumo.SaldoFormatado = ("R$ " + resumo.Saldo.ToString("N2"));

            if (resumo.Saldo >= 0)
            {
                resumo.SaldoFormatadoIcone = eTipoOperacao.Receita.ObterCodigoGlyph();
                resumo.SaldoFormatadoCor = eCor.Verde1.ObterCor();
            }
            else
            {
                resumo.SaldoFormatadoIcone = eTipoOperacao.Despesa.ObterCodigoGlyph();
                resumo.SaldoFormatadoCor = eCor.Vermelho1.ObterCor();
            }
        }
    }
}
