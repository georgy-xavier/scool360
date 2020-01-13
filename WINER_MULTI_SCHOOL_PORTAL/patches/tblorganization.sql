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
# Table structure for table tblorganization
#

DROP TABLE IF EXISTS `tblorganization`;
CREATE TABLE `tblorganization` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) DEFAULT NULL,
  `Address` varchar(255) DEFAULT NULL,
  `Logo` longblob,
  `Status` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;
INSERT INTO `tblorganization` VALUES (1,'MGM Group Of Institutions','Bangalore',x'FFD8FFE0','1');
INSERT INTO `tblorganization` VALUES (2,'Carmel Group ','Bangalore',NULL,'1');

/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
