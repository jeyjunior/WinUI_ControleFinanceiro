-- =============================================
-- SCRIPT DE INSERTS PARA DADOS DE TESTE
-- =============================================

-- Inserir Usuário de teste
INSERT INTO Usuario (Email, Nome, LoginApi, DataCadastro, Ativo)
VALUES ('usuario.teste@gmail.com', 'Usuário Teste', 'google_oauth_data', GETDATE(), 1);

-- Inserir Tipos de Operação
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
('Salário', 1),
('Freelance', 1),
('Aluguel', 1),
('Condomínio', 1),
('Alimentação', 1),
('Transporte', 1),
('Lazer', 1),
('Saúde', 1),
('Educação', 1),
('Investimentos', 1);

-- Inserir Entidades Financeiras
INSERT INTO EntidadeFinanceira (Nome, FK_Usuario) VALUES 
('Empresa XYZ', 1),
('Imobiliária ABC', 1),
('Condomínio Edifício Central', 1),
('Supermercado Preço Bom', 1),
('Posto de Gasolina Shell', 1),
('Uber/Táxi', 1),
('Restaurante Sabor Caseiro', 1),
('Academia Fit', 1),
('Hospital Saúde Total', 1),
('Faculdade USP', 1),
('Senhor Zé - Serviços Gerais', 1);

-- Inserir Operações Financeiras de exemplo
INSERT INTO OperacaoFinanceira (
    FK_TipoOperacaoFinanceira, FK_EntidadeFinanceira, FK_Categoria, FK_StatusPagamento, 
    Valor, DataOperacao, DataVencimento, Descricao, FK_Usuario
) VALUES 
-- ENTRADAS
(1, 1, 1, 2, 2500.00, '2024-01-05', '2024-01-05', 'Salário mensal', 1), -- Pago
(1, 1, 2, 1, 500.00, NULL, '2024-01-20', 'Freelance desenvolvimento', 1), -- Em Aberto

-- SAÍDAS - PAGAS
(2, 2, 3, 2, 1000.00, '2024-01-10', '2024-01-10', 'Aluguel janeiro', 1),
(2, 3, 4, 2, 350.00, '2024-01-08', '2024-01-08', 'Condomínio janeiro', 1),
(2, 4, 5, 2, 450.00, '2024-01-12', '2024-01-12', 'Compras do mês', 1),
(2, 5, 6, 2, 200.00, '2024-01-15', '2024-01-15', 'Abastecimento carro', 1),
(2, 6, 6, 2, 80.00, '2024-01-18', '2024-01-18', 'Corridas de Uber', 1),

-- SAÍDAS - EM ABERTO (contas futuras)
(2, 2, 3, 1, 1000.00, NULL, '2024-02-10', 'Aluguel fevereiro', 1),
(2, 3, 4, 1, 350.00, NULL, '2024-02-08', 'Condomínio fevereiro', 1),
(2, 8, 8, 1, 120.00, NULL, '2024-02-05', 'Mensalidade academia', 1),
(2, 11, 7, 1, 300.00, NULL, '2024-02-15', 'Serviço de pintura', 1),

-- SAÍDAS - VENCIDAS (exemplo de atraso)
(2, 9, 8, 3, 150.00, NULL, '2023-12-20', 'Consulta médica', 1);

-- =============================================
-- CONSULTAS DE VERIFICAÇÃO
-- =============================================

-- Verificar totais por tipo
SELECT 
    tof.Nome as Tipo,
    COUNT(*) as Quantidade,
    SUM(o.Valor) as Total
FROM OperacaoFinanceira o
INNER JOIN TipoOperacaoFinanceira tof ON o.FK_TipoOperacaoFinanceira = tof.PK_TipoOperacaoFinanceira
GROUP BY tof.Nome;

-- Verificar operações por status
SELECT 
    sp.Nome as Status,
    COUNT(*) as Quantidade,
    SUM(o.Valor) as Total
FROM OperacaoFinanceira o
INNER JOIN StatusPagamento sp ON o.FK_StatusPagamento = sp.PK_StatusPagamento
GROUP BY sp.Nome;

-- Listar operações em aberto (próximas do vencimento)
SELECT 
    o.PK_OperacaoFinanceira,
    tof.Nome as Tipo,
    ef.Nome as Entidade,
    c.Nome as Categoria,
    o.Valor,
    o.DataVencimento,
    o.Descricao
FROM OperacaoFinanceira o
INNER JOIN TipoOperacaoFinanceira tof ON o.FK_TipoOperacaoFinanceira = tof.PK_TipoOperacaoFinanceira
INNER JOIN EntidadeFinanceira ef ON o.FK_EntidadeFinanceira = ef.PK_EntidadeFinanceira
INNER JOIN Categoria c ON o.FK_Categoria = c.PK_Categoria
INNER JOIN StatusPagamento sp ON o.FK_StatusPagamento = sp.PK_StatusPagamento
WHERE sp.Nome = 'Em Aberto'
AND o.DataVencimento >= GETDATE()
ORDER BY o.DataVencimento;