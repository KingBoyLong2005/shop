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
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `user_id` int NOT NULL AUTO_INCREMENT,
  `user_customer_admin_id` int DEFAULT NULL,
  `username` varchar(255) DEFAULT NULL,
  `password_hash` varchar(255) DEFAULT NULL,
  `role` enum('user','admin','superadmin') DEFAULT NULL,
  `active` tinyint(1) DEFAULT '1',
  PRIMARY KEY (`user_id`),
  UNIQUE KEY `username` (`username`),
  KEY `ibfk_users_admin_idx` (`user_customer_admin_id`),
  CONSTRAINT `ibfk_user_admin` FOREIGN KEY (`user_customer_admin_id`) REFERENCES `admins` (`admin_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `ibfk_user_customer` FOREIGN KEY (`user_customer_admin_id`) REFERENCES `customers` (`customer_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=187 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (116,1,'adminone','hashed_password1','admin',1),(117,2,'admintwo','hashed_password2','admin',1),(118,3,'adminthree','hashed_password3','admin',1),(119,4,'adminfour','hashed_password4','admin',1),(120,5,'adminfive','hashed_password5','admin',1),(121,6,'adminsix','hashed_password6','admin',1),(122,7,'adminseven','hashed_password7','admin',1),(123,8,'admineight','hashed_password8','admin',1),(124,9,'adminnine','hashed_password9','admin',1),(125,10,'adminten','hashed_password10','admin',1),(126,11,'customerone','hashed_password11','user',1),(127,12,'customertwo','hashed_password12','user',1),(128,13,'customerthree','hashed_password13','user',1),(129,14,'customerfour','hashed_password14','user',1),(130,15,'customerfive','hashed_password15','user',1),(131,16,'customersix','hashed_password16','user',1),(132,17,'customerseven','hashed_password17','user',1),(133,18,'customereight','hashed_password18','user',1),(134,19,'customernine','hashed_password19','user',1),(135,20,'customerten','hashed_password20','user',0),(136,21,'customer11','hashed_password21','user',1),(137,22,'customer12','hashed_password22','user',1),(138,23,'customer13','hashed_password23','user',1),(139,24,'customer14','hashed_password24','user',1),(140,25,'customer15','hashed_password25','user',1),(141,26,'customer16','hashed_password26','user',1),(142,27,'customer17','hashed_password27','user',1),(143,28,'customer18','hashed_password28','user',1),(144,29,'customer19','hashed_password29','user',1),(145,30,'customer20','hashed_password30','user',1),(146,31,'customer21','hashed_password31','user',1),(147,32,'customer22','hashed_password32','user',1),(148,33,'customer23','hashed_password33','user',1),(149,34,'customer24','hashed_password34','user',1),(150,35,'customer25','hashed_password35','user',1),(151,36,'customer26','hashed_password36','user',1),(152,37,'customer27','hashed_password37','user',1),(153,38,'customer28','hashed_password38','user',1),(154,39,'customer29','hashed_password39','user',1),(155,40,'customer30','hashed_password40','user',1),(156,41,'customer31','hashed_password41','user',1),(157,42,'customer32','hashed_password42','user',1),(158,43,'customer33','hashed_password43','user',1),(159,44,'customer34','hashed_password44','user',1),(160,45,'customer35','hashed_password45','user',1),(161,46,'customer36','hashed_password46','user',1),(162,47,'customer37','hashed_password47','user',1),(163,48,'customer38','hashed_password48','user',1),(164,49,'customer39','hashed_password49','user',1),(165,50,'customer40','hashed_password50','user',1),(176,1,'customernow','hashed_password1','user',1),(177,2,'customerba','hashed_password2','user',1),(178,3,'customerda','hashed_password3','user',1),(179,4,'customereq','hashed_password4','user',1),(180,5,'customerlk','hashed_password5','user',1),(181,6,'customerxcv','hashed_password6','user',1),(182,7,'customeroii','hashed_password7','user',1),(183,8,'customeradd','hashed_password8','user',1),(184,9,'customermmm','hashed_password9','user',1),(185,10,'customerpppp','hashed_password10','user',1),(186,1,'superadmin','superadmin123','superadmin',1);
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
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
