ALTER TABLE `characters` ADD `headTop` INT NOT NULL DEFAULT '0' AFTER `hair`;
ALTER TABLE `characters` ADD `headMiddle` INT NOT NULL DEFAULT '0' AFTER `headTop`;
ALTER TABLE `characters` ADD `headBottom` INT NOT NULL DEFAULT '0' AFTER `headMiddle`;
