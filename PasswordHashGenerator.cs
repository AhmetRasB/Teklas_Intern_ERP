using System;
using System.Security.Cryptography;
using System.Text;

class PasswordHashGenerator
{
    static void Main()
    {
        Console.WriteLine("=== Teklas ERP Password Hash Generator ===");
        Console.WriteLine();
        
        Console.Write("Enter the password you want to hash: ");
        string password = Console.ReadLine() ?? "admin123";
        
        string salt = "TeklasSalt";
        string hash = HashPassword(password, salt);
        
        Console.WriteLine();
        Console.WriteLine("=== Generated SQL Queries ===");
        Console.WriteLine();
        
        // Query 1: Change existing admin password
        Console.WriteLine("-- 1. Change existing admin password");
        Console.WriteLine($"UPDATE Users");
        Console.WriteLine($"SET PasswordHash = '{hash}',");
        Console.WriteLine($"    PasswordSalt = '{salt}',");
        Console.WriteLine($"    UpdateDate = GETDATE()");
        Console.WriteLine($"WHERE Username = 'admin';");
        Console.WriteLine();
        
        // Query 2: Add new admin user
        Console.WriteLine("-- 2. Add new admin user");
        Console.WriteLine($"INSERT INTO Users (");
        Console.WriteLine($"    Username, Email, PasswordHash, PasswordSalt, FirstName, LastName,");
        Console.WriteLine($"    IsActive, CreateDate, CreateUserId, IsDeleted, Status");
        Console.WriteLine($") VALUES (");
        Console.WriteLine($"    'newadmin', 'newadmin@teklas.com', '{hash}', '{salt}', 'New', 'Admin',");
        Console.WriteLine($"    1, GETDATE(), 1, 0, 1");
        Console.WriteLine($");");
        Console.WriteLine();
        
        // Query 3: Assign admin role to new user
        Console.WriteLine("-- 3. Assign admin role to new user (run after creating user)");
        Console.WriteLine($"INSERT INTO UserRoles (");
        Console.WriteLine($"    UserId, RoleId, AssignedDate, IsActive, CreateDate, CreateUserId, IsDeleted, Status");
        Console.WriteLine($") VALUES (");
        Console.WriteLine($"    (SELECT Id FROM Users WHERE Username = 'newadmin'),");
        Console.WriteLine($"    (SELECT Id FROM Roles WHERE Name = 'Admin'),");
        Console.WriteLine($"    GETDATE(), 1, GETDATE(), 1, 0, 1");
        Console.WriteLine($");");
        Console.WriteLine();
        
        // Query 4: Complete admin creation in one transaction
        Console.WriteLine("-- 4. Complete admin creation in one transaction");
        Console.WriteLine($"BEGIN TRANSACTION;");
        Console.WriteLine();
        Console.WriteLine($"-- Insert new admin user");
        Console.WriteLine($"INSERT INTO Users (");
        Console.WriteLine($"    Username, Email, PasswordHash, PasswordSalt, FirstName, LastName,");
        Console.WriteLine($"    IsActive, CreateDate, CreateUserId, IsDeleted, Status");
        Console.WriteLine($") VALUES (");
        Console.WriteLine($"    'superadmin', 'superadmin@teklas.com', '{hash}', '{salt}', 'Super', 'Admin',");
        Console.WriteLine($"    1, GETDATE(), 1, 0, 1");
        Console.WriteLine($");");
        Console.WriteLine();
        Console.WriteLine($"-- Get the new user ID");
        Console.WriteLine($"DECLARE @NewUserId BIGINT = SCOPE_IDENTITY();");
        Console.WriteLine();
        Console.WriteLine($"-- Assign admin role");
        Console.WriteLine($"INSERT INTO UserRoles (");
        Console.WriteLine($"    UserId, RoleId, AssignedDate, IsActive, CreateDate, CreateUserId, IsDeleted, Status");
        Console.WriteLine($") VALUES (");
        Console.WriteLine($"    @NewUserId,");
        Console.WriteLine($"    (SELECT Id FROM Roles WHERE Name = 'Admin'),");
        Console.WriteLine($"    GETDATE(), 1, GETDATE(), 1, 0, 1");
        Console.WriteLine($");");
        Console.WriteLine();
        Console.WriteLine($"COMMIT TRANSACTION;");
        Console.WriteLine();
        
        Console.WriteLine("=== Password Information ===");
        Console.WriteLine($"Password: {password}");
        Console.WriteLine($"Hash: {hash}");
        Console.WriteLine($"Salt: {salt}");
        Console.WriteLine();
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    static string HashPassword(string password, string salt)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
        return Convert.ToBase64String(hashedBytes);
    }
} 