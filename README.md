In order to use this project. You must have a local database in Microsoft SQL Server Management studio.
Name your local database: "Bokhanteringsdatabas".

Use this SQL code to create the tables: 

-- Create the Authors-table
CREATE TABLE Authors (
    AuthorID INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL
);

-- Create the Books-table
CREATE TABLE Books (
    BookID INT IDENTITY(1,1) PRIMARY KEY,
    Title VARCHAR(200) NOT NULL,
    PublishedYear INT NOT NULL,
    AuthorID INT NOT NULL,
    FOREIGN KEY (AuthorID) REFERENCES Authors(AuthorID)
);
