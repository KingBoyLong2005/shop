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
-- Table structure for table `admins`
--

DROP TABLE IF EXISTS `admins`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `admins` (
  `admin_id` int NOT NULL AUTO_INCREMENT,
  `admin_name` varchar(255) DEFAULT NULL,
  `admin_phone` varchar(255) DEFAULT NULL,
  `admin_email` varchar(45) DEFAULT NULL,
  `admin_gender` varchar(10) DEFAULT NULL,
  `active` tinyint(1) DEFAULT '1',
  PRIMARY KEY (`admin_id`),
  UNIQUE KEY `admin_name` (`admin_name`)
) ENGINE=InnoDB AUTO_INCREMENT=32 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `admins`
--

LOCK TABLES `admins` WRITE;
/*!40000 ALTER TABLE `admins` DISABLE KEYS */;
INSERT INTO `admins` VALUES (1,'HI','321','add','fgf',1),(31,'hello','123','fg','hgfd',1);
/*!40000 ALTER TABLE `admins` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cart`
--

DROP TABLE IF EXISTS `cart`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cart` (
  `cart_id` int NOT NULL AUTO_INCREMENT,
  `cart_customer_id` int DEFAULT NULL,
  `cart_product_id` int DEFAULT NULL,
  `cart_product_price` decimal(10,2) DEFAULT NULL,
  `cart_order_price` decimal(10,2) DEFAULT NULL,
  `cart_total_products` int DEFAULT NULL,
  `active` tinyint(1) DEFAULT '1',
  PRIMARY KEY (`cart_id`),
  KEY `cart_product_id` (`cart_product_id`),
  KEY `cart_ibfk_1_idx` (`cart_customer_id`),
  CONSTRAINT `cart_ibfk_1` FOREIGN KEY (`cart_customer_id`) REFERENCES `customers` (`customer_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `cart_ibfk_2` FOREIGN KEY (`cart_product_id`) REFERENCES `products` (`product_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=29 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cart`
--

LOCK TABLES `cart` WRITE;
/*!40000 ALTER TABLE `cart` DISABLE KEYS */;
INSERT INTO `cart` VALUES (14,31,30,0.00,0.00,0,0),(15,31,38,0.00,0.00,0,0),(18,32,31,0.00,0.00,0,1),(19,34,30,0.00,0.00,0,1),(20,34,43,0.00,0.00,0,1),(21,42,44,0.00,0.00,0,1),(22,42,42,0.00,0.00,0,1),(23,58,30,0.00,0.00,0,1),(24,58,30,0.00,0.00,0,1),(25,58,30,0.00,0.00,0,1),(26,58,36,0.00,0.00,0,1),(27,69,30,0.00,0.00,0,1),(28,69,30,0.00,0.00,0,1);
/*!40000 ALTER TABLE `cart` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `categories`
--

DROP TABLE IF EXISTS `categories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `categories` (
  `category_id` int NOT NULL AUTO_INCREMENT,
  `category_name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `category_description` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `active` tinyint(1) DEFAULT '1',
  PRIMARY KEY (`category_id`)
) ENGINE=InnoDB AUTO_INCREMENT=333 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `categories`
--

LOCK TABLES `categories` WRITE;
/*!40000 ALTER TABLE `categories` DISABLE KEYS */;
INSERT INTO `categories` VALUES (1,'Smartphones','Devices that combine a mobile phone with a handheld computer, typically offering Internet access, data storage, email capability, etc.',1),(2,'Laptops','Portable computers with a screen and alphanumeric keyboard.',1),(3,'Tablets','Mobile devices with a touchscreen display, designed for portability.',1),(4,'Smartwatches','Wearable devices that offer smartphone-like functionalities.',1),(5,'Televisions','Electronic devices used for viewing multimedia content.',1),(6,'Cameras','Devices for capturing photographs and videos.',1),(7,'Headphones','Audio devices worn on or around the head over a user\'s ears.',1),(8,'Speakers','Devices that convert electrical audio signals into sound.',1),(9,'Gaming Consoles','Devices designed primarily for playing video games.',1),(10,'Printers','Devices that produce a hard copy of documents stored in electronic form.',1),(11,'Routers','Devices that forward data packets along networks.',1),(12,'Monitors','Output devices that display information in pictorial form.',1),(13,'Keyboards','Input devices used to input text, characters, and other commands into a computer.',1),(14,'Mice','Pointing devices used to interact with a computer.',1),(15,'External Hard Drives','Devices used for storing and retrieving digital information.',1);
/*!40000 ALTER TABLE `categories` ENABLE KEYS */;
UNLOCK TABLES;

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
) ENGINE=InnoDB AUTO_INCREMENT=83 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `customers`
--

