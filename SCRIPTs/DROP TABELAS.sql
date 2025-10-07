-- =============================================
-- SCRIPT DE DROP DAS TABELAS (Ordem reversa por FK)
-- =============================================

IF OBJECT_ID('OperacaoFinanceira', 'U') IS NOT NULL
    DROP TABLE OperacaoFinanceira;

IF OBJECT_ID('StatusPagamento', 'U') IS NOT NULL
    DROP TABLE StatusPagamento;

IF OBJECT_ID('TipoOperacaoFinanceira', 'U') IS NOT NULL
    DROP TABLE TipoOperacaoFinanceira;

IF OBJECT_ID('EntidadeFinanceira', 'U') IS NOT NULL
    DROP TABLE EntidadeFinanceira;

IF OBJECT_ID('Categoria', 'U') IS NOT NULL
    DROP TABLE Categoria;

IF OBJECT_ID('Usuario', 'U') IS NOT NULL
    DROP TABLE Usuario;