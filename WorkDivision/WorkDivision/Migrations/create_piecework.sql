CREATE TABLE "PieceWork" (
	"id"	INTEGER NOT NULL UNIQUE,
	"id_worker"	INTEGER,
	"id_indivision"	INTEGER,
	"id_division"	INTEGER,
	"cardnum"	INTEGER,
	"cnt"	REAL,
	"mm"	INTEGER,
	"yy"	INTEGER,
	PRIMARY KEY("id" AUTOINCREMENT)
)