CREATE DATABASE IF NOT EXISTS `sabine` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
USE `sabine`;

CREATE TABLE `accounts` (
  `accountId` bigint(20) NOT NULL,
  `username` varchar(64) NOT NULL,
  `password` varchar(64) NOT NULL,
  `sex` tinyint(4) NOT NULL,
  `authority` tinyint(4) NOT NULL,
  `sessionId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE `characters` (
  `characterId` bigint(20) NOT NULL,
  `accountId` bigint(20) NOT NULL,
  `slot` int(11) NOT NULL,
  `name` varchar(64) NOT NULL,
  `mapId` int(11) NOT NULL,
  `x` int(11) NOT NULL,
  `y` int(11) NOT NULL,
  `job` int(11) NOT NULL DEFAULT 0,
  `baseLevel` int(11) NOT NULL,
  `jobLevel` int(11) NOT NULL,
  `baseExp` int(11) NOT NULL DEFAULT 0,
  `jobExp` int(11) NOT NULL DEFAULT 0,
  `statPoints` int(11) NOT NULL DEFAULT 0,
  `skillPoints` int(11) NOT NULL DEFAULT 0,
  `str` int(11) NOT NULL,
  `agi` int(11) NOT NULL,
  `vit` int(11) NOT NULL,
  `int` int(11) NOT NULL,
  `dex` int(11) NOT NULL,
  `luk` int(11) NOT NULL,
  `hp` int(11) NOT NULL,
  `hpMax` int(11) NOT NULL,
  `sp` int(11) NOT NULL,
  `spMax` int(11) NOT NULL,
  `zeny` int(11) NOT NULL,
  `hair` int(11) NOT NULL DEFAULT 0,
  `weapon` int(11) NOT NULL DEFAULT 0,
  `speed` int(11) NOT NULL DEFAULT 200,
  `weight` int(11) NOT NULL DEFAULT 10,
  `weightMax` int(11) NOT NULL DEFAULT 20000
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE `items` (
  `itemId` int(11) NOT NULL,
  `characterId` bigint(20) NOT NULL,
  `classId` int(11) NOT NULL,
  `amount` int(11) NOT NULL,
  `equipped` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE `vars_account` (
  `varId` bigint(20) NOT NULL,
  `ownerId` bigint(20) NOT NULL,
  `name` varchar(128) NOT NULL,
  `type` char(2) NOT NULL,
  `value` mediumtext NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ROW_FORMAT=COMPACT;

CREATE TABLE `vars_character` (
  `varId` bigint(20) NOT NULL,
  `ownerId` bigint(20) NOT NULL,
  `name` varchar(128) NOT NULL,
  `type` char(2) NOT NULL,
  `value` mediumtext NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ROW_FORMAT=COMPACT;


ALTER TABLE `accounts`
  ADD PRIMARY KEY (`accountId`);

ALTER TABLE `characters`
  ADD PRIMARY KEY (`characterId`),
  ADD KEY `accountId` (`accountId`);

ALTER TABLE `items`
  ADD PRIMARY KEY (`itemId`),
  ADD KEY `characterId` (`characterId`);

ALTER TABLE `vars_account`
  ADD PRIMARY KEY (`varId`),
  ADD KEY `accountId` (`ownerId`);

ALTER TABLE `vars_character`
  ADD PRIMARY KEY (`varId`),
  ADD KEY `accountId` (`ownerId`);


ALTER TABLE `accounts`
  MODIFY `accountId` bigint(20) NOT NULL AUTO_INCREMENT;

ALTER TABLE `characters`
  MODIFY `characterId` bigint(20) NOT NULL AUTO_INCREMENT;

ALTER TABLE `items`
  MODIFY `itemId` int(11) NOT NULL AUTO_INCREMENT;

ALTER TABLE `vars_account`
  MODIFY `varId` bigint(20) NOT NULL AUTO_INCREMENT;

ALTER TABLE `vars_character`
  MODIFY `varId` bigint(20) NOT NULL AUTO_INCREMENT;


ALTER TABLE `characters`
  ADD CONSTRAINT `characters_ibfk_1` FOREIGN KEY (`accountId`) REFERENCES `accounts` (`accountId`) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE `items`
  ADD CONSTRAINT `items_ibfk_1` FOREIGN KEY (`characterId`) REFERENCES `characters` (`characterId`) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE `vars_account`
  ADD CONSTRAINT `vars_account_ibfk_1` FOREIGN KEY (`ownerId`) REFERENCES `accounts` (`accountId`) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE `vars_character`
  ADD CONSTRAINT `vars_character_ibfk_1` FOREIGN KEY (`ownerId`) REFERENCES `characters` (`characterId`) ON DELETE CASCADE ON UPDATE CASCADE;
