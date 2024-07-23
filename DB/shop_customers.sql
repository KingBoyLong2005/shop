CREATE DATABASE  IF NOT EXISTS `shop` /*!40100 DEFAULT CHARACTER SET utf8mb3 */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `shop`;
-- MySQL dump 10.13  Distrib 8.0.36, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: shop
-- ------------------------------------------------------
-- Server version	8.3.0

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `customers`
--

DROP TABLE IF EXISTS `customers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `customers` (
  `customer_id` int NOT NULL AUTO_INCREMENT,
  `customer_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `customer_phone_number` varchar(15) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `customer_address` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `customer_email` varchar(45) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `customer_gender` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `customer_dateofbirth` date DEFAULT NULL,
  `customer_count` int DEFAULT '0',
  `customer_totalspent` decimal(10,2) DEFAULT '0.00',
  `active` tinyint(1) DEFAULT '1',
  PRIMARY KEY (`customer_id`),
  UNIQUE KEY `customer_phone_number_UNIQUE` (`customer_phone_number`),
  UNIQUE KEY `customer_email_UNIQUE` (`customer_email`)
) ENGINE=InnoDB AUTO_INCREMENT=84 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `customers`
--

LOCK TABLES `customers` WRITE;
/*!40000 ALTER TABLE `customers` DISABLE KEYS */;
INSERT INTO `customers` VALUES (1,'Customer One','2345678901','123 Elm St','customerone@example.com','F','1990-01-01',0,0.00,1),(2,'Customer Two','2345678902','456 Oak St','customertwo@example.com','M','1991-02-02',0,0.00,1),(3,'Customer Three','2345678903','789 Pine St','customerthree@example.com','F','1992-03-03',0,0.00,1),(4,'Customer Four','2345678904','101 Maple St','customerfour@example.com','M','1993-04-04',0,0.00,1),(5,'Customer Five','2345678905','202 Birch St','customerfive@example.com','F','1994-05-05',0,0.00,1),(6,'Customer Six','2345678906','303 Cedar St','customersix@example.com','M','1995-06-06',0,0.00,1),(7,'Customer Seven','2345678907','404 Fir St','customerseven@example.com','F','1996-07-07',1,89990.00,1),(8,'Customer Eight','2345678908','505 Spruce St','customereight@example.com','M','1997-08-08',0,0.00,1),(9,'Customer Nine','2345678909','606 Redwood St','customernine@example.com','F','1998-09-09',0,0.00,1),(10,'Customer Ten','2345678910','707 Chestnut St','customerten@example.com','M','1999-10-10',0,0.00,1),(11,'Customer Eleven','2345678911','808 Poplar St','customer11@example.com','F','2000-11-11',0,0.00,1),(12,'Customer Twelve','2345678912','909 Willow St','customer12@example.com','M','2001-12-12',0,0.00,1),(13,'Customer Thirteen','2345678913','1010 Sycamore St','customer13@example.com','F','2002-01-01',0,0.00,1),(14,'Customer Fourteen','2345678914','1111 Elm St','customer14@example.com','M','2003-02-02',0,0.00,1),(15,'Customer helllo','23456734','1212 Oak St','customer15@example.com','F','2004-03-03',0,0.00,1),(16,'Customer Sixteen','2345678916','1313 Pine St','customer16@example.com','M','2005-04-04',0,0.00,1),(17,'Customer Seventeen','2345678917','1414 Maple St','customer17@example.com','F','2006-05-05',0,0.00,1),(18,'Customer Eighteen','2345678918','1515 Birch St','customer18@example.com','M','2007-06-06',0,0.00,1),(19,'Customer Nineteen','2345678919','1616 Cedar St','customer19@example.com','F','2008-07-07',0,0.00,1),(20,'Customer Twenty','2345678920','1717 Fir St','customer20@example.com','M','2009-08-08',0,0.00,0),(21,'Customer Twenty-One','2345678921','1818 Spruce St','customer21@example.com','F','2010-09-09',0,0.00,1),(22,'Customer Twenty-Two','2345678922','1919 Redwood St','customer22@example.com','M','2011-10-10',0,0.00,1),(23,'Customer Twenty-Three','2345678923','2020 Chestnut St','customer23@example.com','F','2012-11-11',1,80991.00,1),(24,'Customer Twenty-Four','2345678924','2121 Poplar St','customer24@example.com','M','2013-12-12',0,0.00,1),(25,'Customer Twenty-Five','2345678925','2222 Willow St','customer25@example.com','F','2014-01-01',0,0.00,1),(26,'Customer Twenty-Six','2345678926','2323 Sycamore St','customer26@example.com','M','2015-02-02',0,0.00,1),(27,'Customer Twenty-Seven','2345678927','2424 Elm St','customer27@example.com','F','2016-03-03',0,0.00,1),(28,'Customer Twenty-Eight','2345678928','2525 Oak St','customer28@example.com','M','2017-04-04',0,0.00,1),(29,'Customer Twenty-Nine','2345678929','2626 Pine St','customer29@example.com','F','2018-05-05',0,0.00,1),(30,'Customer Thirty','2345678930','2727 Maple St','customer30@example.com','M','2019-06-06',0,0.00,1),(31,'Customer Thirty-One','2345678931','2828 Birch St','customer31@example.com','F','2020-07-07',0,0.00,1),(32,'Customer Thirty-Two','2345678932','2929 Cedar St','customer32@example.com','M','2021-08-08',0,0.00,1),(33,'Customer Thirty-Three','2345678933','3030 Fir St','customer33@example.com','F','2022-09-09',0,0.00,1),(34,'Customer Thirty-Four','2345678934','3131 Spruce St','customer34@example.com','M','2023-10-10',0,0.00,1),(35,'Customer Thirty-Five','2345678935','3232 Redwood St','customer35@example.com','F','2024-11-11',0,0.00,1),(36,'Customer Thirty-Six','2345678936','3333 Chestnut St','customer36@example.com','M','2025-12-12',0,0.00,1),(37,'Customer Thirty-Seven','2345678937','3434 Poplar St','customer37@example.com','F','2026-01-01',0,0.00,1),(38,'Customer Thirty-Eight','2345678938','3535 Willow St','customer38@example.com','M','2027-02-02',0,0.00,1),(39,'Customer Thirty-Nine','2345678939','3636 Sycamore St','customer39@example.com','F','2028-03-03',0,0.00,1),(40,'Customer Forty','2345678940','3737 Elm St','customer40@example.com','M','2029-04-04',0,0.00,1),(41,'Customer Forty-One','2345678941','3838 Oak St','customer41@example.com','F','2030-05-05',0,0.00,1),(42,'Customer Forty-Two','2345678942','3939 Pine St','customer42@example.com','M','2031-06-06',0,0.00,1),(43,'Customer Forty-Three','2345678943','4040 Maple St','customer43@example.com','F','2032-07-07',0,0.00,1),(44,'Customer Forty-Four','2345678944','4141 Birch St','customer44@example.com','M','2033-08-08',0,0.00,1),(45,'Customer Forty-Five','2345678945','4242 Cedar St','customer45@example.com','F','2034-09-09',0,0.00,1),(46,'Customer Forty-Six','2345678946','4343 Fir St','customer46@example.com','M','2035-10-10',0,0.00,1),(47,'Customer Forty-Seven','2345678947','4444 Spruce St','customer47@example.com','F','2036-11-11',0,0.00,1),(48,'Customer Forty-Eight','2345678948','4545 Redwood St','customer48@example.com','M','2037-12-12',0,0.00,1),(49,'Customer Forty-Nine','2345678949','4646 Chestnut St','customer49@example.com','F','2038-01-01',0,0.00,1),(50,'Customer Fifty','2345678950','4747 Poplar St','customer50@example.com','M','2039-02-02',0,0.00,1);
/*!40000 ALTER TABLE `customers` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-07-23 12:32:21
