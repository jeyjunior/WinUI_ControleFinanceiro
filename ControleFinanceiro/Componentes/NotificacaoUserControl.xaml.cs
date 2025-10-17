using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ControleFinanceiro.Componentes
{
    public sealed partial class NotificacaoUserControl : UserControl
    {
        public NotificacaoUserControl()
        {
            InitializeComponent();
        }
        public void SetMessage(string message)
        {
            MessageText.Text = message;
        }

        public async Task ShowAsync(int durationMs = 3000)
        {
            // Fade in
            var fadeIn = new FadeInThemeAnimation();
            gPrincipal.Opacity = 1;
            Storyboard sbIn = new Storyboard();
            sbIn.Children.Add(fadeIn);
            Storyboard.SetTarget(fadeIn, gPrincipal);
            sbIn.Begin();

            return;
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
