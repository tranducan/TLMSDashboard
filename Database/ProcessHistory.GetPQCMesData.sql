USE [ERPSOFT]
GO
/****** Object:  StoredProcedure [ProcessHistory].[GetPQCMesData]    Script Date: 21-Mar-22 1:00:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
ALTER PROCEDURE [ProcessHistory].[GetPQCMesData]
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
			WHERE InspectDateTime >= @InspectStart
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
