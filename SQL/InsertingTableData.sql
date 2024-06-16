use EmployeePerformanceDB;
go

SET IDENTITY_INSERT Employee.EmployeeInfo ON;
go

CREATE PROCEDURE InsertEmployeeInfo
    @EmployeeID INT,
    @FirstName VARCHAR(255),
    @LastName VARCHAR(255),
    @Position VARCHAR(255),
    @HireDate DATE,
	@UserName VARCHAR(255),
    @Role VARCHAR(255)
AS
BEGIN
    INSERT INTO Employee.EmployeeInfo (employee_id, first_name, last_name, position, role_position, hire_date)
    VALUES (@EmployeeID, @FirstName, @LastName, @Position, @Role, @HireDate);
	 
    -- Insert user into AspNetUsers
    INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount)
    VALUES (@EmployeeID, @UserName, UPPER(@UserName), @UserName + '@example.com', UPPER(@UserName + '@example.com'), 1, 
            'AQAAAAIAAYagAAAAEC9LifLJYADjCODAqX2wGnmCnA1cACbMkhsEArIPy4ZXvRVG5fZCieOeU51wqjTohg==', 
            NEWID(), NEWID(), 0, 0, 0, 0);

    -- Insert user role into AspNetUserRoles
    DECLARE @UserId NVARCHAR(450) = (SELECT Id FROM AspNetUsers WHERE UserName = @UserName);
    DECLARE @RoleId NVARCHAR(450) = (SELECT Id FROM AspNetRoles WHERE Name = @Role);

    IF @RoleId IS NULL
    BEGIN
        -- Insert role into AspNetRoles if it doesn't exist
        SET @RoleId = NEWID();
        INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
        VALUES (@RoleId, @Role, UPPER(@Role), NEWID());
    END

    INSERT INTO AspNetUserRoles (UserId, RoleId)
    VALUES (@UserId, @RoleId);
END;
go
 
-- TEAM A  
EXEC InsertEmployeeInfo 1, 'John', 'Doe', 'Tech Lead', '2022-01-15', 'john.doe', 'Manager';
EXEC InsertEmployeeInfo 2, 'Jane', 'Smith', 'Software Developer', '2022-03-22', 'jane.smith',  'Employee';
EXEC InsertEmployeeInfo 3, 'Alice', 'Johnson', 'Software Developer', '2022-06-10', 'alice.johnson',  'Employee';
EXEC InsertEmployeeInfo 4, 'Bob', 'Williams', 'Software Developer', '2022-08-05', 'bob.williams',  'Employee';
EXEC InsertEmployeeInfo 5, 'Charlie', 'Brown', 'Software Developer', '2022-10-18', 'charlie.brown',  'Employee';
EXEC InsertEmployeeInfo 6, 'David', 'Jones', 'Software Developer', '2023-02-14', 'david.jones',  'Employee';
EXEC InsertEmployeeInfo 7, 'Eva', 'Miller', 'Software Developer', '2023-04-30', 'eva.miller',  'Employee';
EXEC InsertEmployeeInfo 8, 'Frank', 'Davis', 'Software Developer', '2023-07-21', 'frank.davis',  'Employee';

-- TEAM B 
EXEC InsertEmployeeInfo 9, 'Grace', 'Hall', 'Tech Lead', '2023-09-12', 'grace.hall',  'Manager';
EXEC InsertEmployeeInfo 10, 'Hannah', 'Moore', 'Software Developer', '2023-09-12', 'hannah.moore',  'Employee';
EXEC InsertEmployeeInfo 11, 'Isaac', 'Clark', 'Software Developer', '2024-01-15', 'isaac.clark',  'Employee';
EXEC InsertEmployeeInfo 12, 'Jack', 'Lewis', 'Software Developer', '2024-01-20', 'jack.lewis',  'Employee';

-- HR
EXEC InsertEmployeeInfo 13, 'Kate', 'Martin', 'HR Lead', '2023-05-09', 'kate.martin',  'HR';
EXEC InsertEmployeeInfo 14, 'Liam', 'Walker', 'HR', '2023-07-22', 'liam.walker',  'HR';

