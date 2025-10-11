using CF.Domain.Interfaces.ViewModel;
using CF.ViewModel;
using CF.ViewModel.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ControleFinanceiro.Telas
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
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
        }
        #endregion

        #region Eventos
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _operacaoViewModel.CarregarColecoes();
        }
        private async void btnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var cadastroOperacaoDialog = new CadastroOperacaoDialog(CF.Domain.Enumeradores.eTipoOperacao.Adicionar, 0);

                cadastroOperacaoDialog.XamlRoot = this.XamlRoot;
                cadastroOperacaoDialog.HorizontalAlignment = HorizontalAlignment.Center;
                cadastroOperacaoDialog.VerticalAlignment = VerticalAlignment.Center;

                await cadastroOperacaoDialog.ShowAsync();
                _operacaoViewModel.CarregarColecoes();
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

                int pK_OperacaoFinanceira = Convert.ToInt32(btn.Tag);
                var cadastroOperacaoDialog = new CadastroOperacaoDialog(CF.Domain.Enumeradores.eTipoOperacao.Editar, pK_OperacaoFinanceira);

                cadastroOperacaoDialog.XamlRoot = this.XamlRoot;
                cadastroOperacaoDialog.HorizontalAlignment = HorizontalAlignment.Center;
                cadastroOperacaoDialog.VerticalAlignment = VerticalAlignment.Center;

                await cadastroOperacaoDialog.ShowAsync();
                _operacaoViewModel.CarregarColecoes();
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

                int pK_OperacaoFinanceira = Convert.ToInt32(btn.Tag);

                var cadastroOperacaoDialog = new CadastroOperacaoDialog(CF.Domain.Enumeradores.eTipoOperacao.Excluir, pK_OperacaoFinanceira);

                cadastroOperacaoDialog.XamlRoot = this.XamlRoot;
                cadastroOperacaoDialog.HorizontalAlignment = HorizontalAlignment.Center;
                cadastroOperacaoDialog.VerticalAlignment = VerticalAlignment.Center;

                await cadastroOperacaoDialog.ShowAsync();
                _operacaoViewModel.CarregarColecoes();
            }
            catch (Exception ex)
            {
                var m = new MessageDialog(ex.Message);
                await m.ShowAsync();
            }
        }
        private void scroll_LayoutUpdated(object sender, object e)
        {

        }
        #endregion

        #region Metodos
        #endregion


    }
}
