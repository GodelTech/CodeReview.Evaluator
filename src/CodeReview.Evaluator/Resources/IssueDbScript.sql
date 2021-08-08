CREATE TABLE "Issues" (
	"Id"	INTEGER,
	"RuleId"	TEXT NOT NULL,
	"Level"	TEXT NOT NULL,
	"Title"	TEXT,
	"Message"	TEXT NOT NULL,
	"Description"	TEXT,
	"DetailsUrl"	TEXT,
	"Category"	TEXT,
	PRIMARY KEY("Id"  AUTOINCREMENT)
);

CREATE TABLE "IssueLocations" (
	"Id"	INTEGER,
	"IssueId"	INTEGER,
	"FilePath"	TEXT NOT NULL,
	"StartLine"	INTEGER,
	"EndLine"	INTEGER,
	PRIMARY KEY("Id" AUTOINCREMENT),
	FOREIGN KEY ("IssueId")
		REFERENCES "Issues" ("Id") 
			ON DELETE CASCADE
);

CREATE TABLE "IssueTags" (
	"Id"	INTEGER,
	"IssueId"	INTEGER,
	"Name"	TEXT NOT NULL,
	PRIMARY KEY("Id" AUTOINCREMENT),
	FOREIGN KEY ("IssueId")
		REFERENCES "Issues" ("Id") 
			ON DELETE CASCADE
);

CREATE TABLE "IssueHashes" (
	"Id"	    INTEGER,
	"IssueId"	INTEGER,
	"Algorithm"	TEXT NOT NULL,
	"Value" 	TEXT NOT NULL,
	PRIMARY KEY("Id" AUTOINCREMENT),
	FOREIGN KEY ("IssueId")
		REFERENCES "Issues" ("Id") 
			ON DELETE CASCADE
);


CREATE TABLE "IssueProperties" (
	"Id"	    INTEGER,
	"IssueId"	INTEGER,
	"Property"	TEXT NOT NULL,
	"Value" 	TEXT NOT NULL,
	PRIMARY KEY("Id" AUTOINCREMENT),
	FOREIGN KEY ("IssueId")
		REFERENCES "Issues" ("Id") 
			ON DELETE CASCADE
);

CREATE INDEX "IssueLocations_IssueId"
	ON "IssueLocations" ("IssueId");

CREATE INDEX "IssueTags_IssueId"
	ON "IssueTags" ("IssueId");

CREATE INDEX "IssueHashes_IssueId"
	ON "IssueHashes" ("IssueId");

CREATE INDEX "IssueHashes_Value_Algorithm"
	ON "IssueHashes" ("Value", "Algorithm");

CREATE INDEX "IssueProperties_Property"
	ON "IssueProperties" ("Property");