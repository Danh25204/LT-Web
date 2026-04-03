-- =============================================
-- LMS Database Schema
-- Hệ Thống Quản Lý Thư Viện
-- =============================================

CREATE DATABASE IF NOT EXISTS lms_db CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE lms_db;

-- =============================================
-- Table: Categories
-- =============================================
CREATE TABLE Categories (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Description TEXT,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- =============================================
-- Table: Books
-- =============================================
CREATE TABLE Books (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Title VARCHAR(255) NOT NULL,
    Author VARCHAR(255) NOT NULL,
    ISBN VARCHAR(20),
    Description TEXT,
    CoverImagePath VARCHAR(500),
    CategoryId INT NOT NULL,
    Quantity INT NOT NULL DEFAULT 1,
    AvailableQuantity INT NOT NULL DEFAULT 1,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id) ON DELETE CASCADE,
    INDEX idx_category (CategoryId),
    INDEX idx_title (Title)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- =============================================
-- Table: Users
-- =============================================
CREATE TABLE Users (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    FullName VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL,
    Role VARCHAR(20) NOT NULL DEFAULT 'User',
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    INDEX idx_email (Email)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- =============================================
-- Table: BorrowRecords
-- =============================================
CREATE TABLE BorrowRecords (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    UserId INT NOT NULL,
    BookId INT NOT NULL,
    BorrowDate DATETIME NOT NULL,
    DueDate DATETIME NOT NULL,
    ReturnDate DATETIME,
    Status VARCHAR(20) NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    FOREIGN KEY (BookId) REFERENCES Books(Id) ON DELETE CASCADE,
    INDEX idx_user_status (UserId, Status),
    INDEX idx_book (BookId)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- =============================================
-- Table: __EFMigrationsHistory (for EF Core)
-- =============================================
CREATE TABLE __EFMigrationsHistory (
    MigrationId VARCHAR(150) NOT NULL PRIMARY KEY,
    ProductVersion VARCHAR(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- =============================================
-- Seed Data: Categories
-- =============================================
INSERT INTO Categories (Name, Description) VALUES
('Công Nghệ', 'Sách về lập trình, phần mềm, và công nghệ thông tin'),
('Toán Học', 'Sách về toán học, giải tích, đại số và lý thuyết số'),
('Khoa Học', 'Sách về vật lý, hóa học, sinh học và các ngành khoa học tự nhiên'),
('Văn Học', 'Tiểu thuyết, thơ ca, và các tác phẩm văn học kinh điển'),
('Lịch Sử', 'Sách về lịch sử thế giới, văn minh và các sự kiện lịch sử');

-- =============================================
-- Seed Data: Admin User
-- Email: admin@lms.com
-- Password: Admin@123
-- =============================================
INSERT INTO Users (FullName, Email, PasswordHash, Role, CreatedAt) VALUES
('Administrator', 'admin@lms.com', '$2a$11$GAWhsdOts9ND916lKvaMOOAdUBJSaFRqJcfkq6q53oMfOUGBX5.WK', 'Admin', '2024-01-01 00:00:00');

-- =============================================
-- Note: Sử dụng file seed_books.sql để thêm 50 sách mẫu
-- =============================================
