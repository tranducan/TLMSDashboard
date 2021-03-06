USE [ERPSOFT]
GO
/****** Object:  StoredProcedure [ProcessHistory].[GetPQCMesDataRealtime]    Script Date: 21-Mar-22 1:01:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [ProcessHistory].[GetPQCMesDataRealtime](
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
		ISNULL(__attrs.OPQty,0) AS OPQty,
		ISNULL(__attrs.NGQty,0) AS NGQty,
		ISNULL(__attrs.RWQty,0) AS RWQty,
		ISNULL(__attrs.OPQty,0) AS Passed,
		(ISNULL(__attrs.NGQty,0)+ISNULL(__attrs.RWQty,0)) AS NotPassed
	FROM 
	(	
	SELECT 
		CONVERT(DATE,InspectDateTime,101 ) AS 'Date',
		DATEPART(HOUR, InspectDateTime) AS 'Hour',
		Line,
		Model as Product,
		CASE  
		WHEN AttributeType = 'OP'  THEN 'OPQty'
		WHEN AttributeType = 'NG' THEN 'NGQty'
		WHEN AttributeType = 'RW' THEN 'RWQty'
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
			OPQty,
			NGQty,
			RWQty
		)
	) AS __attrs
	ORDER BY __attrs.Date, __attrs.Hour, __attrs.Product
END
