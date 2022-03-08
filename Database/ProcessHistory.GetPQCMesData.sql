USE ERPSOFT;

GO

-- =============================================
CREATE PROCEDURE ProcessHistory.GetPQCMesData
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
		SUM(__attrs.OP) as OPQty,
		SUM(__attrs.NG) as NGQty,
		SUM(__attrs.RW) as RWQty
	FROM
	(

		SELECT 
				MIN(InspectDateTime) as StartTime,
				MAX(InspectDateTime) as EndTime,
				Line,
				Model as Product,
				AttributeType,
				SUM(Quantity) as Quantity
			FROM ProcessHistory.PQCMesData
			WHERE InspectDateTime >= @InspectStart
				AND InspectDateTime <= @InspectEnd
			GROUP BY Line, Model, AttributeType
		) AS __piv
		PIVOT
		(
			SUM(__piv.Quantity)
			FOR __piv.AttributeType IN (
				NG,
				OP,
				RW
			)
		) AS __attrs
	GROUP BY Line, Product
END
GO