-- SUPPORT
EXEC InsertEmployeeInfo 15, 'Mia', 'Harris', 'Support Team Lead', '2023-06-13', 'mia.harris',  'Manager';
EXEC InsertEmployeeInfo 16, 'Noah', 'Clarkson', 'Support Team', '2023-08-25', 'noah.clarkson',  'Employee';
GO

go


SET IDENTITY_INSERT Employee.EmployeeInfo OFF;
go
 

CREATE PROCEDURE InsertPerformanceReview
    @EmployeeID INT,
    @ReviewDate DATE,
    @Score INT,
    @Comments VARCHAR(255)
AS
BEGIN
    INSERT INTO Employee.PerformanceReview (employee_id, review_date, score, comments)
    VALUES (@EmployeeID, @ReviewDate, @Score, @Comments);
END;
go

--TEAM A 
EXEC InsertPerformanceReview 1, '2023-04-15', 92, 'Continued strong leadership skills';
EXEC InsertPerformanceReview 1, '2023-06-20', 88, 'Effective team collaboration';
EXEC InsertPerformanceReview 1, '2023-09-25', 91, 'Exceeds expectations in all tasks';

EXEC InsertPerformanceReview 2, '2023-05-10', 82, 'Improving communication skills';
EXEC InsertPerformanceReview 2, '2023-07-15', 85, 'Strong technical abilities';
EXEC InsertPerformanceReview 2, '2023-11-22', 87, 'Excellent problem-solving skills';

EXEC InsertPerformanceReview 3, '2023-04-18', 78, 'Steady improvement';
EXEC InsertPerformanceReview 3, '2023-06-25', 80, 'Good project delivery';
EXEC InsertPerformanceReview 3, '2023-09-10', 83, 'Consistent performance';

EXEC InsertPerformanceReview 4, '2023-11-05', 68, 'Improving in code quality';
EXEC InsertPerformanceReview 4, '2024-01-15', 72, 'Better communication';
EXEC InsertPerformanceReview 4, '2024-03-10', 75, 'Meeting performance expectations';

EXEC InsertPerformanceReview 5, '2023-07-10', 84, 'Solid performance';
EXEC InsertPerformanceReview 5, '2023-10-22', 88, 'Great leadership';
EXEC InsertPerformanceReview 5, '2024-01-05', 90, 'Exemplary work ethic';

EXEC InsertPerformanceReview 6, '2023-08-12', 58, 'Needs more focus';
EXEC InsertPerformanceReview 6, '2023-10-15', 62, 'Showing improvement';
EXEC InsertPerformanceReview 6, '2024-01-20', 68, 'Consistent progress';

EXEC InsertPerformanceReview 7, '2023-07-25', 92, 'Exceptional work quality';
EXEC InsertPerformanceReview 7, '2023-10-10', 94, 'Outstanding problem solving';
EXEC InsertPerformanceReview 7, '2024-01-15', 95, 'Excellent performance';

EXEC InsertPerformanceReview 8, '2023-08-20', 83, 'Good collaboration skills';
EXEC InsertPerformanceReview 8, '2023-11-15', 85, 'Consistent contributions';
EXEC InsertPerformanceReview 8, '2024-01-10', 88, 'Reliable and dependable';

--TEAM B
EXEC InsertPerformanceReview 9, '2023-10-10', 90, 'Strong leadership';
EXEC InsertPerformanceReview 9, '2023-12-05', 92, 'Excellent strategic planning';
EXEC InsertPerformanceReview 9, '2024-02-10', 94, 'Great team management';

EXEC InsertPerformanceReview 10, '2023-07-20', 81, 'Solid technical skills';
EXEC InsertPerformanceReview 10, '2023-10-18', 84, 'Good coding practices';
EXEC InsertPerformanceReview 10, '2024-01-22', 86, 'Consistent performance';

