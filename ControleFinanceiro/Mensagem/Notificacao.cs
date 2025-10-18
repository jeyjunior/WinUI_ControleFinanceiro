using CF.Domain.Enumeradores;
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

        public static void Exibir(string mensagem, eNotificacao notificacao)
        {
            if (_container == null) 
                return;

            var notificacaoUserControl = new NotificacaoUserControl();
            _container.Children.Add(notificacaoUserControl);

            notificacaoUserControl.DefinirTipoNotificacao(mensagem, notificacao);
            _ = notificacaoUserControl.ExibirAsync();
        }
    }
}
