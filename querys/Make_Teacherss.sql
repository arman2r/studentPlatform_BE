INSERT INTO [EducationDB].[dbo].[Teacher] ([FullName])
VALUES 
('Carlos Gómez'),
('Ana Martínez'),
('Luis Rodríguez'),
('María Fernanda Pérez'),
('Jorge Herrera');

SELECT TOP (5) [Id]
      ,[FullName]
  FROM [EducationDB].[dbo].[Teacher]