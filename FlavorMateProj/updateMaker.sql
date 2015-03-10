DELIMITER $$

DROP PROCEDURE IF EXISTS `updateMaker` $$
CREATE  PROCEDURE `updateMaker`(_clientID int , _id int, _name varchar(1000), _isDeleting bit)
BEGIN

	if _isDeleting then
		
		update maker
		set isDeleted=1
		where clientID=_clientID
		and id=_id;

	else
		if _id <= 0 then
		
			insert into maker(name, clientID)
			values(_name, _clientID);
			
		else
		
			update maker
			set name = _name
			where clientID = _clientID
			and   id=_id;
		
		end if;
	end if;
	

END $$

DELIMITER ;