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
    public sealed partial class EntidadePage : Page
    {
        #region Interfaces
        private readonly IEntidadeViewModel _entidadeViewModel;
        #endregion

        #region Propriedades
        #endregion

        #region Método Construtor
        public EntidadePage()
        {
            InitializeComponent();
            _entidadeViewModel = Bootstrap.ServiceProvider.GetRequiredService<IEntidadeViewModel>();

            this.DataContext = _entidadeViewModel;
            dtgPrincipal.ItemsSource = _entidadeViewModel.EntidadeFinanceiraCollection;
        }
        #endregion

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _entidadeViewModel.CarregarColecoes();
            _entidadeViewModel.DefinirTipoOperacao(eTipoOperacaoCrud.Cancelar);
            dtgPrincipal.SelectedIndex = _entidadeViewModel.SelecionarIndice;
        }
        private void btnDeletar_Click(object sender, RoutedEventArgs e)
        {
            _entidadeViewModel.DefinirTipoOperacao(eTipoOperacaoCrud.Excluir);
        }
        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            _entidadeViewModel.DefinirTipoOperacao(eTipoOperacaoCrud.Editar);
            txtNome.Focus(FocusState.Keyboard);
            txtNome.SelectAll();
        }
        private void btnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            _entidadeViewModel.DefinirTipoOperacao(eTipoOperacaoCrud.Adicionar);
            txtNome.Focus(FocusState.Keyboard);
            txtNome.SelectAll();
        }
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            _entidadeViewModel.DefinirTipoOperacao(eTipoOperacaoCrud.Cancelar);
            dtgPrincipal.SelectedIndex = _entidadeViewModel.SelecionarIndice;
        }
        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            _entidadeViewModel.Salvar();
            dtgPrincipal.SelectedIndex = _entidadeViewModel.SelecionarIndice;
        }
        private void dtgPrincipal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EntidadeFinanceira entidadeFinanceira = new EntidadeFinanceira();

            if (dtgPrincipal.SelectedItem != null)
                entidadeFinanceira = dtgPrincipal.SelectedItem as EntidadeFinanceira;

            _entidadeViewModel.DefinirItemSelecionado(entidadeFinanceira);
        }
    }
}