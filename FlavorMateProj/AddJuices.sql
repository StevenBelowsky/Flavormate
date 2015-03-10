DELIMITER $$

DROP PROCEDURE IF EXISTS `updateJuice` $$
CREATE  PROCEDURE `updateJuice`(_clientID int , _id int, _name varchar(1000), _description varchar(9000),
								_makerID int, _ingredients varchar(2000), _isDeleting bit)
BEGIN

	if _isDeleting then
		
		update juice
		set isDeleted=1
		where clientID=_clientID
		and id=_id;

		insert into juice_audit(changeLog, juiceID, name, description, makerID, ingredients, clientID)
		values('Deleted Juice.',_id,_name, _description, _makerID, _ingredients, _clientID);
			 
	else
		if _id <= 0 then
		
			insert into juice(name, description, makerID, ingredients, clientID)
			values(_name, _description, _makerID, _ingredients, _clientID);
			
			insert into juice_audit(changeLog, juiceID, name, description, makerID, ingredients, clientID)
			values('Inserted new juice.',LAST_INSERT_ID(),_name, _description, _makerID, _ingredients, _clientID);
			 
		else
		
			update juice
			set name = _name,
				description=_description,
				makerID = _makerID,
				ingredients = _ingredients
			where clientID = _clientID
			and   id=_id;
		
			insert into juice_audit(changeLog, juiceID, name, description, makerID, ingredients, clientID)
			values('Updated juice.',_name, _id, _description, _makerID, _ingredients, _clientID);
		
		end if;
	end if;
	

END $$

DELIMITER ;