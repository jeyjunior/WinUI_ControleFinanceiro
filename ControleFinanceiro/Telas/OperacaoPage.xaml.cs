using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.VisualBasic;
using CF.Domain.Entidades;
using CF.Domain.Interfaces.ViewModel;
using CF.ViewModel;
using CF.ViewModel.ViewModel;

namespace ControleFinanceiro.Telas
{
    public sealed partial class OperacaoPage : Page
    {
        #region Interfaces
        private readonly IOperacaoViewModel _operacaoViewModel;
        #endregion

        #region Propriedades
        #endregion

        #region Construtor
        public OperacaoPage()
        {
            InitializeComponent();

            _operacaoViewModel = Bootstrap.ServiceProvider.GetRequiredService<IOperacaoViewModel>();

            this.DataContext = _operacaoViewModel;
            dtgPrincipal.ItemsSource = _operacaoViewModel.OperacaoFinanceiraCollection;
        }
        #endregion

        #region Eventos
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CarregarMesAtual();
            Pesquisar();
        }
        private async void btnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var cadastroOperacaoDialog = new CadastroOperacaoDialog(CF.Domain.Enumeradores.eTipoOperacaoCrud.Adicionar, 0);

                cadastroOperacaoDialog.XamlRoot = this.XamlRoot;
                cadastroOperacaoDialog.HorizontalAlignment = HorizontalAlignment.Center;
                cadastroOperacaoDialog.VerticalAlignment = VerticalAlignment.Center;

                await cadastroOperacaoDialog.ShowAsync();

                Pesquisar();
            }
            catch (Exception ex)
            {
                var m = new MessageDialog(ex.Message);
                await m.ShowAsync();
            }
        }
        private async void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                if (btn == null)
                    return;

                var cadastroOperacaoDialog = new CadastroOperacaoDialog(CF.Domain.Enumeradores.eTipoOperacaoCrud.Editar, _operacaoViewModel.PK_OperacaoFinanceiraSelecionada);

                cadastroOperacaoDialog.XamlRoot = this.XamlRoot;
                cadastroOperacaoDialog.HorizontalAlignment = HorizontalAlignment.Center;
                cadastroOperacaoDialog.VerticalAlignment = VerticalAlignment.Center;

                await cadastroOperacaoDialog.ShowAsync();

                Pesquisar();
            }
            catch (Exception ex)
            {
                var m = new MessageDialog(ex.Message);
                await m.ShowAsync();
            }
        }
        private async void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                if (btn == null)
                    return;

                var cadastroOperacaoDialog = new CadastroOperacaoDialog(CF.Domain.Enumeradores.eTipoOperacaoCrud.Excluir, _operacaoViewModel.PK_OperacaoFinanceiraSelecionada);

                cadastroOperacaoDialog.XamlRoot = this.XamlRoot;
                cadastroOperacaoDialog.HorizontalAlignment = HorizontalAlignment.Center;
                cadastroOperacaoDialog.VerticalAlignment = VerticalAlignment.Center;
                
                await cadastroOperacaoDialog.ShowAsync();
                
                Pesquisar();
            }
            catch (Exception ex)
            {
                var m = new MessageDialog(ex.Message);
                await m.ShowAsync();
            }
        }
        private void dtgPrincipal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _operacaoViewModel.PK_OperacaoFinanceiraSelecionada = 0;

            if (dtgPrincipal.SelectedItem != null)
            {
                var operacaoFinanceiraGrid = dtgPrincipal.SelectedItem as OperacaoFinanceiraGrid;
                if (operacaoFinanceiraGrid != null)
                    _operacaoViewModel.PK_OperacaoFinanceiraSelecionada = operacaoFinanceiraGrid.PK_OperacaoFinanceira;
            }
        }
        private void btnPesquisar_Click(object sender, RoutedEventArgs e)
        {
            Pesquisar();
        }
        private void scroll_LayoutUpdated(object sender, object e)
        {

        }
        private void cdpDataFinal_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            _operacaoViewModel.DataFinalSelecionada = (args.NewDate != null);
        }
        private void cdpDataInicial_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            _operacaoViewModel.DataInicialSelecionada = (args.NewDate != null);
        }
        #endregion

        #region Metodos
        private void CarregarMesAtual()
        {
            DateTime hoje = DateTime.Today;
            DateTime primeiroDiaDoMes = new DateTime(hoje.Year, hoje.Month, 1);
            DateTime ultimoDiaDoMes = primeiroDiaDoMes.AddMonths(1).AddDays(-1);

            cdpDataInicial.Date = primeiroDiaDoMes;
            cdpDataFinal.Date = ultimoDiaDoMes;
        }
        private void Pesquisar()
        {
            DateTime dataInicial = cdpDataInicial.Date.Value.DateTime;
            DateTime dataFinal = cdpDataFinal.Date.Value.DateTime;

            _operacaoViewModel.Pesquisar(dataInicial, dataFinal);
        }
        #endregion
    }
}
