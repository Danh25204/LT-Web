USE lms_db;

INSERT INTO Books (Title, Author, ISBN, Description, CoverImagePath, CategoryId, Quantity, AvailableQuantity, CreatedAt) VALUES

-- Technology (CategoryId = 1)
('Clean Code', 'Robert C. Martin', '978-0132350884', 'A handbook of agile software craftsmanship. Teaches how to write readable, maintainable code.', NULL, 1, 5, 5, NOW()),
('The Pragmatic Programmer', 'David Thomas, Andrew Hunt', '978-0135957059', 'Your journey to mastery in software development, full of practical tips and techniques.', NULL, 1, 4, 4, NOW()),
('Design Patterns: Elements of Reusable Object-Oriented Software', 'Gang of Four', '978-0201633610', 'Classic reference for software design patterns used in object-oriented programming.', NULL, 1, 3, 3, NOW()),
('Introduction to Algorithms', 'Thomas H. Cormen', '978-0262033848', 'Comprehensive textbook covering a broad range of algorithms in depth.', NULL, 1, 6, 6, NOW()),
('You Don\'t Know JS', 'Kyle Simpson', '978-1491924464', 'Deep dive series into the core mechanisms of the JavaScript language.', NULL, 1, 5, 5, NOW()),
('Refactoring: Improving the Design of Existing Code', 'Martin Fowler', '978-0134757599', 'Guide to restructuring code to improve its structure without changing its behavior.', NULL, 1, 4, 4, NOW()),
('The Algorithm Design Manual', 'Steven S. Skiena', '978-1849967204', 'Practical guide to algorithm design and analysis with real-world applications.', NULL, 1, 3, 3, NOW()),
('Structure and Interpretation of Computer Programs', 'Harold Abelson', '978-0262510875', 'Foundational text in computer science using Scheme to teach programming fundamentals.', NULL, 1, 4, 4, NOW()),
('Continuous Delivery', 'Jez Humble, David Farley', '978-0321601919', 'Reliable software releases through build, test, and deployment automation.', NULL, 1, 3, 3, NOW()),
('Computer Networks', 'Andrew S. Tanenbaum', '978-0132126953', 'Comprehensive textbook on networking protocols, architectures, and technologies.', NULL, 1, 5, 5, NOW()),

-- Mathematics (CategoryId = 2)
('Calculus: Early Transcendentals', 'James Stewart', '978-1285741550', 'Standard calculus textbook used in universities worldwide covering limits, derivatives, and integrals.', NULL, 2, 6, 6, NOW()),
('Linear Algebra and Its Applications', 'Gilbert Strang', '978-0030105678', 'Clear introduction to linear algebra with applications in science and engineering.', NULL, 2, 5, 5, NOW()),
('Discrete Mathematics and Its Applications', 'Kenneth H. Rosen', '978-0073383095', 'Covers logic, set theory, combinatorics, graph theory, and computation.', NULL, 2, 5, 5, NOW()),
('Introduction to Probability', 'Dimitri P. Bertsekas', '978-1886529237', 'Rigorous introduction to probability theory with applications.', NULL, 2, 4, 4, NOW()),
('Abstract Algebra', 'David S. Dummit', '978-0471433347', 'Comprehensive coverage of group theory, ring theory, and field theory.', NULL, 2, 3, 3, NOW()),
('Numerical Analysis', 'Richard L. Burden', '978-1305253667', 'Methods for solving mathematical problems numerically using computers.', NULL, 2, 4, 4, NOW()),
('Mathematical Analysis', 'Tom M. Apostol', '978-0201002881', 'Rigorous treatment of real analysis including sequences, series, and continuity.', NULL, 2, 3, 3, NOW()),
('Graph Theory', 'Reinhard Diestel', '978-3662536216', 'Modern introduction to graph theory from the basics to advanced topics.', NULL, 2, 3, 3, NOW()),
('Statistics: The Art and Science of Learning from Data', 'Alan Agresti', '978-0321997838', 'Accessible introduction to statistical methods with real-world data examples.', NULL, 2, 5, 5, NOW()),
('Number Theory', 'George E. Andrews', '978-0486682525', 'Elementary introduction to number theory covering divisibility, primes, and congruences.', NULL, 2, 4, 4, NOW()),

