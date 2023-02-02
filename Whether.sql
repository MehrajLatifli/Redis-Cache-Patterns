CREATE DATABASE [Weather]


USE [Weather]


CREATE TABLE [WeatherInformation]
(
IdWeather INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
[Status]  NVARCHAR(max) NOT NULL,
[Country] NVARCHAR(max) NOT NULL,
[City] NVARCHAR(max) NOT NULL,
[When]  datetime2 DEFAULT GETDATE(),
[Description]  NVARCHAR(max) NOT NULL,
)

INSERT INTO [WeatherInformation] ([Status], [Country], [City], [Description])
VALUES (N'[Status]', N'[Country]', N'[City]',N'[Description]'); 


Select *From [WeatherInformation]