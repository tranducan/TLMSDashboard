USE [ERPSOFT]
GO

-- =============================================
ALTER PROCEDURE [ProcessHistory].[GetPQCMesDataLineSummary]
(
	@Line VARCHAR(10),
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
		ISNULL(SUM(__attrs.OUTPUT),0) as OUTPUT,
		ISNULL(SUM(__attrs.NG1),0) as NG1,
		ISNULL(SUM(__attrs.NG2),0) as NG2,
		ISNULL(SUM(__attrs.NG3),0) as NG3,
		ISNULL(SUM(__attrs.NG4),0) as NG4,
		ISNULL(SUM(__attrs.NG5),0) as NG5,
		ISNULL(SUM(__attrs.NG6),0) as NG6,
		ISNULL(SUM(__attrs.NG7),0) as NG7,
		ISNULL(SUM(__attrs.NG8),0) as NG8,
		ISNULL(SUM(__attrs.NG9),0) as NG9,
		ISNULL(SUM(__attrs.NG10),0) as NG10,
		ISNULL(SUM(__attrs.RW1),0) as RW1,
		ISNULL(SUM(__attrs.RW2),0) as RW2,
		ISNULL(SUM(__attrs.RW3),0) as RW3,
		ISNULL(SUM(__attrs.RW4),0) as RW4,
		ISNULL(SUM(__attrs.RW5),0) as RW5,
		ISNULL(SUM(__attrs.RW6),0) as RW6,
		ISNULL(SUM(__attrs.RW7),0) as RW7,
		ISNULL(SUM(__attrs.RW8),0) as RW8,
		ISNULL(SUM(__attrs.RW9),0) as RW9,
		ISNULL(SUM(__attrs.RW10),0) as RW10
	FROM
	(
		SELECT 
				MIN(InspectDateTime) as StartTime,
				MAX(InspectDateTime) as EndTime,
				pd.Line,
				pd.Model as Product,
				AttributeType,
				Attribute,
				SUM(Quantity) as Quantity
			FROM ProcessHistory.PQCMesData as pd
			WHERE InspectDateTime >= @InspectStart
				AND InspectDateTime <= @InspectEnd
				AND Line = @Line
			GROUP BY pd.Line, pd.Model, Attribute, AttributeType
		) AS __piv
		PIVOT
		(
			SUM(__piv.Quantity)
			FOR __piv.Attribute IN (
				OUTPUT,
				NG1,
				NG2,
				NG3,
				NG4,
				NG5,
				NG6,
				NG7,
				NG8,
				NG9,
				NG10,
				RW1,
				RW2,
				RW3,
				RW4,
				RW5,
				RW6,
				RW7,
				RW8,
				RW9,
				RW10
			)
		) AS __attrs
	GROUP BY Line, Product
END
