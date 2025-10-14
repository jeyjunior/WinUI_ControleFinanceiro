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
    public sealed partial class CategoriaPage : Page
    {
        #region Interfaces
        private readonly ICategoriaViewModel _categoriaViewModel;
        #endregion

        #region Propriedades
        #endregion

        #region Método Construtor
        public CategoriaPage()
        {
            InitializeComponent();
            _categoriaViewModel = Bootstrap.ServiceProvider.GetRequiredService<ICategoriaViewModel>();

            this.DataContext = _categoriaViewModel;
            dtgPrincipal.ItemsSource = _categoriaViewModel.CategoriaCollection;
        }
        #endregion

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _categoriaViewModel.CarregarColecoes();
            _categoriaViewModel.DefinirTipoOperacao(eTipoOperacaoCrud.Cancelar);
            dtgPrincipal.SelectedIndex = _categoriaViewModel.SelecionarIndice;
        }
        private void btnDeletar_Click(object sender, RoutedEventArgs e)
        {
            _categoriaViewModel.DefinirTipoOperacao(eTipoOperacaoCrud.Excluir);
        }
        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            _categoriaViewModel.DefinirTipoOperacao(eTipoOperacaoCrud.Editar);
            txtNome.Focus(FocusState.Keyboard);
            txtNome.SelectAll();
        }
        private void btnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            _categoriaViewModel.DefinirTipoOperacao(eTipoOperacaoCrud.Adicionar);
            txtNome.Focus(FocusState.Keyboard);
            txtNome.SelectAll();
        }
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            _categoriaViewModel.DefinirTipoOperacao(eTipoOperacaoCrud.Cancelar);
            dtgPrincipal.SelectedIndex = _categoriaViewModel.SelecionarIndice;
        }
        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            _categoriaViewModel.Salvar();
            dtgPrincipal.SelectedIndex = _categoriaViewModel.SelecionarIndice;
        }
        private void dtgPrincipal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Categoria categoria = new Categoria();

            if (dtgPrincipal.SelectedItem != null)
                categoria = dtgPrincipal.SelectedItem as Categoria;

            _categoriaViewModel.DefinirItemSelecionado(categoria);
        }
    }
}