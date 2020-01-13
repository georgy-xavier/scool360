-- MySQL Administrator dump 1.4
--
-- ------------------------------------------------------
-- Server version	5.1.30-community


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


--
-- Create schema winschooldb
--

CREATE DATABASE IF NOT EXISTS winschooldb;
USE winschooldb;

--
-- Definition of table `tblaccount`
--

DROP TABLE IF EXISTS `tblaccount`;
CREATE TABLE `tblaccount` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `AccountName` varchar(100) DEFAULT NULL,
  `Parent` int(11) DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblaccount`
--

/*!40000 ALTER TABLE `tblaccount` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblaccount` ENABLE KEYS */;


--
-- Definition of table `tblaction`
--

DROP TABLE IF EXISTS `tblaction`;
CREATE TABLE `tblaction` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ActionName` varchar(35) DEFAULT NULL,
  `Description` varchar(500) DEFAULT NULL,
  `Link` varchar(30) DEFAULT NULL,
  `ActionType` varchar(30) DEFAULT NULL,
  `Order` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=50 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblaction`
--

/*!40000 ALTER TABLE `tblaction` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblaction` ENABLE KEYS */;


--
-- Definition of table `tblbatch`
--

DROP TABLE IF EXISTS `tblbatch`;
CREATE TABLE `tblbatch` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `BatchName` varchar(100) DEFAULT NULL,
  `StartDate` date DEFAULT NULL,
  `EndDate` date DEFAULT NULL,
  `Status` int(5) DEFAULT '0',
  `Created` int(2) DEFAULT '0',
  `NOfWorkingDays` int(11) DEFAULT NULL,
  `LastbatchId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=65 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblbatch`
--

/*!40000 ALTER TABLE `tblbatch` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblbatch` ENABLE KEYS */;


--
-- Definition of table `tblbloodgrp`
--

DROP TABLE IF EXISTS `tblbloodgrp`;
CREATE TABLE `tblbloodgrp` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `GroupName` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblbloodgrp`
--

/*!40000 ALTER TABLE `tblbloodgrp` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblbloodgrp` ENABLE KEYS */;


--
-- Definition of table `tblclass`
--

DROP TABLE IF EXISTS `tblclass`;
CREATE TABLE `tblclass` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ClassName` varchar(100) DEFAULT NULL,
  `Standard` varchar(50) DEFAULT NULL,
  `ParentGroupID` int(11) DEFAULT NULL,
  `Status` int(5) DEFAULT '1',
  `Division` varchar(30) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblclass`
--

/*!40000 ALTER TABLE `tblclass` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblclass` ENABLE KEYS */;


--
-- Definition of table `tblclassroom`
--

DROP TABLE IF EXISTS `tblclassroom`;
CREATE TABLE `tblclassroom` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `RoomNumber` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblclassroom`
--

/*!40000 ALTER TABLE `tblclassroom` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblclassroom` ENABLE KEYS */;


--
-- Definition of table `tblclassschedule`
--

DROP TABLE IF EXISTS `tblclassschedule`;
CREATE TABLE `tblclassschedule` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ClassId` int(11) DEFAULT NULL,
  `BatchId` int(11) DEFAULT NULL,
  `ClassTeacherId` int(11) DEFAULT NULL,
  `ClassRoom` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblclassschedule`
--

/*!40000 ALTER TABLE `tblclassschedule` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblclassschedule` ENABLE KEYS */;


--
-- Definition of table `tblclassstaffmap`
--

