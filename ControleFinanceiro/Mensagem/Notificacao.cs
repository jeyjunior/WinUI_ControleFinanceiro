using CF.Domain.Enumeradores;
using ControleFinanceiro.Componentes;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Linq;

namespace ControleFinanceiro.Mensagem
{
    public static class Notificacao
    {
        private static Grid _container;
        private static readonly Queue<NotificacaoUserControl> _filaNotificacoes = new Queue<NotificacaoUserControl>();
        private static readonly List<NotificacaoUserControl> _notificacoesAtivas = new List<NotificacaoUserControl>();

        public static void RegisterContainer(Grid container)
        {
            _container = container;
        }

        public static void Exibir(NotificacaoRequest request)
        {
            if (_container == null)
                return;

            var notificacao = new NotificacaoUserControl();
            notificacao.DefinirTipoNotificacao(request.Mensagem, request.TipoNotificacao);

            AdicionarNotificacao(notificacao);
        }

        private static void AdicionarNotificacao(NotificacaoUserControl notificacao)
        {
            if (_notificacoesAtivas.Count >= 3)
            {
                RemoverNotificacaoMaisAntiga();
            }

            _notificacoesAtivas.Add(notificacao);

            ReorganizarNotificacoes();

            _ = notificacao.ExibirAsync().ContinueWith(t =>
            {
                RemoverNotificacao(notificacao);
            }, System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext());
        }

        private static void ReorganizarNotificacoes()
        {
            _container.Children.Clear();

            for (int i = 0; i < _notificacoesAtivas.Count; i++)
            {
                var notificacao = _notificacoesAtivas[i];
                Grid.SetRow(notificacao, i); 
                _container.Children.Add(notificacao);
            }
        }

        private static void RemoverNotificacao(NotificacaoUserControl notificacao)
        {
            _notificacoesAtivas.Remove(notificacao);
            _container.Children.Remove(notificacao);

            ReorganizarNotificacoes();
        }

        private static void RemoverNotificacaoMaisAntiga()
        {
            if (_notificacoesAtivas.Count > 0)
            {
                var notificacaoMaisAntiga = _notificacoesAtivas[0];
                notificacaoMaisAntiga.ForceClose();
                _notificacoesAtivas.RemoveAt(0);
                _container.Children.Remove(notificacaoMaisAntiga);
            }
        }
    }

    public class NotificacaoRequest
    {
        public string Mensagem { get; set; }
        public eNotificacao TipoNotificacao { get; set; }
    }
}