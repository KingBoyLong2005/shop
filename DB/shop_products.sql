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
-- Table structure for table `products`
--

DROP TABLE IF EXISTS `products`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `products` (
  `product_id` int NOT NULL AUTO_INCREMENT,
  `product_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `product_stock_quantity` int DEFAULT NULL,
  `product_price` decimal(10,2) DEFAULT NULL,
  `product_category_id` int DEFAULT NULL,
  `product_brand` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `is_active` tinyint(1) DEFAULT '1',
  PRIMARY KEY (`product_id`),
  KEY `product_category_id` (`product_category_id`),
  CONSTRAINT `products_ibfk_1` FOREIGN KEY (`product_category_id`) REFERENCES `categories` (`category_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=89 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `products`
--

LOCK TABLES `products` WRITE;
/*!40000 ALTER TABLE `products` DISABLE KEYS */;
INSERT INTO `products` VALUES (29,'iPhone 14',50,999.99,1,'Apple',0),(30,'Samsung Galaxy S22',21,8999.00,1,'Samsung',1),(31,'Google Pixel 7',30,699.99,1,'Google',1),(32,'OnePlus 10',25,749.99,1,'OnePlus',1),(33,'Dell XPS 13',20,1299.99,2,'Dell',1),(34,'MacBook Pro',15,2399.99,2,'Apple',1),(35,'HP Spectre x360',10,1399.99,2,'HP',1),(36,'Lenovo ThinkPad X1',18,1599.99,2,'Lenovo',1),(37,'Apple iPad Air',31,599.99,3,'Apple',1),(38,'Samsung Galaxy Tab S8',16,649.99,3,'Samsung',1),(39,'Microsoft Surface Pro 8',17,999.99,3,'Microsoft',1),(40,'Lenovo Tab P11',28,349.99,3,'Lenovo',1),(41,'Apple Watch Series 8',30,399.99,4,'Apple',1),(42,'Samsung Galaxy Watch 5',25,349.99,4,'Samsung',1),(43,'Garmin Forerunner 945',16,599.99,4,'Garmin',1),(44,'Fitbit Versa 4',16,229.99,4,'Fitbit',1),(45,'LG OLED C1',8,1499.99,5,'LG',1),(46,'Samsung QN90B',10,1299.99,5,'Samsung',1),(47,'Sony Bravia X90J',14,1099.99,5,'Sony',1),(48,'TCL 6-Series',16,899.99,5,'TCL',1),(49,'Canon EOS R5',8,3899.99,6,'Canon',1),(50,'Nikon Z6 II',10,1999.99,6,'Nikon',1),(51,'Sony Alpha A7 IV',7,2499.99,6,'Sony',1),(52,'Fujifilm X-T4',12,1699.99,6,'Fujifilm',1),(53,'Sony WH-1000XM4',45,349.99,7,'Sony',1),(54,'Bose QuietComfort 35 II',50,299.99,7,'Bose',1),(55,'Apple AirPods Max',30,549.99,7,'Apple',1),(56,'Sennheiser Momentum 3',25,399.99,7,'Sennheiser',1),(57,'JBL Charge 5',40,149.99,8,'JBL',1),(58,'Bose SoundLink Revolve',35,229.99,8,'Bose',1),(59,'Sonos One',28,199.99,8,'Sonos',1),(60,'Ultimate Ears MEGABOOM 3',32,179.99,8,'Ultimate Ears',1),(61,'PlayStation 5',22,499.99,9,'Sony',1),(62,'Xbox Series X',20,499.99,9,'Microsoft',1),(63,'Nintendo Switch',30,299.99,9,'Nintendo',1),(64,'Steam Deck',18,399.99,9,'Valve',1),(65,'HP OfficeJet Pro 9015',15,229.99,10,'HP',1),(66,'Canon PIXMA TR8620',20,179.99,10,'Canon',1),(67,'Epson EcoTank ET-2760',12,299.99,10,'Epson',1),(68,'Brother MFC-L3770CDW',10,349.99,10,'Brother',1),(69,'Netgear Nighthawk RAX80',25,399.99,11,'Netgear',1),(70,'Asus RT-AX86U',20,299.99,11,'Asus',1),(71,'TP-Link Archer AX73',30,249.99,11,'TP-Link',1),(72,'Linksys MR9600',18,349.99,11,'Linksys',1),(73,'Dell UltraSharp U2720Q',28,499.99,12,'Dell',1),(74,'LG 27GN950-B',22,699.99,12,'LG',1),(75,'ASUS ProArt PA32UCX',15,1299.99,12,'ASUS',1),(76,'Acer Predator X34',18,799.99,12,'Acer',1),(77,'Corsair K95 RGB Platinum',35,199.99,13,'Corsair',1),(78,'Razer BlackWidow V3',40,159.99,13,'Razer',1),(79,'Logitech MX Keys',30,129.99,13,'Logitech',1),(80,'SteelSeries Apex Pro',25,179.99,13,'SteelSeries',1),(81,'Logitech MX Master 3',45,99.99,14,'Logitech',1),(82,'Razer DeathAdder V2',50,69.99,14,'Razer',1),(83,'SteelSeries Rival 3',30,39.99,14,'SteelSeries',1),(84,'Corsair Dark Core RGB',25,89.99,14,'Corsair',1),(85,'WD My Passport 4TB',20,129.99,15,'Western Digital',1),(86,'Seagate Backup Plus 5TB',25,149.99,15,'Seagate',1),(87,'Samsung T7 Touch 1TB',15,199.99,15,'Samsung',1),(88,'LaCie Rugged 2TB',18,169.99,15,'LaCie',1);
/*!40000 ALTER TABLE `products` ENABLE KEYS */;
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
