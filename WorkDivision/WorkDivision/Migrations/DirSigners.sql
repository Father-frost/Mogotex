BEGIN TRANSACTION;
DROP TABLE IF EXISTS "DirSigners";
CREATE TABLE IF NOT EXISTS "DirSigners" (
	"id"	INTEGER NOT NULL UNIQUE,
	"Post"	TEXT,
	"FIO"	TEXT,
	"ord"	INTEGER,
	"place"	INTEGER,
	PRIMARY KEY("id" AUTOINCREMENT)
);
INSERT INTO "DirSigners" ("id","Post","FIO","ord","place") VALUES (1,'Инженер технолог ЭУ','Т.М. Заруба',1,1);
INSERT INTO "DirSigners" ("id","Post","FIO","ord","place") VALUES (2,'Мастер ПРУ','Т.И. Колтачук',2,1);
INSERT INTO "DirSigners" ("id","Post","FIO","ord","place") VALUES (3,'Мастер ПРУ','Т.И. Колтачук',3,2);
INSERT INTO "DirSigners" ("id","Post","FIO","ord","place") VALUES (4,'Экономист','А.А. Прищепа',4,2);
INSERT INTO "DirSigners" ("id","Post","FIO","ord","place") VALUES (5,'Заместитель директора-главный инженер','Е.В. Панасюк',0,0);
COMMIT;