DROP TABLE IF EXISTS `tblclassstaffmap`;
CREATE TABLE `tblclassstaffmap` (
  `ClassId` int(11) DEFAULT NULL,
  `SubjectId` int(11) DEFAULT NULL,
  `StaffId` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblclassstaffmap`
--

/*!40000 ALTER TABLE `tblclassstaffmap` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblclassstaffmap` ENABLE KEYS */;


--
-- Definition of table `tblclasssubmap`
--

DROP TABLE IF EXISTS `tblclasssubmap`;
CREATE TABLE `tblclasssubmap` (
  `ClassId` int(11) DEFAULT NULL,
  `SubjectId` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblclasssubmap`
--

/*!40000 ALTER TABLE `tblclasssubmap` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblclasssubmap` ENABLE KEYS */;


--
-- Definition of table `tblconfiguration`
--

DROP TABLE IF EXISTS `tblconfiguration`;
CREATE TABLE `tblconfiguration` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Module` varchar(255) DEFAULT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `Value` varchar(255) DEFAULT NULL,
  `Disc` varchar(500) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblconfiguration`
--

/*!40000 ALTER TABLE `tblconfiguration` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblconfiguration` ENABLE KEYS */;


--
-- Definition of table `tblcriteria`
--

DROP TABLE IF EXISTS `tblcriteria`;
CREATE TABLE `tblcriteria` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Module` varchar(255) DEFAULT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `Value1` int(11) DEFAULT NULL,
  `Value2` int(11) DEFAULT NULL,
  `Condition` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblcriteria`
--

/*!40000 ALTER TABLE `tblcriteria` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblcriteria` ENABLE KEYS */;


--
-- Definition of table `tblexam`
--

DROP TABLE IF EXISTS `tblexam`;
CREATE TABLE `tblexam` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ExamName` varchar(255) DEFAULT NULL,
  `ClassId` int(11) DEFAULT NULL,
  `CreationDate` datetime DEFAULT NULL,
  `Status` int(3) DEFAULT '1',
  `TypeId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblexam`
--

/*!40000 ALTER TABLE `tblexam` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblexam` ENABLE KEYS */;


--
-- Definition of table `tblexammark`
--

DROP TABLE IF EXISTS `tblexammark`;
CREATE TABLE `tblexammark` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ExamSchId` int(11) DEFAULT NULL,
  `MarkColumn` varchar(100) DEFAULT NULL,
  `SubjectId` int(11) DEFAULT NULL,
  `PassMark` double DEFAULT NULL,
  `MaxMark` double DEFAULT NULL,
  `ExamDate` date DEFAULT NULL,
  `StartTime` varchar(100) DEFAULT NULL,
  `EndTime` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblexammark`
--

/*!40000 ALTER TABLE `tblexammark` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblexammark` ENABLE KEYS */;


--
-- Definition of table `tblexamschedule`
--

DROP TABLE IF EXISTS `tblexamschedule`;
CREATE TABLE `tblexamschedule` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `BatchId` int(11) DEFAULT NULL,
  `ExamId` int(11) DEFAULT NULL,
  `Status` varchar(255) DEFAULT NULL,
  `ScheduledDate` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblexamschedule`
--

/*!40000 ALTER TABLE `tblexamschedule` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblexamschedule` ENABLE KEYS */;


--
-- Definition of table `tblexamsubjectmap`
--

DROP TABLE IF EXISTS `tblexamsubjectmap`;
CREATE TABLE `tblexamsubjectmap` (
  `ExamId` int(11) DEFAULT NULL,
  `SubjectId` int(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblexamsubjectmap`
--

/*!40000 ALTER TABLE `tblexamsubjectmap` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblexamsubjectmap` ENABLE KEYS */;


--
-- Definition of table `tblfeeaccount`
--

DROP TABLE IF EXISTS `tblfeeaccount`;
CREATE TABLE `tblfeeaccount` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `AccountName` varchar(255) DEFAULT NULL,
  `FrequencyId` int(11) DEFAULT NULL,
  `AssociatedId` int(11) DEFAULT NULL,
  `Desciptrion` varchar(500) DEFAULT NULL,
  `CreatedUserId` int(11) DEFAULT NULL,
  `CreatedTime` datetime DEFAULT NULL,
  `Status` int(3) DEFAULT '1',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblfeeaccount`
--

/*!40000 ALTER TABLE `tblfeeaccount` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblfeeaccount` ENABLE KEYS */;


--
-- Definition of table `tblfeeasso`
--

DROP TABLE IF EXISTS `tblfeeasso`;
CREATE TABLE `tblfeeasso` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblfeeasso`
--

/*!40000 ALTER TABLE `tblfeeasso` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblfeeasso` ENABLE KEYS */;


--
-- Definition of table `tblfeebill`
--

DROP TABLE IF EXISTS `tblfeebill`;
CREATE TABLE `tblfeebill` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `StudentID` varchar(255) DEFAULT NULL,
  `TotalAmount` double DEFAULT NULL,
  `Date` datetime DEFAULT NULL,
  `PaymentMode` varchar(100) DEFAULT NULL,
  `PaymentModeId` varchar(50) DEFAULT NULL,
  `BankName` varchar(100) DEFAULT NULL,
  `BillNo` varchar(50) DEFAULT '',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `NewIndex` (`BillNo`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblfeebill`
--

/*!40000 ALTER TABLE `tblfeebill` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblfeebill` ENABLE KEYS */;


--
-- Definition of table `tblfeefrequencytype`
--

DROP TABLE IF EXISTS `tblfeefrequencytype`;
CREATE TABLE `tblfeefrequencytype` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FreequencyName` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblfeefrequencytype`
--

/*!40000 ALTER TABLE `tblfeefrequencytype` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblfeefrequencytype` ENABLE KEYS */;


--
-- Definition of table `tblfeeschedule`
--

DROP TABLE IF EXISTS `tblfeeschedule`;
CREATE TABLE `tblfeeschedule` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FeeId` int(11) NOT NULL DEFAULT '0',
  `BatchId` int(11) NOT NULL DEFAULT '0',
  `Duedate` date NOT NULL DEFAULT '0000-00-00',
  `LastDate` date NOT NULL DEFAULT '0000-00-00',
  `Status` varchar(100) DEFAULT NULL,
  `ClassId` int(11) DEFAULT NULL,
  `PeriodId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblfeeschedule`
--

/*!40000 ALTER TABLE `tblfeeschedule` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblfeeschedule` ENABLE KEYS */;


--
-- Definition of table `tblfeestudent`
--

DROP TABLE IF EXISTS `tblfeestudent`;
CREATE TABLE `tblfeestudent` (
  `Id` int(20) NOT NULL AUTO_INCREMENT,
  `SchId` int(20) DEFAULT NULL,
  `StudId` int(20) DEFAULT NULL,
  `Amount` double DEFAULT NULL,
  `BalanceAmount` double DEFAULT NULL,
  `Status` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblfeestudent`
--

/*!40000 ALTER TABLE `tblfeestudent` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblfeestudent` ENABLE KEYS */;


--
-- Definition of table `tblfileurl`
--

DROP TABLE IF EXISTS `tblfileurl`;
CREATE TABLE `tblfileurl` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FilePath` varchar(255) DEFAULT NULL,
  `Type` varchar(50) DEFAULT NULL,
  `UserId` int(50) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblfileurl`
--

/*!40000 ALTER TABLE `tblfileurl` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblfileurl` ENABLE KEYS */;


--
-- Definition of table `tblfine`
--

DROP TABLE IF EXISTS `tblfine`;
CREATE TABLE `tblfine` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Amount` double DEFAULT NULL,
  `Frequency` double DEFAULT NULL,
  `Type` varchar(50) DEFAULT NULL,
  `FeeId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblfine`
--

/*!40000 ALTER TABLE `tblfine` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblfine` ENABLE KEYS */;


--
-- Definition of table `tblgrade`
--

DROP TABLE IF EXISTS `tblgrade`;
CREATE TABLE `tblgrade` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Grade` varchar(50) DEFAULT NULL,
  `UpperLimit` double DEFAULT NULL,
  `LowerLimit` double DEFAULT NULL,
  `Type` varchar(50) DEFAULT NULL,
  `Status` int(4) DEFAULT NULL,
  `Result` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblgrade`
--

/*!40000 ALTER TABLE `tblgrade` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblgrade` ENABLE KEYS */;


--
-- Definition of table `tblgroup`
--

DROP TABLE IF EXISTS `tblgroup`;
CREATE TABLE `tblgroup` (
  `Id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `GroupName` varchar(30) DEFAULT NULL,
  `Discription` varchar(255) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT NULL,
  `ParentId` int(11) DEFAULT '-1',
  `ManagerId` int(11) DEFAULT '-1',
  `GroupTypeId` int(11) DEFAULT '1',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblgroup`
--

/*!40000 ALTER TABLE `tblgroup` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblgroup` ENABLE KEYS */;


--
-- Definition of table `tblgrouprelation`
--

DROP TABLE IF EXISTS `tblgrouprelation`;
CREATE TABLE `tblgrouprelation` (
  `ParentId` int(11) unsigned DEFAULT NULL,
  `ChildId` int(11) unsigned DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblgrouprelation`
--

/*!40000 ALTER TABLE `tblgrouprelation` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblgrouprelation` ENABLE KEYS */;


--
-- Definition of table `tblgrouptype`
--

DROP TABLE IF EXISTS `tblgrouptype`;
CREATE TABLE `tblgrouptype` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `TypeName` varchar(30) DEFAULT NULL,
  `Description` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblgrouptype`
--

/*!40000 ALTER TABLE `tblgrouptype` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblgrouptype` ENABLE KEYS */;


--
-- Definition of table `tblgroupusermap`
--

DROP TABLE IF EXISTS `tblgroupusermap`;
CREATE TABLE `tblgroupusermap` (
  `GroupId` int(11) DEFAULT NULL,
  `UserId` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblgroupusermap`
--

/*!40000 ALTER TABLE `tblgroupusermap` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblgroupusermap` ENABLE KEYS */;


--
-- Definition of table `tbllanguage`
--

DROP TABLE IF EXISTS `tbllanguage`;
CREATE TABLE `tbllanguage` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Language` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tbllanguage`
--

/*!40000 ALTER TABLE `tbllanguage` DISABLE KEYS */;
/*!40000 ALTER TABLE `tbllanguage` ENABLE KEYS */;


--
-- Definition of table `tblmodule`
--

DROP TABLE IF EXISTS `tblmodule`;
CREATE TABLE `tblmodule` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ModuleName` varchar(30) DEFAULT NULL,
  `Discription` varchar(255) DEFAULT NULL,
  `MenuName` varchar(30) DEFAULT NULL,
  `Link` varchar(30) DEFAULT NULL,
  `Order` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblmodule`
--

/*!40000 ALTER TABLE `tblmodule` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblmodule` ENABLE KEYS */;


--
-- Definition of table `tblmoduleactionmap`
--

DROP TABLE IF EXISTS `tblmoduleactionmap`;
CREATE TABLE `tblmoduleactionmap` (
  `ModuleId` int(11) unsigned DEFAULT NULL,
  `ActionId` int(11) unsigned DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblmoduleactionmap`
--

/*!40000 ALTER TABLE `tblmoduleactionmap` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblmoduleactionmap` ENABLE KEYS */;


--
-- Definition of table `tblperiod`
--

DROP TABLE IF EXISTS `tblperiod`;
CREATE TABLE `tblperiod` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Period` varchar(255) DEFAULT NULL,
  `FrequencyId` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=29 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblperiod`
--

/*!40000 ALTER TABLE `tblperiod` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblperiod` ENABLE KEYS */;


--
-- Definition of table `tblpreviousschooldata`
--

DROP TABLE IF EXISTS `tblpreviousschooldata`;
CREATE TABLE `tblpreviousschooldata` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `StudentId` int(11) DEFAULT NULL,
  `NameofSchool` varchar(255) DEFAULT NULL,
  `ConcessionScholarshipDetails` varchar(255) DEFAULT NULL,
  `StandarsCoveredDetails` varchar(255) DEFAULT NULL,
  `DateofLeaving` date DEFAULT NULL,
  `Reason` varchar(255) DEFAULT NULL,
  `TCNumber` varchar(100) DEFAULT NULL,
  `Innoculated` varchar(50) DEFAULT NULL,
  `1stLanguage` varchar(100) DEFAULT NULL,
  `LanguagesStudied` varchar(255) DEFAULT NULL,
  `MediumOfInstruction` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblpreviousschooldata`
--

/*!40000 ALTER TABLE `tblpreviousschooldata` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblpreviousschooldata` ENABLE KEYS */;


--
-- Definition of table `tblpromotionmap`
--

DROP TABLE IF EXISTS `tblpromotionmap`;
CREATE TABLE `tblpromotionmap` (
  `StdFrom` varchar(100) DEFAULT NULL,
  `StdTo` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblpromotionmap`
--

/*!40000 ALTER TABLE `tblpromotionmap` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblpromotionmap` ENABLE KEYS */;


--
-- Definition of table `tblquatermonthmap`
--

DROP TABLE IF EXISTS `tblquatermonthmap`;
CREATE TABLE `tblquatermonthmap` (
  `PeriodId` int(11) DEFAULT NULL,
  `MonthId` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblquatermonthmap`
--

/*!40000 ALTER TABLE `tblquatermonthmap` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblquatermonthmap` ENABLE KEYS */;


--
-- Definition of table `tblreligion`
--

DROP TABLE IF EXISTS `tblreligion`;
CREATE TABLE `tblreligion` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Religion` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblreligion`
--

/*!40000 ALTER TABLE `tblreligion` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblreligion` ENABLE KEYS */;


--
-- Definition of table `tblresignreason`
--

DROP TABLE IF EXISTS `tblresignreason`;
CREATE TABLE `tblresignreason` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Reason` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblresignreason`
--

/*!40000 ALTER TABLE `tblresignreason` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblresignreason` ENABLE KEYS */;


--
-- Definition of table `tblresignuser`
--

DROP TABLE IF EXISTS `tblresignuser`;
CREATE TABLE `tblresignuser` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` varchar(255) DEFAULT NULL,
  `ReasionId` int(11) DEFAULT NULL,
  `Discription` varchar(300) DEFAULT NULL,
  `ResignDate` date DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblresignuser`
--

/*!40000 ALTER TABLE `tblresignuser` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblresignuser` ENABLE KEYS */;


--
-- Definition of table `tblrole`
--

DROP TABLE IF EXISTS `tblrole`;
CREATE TABLE `tblrole` (
  `Id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `RoleName` varchar(25) DEFAULT NULL,
  `Discription` varchar(255) DEFAULT NULL,
  `Type` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblrole`
--

/*!40000 ALTER TABLE `tblrole` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblrole` ENABLE KEYS */;


--
-- Definition of table `tblroleactionmap`
--

DROP TABLE IF EXISTS `tblroleactionmap`;
CREATE TABLE `tblroleactionmap` (
  `RoleId` int(11) unsigned DEFAULT NULL,
  `ActionId` int(11) unsigned DEFAULT NULL,
  `ModuleId` int(11) unsigned DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblroleactionmap`
--

/*!40000 ALTER TABLE `tblroleactionmap` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblroleactionmap` ENABLE KEYS */;


--
-- Definition of table `tblschooldetails`
--

DROP TABLE IF EXISTS `tblschooldetails`;
CREATE TABLE `tblschooldetails` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `LogoUrl` varchar(255) DEFAULT NULL,
  `SchoolName` varchar(255) DEFAULT NULL,
  `Address` varchar(500) DEFAULT NULL,
  `Syllabus` varchar(50) DEFAULT NULL,
  `MediumofInstruction` varchar(50) DEFAULT NULL,
  `Disc` varchar(1000) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblschooldetails`
--

/*!40000 ALTER TABLE `tblschooldetails` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblschooldetails` ENABLE KEYS */;


--
-- Definition of table `tblstaffdetails`
--

DROP TABLE IF EXISTS `tblstaffdetails`;
CREATE TABLE `tblstaffdetails` (
  `UserId` int(11) DEFAULT NULL,
  `JoiningDate` date DEFAULT NULL,
  `Address` varchar(255) DEFAULT NULL,
  `Sex` varchar(50) DEFAULT NULL,
  `Experience` double DEFAULT NULL,
  `ExpDescription` varchar(255) DEFAULT NULL,
  `Designation` varchar(100) DEFAULT NULL,
  `PhoneNumber` varchar(50) DEFAULT NULL,
  `EduQualifications` varchar(255) DEFAULT NULL,
  `Dob` date DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblstaffdetails`
--

/*!40000 ALTER TABLE `tblstaffdetails` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblstaffdetails` ENABLE KEYS */;


--
-- Definition of table `tblstaffsubjectmap`
--

DROP TABLE IF EXISTS `tblstaffsubjectmap`;
CREATE TABLE `tblstaffsubjectmap` (
  `StaffId` int(30) DEFAULT NULL,
  `SubjectId` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblstaffsubjectmap`
--

/*!40000 ALTER TABLE `tblstaffsubjectmap` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblstaffsubjectmap` ENABLE KEYS */;


--
-- Definition of table `tblstandard`
--

DROP TABLE IF EXISTS `tblstandard`;
CREATE TABLE `tblstandard` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblstandard`
--

/*!40000 ALTER TABLE `tblstandard` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblstandard` ENABLE KEYS */;


--
-- Definition of table `tblstayingwith`
--

DROP TABLE IF EXISTS `tblstayingwith`;
CREATE TABLE `tblstayingwith` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `StayingWith` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblstayingwith`
--

/*!40000 ALTER TABLE `tblstayingwith` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblstayingwith` ENABLE KEYS */;


--
-- Definition of table `tblstudent`
--

DROP TABLE IF EXISTS `tblstudent`;
CREATE TABLE `tblstudent` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `StudentName` varchar(256) DEFAULT NULL,
  `GardianName` varchar(100) DEFAULT NULL,
  `AdmitionNo` varchar(255) DEFAULT NULL,
  `DOB` date DEFAULT NULL,
  `Sex` varchar(30) DEFAULT NULL,
  `Address` varchar(500) DEFAULT NULL,
  `BloodGroup` varchar(255) DEFAULT NULL,
  `Religion` varchar(50) DEFAULT NULL,
  `Cast` varchar(255) DEFAULT NULL,
  `DateofJoining` date DEFAULT NULL,
  `DateOfLeaving` date DEFAULT NULL,
  `Status` int(5) DEFAULT NULL,
  `Email` varchar(50) DEFAULT NULL,
  `Location` varchar(100) DEFAULT NULL,
  `Pin` int(50) DEFAULT NULL,
  `State` varchar(50) DEFAULT NULL,
  `Nationality` varchar(50) DEFAULT NULL,
  `FatherEduQuali` varchar(255) DEFAULT NULL,
  `MothersName` varchar(100) DEFAULT NULL,
  `MotherEduQuali` varchar(100) DEFAULT NULL,
  `FatherOccupation` varchar(100) DEFAULT NULL,
  `AnnualIncome` double DEFAULT NULL,
  `ScheduledcasteType` varchar(50) DEFAULT NULL,
  `MotherTongue` varchar(50) DEFAULT NULL,
  `OtherLanguages` varchar(255) DEFAULT NULL,
  `ResidencePhNo` varchar(25) DEFAULT NULL,
  `OfficePhNo` varchar(25) DEFAULT NULL,
  `FeeReceiptNo` varchar(100) DEFAULT NULL,
  `1stLanguage` varchar(100) DEFAULT NULL,
  `StayingWith` varchar(50) DEFAULT NULL,
  `PlaceOfBirth` varchar(50) DEFAULT NULL,
  `Village` varchar(50) DEFAULT NULL,
  `Town` varchar(50) DEFAULT NULL,
  `Taluk` varchar(50) DEFAULT NULL,
  `District` varchar(50) DEFAULT NULL,
  `NumberofBrothers` int(11) DEFAULT NULL,
  `NumberOfSysters` int(11) DEFAULT NULL,
  `JoinBatch` varchar(11) DEFAULT NULL,
  `CreationTime` varchar(100) DEFAULT NULL,
  `Addresspresent` varchar(500) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblstudent`
--

/*!40000 ALTER TABLE `tblstudent` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblstudent` ENABLE KEYS */;


--
-- Definition of table `tblstudentclassmap`
--

DROP TABLE IF EXISTS `tblstudentclassmap`;
CREATE TABLE `tblstudentclassmap` (
  `StudentId` int(30) DEFAULT NULL,
  `ClassId` int(11) DEFAULT NULL,
  `Standard` varchar(100) DEFAULT NULL,
  `BatchId` int(11) DEFAULT NULL,
  `RollNo` int(11) DEFAULT '-1',
  `Status` int(3) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblstudentclassmap`
--

/*!40000 ALTER TABLE `tblstudentclassmap` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblstudentclassmap` ENABLE KEYS */;


--
-- Definition of table `tblstudentmark`
--

DROP TABLE IF EXISTS `tblstudentmark`;
CREATE TABLE `tblstudentmark` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ExamSchId` int(11) DEFAULT NULL,
  `StudId` int(30) DEFAULT NULL,
  `Mark1` double DEFAULT NULL,
  `Mark2` double DEFAULT NULL,
  `Mark3` double DEFAULT NULL,
  `Mark4` double DEFAULT NULL,
  `Mark5` double DEFAULT NULL,
  `Mark6` double DEFAULT NULL,
  `Mark7` double DEFAULT NULL,
  `Mark8` double DEFAULT NULL,
  `Mark9` double DEFAULT NULL,
  `Mark10` double DEFAULT NULL,
  `Mark11` double DEFAULT NULL,
  `Mark12` double DEFAULT NULL,
  `Mark13` double DEFAULT NULL,
  `Mark14` double DEFAULT NULL,
  `Mark15` double DEFAULT NULL,
  `Mark16` double DEFAULT NULL,
  `Mark17` double DEFAULT NULL,
  `Mark18` double DEFAULT NULL,
  `Mark19` double DEFAULT NULL,
  `Mark20` double DEFAULT NULL,
  `TotalMark` double DEFAULT NULL,
  `TotalMax` double DEFAULT NULL,
  `Avg` double DEFAULT NULL,
  `Grade` varchar(200) DEFAULT '',
  `Result` varchar(100) DEFAULT NULL,
  `Rank` int(11) DEFAULT NULL,
  `Remark` varchar(500) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblstudentmark`
--

/*!40000 ALTER TABLE `tblstudentmark` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblstudentmark` ENABLE KEYS */;


--
-- Definition of table `tblsubject_type`
--

DROP TABLE IF EXISTS `tblsubject_type`;
CREATE TABLE `tblsubject_type` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `sbject_type` varchar(255) DEFAULT NULL,
  `TypeDisc` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblsubject_type`
--

/*!40000 ALTER TABLE `tblsubject_type` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblsubject_type` ENABLE KEYS */;


--
-- Definition of table `tblsubjects`
--

DROP TABLE IF EXISTS `tblsubjects`;
CREATE TABLE `tblsubjects` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `subject_name` varchar(255) DEFAULT NULL,
  `sub_description` varchar(255) DEFAULT NULL,
  `sub_Catagory` int(11) DEFAULT NULL,
  `SubjectCode` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblsubjects`
--

/*!40000 ALTER TABLE `tblsubjects` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblsubjects` ENABLE KEYS */;


--
-- Definition of table `tbltc`
--

DROP TABLE IF EXISTS `tbltc`;
CREATE TABLE `tbltc` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `StudentId` int(11) DEFAULT NULL,
  `TcNumber` varchar(50) DEFAULT NULL,
  `StudentName` varchar(50) DEFAULT NULL,
  `NameOFSchool` varchar(100) DEFAULT NULL,
  `AdmissionNo` varchar(50) DEFAULT NULL,
  `CumulativeRecNo` varchar(50) DEFAULT NULL,
  `Sex` varchar(20) DEFAULT NULL,
  `NameOfFather` varchar(50) DEFAULT NULL,
  `Nationality` varchar(50) DEFAULT NULL,
  `Religion` varchar(50) DEFAULT NULL,
  `Cast` varchar(50) DEFAULT NULL,
  `CasteType` varchar(50) DEFAULT NULL,
  `Dob` date DEFAULT NULL,
  `CurrStandard` varchar(50) DEFAULT NULL,
  `LangStudied` varchar(100) DEFAULT NULL,
  `MediumOfIns` varchar(50) DEFAULT NULL,
  `Syllabus` varchar(50) DEFAULT NULL,
  `DateOfAdmission` varchar(50) DEFAULT NULL,
  `WhetherQualiForPromo` varchar(50) DEFAULT NULL,
  `FeesDue` varchar(50) DEFAULT NULL,
  `FeeConcessions` varchar(300) DEFAULT NULL,
  `SCholarship` varchar(300) DEFAULT NULL,
  `MedicalyExmnd` varchar(50) DEFAULT NULL,
  `LastAttendanceDate` date DEFAULT NULL,
  `AppForTcRecvedDate` date DEFAULT NULL,
  `DateOfIssueOfTC` date DEFAULT NULL,
  `NoOfSchoolDays` varchar(20) DEFAULT NULL,
  `SchoolDaysAttended` varchar(20) DEFAULT NULL,
  `CharacterNConduct` varchar(300) DEFAULT NULL,
  `CreationDate` date DEFAULT NULL,
  `Status` int(11) DEFAULT NULL,
  `LastBatchId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tbltc`
--

/*!40000 ALTER TABLE `tbltc` DISABLE KEYS */;
/*!40000 ALTER TABLE `tbltc` ENABLE KEYS */;


--
-- Definition of table `tbltransaction`
--

DROP TABLE IF EXISTS `tbltransaction`;
CREATE TABLE `tbltransaction` (
  `TransationId` int(11) NOT NULL AUTO_INCREMENT,
  `PaymentElementId` int(11) NOT NULL DEFAULT '0',
  `UserId` int(11) DEFAULT NULL,
  `Amount` double NOT NULL DEFAULT '0',
  `PaidDate` date DEFAULT NULL,
  `BillNo` varchar(50) DEFAULT NULL,
  `AccountTo` int(5) DEFAULT NULL,
  `AccountFrom` int(5) DEFAULT NULL,
  `Type` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`TransationId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tbltransaction`
--

/*!40000 ALTER TABLE `tbltransaction` DISABLE KEYS */;
/*!40000 ALTER TABLE `tbltransaction` ENABLE KEYS */;


--
-- Definition of table `tbluser`
--

DROP TABLE IF EXISTS `tbluser`;
CREATE TABLE `tbluser` (
  `Id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `UserName` varchar(25) DEFAULT NULL,
  `Password` varchar(50) DEFAULT NULL,
  `EmailId` varchar(50) DEFAULT NULL,
  `SurName` varchar(100) DEFAULT NULL,
  `LastLogin` datetime DEFAULT NULL,
  `CreationTime` datetime DEFAULT NULL,
  `RoleId` int(11) DEFAULT NULL,
  `CanLogin` int(1) unsigned NOT NULL DEFAULT '0',
  `Status` int(3) DEFAULT '1',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=33 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tbluser`
--

/*!40000 ALTER TABLE `tbluser` DISABLE KEYS */;
/*!40000 ALTER TABLE `tbluser` ENABLE KEYS */;


--
-- Definition of table `tbluserdetails`
--

DROP TABLE IF EXISTS `tbluserdetails`;
CREATE TABLE `tbluserdetails` (
  `UserId` int(25) DEFAULT NULL,
  `Address` varchar(255) DEFAULT NULL,
  `DOB` varchar(30) DEFAULT NULL,
  `Phone` varchar(15) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tbluserdetails`
--

/*!40000 ALTER TABLE `tbluserdetails` DISABLE KEYS */;
/*!40000 ALTER TABLE `tbluserdetails` ENABLE KEYS */;


--
-- Definition of table `tblversion`
--

DROP TABLE IF EXISTS `tblversion`;
CREATE TABLE `tblversion` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) DEFAULT NULL,
  `Disc` varchar(500) DEFAULT NULL,
  `Number` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tblversion`
--

/*!40000 ALTER TABLE `tblversion` DISABLE KEYS */;
/*!40000 ALTER TABLE `tblversion` ENABLE KEYS */;




/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
