use fearless_flavormate;

call updateJuice(1, 0, 'Dynamic 1', 'This was generated programatically. How sick, right?!', 3, '1,2,3,4', 1);
call updateJuice(1, 0, 'Dynamic 2', 'This was generated programatically. How sick, right?!', 3, '1,2,3,4');
call updateJuice(2, 0, 'Dynamic 1', 'This was generated programatically. How sick, right?!', 4, '1,2,3,4');
call updateJuice(2, 0, 'Dynamic 2', 'This was generated programatically. How sick, right?!', 4, '1,2,3,4');
call updateJuice(2, 0, 'Dynamic 3', 'This was generated programatically. How sick, right?!', 4, '1,2,3,4', 0);
call updateJuice(2, 0, 'Dynamic 4', 'This was generated programatically. How sick, right?!', 4, '1,2,3,4', 0);

select * from juice;



call getMakers(1);
call getJuices(2);

call updateMaker(1, 0, 'NewGuy', 0);
call updateMaker(2, 0, 'NewGuy', 0);

select * from maker;

select * from juice;
select * from juice_audit;

select * from ingredient;
