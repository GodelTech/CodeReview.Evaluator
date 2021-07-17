CREATE TABLE "FileDetails" (
	"Id"	INTEGER,
	"FilePath"	TEXT NOT NULL,
	"Language"	TEXT NOT NULL,
	"Blank"		INTEGER,
	"Code"		INTEGER,
	"Commented"	INTEGER,

	PRIMARY KEY("Id" AUTOINCREMENT)
);
