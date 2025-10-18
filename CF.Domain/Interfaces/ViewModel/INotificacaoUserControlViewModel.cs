using CF.Domain.Enumeradores;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Domain.Interfaces.ViewModel
{
    public interface INotificacaoUserControlViewModel : IViewModelBase
    {
        SolidColorBrush CorNotificacao { get; }
        string Mensagem { get; }
        void DefinirTipoNotificacao(string mensagem, eNotificacao notificacao);
    }
}
