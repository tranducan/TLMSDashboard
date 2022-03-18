USE [ERPSOFT]
GO


-- =============================================
CREATE PROCEDURE [ProcessHistory].[GetPQCMesData]
(
	@InspectStart DATETIME,
	@InspectEnd DATETIME
)
AS
BEGIN
	SELECT 
		MIN(__attrs.StartTime) as StartTime,
		MAX(__attrs.EndTime) as EndTime,
		__attrs.Line,
		__attrs.Product,
		ISNULL(SUM(__attrs.OP),0) as OPQty,
		ISNULL(SUM(__attrs.NG),0) as NGQty,
		ISNULL(SUM(__attrs.RW),0) as RWQty
	FROM
	(

		SELECT 
				MIN(InspectDateTime) as StartTime,
				MAX(InspectDateTime) as EndTime,
				pd.Line,
				pd.Model as Product,
				AttributeType,
				SUM(Quantity) as Quantity
			FROM ProcessHistory.PQCMesData as pd
			LEFT JOIN ProcessHistory.DailyPerformanceGoal as dg
				ON pd.Model = dg.Model
			WHERE InspectDateTime >= @InspectStart
				AND InspectDateTime <= @InspectEnd
				AND dg.StartDate <= GETDATE()
				AND (dg.EndDate IS NULL OR dg.EndDate >= GETDATE())
			GROUP BY pd.Line, pd.Model, AttributeType
		) AS __piv
		PIVOT
		(
			SUM(__piv.Quantity)
			FOR __piv.AttributeType IN (
				OP,
				NG,
				RW
			)
		) AS __attrs
	GROUP BY Line, Product
END
