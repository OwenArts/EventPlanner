-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Gegenereerd op: 10 jun 2024 om 17:14
-- Serverversie: 10.4.32-MariaDB
-- PHP-versie: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `eventplanner`
--

-- --------------------------------------------------------

--
-- Tabelstructuur voor tabel `festival`
--

CREATE TABLE `festival` (
  `endMoment` datetime DEFAULT NULL,
  `id` char(36) NOT NULL,
  `name` varchar(255) DEFAULT NULL,
  `startMoment` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Gegevens worden geëxporteerd voor tabel `festival`
--

INSERT INTO `festival` (`endMoment`, `id`, `name`, `startMoment`) VALUES
('2024-06-09 18:00:00', '5a5734aa-f9e8-4266-b7d8-5224f28209a1', 'Talent Show 2024', '2024-06-09 11:00:00');

-- --------------------------------------------------------

--
-- Tabelstructuur voor tabel `participant`
--

CREATE TABLE `participant` (
  `birthDay` datetime DEFAULT NULL,
  `email` varchar(255) DEFAULT NULL,
  `firstName` varchar(255) DEFAULT NULL,
  `id` char(36) NOT NULL,
  `lastName` varchar(255) DEFAULT NULL,
  `middleName` varchar(255) DEFAULT NULL,
  `phoneNumber` varchar(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Gegevens worden geëxporteerd voor tabel `participant`
--

INSERT INTO `participant` (`birthDay`, `email`, `firstName`, `id`, `lastName`, `middleName`, `phoneNumber`) VALUES
('1996-08-21 00:00:00', 'samantha.martin@example.com', 'Samantha', '08d6a2f1-c8f5-4d6f-a7d9-097010fc2331', 'Martin', '', '0623456789'),
('1999-06-30 00:00:00', 'sarah.jones@example.com', 'Sarah', '0e45af7e-8233-4ad9-b5f3-564239d893b1', 'Jones', '', '0667890123'),
('1991-02-18 00:00:00', 'emma.wilson@example.com', 'Emma', '15508b1f-33db-4121-9fb1-bca7d4200c07', 'Wilson', '', '0656789012'),
('1989-09-27 00:00:00', 'andrew.lee@example.com', 'Andrew', '18779c2b-5990-42c3-a968-3ed2bd822415', 'Lee', '', '0634567890'),
('1987-12-01 00:00:00', 'olivia.lopez@example.com', 'Olivia', '1b20a313-f591-4873-af9b-cad8b9879b88', 'Lopez', '', '0634567890'),
('1990-02-28 00:00:00', 'natalie.lewis@example.com', 'Natalie', '268feb07-3424-4bdd-8426-86df898a5cdb', 'Lewis', '', '0689012345'),
('1988-07-19 00:00:00', 'david.schweitz@example.com', 'David', '29ecd86b-d1a8-4c1a-bfd5-e0806724c02f', 'Schweitz', '', '0678901234'),
('2000-10-20 00:00:00', 'megan.martinez@example.com', 'Megan', '355f59d8-5cdc-4fab-9689-d0e7ab871031', 'Martinez', '', '0612345678'),
('1992-08-25 00:00:00', 'laura.davis@example.com', 'Laura', '40ca60ec-43d3-44b3-b850-cfd7e26fbead', 'Davis', '', '0689012345'),
('2001-12-29 00:00:00', 'alyssa.harris@example.com', 'Alyssa', '43ef1cdc-ec25-4401-ad84-983f2e4966a0', 'Harris', '', '0667890123'),
('1982-05-05 00:00:00', 'michael.williams@example.com', 'Michael', '5859043c-0cf3-411c-8755-256a6f839b86', 'Williams', '', '0656789012'),
('1985-02-14 00:00:00', 'jane.smith@example.com', 'Jane', '5a73af3d-e400-466e-b651-0ade80131bc5', 'Smith', '', '0623456789'),
('1995-04-26 00:00:00', 'mia.young@example.com', 'Mia', '5ca1b791-01db-4098-8ffc-6b826cb90508', 'Young', '', '0612345678'),
('2005-09-17 00:00:00', 'anne.kieneker@example.com', 'Anne', '649c6b56-2d7e-4b8e-9fa3-c82cb9b7dc1c', 'Kieneker', '', '0622886576'),
('1994-11-11 00:00:00', 'charles.hernandez@example.com', 'Charles', '77d224b6-fe14-4829-b877-6bf474c63993', 'Hernandez', '', '0623456789'),
('1986-10-03 00:00:00', 'ashley.perez@example.com', 'Ashley', '7d46284a-a02c-4eba-90d4-c2ef6e38ae50', 'Perez', '', '0645678901'),
('1981-05-23 00:00:00', 'christopher.taylor@example.com', 'Christopher', '891bdf7b-08c9-4997-891a-e55869b872d8', 'Taylor', '', '0689012345'),
('1990-01-01 00:00:00', 'john.doe@example.com', 'John', '98743fd7-19ea-4613-bae1-601813915c68', 'Doe', '', '0612345678'),
('1993-06-06 00:00:00', 'hannah.moore@example.com', 'Hannah', 'a1ce5a36-51fd-4370-9e9a-dd940d0cd1ff', 'Moore', '', '0690123456'),
('1995-04-10 00:00:00', 'emily.brown@example.com', 'Emily', 'a3bb72b1-2685-4f80-8d33-b868d3c367b8', 'Brown', '', '0645678901'),
('1979-11-16 00:00:00', 'joseph.white@example.com', 'Joseph', 'a51a6c8b-d2d8-401c-b3a2-4758dfdf4e5e', 'White', '', '0656789012'),
('1984-03-05 00:00:00', 'daniel.anderson@example.com', 'Daniel', 'ba79de76-cfb5-4b11-8639-65952395b454', 'Anderson', '', '0667890123'),
('1980-07-31 00:00:00', 'matthew.jackson@example.com', 'Matthew', 'cddc13ee-afb3-4f66-84ff-5933c5532057', 'Jackson', '', '0612345678'),
('1978-03-22 00:00:00', 'robert.johnson@example.com', 'Robert', 'cfcd2c1f-7af0-41d7-83cf-6f0d74a48fa4', 'Johnson', '', '0634567890'),
('1983-09-09 00:00:00', 'james.garcia@example.com', 'James', 'd719e7a4-af42-4adb-a2c3-ef9c6c781804', 'Garcia', '', '0690123456'),
('1997-01-04 00:00:00', 'ethan.clark@example.com', 'Ethan', 'd99f718b-7ca8-409d-bf8f-8fa3cab220fe', 'Clark', '', '0678901234'),
('1987-05-09 00:00:00', 'alexander.king@example.com', 'Alexander', 'eaf87412-2a43-48bc-9173-b38b19d6e47e', 'King', '', '0623456789'),
('1982-03-12 00:00:00', 'william.walker@example.com', 'William', 'f7fceb3e-69f9-4130-9a15-6eaa47549b9b', 'Walker', '', '0690123456'),
('1998-04-14 00:00:00', 'sophia.thomas@example.com', 'Sophia', 'fa91afaf-d85b-4c8b-b931-a13b3c2763de', 'Thomas', '', '0678901234');

-- --------------------------------------------------------

--
-- Tabelstructuur voor tabel `room`
--

CREATE TABLE `room` (
  `festivalId` char(36) DEFAULT NULL,
  `id` char(36) NOT NULL,
  `name` varchar(255) DEFAULT NULL,
  `timeClose` datetime DEFAULT NULL,
  `timeOpen` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Gegevens worden geëxporteerd voor tabel `room`
--

INSERT INTO `room` (`festivalId`, `id`, `name`, `timeClose`, `timeOpen`) VALUES
('5a5734aa-f9e8-4266-b7d8-5224f28209a1', '014c3599-3e35-4ea5-9014-0a918b1abb9b', 'Expo', '2024-06-09 17:30:00', '2024-06-09 13:00:00'),
('5a5734aa-f9e8-4266-b7d8-5224f28209a1', '2c28baee-d3a0-4b61-81d2-1c2651c72bfd', 'Studio', '2024-06-09 17:00:00', '2024-06-09 13:00:00');

-- --------------------------------------------------------

--
-- Tabelstructuur voor tabel `segment`
--

CREATE TABLE `segment` (
  `duration` int(11) DEFAULT NULL,
  `firstPlace` char(36) DEFAULT NULL,
  `id` char(36) NOT NULL,
  `name` varchar(255) DEFAULT NULL,
  `roomId` char(36) DEFAULT NULL,
  `secondPlace` char(36) DEFAULT NULL,
  `thirdPlace` char(36) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Gegevens worden geëxporteerd voor tabel `segment`
--

INSERT INTO `segment` (`duration`, `firstPlace`, `id`, `name`, `roomId`, `secondPlace`, `thirdPlace`) VALUES
(5, NULL, '368fbc95-b9a0-4fd6-baa9-dd5c1a6c78e1', 'Dance', '014c3599-3e35-4ea5-9014-0a918b1abb9b', NULL, NULL),
(15, NULL, '4b18e579-2736-43f4-aec5-a0c435128f77', 'Singing', '2c28baee-d3a0-4b61-81d2-1c2651c72bfd', NULL, NULL),
(10, NULL, '6c8407a5-251c-4222-bc20-323f5f021946', 'Art', '014c3599-3e35-4ea5-9014-0a918b1abb9b', NULL, NULL),
(15, NULL, 'bc352ef0-691b-485f-ac54-f62cf21855d7', 'Bands', '2c28baee-d3a0-4b61-81d2-1c2651c72bfd', NULL, NULL),
(15, NULL, 'f48c6a00-13ce-4c21-a888-965fda0f11e4', 'DJ', '2c28baee-d3a0-4b61-81d2-1c2651c72bfd', NULL, NULL);

-- --------------------------------------------------------

--
-- Tabelstructuur voor tabel `segment_participant`
--

CREATE TABLE `segment_participant` (
  `participantId` char(36) NOT NULL,
  `segmentId` char(36) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Gegevens worden geëxporteerd voor tabel `segment_participant`
--

INSERT INTO `segment_participant` (`participantId`, `segmentId`) VALUES
('08d6a2f1-c8f5-4d6f-a7d9-097010fc2331', '4b18e579-2736-43f4-aec5-a0c435128f77'),
('0e45af7e-8233-4ad9-b5f3-564239d893b1', '4b18e579-2736-43f4-aec5-a0c435128f77'),
('15508b1f-33db-4121-9fb1-bca7d4200c07', '6c8407a5-251c-4222-bc20-323f5f021946'),
('1b20a313-f591-4873-af9b-cad8b9879b88', '368fbc95-b9a0-4fd6-baa9-dd5c1a6c78e1'),
('268feb07-3424-4bdd-8426-86df898a5cdb', 'f48c6a00-13ce-4c21-a888-965fda0f11e4'),
('29ecd86b-d1a8-4c1a-bfd5-e0806724c02f', '368fbc95-b9a0-4fd6-baa9-dd5c1a6c78e1'),
('29ecd86b-d1a8-4c1a-bfd5-e0806724c02f', 'bc352ef0-691b-485f-ac54-f62cf21855d7'),
('5859043c-0cf3-411c-8755-256a6f839b86', 'bc352ef0-691b-485f-ac54-f62cf21855d7'),
('5a73af3d-e400-466e-b651-0ade80131bc5', '368fbc95-b9a0-4fd6-baa9-dd5c1a6c78e1'),
('5a73af3d-e400-466e-b651-0ade80131bc5', '6c8407a5-251c-4222-bc20-323f5f021946'),
('5ca1b791-01db-4098-8ffc-6b826cb90508', 'bc352ef0-691b-485f-ac54-f62cf21855d7'),
('649c6b56-2d7e-4b8e-9fa3-c82cb9b7dc1c', '368fbc95-b9a0-4fd6-baa9-dd5c1a6c78e1'),
('77d224b6-fe14-4829-b877-6bf474c63993', '6c8407a5-251c-4222-bc20-323f5f021946'),
('7d46284a-a02c-4eba-90d4-c2ef6e38ae50', 'f48c6a00-13ce-4c21-a888-965fda0f11e4'),
('891bdf7b-08c9-4997-891a-e55869b872d8', 'f48c6a00-13ce-4c21-a888-965fda0f11e4'),
('a51a6c8b-d2d8-401c-b3a2-4758dfdf4e5e', '368fbc95-b9a0-4fd6-baa9-dd5c1a6c78e1'),
('ba79de76-cfb5-4b11-8639-65952395b454', 'f48c6a00-13ce-4c21-a888-965fda0f11e4'),
('cddc13ee-afb3-4f66-84ff-5933c5532057', '368fbc95-b9a0-4fd6-baa9-dd5c1a6c78e1'),
('cfcd2c1f-7af0-41d7-83cf-6f0d74a48fa4', '4b18e579-2736-43f4-aec5-a0c435128f77'),
('d719e7a4-af42-4adb-a2c3-ef9c6c781804', '6c8407a5-251c-4222-bc20-323f5f021946'),
('f7fceb3e-69f9-4130-9a15-6eaa47549b9b', '4b18e579-2736-43f4-aec5-a0c435128f77'),
('fa91afaf-d85b-4c8b-b931-a13b3c2763de', '4b18e579-2736-43f4-aec5-a0c435128f77');

--
-- Indexen voor geëxporteerde tabellen
--

--
-- Indexen voor tabel `festival`
--
ALTER TABLE `festival`
  ADD PRIMARY KEY (`id`);

--
-- Indexen voor tabel `participant`
--
ALTER TABLE `participant`
  ADD PRIMARY KEY (`id`);

--
-- Indexen voor tabel `room`
--
ALTER TABLE `room`
  ADD PRIMARY KEY (`id`),
  ADD KEY `festivalId` (`festivalId`);

--
-- Indexen voor tabel `segment`
--
ALTER TABLE `segment`
  ADD PRIMARY KEY (`id`),
  ADD KEY `roomId` (`roomId`),
  ADD KEY `firstPlace` (`firstPlace`),
  ADD KEY `secondPlace` (`secondPlace`),
  ADD KEY `thirdPlace` (`thirdPlace`);

--
-- Indexen voor tabel `segment_participant`
--
ALTER TABLE `segment_participant`
  ADD PRIMARY KEY (`participantId`,`segmentId`),
  ADD KEY `segmentId` (`segmentId`);

--
-- Beperkingen voor geëxporteerde tabellen
--

--
-- Beperkingen voor tabel `room`
--
ALTER TABLE `room`
  ADD CONSTRAINT `room_ibfk_1` FOREIGN KEY (`festivalId`) REFERENCES `festival` (`id`);

--
-- Beperkingen voor tabel `segment`
--
ALTER TABLE `segment`
  ADD CONSTRAINT `segment_ibfk_1` FOREIGN KEY (`roomId`) REFERENCES `room` (`id`),
  ADD CONSTRAINT `segment_ibfk_2` FOREIGN KEY (`firstPlace`) REFERENCES `participant` (`id`),
  ADD CONSTRAINT `segment_ibfk_3` FOREIGN KEY (`secondPlace`) REFERENCES `participant` (`id`),
  ADD CONSTRAINT `segment_ibfk_4` FOREIGN KEY (`thirdPlace`) REFERENCES `participant` (`id`);

--
-- Beperkingen voor tabel `segment_participant`
--
ALTER TABLE `segment_participant`
  ADD CONSTRAINT `segment_participant_ibfk_1` FOREIGN KEY (`participantId`) REFERENCES `participant` (`id`),
  ADD CONSTRAINT `segment_participant_ibfk_2` FOREIGN KEY (`segmentId`) REFERENCES `segment` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
