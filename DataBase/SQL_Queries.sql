
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