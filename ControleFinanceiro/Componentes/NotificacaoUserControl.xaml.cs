using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using CF.Domain.Enumeradores;
using CF.Domain.Extensoes;
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
        public NotificacaoUserControl()
        {
            InitializeComponent();
        }
        public void DefinirTipoNotificacao(string mensagem, eNotificacao notificacao = eNotificacao.Informacao)
        {
           txtNotificacao.Text = mensagem;

            switch (notificacao)
            {
                case eNotificacao.Informacao:
                    gPrincipal.BorderBrush = eCor.Azul3.ObterCor();
                    break;
                case eNotificacao.Sucesso:
                    gPrincipal.BorderBrush = eCor.Verde3.ObterCor();
                    break;
                case eNotificacao.Aviso:
                    gPrincipal.BorderBrush = eCor.Amarelo.ObterCor();
                    break;
                case eNotificacao.Erro:
                    gPrincipal.BorderBrush = eCor.Vermelho3.ObterCor();
                    break;
                default:
                    gPrincipal.BorderBrush = eCor.Cinza10.ObterCor();
                    break;
            }
        }
        public async Task ExibirAsync(int durationMs = 2000)
        {
            try
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

                var tcs = new TaskCompletionSource<bool>();
                sbOut.Completed += (s, e) =>
                {
                    (this.Parent as Panel)?.Children.Remove(this);
                    tcs.SetResult(true);
                };

                sbOut.Begin();
                await tcs.Task;
            }
            catch (TaskCanceledException)
            {
                (this.Parent as Panel)?.Children.Remove(this);
            }
        }
        public void ForceClose()
        {
            (this.Parent as Panel)?.Children.Remove(this);
        }
    }
}