LOCK TABLES `customers` WRITE;
/*!40000 ALTER TABLE `customers` DISABLE KEYS */;
INSERT INTO `customers` VALUES (31,'Long','123456','QN','ki','Male','2005-06-13',2,2849.96,1),(33,'John Doe','123-456-0001','123 Elm Street, Springfield','johndoe1@example.com','M','1985-06-15',0,0.00,1),(34,'Jane Smith','123-456-0002','456 Oak Avenue, Springfield','janesmith2@example.com','F','1990-07-22',0,0.00,1),(35,'Alice Johnson','123-456-0003','789 Pine Road, Springfield','alicejohnson3@example.com','F','1988-03-11',0,0.00,1),(36,'Bob Brown','123-456-0004','101 Maple Drive, Springfield','bobbrown4@example.com','M','1979-11-30',0,0.00,1),(37,'Carol Davis','123-456-0005','202 Birch Lane, Springfield','caroldavis5@example.com','F','1995-01-05',0,0.00,1),(38,'David Wilson','123-456-0006','303 Cedar Street, Springfield','davidwilson6@example.com','M','1982-08-18',0,0.00,1),(39,'Emily Clark','123-456-0007','404 Walnut Way, Springfield','emilyclark7@example.com','F','1993-10-27',0,0.00,1),(40,'Frank Harris','123-456-0008','505 Cherry Hill, Springfield','frankharris8@example.com','M','1987-02-09',0,0.00,1),(41,'Grace Lewis','123-456-0009','606 Spruce Road, Springfield','gracelewis9@example.com','F','1991-09-14',0,0.00,1),(42,'Henry Walker','123-456-0010','707 Ash Street, Springfield','henrywalker10@example.com','M','1986-12-21',0,0.00,1),(43,'Irene Martinez','123-456-0011','808 Maple Street, Springfield','irenemartinez11@example.com','F','1989-04-17',0,0.00,1),(44,'James Thompson','123-456-0012','909 Elm Avenue, Springfield','jamesthompson12@example.com','M','1992-05-30',0,0.00,1),(45,'Katherine White','123-456-0013','1010 Oak Road, Springfield','katherinewhite13@example.com','F','1984-06-25',0,0.00,1),(46,'Liam Anderson','123-456-0014','1111 Birch Lane, Springfield','liamanderson14@example.com','M','1987-07-15',0,0.00,1),(47,'Megan Hall','123-456-0015','1212 Pine Hill, Springfield','meganhall15@example.com','F','1993-08-22',0,0.00,1),(48,'Nathan Young','123-456-0016','1313 Cedar Drive, Springfield','nathanyoung16@example.com','M','1986-09-09',0,0.00,1),(49,'Olivia King','123-456-0017','1414 Spruce Street, Springfield','oliviaking17@example.com','F','1991-10-13',0,0.00,1),(50,'Patrick Scott','123-456-0018','1515 Cherry Avenue, Springfield','patrickscott18@example.com','M','1985-11-29',0,0.00,1),(51,'Quinn Adams','123-456-0019','1616 Walnut Road, Springfield','quinnadams19@example.com','F','1992-01-04',0,0.00,1),(52,'Rachel Turner','123-456-0020','1717 Maple Hill, Springfield','rachelturner20@example.com','F','1994-03-20',0,0.00,1),(53,'Samuel Lee','123-456-0021','1818 Oak Lane, Springfield','samuellee21@example.com','M','1983-04-11',0,0.00,1),(54,'Tina Parker','123-456-0022','1919 Birch Street, Springfield','tinaparker22@example.com','F','1987-06-16',0,0.00,1),(55,'Ulysses Carter','123-456-0023','2020 Pine Drive, Springfield','ulyssescarter23@example.com','M','1989-07-25',0,0.00,1),(56,'Vera Cooper','123-456-0024','2121 Cedar Avenue, Springfield','veracooper24@example.com','F','1990-09-03',0,0.00,1),(57,'Walter Davis','123-456-0025','2222 Spruce Hill, Springfield','walterdavis25@example.com','M','1985-10-17',0,0.00,1),(58,'Xena Mitchell','123-456-0026','2323 Cherry Street, Springfield','xenamitchell26@example.com','F','1994-07-30',0,0.00,1),(59,'Yvonne Nelson','123-456-0027','2424 Walnut Lane, Springfield','yvonnenelson27@example.com','F','1988-01-25',0,0.00,1),(60,'Zachary Robinson','123-456-0028','2525 Maple Road, Springfield','zacharyrobinson28@example.com','M','1991-02-14',0,0.00,1),(61,'Anna Evans','123-456-0029','2626 Oak Street, Springfield','annaevans29@example.com','F','1992-03-18',0,0.00,1),(62,'Brian Lee','123-456-0030','2727 Birch Avenue, Springfield','brianlee30@example.com','M','1990-04-29',0,0.00,1),(63,'Catherine Moore','123-456-0031','2828 Pine Road, Springfield','catherinemoore31@example.com','F','1986-06-10',0,0.00,1),(64,'Daniel Taylor','123-456-0032','2929 Cedar Hill, Springfield','danieltaylor32@example.com','M','1989-07-20',0,0.00,1),(65,'Ella White','123-456-0033','3030 Spruce Drive, Springfield','ellawhite33@example.com','F','1993-08-14',0,0.00,1),(66,'Franklin Harris','123-456-0034','3131 Cherry Avenue, Springfield','franklinharris34@example.com','M','1985-09-22',0,0.00,1),(67,'Gabrielle Thompson','123-456-0035','3232 Walnut Street, Springfield','gabriellethompson35@example.com','F','1991-10-05',0,0.00,1),(68,'Harrison King','123-456-0036','3333 Maple Lane, Springfield','harrisonking36@example.com','M','1987-11-11',0,0.00,1),(69,'Isabella Young','123-456-0037','3434 Oak Hill, Springfield','isabellayoung37@example.com','F','1990-12-29',0,0.00,1),(70,'Jack Scott','123-456-0038','3535 Birch Drive, Springfield','jackscott38@example.com','M','1994-01-23',0,0.00,1),(71,'Kelsey Adams','123-456-0039','3636 Pine Street, Springfield','kelseyadams39@example.com','F','1988-02-11',0,0.00,1),(72,'Leo Walker','123-456-0040','3737 Cedar Road, Springfield','leowalker40@example.com','M','1992-03-07',0,0.00,1),(73,'Molly Wright','123-456-0041','3838 Spruce Lane, Springfield','mollywright41@example.com','F','1985-04-19',0,0.00,1),(74,'Nolan Johnson','123-456-0042','3939 Cherry Hill, Springfield','nolanjohnson42@example.com','M','1989-05-29',0,0.00,1),(75,'Olive Smith','123-456-0043','4040 Walnut Avenue, Springfield','olivesmith43@example.com','F','1993-06-14',0,0.00,1),(76,'Paul Martinez','123-456-0044','4141 Maple Street, Springfield','paulmartinez44@example.com','M','1988-07-01',0,0.00,1),(77,'Quinn Lee','123-456-0045','4242 Oak Avenue, Springfield','quinnlee45@example.com','F','1994-08-09',0,0.00,1),(78,'Rachel Davis','123-456-0046','4343 Birch Road, Springfield','racheldavis46@example.com','F','1990-09-15',0,0.00,1),(79,'Steven Wilson','123-456-0047','4444 Pine Hill, Springfield','stevenwilson47@example.com','M','1987-10-28',0,0.00,1),(80,'Tina Clark','123-456-0048','4545 Cedar Lane, Springfield','tinaclark48@example.com','F','1993-11-04',0,0.00,1),(81,'Ursula Robinson','123-456-0049','4646 Spruce Avenue, Springfield','ursularobinson49@example.com','F','1985-12-12',0,0.00,1),(82,'Victor Martinez','123-456-0050','4747 Cherry Street, Springfield','victormartinez50@example.com','M','1992-01-20',0,0.00,1);
/*!40000 ALTER TABLE `customers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `orders`
--

DROP TABLE IF EXISTS `orders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `orders` (
  `order_id` int NOT NULL AUTO_INCREMENT,
  `order_customer_id` int DEFAULT NULL,
  `order_quantity` int DEFAULT NULL,
  `order_product_id` int DEFAULT NULL,
  `order_total_price` decimal(10,2) DEFAULT NULL,
  `order_payment_method` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `order_status` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `order_delivery_address` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`order_id`),
  KEY `orders_ibfk_1_idx` (`order_product_id`)
) ENGINE=InnoDB AUTO_INCREMENT=54 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `orders`
--

LOCK TABLES `orders` WRITE;
/*!40000 ALTER TABLE `orders` DISABLE KEYS */;
INSERT INTO `orders` VALUES (52,31,3,38,1949.97,'Tm','Pending','QN'),(53,31,1,30,899.99,'TM','Pending','QN');
/*!40000 ALTER TABLE `orders` ENABLE KEYS */;
UNLOCK TABLES;

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
INSERT INTO `products` VALUES (29,'iPhone 14',50,999.99,1,'Apple',0),(30,'Samsung Galaxy S22',39,899.99,1,'Samsung',1),(31,'Google Pixel 7',30,699.99,1,'Google',1),(32,'OnePlus 10',25,749.99,1,'OnePlus',1),(33,'Dell XPS 13',20,1299.99,2,'Dell',1),(34,'MacBook Pro',15,2399.99,2,'Apple',1),(35,'HP Spectre x360',10,1399.99,2,'HP',1),(36,'Lenovo ThinkPad X1',18,1599.99,2,'Lenovo',1),(37,'Apple iPad Air',35,599.99,3,'Apple',1),(38,'Samsung Galaxy Tab S8',19,649.99,3,'Samsung',1),(39,'Microsoft Surface Pro 8',17,999.99,3,'Microsoft',1),(40,'Lenovo Tab P11',28,349.99,3,'Lenovo',1),(41,'Apple Watch Series 8',30,399.99,4,'Apple',1),(42,'Samsung Galaxy Watch 5',25,349.99,4,'Samsung',1),(43,'Garmin Forerunner 945',20,599.99,4,'Garmin',1),(44,'Fitbit Versa 4',18,229.99,4,'Fitbit',1),(45,'LG OLED C1',12,1499.99,5,'LG',1),(46,'Samsung QN90B',10,1299.99,5,'Samsung',1),(47,'Sony Bravia X90J',14,1099.99,5,'Sony',1),(48,'TCL 6-Series',16,899.99,5,'TCL',1),(49,'Canon EOS R5',8,3899.99,6,'Canon',1),(50,'Nikon Z6 II',10,1999.99,6,'Nikon',1),(51,'Sony Alpha A7 IV',7,2499.99,6,'Sony',1),(52,'Fujifilm X-T4',12,1699.99,6,'Fujifilm',1),(53,'Sony WH-1000XM4',45,349.99,7,'Sony',1),(54,'Bose QuietComfort 35 II',50,299.99,7,'Bose',1),(55,'Apple AirPods Max',30,549.99,7,'Apple',1),(56,'Sennheiser Momentum 3',25,399.99,7,'Sennheiser',1),(57,'JBL Charge 5',40,149.99,8,'JBL',1),(58,'Bose SoundLink Revolve',35,229.99,8,'Bose',1),(59,'Sonos One',28,199.99,8,'Sonos',1),(60,'Ultimate Ears MEGABOOM 3',32,179.99,8,'Ultimate Ears',1),(61,'PlayStation 5',22,499.99,9,'Sony',1),(62,'Xbox Series X',20,499.99,9,'Microsoft',1),(63,'Nintendo Switch',30,299.99,9,'Nintendo',1),(64,'Steam Deck',18,399.99,9,'Valve',1),(65,'HP OfficeJet Pro 9015',15,229.99,10,'HP',1),(66,'Canon PIXMA TR8620',20,179.99,10,'Canon',1),(67,'Epson EcoTank ET-2760',12,299.99,10,'Epson',1),(68,'Brother MFC-L3770CDW',10,349.99,10,'Brother',1),(69,'Netgear Nighthawk RAX80',25,399.99,11,'Netgear',1),(70,'Asus RT-AX86U',20,299.99,11,'Asus',1),(71,'TP-Link Archer AX73',30,249.99,11,'TP-Link',1),(72,'Linksys MR9600',18,349.99,11,'Linksys',1),(73,'Dell UltraSharp U2720Q',28,499.99,12,'Dell',1),(74,'LG 27GN950-B',22,699.99,12,'LG',1),(75,'ASUS ProArt PA32UCX',15,1299.99,12,'ASUS',1),(76,'Acer Predator X34',18,799.99,12,'Acer',1),(77,'Corsair K95 RGB Platinum',35,199.99,13,'Corsair',1),(78,'Razer BlackWidow V3',40,159.99,13,'Razer',1),(79,'Logitech MX Keys',30,129.99,13,'Logitech',1),(80,'SteelSeries Apex Pro',25,179.99,13,'SteelSeries',1),(81,'Logitech MX Master 3',45,99.99,14,'Logitech',1),(82,'Razer DeathAdder V2',50,69.99,14,'Razer',1),(83,'SteelSeries Rival 3',30,39.99,14,'SteelSeries',1),(84,'Corsair Dark Core RGB',25,89.99,14,'Corsair',1),(85,'WD My Passport 4TB',20,129.99,15,'Western Digital',1),(86,'Seagate Backup Plus 5TB',25,149.99,15,'Seagate',1),(87,'Samsung T7 Touch 1TB',15,199.99,15,'Samsung',1),(88,'LaCie Rugged 2TB',18,169.99,15,'LaCie',1);
/*!40000 ALTER TABLE `products` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `user_id` int NOT NULL AUTO_INCREMENT,
  `user_customer_id` int DEFAULT NULL,
  `username` varchar(255) DEFAULT NULL,
  `password_hash` varchar(255) DEFAULT NULL,
  `role` enum('user','admin','superadmin') DEFAULT NULL,
  `active` tinyint(1) DEFAULT '1',
  PRIMARY KEY (`user_id`),
  UNIQUE KEY `username` (`username`)
) ENGINE=InnoDB AUTO_INCREMENT=115 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (10,31,'lolo','koko','user',1),(11,1,'admin','admin123','admin',1),(12,31,'admin1','admin456','admin',1),(13,1,'superadmin','superadmin123','superadmin',1),(66,33,'janesmith2','hashed_password_2','admin',1),(67,34,'alicejohnson3','hashed_password_3','user',1),(68,35,'bobbrown4','hashed_password_4','user',1),(69,36,'caroldavis5','hashed_password_5','admin',1),(70,37,'davidwilson6','hashed_password_6','user',1),(71,38,'emilyclark7','hashed_password_7','user',1),(72,39,'frankharris8','hashed_password_8','user',1),(73,40,'gracelewis9','hashed_password_9','user',1),(74,41,'henrywalker10','hashed_password_10','admin',1),(75,42,'irenemartinez11','hashed_password_11','user',1),(76,43,'jamesthompson12','hashed_password_12','user',1),(77,44,'katherinewhite13','hashed_password_13','admin',1),(78,45,'liamanderson14','hashed_password_14','user',1),(79,46,'meganhall15','hashed_password_15','user',1),(80,47,'nathanyoung16','hashed_password_16','user',1),(81,48,'oliviaking17','hashed_password_17','user',1),(82,49,'patrickscott18','hashed_password_18','admin',1),(83,50,'quinnadams19','hashed_password_19','user',1),(84,51,'rachelturner20','hashed_password_20','user',1),(85,52,'stevenwilson47','hashed_password_21','user',1),(86,53,'tinaclark48','hashed_password_22','admin',1),(87,54,'ursularobinson49','hashed_password_23','user',1),(88,55,'victormartinez50','hashed_password_24','user',1),(89,56,'johndoe51','hashed_password_25','user',1),(90,57,'janesmith52','hashed_password_26','admin',1),(91,58,'alicejohnson53','hashed_password_27','user',1),(92,59,'bobbrown54','hashed_password_28','user',1),(93,60,'caroldavis55','hashed_password_29','admin',1),(94,61,'davidwilson56','hashed_password_30','user',1),(95,62,'emilyclark57','hashed_password_31','user',0),(96,63,'frankharris58','hashed_password_32','user',1),(97,64,'gracelewis59','hashed_password_33','user',1),(98,65,'henrywalker60','hashed_password_34','admin',1),(99,66,'irenemartinez61','hashed_password_35','user',1),(100,67,'jamesthompson62','hashed_password_36','user',1),(101,68,'katherinewhite63','hashed_password_37','admin',1),(102,69,'liamanderson64','hashed_password_38','user',1),(103,70,'meganhall65','hashed_password_39','user',1),(104,71,'nathanyoung66','hashed_password_40','user',1),(105,72,'oliviaking67','hashed_password_41','user',1),(106,73,'patrickscott68','hashed_password_42','admin',1),(107,74,'quinnadams69','hashed_password_43','user',1),(108,75,'rachelturner70','hashed_password_44','user',1),(109,76,'stevenwilson71','hashed_password_45','user',1),(110,77,'tinaclark72','hashed_password_46','admin',1),(111,78,'ursularobinson73','hashed_password_47','user',1),(112,79,'victormartinez74','hashed_password_48','user',1),(113,80,'johndoe75','hashed_password_49','user',1),(114,81,'victorrmartinez50','hashed_password_50','admin',1);
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

-- Dump completed on 2024-07-21  0:42:38
