
CREATE TABLE "User" (
	"Id"	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	"FirstName"	TEXT NOT NULL,
	"LastName"	TEXT NOT NULL,
	"Email"	TEXT NOT NULL,
	"PasswordHash"	BLOB,
	"PasswordSalt"	BLOB,
	"Role"	TEXT NOT NULL,
	"IsActive"	INTEGER DEFAULT 1,
	"DateIn"	TEXT NOT NULL DEFAULT (datetime('now'))
);

SELECT * FROM User

delete from User

INSERT INTO "main"."User"
("FirstName","LastName","Email","PasswordHash","PasswordSalt","Token","Role") 
VALUES (
	'Henrique',
	'Adriano',
	'henrique.adriano@gmail.com',
	NULL,
	NULL,
	NULL,
	'Admin'
);

SELECT * FROM User

--delete from User

INSERT INTO User
(FirstName,LastName,Email,PasswordHash,PasswordSalt,Token,Role) 
VALUES 
('Henrique','Adriano','henrique.adriano@gmail.com',NULL,NULL,NULL,'Admin'),
('Kelvia','Souza','kelviasouza@gmail.com',NULL,NULL,NULL,'User');

Update User set 
	FirstName = 'kelvia',
	LastName = 'souza',
	Email = 'email@email.com',
	PasswordHash = NULL,
	PasswordSalt = NULL,
	Token = NULL,
	Role = 'User' 
Where Id =6

/*
https://jasonwatmore.com/post/2019/10/14/aspnet-core-3-simple-api-for-authentication-registration-and-user-management
https://dapper-tutorial.net/dapper-mapper
https://jasonwatmore.com/post/2019/10/16/aspnet-core-3-role-based-authorization-tutorial-with-example-api
https://www.c-sharpcorner.com/article/model-validation-using-data-annotations-in-asp-net-mvc/
https://blog.maskalik.com/asp-net/sqlite-simple-database-with-dapper/
https://medium.com/@berkayyerdelen/building-restful-api-with-dapper-and-asp-net-core-37e6d9d1bdda
https://piotrgankiewicz.com/2017/06/12/asp-net-core-deployment-using-docker-nginx-and-ubuntu-server/

*/