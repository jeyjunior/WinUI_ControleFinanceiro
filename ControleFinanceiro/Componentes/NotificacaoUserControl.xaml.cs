using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using CF.Domain.Enumeradores;
using CF.Domain.Interfaces.ViewModel;
using CF.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;

namespace ControleFinanceiro.Componentes
{
    public sealed partial class NotificacaoUserControl : UserControl
    {
        private readonly INotificacaoUserControlViewModel _viewModel;

        public NotificacaoUserControl()
        {
            InitializeComponent();

            _viewModel = Bootstrap.ServiceProvider.GetRequiredService<INotificacaoUserControlViewModel>();

            this.DataContext = _viewModel;
        }
        public void DefinirTipoNotificacao(string mensagem, eNotificacao notificacao = eNotificacao.Informacao)
        {
            _viewModel.DefinirTipoNotificacao(mensagem, notificacao);
        }
        public async Task ExibirAsync(int durationMs = 3000)
        {
            // Fade in
            var fadeIn = new FadeInThemeAnimation();
            gPrincipal.Opacity = 1;
            Storyboard sbIn = new Storyboard();
            sbIn.Children.Add(fadeIn);
            Storyboard.SetTarget(fadeIn, gPrincipal);
            sbIn.Begin();

            await Task.Delay(durationMs);

            // Fade out
            var fadeOut = new FadeOutThemeAnimation();
            Storyboard sbOut = new Storyboard();
            sbOut.Children.Add(fadeOut);
            Storyboard.SetTarget(fadeOut, gPrincipal);
            sbOut.Completed += (s, e) =>
            {
                (this.Parent as Panel)?.Children.Remove(this);
            };

            sbOut.Begin();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            (this.Parent as Panel)?.Children.Remove(this);
        }
    }
}
