-- phpMyAdmin SQL Dump
-- version 4.8.3
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1:3306
-- Generation Time: Mar 19, 2025 at 08:55 PM
-- Server version: 5.7.23
-- PHP Version: 7.2.10

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `library`
--

-- --------------------------------------------------------

--
-- Table structure for table `books`
--

DROP TABLE IF EXISTS `books`;
CREATE TABLE IF NOT EXISTS `books` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `author` varchar(255) NOT NULL,
  `published_date` date NOT NULL,
  `status` enum('Available','Issued','Reserved') NOT NULL,
  `image` varchar(500) DEFAULT NULL,
  `date_insert` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `date_update` timestamp NULL DEFAULT NULL,
  `date_delete` timestamp NULL DEFAULT NULL,
  `book_title` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `books`
--

INSERT INTO `books` (`id`, `author`, `published_date`, `status`, `image`, `date_insert`, `date_update`, `date_delete`, `book_title`) VALUES
(1, 'me me', '2025-02-05', 'Available', 'C:\\Users\\40737385\\Desktop\\Library-Management-System-using-CSharp-main\\LibraryManagementSystem\\LibraryManagementSystem\\how to not fuck upme me.jpg', '2025-03-19 00:00:00', NULL, NULL, 'how to not fuck up'),
(2, 'sdf', '2025-03-19', 'Available', 'C:\\Users\\40737385\\Desktop\\Library-Management-System-using-CSharp-main\\LibraryManagementSystem\\LibraryManagementSystem\\sdfsdf.jpg', '2025-03-19 00:00:00', NULL, NULL, 'sdf'),
(3, 'meee', '2025-02-25', 'Available', 'C:\\Users\\40737385\\Desktop\\Library-Management-System-using-CSharp-main\\LibraryManagementSystem\\LibraryManagementSystem\\how to howmeee.jpg', '2025-03-19 00:00:00', NULL, NULL, 'how to how');

-- --------------------------------------------------------

--
-- Table structure for table `issues`
--

DROP TABLE IF EXISTS `issues`;
CREATE TABLE IF NOT EXISTS `issues` (
  `issue_id` int(11) NOT NULL AUTO_INCREMENT,
  `full_name` varchar(255) NOT NULL,
  `contact` varchar(50) NOT NULL,
  `email` varchar(255) NOT NULL,
  `book_id` int(11) NOT NULL,
  `book_title` varchar(255) NOT NULL,
  `author` varchar(255) NOT NULL,
  `status` varchar(20) DEFAULT 'Issued',
  `issue_date` date NOT NULL,
  `return_date` date NOT NULL,
  `date_insert` datetime DEFAULT CURRENT_TIMESTAMP,
  `date_update` datetime DEFAULT NULL,
  `date_delete` datetime DEFAULT NULL,
  PRIMARY KEY (`issue_id`),
  KEY `book_id` (`book_id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
CREATE TABLE IF NOT EXISTS `users` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `username` varchar(20) NOT NULL,
  `password` varchar(20) NOT NULL,
  `email` varchar(50) NOT NULL,
  `date_register` date NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`id`, `username`, `password`, `email`, `date_register`) VALUES
(1, 'asd', 'asd', 'prashantr', '2025-03-19'),
(2, 'pp', '123', 'pp', '2025-03-19'),
(3, 'ass', 'ass', 'ass', '2025-03-19');
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
