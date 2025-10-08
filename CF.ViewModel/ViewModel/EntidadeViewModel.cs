using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using CF.Domain.Entidades;
using CF.Domain.Enumeradores;
using CF.Domain.Interfaces.Repository;
using CF.Domain.Interfaces.ViewModel;
using Windows.UI.Input;

namespace CF.ViewModel.ViewModel
{
    public class EntidadeViewModel : ViewModelBase, IEntidadeViewModel
    {
        private readonly IEntidadeFinanceiraRepository _entidadeFinanceiraRepository;
        private eHabilitarEdicao _habilitarEdicao;
        private eTipoOperacao _tipoOperacao;
        public EntidadeViewModel()
        {
            _entidadeFinanceiraRepository = Bootstrap.ServiceProvider.GetRequiredService<IEntidadeFinanceiraRepository>();
        }

        // COLEÇÕES
        private ObservableCollection<EntidadeFinanceira> _entidadeFinanceiraCollection = new ObservableCollection<EntidadeFinanceira>();
        public ObservableCollection<EntidadeFinanceira> EntidadeFinanceiraCollection { get => _entidadeFinanceiraCollection; }

        // ITEM SELECIONADO
        private EntidadeFinanceira _entidadeFinanceiraSelecionada = new EntidadeFinanceira();
        public EntidadeFinanceira EntidadeFinanceiraSelecionada { get => _entidadeFinanceiraSelecionada; }

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
        public bool HabilitaBotaoEditarExcluir { get => _entidadeFinanceiraCollection.Count > 0 && _entidadeFinanceiraSelecionada != null && _entidadeFinanceiraSelecionada.PK_EntidadeFinanceira > 0; }

        private int _indice = -1;
        public int SelecionarIndice { get => _indice; }

        // METÓDOS
        public void CarregarColecoes()
        {
            if (_entidadeFinanceiraCollection == null)
                _entidadeFinanceiraCollection = new ObservableCollection<EntidadeFinanceira>();

            if (_entidadeFinanceiraCollection.Any())
                _entidadeFinanceiraCollection.Clear();

            var categorias = _entidadeFinanceiraRepository.ObterLista().OrderBy(i => i.Nome);

            foreach (var item in categorias)
                _entidadeFinanceiraCollection.Add(item);

            PropriedadeAlterada(nameof(EntidadeFinanceiraCollection));
        }
        public void DefinirTipoOperacao(eTipoOperacao tipoOperacao)
        {
            _tipoOperacao = tipoOperacao;
            _habilitarEdicao = (tipoOperacao == eTipoOperacao.Adicionar || tipoOperacao == eTipoOperacao.Editar || tipoOperacao == eTipoOperacao.Excluir) ? eHabilitarEdicao.Sim: eHabilitarEdicao.Nao;

            if (tipoOperacao != eTipoOperacao.Adicionar)
            {
                _nome = _entidadeFinanceiraSelecionada != null ? _entidadeFinanceiraSelecionada.Nome : "";
            }
            else
            {
                _nome = "";
            }

            AtualizarPropriedades();
        }
        public void DefinirItemSelecionado(EntidadeFinanceira entidadeFinanceira)
        {
            _entidadeFinanceiraSelecionada = entidadeFinanceira == null ? new EntidadeFinanceira() { PK_EntidadeFinanceira = 0, Nome = "", FK_Usuario = null} : entidadeFinanceira;

            _nome = "";

            if (!string.IsNullOrWhiteSpace(_entidadeFinanceiraSelecionada.Nome))
                _nome = _entidadeFinanceiraSelecionada.Nome;

            AtualizarPropriedades();
        }
        private void AtualizarPropriedades()
        {
            PropriedadeAlterada(nameof(ExibeBotoesCrud));
            PropriedadeAlterada(nameof(ExibeBotoesConfirmacao));
            PropriedadeAlterada(nameof(HabilitaBotaoEditarExcluir));
            PropriedadeAlterada(nameof(Nome));

            if (_entidadeFinanceiraCollection.Count > 0 && (_entidadeFinanceiraSelecionada == null || _entidadeFinanceiraSelecionada.PK_EntidadeFinanceira <= 0))
                _indice = 0;

            PropriedadeAlterada(nameof(SelecionarIndice));
        }
        public void Salvar()
        {
            if (_tipoOperacao == eTipoOperacao.Adicionar)
            {
                _entidadeFinanceiraSelecionada = new EntidadeFinanceira { PK_EntidadeFinanceira = 0, Nome = _nome, FK_Usuario = null };
                var ret = _entidadeFinanceiraRepository.Adicionar(_entidadeFinanceiraSelecionada);
            }
            else if (_tipoOperacao == eTipoOperacao.Editar)
            {
                _entidadeFinanceiraSelecionada.Nome = _nome;
                var ret = _entidadeFinanceiraRepository.Atualizar(_entidadeFinanceiraSelecionada);
            }
            else if (_tipoOperacao == eTipoOperacao.Excluir)
            {
                var ret = _entidadeFinanceiraRepository.Deletar(_entidadeFinanceiraSelecionada.PK_EntidadeFinanceira);
            }

            CarregarColecoes();
            DefinirItemSelecionado(null);
            DefinirTipoOperacao(eTipoOperacao.Salvar);
        }
    }
}
