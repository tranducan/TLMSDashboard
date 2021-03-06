USE [ERPSOFT]
GO
/****** Object:  StoredProcedure [ProcessHistory].[GetPQCMesDataFilterLine]    Script Date: 21-Mar-22 1:00:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
ALTER PROCEDURE [ProcessHistory].[GetPQCMesDataFilterLine]
(	@Line VARCHAR(10),
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
				AND Line = @Line
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
