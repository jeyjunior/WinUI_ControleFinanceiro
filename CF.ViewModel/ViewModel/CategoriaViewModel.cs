using CF.Domain.Entidades;
using CF.Domain.Enumeradores;
using CF.Domain.Interfaces.Repository;
using CF.Domain.Interfaces.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.ViewModel.ViewModel
{
    public class CategoriaViewModel : ViewModelBase, ICategoriaViewModel
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private eHabilitarEdicao _habilitarEdicao;
        private eTipoOperacao _tipoOperacao;
        public CategoriaViewModel()
        {
            _categoriaRepository = Bootstrap.ServiceProvider.GetRequiredService<ICategoriaRepository>();
        }

        // COLEÇÕES
        private ObservableCollection<Categoria> _categoriaCollection = new ObservableCollection<Categoria>();
        public ObservableCollection<Categoria> CategoriaCollection { get => _categoriaCollection;}

        // ITEM SELECIONADO
        private Categoria _categoriaSelecionada = new Categoria();
        public Categoria CategoriaSelecionada { get => _categoriaSelecionada; }

        // PROPRIEDADES OBJETO PRINCIPAL
        private string _nome = "";
        public string Nome 
        { 
            get => _nome;
            set => _nome = value;
        }

        // CONTROLAR EXIBIÇÃO DE COMPONENTES
        public bool ExibeBotoesCrud { get => this._habilitarEdicao == eHabilitarEdicao.Nao; }
        public bool ExibeBotoesConfirmacao { get => !ExibeBotoesCrud; }
        public bool HabilitaBotaoEditarExcluir { get => _categoriaCollection.Count > 0 && _categoriaSelecionada != null && _categoriaSelecionada.PK_Categoria > 0; }

        // METÓDOS
        public void CarregarCategorias()
        {
            if (_categoriaCollection == null)
                _categoriaCollection = new ObservableCollection<Categoria>();

            if (_categoriaCollection.Any())
                _categoriaCollection.Clear();

            var categorias = _categoriaRepository.ObterLista().OrderBy(i => i.Nome);

            foreach (var item in categorias)
                _categoriaCollection.Add(item);

            PropriedadeAlterada(nameof(CategoriaCollection));
        }
        public void DefinirTipoOperacao(eTipoOperacao tipoOperacao)
        {
            _tipoOperacao = tipoOperacao;

            _habilitarEdicao = (tipoOperacao == eTipoOperacao.Adicionar || tipoOperacao == eTipoOperacao.Editar || tipoOperacao == eTipoOperacao.Excluir) ? eHabilitarEdicao.Sim: eHabilitarEdicao.Nao;
            
            AtualizarPropriedades();
        }

        public void Salvar()
        {
            if (_tipoOperacao == eTipoOperacao.Adicionar)
            {
                _categoriaSelecionada = new Categoria { PK_Categoria = 0, Nome = _nome, FK_Usuario = null };
                var ret = _categoriaRepository.Adicionar(_categoriaSelecionada);
            }
            else if (_tipoOperacao == eTipoOperacao.Editar)
            {
                _categoriaSelecionada.Nome = _nome;
                var ret = _categoriaRepository.Atualizar(_categoriaSelecionada);
            }
            else if (_tipoOperacao == eTipoOperacao.Excluir)
            {
                var ret = _categoriaRepository.Deletar(_categoriaSelecionada.PK_Categoria);
            }

            CarregarCategorias();
            DefinirCategoriaSelecionada(null);
            DefinirTipoOperacao(eTipoOperacao.Salvar);
        }

        public void DefinirCategoriaSelecionada(Categoria categoria)
        {
            _categoriaSelecionada = categoria == null ? new Categoria() : categoria;

            _nome = _categoriaSelecionada.Nome;

            AtualizarPropriedades();
        }
        private void AtualizarPropriedades()
        {
            PropriedadeAlterada(nameof(ExibeBotoesCrud));
            PropriedadeAlterada(nameof(ExibeBotoesConfirmacao));
            PropriedadeAlterada(nameof(HabilitaBotaoEditarExcluir));
            PropriedadeAlterada(nameof(Nome));
        }
    }
}
