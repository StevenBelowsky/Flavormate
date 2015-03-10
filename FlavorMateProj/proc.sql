DELIMITER $$

DROP PROCEDURE IF EXISTS `getJuices` $$
CREATE  PROCEDURE `getJuices`(_clientID int)
BEGIN

	select t.Name, t.Maker, t.Ingredients, t.Description, t.JuiceID, t.makerID
	from(
		select  juice.name as Name, 
				maker.name as Maker,  
				maker.id as MakerID,
				concat(group_concat(ingredient.name),'-',group_concat(ingredient.id)) as Ingredients, 
				juice.description as Description,
				juice.id as JuiceID
		from  juice
		inner join maker
			on maker.id = juice.makerid
		inner join ingredient
			on find_in_set(ingredient.id, juice.ingredients)
		where juice.clientid=_clientID
		and   juice.isDeleted = 0
		group by juice.id 
		order by maker.id
	) t;



END $$

DELIMITER ;