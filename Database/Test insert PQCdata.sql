USE [ERPSOFT]
GO

INSERT INTO [ProcessHistory].[PQCMesData]
           ([POCode]
           ,[LotNumber]
           ,[Model]
           ,[Site]
           ,[Line]
           ,[Process]
           ,[Attribute]
           ,[AttributeType]
           ,[Quantity]
           ,[Inspector]
           ,[InspectDateTime]
		   )
     VALUES
           ('ABDCXYZ-New'
           ,'202103200001'
           ,'ABCXYZ-TEST'
           ,'TL01'
           ,'L07'
           ,'PQC'
           ,'OUTPUT'
           ,'OP'
           ,50
           ,'AN'
           ,GETDATE()
		   )
GO

select * from [ProcessHistory].[PQCMesData]
wHERE InspectDateTime > GETDATE() -1
