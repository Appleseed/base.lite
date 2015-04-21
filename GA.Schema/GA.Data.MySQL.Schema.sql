
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

