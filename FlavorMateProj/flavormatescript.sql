CREATE TABLE if not exists `client` (
	id INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
	name VARCHAR(255) NOT NULL DEFAULT '',
	password varchar(255) not null default '',
	logoUrl varchar(2000) not null default '',
	lastUpdate datetime,
	`timestamp` timestamp NULL DEFAULT CURRENT_TIMESTAMP 
			ON UPDATE CURRENT_TIMESTAMP,
	
	PRIMARY KEY (`id`)
	)
ENGINE = InnoDB;

CREATE TABLE if not exists `juice` (
	id INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
	name VARCHAR(255) NOT NULL DEFAULT '',
	description varchar(5000) not null default '(Enter Description)',
	imageURLbig varchar(1000) not null default '',
	imageURLsmall varchar(1000) not null default '',
	makerID int not null default 0,
	ingredients varchar(1000) not null default '',
	clientID INTEGER UNSIGNED NOT NULL DEFAULT 0,
	isDeleted bit not null default 0,
	createdAt datetime,
	`timestamp` timestamp NULL DEFAULT CURRENT_TIMESTAMP 
			ON UPDATE CURRENT_TIMESTAMP,
	PRIMARY KEY (`id`),
	INDEX `client_index`(`clientID`)
)
ENGINE = InnoDB;

drop table juice_group;
CREATE TABLE if not exists `ingredients_group` (
	id INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
	name VARCHAR(255) NOT NULL DEFAULT '',
	description varchar(5000) not null default '(Enter Description)',
	isDeleted bit not null default 0,
	createdAt datetime,
	`timestamp` timestamp NULL DEFAULT CURRENT_TIMESTAMP 
			ON UPDATE CURRENT_TIMESTAMP,
	PRIMARY KEY (`id`)
)
ENGINE = InnoDB;


drop table if exists `juice_audit`;
CREATE TABLE if not exists `juice_audit` (
	id INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
	juiceID integer unsigned not null default 0,
	changeLog varchar(1000) not null default '',
	description varchar(5000) not null default '',
	name VARCHAR(255) NOT NULL DEFAULT '',
	imageURLbig varchar(1000) not null default '',
	imageURLsmall varchar(1000) not null default '',
	makerID int not null default 0,
	ingredients varchar(1000) not null default '',
	clientID INTEGER UNSIGNED NOT NULL DEFAULT 0,
	createdAt datetime,
	`timestamp` timestamp NULL DEFAULT CURRENT_TIMESTAMP 
			ON UPDATE CURRENT_TIMESTAMP,
	PRIMARY KEY (`id`),
	INDEX `client_index`(`clientID`)
)
ENGINE = InnoDB;
CREATE TABLE if not exists `log` (
	id INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
	message varchar(9000) not null default '',
	clientID INTEGER UNSIGNED NOT NULL DEFAULT 0,
	createdAt datetime,
	`timestamp` timestamp NULL DEFAULT CURRENT_TIMESTAMP 
			ON UPDATE CURRENT_TIMESTAMP,
	PRIMARY KEY (`id`),
	INDEX `client_index`(`clientID`)
)
ENGINE = InnoDB;

CREATE TABLE if not exists `maker` (
	id INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
	name VARCHAR(255) NOT NULL DEFAULT '',
	clientID INTEGER UNSIGNED NOT NULL DEFAULT 0,
isDeleted bit not null default 0,
	websiteURL varchar(1000),
	`timestamp` timestamp NULL DEFAULT CURRENT_TIMESTAMP 
			ON UPDATE CURRENT_TIMESTAMP,
	PRIMARY KEY (`id`),
	INDEX `client_index`(`clientID`)
)
ENGINE = InnoDB;

drop table ingredient;
CREATE TABLE if not exists `ingredient` (
	id INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
	name VARCHAR(255) NOT NULL DEFAULT '',
	description varchar(2000) not null default '',
    groupID integer unsigned not null default 0,
	`timestamp` timestamp NULL DEFAULT CURRENT_TIMESTAMP 
			ON UPDATE CURRENT_TIMESTAMP,
	PRIMARY KEY (`id`)
)
ENGINE = InnoDB;

