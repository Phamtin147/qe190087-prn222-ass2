CREATE DATABASE IF NOT EXISTS FUNewsManagement CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE FUNewsManagement;

CREATE TABLE IF NOT EXISTS Category (
    CategoryID SMALLINT NOT NULL AUTO_INCREMENT,
    CategoryName VARCHAR(100) NOT NULL,
    CategoryDesciption VARCHAR(250) NOT NULL,
    ParentCategoryID SMALLINT NULL,
    IsActive BOOLEAN NULL,
    CONSTRAINT PK_Category PRIMARY KEY (CategoryID)
);

CREATE TABLE IF NOT EXISTS SystemAccount (
    AccountID SMALLINT NOT NULL,
    AccountName VARCHAR(100) NOT NULL,
    AccountEmail VARCHAR(70) NOT NULL,
    AccountRole INT NOT NULL,
    AccountPassword VARCHAR(70) NOT NULL,
    CONSTRAINT PK_SystemAccount PRIMARY KEY (AccountID)
);

CREATE TABLE IF NOT EXISTS Tag (
    TagID INT NOT NULL,
    TagName VARCHAR(50) NOT NULL,
    Note VARCHAR(400) NULL,
    CONSTRAINT PK_HashTag PRIMARY KEY (TagID)
);

CREATE TABLE IF NOT EXISTS NewsArticle (
    NewsArticleID VARCHAR(20) NOT NULL,
    NewsTitle VARCHAR(400) NULL,
    Headline VARCHAR(150) NOT NULL,
    CreatedDate DATETIME NULL,
    NewsContent VARCHAR(4000) NULL,
    NewsSource VARCHAR(400) NULL,
    CategoryID SMALLINT NOT NULL,
    NewsStatus BOOLEAN NULL,
    CreatedByID SMALLINT NOT NULL,
    UpdatedByID SMALLINT NULL,
    ModifiedDate DATETIME NULL,
    CONSTRAINT PK_NewsArticle PRIMARY KEY (NewsArticleID)
);

CREATE TABLE IF NOT EXISTS NewsTag (
    NewsArticleID VARCHAR(20) NOT NULL,
    TagID INT NOT NULL,
    CONSTRAINT PK_NewsTag PRIMARY KEY (NewsArticleID, TagID)
);

ALTER TABLE Category
    ADD CONSTRAINT FK_Category_Category FOREIGN KEY IF NOT EXISTS (ParentCategoryID) REFERENCES Category (CategoryID);

ALTER TABLE NewsArticle
    ADD CONSTRAINT FK_NewsArticle_Category FOREIGN KEY IF NOT EXISTS (CategoryID) REFERENCES Category (CategoryID)
        ON UPDATE CASCADE ON DELETE CASCADE;

ALTER TABLE NewsArticle
    ADD CONSTRAINT FK_NewsArticle_SystemAccount FOREIGN KEY IF NOT EXISTS (CreatedByID) REFERENCES SystemAccount (AccountID)
        ON UPDATE CASCADE ON DELETE CASCADE;

ALTER TABLE NewsTag
    ADD CONSTRAINT FK_NewsTag_NewsArticle FOREIGN KEY IF NOT EXISTS (NewsArticleID) REFERENCES NewsArticle (NewsArticleID);

ALTER TABLE NewsTag
    ADD CONSTRAINT FK_NewsTag_Tag FOREIGN KEY IF NOT EXISTS (TagID) REFERENCES Tag (TagID);

INSERT IGNORE INTO Category (CategoryID, CategoryName, CategoryDesciption, ParentCategoryID, IsActive) VALUES
(1, 'Academic news', 'Research findings, faculty appointments and academic announcements.', 1, 1),
(2, 'Student Affairs', 'Student activities, events, clubs, organizations and sports.', 2, 1),
(3, 'Campus Safety', 'Incidents and safety measures implemented on campus.', 3, 1),
(4, 'Alumni News', 'Achievements and accomplishments of former students and alumni.', 4, 1),
(5, 'Capstone Project News', 'Capstone project reports.', 5, 0);

INSERT IGNORE INTO SystemAccount (AccountID, AccountName, AccountEmail, AccountRole, AccountPassword) VALUES
(1, 'Emma William', 'EmmaWilliam@FUNewsManagement.org', 2, '@1'),
(2, 'Olivia James', 'OliviaJames@FUNewsManagement.org', 2, '@1'),
(3, 'Isabella David', 'IsabellaDavid@FUNewsManagement.org', 1, '@1'),
(4, 'Michael Charlotte', 'MichaelCharlotte@FUNewsManagement.org', 1, '@1'),
(5, 'Steve Paris', 'SteveParis@FUNewsManagement.org', 1, '@1');

INSERT IGNORE INTO Tag (TagID, TagName, Note) VALUES
(1, 'Education', 'Education Note'),
(2, 'Technology', 'Technology Note'),
(3, 'Research', 'Research Note'),
(4, 'Innovation', 'Innovation Note'),
(5, 'Campus Life', 'Campus Life Note'),
(6, 'Faculty', 'Faculty Achievements'),
(7, 'Alumni', 'Alumni News'),
(8, 'Events', 'University Events'),
(9, 'Safety', 'Campus Safety');
