-- Drop existing tables if they exist to avoid conflicts
DROP TABLE IF EXISTS Booking;
DROP TABLE IF EXISTS Event;
DROP TABLE IF EXISTS Venue;

-- Create the Venue table
CREATE TABLE Venue (
    VenueId INT IDENTITY(1,1) PRIMARY KEY,
    VenueName NVARCHAR(100) NOT NULL,
    Location NVARCHAR(255) NOT NULL,
    Capacity INT NOT NULL,
    ImageUrl NVARCHAR(255) NULL
);

-- Create the Event table (linked to Venue)
CREATE TABLE Event (
    EventId INT IDENTITY(1,1) PRIMARY KEY,
    EventName NVARCHAR(100) NOT NULL,
    EventDate DATETIME NOT NULL,
    Description NVARCHAR(500) NULL,
    VenueId INT NOT NULL,
    CONSTRAINT FK_Event_Venue FOREIGN KEY (VenueId) REFERENCES Venue(VenueId) ON DELETE CASCADE
);

-- Create the Booking table (linked to Event)
CREATE TABLE Booking (
    BookingId INT IDENTITY(1,1) PRIMARY KEY,
    EventId INT NOT NULL,
    VenueId INT NOT NULL,  -- Add VenueId column
    BookingDate DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Booking_Event FOREIGN KEY (EventId) REFERENCES Event(EventId) ON DELETE CASCADE,
    CONSTRAINT FK_Booking_Venue FOREIGN KEY (VenueId) REFERENCES Venue(VenueId) -- Add foreign key to Venue table
);

-- Insert data into Venue table
INSERT INTO Venue (VenueName, Location, Capacity, ImageUrl)
VALUES 
('Grand Hall', '123 Main Street, Cape Town', 500, 'https://example.com/grandhall.jpg'),
('Ocean View Conference Center', '456 Beach Road, Durban', 300, 'https://example.com/oceanview.jpg'),
('Mountain Retreat', '789 Hilltop Lane, Pretoria', 200, 'https://example.com/mountainretreat.jpg');

-- Insert data into Event table (ensuring VenueId exists)
INSERT INTO Event (EventName, EventDate, Description, VenueId)
VALUES 
('Tech Conference 2025', '2025-07-10 09:00:00', 'A conference on the latest technology trends.', 1),
('Wedding Expo', '2025-08-15 11:00:00', 'A showcase of the best wedding service providers.', 2),
('Music Festival', '2025-09-05 18:00:00', 'An outdoor music festival with live performances.', 3);

-- Insert data into Booking table (ensuring EventId and VenueId exists)
INSERT INTO Booking (EventId, VenueId, BookingDate)
VALUES 
(1, 1, GETDATE()),  -- Booking for Event 1, Venue 1
(2, 2, GETDATE()),  -- Booking for Event 2, Venue 2
(3, 3, GETDATE());  -- Booking for Event 3, Venue 3

-- Check if data was inserted correctly
SELECT * FROM Venue;
SELECT * FROM Event;
SELECT * FROM Booking;
