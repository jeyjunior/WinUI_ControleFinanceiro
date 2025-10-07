-- =============================================
-- SCRIPT DE CRIAÇÃO DAS TABELAS
-- Controle Financeiro - SQL Server
-- =============================================

-- Tabela de Usuários
CREATE TABLE Usuario (
    PK_Usuario INT IDENTITY(1,1) PRIMARY KEY,
    Email NVARCHAR(255) NOT NULL,
    Nome NVARCHAR(255) NOT NULL,
    LoginApi NVARCHAR(2000) NULL,
    DataCadastro DATETIME NOT NULL DEFAULT GETDATE(),
    Ativo BIT NOT NULL DEFAULT 1
);

-- Tabela de Categorias
CREATE TABLE Categoria (
    PK_Categoria INT IDENTITY(1,1) PRIMARY KEY,
    Nome NVARCHAR(100) NOT NULL,
    FK_Usuario INT NULL,
    CONSTRAINT FK_Categoria_Usuario FOREIGN KEY (FK_Usuario) REFERENCES Usuario(PK_Usuario)
);

-- Tabela de Entidades Financeiras
CREATE TABLE EntidadeFinanceira (
    PK_EntidadeFinanceira BIGINT IDENTITY(1,1) PRIMARY KEY,
    Nome NVARCHAR(100) NOT NULL,
    FK_Usuario INT NULL,
    CONSTRAINT FK_EntidadeFinanceira_Usuario FOREIGN KEY (FK_Usuario) REFERENCES Usuario(PK_Usuario)
);

-- Tabela de Tipos de Operação (Entrada/Saída)
CREATE TABLE TipoOperacaoFinanceira (
    PK_TipoOperacaoFinanceira INT IDENTITY(1,1) PRIMARY KEY,
    Nome NVARCHAR(50) NOT NULL
);

-- Tabela de Status de Pagamento
CREATE TABLE StatusPagamento (
    PK_StatusPagamento INT IDENTITY(1,1) PRIMARY KEY,
    Nome NVARCHAR(20) NOT NULL
);

-- Tabela Principal de Operações Financeiras
CREATE TABLE OperacaoFinanceira (
    PK_OperacaoFinanceira BIGINT IDENTITY(1,1) PRIMARY KEY,
    FK_TipoOperacaoFinanceira INT NOT NULL,
    FK_EntidadeFinanceira BIGINT NOT NULL,
    FK_Categoria INT NOT NULL,
    FK_StatusPagamento INT NOT NULL,
    Valor DECIMAL(18,2) NOT NULL,
    DataOperacao DATETIME NULL,
    DataVencimento DATETIME NOT NULL,
    Descricao NVARCHAR(200) NULL,
    FK_Usuario INT NULL,
    
    CONSTRAINT FK_Operacao_TipoOperacao FOREIGN KEY (FK_TipoOperacaoFinanceira) REFERENCES TipoOperacaoFinanceira(PK_TipoOperacaoFinanceira),
    CONSTRAINT FK_Operacao_EntidadeFinanceira FOREIGN KEY (FK_EntidadeFinanceira) REFERENCES EntidadeFinanceira(PK_EntidadeFinanceira),
    CONSTRAINT FK_Operacao_Categoria FOREIGN KEY (FK_Categoria) REFERENCES Categoria(PK_Categoria),
    CONSTRAINT FK_Operacao_StatusPagamento FOREIGN KEY (FK_StatusPagamento) REFERENCES StatusPagamento(PK_StatusPagamento),
    CONSTRAINT FK_Operacao_Usuario FOREIGN KEY (FK_Usuario) REFERENCES Usuario(PK_Usuario)
);

-- Índices para melhor performance
CREATE INDEX IX_OperacaoFinanceira_DataVencimento ON OperacaoFinanceira(DataVencimento);
CREATE INDEX IX_OperacaoFinanceira_Status ON OperacaoFinanceira(FK_StatusPagamento);
CREATE INDEX IX_OperacaoFinanceira_Tipo ON OperacaoFinanceira(FK_TipoOperacaoFinanceira);
CREATE INDEX IX_Categoria_Usuario ON Categoria(FK_Usuario);
CREATE INDEX IX_EntidadeFinanceira_Usuario ON EntidadeFinanceira(FK_Usuario);