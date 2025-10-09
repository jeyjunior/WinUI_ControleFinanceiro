using CF.Domain.Entidades;
using CF.Domain.Enumeradores;
using CF.Domain.Interfaces.Repository;
using CF.Domain.Interfaces.ViewModel;
using CF.InfraData.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Input;

namespace CF.ViewModel.ViewModel
{
    public class OperacaoFinanceiraViewModel : ViewModelBase, IOperacaoFinanceiraViewModel
    {
        private readonly IOperacaoFinanceiraRepository _operacaoFinanceiraRepository;
        private readonly IEntidadeFinanceiraRepository _entidadeFinanceiraRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly ITipoOperacaoRepository _tipoOperacaoRepository;
        private readonly IStatusPagamentoRepository _statusPagamentoRepository;

        private eHabilitarEdicao _habilitarEdicao;
        private eTipoOperacao _tipoOperacao;
        public OperacaoFinanceiraViewModel()
        {
            _operacaoFinanceiraRepository = Bootstrap.ServiceProvider.GetRequiredService<IOperacaoFinanceiraRepository>();
            _entidadeFinanceiraRepository = Bootstrap.ServiceProvider.GetRequiredService<IEntidadeFinanceiraRepository>();
            _categoriaRepository = Bootstrap.ServiceProvider.GetRequiredService<ICategoriaRepository>();
            _tipoOperacaoRepository = Bootstrap.ServiceProvider.GetRequiredService<ITipoOperacaoRepository>();
            _statusPagamentoRepository = Bootstrap.ServiceProvider.GetRequiredService<IStatusPagamentoRepository>();
        }

        #region OperacaoFinanceira
        private ObservableCollection<OperacaoFinanceiraGrid> _operacaoFinanceiraCollection = new ObservableCollection<OperacaoFinanceiraGrid>();
        public ObservableCollection<OperacaoFinanceiraGrid> OperacaoFinanceiraCollection { get => _operacaoFinanceiraCollection; }
        
        private OperacaoFinanceiraGrid _operacaoFinanceiraSelecionada = new OperacaoFinanceiraGrid();
        public OperacaoFinanceiraGrid OperacaoFinanceiraSelecionada { get => _operacaoFinanceiraSelecionada; }
        #endregion

        #region TipoOperacao
        public ObservableCollection<TipoOperacao> _tipoOperacaoCollection = new ObservableCollection<TipoOperacao>();
        public ObservableCollection<TipoOperacao> TipoOperacaoCollection { get => _tipoOperacaoCollection; }
        private int _pK_TipoOperacaoSelecionada = 0;
        public int PK_TipoOperacaoSelecionada { get => _pK_TipoOperacaoSelecionada; set => _pK_TipoOperacaoSelecionada = value; }
        #endregion

        #region Categoria
        public ObservableCollection<Categoria> _categoriaCollection = new ObservableCollection<Categoria>();
        public ObservableCollection<Categoria> CategoriaCollection { get => _categoriaCollection; }

        private int _pK_CategoriaSelecionada = 0;
        public int PK_CategoriaSelecionada { get => _pK_CategoriaSelecionada; set => _pK_CategoriaSelecionada = value; }

        #endregion

        #region EntidadeFinanceira
        public ObservableCollection<EntidadeFinanceira> _entidadeFinanceiraCollection = new ObservableCollection<EntidadeFinanceira>();
        public ObservableCollection<EntidadeFinanceira> EntidadeFinanceiraCollection { get => _entidadeFinanceiraCollection; }
        private int _pK_EntidadeFinanceiraSelecionada = 0;
        public int PK_EntidadeFinanceiraSelecionada { get => _pK_EntidadeFinanceiraSelecionada; set => _pK_EntidadeFinanceiraSelecionada = value; }
        #endregion

        #region StatusPagamento
        public ObservableCollection<StatusPagamento> _statusPagamentoCollection = new ObservableCollection<StatusPagamento>();
        public ObservableCollection<StatusPagamento> StatusPagamentoCollection { get => _statusPagamentoCollection; }
        private int _pK_StatusPagamentoSelecionada = 0;
        public int PK_StatusPagamentoSelecionada { get => _pK_StatusPagamentoSelecionada; set => _pK_StatusPagamentoSelecionada = value; }
        #endregion

        #region Valor
        // PROPRIEDADES OBJETO PRINCIPAL
        private decimal _valor = 0;
        public decimal Valor 
        { 
            get => _valor;
            set => _valor = value;
        }
        #endregion

