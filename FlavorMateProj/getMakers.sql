DELIMITER $$

DROP PROCEDURE IF EXISTS `getMakers` $$
CREATE  PROCEDURE `getMakers`(_clientID int)
BEGIN

	select t.Name, t.MakerID
	from(
		select  maker.name as Name,  
				maker.id as MakerID
		from  maker
		where maker.clientid=_clientID
		and   maker.isDeleted = 0
		group by maker.id 
		order by maker.Name
	) t;



END $$

DELIMITER ;