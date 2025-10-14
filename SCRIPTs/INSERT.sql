-- =============================================
-- SCRIPT DE INSERTS PARA DADOS DE TESTE
-- =============================================

INSERT INTO Usuario (Email, Nome, LoginApi, DataCadastro, Ativo)
VALUES ('usuario.teste@gmail.com', 'Usuário Teste', 'google_oauth_data', GETDATE(), 1);


INSERT INTO TipoOperacaoFinanceira (Nome) VALUES 
('Entrada'),
('Saída');

-- Inserir Status de Pagamento
INSERT INTO StatusPagamento (Nome) VALUES 
('Pago'),
('Em Aberto'),
('Vencido');

-- Inserir Categorias
INSERT INTO Categoria (Nome, FK_Usuario) VALUES 
('Salário', NULL),
('Freelance', NULL),
('Aluguel', NULL),
('Condominio', NULL),
('Alimentação', NULL),
('Transporte', NULL),
('Lazer', NULL),
('Saúde', NULL),
('Educação', NULL),
('Investimentos',NULL);

-- Inserir Entidades Financeiras
INSERT INTO EntidadeFinanceira (Nome, FK_Usuario) VALUES 
('Empresa XYZ', NULL),
('Imobiliária ABC', NULL),
('Condomínio Edifício Central', NULL),
('Supermercado Preço Bom', NULL),
('Posto de Gasolina Shell', NULL),
('Uber/Táxi', NULL),
('Restaurante Sabor Caseiro', NULL),
('Academia Fit', NULL),
('Hospital Saúde Total', NULL),
('Faculdade USP', NULL),
('Senhor Zé - Serviços Gerais', NULL);

-- INSERINDO OPERAÇÕES FINANCEIRAS (OUTUBRO/2025)
INSERT INTO OperacaoFinanceira 
    (FK_TipoOperacao, FK_EntidadeFinanceira, FK_Categoria, Valor, DataVencimento, DataTransacao, Anotacao, FK_Usuario)
VALUES
-- ENTRADAS
(1, 1, 1, 10.99, GETDATE(), NULL, 'Salário recebido', NULL),
(1, 2, 2, 20.99,      GETDATE(), GETDATE(), 'Aluguel recebido', NULL),
(1, 3, 3, 30.00,   GETDATE(), NULL, 'Freelance projeto X', NULL),
(1, 4, 4, 40.50,       GETDATE(), GETDATE(), 'Reembolso condomínio', NULL),

-- SAÍDAS
(2, 5, 5, 5.99, GETDATE(), NULL, 'Compra supermercado', NULL),
(2, 6, 6, 6.00,       GETDATE(), NULL, 'Combustível / transporte', NULL),
(2, 7, 7, 7.75,      GETDATE(), GETDATE(), 'Restaurante / lazer', NULL),
(2, 8, 8, 8.00,       GETDATE(), NULL, 'Academia mensalidade', NULL),
(2, 9, 9, 9.99,  GETDATE(), GETDATE(), 'Exames hospitalares', NULL);

