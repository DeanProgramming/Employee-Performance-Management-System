USE EmployeePerformanceDB;
GO


IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'HR')
    CREATE ROLE HR;
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'Manager')
    CREATE ROLE Manager;
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'Employee')
    CREATE ROLE Employee;
GO


GRANT SELECT, INSERT, UPDATE, DELETE ON Employee.EmployeeInfo TO HR;
GRANT SELECT, INSERT, UPDATE, DELETE ON Employee.PerformanceReview TO HR;
GRANT SELECT, INSERT, UPDATE, DELETE ON Department.EmployeeDepartment TO HR;

GRANT SELECT, INSERT, UPDATE ON Employee.EmployeeInfo TO Manager;
GRANT SELECT, INSERT, UPDATE ON Employee.PerformanceReview TO Manager;

GRANT SELECT ON Employee.EmployeeInfo TO Employee;
GO
