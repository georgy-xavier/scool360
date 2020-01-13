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
# Table structure for table tbluser
#

DROP TABLE IF EXISTS `tbluser`;
CREATE TABLE `tbluser` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) DEFAULT NULL,
  `username` varchar(255) DEFAULT NULL,
  `status` int(11) DEFAULT NULL,
  `actionrights` int(11) DEFAULT NULL,
  `createtime` varchar(255) DEFAULT NULL,
  `GmailId` varchar(255) DEFAULT NULL,
  `OrganizationId` int(11) DEFAULT NULL,
  `password` varchar(50) DEFAULT NULL,
  `CanLogin` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=latin1;
INSERT INTO `tbluser` VALUES (3,'georgy','Georgy Xavier',1,1,' 2017-06-09T16:00:44','georgy@winceron.com',1,'KM/S1Bh3rU4kakmeDDIYPw==',1);
INSERT INTO `tbluser` VALUES (4,'georgy','Georgy Xavier',0,1,' 2017-06-09T16:10:03','georgy@winceron.com',0,'KM/S1Bh3rU4kakmeDDIYPw==',1);
INSERT INTO `tbluser` VALUES (5,'amal','amal',1,2,' 2017-06-09T16:10:28','amal@winceron.com',2,'7M+fTNyY/dCvPFEWRrpeTA==',1);

/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
