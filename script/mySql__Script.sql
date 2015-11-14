CREATE DATABASE  IF NOT EXISTS `ga` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `ga`;
-- MySQL dump 10.13  Distrib 5.6.24, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: ga
-- ------------------------------------------------------
-- Server version	5.6.27-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `basecollectionitem`
--

DROP TABLE IF EXISTS `basecollectionitem`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `basecollectionitem` (
  `TableId` int(11) NOT NULL AUTO_INCREMENT,
  `Id` char(64) DEFAULT NULL,
  `Data` blob,
  `ItemProcessed` tinyint(1) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  PRIMARY KEY (`TableId`),
  UNIQUE KEY `TableId_UNIQUE` (`TableId`),
  UNIQUE KEY `Id_UNIQUE` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=137 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `basecollectionitem`
--


--
-- Table structure for table `collectionitemqueue`
--

DROP TABLE IF EXISTS `collectionitemqueue`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `collectionitemqueue` (
  `ItemID` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `ItemTitle` varchar(128) DEFAULT NULL,
  `ItemUrl` varchar(256) DEFAULT NULL,
  `ItemDescription` varchar(256) DEFAULT NULL,
  `ItemTags` varchar(256) DEFAULT NULL,
  `ItemProcessed` bit(1) DEFAULT NULL,
  PRIMARY KEY (`ItemID`),
  UNIQUE KEY `ItemID_UNIQUE` (`ItemID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `collectionitemqueue`
--

LOCK TABLES `collectionitemqueue` WRITE;
/*!40000 ALTER TABLE `collectionitemqueue` DISABLE KEYS */;
INSERT INTO `collectionitemqueue` VALUES (1,'RethinkDB','http://rethinkdb.com/','A really cool nosql db which updates its clients in realtime.','database,nosql, open source','\0'),(2,'MongoDB','http://www.mongodb.com','Another nosql database that stores data as JSON','database, nosql, open source','\0'),(3,'SQL Datareader Read','https://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqldatareader.read%28v=vs.110%29.aspx','Comprehensive tutorial on SQL Data Read','.net,sql,ado','\0');
/*!40000 ALTER TABLE `collectionitemqueue` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2015-11-10 23:12:14
