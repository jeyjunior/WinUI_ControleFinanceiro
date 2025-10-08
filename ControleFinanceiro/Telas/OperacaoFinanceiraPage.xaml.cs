using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using CF.Domain.Entidades;
using CF.Domain.Enumeradores;
using CF.Domain.Interfaces.ViewModel;
using CF.ViewModel;

namespace ControleFinanceiro.Telas
{
    public sealed partial class OperacaoFinanceiraPage : Page
    {
        #region Interfaces
        private readonly IOperacaoFinanceiraViewModel _operacaoFinanceiraViewModel;
        #endregion

        #region Propriedades
        #endregion

        #region Método Construtor
        public OperacaoFinanceiraPage()
        {
            InitializeComponent();
            _operacaoFinanceiraViewModel = Bootstrap.ServiceProvider.GetRequiredService<IOperacaoFinanceiraViewModel>();

            this.DataContext = _operacaoFinanceiraViewModel;
            dtgPrincipal.ItemsSource = _operacaoFinanceiraViewModel.OperacaoFinanceiraCollection;
        }
        #endregion

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _operacaoFinanceiraViewModel.CarregarColecoes();
            _operacaoFinanceiraViewModel.DefinirTipoOperacao(eTipoOperacao.Cancelar);
            dtgPrincipal.SelectedIndex = _operacaoFinanceiraViewModel.SelecionarIndice;
        }
        private void btnDeletar_Click(object sender, RoutedEventArgs e)
        {
            _operacaoFinanceiraViewModel.DefinirTipoOperacao(eTipoOperacao.Excluir);
        }
        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            _operacaoFinanceiraViewModel.DefinirTipoOperacao(eTipoOperacao.Editar);
        }
        private void btnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            _operacaoFinanceiraViewModel.DefinirTipoOperacao(eTipoOperacao.Adicionar);
        }
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            _operacaoFinanceiraViewModel.DefinirTipoOperacao(eTipoOperacao.Cancelar);
            dtgPrincipal.SelectedIndex = _operacaoFinanceiraViewModel.SelecionarIndice;
        }
        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            _operacaoFinanceiraViewModel.Salvar();
            dtgPrincipal.SelectedIndex = _operacaoFinanceiraViewModel.SelecionarIndice;
        }
        private void dtgPrincipal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OperacaoFinanceiraGrid operacaoFinanceiraGrid = new OperacaoFinanceiraGrid();

            if (dtgPrincipal.SelectedItem != null)
                operacaoFinanceiraGrid = dtgPrincipal.SelectedItem as OperacaoFinanceiraGrid;

            _operacaoFinanceiraViewModel.DefinirItemSelecionado(operacaoFinanceiraGrid);
        }
    }
}