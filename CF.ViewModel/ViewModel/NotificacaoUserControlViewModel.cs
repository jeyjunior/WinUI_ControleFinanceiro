using CF.Domain.Enumeradores;
using CF.Domain.Extensoes;
using CF.Domain.Interfaces.ViewModel;
using Microsoft.UI.Xaml.Media;
using Org.BouncyCastle.Pqc.Crypto.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.ViewModel.ViewModel
{
    public class NotificacaoUserControlViewModel : ViewModelBase, INotificacaoUserControlViewModel
    {
        private SolidColorBrush _corNotificacao = null;
        public SolidColorBrush CorNotificacao => _corNotificacao;

        private string _mensagem = "";
        public string Mensagem => _mensagem; 

        public void DefinirTipoNotificacao(string mensagem, eNotificacao notificacao)
        {
            _mensagem = mensagem;

            switch (notificacao)
            {
                case eNotificacao.Informacao:
                    _corNotificacao = eCor.Azul3.ObterCor();
                    break;
                case eNotificacao.Sucesso:
                    _corNotificacao = eCor.Verde3.ObterCor();
                    break;
                case eNotificacao.Aviso:
                    _corNotificacao = eCor.Amarelo.ObterCor();
                    break;
                case eNotificacao.Erro:
                    _corNotificacao = eCor.Vermelho3.ObterCor();
                    break;
                default:
                    break;
            }

            PropriedadeAlterada(nameof(Mensagem));
            PropriedadeAlterada(nameof(CorNotificacao));
        }
    }
}
