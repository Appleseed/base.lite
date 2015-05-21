
CREATE TABLE `collectionitemqueue` (
  `ItemID` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `ItemTitle` varchar(128) DEFAULT NULL,
  `ItemUrl` varchar(256) DEFAULT NULL,
  `ItemDescription` varchar(256) DEFAULT NULL,
  `ItemTags` varchar(256) DEFAULT NULL,
  `ItemProcessed` bit(1) DEFAULT NULL,
  PRIMARY KEY (`ItemID`),
  UNIQUE KEY `ItemID_UNIQUE` (`ItemID`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;

CREATE TABLE `basecollectionitem` (
  `ItemID` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `ItemPath` varchar(256) DEFAULT NULL,
  `ItemUrl` varchar(256) DEFAULT NULL,
  `ItemName` varchar(256) DEFAULT NULL,
  `ItemTitle` varchar(128) DEFAULT NULL,
  `ItemDescription` varchar(256) DEFAULT NULL,
  `ItemSummary` varchar(256) DEFAULT NULL,
  `ItemContent_Raw` varchar(256) DEFAULT NULL,  
  `ItemContent_Rich` varchar(256) DEFAULT NULL,
  `ItemContent_Text` varchar(256) DEFAULT NULL,
  `ItemContent_Image` varchar(256) DEFAULT NULL,
  `ItemContent_Image_Url` varchar(256) DEFAULT NULL,
  `ItemTags` varchar(256) DEFAULT NULL,
  `ItemKeywords` varchar(256) DEFAULT NULL,
  `ItemCategories` varchar(256) DEFAULT NULL,
  `ItemCreatedDate` datetime DEFAULT NULL,
  `ItemProcessedDate` datetime DEFAULT NULL,
  `ItemProcessed` bit(1) DEFAULT NULL,
  `ItemQueue` varchar(256) DEFAULT NULL,
  PRIMARY KEY (`ItemID`),
  UNIQUE KEY `ItemID_UNIQUE` (`ItemID`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=latin1;
