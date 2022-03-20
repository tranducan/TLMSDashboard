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
           ('ABDCXYZ'
           ,'202103200001'
           ,'ABCXYZ'
           ,'TL01'
           ,'L03'
           ,'PQC'
           ,'OUTPUT'
           ,'OP'
           ,20
           ,'AN'
           ,GETDATE()
		   ),

		    ('ABDCXYZ'
           ,'202103200001'
           ,'BFFCHR4383P'
           ,'TL01'
           ,'L01'
           ,'PQC'
           ,'OUTPUT'
           ,'OP'
           ,30
           ,'AN'
           ,GETDATE()
		   )
GO

select * from [ProcessHistory].[PQCMesData]
wHERE InspectDateTime > GETDATE() -1