-- Science (CategoryId = 3)
('A Brief History of Time', 'Stephen Hawking', '978-0553380163', 'Landmark book on cosmology explaining black holes, the Big Bang, and the nature of time.', NULL, 3, 7, 7, NOW()),
('The Selfish Gene', 'Richard Dawkins', '978-0198788607', 'Groundbreaking exploration of evolution from the perspective of the gene.', NULL, 3, 5, 5, NOW()),
('Cosmos', 'Carl Sagan', '978-0345539434', 'Epic journey through the universe and humanity\'s place within it.', NULL, 3, 6, 6, NOW()),
('The Double Helix', 'James D. Watson', '978-0743216302', 'Personal account of the discovery of the DNA structure.', NULL, 3, 4, 4, NOW()),
('Surely You\'re Joking, Mr. Feynman!', 'Richard P. Feynman', '978-0393316049', 'Adventures of a curious character — physicist Richard Feynman\'s amusing anecdotes.', NULL, 3, 5, 5, NOW()),
('The Gene: An Intimate History', 'Siddhartha Mukherjee', '978-1476733524', 'Comprehensive history of genetics from Mendel to CRISPR.', NULL, 3, 4, 4, NOW()),
('Physics of the Impossible', 'Michio Kaku', '978-0307278821', 'Exploration of futuristic technologies once thought impossible.', NULL, 3, 5, 5, NOW()),
('The Elegant Universe', 'Brian Greene', '978-0393338102', 'Superstrings, hidden dimensions, and the quest for the ultimate theory.', NULL, 3, 4, 4, NOW()),
('The Origin of Species', 'Charles Darwin', '978-0486450063', 'Darwin\'s foundational work on natural selection and evolution.', NULL, 3, 6, 6, NOW()),
('Quantum Mechanics: The Theoretical Minimum', 'Leonard Susskind', '978-0465062904', 'Rigorous introduction to quantum mechanics for those who want to truly understand it.', NULL, 3, 3, 3, NOW()),

-- Literature (CategoryId = 4)
('To Kill a Mockingbird', 'Harper Lee', '978-0061935466', 'Pulitzer Prize-winning masterpiece of American literature about racial injustice and childhood.', NULL, 4, 8, 8, NOW()),
('1984', 'George Orwell', '978-0451524935', 'Dystopian novel depicting a totalitarian society under surveillance and thought control.', NULL, 4, 7, 7, NOW()),
('The Great Gatsby', 'F. Scott Fitzgerald', '978-0743273565', 'Iconic story of ambition, wealth, and the American dream in the Jazz Age.', NULL, 4, 6, 6, NOW()),
('Brave New World', 'Aldous Huxley', '978-0060850524', 'Visionary novel imagining a genetically engineered future society controlled by pleasure.', NULL, 4, 6, 6, NOW()),
('The Catcher in the Rye', 'J.D. Salinger', '978-0316769174', 'Classic coming-of-age novel narrated by teenage Holden Caulfield.', NULL, 4, 5, 5, NOW()),
('One Hundred Years of Solitude', 'Gabriel Garcia Marquez', '978-0060883287', 'Epic magical realist saga of the Buendia family across seven generations.', NULL, 4, 5, 5, NOW()),
('Crime and Punishment', 'Fyodor Dostoevsky', '978-0143058144', 'Psychological novel about a student who commits murder and deals with his conscience.', NULL, 4, 4, 4, NOW()),
('The Alchemist', 'Paulo Coelho', '978-0062315007', 'Philosophical novel about a young shepherd\'s journey to find his personal legend.', NULL, 4, 7, 7, NOW()),
('Pride and Prejudice', 'Jane Austen', '978-0141439518', 'Romantic novel following Elizabeth Bennet as she navigates social class and marriage.', NULL, 4, 6, 6, NOW()),
('Hamlet', 'William Shakespeare', '978-0521618748', 'Tragedy about Prince Hamlet\'s quest for revenge against his uncle who murdered his father.', NULL, 4, 5, 5, NOW()),

-- History (CategoryId = 5)
('Sapiens: A Brief History of Humankind', 'Yuval Noah Harari', '978-0062316097', 'Sweeping history of the human species from the Stone Age to the twenty-first century.', NULL, 5, 8, 8, NOW()),
('The Art of War', 'Sun Tzu', '978-1590302255', 'Ancient Chinese military treatise on strategy, tactics, and philosophy.', NULL, 5, 6, 6, NOW()),
('Guns, Germs, and Steel', 'Jared Diamond', '978-0393317558', 'Examination of why Western civilization came to dominate the world.', NULL, 5, 5, 5, NOW()),
('The Rise and Fall of the Third Reich', 'William L. Shirer', '978-1451651683', 'Definitive history of Nazi Germany written by an eyewitness journalist.', NULL, 5, 4, 4, NOW()),
('A People\'s History of the United States', 'Howard Zinn', '978-0062397348', 'American history told from the perspective of ordinary people, not rulers.', NULL, 5, 5, 5, NOW()),
('The Silk Roads', 'Peter Frankopan', '978-1101912379', 'New history of the world through the lens of the Silk Roads and global trade.', NULL, 5, 4, 4, NOW()),
('Homo Deus: A Brief History of Tomorrow', 'Yuval Noah Harari', '978-0062464316', 'Exploration of humanity\'s future projects, dreams, and nightmares.', NULL, 5, 6, 6, NOW()),
('The Second World War', 'Antony Beevor', '978-0316023740', 'Panoramic history of the Second World War drawing on previously classified archives.', NULL, 5, 4, 4, NOW()),
('World Order', 'Henry Kissinger', '978-0143127710', 'Examination of the historical roots of international crises and a pathway to world order.', NULL, 5, 3, 3, NOW()),
('The History of the Ancient World', 'Susan Wise Bauer', '978-0393059748', 'Comprehensive narrative of ancient history from the earliest accounts to the fall of Rome.', NULL, 5, 5, 5, NOW());
