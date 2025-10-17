-- PERÍODO DA PESQUISA
SET DATEFORMAT  DMY

DECLARE @DataInicial DATETIME	= DATEADD(D, -14, GETDATE()),
		@DataFinal DATETIME		= DATEADD(D, 18, GETDATE())
	
-- DESPESA
--SELECT	
--		SUM (CASE WHEN DataTransacao IS NOT NULL THEN Valor ELSE 0 END) AS TotalDespesaPaga,
--		SUM (Valor) AS TotalDespesa
--FROM	OperacaoFinanceira
--WHERE	(DataVencimento > @DataInicial	AND DataVencimento < @DataFinal)
--		AND
--		FK_TipoOperacao = 2

-- DESPESA
SELECT	*
FROM	OperacaoFinanceira
WHERE	(DataVencimento > @DataInicial	AND DataVencimento < @DataFinal)
ORDER	BY	
		FK_TipoOperacao,
		DataTransacao

	
SELECT	Operacao.TotalReceitaPaga,
		Operacao.TotalReceita,
		Operacao.TotalDespesaPaga,
		Operacao.TotalDespesa,
		(Operacao.TotalReceita - Operacao.TotalDespesa) AS Saldo
FROM	
(
	SELECT	SUM (CASE WHEN DataTransacao IS NOT NULL AND FK_TipoOperacao = 2 THEN Valor ELSE 0 END) AS TotalDespesaPaga,
			SUM (CASE WHEN FK_TipoOperacao = 2 THEN Valor ELSE 0 END) AS TotalDespesa,
			SUM (CASE WHEN DataTransacao IS NOT NULL AND FK_TipoOperacao = 1 THEN Valor ELSE 0 END) AS TotalReceitaPaga,
			SUM (CASE WHEN FK_TipoOperacao = 1 THEN Valor ELSE 0 END) AS TotalReceita
	FROM	OperacaoFinanceira
	WHERE	(DataVencimento > @DataInicial	AND DataVencimento < @DataFinal)
)	AS Operacao





 SELECT    Operacao.TotalReceitaPaga,
           Operacao.TotalReceita,
           Operacao.TotalDespesaPaga,
           Operacao.TotalDespesa,
           (Operacao.TotalReceita - Operacao.TotalDespesa) AS Saldo
 FROM 
 (
       SELECT  SUM (CASE WHEN DataTransacao IS NOT NULL AND FK_TipoOperacao = 2 THEN Valor ELSE 0 END) AS TotalDespesaPaga,
               SUM (CASE WHEN FK_TipoOperacao = 2 THEN Valor ELSE 0 END) AS TotalDespesa,
               SUM (CASE WHEN DataTransacao IS NOT NULL AND FK_TipoOperacao = 1 THEN Valor ELSE 0 END) AS TotalReceitaPaga,
               SUM (CASE WHEN FK_TipoOperacao = 1 THEN Valor ELSE 0 END) AS TotalReceita
       FROM    OperacaoFinanceira
       WHERE   (OperacaoFinanceira.DataVencimento > @DataInicial AND OperacaoFinanceira.DataVencimento < @DataFinal)
 )     AS Operacao

 select len('Gestor Tecnologia')