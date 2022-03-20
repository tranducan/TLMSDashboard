DECLARE @StartDate DATETIME = '2022-03-20 07:00:00'
DECLARE @EndTime DATETIME = (SELECT GETDATE())

EXECUTE ProcessHistory.GetPQCMesData
@StartDate, @EndTime

EXECUTE ProcessHistory.GetPQCMesDataRealtime
'L01', @StartDate, @EndTime


EXECUTE ProcessHistory.GetPQCMesDataRealtime2
'L01', @StartDate, @EndTime