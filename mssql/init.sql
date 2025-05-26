USE master;
GO

-- Cierra conexiones activas a EducationDB
ALTER DATABASE EducationDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO

-- Restaura la base de datos
RESTORE DATABASE EducationDB
FROM DISK = '/backup/EducationDB.bak'
WITH MOVE 'EducationDB' TO '/var/opt/mssql/data/EducationDB.mdf',
     MOVE 'EducationDB_log' TO '/var/opt/mssql/data/EducationDB_log.ldf',
     REPLACE;
GO

-- Devuelve a MULTI_USER después de restaurar
ALTER DATABASE EducationDB SET MULTI_USER;
GO