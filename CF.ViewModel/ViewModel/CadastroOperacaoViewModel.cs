using CF.Domain.Entidades;
using CF.Domain.Enumeradores;
using CF.Domain.Interfaces.Repository;
using CF.Domain.Interfaces.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.ViewModel.ViewModel
{
    public class CadastroOperacaoViewModel : ViewModelBase, ICadastroOperacaoViewModel
    {
        private readonly IOperacaoFinanceiraRepository _operacaoFinanceiraRepository;
        private readonly IEntidadeFinanceiraRepository _entidadeFinanceiraRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly ITipoOperacaoRepository _tipoOperacaoRepository;

        private eHabilitarEdicao _habilitarEdicao;
        private eTipoOperacao _tipoOperacao;
        public CadastroOperacaoViewModel()
        {
            _operacaoFinanceiraRepository = Bootstrap.ServiceProvider.GetRequiredService<IOperacaoFinanceiraRepository>();
            _entidadeFinanceiraRepository = Bootstrap.ServiceProvider.GetRequiredService<IEntidadeFinanceiraRepository>();
            _categoriaRepository = Bootstrap.ServiceProvider.GetRequiredService<ICategoriaRepository>();
            _tipoOperacaoRepository = Bootstrap.ServiceProvider.GetRequiredService<ITipoOperacaoRepository>();
        }

        #region OperacaoFinanceira
        public OperacaoFinanceira OperacaoFinanceiraSelecionada { get; set; } = new OperacaoFinanceira();

        #endregion

        #region TipoOperacao
        public ObservableCollection<TipoOperacao> TipoOperacaoCollection { get; set; } = new ObservableCollection<TipoOperacao>();
        public int PK_TipoOperacaoSelecionada { get => OperacaoFinanceiraSelecionada.FK_TipoOperacao; set => OperacaoFinanceiraSelecionada.FK_TipoOperacao = value; }
        #endregion

        #region Categoria
        public ObservableCollection<Categoria> CategoriaCollection { get; set; } = new ObservableCollection<Categoria>();
        public int PK_CategoriaSelecionada { get => OperacaoFinanceiraSelecionada.FK_Categoria; set => OperacaoFinanceiraSelecionada.FK_Categoria = value; }

        #endregion

        #region EntidadeFinanceira
        public ObservableCollection<EntidadeFinanceira> EntidadeFinanceiraCollection { get; set; } = new ObservableCollection<EntidadeFinanceira>();
        public int PK_EntidadeFinanceiraSelecionada { get => OperacaoFinanceiraSelecionada.FK_EntidadeFinanceira; set => OperacaoFinanceiraSelecionada.FK_EntidadeFinanceira = value; }
        #endregion

        #region Valor
        public string Valor
        {
            get => OperacaoFinanceiraSelecionada.Valor.ToString("N2");
            set
            {
                OperacaoFinanceiraSelecionada.Valor = Convert.ToDecimal(value);
            }
        }
        #endregion

        #region DataVencimento
        public DateTimeOffset DataVencimento
        {
            get 
            {
                if (_dataVencimento == null || _dataVencimento == "")
                    return DateTime.Now;

                return Convert.ToDateTime(_dataVencimento);
            } 
            set
            {
                OperacaoFinanceiraSelecionada.DataVencimento = (value == null) ? DateTime.Now : Convert.ToDateTime(value.Date);
                PropriedadeAlterada(nameof(DataVencimentoFormatada));
            }
        }
        private string _dataVencimento = "";
        public string DataVencimentoFormatada
        {
            get
            {
                return OperacaoFinanceiraSelecionada.DataVencimento.ToShortDateString();
            }
            set
            {
                if (value == null || value == "")
                {
                    _dataVencimento = "";
                    OperacaoFinanceiraSelecionada.DataVencimento = DateTime.Now;
                }
                else
                {
                    OperacaoFinanceiraSelecionada.DataVencimento = Convert.ToDateTime(value);
                    _dataVencimento = OperacaoFinanceiraSelecionada.DataVencimento.ToShortDateString();
                }

                PropriedadeAlterada(nameof(DataVencimento));
            }
        }
        #endregion

        #region DataTransacao
        public DateTimeOffset? DataTransacao
        {
            get
            {
                if (_dataTransacao == null || _dataTransacao == "")
                    return DateTime.Now;

                return Convert.ToDateTime(_dataTransacao);
            }
            set
            {
                OperacaoFinanceiraSelecionada.DataTransacao = (value == null) ? null : Convert.ToDateTime(value.Value.Date);
                PropriedadeAlterada(nameof(DataTransacaoFormatada));
            }
        }
        private string _dataTransacao = "";
        public string DataTransacaoFormatada
        {
            get
            {
                if (OperacaoFinanceiraSelecionada.DataTransacao == null)
                    return "";

                return OperacaoFinanceiraSelecionada.DataTransacao.Value.ToShortDateString();
            }
            set
            {
                if (value == null || value == "")
                {
                    _dataTransacao = "";
                    OperacaoFinanceiraSelecionada.DataTransacao = null;
                }
                else
                {
                    OperacaoFinanceiraSelecionada.DataTransacao = Convert.ToDateTime(value);
                    _dataTransacao = OperacaoFinanceiraSelecionada.DataTransacao.Value.ToShortDateString();
                }

                PropriedadeAlterada(nameof(DataTransacao));
            }
        }
        #endregion

        #region Anotacao
        public string Anotacao
        {
            get => OperacaoFinanceiraSelecionada.Anotacao;
            set => OperacaoFinanceiraSelecionada.Anotacao = value;
        }
        #endregion

        public string TextoBotaoPrimario
        {
            get
            {
                if (_tipoOperacao == eTipoOperacao.Excluir)
                    return "Excluir";

                return "Salvar";
            }
        }

        // METÓDOS
        public void CarregarColecoes()
        {
            CarregarColecaoCategoria();
            CarregarColecaoEntidadeFinanceira();
            CarregarColecaoTipoOperacao();
        }
        private void CarregarColecaoCategoria()
        {
            if (CategoriaCollection == null)
                CategoriaCollection = new ObservableCollection<Categoria>();

            if (CategoriaCollection.Any())
                CategoriaCollection.Clear();

            var categoria = _categoriaRepository.ObterLista().OrderBy(i => i.Nome);

            foreach (var item in categoria)
                CategoriaCollection.Add(item);

            PropriedadeAlterada(nameof(CategoriaCollection));
        }
        private void CarregarColecaoEntidadeFinanceira()
        {
            if (EntidadeFinanceiraCollection == null)
                EntidadeFinanceiraCollection = new ObservableCollection<EntidadeFinanceira>();

            if (EntidadeFinanceiraCollection.Any())
                EntidadeFinanceiraCollection.Clear();

            var entidadeFinanceira = _entidadeFinanceiraRepository.ObterLista().OrderBy(i => i.Nome);

            foreach (var item in entidadeFinanceira)
                EntidadeFinanceiraCollection.Add(item);

            PropriedadeAlterada(nameof(CategoriaCollection));
        }
        private void CarregarColecaoTipoOperacao()
        {
            if (TipoOperacaoCollection == null)
                TipoOperacaoCollection = new ObservableCollection<TipoOperacao>();

            if (TipoOperacaoCollection.Any())
                TipoOperacaoCollection.Clear();

            var tipoOperacao = _tipoOperacaoRepository.ObterLista().OrderBy(i => i.Nome);

            foreach (var item in tipoOperacao)
                TipoOperacaoCollection.Add(item);

            PropriedadeAlterada(nameof(TipoOperacaoCollection));
        }

        public void DefinirOperacao(eTipoOperacao _tipoOperacao, int PK_OperacaoFinanceira)
        {
            this._tipoOperacao = _tipoOperacao;

            if (PK_OperacaoFinanceira <= 0)
            {
                OperacaoFinanceiraSelecionada = new OperacaoFinanceira
                {
                    PK_OperacaoFinanceira = 0,
                    FK_Categoria = 0,
                    FK_EntidadeFinanceira = 0,
                    FK_TipoOperacao = 0,
                    Anotacao = "",
                    DataTransacao = null,
                    DataVencimento = DateTime.Now,
                    Valor = 0,
                    FK_Usuario = null,
                    Categoria = null,
                    EntidadeFinanceira = null,
                    TipoOperacao = null
                };
            }
            else
            {
                OperacaoFinanceiraSelecionada = _operacaoFinanceiraRepository.Obter(PK_OperacaoFinanceira);
            }

            CarregarColecoes();
            AtualizarPropriedades();
        }
        private void AtualizarPropriedades()
        {
            PropriedadeAlterada(nameof(PK_TipoOperacaoSelecionada));
            PropriedadeAlterada(nameof(PK_CategoriaSelecionada));
            PropriedadeAlterada(nameof(PK_EntidadeFinanceiraSelecionada));
            PropriedadeAlterada(nameof(Valor));
            PropriedadeAlterada(nameof(DataVencimento));
            PropriedadeAlterada(nameof(DataVencimentoFormatada));
            PropriedadeAlterada(nameof(DataTransacao));
            PropriedadeAlterada(nameof(DataTransacaoFormatada));
            PropriedadeAlterada(nameof(Anotacao));
            PropriedadeAlterada(nameof(TextoBotaoPrimario));
        }
        public void Salvar()
        {
            if (_tipoOperacao == eTipoOperacao.Adicionar)
            {
                OperacaoFinanceiraSelecionada = new OperacaoFinanceira
                {
                    PK_OperacaoFinanceira = 0,
                    FK_Categoria = PK_CategoriaSelecionada,
                    FK_EntidadeFinanceira = PK_EntidadeFinanceiraSelecionada,
                    FK_TipoOperacao = PK_TipoOperacaoSelecionada,
                    Anotacao = Anotacao,
                    DataTransacao = DataTransacaoFormatada == "" ? null : Convert.ToDateTime(DataTransacaoFormatada),
                    DataVencimento = DataVencimento.Date,
                    Valor = Convert.ToDecimal(Valor),
                };

                var ret = _operacaoFinanceiraRepository.Adicionar(OperacaoFinanceiraSelecionada);
            }
            else if (_tipoOperacao == eTipoOperacao.Editar)
            {
                OperacaoFinanceiraSelecionada.FK_Categoria = PK_CategoriaSelecionada;
                OperacaoFinanceiraSelecionada.FK_EntidadeFinanceira = PK_EntidadeFinanceiraSelecionada;
                OperacaoFinanceiraSelecionada.FK_TipoOperacao = PK_TipoOperacaoSelecionada;
                OperacaoFinanceiraSelecionada.Anotacao = Anotacao;
                OperacaoFinanceiraSelecionada.DataTransacao = DataTransacaoFormatada == "" ? null : Convert.ToDateTime(DataTransacaoFormatada);
                OperacaoFinanceiraSelecionada.DataVencimento = DataVencimentoFormatada == "" ? DateTime.Now : Convert.ToDateTime(DataVencimentoFormatada);
                OperacaoFinanceiraSelecionada.Valor = Convert.ToDecimal(Valor);

                var ret = _operacaoFinanceiraRepository.Atualizar(OperacaoFinanceiraSelecionada);
            }
            else if (_tipoOperacao == eTipoOperacao.Excluir)
            {
                var ret = _operacaoFinanceiraRepository.Deletar(OperacaoFinanceiraSelecionada.PK_OperacaoFinanceira);
            }
        }
        public void Limpar()
        {
            OperacaoFinanceiraSelecionada = null;
        }
    }
}
