using CF.Domain.Entidades;
using CF.Domain.Enumeradores;
using CF.Domain.Interfaces.ViewModel;
using CF.ViewModel;
using CF.ViewModel.ViewModel;
using ControleFinanceiro.Mensagem;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

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
            lstPrincipal.ItemsSource = _categoriaViewModel.CategoriaCollection;
        }
        #endregion

        #region Eventos
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _categoriaViewModel.CarregarColecoes();
            _categoriaViewModel.DefinirTipoOperacao(eTipoOperacaoCrud.Cancelar);
            lstPrincipal.SelectedIndex = _categoriaViewModel.SelecionarIndice;
        }
        private async void btnDeletar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var ret = await this.XamlRoot.ExibirAsync("Tem certeza que deseja excluir essa entidade financeira?\nEsta operação não poderá ser desfeita.", eMensagem.Pergunta);

                if (ret != eMensagemResultado.Sim)
                    return;

                _categoriaViewModel.DefinirTipoOperacao(eTipoOperacaoCrud.Excluir);
                var retorno = _categoriaViewModel.Salvar();

                if (!retorno.Sucesso)
                {
                    string erros = string.Join("\n", retorno.Erros);
                    await this.XamlRoot.ExibirAsync(erros, eMensagem.Confirmacao);
                }
            }
            catch (Exception ex)
            {
                await this.XamlRoot.ExibirAsync(ex.Message, eMensagem.Confirmacao);
            }
        }
        private async void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _categoriaViewModel.DefinirTipoOperacao(eTipoOperacaoCrud.Editar);
                txtNome.Focus(FocusState.Keyboard);
                txtNome.SelectAll();
            }
            catch (Exception ex)
            {
                await this.XamlRoot.ExibirAsync(ex.Message, eMensagem.Confirmacao);
            }
        }
        private async void btnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _categoriaViewModel.DefinirTipoOperacao(eTipoOperacaoCrud.Adicionar);
                txtNome.Focus(FocusState.Keyboard);
                txtNome.SelectAll();
            }
            catch (Exception ex)
            {
                await this.XamlRoot.ExibirAsync(ex.Message, eMensagem.Confirmacao);
            }
        }
        private async void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _categoriaViewModel.DefinirTipoOperacao(eTipoOperacaoCrud.Cancelar);
                lstPrincipal.SelectedIndex = _categoriaViewModel.SelecionarIndice;
            }
            catch (Exception ex)
            {
                await this.XamlRoot.ExibirAsync(ex.Message, eMensagem.Confirmacao);
            }
        }
        private async void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var ret = _categoriaViewModel.Salvar();
                lstPrincipal.SelectedIndex = _categoriaViewModel.SelecionarIndice;

                if (ret.Sucesso)
                {
                    Notificacao.Exibir(new NotificacaoRequest
                    {
                        Mensagem = "Operação realizada com sucesso.",
                        TipoNotificacao = eNotificacao.Sucesso
                    });
                }
                else
                {
                    Notificacao.Exibir(new NotificacaoRequest
                    {
                        Mensagem = ret.Erros.FirstOrDefault(),
                        TipoNotificacao = eNotificacao.Aviso
                    });
                }
            }
            catch (Exception ex)
            {
                await this.XamlRoot.ExibirAsync(ex.Message, eMensagem.Confirmacao);
            }
        }
        private async void lstPrincipal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Categoria categoria = new Categoria();

                if (lstPrincipal.SelectedItem != null)
                    categoria = (Categoria)lstPrincipal.SelectedItem;

                _categoriaViewModel.DefinirItemSelecionado(categoria);
            }
            catch (Exception ex)
            {
                await this.XamlRoot.ExibirAsync(ex.Message, eMensagem.Confirmacao);
            }
        }
        #endregion
    }
}