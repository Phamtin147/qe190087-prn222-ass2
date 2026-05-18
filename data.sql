USE FUNewsManagement;
INSERT INTO SystemAccount
    (AccountID, AccountName, AccountEmail, AccountRole, AccountPassword)
VALUES
    (1, 'Admin Demo', 'admin@FUNewsManagementSystem.org', 0, '@@abc123@@'),
    (2, 'Staff Demo', 'staff@funews.org', 1, '123'),
    (3, 'Lecturer Demo', 'lecturer@funews.org', 2, '123')
ON DUPLICATE KEY UPDATE
    AccountName = VALUES(AccountName),
    AccountEmail = VALUES(AccountEmail),
    AccountRole = VALUES(AccountRole),
    AccountPassword = VALUES(AccountPassword);

INSERT IGNORE INTO NewsArticle 
(NewsArticleID, NewsTitle, Headline, CreatedDate, NewsContent, NewsSource, CategoryID, NewsStatus, CreatedByID, UpdatedByID, ModifiedDate) 
VALUES
('NEWS001', 'FPT University Research on AI 2026', 'Leading the AI Revolution in Vietnam', '2026-05-10 08:30:00', 'Content about AI research findings...', 'FPT News', 1, 1, 1, 1, '2026-05-11 09:00:00'),
('NEWS002', 'Annual Sports Day 2026', 'A Day of Energy and Spirit', '2026-05-12 10:00:00', 'Students participating in various sports activities...', 'Student Council', 2, 1, 2, NULL, NULL),
('NEWS003', 'New Security Measures on Campus', 'Keeping our Students Safe', '2026-05-08 14:00:00', 'Implementation of new AI-powered camera systems...', 'Campus Security', 3, 1, 3, 3, '2026-05-09 15:00:00'),
('NEWS004', 'Alumni Success Story: CEO of TechVision', 'From FPT Student to Industry Leader', '2026-05-05 16:20:00', 'Interviews with our successful alumni...', 'Alumni Hub', 4, 1, 1, NULL, NULL),
('NEWS005', 'Capstone Project Showcase - Spring 2026', 'Top Innovation Projects Disclosed', '2026-05-01 09:00:00', 'Briefing on the top 10 capstone projects...', 'Academic Dept', 5, 0, 4, 5, '2026-05-02 10:00:00');

-- 2. Mock data cho NewsTag (Bảng trung gian)
-- Gắn thẻ (Tag) cho các bài viết trên
INSERT IGNORE INTO NewsTag (NewsArticleID, TagID) VALUES
('NEWS001', 2), -- AI Research -> Technology
('NEWS001', 3), -- AI Research -> Research
('NEWS001', 4), -- AI Research -> Innovation
('NEWS002', 5), -- Sports Day -> Campus Life
('NEWS002', 8), -- Sports Day -> Events
('NEWS003', 9), -- Security -> Safety
('NEWS004', 7), -- Alumni Story -> Alumni
('NEWS005', 4), -- Capstone -> Innovation
('NEWS005', 1); -- Capstone -> Education