INSERT INTO [EducationDB].[dbo].[Subject] ([Name], [Credits])
VALUES 
('Matem�ticas', 3),
('F�sica', 3),
('Qu�mica', 3),
('Lengua y Literatura', 3),
('Historia', 3),
('Biolog�a', 3),
('Inform�tica', 3),
('Educaci�n F�sica', 3),
('Geograf�a', 3),
('Filosof�a', 3);

SELECT TOP (10) [Id]
      ,[Name]
      ,[Credits]
  FROM [EducationDB].[dbo].[Subject]