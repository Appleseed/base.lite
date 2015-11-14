CREATE TABLE `basecollectionitem` (
  `TableId` int(11) NOT NULL AUTO_INCREMENT,
  `Id` char(64) DEFAULT NULL,
  `Data` blob,
  `ItemProcessed` tinyint(1) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  PRIMARY KEY (`TableId`),
  UNIQUE KEY `TableId_UNIQUE` (`TableId`),
  UNIQUE KEY `Id_UNIQUE` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=91 DEFAULT CHARSET=utf8;
