INSERT INTO [EducationDB].[dbo].[Subject] ([Name], [Credits])
VALUES 
('Matemáticas', 3),
('Física', 3),
('Química', 3),
('Lengua y Literatura', 3),
('Historia', 3),
('Biología', 3),
('Informática', 3),
('Educación Física', 3),
('Geografía', 3),
('Filosofía', 3);

SELECT TOP (10) [Id]
      ,[Name]
      ,[Credits]
  FROM [EducationDB].[dbo].[Subject]