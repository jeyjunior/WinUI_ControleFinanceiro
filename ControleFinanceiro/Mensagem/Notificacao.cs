using ControleFinanceiro.Componentes;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleFinanceiro.Mensagem
{
    public static class Notificacao
    {
        private static Grid _container;

        public static void RegisterContainer(Grid container)
        {
            _container = container;
        }

        public static void Exibir(string message)
        {
            if (_container == null) return;

            var alert = new NotificacaoUserControl();
            alert.SetMessage(message);
            _container.Children.Add(alert);
            _ = alert.ShowAsync();
        }
    }
}
