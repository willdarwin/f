Use FlowerPot
Go
IF Exists (Select 1  From  sysobjects  
           Where  id = object_id('ColumnInAnalyzer') 
           And type = 'U')  
Drop table [ColumnInAnalyzer]
GO
IF Exists (Select 1  From  sysobjects  
           Where  id = object_id('FilterInAnalyzer') 
           And type = 'U')  
Drop table [FilterInAnalyzer]
GO
IF Exists (Select 1  From  sysobjects  
           Where  id = object_id('Analyzer') 
           And type = 'U')  
Drop table [Analyzer]
GO