        #region DataVencimento
        private DateTimeOffset? _dataVencimento = null;
        public DateTimeOffset? DataVencimento
        {
            get => _dataVencimento;
            set => _dataVencimento = value;
        }
        public string DataVencimentoFormatada
        {
            get
            {
                if (_dataVencimento == null)
                    return "";

                return _dataVencimento.Value.Date.ToShortDateString();
            }
            set
            {
                _dataVencimento = (value == null || value == "") ? DateTime.Now : Convert.ToDateTime(value);
            }
        }
        #endregion
        #region DataOperacao
        private DateTimeOffset? _dataOperacao = null;
        public DateTimeOffset? DataOperacao
        {
            get => _dataOperacao;
            set => _dataOperacao = value;
        }

        public string DataOperacaoFormatada
        {
            get
            {
                if (_dataOperacao == null)
                    return "";

                return _dataOperacao.Value.Date.ToShortDateString();
            }
            set
            {
                _dataOperacao = (value == null || value == "") ? DateTime.Now : Convert.ToDateTime(value);
            }
        }
        #endregion

        #region Anotacao
        // PROPRIEDADES OBJETO PRINCIPAL
        private string _anotacao = "";
        public string Anotacao
        {
            get => _anotacao;
            set => _anotacao = value;
        }
        #endregion

        // CONTROLAR EXIBIÇÃO DE COMPONENTES
        public bool ExibeBotoesCrud { get => this._habilitarEdicao == eHabilitarEdicao.Nao; }
        public bool ExibeBotoesConfirmacao { get => !ExibeBotoesCrud; }
        public bool HabilitaBotaoEditarExcluir { get => _operacaoFinanceiraCollection.Count > 0 && _operacaoFinanceiraSelecionada != null && _operacaoFinanceiraSelecionada.PK_OperacaoFinanceira > 0; }

        private int _indice = -1;
        public int SelecionarIndice { get => _indice; }

        // METÓDOS
        public void CarregarColecoes()
        {
            CarregarColecaoOperacaoFinanceira();
            CarregarColecaoCategoria();
            CarregarColecaoEntidadeFinanceira();
            CarregarColecaoTipoOperacao();
            CarregarColecaoStatusPagamento();
        }

