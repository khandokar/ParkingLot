CREATE DATABASE PMS; 
GO

USE [PMS];
-- ParkIn Table
CREATE TABLE [dbo].[ParkIn](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TagNumber] [varchar](10) NOT NULL,
	[CheckIn] [datetime] NOT NULL,
 CONSTRAINT [PK_ParkIn] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [TagNumberUnique] UNIQUE NONCLUSTERED 
(
	[TagNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

USE [PMS];
-- ParkOut Table
CREATE TABLE [dbo].[ParkOut](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[InsertTime] [datetime] NOT NULL,
	[TagNumber] [varchar](10) NOT NULL,
	[CheckIn] [datetime] NOT NULL,
	[CheckOut] [datetime] NOT NULL,
	[ElaspedTime] [decimal](18, 2) NOT NULL,
	[HourlyFee] [decimal](18, 2) NOT NULL,
	[Total] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_ParkOut] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


-- TotalHours function
CREATE FUNCTION [dbo].[TotalHours](
    @fromDate DateTime,
    @todate DateTime
)
RETURNS decimal(10,2)
AS 
BEGIN
    
	DECLARE @hours AS decimal(10,2);

	IF(DATEDIFF(MINUTE, @fromDate, @Todate) <= 0)
	BEGIN
	   SET @hours = 0; 
	END
	ELSE IF(DATEDIFF(MINUTE, @fromDate, @Todate) >= 60)
	BEGIN
	  SET @hours = round(DATEDIFF(MINUTE, @fromDate, @todate)/60 + DATEDIFF(MINUTE, @fromDate, @todate)%60/100.0,2);
	END
	ELSE
	BEGIN
	   SET @hours = round(DATEDIFF(MINUTE, @fromDate, @todate)/100.0,2);
	END

    RETURN @hours;
END;
GO
