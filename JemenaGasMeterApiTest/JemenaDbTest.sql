/* 
// To Reset Database
DROP TABLE [dbo].[MeterHistories];
DROP TABLE [dbo].[Users];
DROP TABLE [dbo].[Meters];
DROP TABLE [dbo].[Depots];
DROP TABLE [dbo].[Installations];
DROP TABLE [dbo].[Warehouses];
DROP TABLE [dbo].[Transfers];

DROP SEQUENCE MeterHistory;
DROP SEQUENCE Transfer;
DROP SEQUENCE Installation;

use master;
GO
DROP DATABASE If Exists [JemenaDbTest];

*/

CREATE DATABASE JemenaDbTest;
GO

USE [JemenaDbTest]
GO

CREATE SEQUENCE MeterHistory;
CREATE SEQUENCE Transfer;
CREATE SEQUENCE Installation;

/* Create Users */
CREATE TABLE [dbo].[Users] (
    [PayRollID]  NVARCHAR (450) NOT NULL,
    [FirstName]  NVARCHAR (50)  NOT NULL,
    [LastName]   NVARCHAR (50)  NOT NULL,
    [UserType]   INT            NOT NULL,
    [PIN]        NVARCHAR (MAX) NULL,
    [UserStatus] INT            NOT NULL,
    [ModifyDate] DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([PayRollID] ASC)
);

/* Create Meters */
CREATE TABLE [dbo].[Meters] (
    [MIRN]           NVARCHAR (450) NOT NULL,
    [MeterType]      INT            NOT NULL,
    [MeterStatus]    INT            NOT NULL,
    [MeterCondition] INT            NOT NULL,
    [ExpiryDate]     DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_Meters] PRIMARY KEY CLUSTERED ([MIRN] ASC)
);


/* Create Depots */
CREATE TABLE [dbo].[Depots] (
    [DepotID]    NVARCHAR (450) NOT NULL,
    [StreetName] NVARCHAR (MAX) NULL,
    [Suburb]     NVARCHAR (MAX) NULL,
    [PostCode]   INT            NOT NULL,
    [Status]     INT            NOT NULL,
    CONSTRAINT [PK_Depots] PRIMARY KEY CLUSTERED ([DepotID] ASC)
);

/* Create Warehouses */
CREATE TABLE [dbo].[Warehouses] (
    [WarehouseID] NVARCHAR (450) NOT NULL,
    [StreetName]  NVARCHAR (MAX) NULL,
    [Suburb]      NVARCHAR (MAX) NULL,
    [PostCode]    INT            NOT NULL,
    [Status]      INT            NOT NULL,
    CONSTRAINT [PK_Warehouses] PRIMARY KEY CLUSTERED ([WarehouseID] ASC)
);

/* Create MeterTransaction */
CREATE TABLE [dbo].[MeterHistories] (
    [MeterHistoryID]  INT            IDENTITY (1, 1) NOT NULL,
    [MIRN]            NVARCHAR (450) NULL,
    [PayRollID]       NVARCHAR (450) NULL,
    [MeterStatus]     INT            NOT NULL,
    [Location]        NVARCHAR (MAX) NULL,
    [TransfereeID]    NVARCHAR (MAX) NULL,
    [TransactionDate] DATETIME2 (7)  NOT NULL,
    [Comment]         NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_MeterHistories] PRIMARY KEY CLUSTERED ([MeterHistoryID] ASC),
    CONSTRAINT [FK_MeterHistories_Meters_MIRN] FOREIGN KEY ([MIRN]) REFERENCES [dbo].[Meters] ([MIRN]),
    CONSTRAINT [FK_MeterHistories_Users_PayRollID] FOREIGN KEY ([PayRollID]) REFERENCES [dbo].[Users] ([PayRollID])
);

/* Installations */
CREATE TABLE [dbo].[Installations] (
    [InstallationID] INT            IDENTITY (1, 1) NOT NULL,
    [MIRN]           NVARCHAR (MAX) NULL,
    [StreetNo]       NVARCHAR (MAX) NULL,
    [StreetName]     NVARCHAR (MAX) NULL,
    [Suburb]         NVARCHAR (MAX) NULL,
    [State]          NVARCHAR (MAX) NULL,
    [PostCode]       INT            NOT NULL,
    [Status]         INT            NOT NULL,
    CONSTRAINT [PK_Installations] PRIMARY KEY CLUSTERED ([InstallationID] ASC)
);

/* Transfer */
CREATE TABLE [dbo].[Transfers] (
    [TransferID] INT            IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (MAX) NULL,
    [Company]    NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Transfers] PRIMARY KEY CLUSTERED ([TransferID] ASC)
);

