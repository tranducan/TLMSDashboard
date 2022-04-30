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
           ,'BFFCHR4383P-NEW'
           ,'TL03'
           ,'L01'
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
