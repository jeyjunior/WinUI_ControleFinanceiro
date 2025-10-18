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
using ControleFinanceiro.Mensagem;
using Windows.UI.Popups;

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

        #region Eventos
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _entidadeViewModel.CarregarColecoes();
            _entidadeViewModel.DefinirTipoOperacao(eTipoOperacaoCrud.Cancelar);
            dtgPrincipal.SelectedIndex = _entidadeViewModel.SelecionarIndice;
        }
        private async void btnDeletar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var ret = await this.XamlRoot.ExibirAsync("Tem certeza que deseja excluir essa entidade financeira?\nEsta operação não poderá ser desfeita.", eMensagem.Pergunta);

                if (ret != eMensagemResultado.Sim)
                    return;

                _entidadeViewModel.DefinirTipoOperacao(eTipoOperacaoCrud.Excluir);
                _entidadeViewModel.Salvar();
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
                _entidadeViewModel.DefinirTipoOperacao(eTipoOperacaoCrud.Editar);
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
                _entidadeViewModel.DefinirTipoOperacao(eTipoOperacaoCrud.Adicionar);
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
                _entidadeViewModel.DefinirTipoOperacao(eTipoOperacaoCrud.Cancelar);
                dtgPrincipal.SelectedIndex = _entidadeViewModel.SelecionarIndice;
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
                var ret = _entidadeViewModel.Salvar();
                dtgPrincipal.SelectedIndex = _entidadeViewModel.SelecionarIndice;

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
        private async void dtgPrincipal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                EntidadeFinanceira entidadeFinanceira = new EntidadeFinanceira();

                if (dtgPrincipal.SelectedItem != null)
                    entidadeFinanceira = dtgPrincipal.SelectedItem as EntidadeFinanceira;

                _entidadeViewModel.DefinirItemSelecionado(entidadeFinanceira);
            }
            catch (Exception ex)
            {
                await this.XamlRoot.ExibirAsync(ex.Message, eMensagem.Confirmacao);
            }
        }
        #endregion
    }
}