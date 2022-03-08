USE [ERPSOFT]
GO

-- =============================================
CREATE PROCEDURE [ProcessHistory].[GetPQCMesDataRealtime](
	@Line VARCHAR(10),
	@InspectStart DATETIME,
	@InspectEnd DATETIME
)

AS
BEGIN
	SELECT 
		__attrs.Date,
		__attrs.Hour,
		__attrs.Line,
		__attrs.Product,
		ISNULL(__attrs.Passed,0) AS Passed,
		ISNULL(__attrs.NotPassed,0) AS NotPassed
	FROM 
	(	
	SELECT 
		CONVERT(DATE,InspectDateTime,101 ) AS 'Date',
		DATEPART(HOUR, InspectDateTime) AS 'Hour',
		Line,
		Model as Product,
		CASE  
		WHEN AttributeType = 'OP'  THEN 'Passed'
		WHEN AttributeType = 'NG' THEN 'NotPassed'
		WHEN AttributeType = 'RW' THEN 'NotPassed'
		END AS Result,
		ISNULL(SUM(Quantity),0) AS Quantity
	FROM ProcessHistory.PQCMesData
	WHERE InspectDateTime >= @InspectStart
		AND InspectDateTime <= @InspectEnd
		AND Line = @Line
	GROUP BY 
		Line, 
		Model, 
		CONVERT(DATE,InspectDateTime,101 ),DATEPART(HOUR, InspectDateTime),
		AttributeType
	) AS __piv
	PIVOT
	(
		SUM(__piv.Quantity)
		FOR __piv.Result IN (
			Passed,
			NotPassed
		)
	) AS __attrs
	ORDER BY __attrs.Date, __attrs.Hour, __attrs.Product
END
