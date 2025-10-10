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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

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
            var cadastroOperacaoDialog = new CadastroOperacaoDialog(CF.Domain.Enumeradores.eTipoOperacao.Adicionar, 0);

            cadastroOperacaoDialog.XamlRoot = this.XamlRoot;
            cadastroOperacaoDialog.HorizontalAlignment = HorizontalAlignment.Center;
            cadastroOperacaoDialog.VerticalAlignment = VerticalAlignment.Center;

            await cadastroOperacaoDialog.ShowAsync();
        }
        private void scroll_LayoutUpdated(object sender, object e)
        {

        }
        #endregion

        #region Metodos
        #endregion
    }
}
