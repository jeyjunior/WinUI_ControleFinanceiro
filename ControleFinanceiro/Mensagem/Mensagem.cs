using CF.Domain.Enumeradores;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleFinanceiro.Mensagem
{
    public static class Mensagem
    {
        public static async Task<eMensagemResultado> ExibirAsync(this XamlRoot xamlRoot, string mensagem, eMensagem tipoMensagem)
        {
            var (titulo, botoes) = ObterConfiguracaoMensagem(tipoMensagem);

            var contentDialog = new ContentDialog
            {
                Title = titulo,
                Content = mensagem,
                XamlRoot = xamlRoot
            };

            ConfigurarBotoes(contentDialog, botoes);

            var resultado = await contentDialog.ShowAsync();

            return TratarResultado(resultado, tipoMensagem);
        }

        public static async Task<bool> ExibirConfirmacaoAsync(this XamlRoot xamlRoot, string mensagem)
        {
            var contentDialog = new ContentDialog
            {
                Title = "Confirmação",
                Content = mensagem,
                PrimaryButtonText = "Sim",
                CloseButtonText = "Não",
                XamlRoot = xamlRoot,
                DefaultButton = ContentDialogButton.Primary
            };

            var resultado = await contentDialog.ShowAsync();
            return resultado == ContentDialogResult.Primary;
        }

        private static (string Titulo, eBotoesMensagem Botoes) ObterConfiguracaoMensagem(eMensagem tipoMensagem)
        {
            return tipoMensagem switch
            {
                eMensagem.Informacao => ("Informação", eBotoesMensagem.OK),
                eMensagem.Erro => ("Erro", eBotoesMensagem.OK),
                eMensagem.Pergunta => ("Pergunta", eBotoesMensagem.SimNao),
                eMensagem.Confirmacao => ("Confirmação", eBotoesMensagem.OKCancelar),
                _ => ("Mensagem", eBotoesMensagem.OK)
            };
        }

        private static void ConfigurarBotoes(ContentDialog dialog, eBotoesMensagem botoes)
        {
            switch (botoes)
            {
                case eBotoesMensagem.OK:
                    dialog.PrimaryButtonText = "OK";
                    dialog.DefaultButton = ContentDialogButton.Primary;
                    break;

                case eBotoesMensagem.SimNao:
                    dialog.PrimaryButtonText = "Sim";
                    dialog.CloseButtonText = "Não";
                    dialog.DefaultButton = ContentDialogButton.Primary;
                    break;

                case eBotoesMensagem.OKCancelar:
                    dialog.PrimaryButtonText = "OK";
                    dialog.CloseButtonText = "Cancelar";
                    dialog.DefaultButton = ContentDialogButton.Primary;
                    break;
            }
        }

        private static eMensagemResultado TratarResultado(ContentDialogResult resultado, eMensagem tipoMensagem)
        {
            var ret = eMensagemResultado.Nenhuma;
            
            if (tipoMensagem == eMensagem.Informacao || tipoMensagem == eMensagem.Erro)
            {
                return eMensagemResultado.OK;
            }
            else if(tipoMensagem == eMensagem.Confirmacao)
            {
                ret = resultado == ContentDialogResult.Primary ? eMensagemResultado.OK : eMensagemResultado.Cancelar;
            }
            else if(tipoMensagem == eMensagem.Pergunta)
            {
                ret = resultado == ContentDialogResult.Primary ? eMensagemResultado.Sim : eMensagemResultado.Nao;
            }

            return ret;
        }
    }
}

