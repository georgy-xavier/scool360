# MySQL-Front 5.0  (Build 1.0)

/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE */;
/*!40101 SET SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES */;
/*!40103 SET SQL_NOTES='ON' */;


# Host: localhost    Database: centraldb
# ------------------------------------------------------
# Server version 5.5.56-log

USE `centraldb`;

#
# Table structure for table tblschool_list
#

DROP TABLE IF EXISTS `tblschool_list`;
CREATE TABLE `tblschool_list` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `SchoolName` varchar(100) DEFAULT NULL,
  `ConnectionString` varchar(255) DEFAULT NULL,
  `IsActice` tinyint(3) DEFAULT '0' COMMENT '1 means working active, 0 means not working',
  `DatabaseName` varchar(50) DEFAULT NULL,
  `IsEmail` tinyint(3) DEFAULT '0',
  `FilePath` varchar(50) DEFAULT NULL,
  `Sysversion` varchar(200) DEFAULT NULL,
  `Patchversion` varchar(200) DEFAULT NULL,
  `HostName` varchar(200) DEFAULT NULL,
  `UserName` varchar(200) DEFAULT NULL,
  `Password` varchar(200) DEFAULT NULL,
  `ExpireDate` varchar(200) DEFAULT NULL,
  `CustomerId` varchar(255) DEFAULT NULL,
  `OrganizationId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=latin1;
INSERT INTO `tblschool_list` VALUES (2,'Aradhana','Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=carmel1db;User=root;Password=Mysql#ceron111',0,'aradhanadb',0,'aradhanadb',NULL,NULL,'localhost','root','Mysql#ceron111',NULL,NULL,1);
INSERT INTO `tblschool_list` VALUES (3,'GreenVallye','Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=greenvallystatedb;User=root;Password=Mysql#ceron111',1,'greenvallystatedb',0,'aradyhana',NULL,NULL,'localhost','root','Mysql#ceron111',NULL,NULL,2);
INSERT INTO `tblschool_list` VALUES (4,'HAM Academy','Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=hamacademydb;User=root;Password=Mysql#ceron111',1,'hamacademydb',1,'ham',NULL,NULL,'localhost','root','Mysql#ceron111',NULL,NULL,1);
INSERT INTO `tblschool_list` VALUES (5,'Carmel','Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=carmeldb;User=root;Password=Mysql#ceron111',1,'carmeldb',0,'carmel',NULL,NULL,'localhost','root','Mysql#ceron111',NULL,NULL,2);
INSERT INTO `tblschool_list` VALUES (6,'Selvam','Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=selvamdb;User=root;Password=Mysql#ceron111',0,'selvamdb',0,'selvam',NULL,NULL,'localhost','root','Mysql#ceron111',NULL,NULL,1);
INSERT INTO `tblschool_list` VALUES (7,'Vishwajyothi PUC','Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=vishwajyothipucdb;User=root;Password=Mysql#ceron111',0,'vishwajyothipucdb',0,'Vispuc',NULL,NULL,'localhost','root','Mysql#ceron111',NULL,NULL,2);
INSERT INTO `tblschool_list` VALUES (8,'Green Valley ICSE','Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=greenvallyicsedb;User=root;Password=Mysql#ceron111',1,'greenvallyicsedb',0,'greenicse',NULL,NULL,'localhost','root','Mysql#ceron111',NULL,NULL,1);
INSERT INTO `tblschool_list` VALUES (9,'Green Valley State','Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=greenvallystatedb;User=root;Password=Mysql#ceron111',1,'greenvallystatedb',0,'greenstate',NULL,NULL,'localhost','root','Mysql#ceron111',NULL,NULL,2);
INSERT INTO `tblschool_list` VALUES (10,'Green valley CBSE','Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=greenvallycbsedb;User=root;Password=Mysql#ceron111',1,'greenvallycbsedb',0,'greencbse',NULL,NULL,'localhost','root','Mysql#ceron111',NULL,NULL,1);
INSERT INTO `tblschool_list` VALUES (12,'MGM','Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=freshdb;User=root;Password=Mysql#ceron111',0,'freshdb',0,'mgm',NULL,NULL,'localhost','root','Mysql#ceron111',NULL,NULL,2);
INSERT INTO `tblschool_list` VALUES (13,'Bolosing','Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=bolosing;User=root;Password=Mysql#ceron111',1,'bolosing',0,'Bolosing',NULL,NULL,'localhost','root','Mysql#ceron111',NULL,NULL,1);
INSERT INTO `tblschool_list` VALUES (14,'Kenz','Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=kenzdb2;User=root;Password=Mysql#ceron111',1,'kenzdb2',0,'Kenz',NULL,NULL,'localhost','root','Mysql#ceron111',NULL,NULL,2);
INSERT INTO `tblschool_list` VALUES (15,'Sinclairs','Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=sinclairsdb;User=root;Password=Mysql#ceron111',0,'sinclairsdb',0,'Sinclairs',NULL,NULL,'localhost','root','Mysql#ceron111',NULL,NULL,1);
INSERT INTO `tblschool_list` VALUES (16,'Testschool','Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=testschooldb;User=root;Password=Mysql#ceron111',0,'testschooldb',0,'Testschool',NULL,NULL,'localhost','root','Mysql#ceron111',NULL,NULL,2);
INSERT INTO `tblschool_list` VALUES (17,'HamKidz','Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=hamkidsdb;User=root;Password=Mysql#ceron111',0,'hamkidsdb',0,'Hamkidz',NULL,NULL,'localhost','root','Mysql#ceron111',NULL,NULL,1);
INSERT INTO `tblschool_list` VALUES (18,'Little Heart','Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=littleheartdb;User=root;Password=Mysql#ceron111',1,'littleheartdb',0,'littleheart',NULL,NULL,'localhost','root','Mysql#ceron111',NULL,NULL,2);
INSERT INTO `tblschool_list` VALUES (19,'Vishwajyothi','Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=vishwajyothidb;User=root;Password=Mysql#ceron111',0,'vishwajyothidb',0,'vishwajyothi',NULL,NULL,'localhost','root','Mysql#ceron111',NULL,NULL,1);
INSERT INTO `tblschool_list` VALUES (20,'Prince','Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=princedb;User=root;Password=Mysql#ceron111',1,'princedb',0,'prince',NULL,NULL,'localhost','root','Mysql#ceron111',NULL,NULL,2);
INSERT INTO `tblschool_list` VALUES (21,'ham','Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=hamacademydb1;User=root;Password=Mysql#ceron111',0,'hamacademydb1',0,'ham1',NULL,NULL,'localhost','root','Mysql#ceron111',NULL,NULL,1);
INSERT INTO `tblschool_list` VALUES (22,'st savio','Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=saintsaviodb;User=root;Password=Mysql#ceron111',1,'saintsaviodb',0,'savio',NULL,NULL,'localhost','root','Mysql#ceron111',NULL,NULL,2);
INSERT INTO `tblschool_list` VALUES (23,'ces','Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=cesdb;User=root;Password=Mysql#ceron111',0,'cesdb',0,'ces',NULL,NULL,'localhost','root','Mysql#ceron111',NULL,NULL,1);
INSERT INTO `tblschool_list` VALUES (24,'ham1','Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=hamdb;User=root;Password=Mysql#ceron111',0,'hamdb',0,'ham1',NULL,NULL,'localhost','root','Mysql#ceron111',NULL,NULL,2);
INSERT INTO `tblschool_list` VALUES (25,'gems','Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=gemsdb;User=root;Password=Mysql#ceron111',0,'gemsdb',0,'gems',NULL,NULL,'localhost','root','Mysql#ceron111',NULL,NULL,1);
INSERT INTO `tblschool_list` VALUES (26,'ces1','Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=cesdb1;User=root;Password=Mysql#ceron111',0,'cesdb1',0,'ces1',NULL,NULL,'localhost','root','Mysql#ceron111',NULL,NULL,2);
INSERT INTO `tblschool_list` VALUES (27,'vidhyashankari','Driver={MySQL ODBC 5.1 Driver};Server=localhost;Database=vidyashankaridb;User=root;Password=Mysql#ceron111',0,'vidyashankaridb',0,'vidyashankari',NULL,NULL,'localhost','root','Mysql#ceron111',NULL,NULL,1);

/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
