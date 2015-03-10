DELIMITER $$

DROP PROCEDURE IF EXISTS `managerLogin` $$
CREATE  PROCEDURE `managerLogin`(_clientID int, _password varchar(255))
BEGIN

	declare _retVal varchar(255) default 'SQL Error.';
	
	if exists ( select *
				from client 
				where clientID=_clientID
				and   managerPassword=_password
				limit 1;
				)  then

		set _retVal := 'Success';
	else
		set _retVal := 'Password or ClientID incorrect';
	end if;

	select _retVal as 'returnValue';

END $$

DELIMITER ;