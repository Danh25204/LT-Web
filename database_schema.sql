-- =============================================
-- LMS Database Schema (Full)
-- Hệ Thống Quản Lý Thư Viện
-- Cập nhật: 07/04/2026
-- Bao gồm: Categories, Books, Users,
--           BorrowRecords (gia hạn), BookReviews
-- =============================================

-- Reset toàn bộ database cũ
DROP DATABASE IF EXISTS lms_db;
CREATE DATABASE lms_db CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
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
    Role VARCHAR(20) NOT NULL DEFAULT 'User',  -- 'Admin' | 'User'
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    INDEX idx_email (Email)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- =============================================
-- Table: BorrowRecords
-- Status: Pending | Approved | Rejected | Returned | Late
-- ExtendCount: số lần gia hạn (tối đa 1 lần, mỗi lần +7 ngày)
-- =============================================
CREATE TABLE BorrowRecords (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    UserId INT NOT NULL,
    BookId INT NOT NULL,
    BorrowDate DATETIME NOT NULL,
    DueDate DATETIME NOT NULL,
    ReturnDate DATETIME,
    Status INT NOT NULL DEFAULT 0,  -- Pending=0, Approved=1, Returned=2, Late=3, Rejected=4
    ExtendCount INT NOT NULL DEFAULT 0,
    Notes VARCHAR(500) NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    FOREIGN KEY (BookId) REFERENCES Books(Id) ON DELETE CASCADE,
    INDEX idx_user_status (UserId, Status),
    INDEX idx_book (BookId)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- =============================================
-- Table: BookReviews
-- Mỗi user chỉ review 1 lần / 1 cuốn sách
-- Chỉ user đã mượn và trả sách mới được review
-- Rating: 1–5 sao
-- =============================================
CREATE TABLE BookReviews (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    BookId INT NOT NULL,
    UserId INT NOT NULL,
    Rating INT NOT NULL CHECK (Rating BETWEEN 1 AND 5),
    Comment VARCHAR(1000),
    CreatedAt DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    FOREIGN KEY (BookId) REFERENCES Books(Id) ON DELETE CASCADE,
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    UNIQUE KEY uq_user_book_review (BookId, UserId),
    INDEX idx_book_review (BookId)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- =============================================
-- Table: __EFMigrationsHistory (for EF Core)
-- =============================================
CREATE TABLE __EFMigrationsHistory (
    MigrationId VARCHAR(150) NOT NULL PRIMARY KEY,
    ProductVersion VARCHAR(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Mark migrations as applied (khớp với EF Core history)
INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion) VALUES
('20260325020458_InitialCreate', '8.0.0'),
('20260406170645_AddBookReviewAndExtend', '8.0.0');

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
-- Seed Data: Books (20 cuốn mẫu, 4/thể loại)
-- ID map:
--   1–4   : Công Nghệ
--   5–8   : Toán Học
--   9–12  : Khoa Học
--   13–16 : Văn Học
--   17–20 : Lịch Sử
--
-- Sách liên quan đến BorrowRecords / Reviews:
--   1  = Clean Code          (Bình đang mượn – Approved)
--   9  = A Brief History of Time (Châu đã trả – Returned)
--   11 = Cosmos              (An đã trả – Returned)
--   13 = To Kill a Mockingbird (Bình bị từ chối – Rejected)
--   14 = 1984                (An chờ duyệt – Pending)
--   15 = The Alchemist       (An đang mượn quá hạn – Overdue)
--   16 = Hamlet              (An đã trả, có gia hạn – Returned+Extended)
--   17 = Sapiens             (Châu đang mượn+gia hạn; Bình từng trả trễ)
-- =============================================
INSERT INTO Books (Title, Author, ISBN, Description, CoverImagePath, CategoryId, Quantity, AvailableQuantity, CreatedAt) VALUES

-- Công Nghệ (CategoryId = 1)
-- BookId 1: Clean Code — Bình đang mượn (Approved) → AvailableQty = Qty - 1
('Clean Code', 'Robert C. Martin', '978-0132350884', 'A handbook of agile software craftsmanship. Teaches how to write readable, maintainable code.', NULL, 1, 3, 2, NOW()),
-- BookId 2
('The Pragmatic Programmer', 'David Thomas, Andrew Hunt', '978-0135957059', 'Your journey to mastery in software development, full of practical tips and techniques.', NULL, 1, 3, 3, NOW()),
-- BookId 3
('Design Patterns', 'Gang of Four', '978-0201633610', 'Classic reference for software design patterns used in object-oriented programming.', NULL, 1, 3, 3, NOW()),
-- BookId 4
('Introduction to Algorithms', 'Thomas H. Cormen', '978-0262033848', 'Comprehensive textbook covering a broad range of algorithms in depth.', NULL, 1, 4, 4, NOW()),

-- Toán Học (CategoryId = 2)
-- BookId 5
('Calculus: Early Transcendentals', 'James Stewart', '978-1285741550', 'Standard calculus textbook covering limits, derivatives, and integrals used at universities worldwide.', NULL, 2, 4, 4, NOW()),
-- BookId 6
('Linear Algebra and Its Applications', 'Gilbert Strang', '978-0030105678', 'Clear introduction to linear algebra with applications in science and engineering.', NULL, 2, 3, 3, NOW()),
-- BookId 7
('Discrete Mathematics and Its Applications', 'Kenneth H. Rosen', '978-0073383095', 'Covers logic, set theory, combinatorics, graph theory, and computation.', NULL, 2, 3, 3, NOW()),
-- BookId 8
('Introduction to Probability', 'Dimitri P. Bertsekas', '978-1886529237', 'Rigorous introduction to probability theory with real-world applications.', NULL, 2, 3, 3, NOW()),

-- Khoa Học (CategoryId = 3)
-- BookId 9: A Brief History of Time — Châu đã trả → AvailableQty = Qty (đã cộng lại)
('A Brief History of Time', 'Stephen Hawking', '978-0553380163', 'Landmark book on cosmology explaining black holes, the Big Bang, and the nature of time.', NULL, 3, 4, 4, NOW()),
-- BookId 10
('The Selfish Gene', 'Richard Dawkins', '978-0198788607', 'Groundbreaking exploration of evolution from the perspective of the gene.', NULL, 3, 3, 3, NOW()),
-- BookId 11: Cosmos — An đã trả → AvailableQty = Qty
('Cosmos', 'Carl Sagan', '978-0345539434', 'Epic journey through the universe and humanity''s place within it.', NULL, 3, 4, 4, NOW()),
-- BookId 12
('Surely You''re Joking, Mr. Feynman!', 'Richard P. Feynman', '978-0393316049', 'Adventures of a curious character — physicist Richard Feynman''s amusing anecdotes.', NULL, 3, 3, 3, NOW()),

-- Văn Học (CategoryId = 4)
-- BookId 13: To Kill a Mockingbird — Bình bị từ chối → AvailableQty = Qty (Rejected không giảm)
('To Kill a Mockingbird', 'Harper Lee', '978-0061935466', 'Pulitzer Prize-winning masterpiece about racial injustice and childhood in the American South.', NULL, 4, 4, 4, NOW()),
-- BookId 14: 1984 — An chờ duyệt (Pending không giảm AvailableQty)
('1984', 'George Orwell', '978-0451524935', 'Dystopian novel depicting a totalitarian society under surveillance and thought control.', NULL, 4, 4, 4, NOW()),
-- BookId 15: The Alchemist — An đang mượn quá hạn (Approved) → AvailableQty = Qty - 1
('The Alchemist', 'Paulo Coelho', '978-0062315007', 'Philosophical novel about a young shepherd''s journey to find his personal legend.', NULL, 4, 3, 2, NOW()),
-- BookId 16: Hamlet — An đã trả (Returned) → AvailableQty = Qty
('Hamlet', 'William Shakespeare', '978-0521618748', 'Tragedy about Prince Hamlet''s quest for revenge against his uncle who murdered his father.', NULL, 4, 3, 3, NOW()),

-- Lịch Sử (CategoryId = 5)
-- BookId 17: Sapiens — Châu đang mượn+gia hạn (Approved) → AvailableQty = Qty - 1
('Sapiens: A Brief History of Humankind', 'Yuval Noah Harari', '978-0062316097', 'Sweeping history of the human species from the Stone Age to the twenty-first century.', NULL, 5, 3, 2, NOW()),
-- BookId 18
('The Art of War', 'Sun Tzu', '978-1590302255', 'Ancient Chinese military treatise on strategy, tactics, and philosophy.', NULL, 5, 4, 4, NOW()),
-- BookId 19
('Guns, Germs, and Steel', 'Jared Diamond', '978-0393317558', 'Examination of why Western civilization came to dominate the world.', NULL, 5, 3, 3, NOW()),
-- BookId 20
('Homo Deus: A Brief History of Tomorrow', 'Yuval Noah Harari', '978-0062464316', 'Exploration of humanity''s future projects, dreams, and nightmares.', NULL, 5, 3, 3, NOW());

-- =============================================
-- Seed Data: Normal Users (mật khẩu: User@123)
-- BCrypt hash của "User@123"
-- =============================================
INSERT INTO Users (FullName, Email, PasswordHash, Role, CreatedAt) VALUES
('Nguyễn Văn An',  'an.nguyen@email.com',  '$2a$11$PqpshrqZXDcLLq/OGmvFpOzBub/WnHMeTwDhJgakF9yEaO.AqpOja', 'User', '2025-09-01 08:00:00'),
('Trần Thị Bình',  'binh.tran@email.com',  '$2a$11$PqpshrqZXDcLLq/OGmvFpOzBub/WnHMeTwDhJgakF9yEaO.AqpOja', 'User', '2025-09-05 09:00:00'),
('Lê Minh Châu',   'chau.le@email.com',    '$2a$11$PqpshrqZXDcLLq/OGmvFpOzBub/WnHMeTwDhJgakF9yEaO.AqpOja', 'User', '2025-10-10 10:00:00');
-- Tài khoản test: user@lms.com / User@123

-- =============================================
-- Seed Data: BorrowRecords
-- Phản ánh đầy đủ các trạng thái:
--   Pending   → chờ admin duyệt
--   Approved  → đang mượn (trong hạn)
--   Approved  → đang mượn + đã gia hạn (ExtendCount=1)
--   Rejected  → bị từ chối
--   Returned  → đã trả đúng hạn
--   Late      → trả trễ
--   Overdue   → quá hạn chưa trả (Status=Approved, DueDate < NOW())
-- UserId: 2=An, 3=Bình, 4=Châu
-- Status: Pending=0, Approved=1, Returned=2, Late=3, Rejected=4
-- =============================================
INSERT INTO BorrowRecords (UserId, BookId, BorrowDate, DueDate, ReturnDate, Status, ExtendCount) VALUES
-- Pending: An xin mượn "1984" (BookId=14) → chờ admin duyệt
(2, 14, DATE_SUB(NOW(), INTERVAL 1 DAY),  DATE_ADD(NOW(), INTERVAL 13 DAY), NULL, 0, 0),

-- Approved (trong hạn): Bình mượn "Clean Code" (BookId=1)
(3, 1,  DATE_SUB(NOW(), INTERVAL 5 DAY),  DATE_ADD(NOW(), INTERVAL 9 DAY),  NULL, 1, 0),

-- Approved + đã gia hạn 1 lần: Châu mượn "Sapiens" (BookId=17), DueDate đã +7 ngày
(4, 17, DATE_SUB(NOW(), INTERVAL 10 DAY), DATE_ADD(NOW(), INTERVAL 11 DAY), NULL, 1, 1),

-- Approved OVERDUE (quá hạn, chưa trả): An mượn "The Alchemist" (BookId=15)
(2, 15, DATE_SUB(NOW(), INTERVAL 20 DAY), DATE_SUB(NOW(), INTERVAL 6 DAY),  NULL, 1, 0),

-- Rejected: Bình bị từ chối "To Kill a Mockingbird" (BookId=13)
(3, 13, DATE_SUB(NOW(), INTERVAL 3 DAY),  DATE_ADD(NOW(), INTERVAL 11 DAY), NULL, 4, 0),

-- Returned (đúng hạn): An đã trả "Cosmos" (BookId=11)
(2, 11, DATE_SUB(NOW(), INTERVAL 30 DAY), DATE_SUB(NOW(), INTERVAL 16 DAY), DATE_SUB(NOW(), INTERVAL 18 DAY), 2, 0),

-- Returned (đúng hạn): Châu đã trả "A Brief History of Time" (BookId=9)
(4, 9,  DATE_SUB(NOW(), INTERVAL 25 DAY), DATE_SUB(NOW(), INTERVAL 11 DAY), DATE_SUB(NOW(), INTERVAL 12 DAY), 2, 0),

-- Late (trả trễ): Bình trả "Sapiens" (BookId=17) lần trước, trễ 6 ngày
(3, 17, DATE_SUB(NOW(), INTERVAL 45 DAY), DATE_SUB(NOW(), INTERVAL 31 DAY), DATE_SUB(NOW(), INTERVAL 25 DAY), 3, 0),

-- Returned + gia hạn: An mượn "Hamlet" (BookId=16), gia hạn 1 lần rồi trả đúng hạn
(2, 16, DATE_SUB(NOW(), INTERVAL 35 DAY), DATE_SUB(NOW(), INTERVAL 14 DAY), DATE_SUB(NOW(), INTERVAL 15 DAY), 2, 1);

-- =============================================
-- Seed Data: BookReviews
-- Chỉ user đã có BorrowRecord Returned/Late mới có thể review
-- An (UserId=2)  : đã trả Cosmos(11), Hamlet(16)
-- Châu (UserId=4): đã trả A Brief History of Time(9)
-- Bình (UserId=3): đã trả trễ Sapiens(17)
-- =============================================
INSERT INTO BookReviews (BookId, UserId, Rating, Comment, CreatedAt) VALUES
(11, 2, 5, 'Cuốn sách tuyệt vời! Carl Sagan dẫn dắt người đọc qua vũ trụ bao la với văn phong cực kỳ cuốn hút.', NOW()),
(16, 2, 4, 'Shakespeare viết hay nhưng hơi khó hiểu với ngôn ngữ cổ. Vẫn rất đáng đọc.', NOW()),
(9,  4, 5, 'Stephen Hawking giải thích vật lý phức tạp một cách dễ hiểu đến kinh ngạc. Sách cực hay!', NOW()),
(17, 3, 4, 'Yuval Harari có cái nhìn rất sắc bén về lịch sử loài người. Đọc một hơi không muốn dừng.', NOW());

