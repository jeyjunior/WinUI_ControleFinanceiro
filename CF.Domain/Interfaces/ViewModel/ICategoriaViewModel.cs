using CF.Domain.Entidades;
using CF.Domain.Enumeradores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Domain.Interfaces.ViewModel
{
    public interface ICategoriaViewModel : IViewModelBase
    {
        ObservableCollection<Categoria> CategoriaCollection { get; }
        Categoria CategoriaSelecionada { get; }

        bool ExibeBotoesCrud { get; }
        bool ExibeBotoesConfirmacao { get; }
        bool HabilitaBotaoEditarExcluir { get; }
        string Nome { get; set; }
        bool HabilitarNome { get; }
        int SelecionarIndice { get; }
        string Total { get; }
        void CarregarColecoes();
        void DefinirTipoOperacao(eTipoOperacaoCrud tipoOperacao);
        void DefinirItemSelecionado(Categoria categoria);
        void Salvar();
    }
}
