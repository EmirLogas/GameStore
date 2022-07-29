--CREATE DATABASE GameStoreDB

--USE GameStoreDB

--DROP DATABASE GameStoreDB

CREATE TABLE UserTypes(
	UserTypeID INT IDENTITY(1,1) PRIMARY KEY,
	UserTypeName VARCHAR(30) NOT NULL,
);

CREATE TABLE Users(
	UserID INT IDENTITY(1,1) PRIMARY KEY,
	UserName VARCHAR(30) NOT NULL,
	UserEmail VARCHAR(30) NOT NULL,
	UserPassword VARCHAR(30) NOT NULL,
	UserTypeID INT NOT NULL,
	UserRegisterDate DATETIME DEFAULT GETDATE()
);

CREATE TABLE FriendUsers(
	UserID1 INT NOT NULL,
	UserID2 INT NOT NULL,
	FOREIGN KEY (UserID1) REFERENCES Users(UserID),
	FOREIGN KEY (UserID2) REFERENCES Users(UserID),
	PRIMARY KEY (UserID1, UserID2)
);

CREATE TABLE Categories(
	CategoryID INT IDENTITY(1,1) PRIMARY KEY,
	CategoryName VARCHAR(30) NOT NULL,
);

CREATE TABLE Games(
	UserID INT NOT NULL,
	GameID INT IDENTITY(1,1) PRIMARY KEY,
	GameName VARCHAR(30) NOT NULL,
	GameDescription VARCHAR(300) NOT NULL,
	GamePrice MONEY NOT NULL,
	GameCategoryID INT NOT NULL,
	GameReleaseDate DATE NOT NULL,
	GameCoverImagePath VARCHAR(100) NOT NULL,
	GameDeveloper VARCHAR(30) NOT NULL,
	GamePublisher VARCHAR(30) NOT NULL,
	FOREIGN KEY (GameCategoryID) REFERENCES Categories(CategoryID),
	FOREIGN KEY (UserID) REFERENCES Users(UserID),
);

CREATE TABLE ContentImages(
	ContentImageID INT IDENTITY(1,1) PRIMARY KEY,
	GameID INT NOT NULL,
	ContentImagePath VARCHAR(100) NOT NULL,
	FOREIGN KEY (GameID) REFERENCES Games(GameID),
);

/*CREATE TABLE Developers(
	DeveloperID INT IDENTITY(1,1) PRIMARY KEY,
	DeveloperName VARCHAR(30) NOT NULL,
);

CREATE TABLE GameDevelopers(
	GameID INT NOT NULL,
	DeveloperID INT NOT NULL,
	FOREIGN KEY (GameID) REFERENCES Games(GameID),
	FOREIGN KEY (DeveloperID) REFERENCES Developers(DeveloperID),
	PRIMARY KEY (GameID, DeveloperID)
);

CREATE TABLE Publisher(
	PublisherID INT IDENTITY(1,1) PRIMARY KEY,
	PublisherName VARCHAR(30) NOT NULL,
);

CREATE TABLE GamePublishers(
	GameID INT NOT NULL,
	PublisherID INT NOT NULL,
	FOREIGN KEY (GameID) REFERENCES Games(GameID),
	FOREIGN KEY (PublisherID) REFERENCES Publisher(PublisherID),
	PRIMARY KEY (GameID, PublisherID)
);
*/
CREATE TABLE Tags(
	TagID INT IDENTITY(1,1) PRIMARY KEY,
	TagName VARCHAR(30) NOT NULL,
);

CREATE TABLE GameTags(
	GameID INT NOT NULL,
	TagID INT NOT NULL,
	FOREIGN KEY (GameID) REFERENCES Games(GameID),
	FOREIGN KEY (TagID) REFERENCES Tags(TagID),
	PRIMARY KEY (GameID, TagID)
);

CREATE TABLE OSystems(
	OSystemID INT IDENTITY(1,1) PRIMARY KEY,
	OSystemName VARCHAR(30) NOT NULL,
);

CREATE TABLE GameOSystems(
	GameID INT NOT NULL,
	OSystemID INT NOT NULL,
	FOREIGN KEY (GameID) REFERENCES Games(GameID),
	FOREIGN KEY (OSystemID) REFERENCES OSystems(OSystemID),
	PRIMARY KEY (GameID, OSystemID)
);

CREATE TABLE Comments(
	CommentID INT IDENTITY(1,1) PRIMARY KEY,
	CommentText VARCHAR(300) NOT NULL,
	CommentDate DATETIME DEFAULT GETDATE(),
	UserID INT NOT NULL,
	GameID INT NOT NULL,
	FOREIGN KEY (UserID) REFERENCES Users(UserID),
	FOREIGN KEY (GameID) REFERENCES Games(GameID),
);

CREATE TABLE UserGames(
	UserGameID INT IDENTITY(1,1) PRIMARY KEY,
	UserID INT NOT NULL,
	GameID INT NOT NULL,
	FOREIGN KEY (UserID) REFERENCES Users(UserID),
	FOREIGN KEY (GameID) REFERENCES Games(GameID),
	UNIQUE (UserID, GameID)
);

INSERT INTO Categories(CategoryName) VALUES ('Action');
INSERT INTO Categories(CategoryName) VALUES ('Adventure');
INSERT INTO Categories(CategoryName) VALUES ('RPG');
INSERT INTO Categories(CategoryName) VALUES ('Simulation');
INSERT INTO Categories(CategoryName) VALUES ('Strategy');
INSERT INTO Categories(CategoryName) VALUES ('Sports');
INSERT INTO Categories(CategoryName) VALUES ('Racing');
INSERT INTO Categories(CategoryName) VALUES ('Fighting');
INSERT INTO Categories(CategoryName) VALUES ('Puzzle');
INSERT INTO Categories(CategoryName) VALUES ('Shooter');
INSERT INTO Categories(CategoryName) VALUES ('Horror');
INSERT INTO Categories(CategoryName) VALUES ('Platformer');
INSERT INTO Categories(CategoryName) VALUES ('MMO');
INSERT INTO Categories(CategoryName) VALUES ('Casual');
INSERT INTO Categories(CategoryName) VALUES ('Indie');
INSERT INTO Categories(CategoryName) VALUES ('Roleplay');
INSERT INTO Categories(CategoryName) VALUES ('Space & Flight');
INSERT INTO Categories(CategoryName) VALUES ('Card & Board');
INSERT INTO Categories(CategoryName) VALUES ('Building & Automation');

INSERT INTO UserTypes(UserTypeName) VALUES ('Admin');
INSERT INTO UserTypes(UserTypeName) VALUES ('User');

INSERT INTO Users(UserName, UserEmail, UserPassword, UserTypeID) VALUES ('Admin', 'admin@hotmail.com', '123',1);
INSERT INTO Users(UserName, UserEmail, UserPassword, UserTypeID) VALUES ('Emir', 'emir@hotmail.com', '123', 2);
INSERT INTO Users(UserName, UserEmail, UserPassword, UserTypeID) VALUES ('Emir2', 'emir2@hotmail.com', '123', 2);