select * from client;
select * from juice;

insert into juice(id, name, description, imageURLbig, imageURLsmall, makerID, ingredients, clientID, createdAt)
values(1, 'Vanilla Chocolate', 'Has both vanilla goodness, and chocolate goodness', 'www.google.com', 'www.google.com', 1, '1,2', 2, now());
insert into juice(id, name, description, imageURLbig, imageURLsmall, makerID, ingredients, clientID, createdAt)
values(2, 'Sprango', 'Has both mango goodness, and sprite goodness... wtf.', 'www.google.com', 'www.google.com', 2, '3,4', 2, now());
insert into juice(id, name, description, imageURLbig, imageURLsmall, makerID, ingredients, clientID, createdAt)
values(3, 'Depressed Panda Choco Mocha', 'Jason will love this flavor regardless.', 'www.google.com', 'www.google.com', 1, '1,2,4,5,6', 2, now());
insert into juice(id, name, description, imageURLbig, imageURLsmall, makerID, ingredients, clientID, createdAt)
values(4, 'Adida', 'I dont fucking know.. what do you want from me', 'www.google.com', 'www.google.com', 2, '7', 2, now());
insert into juice(id, name, description, imageURLbig, imageURLsmall, makerID, ingredients, clientID, createdAt)
values(5, 'Depressed Panda Choco Mocha (For other guyz)', 'Jason will love this flavor regardless.', 'www.google.com', 'www.google.com', 1, '1,2,4,5,6', 1, now());
insert into juice(id, name, description, imageURLbig, imageURLsmall, makerID, ingredients, clientID, createdAt)
values(6, 'Adida (For other guyz)', 'I dont fucking know.. what do you want from me', 'www.google.com', 'www.google.com', 2, '7', 1, now());

insert into maker(id, name, clientID, websiteURL)
values(1, 'Happy Sad E-Cig Company', 2, 'www.google.com');
insert into maker(id, name, clientID, websiteURL)
values(2, 'Han Sauce', 2, 'www.google.com');
insert into maker( name, clientID, websiteURL)
values( 'Happy Sad E-Cig Company', 1, 'www.google.com');
insert into maker( name, clientID, websiteURL)
values( 'Han Sauce', 1, 'www.google.com');

insert into ingredient(id, name, description, groupID)
values (1, 'Vanilla', 'its vanilla lol', 0);
insert into ingredient(id, name, description, groupID)
values (2, 'Chocolate', 'its Chocolate lol', 0);
insert into ingredient(id, name, description, groupID)
values (3, 'Sprite', 'its Sprite lol', 0);
insert into ingredient(id, name, description, groupID)
values (4, 'Mango', 'its Mango lol', 0);
insert into ingredient(id, name, description, groupID)
values (5, 'Mocha', 'its Mocha lol', 0);
insert into ingredient(id, name, description, groupID)
values (6, 'Boba', 'its boba lol', 0);
insert into ingredient(id, name, description, groupID)
values (7, 'Adida Sauce Stuff', 'its wtf lol', 0);

insert into ingredients_group(id, name, description)
values(1, 'Tobacco', 'The same tobacco flavors you are used to.');
insert into ingredients_group(id, name, description)
values(2, 'Menthol', 'Menthol as usual.');
insert into ingredients_group(id, name, description)
values(3, 'Fruit', 'Bright fruity flavors for your enjoyment.');
insert into ingredients_group(id, name, description)
values(4, 'Dessert', 'Who doesnt love dessert?');
insert into ingredients_group(id, name, description)
values(5, 'Other', 'Miscellanous.');


select t.name, t.Maker, t.Description, t.ingname
from(
	select  juice.name as Name, 
			maker.name as Maker,  
			juice.description as Description,
			group_concat(ingredient.name) as ingname
	from  juice
	inner join maker
		on maker.id = juice.makerid
	inner join ingredient
		on find_in_set(ingredient.id, juice.ingredients)
	where juice.clientid=2
	group by juice.id
	 ) t;

call getJuices(2);
call attemptLogin('asdf');

select * from juice;
use fearless_flavormate;
