INSERT INTO [EducationDB].[dbo].[Teacher] ([FullName])
VALUES 
('Carlos G�mez'),
('Ana Mart�nez'),
('Luis Rodr�guez'),
('Mar�a Fernanda P�rez'),
('Jorge Herrera');

SELECT TOP (5) [Id]
      ,[FullName]
  FROM [EducationDB].[dbo].[Teacher]