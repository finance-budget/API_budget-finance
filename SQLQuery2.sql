CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50),
    Password NVARCHAR(50)
);

INSERT INTO Users (Username, Password) VALUES ('admin', '1234');

SELECT * FROM Users;