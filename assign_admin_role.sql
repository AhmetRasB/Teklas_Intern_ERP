-- ===================================================================
-- ASSIGN ADMIN ROLE TO USER
-- ===================================================================

-- First, let's check if the admin role exists
SELECT 'Admin Role Check:' as Info;
SELECT Id, Name, DisplayName, IsSystemRole, IsActive 
FROM Roles 
WHERE Name = 'Admin';

-- Check if admin user exists
SELECT 'Admin User Check:' as Info;
SELECT Id, Username, Email, FirstName, LastName, IsActive 
FROM Users 
WHERE Username = 'admin';

-- Check current user roles
SELECT 'Current User Roles:' as Info;
SELECT 
    u.Username,
    r.Name as RoleName,
    ur.AssignedDate,
    ur.IsActive
FROM UserRoles ur
JOIN Users u ON ur.UserId = u.Id
JOIN Roles r ON ur.RoleId = r.Id
WHERE u.Username = 'admin';

-- If admin role doesn't exist, create it
IF NOT EXISTS (SELECT 1 FROM Roles WHERE Name = 'Admin')
BEGIN
    INSERT INTO Roles (
        Name, DisplayName, Description, IsSystemRole, Priority, IsActive, CreateDate, CreateUserId, IsDeleted, Status
    ) VALUES (
        'Admin', 'System Administrator', 'Full system access and control', 1, 100, 1, GETDATE(), 1, 0, 1
    );
    PRINT 'Admin role created.';
END

-- If admin user doesn't exist, create it
IF NOT EXISTS (SELECT 1 FROM Users WHERE Username = 'admin')
BEGIN
    INSERT INTO Users (
        Username, Email, PasswordHash, PasswordSalt, FirstName, LastName, IsActive, CreateDate, CreateUserId, IsDeleted, Status
    ) VALUES (
        'admin', 'admin@teklas.com', 'hashedpassword1', 'salt1', 'Admin', 'User', 1, GETDATE(), 1, 0, 1
    );
    PRINT 'Admin user created.';
END

-- Assign admin role to admin user (if not already assigned)
IF NOT EXISTS (
    SELECT 1 FROM UserRoles ur
    JOIN Users u ON ur.UserId = u.Id
    JOIN Roles r ON ur.RoleId = r.Id
    WHERE u.Username = 'admin' AND r.Name = 'Admin'
)
BEGIN
    INSERT INTO UserRoles (
        UserId, RoleId, AssignedDate, IsActive, Id, CreateDate, CreateUserId, IsDeleted, Status
    )
    SELECT 
        u.Id, 
        r.Id, 
        GETDATE(), 
        1, 
        1, -- Id value
        GETDATE(), 
        1, 
        0, 
        1
    FROM Users u, Roles r
    WHERE u.Username = 'admin' AND r.Name = 'Admin';
    
    PRINT 'Admin role assigned to admin user.';
END
ELSE
BEGIN
    PRINT 'Admin role is already assigned to admin user.';
END

-- Final verification
SELECT 'Final Verification:' as Info;
SELECT 
    u.Username,
    r.Name as RoleName,
    ur.AssignedDate,
    ur.IsActive
FROM UserRoles ur
JOIN Users u ON ur.UserId = u.Id
JOIN Roles r ON ur.RoleId = r.Id
WHERE u.Username = 'admin'; 