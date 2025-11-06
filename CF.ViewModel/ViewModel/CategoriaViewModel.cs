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
    public class CategoriaViewModel : ViewModelBase, ICategoriaViewModel
    {
        private readonly IOperacaoFinanceiraRepository _operacaoFinanceiraRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private eHabilitarEdicao _habilitarEdicao;
        private eTipoOperacaoCrud _tipoOperacao;
        public CategoriaViewModel()
        {
            _operacaoFinanceiraRepository = Bootstrap.ServiceProvider.GetRequiredService<IOperacaoFinanceiraRepository>();
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
        public bool HabilitarNome { get => !ExibeBotoesCrud && _tipoOperacao != eTipoOperacaoCrud.Excluir; }

        // CONTROLAR EXIBIÇÃO DE COMPONENTES
        public bool ExibeBotoesCrud { get => this._habilitarEdicao == eHabilitarEdicao.Nao; }
        public bool ExibeBotoesConfirmacao { get => !ExibeBotoesCrud; }
        public bool HabilitaBotaoEditarExcluir { get => _categoriaCollection.Count > 0 && _categoriaSelecionada != null && _categoriaSelecionada.PK_Categoria > 0; }
        private int _indice = -1;
        public int SelecionarIndice { get => _indice; }
        public string Total { get => ("Categorias: " + CategoriaCollection.Count.ToString("N0")); }

        // METÓDOS
        public void CarregarColecoes()
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
        public void DefinirTipoOperacao(eTipoOperacaoCrud tipoOperacao)
        {
            _tipoOperacao = tipoOperacao;
            _habilitarEdicao = (tipoOperacao == eTipoOperacaoCrud.Adicionar || tipoOperacao == eTipoOperacaoCrud.Editar) ? eHabilitarEdicao.Sim: eHabilitarEdicao.Nao;

            if (tipoOperacao != eTipoOperacaoCrud.Adicionar)
            {
                _nome = CategoriaSelecionada != null ? CategoriaSelecionada.Nome : "";
            }
            else
            {
                _nome = "";
            }

            AtualizarPropriedades();
        }
        public void DefinirItemSelecionado(Categoria categoria)
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

            if (CategoriaCollection.Count > 0 && (CategoriaSelecionada == null || CategoriaSelecionada.PK_Categoria <= 0))
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
                    _categoriaSelecionada = new Categoria
                    {
                        PK_Categoria = 0,
                        Nome = _nome,
                        FK_Usuario = null
                    };
                    ret = _categoriaRepository.Adicionar(_categoriaSelecionada);

                    if (ret <= 0)
                    {
                        resultado.Sucesso = false;
                        resultado.Erros.Add("Não foi possível adicionar a categoria. Verifique os dados e tente novamente.");
                    }
                }
                else if (_tipoOperacao == eTipoOperacaoCrud.Editar)
                {
                    _categoriaSelecionada.Nome = _nome;
                    ret = _categoriaRepository.Atualizar(_categoriaSelecionada);

                    if (ret <= 0)
                    {
                        resultado.Sucesso = false;
                        resultado.Erros.Add("Não foi possível atualizar a categoria. Verifique os dados e tente novamente.");
                    }
                }
                else if (_tipoOperacao == eTipoOperacaoCrud.Excluir)
                {
                    var operacoesFinanceiras = _operacaoFinanceiraRepository.ObterLista(
                        "OperacaoFinanceira.FK_Categoria = @PK_Categoria",
                        new { PK_Categoria = _categoriaSelecionada.PK_Categoria });

                    if (operacoesFinanceiras == null || !operacoesFinanceiras.Any())
                    {
                        ret = _categoriaRepository.Deletar(_categoriaSelecionada.PK_Categoria);

                        if (ret <= 0)
                        {
                            resultado.Sucesso = false;
                            resultado.Erros.Add("Não foi possível excluir a categoria. Tente novamente.");
                        }
                    }
                    else
                    {
                        resultado.Sucesso = false;
                        resultado.Erros.Add($"Não é possível excluir a categoria '{_categoriaSelecionada.Nome}' pois ela está vinculada a {operacoesFinanceiras.Count()} operação(ões) financeira(s).\n");
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