/* Insert into User */
INSERT INTO Users (PayRollID, FirstName, LastName, PIN, UserType, UserStatus, ModifyDate)
VALUES ('1001', 'Raji', 'Rudhra', '1234', 1, 1,'2020-06-23 07:30:20');
INSERT INTO Users (PayRollID, FirstName, LastName, PIN, UserType, UserStatus, ModifyDate)
VALUES ('1002', 'Dean', 'Jones', '1234', 2, 2,'2020-06-23 07:30:20');
INSERT INTO Users (PayRollID, FirstName, LastName, PIN, UserType, UserStatus, ModifyDate)
VALUES ('1003', 'Robert', 'Fawkner', '1234', 1, 2,'2020-06-23 07:30:20');
INSERT INTO Users (PayRollID, FirstName, LastName, PIN, UserType, UserStatus, ModifyDate)
VALUES ('1004', 'Peter', 'D', '1234', 2, 1,'2020-06-23 07:30:20');
INSERT INTO Users (PayRollID, FirstName, LastName, PIN, UserType, UserStatus, ModifyDate)
VALUES ('1005', 'Troy', 'E', '1234', 1, 1,'2020-06-23 07:30:20');
INSERT INTO Users (PayRollID, FirstName, LastName, PIN, UserType, UserStatus, ModifyDate)
VALUES ('1006', 'Clare', 'N', '1234', 1, 1,'2020-06-23 07:30:20');
INSERT INTO Users (PayRollID, FirstName, LastName, PIN, UserType, UserStatus, ModifyDate)
VALUES ('1007', 'Kenny', 'E', '1234', 1, 1,'2020-06-23 07:30:20');
INSERT INTO Users (PayRollID, FirstName, LastName, PIN, UserType, UserStatus, ModifyDate)
VALUES ('1008', 'Pretty', 'EJ', '1234', 1, 1,'2020-06-23 07:30:20');
INSERT INTO Users (PayRollID, FirstName, LastName, PIN, UserType, UserStatus, ModifyDate)
VALUES ('1009', 'Scott', 'D', '1234', 1, 1,'2020-06-23 07:30:20');
INSERT INTO Users (PayRollID, FirstName, LastName, PIN, UserType, UserStatus, ModifyDate)
VALUES ('1010', 'Ram', 'Q', '1234', 1, 1,'2020-06-23 07:30:20');
INSERT INTO Users (PayRollID, FirstName, LastName, PIN, UserType, UserStatus, ModifyDate)
VALUES ('1011', 'Ben', 'R', '1234', 1, 1,'2020-06-23 07:30:20');
INSERT INTO Users (PayRollID, FirstName, LastName, PIN, UserType, UserStatus, ModifyDate)
VALUES ('1012', 'Genie', 'A', '1234', 1, 1,'2020-06-23 07:30:20');

/* Insert into Meter */
INSERT INTO Meters(MIRN, MeterType, MeterStatus, MeterCondition, ExpriyDate)
VALUES ('IF0001',1,1,1,'2020-06-30 07:30:20');
INSERT INTO Meters (MIRN, MeterType, MeterStatus, MeterCondition, ExpriyDate)
VALUES('IF0002',1,1,1,'2020-06-30 07:30:20');
INSERT INTO Meters (MIRN, MeterType, MeterStatus, MeterCondition, ExpriyDate)
VALUES ('IF0003',1,1,1,'2020-06-30 07:30:20');
INSERT INTO Meters (MIRN, MeterType, MeterStatus, MeterCondition, ExpriyDate)
VALUES ('IF0004',2,2,1,'2020-06-30 07:30:20');
INSERT INTO Meters (MIRN, MeterType, MeterStatus, MeterCondition, ExpriyDate)
VALUES ('IF0005',1,2,1,'2020-06-30 07:30:20');
INSERT INTO Meters (MIRN, MeterType, MeterStatus, MeterCondition, ExpriyDate)
VALUES ('IF0006',1,2,1,'2020-06-30 07:30:20');
INSERT INTO Meters (MIRN, MeterType, MeterStatus, MeterCondition, ExpriyDate)
VALUES ('IF0007',1,1,1,'2020-06-30 07:30:20');
INSERT INTO Meters (MIRN, MeterType, MeterStatus, MeterCondition, ExpriyDate)
VALUES ('IF0008',1,1,1,'2020-06-30 07:30:20');
INSERT INTO Meters (MIRN, MeterType, MeterStatus, MeterCondition, ExpriyDate)
VALUES ('IF0009',2,1,2,'2020-06-30 07:30:20');
INSERT INTO Meters (MIRN, MeterType, MeterStatus, MeterCondition, ExpriyDate)
VALUES ('IF0010',2,1,3,'2020-06-30 07:30:20');

/* Insert into Depots */
INSERT INTO Depots (DepotID, StreetName, Suburb, PostCode, Status)
VALUES ('D0001','123 Fake Street', 'Melbourne', 3000,1);
INSERT INTO Depots (DepotID, StreetName, Suburb, PostCode, Status)
VALUES ('D0002','unit23 Jake Road', 'Doveton', 3175,1);
INSERT INTO Depots (DepotID, StreetName, Suburb, PostCode, Status)
VALUES ('D0003','53/2 Collin Street', 'Melbourne', 3001,1);
INSERT INTO Depots (DepotID, StreetName, Suburb, PostCode, Status)
VALUES ('D0004','Unit 93A Flinder Street', 'Melbourne', 3000,1);
INSERT INTO Depots (DepotID, StreetName, Suburb, PostCode, Status)
VALUES ('D0005','123 Clyton Road', 'Melbourne', 3012,1);
INSERT INTO Depots (DepotID, StreetName, Suburb, PostCode, Status)
VALUES ('D0006','45/3 Old Dandenong Road', 'Melbourne', 3890,1);

