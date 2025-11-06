using CF.Domain.Dto;
using CF.Domain.Entidades;
using CF.Domain.Enumeradores;
using CF.Domain.Interfaces.Repository;
using CF.Domain.Interfaces.ViewModel;
using CF.InfraData.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Input;

namespace CF.ViewModel.ViewModel
{
    public class EntidadeViewModel : ViewModelBase, IEntidadeViewModel
    {
        private readonly IOperacaoFinanceiraRepository _operacaoFinanceiraRepository;
        private readonly IEntidadeFinanceiraRepository _entidadeFinanceiraRepository;
        private eHabilitarEdicao _habilitarEdicao;
        private eTipoOperacaoCrud _tipoOperacao;

        public EntidadeViewModel()
        {
            _operacaoFinanceiraRepository = Bootstrap.ServiceProvider.GetRequiredService<IOperacaoFinanceiraRepository>();
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
        public bool HabilitarNome { get => !ExibeBotoesCrud && _tipoOperacao != eTipoOperacaoCrud.Excluir; }

        // CONTROLAR EXIBIÇÃO DE COMPONENTES
        public bool ExibeBotoesCrud { get => this._habilitarEdicao == eHabilitarEdicao.Nao; }
        public bool ExibeBotoesConfirmacao { get => !ExibeBotoesCrud; }
        public bool HabilitaBotaoEditarExcluir { get => _entidadeFinanceiraCollection.Count > 0 && _entidadeFinanceiraSelecionada != null && _entidadeFinanceiraSelecionada.PK_EntidadeFinanceira > 0; }

        private int _indice = -1;
        public int SelecionarIndice { get => _indice; }
        public string Total { get => ("Entidades financeiras: " + EntidadeFinanceiraCollection.Count.ToString("N0")); }
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
        public void DefinirTipoOperacao(eTipoOperacaoCrud tipoOperacao)
        {
            _tipoOperacao = tipoOperacao;
            _habilitarEdicao = (tipoOperacao == eTipoOperacaoCrud.Adicionar || tipoOperacao == eTipoOperacaoCrud.Editar) ? eHabilitarEdicao.Sim: eHabilitarEdicao.Nao;

            if (tipoOperacao != eTipoOperacaoCrud.Adicionar)
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
            PropriedadeAlterada(nameof(Total));
            PropriedadeAlterada(nameof(HabilitarNome));
        }
        public ValidacaoResultado Salvar()
        {
            int ret = -1;
            var resultado = new ValidacaoResultado()
            {
                Sucesso = true,
                Erros = new List<string>()
            };

            try
            {
                if (_tipoOperacao == eTipoOperacaoCrud.Adicionar)
                {
                    _entidadeFinanceiraSelecionada = new EntidadeFinanceira
                    {
                        PK_EntidadeFinanceira = 0,
                        Nome = _nome,
                        FK_Usuario = null
                    };
                    ret = _entidadeFinanceiraRepository.Adicionar(_entidadeFinanceiraSelecionada);

                    if (ret <= 0)
                    {
                        resultado.Sucesso = false;
                        resultado.Erros.Add("Não foi possível adicionar a entidade financeira. Verifique os dados e tente novamente.");
                    }
                }
                else if (_tipoOperacao == eTipoOperacaoCrud.Editar)
                {
                    _entidadeFinanceiraSelecionada.Nome = _nome;
                    ret = _entidadeFinanceiraRepository.Atualizar(_entidadeFinanceiraSelecionada);

                    if (ret <= 0)
                    {
                        resultado.Sucesso = false;
                        resultado.Erros.Add("Não foi possível atualizar a entidade financeira. Verifique os dados e tente novamente.");
                    }
                }
                else if (_tipoOperacao == eTipoOperacaoCrud.Excluir)
                {
                    var operacoesFinanceiras = _operacaoFinanceiraRepository.ObterLista(
                        "OperacaoFinanceira.FK_EntidadeFinanceira = @PK_EntidadeFinanceira",
                        new { PK_EntidadeFinanceira = _entidadeFinanceiraSelecionada.PK_EntidadeFinanceira });

                    if (operacoesFinanceiras == null || !operacoesFinanceiras.Any())
                    {
                        ret = _entidadeFinanceiraRepository.Deletar(_entidadeFinanceiraSelecionada.PK_EntidadeFinanceira);

                        if (ret <= 0)
                        {
                            resultado.Sucesso = false;
                            resultado.Erros.Add("Não foi possível excluir a entidade financeira. Tente novamente.");
                        }
                    }
                    else
                    {
                        resultado.Sucesso = false;
                        resultado.Erros.Add($"Não é possível excluir a entidade financeira '{_entidadeFinanceiraSelecionada.Nome}' pois ela está vinculada a {operacoesFinanceiras.Count()} operação(ões) financeira(s).\n");
                        resultado.Erros.Add("Remova ou altere as operações financeiras vinculadas antes de excluir esta categoria.");
                    }
                }

                if (resultado.Sucesso)
                {
                    CarregarColecoes();
                    DefinirItemSelecionado(null);
                    DefinirTipoOperacao(eTipoOperacaoCrud.Salvar);
                }
            }
            catch (Exception ex)
            {
                resultado.Sucesso = false;
                resultado.Erros.Add("Ocorreu um erro inesperado ao processar a operação.");
                resultado.Erros.Add($"Detalhes: {ex.Message}");
            }

            return resultado;
        }
    }
}
