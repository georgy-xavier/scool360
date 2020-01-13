# MySQL-Front 5.0  (Build 1.0)

/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE */;
/*!40101 SET SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES */;
/*!40103 SET SQL_NOTES='ON' */;


# Host: localhost    Database: winschooldb
# ------------------------------------------------------
# Server version 5.1.30-community

CREATE DATABASE `winschooldb` /*!40100 DEFAULT CHARACTER SET latin1 */;
USE `winschooldb`;

#
# Table structure for table tblaccount
#

CREATE TABLE `tblaccount` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `AccountName` varchar(100) DEFAULT NULL,
  `Parent` int(11) DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;

#
# Table structure for table tblaction
#

CREATE TABLE `tblaction` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ActionName` varchar(35) DEFAULT NULL,
  `Description` varchar(500) DEFAULT NULL,
  `Link` varchar(30) DEFAULT NULL,
  `ActionType` varchar(30) DEFAULT NULL,
  `Order` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=50 DEFAULT CHARSET=latin1;

#
# Table structure for table tblbatch
#

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

#
# Table structure for table tblbloodgrp
#

CREATE TABLE `tblbloodgrp` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `GroupName` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=latin1;

#
# Table structure for table tblclass
#

CREATE TABLE `tblclass` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ClassName` varchar(100) DEFAULT NULL,
  `Standard` varchar(50) DEFAULT NULL,
  `ParentGroupID` int(11) DEFAULT NULL,
  `Status` int(5) DEFAULT '1',
  `Division` varchar(30) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Table structure for table tblclassroom
#

CREATE TABLE `tblclassroom` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `RoomNumber` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;

#
# Table structure for table tblclassschedule
#

CREATE TABLE `tblclassschedule` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ClassId` int(11) DEFAULT NULL,
  `BatchId` int(11) DEFAULT NULL,
  `ClassTeacherId` int(11) DEFAULT NULL,
  `ClassRoom` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Table structure for table tblclassstaffmap
#

CREATE TABLE `tblclassstaffmap` (
  `ClassId` int(11) DEFAULT NULL,
  `SubjectId` int(11) DEFAULT NULL,
  `StaffId` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Table structure for table tblclasssubmap
#

CREATE TABLE `tblclasssubmap` (
  `ClassId` int(11) DEFAULT NULL,
  `SubjectId` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Table structure for table tblconfiguration
#

CREATE TABLE `tblconfiguration` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Module` varchar(255) DEFAULT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `Value` varchar(255) DEFAULT NULL,
  `Disc` varchar(500) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

#
# Table structure for table tblcriteria
#

CREATE TABLE `tblcriteria` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Module` varchar(255) DEFAULT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `Value1` int(11) DEFAULT NULL,
  `Value2` int(11) DEFAULT NULL,
  `Condition` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Table structure for table tblexam
#

CREATE TABLE `tblexam` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ExamName` varchar(255) DEFAULT NULL,
  `ClassId` int(11) DEFAULT NULL,
  `CreationDate` datetime DEFAULT NULL,
  `Status` int(3) DEFAULT '1',
  `TypeId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Table structure for table tblexammark
#

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

#
# Table structure for table tblexamschedule
#

CREATE TABLE `tblexamschedule` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `BatchId` int(11) DEFAULT NULL,
  `ExamId` int(11) DEFAULT NULL,
  `Status` varchar(255) DEFAULT NULL,
  `ScheduledDate` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Table structure for table tblexamsubjectmap
#

CREATE TABLE `tblexamsubjectmap` (
  `ExamId` int(11) DEFAULT NULL,
  `SubjectId` int(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Table structure for table tblfeeaccount
#

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

#
# Table structure for table tblfeeasso
#

CREATE TABLE `tblfeeasso` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;

#
# Table structure for table tblfeebill
#

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

#
# Table structure for table tblfeefrequencytype
#

CREATE TABLE `tblfeefrequencytype` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FreequencyName` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=latin1;

#
# Table structure for table tblfeeschedule
#

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

#
# Table structure for table tblfeestudent
#

