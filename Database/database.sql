-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Oct 13, 2023 at 10:03 AM
-- Server version: 10.4.28-MariaDB
-- PHP Version: 8.2.4

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `clash_of_whatever`
--

-- --------------------------------------------------------

--
-- Table structure for table `accounts`
--

CREATE TABLE `accounts` (
  `id` int(11) NOT NULL,
  `name` varchar(300) NOT NULL DEFAULT 'Player',
  `device_id` varchar(500) NOT NULL,
  `gems` int(11) NOT NULL DEFAULT 0,
  `is_online` int(1) NOT NULL DEFAULT 0,
  `client_id` int(11) NOT NULL DEFAULT 0,
  `trophies` int(11) NOT NULL DEFAULT 0,
  `banned` int(1) NOT NULL DEFAULT 0,
  `shield` datetime NOT NULL DEFAULT current_timestamp(),
  `xp` int(11) NOT NULL DEFAULT 0,
  `level` int(11) NOT NULL DEFAULT 1,
  `clan_join_timer` datetime NOT NULL DEFAULT current_timestamp(),
  `clan_id` int(11) NOT NULL DEFAULT 0,
  `clan_rank` int(11) NOT NULL DEFAULT 0,
  `war_id` int(11) NOT NULL DEFAULT -1,
  `war_pos` int(11) NOT NULL DEFAULT 0,
  `global_chat_blocked` int(1) NOT NULL DEFAULT 0,
  `clan_chat_blocked` int(1) NOT NULL DEFAULT 0,
  `last_chat` datetime NOT NULL DEFAULT current_timestamp(),
  `chat_color` varchar(50) NOT NULL DEFAULT '',
  `email` varchar(1000) NOT NULL DEFAULT '',
  `password` varchar(1000) NOT NULL DEFAULT '',
  `map_layout` int(3) NOT NULL DEFAULT 0,
  `shld_cldn_1` datetime NOT NULL DEFAULT current_timestamp(),
  `shld_cldn_2` datetime NOT NULL DEFAULT current_timestamp(),
  `shld_cldn_3` datetime NOT NULL DEFAULT current_timestamp(),
  `last_login` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- --------------------------------------------------------

--
-- Table structure for table `battles`
--

CREATE TABLE `battles` (
  `id` int(11) NOT NULL,
  `attacker_id` int(11) NOT NULL,
  `defender_id` int(11) NOT NULL,
  `replay_path` varchar(2000) NOT NULL,
  `end_time` datetime NOT NULL DEFAULT current_timestamp(),
  `stars` int(1) NOT NULL DEFAULT 0,
  `trophies` int(11) NOT NULL DEFAULT 0,
  `looted_gold` int(11) NOT NULL DEFAULT 0,
  `looted_elixir` int(11) NOT NULL DEFAULT 0,
  `looted_dark_elixir` int(11) NOT NULL DEFAULT 0,
  `seen` int(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- --------------------------------------------------------

--
-- Table structure for table `buildings`
--

CREATE TABLE `buildings` (
  `id` int(11) NOT NULL,
  `global_id` varchar(50) NOT NULL DEFAULT '',
  `account_id` int(11) NOT NULL,
  `level` int(11) NOT NULL DEFAULT 0,
  `x_position` int(11) NOT NULL DEFAULT 0,
  `y_position` int(11) NOT NULL DEFAULT 0,
  `gold_storage` float NOT NULL DEFAULT 0,
  `elixir_storage` float NOT NULL DEFAULT 0,
  `dark_elixir_storage` float NOT NULL DEFAULT 0,
  `boost` datetime NOT NULL DEFAULT current_timestamp(),
  `construction_time` datetime NOT NULL DEFAULT current_timestamp(),
  `is_constructing` int(1) NOT NULL DEFAULT 0,
  `construction_build_time` int(11) NOT NULL DEFAULT 0,
  `track_time` datetime NOT NULL DEFAULT current_timestamp(),
  `x_war` int(11) NOT NULL DEFAULT -1,
  `y_war` int(11) NOT NULL DEFAULT -1
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- --------------------------------------------------------

--
-- Table structure for table `chat_messages`
--

CREATE TABLE `chat_messages` (
  `id` int(11) NOT NULL,
  `account_id` int(11) NOT NULL DEFAULT 0,
  `type` int(11) NOT NULL DEFAULT 0,
  `global_id` int(11) NOT NULL DEFAULT 0,
  `clan_id` int(11) NOT NULL DEFAULT 0,
  `message` varchar(1000) NOT NULL,
  `send_time` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- --------------------------------------------------------

--
-- Table structure for table `chat_reports`
--

CREATE TABLE `chat_reports` (
  `id` int(11) NOT NULL,
  `message_id` int(11) NOT NULL,
  `reporter_id` int(11) NOT NULL,
  `target_id` int(11) NOT NULL,
  `message` varchar(500) NOT NULL,
  `report_time` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- --------------------------------------------------------

--
-- Table structure for table `clans`
--

CREATE TABLE `clans` (
  `id` int(11) NOT NULL,
  `name` varchar(300) NOT NULL,
  `join_type` int(1) NOT NULL DEFAULT 0,
  `xp` int(11) NOT NULL DEFAULT 0,
  `level` int(11) NOT NULL DEFAULT 1,
  `trophies` int(11) NOT NULL DEFAULT 0,
  `min_trophies` int(11) NOT NULL DEFAULT 0,
  `min_townhall_level` int(11) NOT NULL DEFAULT 1,
  `pattern` int(11) NOT NULL DEFAULT 0,
  `background` int(11) NOT NULL DEFAULT 0,
  `pattern_color` varchar(50) NOT NULL,
  `background_color` varchar(50) NOT NULL,
  `war_id` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- --------------------------------------------------------

--
-- Table structure for table `clan_join_requests`
--

CREATE TABLE `clan_join_requests` (
  `id` int(11) NOT NULL,
  `clan_id` int(11) NOT NULL,
  `account_id` int(11) NOT NULL,
  `request_time` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- --------------------------------------------------------

--
-- Table structure for table `clan_wars`
--

CREATE TABLE `clan_wars` (
  `id` int(11) NOT NULL,
  `clan_1_id` int(11) NOT NULL,
  `clan_2_id` int(11) NOT NULL,
  `start_time` datetime NOT NULL DEFAULT current_timestamp(),
  `stage` int(11) NOT NULL DEFAULT 1,
  `report_path` varchar(2000) NOT NULL DEFAULT '',
  `winner_id` int(11) NOT NULL DEFAULT 0,
  `clan_1_stars` int(11) NOT NULL DEFAULT 0,
  `clan_2_stars` int(11) NOT NULL DEFAULT 0,
  `war_size` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- --------------------------------------------------------

--
-- Table structure for table `clan_wars_search`
--

CREATE TABLE `clan_wars_search` (
  `id` int(11) NOT NULL,
  `clan_id` int(11) NOT NULL,
  `account_id` int(11) NOT NULL,
  `search_time` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- --------------------------------------------------------

--
-- Table structure for table `clan_war_attacks`
--

CREATE TABLE `clan_war_attacks` (
  `id` int(11) NOT NULL,
  `war_id` int(11) NOT NULL,
  `attacker_id` int(11) NOT NULL,
  `defender_id` int(11) NOT NULL,
  `start_time` datetime NOT NULL DEFAULT current_timestamp(),
  `stars` int(1) NOT NULL DEFAULT 0,
  `looted_gold` int(11) NOT NULL DEFAULT 0,
  `looted_elixir` int(11) NOT NULL DEFAULT 0,
  `looted_dark_elixir` int(11) NOT NULL DEFAULT 0,
  `replay_path` varchar(2000) NOT NULL DEFAULT ''
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- --------------------------------------------------------

--
-- Table structure for table `iap`
--

CREATE TABLE `iap` (
  `id` int(11) NOT NULL,
  `account_id` int(11) NOT NULL,
  `market` varchar(100) NOT NULL,
  `product_id` varchar(100) NOT NULL,
  `token` varchar(100) NOT NULL,
  `save_time` datetime NOT NULL DEFAULT current_timestamp(),
  `price` varchar(100) NOT NULL DEFAULT '0',
  `currency` varchar(50) NOT NULL DEFAULT 'USD',
  `validated` int(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- --------------------------------------------------------

--
-- Table structure for table `research`
--

CREATE TABLE `research` (
  `id` int(11) NOT NULL,
  `type` int(11) NOT NULL,
  `account_id` int(11) NOT NULL,
  `global_id` varchar(100) NOT NULL,
  `level` int(11) NOT NULL DEFAULT 1,
  `researching` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- --------------------------------------------------------

--
-- Table structure for table `server_buildings`
--

CREATE TABLE `server_buildings` (
  `id` int(11) NOT NULL,
  `global_id` varchar(50) NOT NULL DEFAULT '',
  `level` int(11) NOT NULL DEFAULT 0,
  `req_gold` int(11) NOT NULL DEFAULT 0,
  `req_elixir` int(11) NOT NULL DEFAULT 0,
  `req_gems` int(11) NOT NULL DEFAULT 0,
  `req_dark_elixir` int(11) NOT NULL DEFAULT 0,
  `columns_count` int(11) NOT NULL DEFAULT 0,
  `rows_count` int(11) NOT NULL DEFAULT 0,
  `capacity` int(11) NOT NULL DEFAULT 0,
  `gold_capacity` int(11) NOT NULL DEFAULT 0,
  `elixir_capacity` int(11) NOT NULL DEFAULT 0,
  `dark_elixir_capacity` int(11) NOT NULL DEFAULT 0,
  `speed` float NOT NULL DEFAULT 0,
  `health` int(11) NOT NULL DEFAULT 100,
  `damage` float NOT NULL DEFAULT 0,
  `target_type` varchar(50) NOT NULL DEFAULT 'none',
  `radius` float NOT NULL DEFAULT 0,
  `blind_radius` float NOT NULL DEFAULT 0,
  `splash_radius` float NOT NULL DEFAULT 0,
  `projectile_speed` float NOT NULL DEFAULT 0,
  `build_time` int(11) NOT NULL DEFAULT 0,
  `gained_xp` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Dumping data for table `server_buildings`
--

INSERT INTO `server_buildings` (`id`, `global_id`, `level`, `req_gold`, `req_elixir`, `req_gems`, `req_dark_elixir`, `columns_count`, `rows_count`, `capacity`, `gold_capacity`, `elixir_capacity`, `dark_elixir_capacity`, `speed`, `health`, `damage`, `target_type`, `radius`, `blind_radius`, `splash_radius`, `projectile_speed`, `build_time`, `gained_xp`) VALUES
(1, 'townhall', 1, 0, 0, 0, 0, 4, 4, 0, 1000, 1000, 0, 0, 450, 0, 'none', 0, 0, 0, 0, 0, 0),
(2, 'townhall', 2, 1000, 1000, 0, 0, 4, 4, 0, 2500, 2500, 0, 0, 1600, 0, 'none', 0, 0, 0, 0, 10, 3),
(3, 'townhall', 3, 4000, 4000, 0, 0, 4, 4, 0, 10000, 10000, 0, 0, 1850, 0, 'none', 0, 0, 0, 0, 3600, 60),
(4, 'townhall', 4, 25000, 25000, 0, 0, 4, 4, 0, 50000, 50000, 0, 0, 2100, 0, 'none', 0, 0, 0, 0, 10800, 103),
(5, 'townhall', 5, 150000, 150000, 0, 0, 4, 4, 0, 100000, 100000, 0, 0, 2400, 0, 'none', 0, 0, 0, 0, 21600, 146),
(6, 'townhall', 6, 750000, 750000, 0, 0, 4, 4, 0, 300000, 300000, 0, 0, 2800, 0, 'none', 0, 0, 0, 0, 43200, 207),
(7, 'townhall', 7, 1000000, 1000000, 0, 0, 4, 4, 0, 500000, 500000, 0, 0, 3300, 0, 'none', 0, 0, 0, 0, 64800, 254),
(8, 'townhall', 8, 2000000, 2000000, 0, 0, 4, 4, 0, 750000, 750000, 0, 0, 3900, 0, 'none', 0, 0, 0, 0, 86400, 293),
(9, 'townhall', 9, 3000000, 3000000, 0, 0, 4, 4, 0, 1000000, 1000000, 0, 0, 4600, 0, 'none', 0, 0, 0, 0, 172800, 415),
(10, 'townhall', 10, 3500000, 3500000, 0, 0, 4, 4, 0, 1500000, 1500000, 0, 0, 5500, 0, 'none', 0, 0, 0, 0, 216000, 464),
(11, 'townhall', 11, 4000000, 4000000, 0, 0, 4, 4, 0, 2000000, 2000000, 0, 0, 6800, 0, 'none', 0, 0, 0, 0, 237600, 487),
(12, 'townhall', 12, 6000000, 6000000, 0, 0, 4, 4, 0, 2000000, 2000000, 0, 0, 7500, 0, 'none', 0, 0, 0, 0, 367200, 605),
(13, 'townhall', 13, 9000000, 9000000, 0, 0, 4, 4, 0, 2000000, 2000000, 0, 0, 8200, 0, 'none', 0, 0, 0, 0, 648000, 804),
(14, 'townhall', 14, 15000000, 15000000, 0, 0, 4, 4, 0, 2000000, 2000000, 0, 0, 8900, 0, 'none', 0, 0, 0, 0, 1144800, 1069),
(15, 'townhall', 15, 18000000, 18000000, 0, 0, 4, 4, 0, 2000000, 2000000, 0, 0, 9600, 0, 'none', 0, 0, 0, 0, 1296000, 1138),
(16, 'goldmine', 1, 0, 150, 0, 0, 3, 3, 0, 1000, 0, 0, 200, 400, 0, 'none', 0, 0, 0, 0, 10, 3),
(17, 'goldmine', 2, 0, 300, 0, 0, 3, 3, 0, 2000, 0, 0, 400, 440, 0, 'none', 0, 0, 0, 0, 60, 7),
(18, 'goldmine', 3, 0, 700, 0, 0, 3, 3, 0, 3000, 0, 0, 600, 480, 0, 'none', 0, 0, 0, 0, 240, 15),
(19, 'goldmine', 4, 0, 1400, 0, 0, 3, 3, 0, 5000, 0, 0, 800, 520, 0, 'none', 0, 0, 0, 0, 600, 24),
(20, 'goldmine', 5, 0, 3000, 0, 0, 3, 3, 0, 10000, 0, 0, 1000, 560, 0, 'none', 0, 0, 0, 0, 2400, 48),
(21, 'goldmine', 6, 0, 7000, 0, 0, 3, 3, 0, 20000, 0, 0, 1300, 600, 0, 'none', 0, 0, 0, 0, 10800, 103),
(22, 'goldmine', 7, 0, 14000, 0, 0, 3, 3, 0, 30000, 0, 0, 1600, 640, 0, 'none', 0, 0, 0, 0, 21600, 146),
(23, 'goldmine', 8, 0, 28000, 0, 0, 3, 3, 0, 50000, 0, 0, 1900, 680, 0, 'none', 0, 0, 0, 0, 28800, 169),
(24, 'goldmine', 9, 0, 56000, 0, 0, 3, 3, 0, 75000, 0, 0, 2200, 720, 0, 'none', 0, 0, 0, 0, 36000, 189),
(25, 'goldmine', 10, 0, 75000, 0, 0, 3, 3, 0, 100000, 0, 0, 2800, 780, 0, 'none', 0, 0, 0, 0, 43200, 207),
(26, 'goldmine', 11, 0, 100000, 0, 0, 3, 3, 0, 150000, 0, 0, 3500, 860, 0, 'none', 0, 0, 0, 0, 57600, 240),
(27, 'goldmine', 12, 0, 200000, 0, 0, 3, 3, 0, 200000, 0, 0, 4200, 960, 0, 'none', 0, 0, 0, 0, 72000, 268),
(28, 'goldmine', 13, 0, 400000, 0, 0, 3, 3, 0, 250000, 0, 0, 4900, 1080, 0, 'none', 0, 0, 0, 0, 144000, 379),
(29, 'goldmine', 14, 0, 800000, 0, 0, 3, 3, 0, 300000, 0, 0, 5600, 1180, 0, 'none', 0, 0, 0, 0, 288000, 536),
(30, 'goldmine', 15, 0, 1200000, 0, 0, 3, 3, 0, 350000, 0, 0, 6300, 1280, 0, 'none', 0, 0, 0, 0, 518400, 720),
(31, 'elixirmine', 1, 150, 0, 0, 0, 3, 3, 0, 0, 1000, 0, 200, 400, 0, 'none', 0, 0, 0, 0, 10, 3),
(32, 'elixirmine', 2, 300, 0, 0, 0, 3, 3, 0, 0, 2000, 0, 400, 440, 0, 'none', 0, 0, 0, 0, 60, 7),
(33, 'elixirmine', 3, 700, 0, 0, 0, 3, 3, 0, 0, 3000, 0, 600, 480, 0, 'none', 0, 0, 0, 0, 240, 15),
(34, 'elixirmine', 4, 1400, 0, 0, 0, 3, 3, 0, 0, 5000, 0, 800, 520, 0, 'none', 0, 0, 0, 0, 600, 24),
(35, 'elixirmine', 5, 3000, 0, 0, 0, 3, 3, 0, 0, 10000, 0, 1000, 560, 0, 'none', 0, 0, 0, 0, 2400, 48),
(36, 'elixirmine', 6, 7000, 0, 0, 0, 3, 3, 0, 0, 20000, 0, 1300, 600, 0, 'none', 0, 0, 0, 0, 10800, 103),
(37, 'elixirmine', 7, 14000, 0, 0, 0, 3, 3, 0, 0, 30000, 0, 1600, 640, 0, 'none', 0, 0, 0, 0, 21600, 146),
(38, 'elixirmine', 8, 28000, 0, 0, 0, 3, 3, 0, 0, 50000, 0, 1900, 680, 0, 'none', 0, 0, 0, 0, 28800, 169),
(39, 'elixirmine', 9, 56000, 0, 0, 0, 3, 3, 0, 0, 75000, 0, 2200, 720, 0, 'none', 0, 0, 0, 0, 36000, 189),
(40, 'elixirmine', 10, 75000, 0, 0, 0, 3, 3, 0, 0, 100000, 0, 2800, 780, 0, 'none', 0, 0, 0, 0, 43200, 207),
(41, 'elixirmine', 11, 100000, 0, 0, 0, 3, 3, 0, 0, 150000, 0, 3500, 860, 0, 'none', 0, 0, 0, 0, 57600, 240),
(42, 'elixirmine', 12, 200000, 0, 0, 0, 3, 3, 0, 0, 200000, 0, 4200, 960, 0, 'none', 0, 0, 0, 0, 72000, 268),
(43, 'elixirmine', 13, 400000, 0, 0, 0, 3, 3, 0, 0, 250000, 0, 4900, 1080, 0, 'none', 0, 0, 0, 0, 144000, 379),
(44, 'elixirmine', 14, 800000, 0, 0, 0, 3, 3, 0, 0, 300000, 0, 5600, 1180, 0, 'none', 0, 0, 0, 0, 288000, 536),
(45, 'elixirmine', 15, 1200000, 0, 0, 0, 3, 3, 0, 0, 350000, 0, 6300, 1280, 0, 'none', 0, 0, 0, 0, 518400, 720),
(46, 'goldstorage', 1, 0, 300, 0, 0, 3, 3, 0, 1500, 0, 0, 0, 400, 0, 'none', 0, 0, 0, 0, 10, 3),
(47, 'goldstorage', 2, 0, 750, 0, 0, 3, 3, 0, 3000, 0, 0, 0, 600, 0, 'none', 0, 0, 0, 0, 300, 17),
(48, 'goldstorage', 3, 0, 1500, 0, 0, 3, 3, 0, 6000, 0, 0, 0, 800, 0, 'none', 0, 0, 0, 0, 1200, 34),
(49, 'goldstorage', 4, 0, 3000, 0, 0, 3, 3, 0, 12000, 0, 0, 0, 1000, 0, 'none', 0, 0, 0, 0, 3600, 60),
(50, 'goldstorage', 5, 0, 6000, 0, 0, 3, 3, 0, 25000, 0, 0, 0, 1200, 0, 'none', 0, 0, 0, 0, 7200, 84),
(51, 'goldstorage', 6, 0, 12000, 0, 0, 3, 3, 0, 45000, 0, 0, 0, 1400, 0, 'none', 0, 0, 0, 0, 10800, 103),
(52, 'goldstorage', 7, 0, 25000, 0, 0, 3, 3, 0, 100000, 0, 0, 0, 1600, 0, 'none', 0, 0, 0, 0, 14400, 120),
(53, 'goldstorage', 8, 0, 50000, 0, 0, 3, 3, 0, 225000, 0, 0, 0, 1700, 0, 'none', 0, 0, 0, 0, 18000, 134),
(54, 'goldstorage', 9, 0, 100000, 0, 0, 3, 3, 0, 450000, 0, 0, 0, 1800, 0, 'none', 0, 0, 0, 0, 28800, 169),
(55, 'goldstorage', 10, 0, 250000, 0, 0, 3, 3, 0, 850000, 0, 0, 0, 1900, 0, 'none', 0, 0, 0, 0, 43200, 207),
(56, 'goldstorage', 11, 0, 500000, 0, 0, 3, 3, 0, 1750000, 0, 0, 0, 2100, 0, 'none', 0, 0, 0, 0, 57600, 240),
(57, 'goldstorage', 12, 0, 1000000, 0, 0, 3, 3, 0, 2000000, 0, 0, 0, 2500, 0, 'none', 0, 0, 0, 0, 172800, 415),
(58, 'goldstorage', 13, 0, 2000000, 0, 0, 3, 3, 0, 3000000, 0, 0, 0, 2900, 0, 'none', 0, 0, 0, 0, 345600, 587),
(59, 'goldstorage', 14, 0, 3500000, 0, 0, 3, 3, 0, 4000000, 0, 0, 0, 3300, 0, 'none', 0, 0, 0, 0, 583200, 763),
(60, 'goldstorage', 15, 0, 5500000, 0, 0, 3, 3, 0, 4500000, 0, 0, 0, 3700, 0, 'none', 0, 0, 0, 0, 972000, 985),
(61, 'goldstorage', 16, 0, 6500000, 0, 0, 3, 3, 0, 5000000, 0, 0, 0, 3900, 0, 'none', 0, 0, 0, 0, 1036800, 1018),
(62, 'elixirstorage', 1, 300, 0, 0, 0, 3, 3, 0, 0, 1500, 0, 0, 400, 0, 'none', 0, 0, 0, 0, 10, 3),
(63, 'elixirstorage', 2, 750, 0, 0, 0, 3, 3, 0, 0, 3000, 0, 0, 600, 0, 'none', 0, 0, 0, 0, 300, 17),
(64, 'elixirstorage', 3, 1500, 0, 0, 0, 3, 3, 0, 0, 6000, 0, 0, 800, 0, 'none', 0, 0, 0, 0, 1200, 34),
(65, 'elixirstorage', 4, 3000, 0, 0, 0, 3, 3, 0, 0, 12000, 0, 0, 1000, 0, 'none', 0, 0, 0, 0, 3600, 60),
(66, 'elixirstorage', 5, 6000, 0, 0, 0, 3, 3, 0, 0, 25000, 0, 0, 1200, 0, 'none', 0, 0, 0, 0, 7200, 84),
(67, 'elixirstorage', 6, 12000, 0, 0, 0, 3, 3, 0, 0, 45000, 0, 0, 1400, 0, 'none', 0, 0, 0, 0, 10800, 103),
(68, 'elixirstorage', 7, 25000, 0, 0, 0, 3, 3, 0, 0, 100000, 0, 0, 1600, 0, 'none', 0, 0, 0, 0, 14400, 120),
(69, 'elixirstorage', 8, 50000, 0, 0, 0, 3, 3, 0, 0, 225000, 0, 0, 1700, 0, 'none', 0, 0, 0, 0, 18000, 134),
(70, 'elixirstorage', 9, 100000, 0, 0, 0, 3, 3, 0, 0, 450000, 0, 0, 1800, 0, 'none', 0, 0, 0, 0, 28800, 169),
(71, 'elixirstorage', 10, 250000, 0, 0, 0, 3, 3, 0, 0, 850000, 0, 0, 1900, 0, 'none', 0, 0, 0, 0, 43200, 207),
(72, 'elixirstorage', 11, 500000, 0, 0, 0, 3, 3, 0, 0, 1750000, 0, 0, 2100, 0, 'none', 0, 0, 0, 0, 57600, 240),
(73, 'elixirstorage', 12, 1000000, 0, 0, 0, 3, 3, 0, 0, 2000000, 0, 0, 2500, 0, 'none', 0, 0, 0, 0, 172800, 415),
(74, 'elixirstorage', 13, 2000000, 0, 0, 0, 3, 3, 0, 0, 3000000, 0, 0, 2900, 0, 'none', 0, 0, 0, 0, 345600, 587),
(75, 'elixirstorage', 14, 3500000, 0, 0, 0, 3, 3, 0, 0, 4000000, 0, 0, 3300, 0, 'none', 0, 0, 0, 0, 583200, 763),
(76, 'elixirstorage', 15, 5500000, 0, 0, 0, 3, 3, 0, 0, 4500000, 0, 0, 3700, 0, 'none', 0, 0, 0, 0, 972000, 985),
(77, 'elixirstorage', 16, 6500000, 0, 0, 0, 3, 3, 0, 0, 5000000, 0, 0, 3900, 0, 'none', 0, 0, 0, 0, 1036800, 1018),
(78, 'buildershut', 1, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 250, 0, 'none', 0, 0, 0, 0, 0, 0),
(79, 'buildershut', 2, 9000000, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 1000, 0, 'none', 0, 0, 0, 0, 820800, 905),
(80, 'buildershut', 3, 12000000, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 1300, 0, 'none', 0, 0, 0, 0, 993600, 996),
(81, 'buildershut', 4, 14500000, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 1600, 0, 'none', 0, 0, 0, 0, 1144800, 1069),
(82, 'darkelixirmine', 1, 0, 500000, 0, 0, 3, 3, 0, 0, 0, 160, 20, 800, 0, 'none', 0, 0, 0, 0, 21600, 146),
(83, 'darkelixirmine', 2, 0, 700000, 0, 0, 3, 3, 0, 0, 0, 300, 30, 860, 0, 'none', 0, 0, 0, 0, 43200, 207),
(84, 'darkelixirmine', 3, 0, 900000, 0, 0, 3, 3, 0, 0, 0, 540, 45, 920, 0, 'none', 0, 0, 0, 0, 64800, 254),
(85, 'darkelixirmine', 4, 0, 1200000, 0, 0, 3, 3, 0, 0, 0, 840, 60, 980, 0, 'none', 0, 0, 0, 0, 86400, 293),
(86, 'darkelixirmine', 5, 0, 1500000, 0, 0, 3, 3, 0, 0, 0, 1280, 80, 1060, 0, 'none', 0, 0, 0, 0, 129600, 360),
(87, 'darkelixirmine', 6, 0, 1800000, 0, 0, 3, 3, 0, 0, 0, 1800, 100, 1160, 0, 'none', 0, 0, 0, 0, 172800, 415),
(88, 'darkelixirmine', 7, 0, 2400000, 0, 0, 3, 3, 0, 0, 0, 2400, 120, 1280, 0, 'none', 0, 0, 0, 0, 259200, 509),
(89, 'darkelixirmine', 8, 0, 3000000, 0, 0, 3, 3, 0, 0, 0, 3000, 140, 1380, 0, 'none', 0, 0, 0, 0, 345600, 587),
(90, 'darkelixirmine', 9, 0, 4000000, 0, 0, 3, 3, 0, 0, 0, 3600, 160, 1480, 0, 'none', 0, 0, 0, 0, 604800, 777),
(91, 'darkelixirstorage', 1, 0, 250000, 0, 0, 3, 3, 0, 0, 0, 10000, 0, 2000, 0, 'none', 0, 0, 0, 0, 28800, 169),
(92, 'darkelixirstorage', 2, 0, 500000, 0, 0, 3, 3, 0, 0, 0, 17500, 0, 2200, 0, 'none', 0, 0, 0, 0, 57600, 240),
(93, 'darkelixirstorage', 3, 0, 1000000, 0, 0, 3, 3, 0, 0, 0, 40000, 0, 2400, 0, 'none', 0, 0, 0, 0, 86400, 293),
(94, 'darkelixirstorage', 4, 0, 1500000, 0, 0, 3, 3, 0, 0, 0, 75000, 0, 2600, 0, 'none', 0, 0, 0, 0, 129600, 360),
(95, 'darkelixirstorage', 5, 0, 2000000, 0, 0, 3, 3, 0, 0, 0, 140000, 0, 2900, 0, 'none', 0, 0, 0, 0, 172800, 415),
(96, 'darkelixirstorage', 6, 0, 3000000, 0, 0, 3, 3, 0, 0, 0, 180000, 0, 3200, 0, 'none', 0, 0, 0, 0, 259200, 509),
(97, 'darkelixirstorage', 7, 0, 4200000, 0, 0, 3, 3, 0, 0, 0, 220000, 0, 3500, 0, 'none', 0, 0, 0, 0, 302400, 549),
(98, 'darkelixirstorage', 8, 0, 6700000, 0, 0, 3, 3, 0, 0, 0, 280000, 0, 3800, 0, 'none', 0, 0, 0, 0, 648000, 804),
(99, 'darkelixirstorage', 9, 0, 11500000, 0, 0, 3, 3, 0, 0, 0, 330000, 0, 4100, 0, 'none', 0, 0, 0, 0, 1231200, 1109),
(100, 'darkelixirstorage', 10, 0, 12500000, 0, 0, 3, 3, 0, 0, 0, 350000, 0, 4300, 0, 'none', 0, 0, 0, 0, 1296000, 1138),
(101, 'clancastle', 1, 0, 10000, 0, 0, 3, 3, 10, 0, 0, 0, 0, 1000, 0, 'none', 0, 0, 0, 0, 0, 0),
(102, 'clancastle', 2, 0, 100000, 0, 0, 3, 3, 15, 0, 0, 0, 0, 1400, 0, 'none', 0, 0, 0, 0, 28800, 169),
(103, 'clancastle', 3, 0, 800000, 0, 0, 3, 3, 20, 0, 0, 0, 0, 2000, 0, 'none', 0, 0, 0, 0, 43200, 207),
(104, 'clancastle', 4, 0, 1200000, 0, 0, 3, 3, 25, 0, 0, 0, 0, 2600, 0, 'none', 0, 0, 0, 0, 86400, 293),
(105, 'clancastle', 5, 0, 2500000, 0, 0, 3, 3, 30, 0, 0, 0, 0, 3000, 0, 'none', 0, 0, 0, 0, 172800, 415),
(106, 'clancastle', 6, 0, 4200000, 0, 0, 3, 3, 35, 0, 0, 0, 0, 3400, 0, 'none', 0, 0, 0, 0, 432000, 657),
(107, 'clancastle', 7, 0, 5500000, 0, 0, 3, 3, 35, 0, 0, 0, 0, 4000, 0, 'none', 0, 0, 0, 0, 540000, 734),
(108, 'clancastle', 8, 0, 8500000, 0, 0, 3, 3, 40, 0, 0, 0, 0, 4400, 0, 'none', 0, 0, 0, 0, 842400, 917),
(109, 'clancastle', 9, 0, 12000000, 0, 0, 3, 3, 45, 0, 0, 0, 0, 4800, 0, 'none', 0, 0, 0, 0, 1166400, 1080),
(110, 'clancastle', 10, 0, 18000000, 0, 0, 3, 3, 45, 0, 0, 0, 0, 5200, 0, 'none', 0, 0, 0, 0, 1641600, 1281),
(111, 'clancastle', 11, 0, 20000000, 0, 0, 3, 3, 50, 0, 0, 0, 0, 5400, 0, 'none', 0, 0, 0, 0, 1728000, 1314),
(112, 'armycamp', 1, 0, 200, 0, 0, 4, 4, 20, 0, 0, 0, 0, 250, 0, 'none', 0, 0, 0, 0, 300, 17),
(113, 'armycamp', 2, 0, 2000, 0, 0, 4, 4, 30, 0, 0, 0, 0, 270, 0, 'none', 0, 0, 0, 0, 900, 30),
(114, 'armycamp', 3, 0, 10000, 0, 0, 4, 4, 35, 0, 0, 0, 0, 290, 0, 'none', 0, 0, 0, 0, 7200, 84),
(115, 'armycamp', 4, 0, 100000, 0, 0, 4, 4, 40, 0, 0, 0, 0, 310, 0, 'none', 0, 0, 0, 0, 18000, 134),
(116, 'armycamp', 5, 0, 250000, 0, 0, 4, 4, 45, 0, 0, 0, 0, 330, 0, 'none', 0, 0, 0, 0, 28800, 169),
(117, 'armycamp', 6, 0, 750000, 0, 0, 4, 4, 50, 0, 0, 0, 0, 350, 0, 'none', 0, 0, 0, 0, 43200, 207),
(118, 'armycamp', 7, 0, 1500000, 0, 0, 4, 4, 55, 0, 0, 0, 0, 400, 0, 'none', 0, 0, 0, 0, 172800, 415),
(119, 'armycamp', 8, 0, 2500000, 0, 0, 4, 4, 60, 0, 0, 0, 0, 500, 0, 'none', 0, 0, 0, 0, 259200, 509),
(120, 'armycamp', 9, 0, 4200000, 0, 0, 4, 4, 65, 0, 0, 0, 0, 600, 0, 'none', 0, 0, 0, 0, 540000, 734),
(121, 'armycamp', 10, 0, 6300000, 0, 0, 4, 4, 70, 0, 0, 0, 0, 700, 0, 'none', 0, 0, 0, 0, 842400, 917),
(122, 'armycamp', 11, 0, 12000000, 0, 0, 4, 4, 75, 0, 0, 0, 0, 800, 0, 'none', 0, 0, 0, 0, 1036800, 1018),
(123, 'armycamp', 12, 0, 19000000, 0, 0, 4, 4, 80, 0, 0, 0, 0, 850, 0, 'none', 0, 0, 0, 0, 1382400, 1175),
(124, 'barracks', 1, 0, 100, 0, 0, 3, 3, 40, 0, 0, 0, 0, 250, 0, 'none', 0, 0, 0, 0, 10, 3),
(125, 'barracks', 2, 0, 500, 0, 0, 3, 3, 40, 0, 0, 0, 0, 290, 0, 'none', 0, 0, 0, 0, 60, 7),
(126, 'barracks', 3, 0, 2500, 0, 0, 3, 3, 40, 0, 0, 0, 0, 330, 0, 'none', 0, 0, 0, 0, 600, 24),
(127, 'barracks', 4, 0, 5000, 0, 0, 3, 3, 40, 0, 0, 0, 0, 370, 0, 'none', 0, 0, 0, 0, 3600, 60),
(128, 'barracks', 5, 0, 20000, 0, 0, 3, 3, 40, 0, 0, 0, 0, 420, 0, 'none', 0, 0, 0, 0, 28800, 169),
(129, 'barracks', 6, 0, 120000, 0, 0, 3, 3, 40, 0, 0, 0, 0, 470, 0, 'none', 0, 0, 0, 0, 43200, 207),
(130, 'barracks', 7, 0, 270000, 0, 0, 3, 3, 40, 0, 0, 0, 0, 520, 0, 'none', 0, 0, 0, 0, 64800, 254),
(131, 'barracks', 8, 0, 800000, 0, 0, 3, 3, 40, 0, 0, 0, 0, 580, 0, 'none', 0, 0, 0, 0, 86400, 293),
(132, 'barracks', 9, 0, 1200000, 0, 0, 3, 3, 40, 0, 0, 0, 0, 650, 0, 'none', 0, 0, 0, 0, 129600, 360),
(133, 'barracks', 10, 0, 1700000, 0, 0, 3, 3, 40, 0, 0, 0, 0, 730, 0, 'none', 0, 0, 0, 0, 216000, 464),
(134, 'barracks', 11, 0, 2600000, 0, 0, 3, 3, 40, 0, 0, 0, 0, 810, 0, 'none', 0, 0, 0, 0, 345600, 587),
(135, 'barracks', 12, 0, 3700000, 0, 0, 3, 3, 40, 0, 0, 0, 0, 900, 0, 'none', 0, 0, 0, 0, 475200, 689),
(136, 'barracks', 13, 0, 6500000, 0, 0, 3, 3, 40, 0, 0, 0, 0, 980, 0, 'none', 0, 0, 0, 0, 648000, 804),
(137, 'barracks', 14, 0, 9000000, 0, 0, 3, 3, 40, 0, 0, 0, 0, 1050, 0, 'none', 0, 0, 0, 0, 864000, 929),
(138, 'barracks', 15, 0, 12500000, 0, 0, 3, 3, 40, 0, 0, 0, 0, 1150, 0, 'none', 0, 0, 0, 0, 1123200, 1059),
(139, 'barracks', 16, 0, 15000000, 0, 0, 3, 3, 40, 0, 0, 0, 0, 1250, 0, 'none', 0, 0, 0, 0, 1382400, 1175),
(140, 'darkbarracks', 1, 0, 200000, 0, 0, 3, 3, 0, 0, 0, 0, 0, 500, 0, 'none', 0, 0, 0, 0, 28800, 169),
(141, 'darkbarracks', 2, 0, 600000, 0, 0, 3, 3, 0, 0, 0, 0, 0, 550, 0, 'none', 0, 0, 0, 0, 86400, 293),
(142, 'darkbarracks', 3, 0, 1000000, 0, 0, 3, 3, 0, 0, 0, 0, 0, 600, 0, 'none', 0, 0, 0, 0, 129600, 360),
(143, 'darkbarracks', 4, 0, 1600000, 0, 0, 3, 3, 0, 0, 0, 0, 0, 650, 0, 'none', 0, 0, 0, 0, 172800, 415),
(144, 'darkbarracks', 5, 0, 2200000, 0, 0, 3, 3, 0, 0, 0, 0, 0, 700, 0, 'none', 0, 0, 0, 0, 302400, 549),
(145, 'darkbarracks', 6, 0, 2900000, 0, 0, 3, 3, 0, 0, 0, 0, 0, 750, 0, 'none', 0, 0, 0, 0, 388800, 623),
(146, 'darkbarracks', 7, 0, 4000000, 0, 0, 3, 3, 0, 0, 0, 0, 0, 800, 0, 'none', 0, 0, 0, 0, 691200, 831),
(147, 'darkbarracks', 8, 0, 10000000, 0, 0, 3, 3, 0, 0, 0, 0, 0, 900, 0, 'none', 0, 0, 0, 0, 907200, 952),
(148, 'laboratory', 1, 0, 5000, 0, 0, 3, 3, 0, 0, 0, 0, 0, 500, 0, 'none', 0, 0, 0, 0, 60, 7),
(149, 'laboratory', 2, 0, 25000, 0, 0, 3, 3, 0, 0, 0, 0, 0, 550, 0, 'none', 0, 0, 0, 0, 3600, 60),
(150, 'laboratory', 3, 0, 50000, 0, 0, 3, 3, 0, 0, 0, 0, 0, 600, 0, 'none', 0, 0, 0, 0, 7200, 84),
(151, 'laboratory', 4, 0, 100000, 0, 0, 3, 3, 0, 0, 0, 0, 0, 650, 0, 'none', 0, 0, 0, 0, 14400, 120),
(152, 'laboratory', 5, 0, 200000, 0, 0, 3, 3, 0, 0, 0, 0, 0, 700, 0, 'none', 0, 0, 0, 0, 28800, 169),
(153, 'laboratory', 6, 0, 400000, 0, 0, 3, 3, 0, 0, 0, 0, 0, 750, 0, 'none', 0, 0, 0, 0, 57600, 240),
(154, 'laboratory', 7, 0, 800000, 0, 0, 3, 3, 0, 0, 0, 0, 0, 830, 0, 'none', 0, 0, 0, 0, 86400, 293),
(155, 'laboratory', 8, 0, 1300000, 0, 0, 3, 3, 0, 0, 0, 0, 0, 950, 0, 'none', 0, 0, 0, 0, 151200, 388),
(156, 'laboratory', 9, 0, 2100000, 0, 0, 3, 3, 0, 0, 0, 0, 0, 1070, 0, 'none', 0, 0, 0, 0, 237600, 487),
(157, 'laboratory', 10, 0, 4200000, 0, 0, 3, 3, 0, 0, 0, 0, 0, 1140, 0, 'none', 0, 0, 0, 0, 712800, 605),
(158, 'laboratory', 11, 0, 6800000, 0, 0, 3, 3, 0, 0, 0, 0, 0, 1210, 0, 'none', 0, 0, 0, 0, 583200, 763),
(159, 'laboratory', 12, 0, 11500000, 0, 0, 3, 3, 0, 0, 0, 0, 0, 1280, 0, 'none', 0, 0, 0, 0, 993600, 996),
(160, 'laboratory', 13, 0, 12500000, 0, 0, 3, 3, 0, 0, 0, 0, 0, 1350, 0, 'none', 0, 0, 0, 0, 1036800, 1018),
(161, 'spellfactory', 1, 0, 150000, 0, 0, 3, 3, 2, 0, 0, 0, 0, 425, 0, 'none', 0, 0, 0, 0, 28800, 169),
(162, 'spellfactory', 2, 0, 300000, 0, 0, 3, 3, 4, 0, 0, 0, 0, 470, 0, 'none', 0, 0, 0, 0, 86400, 293),
(163, 'spellfactory', 3, 0, 600000, 0, 0, 3, 3, 6, 0, 0, 0, 0, 520, 0, 'none', 0, 0, 0, 0, 172800, 415),
(164, 'spellfactory', 4, 0, 1200000, 0, 0, 3, 3, 8, 0, 0, 0, 0, 600, 0, 'none', 0, 0, 0, 0, 302400, 549),
(165, 'spellfactory', 5, 0, 2000000, 0, 0, 3, 3, 10, 0, 0, 0, 0, 720, 0, 'none', 0, 0, 0, 0, 367200, 605),
(166, 'spellfactory', 6, 0, 3500000, 0, 0, 3, 3, 10, 0, 0, 0, 0, 840, 0, 'none', 0, 0, 0, 0, 432000, 657),
(167, 'spellfactory', 7, 0, 9000000, 0, 0, 3, 3, 10, 0, 0, 0, 0, 960, 0, 'none', 0, 0, 0, 0, 777600, 881),
(168, 'darkspellfactory', 1, 0, 150000, 0, 0, 3, 3, 1, 0, 0, 0, 0, 600, 0, 'none', 0, 0, 0, 0, 21600, 146),
(169, 'darkspellfactory', 2, 0, 300000, 0, 0, 3, 3, 1, 0, 0, 0, 0, 660, 0, 'none', 0, 0, 0, 0, 64800, 254),
(170, 'darkspellfactory', 3, 0, 600000, 0, 0, 3, 3, 1, 0, 0, 0, 0, 720, 0, 'none', 0, 0, 0, 0, 172800, 415),
(171, 'darkspellfactory', 4, 0, 1200000, 0, 0, 3, 3, 1, 0, 0, 0, 0, 780, 0, 'none', 0, 0, 0, 0, 345600, 587),
(172, 'darkspellfactory', 5, 0, 2500000, 0, 0, 3, 3, 1, 0, 0, 0, 0, 840, 0, 'none', 0, 0, 0, 0, 518400, 720),
(173, 'cannon', 1, 250, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0.8, 420, 7.2, 'ground', 9, 0, 0, 10, 10, 3),
(174, 'cannon', 2, 1000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0.8, 470, 8.8, 'ground', 9, 0, 0, 10, 120, 10),
(175, 'cannon', 3, 4000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0.8, 520, 12, 'ground', 9, 0, 0, 10, 600, 24),
(176, 'cannon', 4, 16000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0.8, 570, 15.2, 'ground', 9, 0, 0, 10, 2700, 51),
(177, 'cannon', 5, 50000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0.8, 620, 20, 'ground', 9, 0, 0, 10, 7200, 84),
(178, 'cannon', 6, 100000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0.8, 670, 24.8, 'ground', 9, 0, 0, 10, 14400, 120),
(179, 'cannon', 7, 200000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0.8, 730, 32, 'ground', 9, 0, 0, 10, 28800, 169),
(180, 'cannon', 8, 300000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0.8, 800, 38.4, 'ground', 9, 0, 0, 10, 36000, 189),
(181, 'cannon', 9, 500000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0.8, 880, 44.8, 'ground', 9, 0, 0, 10, 43200, 207),
(182, 'cannon', 10, 700000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0.8, 960, 51.2, 'ground', 9, 0, 0, 10, 64800, 254),
(183, 'cannon', 11, 1000000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0.8, 1060, 59.2, 'ground', 9, 0, 0, 10, 86400, 293),
(184, 'cannon', 12, 1200000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0.8, 1160, 69.6, 'ground', 9, 0, 0, 10, 108000, 328),
(185, 'cannon', 13, 1700000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0.8, 1260, 80, 'ground', 9, 0, 0, 10, 194400, 440),
(186, 'cannon', 14, 2100000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0.8, 1380, 88, 'ground', 9, 0, 0, 10, 259200, 509),
(187, 'cannon', 15, 3200000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0.8, 1500, 94.4, 'ground', 9, 0, 0, 10, 367200, 605),
(188, 'cannon', 16, 4900000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0.8, 1620, 99.2, 'ground', 9, 0, 0, 10, 540000, 734),
(189, 'cannon', 17, 6300000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0.8, 1740, 104, 'ground', 9, 0, 0, 10, 734400, 856),
(190, 'cannon', 18, 8200000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0.8, 1870, 111.2, 'ground', 9, 0, 0, 10, 972000, 985),
(191, 'cannon', 19, 9800000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0.8, 2000, 116.8, 'ground', 9, 0, 0, 10, 1036800, 1018),
(192, 'cannon', 20, 16500000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0.8, 2150, 123.2, 'ground', 9, 0, 0, 10, 1404000, 1184),
(193, 'cannon', 21, 18000000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0.8, 2250, 128, 'ground', 9, 0, 0, 10, 1468800, 1211),
(194, 'monolith', 1, 0, 0, 0, 300000, 3, 3, 0, 0, 0, 0, 1.5, 4747, 225, 'all', 11, 0, 0, 10, 1555200, 1247),
(195, 'monolith', 2, 0, 0, 0, 360000, 3, 3, 0, 0, 0, 0, 1.5, 5050, 300, 'all', 11, 0, 0, 10, 1641600, 1281),
(196, 'spelltower', 1, 14000000, 0, 0, 0, 2, 2, 0, 0, 0, 0, 50, 2500, 0, 'all', 9, 0, 0, 10, 1209600, 1099),
(197, 'spelltower', 2, 16000000, 0, 0, 0, 2, 2, 0, 0, 0, 0, 50, 2800, 0, 'all', 9, 0, 0, 10, 1382400, 1175),
(198, 'spelltower', 3, 18000000, 0, 0, 0, 2, 2, 0, 0, 0, 0, 50, 3100, 0, 'all', 9, 0, 0, 10, 1555200, 1247),
(199, 'scattershot', 1, 11200000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 3.228, 3600, 452, 'all', 10, 3, 0, 10, 972000, 985),
(200, 'scattershot', 2, 12800000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 3.228, 4200, 549, 'all', 10, 3, 0, 10, 1101600, 1049),
(201, 'scattershot', 3, 18000000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 3.228, 4800, 613, 'all', 10, 3, 0, 10, 1555200, 1247),
(202, 'eagleartillery', 1, 6000000, 0, 0, 0, 4, 4, 0, 0, 0, 0, 10, 4000, 300, 'all', 50, 7, 0.75, 10, 432000, 657),
(203, 'eagleartillery', 2, 8000000, 0, 0, 0, 4, 4, 0, 0, 0, 0, 10, 4400, 350, 'all', 50, 7, 0.75, 10, 734400, 856),
(204, 'eagleartillery', 3, 11000000, 0, 0, 0, 4, 4, 0, 0, 0, 0, 10, 4800, 400, 'all', 50, 7, 0.75, 10, 950400, 974),
(205, 'eagleartillery', 4, 15000000, 0, 0, 0, 4, 4, 0, 0, 0, 0, 10, 5200, 450, 'all', 50, 7, 0.75, 10, 1296000, 1138),
(206, 'eagleartillery', 5, 19000000, 0, 0, 0, 4, 4, 0, 0, 0, 0, 10, 5600, 500, 'all', 50, 7, 0.75, 10, 1641600, 1281),
(207, 'infernotower', 1, 1700000, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0.128, 1500, 3.84, 'all', 9, 0, 0, 10, 216000, 464),
(208, 'infernotower', 2, 2500000, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0.128, 1800, 4.48, 'all', 9, 0, 0, 10, 280800, 529),
(209, 'infernotower', 3, 3800000, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0.128, 2100, 5.248, 'all', 9, 0, 0, 10, 367200, 605),
(210, 'infernotower', 4, 4200000, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0.128, 2400, 7.296, 'all', 9, 0, 0, 10, 432000, 657),
(211, 'infernotower', 5, 5200000, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0.128, 2700, 8.96, 'all', 9, 0, 0, 10, 540000, 734),
(212, 'infernotower', 6, 7300000, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0.128, 3000, 10.368, 'all', 9, 0, 0, 10, 842400, 917),
(213, 'infernotower', 7, 12000000, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0.128, 3300, 11.904, 'all', 9, 0, 0, 10, 1101600, 1049),
(214, 'infernotower', 8, 18000000, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0.128, 3700, 13.44, 'all', 9, 0, 0, 10, 1555200, 1247),
(215, 'infernotower', 9, 21000000, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0.128, 4000, 14.72, 'all', 9, 0, 0, 10, 1641600, 1281),
(216, 'airsweeper', 1, 400000, 0, 0, 0, 2, 2, 0, 0, 0, 0, 5, 750, 1.6, 'air', 15, 0, 0, 10, 21600, 146),
(217, 'airsweeper', 2, 700000, 0, 0, 0, 2, 2, 0, 0, 0, 0, 5, 800, 2, 'air', 15, 0, 0, 10, 43200, 207),
(218, 'airsweeper', 3, 1100000, 0, 0, 0, 2, 2, 0, 0, 0, 0, 5, 850, 2.4, 'air', 15, 0, 0, 10, 64800, 254),
(219, 'airsweeper', 4, 1500000, 0, 0, 0, 2, 2, 0, 0, 0, 0, 5, 900, 2.8, 'air', 15, 0, 0, 10, 86400, 293),
(220, 'airsweeper', 5, 2000000, 0, 0, 0, 2, 2, 0, 0, 0, 0, 5, 950, 3.2, 'air', 15, 0, 0, 10, 172800, 415),
(221, 'airsweeper', 6, 2500000, 0, 0, 0, 2, 2, 0, 0, 0, 0, 5, 1000, 3.6, 'air', 15, 0, 0, 10, 216000, 464),
(222, 'airsweeper', 7, 4200000, 0, 0, 0, 2, 2, 0, 0, 0, 0, 5, 1050, 4, 'air', 15, 0, 0, 10, 302400, 549),
(223, 'wall', 1, 50, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 300, 0, 'none', 0, 0, 0, 0, 0, 0),
(224, 'wall', 2, 1000, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 500, 0, 'none', 0, 0, 0, 0, 0, 0),
(225, 'wall', 3, 5000, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 700, 0, 'none', 0, 0, 0, 0, 0, 0),
(226, 'wall', 4, 10000, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 900, 0, 'none', 0, 0, 0, 0, 0, 0),
(227, 'wall', 5, 20000, 20000, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1400, 0, 'none', 0, 0, 0, 0, 0, 0),
(228, 'wall', 6, 30000, 30000, 0, 0, 1, 1, 0, 0, 0, 0, 0, 2000, 0, 'none', 0, 0, 0, 0, 0, 0),
(229, 'wall', 7, 50000, 50000, 0, 0, 1, 1, 0, 0, 0, 0, 0, 2500, 0, 'none', 0, 0, 0, 0, 0, 0),
(230, 'wall', 8, 75000, 75000, 0, 0, 1, 1, 0, 0, 0, 0, 0, 3000, 0, 'none', 0, 0, 0, 0, 0, 0),
(231, 'wall', 9, 100000, 100000, 0, 0, 1, 1, 0, 0, 0, 0, 0, 3500, 0, 'none', 0, 0, 0, 0, 0, 0),
(232, 'wall', 10, 200000, 200000, 0, 0, 1, 1, 0, 0, 0, 0, 0, 4000, 0, 'none', 0, 0, 0, 0, 0, 0),
(233, 'wall', 11, 500000, 500000, 0, 0, 1, 1, 0, 0, 0, 0, 0, 5000, 0, 'none', 0, 0, 0, 0, 0, 0),
(234, 'wall', 12, 1000000, 1000000, 0, 0, 1, 1, 0, 0, 0, 0, 0, 7000, 0, 'none', 0, 0, 0, 0, 0, 0),
(235, 'wall', 13, 2000000, 2000000, 0, 0, 1, 1, 0, 0, 0, 0, 0, 9000, 0, 'none', 0, 0, 0, 0, 0, 0),
(236, 'wall', 14, 4000000, 4000000, 0, 0, 1, 1, 0, 0, 0, 0, 0, 11000, 0, 'none', 0, 0, 0, 0, 0, 0),
(237, 'wall', 15, 7000000, 7000000, 0, 0, 1, 1, 0, 0, 0, 0, 0, 12500, 0, 'none', 0, 0, 0, 0, 0, 0),
(238, 'wall', 16, 8000000, 8000000, 0, 0, 1, 1, 0, 0, 0, 0, 0, 13500, 0, 'none', 0, 0, 0, 0, 0, 0),
(239, 'archertower', 1, 1000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0.5, 380, 5.5, 'all', 10, 0, 0, 10, 60, 7),
(240, 'archertower', 2, 2000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0.5, 420, 7.5, 'all', 10, 0, 0, 10, 900, 30),
(241, 'archertower', 3, 5000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0.5, 460, 9.5, 'all', 10, 0, 0, 10, 2700, 51),
(242, 'archertower', 4, 20000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0.5, 500, 12.5, 'all', 10, 0, 0, 10, 10800, 103),
(243, 'archertower', 5, 80000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0.5, 540, 15, 'all', 10, 0, 0, 10, 18000, 134),
(244, 'archertower', 6, 180000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0.5, 580, 17.5, 'all', 10, 0, 0, 10, 28800, 169),
(245, 'archertower', 7, 360000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0.5, 630, 21, 'all', 10, 0, 0, 10, 36000, 189),
(246, 'archertower', 8, 600000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0.5, 690, 24, 'all', 10, 0, 0, 10, 43200, 207),
(247, 'archertower', 9, 800000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0.5, 750, 28, 'all', 10, 0, 0, 10, 50400, 224),
(248, 'archertower', 10, 1000000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0.5, 810, 31.5, 'all', 10, 0, 0, 10, 64800, 254),
(249, 'archertower', 11, 1200000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0.5, 890, 35, 'all', 10, 0, 0, 10, 86400, 293),
(250, 'archertower', 12, 1500000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0.5, 970, 37.5, 'all', 10, 0, 0, 10, 108000, 328),
(251, 'archertower', 13, 2000000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0.5, 1050, 40, 'all', 10, 0, 0, 10, 194400, 440),
(252, 'archertower', 14, 2500000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0.5, 1130, 46, 'all', 10, 0, 0, 10, 302400, 549),
(253, 'archertower', 15, 3500000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0.5, 1230, 52, 'all', 10, 0, 0, 10, 432000, 657),
(254, 'archertower', 16, 5300000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0.5, 1330, 56, 'all', 10, 0, 0, 10, 540000, 734),
(255, 'archertower', 17, 6800000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0.5, 1410, 60, 'all', 10, 0, 0, 10, 734400, 856),
(256, 'archertower', 18, 8500000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0.5, 1510, 64, 'all', 10, 0, 0, 10, 972000, 985),
(257, 'archertower', 19, 9800000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0.5, 1600, 67, 'all', 10, 0, 0, 10, 1036800, 1018),
(258, 'archertower', 20, 16500000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0.5, 1700, 70, 'all', 10, 0, 0, 10, 1404000, 1184),
(259, 'archertower', 21, 18500000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0.5, 1800, 72.5, 'all', 10, 0, 0, 10, 1468800, 1211),
(260, 'mortor', 1, 5000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 5, 400, 20, 'ground', 11, 4, 1, 5, 10800, 103),
(261, 'mortor', 2, 25000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 5, 450, 25, 'ground', 11, 4, 1, 5, 21600, 146),
(262, 'mortor', 3, 100000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 5, 500, 30, 'ground', 11, 4, 1, 5, 43200, 207),
(263, 'mortor', 4, 200000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 5, 550, 35, 'ground', 11, 4, 1, 5, 86400, 293),
(264, 'mortor', 5, 400000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 5, 600, 45, 'ground', 11, 4, 1, 5, 172800, 415),
(265, 'mortor', 6, 750000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 5, 650, 55, 'ground', 11, 4, 1, 5, 216000, 464),
(266, 'mortor', 7, 1500000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 5, 700, 75, 'ground', 11, 4, 1, 5, 259200, 509),
(267, 'mortor', 8, 2500000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 5, 800, 100, 'ground', 11, 4, 1, 5, 280800, 529),
(268, 'mortor', 9, 3500000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 5, 950, 125, 'ground', 11, 4, 1, 5, 367200, 605),
(269, 'mortor', 10, 4800000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 5, 1100, 150, 'ground', 11, 4, 1, 5, 475200, 689),
(270, 'mortor', 11, 6500000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 5, 1300, 175, 'ground', 11, 4, 1, 5, 734400, 856),
(271, 'mortor', 12, 7200000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 5, 1500, 190, 'ground', 11, 4, 1, 5, 842400, 917),
(272, 'mortor', 13, 9800000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 5, 1700, 210, 'ground', 11, 4, 1, 5, 1036800, 1018),
(273, 'mortor', 14, 16500000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 5, 1950, 240, 'ground', 11, 4, 1, 5, 1468800, 1211),
(274, 'mortor', 15, 19000000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 5, 2150, 270, 'ground', 11, 4, 1, 5, 1555200, 1247),
(275, 'airdefense', 1, 22000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 1, 800, 80, 'air', 10, 0, 0, 10, 10800, 103),
(276, 'airdefense', 2, 90000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 1, 850, 110, 'air', 10, 0, 0, 10, 43200, 207),
(277, 'airdefense', 3, 270000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 1, 900, 140, 'air', 10, 0, 0, 10, 57600, 240),
(278, 'airdefense', 4, 500000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 1, 950, 160, 'air', 10, 0, 0, 10, 86400, 293),
(279, 'airdefense', 5, 1000000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 1, 1000, 190, 'air', 10, 0, 0, 10, 129600, 360),
(280, 'airdefense', 6, 1350000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 1, 1050, 230, 'air', 10, 0, 0, 10, 172800, 415),
(281, 'airdefense', 7, 1750000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 1, 1100, 280, 'air', 10, 0, 0, 10, 259200, 509),
(282, 'airdefense', 8, 3000000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 1, 1210, 320, 'air', 10, 0, 0, 10, 367200, 605),
(283, 'airdefense', 9, 4200000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 1, 1300, 360, 'air', 10, 0, 0, 10, 540000, 734),
(284, 'airdefense', 10, 6500000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 1, 1400, 400, 'air', 10, 0, 0, 10, 842400, 917),
(285, 'airdefense', 11, 10500000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 1, 1500, 440, 'air', 10, 0, 0, 10, 1036800, 1018),
(286, 'airdefense', 12, 17000000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 1, 1650, 500, 'air', 10, 0, 0, 10, 1468800, 1211),
(287, 'airdefense', 13, 19500000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 1, 1750, 540, 'air', 10, 0, 0, 10, 1555200, 1247),
(288, 'wizardtower', 1, 120000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 1.3, 620, 14.3, 'all', 7, 0, 1, 10, 10800, 103),
(289, 'wizardtower', 2, 220000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 1.3, 650, 16.9, 'all', 7, 0, 1, 10, 28800, 169),
(290, 'wizardtower', 3, 420000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 1.3, 680, 20.8, 'all', 7, 0, 1, 10, 43200, 207),
(291, 'wizardtower', 4, 720000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 1.3, 730, 26, 'all', 7, 0, 1, 10, 64800, 254),
(292, 'wizardtower', 5, 920000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 1.3, 840, 31.2, 'all', 7, 0, 1, 10, 86400, 293),
(293, 'wizardtower', 6, 1200000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 1.3, 960, 41.6, 'all', 7, 0, 1, 10, 129600, 360),
(294, 'wizardtower', 7, 2200000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 1.3, 1200, 52, 'all', 7, 0, 1, 10, 172800, 415),
(295, 'wizardtower', 8, 2700000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 1.3, 1440, 58.5, 'all', 7, 0, 1, 10, 259200, 509),
(296, 'wizardtower', 9, 3700000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 1.3, 1680, 65, 'all', 7, 0, 1, 10, 453600, 673),
(297, 'wizardtower', 10, 5200000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 1.3, 2000, 80.6, 'all', 7, 0, 1, 10, 561600, 749),
(298, 'wizardtower', 11, 7200000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 1.3, 2240, 91, 'all', 7, 0, 1, 10, 842400, 917),
(299, 'wizardtower', 12, 10700000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 1.3, 2480, 101.4, 'all', 7, 0, 1, 10, 1036800, 1018),
(300, 'wizardtower', 13, 12200000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 1.3, 2700, 109.2, 'all', 7, 0, 1, 10, 1101600, 1049),
(301, 'wizardtower', 14, 17200000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 1.3, 2900, 117, 'all', 7, 0, 1, 10, 1468800, 1211),
(302, 'wizardtower', 15, 19200000, 0, 0, 0, 3, 3, 0, 0, 0, 0, 1.3, 3000, 123.5, 'all', 7, 0, 1, 10, 1555200, 1247),
(303, 'obstacle', 1, 0, 1000, 0, 0, 2, 2, 0, 0, 0, 0, 0, 100, 0, 'none', 0, 0, 0, 0, 10, 0),
(304, 'obstacle', 2, 0, 1000, 0, 0, 2, 2, 0, 0, 0, 0, 0, 100, 0, 'none', 0, 0, 0, 0, 10, 0),
(305, 'obstacle', 3, 0, 1000, 0, 0, 2, 2, 0, 0, 0, 0, 0, 100, 0, 'none', 0, 0, 0, 0, 10, 0),
(306, 'obstacle', 4, 0, 1000, 0, 0, 2, 2, 0, 0, 0, 0, 0, 100, 0, 'none', 0, 0, 0, 0, 10, 0),
(307, 'obstacle', 5, 0, 1000, 0, 0, 2, 2, 0, 0, 0, 0, 0, 100, 0, 'none', 0, 0, 0, 0, 10, 0),
(308, 'obstacle', 6, 0, 1000, 0, 0, 2, 2, 0, 0, 0, 0, 0, 100, 0, 'none', 0, 0, 0, 0, 10, 0);

-- --------------------------------------------------------

--
-- Table structure for table `server_quest_battles`
--

CREATE TABLE `server_quest_battles` (
  `id` bigint(20) NOT NULL,
  `category` int(11) NOT NULL DEFAULT 0,
  `order_index` int(11) NOT NULL,
  `class_dara` mediumtext NOT NULL DEFAULT ''
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- --------------------------------------------------------

--
-- Table structure for table `server_spells`
--

CREATE TABLE `server_spells` (
  `id` int(11) NOT NULL,
  `global_id` varchar(50) NOT NULL DEFAULT '',
  `level` int(11) NOT NULL DEFAULT 1,
  `building_code` int(11) NOT NULL DEFAULT 0,
  `req_gold` int(11) NOT NULL DEFAULT 0,
  `req_elixir` int(11) NOT NULL DEFAULT 0,
  `req_gem` int(11) NOT NULL DEFAULT 0,
  `req_dark_elixir` int(11) NOT NULL DEFAULT 0,
  `brew_time` int(11) NOT NULL DEFAULT 0,
  `housing` int(11) NOT NULL DEFAULT 1,
  `radius` float NOT NULL DEFAULT 1,
  `pulses_count` int(11) NOT NULL DEFAULT 0,
  `pulses_duration` float NOT NULL DEFAULT 0,
  `pulses_value` float NOT NULL DEFAULT 0,
  `pulses_value_2` float NOT NULL DEFAULT 0,
  `research_time` int(11) NOT NULL DEFAULT 0,
  `research_gold` int(11) NOT NULL DEFAULT 0,
  `research_elixir` int(11) NOT NULL DEFAULT 0,
  `research_dark_elixir` int(11) NOT NULL DEFAULT 0,
  `research_gems` int(11) NOT NULL DEFAULT 0,
  `research_xp` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Dumping data for table `server_spells`
--

INSERT INTO `server_spells` (`id`, `global_id`, `level`, `building_code`, `req_gold`, `req_elixir`, `req_gem`, `req_dark_elixir`, `brew_time`, `housing`, `radius`, `pulses_count`, `pulses_duration`, `pulses_value`, `pulses_value_2`, `research_time`, `research_gold`, `research_elixir`, `research_dark_elixir`, `research_gems`, `research_xp`) VALUES
(1, 'lightning', 1, 0, 0, 15000, 0, 0, 180, 1, 2, 6, 0.4, 25, 0, 0, 0, 0, 0, 0, 0),
(2, 'lightning', 2, 0, 0, 16500, 0, 0, 180, 1, 2, 6, 0.4, 30, 0, 14400, 0, 50000, 0, 0, 120),
(3, 'lightning', 3, 0, 0, 18000, 0, 0, 180, 1, 2, 6, 0.4, 35, 0, 28800, 0, 100000, 0, 0, 169),
(4, 'lightning', 4, 0, 0, 20000, 0, 0, 180, 1, 2, 6, 0.4, 40, 0, 43200, 0, 200000, 0, 0, 207),
(5, 'lightning', 5, 0, 0, 22000, 0, 0, 180, 1, 2, 6, 0.4, 45, 0, 86400, 0, 600000, 0, 0, 293),
(6, 'lightning', 6, 0, 0, 24000, 0, 0, 180, 1, 2, 6, 0.4, 53, 0, 345600, 0, 1500000, 0, 0, 587),
(7, 'lightning', 7, 0, 0, 26000, 0, 0, 180, 1, 2, 6, 0.4, 67, 0, 518400, 0, 2500000, 0, 0, 720),
(8, 'lightning', 8, 0, 0, 28000, 0, 0, 180, 1, 2, 6, 0.4, 80, 0, 626400, 0, 4800000, 0, 0, 791),
(9, 'lightning', 9, 0, 0, 30000, 0, 0, 180, 1, 2, 6, 0.4, 93, 0, 777600, 0, 7000000, 0, 0, 881),
(10, 'lightning', 10, 0, 0, 32000, 0, 0, 180, 1, 2, 6, 0.4, 100, 0, 1382400, 0, 16000000, 0, 0, 1175),
(11, 'healing', 1, 0, 0, 15000, 0, 0, 360, 2, 5, 40, 0.3, 15, 0, 0, 0, 0, 0, 0, 0),
(12, 'healing', 2, 0, 0, 16500, 0, 0, 360, 2, 5, 40, 0.3, 20, 0, 18000, 0, 75000, 0, 0, 134),
(13, 'healing', 3, 0, 0, 18000, 0, 0, 360, 2, 5, 40, 0.3, 25, 0, 36000, 0, 150000, 0, 0, 189),
(14, 'healing', 4, 0, 0, 20000, 0, 0, 360, 2, 5, 40, 0.3, 30, 0, 72000, 0, 300000, 0, 0, 268),
(15, 'healing', 5, 0, 0, 22000, 0, 0, 360, 2, 5, 40, 0.3, 35, 0, 129600, 0, 900000, 0, 0, 360),
(16, 'healing', 6, 0, 0, 24000, 0, 0, 360, 2, 5, 40, 0.3, 40, 0, 345600, 0, 1800000, 0, 0, 587),
(17, 'healing', 7, 0, 0, 26000, 0, 0, 360, 2, 5, 40, 0.3, 45, 0, 518400, 0, 3000000, 0, 0, 720),
(18, 'healing', 8, 0, 0, 28000, 0, 0, 360, 2, 5, 40, 0.3, 50, 0, 1036800, 0, 10500000, 0, 0, 1018),
(19, 'healing', 9, 0, 0, 30000, 0, 0, 360, 2, 5, 40, 0.3, 55, 0, 1468800, 0, 17000000, 0, 0, 1211),
(20, 'rage', 1, 0, 0, 23000, 0, 0, 360, 2, 5, 60, 0.3, 1.3, 2, 0, 0, 0, 0, 0, 0),
(21, 'rage', 2, 0, 0, 25000, 0, 0, 360, 2, 5, 60, 0.3, 1.4, 2.2, 43200, 0, 400000, 0, 0, 207),
(22, 'rage', 3, 0, 0, 27000, 0, 0, 360, 2, 5, 60, 0.3, 1.5, 2.4, 86400, 0, 800000, 0, 0, 293),
(23, 'rage', 4, 0, 0, 30000, 0, 0, 360, 2, 5, 60, 0.3, 1.6, 2.6, 172800, 0, 1600000, 0, 0, 415),
(24, 'rage', 5, 0, 0, 33000, 0, 0, 360, 2, 5, 60, 0.3, 1.7, 2.8, 345600, 0, 2400000, 0, 0, 587),
(25, 'rage', 6, 0, 0, 37000, 0, 0, 360, 2, 5, 60, 0.3, 1.8, 3, 691200, 0, 7700000, 0, 0, 831),
(26, 'jump', 1, 0, 0, 23000, 0, 0, 360, 2, 3.5, 1, 20, 0, 0, 0, 0, 0, 0, 0, 0),
(27, 'jump', 2, 0, 0, 27000, 0, 0, 360, 2, 3.5, 1, 40, 0, 0, 345600, 0, 2000000, 0, 0, 0),
(28, 'jump', 3, 0, 0, 31000, 0, 0, 360, 2, 3.5, 1, 60, 0, 0, 518400, 0, 3400000, 0, 0, 0),
(29, 'jump', 4, 0, 0, 35000, 0, 0, 360, 2, 3.5, 1, 80, 0, 0, 972000, 0, 9000000, 0, 0, 0),
(30, 'jump', 5, 0, 0, 40000, 0, 0, 360, 2, 3.5, 1, 100, 0, 0, 1425600, 0, 16500000, 0, 0, 0),
(31, 'freeze', 1, 0, 0, 6000, 0, 0, 180, 1, 1.75, 1, 2.5, 0, 0, 0, 0, 0, 0, 0, 0),
(32, 'freeze', 2, 0, 0, 7000, 0, 0, 180, 1, 1.75, 1, 3, 0, 0, 129600, 0, 1200000, 0, 0, 360),
(33, 'freeze', 3, 0, 0, 8000, 0, 0, 180, 1, 1.75, 1, 3.5, 0, 0, 223200, 0, 1700000, 0, 0, 472),
(34, 'freeze', 4, 0, 0, 9000, 0, 0, 180, 1, 1.75, 1, 4, 0, 0, 388800, 0, 3000000, 0, 0, 605),
(35, 'freeze', 5, 0, 0, 10000, 0, 0, 180, 1, 1.75, 1, 4.5, 0, 0, 518400, 0, 4200000, 0, 0, 720),
(36, 'freeze', 6, 0, 0, 11000, 0, 0, 180, 1, 1.75, 1, 5, 0, 0, 669600, 0, 6000000, 0, 0, 818),
(37, 'freeze', 7, 0, 0, 12000, 0, 0, 180, 1, 1.75, 1, 5.5, 0, 0, 712800, 0, 7700000, 0, 0, 844),
(38, 'invisibility', 1, 0, 0, 11000, 0, 0, 180, 1, 4, 1, 3.75, 0, 0, 0, 0, 0, 0, 0, 0),
(39, 'invisibility', 2, 0, 0, 12000, 0, 0, 180, 1, 4, 1, 4, 0, 0, 475200, 0, 5600000, 0, 0, 689),
(40, 'invisibility', 3, 0, 0, 13000, 0, 0, 180, 1, 4, 1, 4.25, 0, 0, 691200, 0, 8400000, 0, 0, 831),
(41, 'invisibility', 4, 0, 0, 14000, 0, 0, 180, 1, 4, 1, 4.5, 0, 0, 1015200, 0, 11300000, 0, 0, 1007),
(42, 'recall', 1, 0, 0, 0, 0, 0, 360, 2, 5, 1, 0, 83, 0, 0, 0, 0, 0, 0, 0),
(43, 'recall', 2, 0, 0, 0, 0, 0, 360, 2, 5, 1, 0, 89, 0, 993600, 0, 7500000, 0, 0, 0),
(44, 'recall', 3, 0, 0, 0, 0, 0, 360, 2, 5, 1, 0, 95, 0, 1339200, 0, 14000000, 0, 0, 0),
(45, 'recall', 4, 0, 0, 0, 0, 0, 360, 2, 5, 1, 0, 101, 0, 1512000, 0, 17500000, 0, 0, 0),
(46, 'earthquake', 1, 1, 0, 0, 0, 0, 180, 1, 3.5, 0, 0, 0.145, 0, 0, 0, 0, 0, 0, 0),
(47, 'earthquake', 2, 1, 0, 0, 0, 0, 180, 1, 3.8, 0, 0, 0.17, 0, 64800, 0, 0, 15000, 0, 0),
(48, 'earthquake', 3, 1, 0, 0, 0, 0, 180, 1, 4.1, 0, 0, 0.21, 0, 129600, 0, 0, 30000, 0, 0),
(49, 'earthquake', 4, 1, 0, 0, 0, 0, 180, 1, 4.4, 0, 0, 0.25, 0, 367200, 0, 0, 51000, 0, 0),
(50, 'earthquake', 5, 1, 0, 0, 0, 0, 180, 1, 4.7, 0, 0, 0.29, 0, 669600, 0, 0, 84000, 0, 0),
(51, 'haste', 1, 1, 0, 0, 0, 0, 180, 1, 4, 1, 10, 2.8, 0, 0, 0, 0, 0, 0, 0),
(52, 'haste', 2, 1, 0, 0, 0, 0, 180, 1, 4, 1, 15, 3.4, 0, 129600, 0, 0, 20000, 0, 0),
(53, 'haste', 3, 1, 0, 0, 0, 0, 180, 1, 4, 1, 20, 4, 0, 223200, 0, 0, 34000, 0, 0),
(54, 'haste', 4, 1, 0, 0, 0, 0, 180, 1, 4, 1, 25, 4.6, 0, 432000, 0, 0, 60000, 0, 0),
(55, 'haste', 5, 1, 0, 0, 0, 0, 180, 1, 4, 1, 30, 5.2, 0, 669600, 0, 0, 77000, 0, 0),
(56, 'skeleton', 1, 1, 0, 0, 0, 0, 180, 1, 3.5, 1, 0, 12, 0, 0, 0, 0, 0, 0, 0),
(57, 'skeleton', 2, 1, 0, 0, 0, 0, 180, 1, 3.5, 1, 0, 13, 0, 115200, 0, 0, 22000, 0, 0),
(58, 'skeleton', 3, 1, 0, 0, 0, 0, 180, 1, 3.5, 1, 0, 14, 0, 216000, 0, 0, 34000, 0, 0),
(59, 'skeleton', 4, 1, 0, 0, 0, 0, 180, 1, 3.5, 1, 0, 15, 0, 367200, 0, 0, 50000, 0, 0),
(60, 'skeleton', 5, 1, 0, 0, 0, 0, 180, 1, 3.5, 1, 0, 16, 0, 518400, 0, 0, 87000, 0, 0),
(61, 'skeleton', 6, 1, 0, 0, 0, 0, 180, 1, 3.5, 1, 0, 17, 0, 626400, 0, 0, 105000, 0, 0),
(62, 'skeleton', 7, 1, 0, 0, 0, 0, 180, 1, 3.5, 1, 0, 18, 0, 972000, 0, 0, 187000, 0, 0),
(63, 'bat', 1, 1, 0, 0, 0, 0, 180, 1, 3.5, 1, 0, 7, 0, 0, 0, 0, 0, 0, 0),
(64, 'bat', 2, 1, 0, 0, 0, 0, 180, 1, 3.5, 1, 0, 9, 0, 151200, 0, 0, 26000, 0, 0),
(65, 'bat', 3, 1, 0, 0, 0, 0, 180, 1, 3.5, 1, 0, 11, 0, 302400, 0, 0, 51000, 0, 0),
(66, 'bat', 4, 1, 0, 0, 0, 0, 180, 1, 3.5, 1, 0, 16, 0, 453600, 0, 0, 70000, 0, 0),
(67, 'bat', 5, 1, 0, 0, 0, 0, 180, 1, 3.5, 1, 0, 21, 0, 540000, 0, 0, 105000, 0, 0),
(68, 'bat', 6, 1, 0, 0, 0, 0, 180, 1, 3.5, 1, 0, 22, 0, 1555200, 0, 0, 330000, 0, 0);

-- --------------------------------------------------------

--
-- Table structure for table `server_units`
--

CREATE TABLE `server_units` (
  `id` int(11) NOT NULL,
  `global_id` varchar(50) NOT NULL DEFAULT '',
  `level` int(11) NOT NULL DEFAULT 1,
  `building_code` int(11) NOT NULL DEFAULT 0,
  `req_gold` int(11) NOT NULL DEFAULT 0,
  `req_elixir` int(11) NOT NULL DEFAULT 0,
  `req_gem` int(11) NOT NULL DEFAULT 0,
  `req_dark_elixir` int(11) NOT NULL DEFAULT 0,
  `train_time` int(11) NOT NULL DEFAULT 0,
  `health` int(11) NOT NULL DEFAULT 10,
  `housing` int(11) NOT NULL DEFAULT 1,
  `damage` float NOT NULL DEFAULT 1,
  `attack_range` float NOT NULL DEFAULT 0,
  `attack_speed` float NOT NULL DEFAULT 1,
  `splash_range` float NOT NULL DEFAULT 0,
  `projectile_speed` float NOT NULL DEFAULT 0,
  `move_speed` float NOT NULL DEFAULT 1,
  `move_type` varchar(50) NOT NULL DEFAULT 'ground',
  `target_priority` varchar(50) NOT NULL DEFAULT 'all',
  `priority_multiplier` float NOT NULL DEFAULT 1,
  `research_time` int(11) NOT NULL DEFAULT 0,
  `research_gold` int(11) NOT NULL DEFAULT 0,
  `research_elixir` int(11) NOT NULL DEFAULT 0,
  `research_dark_elixir` int(11) NOT NULL DEFAULT 0,
  `research_gems` int(11) NOT NULL DEFAULT 0,
  `research_xp` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Dumping data for table `server_units`
--

INSERT INTO `server_units` (`id`, `global_id`, `level`, `building_code`, `req_gold`, `req_elixir`, `req_gem`, `req_dark_elixir`, `train_time`, `health`, `housing`, `damage`, `attack_range`, `attack_speed`, `splash_range`, `projectile_speed`, `move_speed`, `move_type`, `target_priority`, `priority_multiplier`, `research_time`, `research_gold`, `research_elixir`, `research_dark_elixir`, `research_gems`, `research_xp`) VALUES
(1, 'barbarian', 1, 0, 0, 25, 0, 0, 5, 45, 1, 8, 0, 1, 0, 0, 2, 'ground', 'all', 1, 0, 0, 0, 0, 0, 0),
(2, 'barbarian', 2, 0, 0, 40, 0, 0, 5, 54, 1, 11, 0, 1, 0, 0, 2, 'ground', 'all', 1, 7200, 0, 20000, 0, 0, 84),
(3, 'barbarian', 3, 0, 0, 60, 0, 0, 5, 65, 1, 14, 0, 1, 0, 0, 2, 'ground', 'all', 1, 18000, 0, 60000, 0, 0, 134),
(4, 'barbarian', 4, 0, 0, 100, 0, 0, 5, 85, 1, 18, 0, 1, 0, 0, 2, 'ground', 'all', 1, 43200, 0, 200000, 0, 0, 207),
(5, 'barbarian', 5, 0, 0, 150, 0, 0, 5, 105, 1, 23, 0, 1, 0, 0, 2, 'ground', 'all', 1, 86400, 0, 650000, 0, 0, 293),
(6, 'barbarian', 6, 0, 0, 200, 0, 0, 5, 125, 1, 26, 0, 1, 0, 0, 2, 'ground', 'all', 1, 129600, 0, 1400000, 0, 0, 360),
(7, 'barbarian', 7, 0, 0, 250, 0, 0, 5, 160, 1, 30, 0, 1, 0, 0, 2, 'ground', 'all', 1, 216000, 0, 2100000, 0, 0, 464),
(8, 'barbarian', 8, 0, 0, 300, 0, 0, 5, 205, 1, 34, 0, 1, 0, 0, 2, 'ground', 'all', 1, 259200, 0, 2800000, 0, 0, 509),
(9, 'barbarian', 9, 0, 0, 350, 0, 0, 5, 230, 1, 38, 0, 1, 0, 0, 2, 'ground', 'all', 1, 604800, 0, 5600000, 0, 0, 777),
(10, 'barbarian', 10, 0, 0, 400, 0, 0, 5, 250, 1, 42, 0, 1, 0, 0, 2, 'ground', 'all', 1, 1123200, 0, 14000000, 0, 0, 1059),
(11, 'barbarian', 11, 0, 0, 450, 0, 0, 5, 270, 1, 45, 0, 1, 0, 0, 2, 'ground', 'all', 1, 1209600, 0, 16000000, 0, 0, 1099),
(12, 'archer', 1, 0, 0, 50, 0, 0, 6, 20, 1, 7, 3.5, 1, 0, 5, 3, 'ground', 'all', 1, 0, 0, 0, 0, 0, 0),
(13, 'archer', 2, 0, 0, 80, 0, 0, 6, 23, 1, 9, 3.5, 1, 0, 5, 3, 'ground', 'all', 1, 10800, 0, 30000, 0, 0, 103),
(14, 'archer', 3, 0, 0, 120, 0, 0, 6, 28, 1, 12, 3.5, 1, 0, 5, 3, 'ground', 'all', 1, 21600, 0, 80000, 0, 0, 146),
(15, 'archer', 4, 0, 0, 200, 0, 0, 6, 33, 1, 16, 3.5, 1, 0, 5, 3, 'ground', 'all', 1, 43200, 0, 300000, 0, 0, 207),
(16, 'archer', 5, 0, 0, 300, 0, 0, 6, 40, 1, 20, 3.5, 1, 0, 5, 3, 'ground', 'all', 1, 86400, 0, 800000, 0, 0, 293),
(17, 'archer', 6, 0, 0, 400, 0, 0, 6, 44, 1, 22, 3.5, 1, 0, 5, 3, 'ground', 'all', 1, 129600, 0, 2000000, 0, 0, 360),
(18, 'archer', 7, 0, 0, 500, 0, 0, 6, 48, 1, 25, 3.5, 1, 0, 5, 3, 'ground', 'all', 1, 216000, 0, 2500000, 0, 0, 464),
(19, 'archer', 8, 0, 0, 600, 0, 0, 6, 52, 1, 28, 3.5, 1, 0, 5, 3, 'ground', 'all', 1, 302400, 0, 3200000, 0, 0, 549),
(20, 'archer', 9, 0, 0, 700, 0, 0, 6, 56, 1, 31, 3.5, 1, 0, 5, 3, 'ground', 'all', 1, 604800, 0, 6300000, 0, 0, 777),
(21, 'archer', 10, 0, 0, 800, 0, 0, 6, 60, 1, 34, 3.5, 1, 0, 5, 3, 'ground', 'all', 1, 1123200, 0, 14500000, 0, 0, 1059),
(22, 'archer', 11, 0, 0, 900, 0, 0, 6, 64, 1, 37, 3.5, 1, 0, 5, 3, 'ground', 'all', 1, 1209600, 0, 16000000, 0, 0, 1099),
(23, 'goblin', 1, 0, 0, 25, 0, 0, 7, 25, 1, 11, 0, 1, 0, 0, 4, 'ground', 'resources', 2, 0, 0, 0, 0, 0, 0),
(24, 'goblin', 2, 0, 0, 40, 0, 0, 7, 30, 1, 14, 0, 1, 0, 0, 4, 'ground', 'resources', 2, 18000, 0, 45000, 0, 0, 134),
(25, 'goblin', 3, 0, 0, 60, 0, 0, 7, 36, 1, 19, 0, 1, 0, 0, 4, 'ground', 'resources', 2, 32400, 0, 175000, 0, 0, 180),
(26, 'goblin', 4, 0, 0, 80, 0, 0, 7, 50, 1, 24, 0, 1, 0, 0, 4, 'ground', 'resources', 2, 43200, 0, 500000, 0, 0, 207),
(27, 'goblin', 5, 0, 0, 100, 0, 0, 7, 65, 1, 32, 0, 1, 0, 0, 4, 'ground', 'resources', 2, 86400, 0, 1200000, 0, 0, 293),
(28, 'goblin', 6, 0, 0, 150, 0, 0, 7, 80, 1, 42, 0, 1, 0, 0, 4, 'ground', 'resources', 2, 129600, 0, 2000000, 0, 0, 360),
(29, 'goblin', 7, 0, 0, 200, 0, 0, 7, 105, 1, 52, 0, 1, 0, 0, 4, 'ground', 'resources', 2, 302400, 0, 3000000, 0, 0, 549),
(30, 'goblin', 8, 0, 0, 300, 0, 0, 7, 126, 1, 62, 0, 1, 0, 0, 4, 'ground', 'resources', 2, 691200, 0, 6300000, 0, 0, 831),
(31, 'giant', 1, 0, 0, 250, 0, 0, 30, 300, 5, 22, 0, 2, 0, 0, 1.5, 'ground', 'defenses', 1, 0, 0, 0, 0, 0, 0),
(32, 'giant', 2, 0, 0, 750, 0, 0, 30, 360, 5, 28, 0, 2, 0, 0, 1.5, 'ground', 'defenses', 1, 14400, 0, 40000, 0, 0, 120),
(33, 'giant', 3, 0, 0, 1250, 0, 0, 30, 450, 5, 38, 0, 2, 0, 0, 1.5, 'ground', 'defenses', 1, 28800, 0, 150000, 0, 0, 169),
(34, 'giant', 4, 0, 0, 1750, 0, 0, 30, 600, 5, 48, 0, 2, 0, 0, 1.5, 'ground', 'defenses', 1, 43200, 0, 500000, 0, 0, 207),
(35, 'giant', 5, 0, 0, 2250, 0, 0, 30, 800, 5, 62, 0, 2, 0, 0, 1.5, 'ground', 'defenses', 1, 86400, 0, 1200000, 0, 0, 293),
(36, 'giant', 6, 0, 0, 3000, 0, 0, 30, 1100, 5, 86, 0, 2, 0, 0, 1.5, 'ground', 'defenses', 1, 172800, 0, 2000000, 0, 0, 415),
(37, 'giant', 7, 0, 0, 3500, 0, 0, 30, 1300, 5, 110, 0, 2, 0, 0, 1.5, 'ground', 'defenses', 1, 302400, 0, 3000000, 0, 0, 549),
(38, 'giant', 8, 0, 0, 4000, 0, 0, 30, 1500, 5, 124, 0, 2, 0, 0, 1.5, 'ground', 'defenses', 1, 475200, 0, 3500000, 0, 0, 689),
(39, 'giant', 9, 0, 0, 4500, 0, 0, 30, 1850, 5, 140, 0, 2, 0, 0, 1.5, 'ground', 'defenses', 1, 777600, 0, 6300000, 0, 0, 881),
(40, 'giant', 10, 0, 0, 5000, 0, 0, 30, 2000, 5, 156, 0, 2, 0, 0, 1.5, 'ground', 'defenses', 1, 972000, 0, 10000000, 0, 0, 985),
(41, 'giant', 11, 0, 0, 5500, 0, 0, 30, 2200, 5, 172, 0, 2, 0, 0, 1.5, 'ground', 'defenses', 1, 1382400, 0, 16500000, 0, 0, 1175),
(42, 'healer', 1, 0, 0, 5000, 0, 0, 120, 500, 14, 25.2, 5, 0.7, 1.5, 5, 2, 'fly', 'none', 1, 0, 0, 0, 0, 0, 0),
(43, 'healer', 2, 0, 0, 6000, 0, 0, 120, 700, 14, 33.6, 5, 0.7, 1.5, 5, 2, 'fly', 'none', 1, 43200, 0, 450000, 0, 0, 207),
(44, 'healer', 3, 0, 0, 8000, 0, 0, 120, 900, 14, 42, 5, 0.7, 1.5, 5, 2, 'fly', 'none', 1, 86400, 0, 900000, 0, 0, 293),
(45, 'healer', 4, 0, 0, 10000, 0, 0, 120, 1200, 14, 46.2, 5, 0.7, 1.5, 5, 2, 'fly', 'none', 1, 172800, 0, 2700000, 0, 0, 415),
(46, 'healer', 5, 0, 0, 14000, 0, 0, 120, 1500, 14, 50.4, 5, 0.7, 1.5, 5, 2, 'fly', 'none', 1, 604800, 0, 4200000, 0, 0, 777),
(47, 'healer', 6, 0, 0, 17000, 0, 0, 120, 1600, 14, 50.4, 5, 0.7, 1.5, 5, 2, 'fly', 'none', 1, 907200, 0, 9800000, 0, 0, 952),
(48, 'healer', 7, 0, 0, 20000, 0, 0, 120, 1700, 14, 50.4, 5, 0.7, 1.5, 5, 2, 'fly', 'none', 1, 1382400, 0, 16000000, 0, 0, 1175),
(49, 'wallbreaker', 1, 0, 0, 600, 0, 0, 15, 20, 2, 6, 0, 1, 2, 0, 3, 'ground', 'walls', 40, 0, 0, 0, 0, 0, 0),
(50, 'wallbreaker', 2, 0, 0, 800, 0, 0, 15, 24, 2, 10, 0, 1, 2, 0, 3, 'ground', 'walls', 40, 21600, 0, 100000, 0, 0, 146),
(51, 'wallbreaker', 3, 0, 0, 1000, 0, 0, 15, 29, 2, 15, 0, 1, 2, 0, 3, 'ground', 'walls', 40, 43200, 0, 250000, 0, 0, 207),
(52, 'wallbreaker', 4, 0, 0, 1200, 0, 0, 15, 35, 2, 20, 0, 1, 2, 0, 3, 'ground', 'walls', 40, 64800, 0, 600000, 0, 0, 254),
(53, 'wallbreaker', 5, 0, 0, 1400, 0, 0, 15, 53, 2, 43, 0, 1, 2, 0, 3, 'ground', 'walls', 40, 86400, 0, 1200000, 0, 0, 293),
(54, 'wallbreaker', 6, 0, 0, 1600, 0, 0, 15, 72, 2, 55, 0, 1, 2, 0, 3, 'ground', 'walls', 40, 183600, 0, 2500000, 0, 0, 428),
(55, 'wallbreaker', 7, 0, 0, 1800, 0, 0, 15, 82, 2, 66, 0, 1, 2, 0, 3, 'ground', 'walls', 40, 302400, 0, 4200000, 0, 0, 549),
(56, 'wallbreaker', 8, 0, 0, 2000, 0, 0, 15, 92, 2, 75, 0, 1, 2, 0, 3, 'ground', 'walls', 40, 604800, 0, 7300000, 0, 0, 777),
(57, 'wallbreaker', 9, 0, 0, 2200, 0, 0, 15, 112, 2, 86, 0, 1, 2, 0, 3, 'ground', 'walls', 40, 950400, 0, 10000000, 0, 0, 974),
(58, 'wallbreaker', 10, 0, 0, 2400, 0, 0, 15, 130, 2, 94, 0, 1, 2, 0, 3, 'ground', 'walls', 40, 1296000, 0, 15200000, 0, 0, 1138),
(59, 'wallbreaker', 11, 0, 0, 2600, 0, 0, 15, 140, 2, 102, 0, 1, 2, 0, 3, 'ground', 'walls', 40, 1382400, 0, 16500000, 0, 0, 1175),
(60, 'wizard', 1, 0, 0, 1000, 0, 0, 30, 75, 4, 75, 3, 1.5, 0.3, 5, 2, 'ground', 'all', 1, 0, 0, 0, 0, 0, 0),
(61, 'wizard', 2, 0, 0, 1400, 0, 0, 30, 90, 4, 105, 3, 1.5, 0.3, 5, 2, 'ground', 'all', 1, 28800, 0, 120000, 0, 0, 169),
(62, 'wizard', 3, 0, 0, 1800, 0, 0, 30, 108, 4, 135, 3, 1.5, 0.3, 5, 2, 'ground', 'all', 1, 43200, 0, 320000, 0, 0, 207),
(63, 'wizard', 4, 0, 0, 2200, 0, 0, 30, 135, 4, 187.5, 3, 1.5, 0.3, 5, 2, 'ground', 'all', 1, 64800, 0, 620000, 0, 0, 254),
(64, 'wizard', 5, 0, 0, 2600, 0, 0, 30, 165, 4, 255, 3, 1.5, 0.3, 5, 2, 'ground', 'all', 1, 86400, 0, 1200000, 0, 0, 293),
(65, 'wizard', 6, 0, 0, 3000, 0, 0, 30, 180, 4, 277.5, 3, 1.5, 0.3, 5, 2, 'ground', 'all', 1, 172800, 0, 2200000, 0, 0, 415),
(66, 'wizard', 7, 0, 0, 3400, 0, 0, 30, 195, 4, 300, 3, 1.5, 0.3, 5, 2, 'ground', 'all', 1, 302400, 0, 3500000, 0, 0, 549),
(67, 'wizard', 8, 0, 0, 3800, 0, 0, 30, 210, 4, 322.5, 3, 1.5, 0.3, 5, 2, 'ground', 'all', 1, 453600, 0, 5000000, 0, 0, 673),
(68, 'wizard', 9, 0, 0, 4200, 0, 0, 30, 230, 4, 345, 3, 1.5, 0.3, 5, 2, 'ground', 'all', 1, 756000, 0, 6500000, 0, 0, 869),
(69, 'wizard', 10, 0, 0, 4600, 0, 0, 30, 250, 4, 367.5, 3, 1.5, 0.3, 5, 2, 'ground', 'all', 1, 950400, 0, 10500000, 0, 0, 974),
(70, 'wizard', 11, 0, 0, 5000, 0, 0, 30, 270, 4, 390, 3, 1.5, 0.3, 5, 2, 'ground', 'all', 1, 1382400, 0, 17200000, 0, 0, 1175),
(71, 'miner', 1, 0, 0, 4200, 0, 0, 30, 550, 6, 136, 0, 1.7, 0, 0, 4, 'underground', 'all', 1, 0, 0, 0, 0, 0, 0),
(72, 'miner', 2, 0, 0, 4800, 0, 0, 30, 610, 6, 149.6, 0, 1.7, 0, 0, 4, 'underground', 'all', 1, 187200, 0, 2500000, 0, 0, 432),
(73, 'miner', 3, 0, 0, 5200, 0, 0, 30, 670, 6, 163.2, 0, 1.7, 0, 0, 4, 'underground', 'all', 1, 302400, 0, 3200000, 0, 0, 549),
(74, 'miner', 4, 0, 0, 5600, 0, 0, 30, 730, 6, 176.8, 0, 1.7, 0, 0, 4, 'underground', 'all', 1, 345600, 0, 3800000, 0, 0, 587),
(75, 'miner', 5, 0, 0, 6000, 0, 0, 30, 800, 6, 190.4, 0, 1.7, 0, 0, 4, 'underground', 'all', 1, 604800, 0, 5000000, 0, 0, 777),
(76, 'miner', 6, 0, 0, 6400, 0, 0, 30, 900, 6, 204, 0, 1.7, 0, 0, 4, 'underground', 'all', 1, 777600, 0, 6500000, 0, 0, 881),
(77, 'miner', 7, 0, 0, 6800, 0, 0, 30, 1000, 6, 217.6, 0, 1.7, 0, 0, 4, 'underground', 'all', 1, 1015200, 0, 10000000, 0, 0, 1007),
(78, 'miner', 8, 0, 0, 7200, 0, 0, 30, 1100, 6, 231.2, 0, 1.7, 0, 0, 4, 'underground', 'all', 1, 1382400, 0, 16500000, 0, 0, 1175),
(79, 'miner', 9, 0, 0, 7600, 0, 0, 30, 1200, 6, 244.8, 0, 1.7, 0, 0, 4, 'underground', 'all', 1, 1468800, 0, 18500000, 0, 0, 1211),
(80, 'balloon', 1, 0, 0, 1750, 0, 0, 30, 150, 5, 75, 0.5, 3, 1.2, 5, 1.3, 'fly', 'defenses', 1, 0, 0, 0, 0, 0, 0),
(81, 'balloon', 2, 0, 0, 2250, 0, 0, 30, 180, 5, 96, 0.5, 3, 1.2, 5, 1.3, 'fly', 'defenses', 1, 28800, 0, 125000, 0, 0, 169),
(82, 'balloon', 3, 0, 0, 2750, 0, 0, 30, 216, 5, 144, 0.5, 3, 1.2, 5, 1.3, 'fly', 'defenses', 1, 43200, 0, 400000, 0, 0, 207),
(83, 'balloon', 4, 0, 0, 3500, 0, 0, 30, 280, 5, 216, 0.5, 3, 1.2, 5, 1.3, 'fly', 'defenses', 1, 64800, 0, 800000, 0, 0, 254),
(84, 'balloon', 5, 0, 0, 4000, 0, 0, 30, 390, 5, 324, 0.5, 3, 1.2, 5, 1.3, 'fly', 'defenses', 1, 86400, 0, 1500000, 0, 0, 293),
(85, 'balloon', 6, 0, 0, 4500, 0, 0, 30, 545, 5, 486, 0.5, 3, 1.2, 5, 1.3, 'fly', 'defenses', 1, 302400, 0, 2750000, 0, 0, 549),
(86, 'balloon', 7, 0, 0, 5000, 0, 0, 30, 690, 5, 594, 0.5, 3, 1.2, 5, 1.3, 'fly', 'defenses', 1, 453600, 0, 4500000, 0, 0, 673),
(87, 'balloon', 8, 0, 0, 5500, 0, 0, 30, 840, 5, 708, 0.5, 3, 1.2, 5, 1.3, 'fly', 'defenses', 1, 820800, 0, 7700000, 0, 0, 905),
(88, 'balloon', 9, 0, 0, 6000, 0, 0, 30, 940, 5, 768, 0.5, 3, 1.2, 5, 1.3, 'fly', 'defenses', 1, 1036800, 0, 10500000, 0, 0, 1018),
(89, 'balloon', 10, 0, 0, 6500, 0, 0, 30, 1040, 5, 828, 0.5, 3, 1.2, 5, 1.3, 'fly', 'defenses', 1, 1468800, 0, 17000000, 0, 0, 1211),
(90, 'pekka', 1, 0, 0, 14000, 0, 0, 180, 3000, 25, 468, 0, 1.8, 0, 0, 2, 'ground', 'all', 1, 0, 0, 0, 0, 0, 0),
(91, 'pekka', 2, 0, 0, 16000, 0, 0, 180, 3500, 25, 522, 0, 1.8, 0, 0, 2, 'ground', 'all', 1, 43200, 0, 1200000, 0, 0, 207),
(92, 'pekka', 3, 0, 0, 18000, 0, 0, 180, 4000, 25, 576, 0, 1.8, 0, 0, 2, 'ground', 'all', 1, 86400, 0, 1800000, 0, 0, 293),
(93, 'pekka', 4, 0, 0, 20000, 0, 0, 180, 4500, 25, 648, 0, 1.8, 0, 0, 2, 'ground', 'all', 1, 172800, 0, 2800000, 0, 0, 415),
(94, 'pekka', 5, 0, 0, 22500, 0, 0, 180, 5000, 25, 738, 0, 1.8, 0, 0, 2, 'ground', 'all', 1, 302400, 0, 3200000, 0, 0, 549),
(95, 'pekka', 6, 0, 0, 25000, 0, 0, 180, 5500, 25, 846, 0, 1.8, 0, 0, 2, 'ground', 'all', 1, 410400, 0, 4200000, 0, 0, 640),
(96, 'pekka', 7, 0, 0, 27500, 0, 0, 180, 5900, 25, 972, 0, 1.8, 0, 0, 2, 'ground', 'all', 1, 518400, 0, 5200000, 0, 0, 720),
(97, 'pekka', 8, 0, 0, 30000, 0, 0, 180, 6300, 25, 1098, 0, 1.8, 0, 0, 2, 'ground', 'all', 1, 864000, 0, 7700000, 0, 0, 929),
(98, 'pekka', 9, 0, 0, 32500, 0, 0, 180, 6700, 25, 1224, 0, 1.8, 0, 0, 2, 'ground', 'all', 1, 972000, 0, 10500000, 0, 0, 985),
(99, 'dragon', 1, 0, 0, 10000, 0, 0, 180, 1900, 20, 175, 3, 1.25, 0.3, 5, 2, 'fly', 'all', 1, 0, 0, 0, 0, 0, 0),
(100, 'dragon', 2, 0, 0, 12000, 0, 0, 180, 2100, 20, 200, 3, 1.25, 0.3, 5, 2, 'fly', 'all', 1, 64800, 0, 1000000, 0, 0, 254),
(101, 'dragon', 3, 0, 0, 14000, 0, 0, 180, 2300, 20, 225, 3, 1.25, 0.3, 5, 2, 'fly', 'all', 1, 129600, 0, 2000000, 0, 0, 360),
(102, 'dragon', 4, 0, 0, 16000, 0, 0, 180, 2700, 20, 262.5, 3, 1.25, 0.3, 5, 2, 'fly', 'all', 1, 259200, 0, 3000000, 0, 0, 509),
(103, 'dragon', 5, 0, 0, 18000, 0, 0, 180, 3100, 20, 300, 3, 1.25, 0.3, 5, 2, 'fly', 'all', 1, 453600, 0, 3800000, 0, 0, 673),
(104, 'dragon', 6, 0, 0, 20000, 0, 0, 180, 3400, 20, 337.5, 3, 1.25, 0.3, 5, 2, 'fly', 'all', 1, 475200, 0, 4900000, 0, 0, 689),
(105, 'dragon', 7, 0, 0, 22000, 0, 0, 180, 3900, 20, 387.5, 3, 1.25, 0.3, 5, 2, 'fly', 'all', 1, 864000, 0, 7000000, 0, 0, 929),
(106, 'dragon', 8, 0, 0, 24000, 0, 0, 180, 4200, 20, 412.5, 3, 1.25, 0.3, 5, 2, 'fly', 'all', 1, 1036800, 0, 11000000, 0, 0, 1018),
(107, 'dragon', 9, 0, 0, 26000, 0, 0, 180, 4500, 20, 437.5, 3, 1.25, 0.3, 5, 2, 'fly', 'all', 1, 1468800, 0, 17500000, 0, 0, 1211),
(108, 'skeleton', 1, 2, 0, 0, 0, 0, 0, 30, 1, 25, 0, 1, 0, 0, 3, 'ground', 'all', 1, 0, 0, 0, 0, 0, 0),
(109, 'bat', 1, 2, 0, 0, 0, 0, 0, 10, 1, 40, 0, 2, 0, 0, 5, 'fly', 'all', 1, 0, 0, 0, 0, 0, 0),
(110, 'babydragon', 1, 0, 0, 5000, 0, 0, 90, 1200, 10, 75, 2.25, 1, 0.3, 0, 2.5, 'fly', 'all', 1, 0, 0, 0, 0, 0, 0),
(111, 'babydragon', 2, 0, 0, 6000, 0, 0, 90, 1300, 10, 85, 2.25, 1, 0.3, 0, 2.5, 'fly', 'all', 1, 172800, 0, 2000000, 0, 0, 415),
(112, 'babydragon', 3, 0, 0, 7000, 0, 0, 90, 1400, 10, 95, 2.25, 1, 0.3, 0, 2.5, 'fly', 'all', 1, 302400, 0, 2500000, 0, 0, 549),
(113, 'babydragon', 4, 0, 0, 8000, 0, 0, 90, 1500, 10, 105, 2.25, 1, 0.3, 0, 2.5, 'fly', 'all', 1, 432000, 0, 3400000, 0, 0, 657),
(114, 'babydragon', 5, 0, 0, 9000, 0, 0, 90, 1600, 10, 115, 2.25, 1, 0.3, 0, 2.5, 'fly', 'all', 1, 540000, 0, 4200000, 0, 0, 734),
(115, 'babydragon', 6, 0, 0, 10000, 0, 0, 90, 1700, 10, 125, 2.25, 1, 0.3, 0, 2.5, 'fly', 'all', 1, 712800, 0, 6300000, 0, 0, 844),
(116, 'babydragon', 7, 0, 0, 11000, 0, 0, 90, 1800, 10, 135, 2.25, 1, 0.3, 0, 2.5, 'fly', 'all', 1, 907200, 0, 9000000, 0, 0, 952),
(117, 'babydragon', 8, 0, 0, 12000, 0, 0, 90, 1900, 10, 145, 2.25, 1, 0.3, 0, 2.5, 'fly', 'all', 1, 1360800, 0, 16000000, 0, 0, 1166);

-- --------------------------------------------------------

--
-- Table structure for table `spells`
--

CREATE TABLE `spells` (
  `id` int(11) NOT NULL,
  `global_id` varchar(50) NOT NULL DEFAULT '',
  `level` int(11) NOT NULL DEFAULT 1,
  `account_id` int(11) NOT NULL,
  `brewed` int(11) NOT NULL DEFAULT 0,
  `ready` int(11) NOT NULL DEFAULT 0,
  `brewed_time` float NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- --------------------------------------------------------

--
-- Table structure for table `units`
--

CREATE TABLE `units` (
  `id` int(11) NOT NULL,
  `global_id` varchar(50) NOT NULL DEFAULT '',
  `level` int(11) NOT NULL DEFAULT 1,
  `account_id` int(11) NOT NULL,
  `trained` int(1) NOT NULL DEFAULT 0,
  `ready` int(1) NOT NULL DEFAULT 0,
  `trained_time` float NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- --------------------------------------------------------

--
-- Table structure for table `verification_codes`
--

CREATE TABLE `verification_codes` (
  `id` int(11) NOT NULL,
  `target` varchar(1000) NOT NULL,
  `device_id` varchar(1000) NOT NULL,
  `code` varchar(50) NOT NULL,
  `expire_time` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `accounts`
--
ALTER TABLE `accounts`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `battles`
--
ALTER TABLE `battles`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `buildings`
--
ALTER TABLE `buildings`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `chat_messages`
--
ALTER TABLE `chat_messages`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `chat_reports`
--
ALTER TABLE `chat_reports`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `clans`
--
ALTER TABLE `clans`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `clan_join_requests`
--
ALTER TABLE `clan_join_requests`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `clan_wars`
--
ALTER TABLE `clan_wars`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `clan_wars_search`
--
ALTER TABLE `clan_wars_search`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `clan_war_attacks`
--
ALTER TABLE `clan_war_attacks`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `iap`
--
ALTER TABLE `iap`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `research`
--
ALTER TABLE `research`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `server_buildings`
--
ALTER TABLE `server_buildings`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `server_quest_battles`
--
ALTER TABLE `server_quest_battles`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `server_spells`
--
ALTER TABLE `server_spells`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `server_units`
--
ALTER TABLE `server_units`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `spells`
--
ALTER TABLE `spells`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `units`
--
ALTER TABLE `units`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `verification_codes`
--
ALTER TABLE `verification_codes`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `accounts`
--
ALTER TABLE `accounts`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=857;

--
-- AUTO_INCREMENT for table `battles`
--
ALTER TABLE `battles`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5454;

--
-- AUTO_INCREMENT for table `buildings`
--
ALTER TABLE `buildings`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=108548;

--
-- AUTO_INCREMENT for table `chat_messages`
--
ALTER TABLE `chat_messages`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=74;

--
-- AUTO_INCREMENT for table `chat_reports`
--
ALTER TABLE `chat_reports`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=21;

--
-- AUTO_INCREMENT for table `clans`
--
ALTER TABLE `clans`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT for table `clan_join_requests`
--
ALTER TABLE `clan_join_requests`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `clan_wars`
--
ALTER TABLE `clan_wars`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT for table `clan_wars_search`
--
ALTER TABLE `clan_wars_search`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=21;

--
-- AUTO_INCREMENT for table `clan_war_attacks`
--
ALTER TABLE `clan_war_attacks`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT for table `iap`
--
ALTER TABLE `iap`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT for table `research`
--
ALTER TABLE `research`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=38;

--
-- AUTO_INCREMENT for table `server_buildings`
--
ALTER TABLE `server_buildings`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=309;

--
-- AUTO_INCREMENT for table `server_quest_battles`
--
ALTER TABLE `server_quest_battles`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `server_spells`
--
ALTER TABLE `server_spells`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=69;

--
-- AUTO_INCREMENT for table `server_units`
--
ALTER TABLE `server_units`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=118;

--
-- AUTO_INCREMENT for table `spells`
--
ALTER TABLE `spells`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=72;

--
-- AUTO_INCREMENT for table `units`
--
ALTER TABLE `units`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=101725;

--
-- AUTO_INCREMENT for table `verification_codes`
--
ALTER TABLE `verification_codes`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=24;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