/* Insert into Warehouses */
INSERT INTO Warehouses (WarehouseID, StreetName, Suburb, PostCode,Status)
VALUES ('W0001','43 Albacore Crescent', 'BURRADOO', 2576,1);
INSERT INTO Warehouses (WarehouseID, StreetName, Suburb, PostCode,Status)
VALUES ('W0002','unit68 Boobialla Street', 'GANMAIN', 2702,1);
INSERT INTO Warehouses (WarehouseID, StreetName, Suburb, PostCode,Status)
VALUES ('W0003','80/2 Sunnyside Road', 'TAYLORVILLE', 5330,1);
INSERT INTO Warehouses (WarehouseID, StreetName, Suburb, PostCode,Status)
VALUES ('W0004','96 Banksia Court', 'CHARTERS TOWERS', 4820,1);
INSERT INTO Warehouses (WarehouseID, StreetName, Suburb, PostCode,Status)
VALUES ('W0005','54 McLachlan Street', 'HORSHAM', 3401,1);
INSERT INTO Warehouses (WarehouseID, StreetName, Suburb, PostCode,Status)
VALUES ('W0006','58 Benny Street', 'ERRIBA', 7310,1);

/* Insert into Installations */
INSERT INTO Installations (MIRN, StreetNo, StreetName, Suburb, State, PostCode,Status)
VALUES ('IF0001', '43', 'Albacore Crescent', 'BURRADOO','NSW', 2576,1);
INSERT INTO Installations (MIRN,StreetNo,StreetName, Suburb, State,PostCode,Status)
VALUES ('IF0002','unit68','Boobialla Street', 'GANMAIN','NSW', 2702,1);
INSERT INTO Installations (MIRN,StreetNo, StreetName, Suburb, State,PostCode,Status)
VALUES ('IF0003','80/2', 'Sunnyside Road', 'TAYLORVILLE','NSW', 5330,1);
INSERT INTO Installations (MIRN,StreetNo, StreetName, Suburb, State,PostCode,Status)
VALUES ('IF0004','96', 'Banksia Court', 'CHARTERS TOWERS','NSW', 4820,1);
INSERT INTO Installations (MIRN,StreetNo, StreetName, Suburb, State,PostCode,Status)
VALUES ('IF0005','54', 'McLachlan Street', 'HORSHAM', 'NSW',3401,1);
INSERT INTO Installations (MIRN,StreetNo, StreetName, Suburb, State,PostCode,Status)
VALUES ('IF0007','60','Bent Road', 'ERRIBA','NSW', 7310,1);

/* Insert into Transfer */
INSERT INTO Transfers(Name, Company) VALUES('Tayla','Layla');
INSERT INTO Transfers(Name, Company) VALUES('Hemmant','NADGEE');
INSERT INTO Transfers(Name, Company) VALUES('Noah','CREEK');
INSERT INTO Transfers(Name, Company) VALUES('Connor','JAY');
INSERT INTO Transfers(Name, Company) VALUES('Flynn','MOOK');
INSERT INTO Transfers(Name, Company) VALUES('Cope','MOLLY');

/* Insert into MeterTransaction */
INSERT INTO MeterHistories (MIRN, PayRollID, MeterStatus, Location, TransfereeID, TransactionDate,Comment)
VALUES ('IF0001','1001','2','W0001','','2018-06-23 07:30:20','');
INSERT INTO MeterHistories (MIRN, PayRollID, MeterStatus, Location, TransfereeID, TransactionDate,Comment)
VALUES ('IF0003','1001','2','D0003','','2018-06-23 07:19:20','');
INSERT INTO MeterHistories (MIRN, PayRollID, MeterStatus, Location, TransfereeID, TransactionDate,Comment)
VALUES ('IF0002','1002','3','W0004','','2018-06-23 07:36:20','');
INSERT INTO MeterHistories (MIRN, PayRollID, MeterStatus, Location, TransfereeID, TransactionDate,Comment)
VALUES ('IF0004','1002','2','','2','2018-06-23 12:42:20','');
INSERT INTO MeterHistories (MIRN, PayRollID, MeterStatus, Location, TransfereeID, TransactionDate,Comment)
VALUES ('IF0005','1001','5','','1','2018-06-23 03:35:20','Transfered to peter');
INSERT INTO MeterHistories (MIRN, PayRollID, MeterStatus, Location, TransfereeID, TransactionDate,Comment)
VALUES ('IF0006','1001','4','','1','2018-06-23 05:56:20','');
INSERT INTO MeterHistories (MIRN, PayRollID, MeterStatus, Location, TransfereeID, TransactionDate,Comment)
VALUES ('IF0007','1003','2','W0002','','2018-06-23 08:23:20','');
INSERT INTO MeterHistories (MIRN, PayRollID, MeterStatus, Location, TransfereeID, TransactionDate,Comment)
VALUES ('IF0008','1004','4','4','','2018-06-23 01:33:20','');



