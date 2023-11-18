/*
-- Table structure for table `players`
*/

CREATE TABLE IF NOT EXISTS `Players` (
  `PlayerID` int(11) NOT NULL AUTO_INCREMENT,
  `Username` longtext,
  `Password` longtext,
  `Nickname` longtext,
  `SettingsINI` longtext,
  PRIMARY KEY (`PlayerID`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=10000000 ;

/*
-- Table structure for table `characters`
*/


/* TODO: Check the auto increment for characters. */

CREATE TABLE IF NOT EXISTS `Characters` (
  `CharacterID` int(11) NOT NULL AUTO_INCREMENT,
  `Name` longtext,
  `LooksBinary` longblob,
  `JobsBinary` longblob,
  `Player_PlayerID` int(11) DEFAULT NULL,
  PRIMARY KEY (`CharacterID`),
  KEY `IX_Player_PlayerID` (`Player_PlayerID`) USING HASH,
  FOREIGN KEY (`Player_PlayerID`) REFERENCES `Players` (`PlayerID`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;


/*
-- Table structure for table `ServerInfoes`
*/

CREATE TABLE IF NOT EXISTS `ServerInfoes` (
  `info` varchar(255) CHARACTER SET utf8 NOT NULL,
  `setting` longtext,
  PRIMARY KEY (`info`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


/*
-- Table structure for table `Teleports`
*/
CREATE TABLE IF NOT EXISTS `Teleports` (
  `ZoneName` varchar(225) NOT NULL,
  `ObjectID` int(11) NOT NULL,
  `RotX` float NOT NULL,
  `RotY` float NOT NULL,
  `RotZ` float NOT NULL,
  `RotW` float NOT NULL,
  `PosX` float NOT NULL,
  `PosY` float NOT NULL,
  `PosZ` float NOT NULL,
  PRIMARY KEY (`ZoneName`,`ObjectID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;


/*
-- Table structure for table `Teleports`
*/
CREATE TABLE IF NOT EXISTS `NPCs` (
  `EntityID` int(11) NOT NULL,
  `ZoneName` varchar(225) NOT NULL,
  `NPCName`  varchar(255) NOT NULL,
  `RotX` float NOT NULL,
  `RotY` float NOT NULL,
  `RotZ` float NOT NULL,
  `RotW` float NOT NULL,
  `PosX` float NOT NULL,
  `PosY` float NOT NULL,
  `PosZ` float NOT NULL,
  PRIMARY KEY (`EntityID`,`ZoneName`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;
   
/*
-- Table structure for table `GameObjects`
*/
CREATE TABLE IF NOT EXISTS `GameObjects` (
  `ObjectID` int(11) NOT NULL,
  `ZoneName` varchar(225) NOT NULL,
  `ObjectName` varchar(225) NOT NULL,
  `ObjectFlags` BLOB NOT NULL,
  `RotX` float NOT NULL,
  `RotY` float NOT NULL,
  `RotZ` float NOT NULL,
  `RotW` float NOT NULL,
  `PosX` float NOT NULL,
  `PosY` float NOT NULL,
  `PosZ` float NOT NULL,
  PRIMARY KEY (`ObjectID`, `ZoneName`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;