CREATE TABLE `tblfeestudent` (
  `Id` int(20) NOT NULL AUTO_INCREMENT,
  `SchId` int(20) DEFAULT NULL,
  `StudId` int(20) DEFAULT NULL,
  `Amount` double DEFAULT NULL,
  `BalanceAmount` double DEFAULT NULL,
  `Status` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Table structure for table tblfileurl
#

CREATE TABLE `tblfileurl` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FilePath` varchar(255) DEFAULT NULL,
  `Type` varchar(50) DEFAULT NULL,
  `UserId` int(50) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Table structure for table tblfine
#

CREATE TABLE `tblfine` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Amount` double DEFAULT NULL,
  `Frequency` double DEFAULT NULL,
  `Type` varchar(50) DEFAULT NULL,
  `FeeId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Table structure for table tblgrade
#

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

#
# Table structure for table tblgroup
#

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

#
# Table structure for table tblgrouprelation
#

CREATE TABLE `tblgrouprelation` (
  `ParentId` int(11) unsigned DEFAULT NULL,
  `ChildId` int(11) unsigned DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Table structure for table tblgrouptype
#

CREATE TABLE `tblgrouptype` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `TypeName` varchar(30) DEFAULT NULL,
  `Description` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;

#
# Table structure for table tblgroupusermap
#

CREATE TABLE `tblgroupusermap` (
  `GroupId` int(11) DEFAULT NULL,
  `UserId` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Table structure for table tbllanguage
#

CREATE TABLE `tbllanguage` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Language` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=latin1;

#
# Table structure for table tblmodule
#

CREATE TABLE `tblmodule` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ModuleName` varchar(30) DEFAULT NULL,
  `Discription` varchar(255) DEFAULT NULL,
  `MenuName` varchar(30) DEFAULT NULL,
  `Link` varchar(30) DEFAULT NULL,
  `Order` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=latin1;

#
# Table structure for table tblmoduleactionmap
#

CREATE TABLE `tblmoduleactionmap` (
  `ModuleId` int(11) unsigned DEFAULT NULL,
  `ActionId` int(11) unsigned DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Table structure for table tblperiod
#

CREATE TABLE `tblperiod` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Period` varchar(255) DEFAULT NULL,
  `FrequencyId` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=29 DEFAULT CHARSET=latin1;

#
# Table structure for table tblpreviousschooldata
#

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

#
# Table structure for table tblpromotionmap
#

CREATE TABLE `tblpromotionmap` (
  `StdFrom` varchar(100) DEFAULT NULL,
  `StdTo` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Table structure for table tblquatermonthmap
#

CREATE TABLE `tblquatermonthmap` (
  `PeriodId` int(11) DEFAULT NULL,
  `MonthId` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Table structure for table tblreligion
#

CREATE TABLE `tblreligion` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Religion` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=latin1;

#
# Table structure for table tblresignreason
#

CREATE TABLE `tblresignreason` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Reason` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

#
# Table structure for table tblresignuser
#

CREATE TABLE `tblresignuser` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` varchar(255) DEFAULT NULL,
  `ReasionId` int(11) DEFAULT NULL,
  `Discription` varchar(300) DEFAULT NULL,
  `ResignDate` date DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Table structure for table tblrole
#

CREATE TABLE `tblrole` (
  `Id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `RoleName` varchar(25) DEFAULT NULL,
  `Discription` varchar(255) DEFAULT NULL,
  `Type` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Table structure for table tblroleactionmap
#

CREATE TABLE `tblroleactionmap` (
  `RoleId` int(11) unsigned DEFAULT NULL,
  `ActionId` int(11) unsigned DEFAULT NULL,
  `ModuleId` int(11) unsigned DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Table structure for table tblschooldetails
#

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

#
# Table structure for table tblstaffdetails
#

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

#
# Table structure for table tblstaffsubjectmap
#

CREATE TABLE `tblstaffsubjectmap` (
  `StaffId` int(30) DEFAULT NULL,
  `SubjectId` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Table structure for table tblstandard
#

CREATE TABLE `tblstandard` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=latin1;

#
# Table structure for table tblstayingwith
#

CREATE TABLE `tblstayingwith` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `StayingWith` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=latin1;

#
# Table structure for table tblstudent
#

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

#
# Table structure for table tblstudentclassmap
#

CREATE TABLE `tblstudentclassmap` (
  `StudentId` int(30) DEFAULT NULL,
  `ClassId` int(11) DEFAULT NULL,
  `Standard` varchar(100) DEFAULT NULL,
  `BatchId` int(11) DEFAULT NULL,
  `RollNo` int(11) DEFAULT '-1',
  `Status` int(3) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Table structure for table tblstudentmark
#

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

#
# Table structure for table tblsubject_type
#

CREATE TABLE `tblsubject_type` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `sbject_type` varchar(255) DEFAULT NULL,
  `TypeDisc` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;

#
# Table structure for table tblsubjects
#

CREATE TABLE `tblsubjects` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `subject_name` varchar(255) DEFAULT NULL,
  `sub_description` varchar(255) DEFAULT NULL,
  `sub_Catagory` int(11) DEFAULT NULL,
  `SubjectCode` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Table structure for table tbltc
#

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

#
# Table structure for table tbltransaction
#

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

#
# Table structure for table tbluser
#

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

#
# Table structure for table tbluserdetails
#

CREATE TABLE `tbluserdetails` (
  `UserId` int(25) DEFAULT NULL,
  `Address` varchar(255) DEFAULT NULL,
  `DOB` varchar(30) DEFAULT NULL,
  `Phone` varchar(15) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Table structure for table tblversion
#

CREATE TABLE `tblversion` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) DEFAULT NULL,
  `Disc` varchar(500) DEFAULT NULL,
  `Number` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;

/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