EXEC InsertPerformanceReview 11, '2023-08-10', 85, 'Reliable team member';
EXEC InsertPerformanceReview 11, '2023-11-20', 87, 'Good time management';
EXEC InsertPerformanceReview 11, '2024-02-25', 89, 'Dependable and consistent';

EXEC InsertPerformanceReview 12, '2023-09-15', 80, 'Solid work ethic';
EXEC InsertPerformanceReview 12, '2023-12-10', 82, 'Good teamwork';
EXEC InsertPerformanceReview 12, '2024-03-05', 84, 'Consistently meets expectations';

--HR
EXEC InsertPerformanceReview 13, '2023-07-25', 88, 'Strong HR management';
EXEC InsertPerformanceReview 13, '2023-10-30', 90, 'Excellent employee relations';
EXEC InsertPerformanceReview 13, '2024-01-20', 92, 'Great at resolving conflicts';

EXEC InsertPerformanceReview 14, '2023-08-20', 78, 'Dependable performance';
EXEC InsertPerformanceReview 14, '2023-11-10', 80, 'Good HR support';
EXEC InsertPerformanceReview 14, '2024-01-25', 83, 'Consistent and reliable';

--SUPPORT
EXEC InsertPerformanceReview 15, '2023-09-15', 87, 'Great customer service';
EXEC InsertPerformanceReview 15, '2023-12-05', 89, 'Excellent problem resolution';
EXEC InsertPerformanceReview 15, '2024-02-28', 91, 'Outstanding support skills';

EXEC InsertPerformanceReview 16, '2023-08-10', 79, 'Good technical support';
EXEC InsertPerformanceReview 16, '2023-11-20', 82, 'Reliable troubleshooting';
EXEC InsertPerformanceReview 16, '2024-03-01', 84, 'Consistent service quality';



go

SET IDENTITY_INSERT Department.DepartmentInfo ON;
go

CREATE PROCEDURE InsertDepartmentInfo
	@DepartmentID INT,
	@DepartmentName VARCHAR(255)
AS
BEGIN 
	INSERT INTO Department.DepartmentInfo (department_id, department_name) 
	VALUES (@DepartmentID, @DepartmentName);
END;
go

EXEC InsertDepartmentInfo 1, 'HR';
EXEC InsertDepartmentInfo 2, 'Main Team A';
EXEC InsertDepartmentInfo 3, 'Secondary Team B';
EXEC InsertDepartmentInfo 4, 'Support Team';
go
 
SET IDENTITY_INSERT Department.DepartmentInfo OFF;
go

 CREATE PROCEDURE InsertEmployeeDepartmentInfo
	@EmployeeID INT,
	@DepartmentID INT
AS
BEGIN 
	INSERT INTO Department.EmployeeDepartment (employee_id, department_id) 
	VALUES (@EmployeeID, @DepartmentID);
END;
go

-- Team A in Software Development
EXEC InsertEmployeeDepartmentInfo 1, 2;
EXEC InsertEmployeeDepartmentInfo 2, 2;
EXEC InsertEmployeeDepartmentInfo 3, 2;
EXEC InsertEmployeeDepartmentInfo 4, 2;
EXEC InsertEmployeeDepartmentInfo 5, 2;
EXEC InsertEmployeeDepartmentInfo 6, 2;
EXEC InsertEmployeeDepartmentInfo 7, 2;
EXEC InsertEmployeeDepartmentInfo 8, 2;

-- Team B in Software Development
EXEC InsertEmployeeDepartmentInfo 9, 2;
EXEC InsertEmployeeDepartmentInfo 10, 2;
EXEC InsertEmployeeDepartmentInfo 11, 2;
EXEC InsertEmployeeDepartmentInfo 12, 2;

-- HR Department
EXEC InsertEmployeeDepartmentInfo 13, 1;
EXEC InsertEmployeeDepartmentInfo 14, 1;

-- Support Team
EXEC InsertEmployeeDepartmentInfo 15, 3;
EXEC InsertEmployeeDepartmentInfo 16, 3;
go
