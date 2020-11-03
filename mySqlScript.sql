CREATE DATABASE CinelAirTickets;

USE CinelAirTickets;

SET time_zone = '+00:00';

/* Table definitions */
-- Regions
CREATE TABLE Regions(
    Id INT AUTO_INCREMENT,
    Name VARCHAR(150) NOT NULL,
    PRIMARY KEY (Id)
);

-- Aeroports
CREATE TABLE Aeroports(
	Id INT AUTO_INCREMENT,
    IATA CHAR(3) NOT NULL,
    Latitude DECIMAL(10, 8) NOT NULL,
    Longitude DECIMAL(11, 8) NOT NULL, 
    RegionId INT NOT NULL,
    FOREIGN KEY (RegionId) REFERENCES Regions(Id),
    PRIMARY KEY (Id)
);

-- Seat Classes
CREATE TABLE SeatClasses(
    Id INT AUTO_INCREMENT,
    Description VARCHAR(20) NOT NULL,
    PRIMARY KEY (Id)
);

-- Tickets
CREATE TABLE Tickets(
    Id INT AUTO_INCREMENT,
    FirstName VARCHAR(70),
    LastName VARCHAR(70),
    MilesProgramNumber VARCHAR(9),
    DateDeparture DATETIME NOT NULL,
    DateArrival DATETIME NOT NULL,
    AeroportDepartureId INT NOT NULL,
    AeroportArrivalId INT NOT NULL,
    SeatClassId INT NOT NULL,
    PRIMARY KEY (Id),
    FOREIGN KEY (AeroportDepartureId) REFERENCES Aeroports(Id),
    FOREIGN KEY (AeroportArrivalId) REFERENCES Aeroports(Id),
    FOREIGN KEY (SeatClassId) REFERENCES SeatClasses(Id),
    CHECK (DateArrival > DateDeparture)
);


/* Data seeding */
-- Regions
INSERT INTO Regions (Name) VALUE
    ('Europe'),
    ('North Africa'),
    ('South Africa'),
    ('North America'),
    ('South America'),
    ('Asia'),
    ('Oceania');

-- Aeroports
INSERT INTO Aeroports (IATA, Latitude, Longitude, RegionId) VALUES
    ('LIS', 38.774167, -9.134167, 1),
    ('OPO', 41.24810028, -8.68138981, 1),
    ('ALC', 38.28219986, -0.55815601, 1),
    ('CMN', 33.36750031, -7.58997011, 2),
    ('RAK', 31.60689926, -8.03629971, 2),
    ('LAS', 36.08010101, -115.1520004, 4),
    ('LAX', 33.94250107, -118.4079971, 4),
    ('GRU', -23.43555641, -46.47305679, 5),
    ('GIG', -22.80999947, -43.25055695, 5);

-- Seat Classes
INSERT INTO SeatClasses Values
    ('Discount'), ('Basic'), ('Classic'), ('Business');

-- Tickets
INSERT INTO Tickets
(FirstName, LastName, MilesProgramNumber, DateDeparture, DateArrival, 
AeroportDepartureId, AeroportArrivalId, SeatClassId) VALUES
    ('Bruno', 'Tavares', '456789123', '2020-10-31 14:30', '2020-10-31 15:45', 1, 2, 2);


/* Queries */
-- API GET Query
SELECT Tickets.MilesProgramNumber, Departure.Latitude, Departure.Latitude, Arrival.Longitude, Arrival.Longitude, Tickets.SeatClassId
FROM Tickets, Aeroports AS Departure, Aeroports AS Arrival
WHERE Tickets.AeroportDepartureId = Departure.Id AND Tickets.AeroportArrivalId = Arrival.Id

SELECT Tickets.MilesProgramNumber, Departure.Latitude, Departure.Latitude, Arrival.Longitude, Arrival.Longitude, Tickets.SeatClassId
FROM Tickets, Aeroports AS Departure, Aeroports AS Arrival
WHERE Tickets.AeroportDepartureId = Departure.Id AND Tickets.AeroportArrivalId = Arrival.Id