        private void CarregarColecaoOperacaoFinanceira()
        {
            if (_operacaoFinanceiraCollection == null)
                _operacaoFinanceiraCollection = new ObservableCollection<OperacaoFinanceiraGrid>();

            if (_operacaoFinanceiraCollection.Any())
                _operacaoFinanceiraCollection.Clear();

            var operacaoFinanceira = _operacaoFinanceiraRepository.ObterListaGrid();

            foreach (var item in operacaoFinanceira)
                _operacaoFinanceiraCollection.Add(item);

            PropriedadeAlterada(nameof(OperacaoFinanceiraCollection));
        }
        private void CarregarColecaoCategoria()
        {
            if (_categoriaCollection == null)
                _categoriaCollection = new ObservableCollection<Categoria>();

            if (_categoriaCollection.Any())
                _categoriaCollection.Clear();

            var categoria = _categoriaRepository.ObterLista().OrderBy(i => i.Nome);

            foreach (var item in categoria)
                _categoriaCollection.Add(item);

            PropriedadeAlterada(nameof(CategoriaCollection));
        }
        private void CarregarColecaoEntidadeFinanceira()
        {
            if (_entidadeFinanceiraCollection == null)
                _entidadeFinanceiraCollection = new ObservableCollection<EntidadeFinanceira>();

            if (_entidadeFinanceiraCollection.Any())
                _entidadeFinanceiraCollection.Clear();

            var entidadeFinanceira = _entidadeFinanceiraRepository.ObterLista().OrderBy(i => i.Nome);

            foreach (var item in entidadeFinanceira)
                _entidadeFinanceiraCollection.Add(item);

            PropriedadeAlterada(nameof(CategoriaCollection));
        }
        private void CarregarColecaoTipoOperacao()
        {
            if (_tipoOperacaoCollection == null)
                _tipoOperacaoCollection = new ObservableCollection<TipoOperacao>();

            if (_tipoOperacaoCollection.Any())
                _tipoOperacaoCollection.Clear();

            var tipoOperacao = _tipoOperacaoRepository.ObterLista().OrderBy(i => i.Nome);

            foreach (var item in tipoOperacao)
                _tipoOperacaoCollection.Add(item);

            PropriedadeAlterada(nameof(TipoOperacaoCollection));
        }
        private void CarregarColecaoStatusPagamento()
        {
            if (_statusPagamentoCollection == null)
                _statusPagamentoCollection = new ObservableCollection<StatusPagamento>();

            if (_statusPagamentoCollection.Any())
                _statusPagamentoCollection.Clear();

            var statusPagamento = _statusPagamentoRepository.ObterLista().OrderBy(i => i.Nome);

            foreach (var item in statusPagamento)
                _statusPagamentoCollection.Add(item);

            PropriedadeAlterada(nameof(StatusPagamentoCollection));
        }
        public void DefinirTipoOperacao(eTipoOperacao tipoOperacao)
        {
            _tipoOperacao = tipoOperacao;
            _habilitarEdicao = (tipoOperacao == eTipoOperacao.Adicionar || tipoOperacao == eTipoOperacao.Editar || tipoOperacao == eTipoOperacao.Excluir) ? eHabilitarEdicao.Sim: eHabilitarEdicao.Nao;

            //if (tipoOperacao != eTipoOperacao.Adicionar)
            //{
            //    //_nome = _operacaoFinanceiraSelecionada != null ? _operacaoFinanceiraSelecionada.Nome : "";
            //}
            //else
            //{
            //    _nome = "";
            //}

            AtualizarPropriedades();
        }
        public void DefinirItemSelecionado(OperacaoFinanceiraGrid operacaoFinanceiraGrid)
        {
            _operacaoFinanceiraSelecionada = operacaoFinanceiraGrid == null ? 
                new OperacaoFinanceiraGrid 
                { 
                    PK_OperacaoFinanceira = 0, 
                    FK_TipoOperacao = 0,
                    FK_Categoria = 0,
                    FK_EntidadeFinanceira = 0,
                    FK_StatusPagamento = 0
                } : 
                operacaoFinanceiraGrid;
            
            _pK_TipoOperacaoSelecionada = _operacaoFinanceiraSelecionada.FK_TipoOperacao;
            _pK_CategoriaSelecionada = _operacaoFinanceiraSelecionada.FK_Categoria;
            _pK_EntidadeFinanceiraSelecionada = _operacaoFinanceiraSelecionada.FK_EntidadeFinanceira;
            _pK_StatusPagamentoSelecionada = _operacaoFinanceiraSelecionada.FK_StatusPagamento;
            _valor = _operacaoFinanceiraSelecionada.Valor;
            _dataVencimento = _operacaoFinanceiraSelecionada.DataVencimento;
            _dataOperacao = _operacaoFinanceiraSelecionada.DataTransacao;
            _anotacao = _operacaoFinanceiraSelecionada.Anotacao;

            AtualizarPropriedades();
        }
        private void AtualizarPropriedades()
        {
            PropriedadeAlterada(nameof(ExibeBotoesCrud));
            PropriedadeAlterada(nameof(ExibeBotoesConfirmacao));
            PropriedadeAlterada(nameof(HabilitaBotaoEditarExcluir));

            if (_operacaoFinanceiraCollection.Count > 0 && (_operacaoFinanceiraSelecionada == null || _operacaoFinanceiraSelecionada.PK_OperacaoFinanceira <= 0))
                _indice = 0;

            PropriedadeAlterada(nameof(SelecionarIndice));
            PropriedadeAlterada(nameof(PK_TipoOperacaoSelecionada));
            PropriedadeAlterada(nameof(PK_CategoriaSelecionada));
            PropriedadeAlterada(nameof(PK_EntidadeFinanceiraSelecionada));
            PropriedadeAlterada(nameof(PK_StatusPagamentoSelecionada));
            PropriedadeAlterada(nameof(Valor));
            PropriedadeAlterada(nameof(DataVencimento));
            PropriedadeAlterada(nameof(DataVencimentoFormatada));
            PropriedadeAlterada(nameof(DataOperacao));
            PropriedadeAlterada(nameof(DataOperacaoFormatada));
            PropriedadeAlterada(nameof(Anotacao));
        }
        public void Salvar()
        {
            //if (_tipoOperacao == eTipoOperacao.Adicionar)
            //{
            //    _operacaoFinanceiraSelecionada = new EntidadeFinanceira { PK_EntidadeFinanceira = 0, Nome = _nome, FK_Usuario = null };
            //    var ret = _entidadeFinanceiraRepository.Adicionar(_operacaoFinanceiraSelecionada);
            //}
            //else if (_tipoOperacao == eTipoOperacao.Editar)
            //{
            //    _entidadeFinanceiraSelecionada.Nome = _nome;
            //    var ret = _entidadeFinanceiraRepository.Atualizar(_entidadeFinanceiraSelecionada);
            //}
            //else if (_tipoOperacao == eTipoOperacao.Excluir)
            //{
            //    var ret = _entidadeFinanceiraRepository.Deletar(_entidadeFinanceiraSelecionada.PK_EntidadeFinanceira);
            //}

            CarregarColecoes();
            DefinirItemSelecionado(null);
            DefinirTipoOperacao(eTipoOperacao.Salvar);
        }
    }

    public class NullableDateTimeOffsetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value is DateTimeOffset dateTime ? dateTime : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTimeOffset dateTime)
            {
                return dateTime;
            }
            return null;
        }
    }
}
