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
    @Login VARCHAR(255),
    @Role VARCHAR(255)
AS
BEGIN
    INSERT INTO Employee.EmployeeInfo (employee_id, first_name, last_name, position, login_name, role_position, hire_date)
    VALUES (@EmployeeID, @FirstName, @LastName, @Position, @Login, @Role, @HireDate);
END;
go
 
-- TEAM A 
EXEC InsertEmployeeInfo 1, 'John', 'Doe', 'Tech Lead', '2022-01-15', 'john.doe', 'Manager';
EXEC InsertEmployeeInfo 2, 'Jane', 'Smith', 'Software Developer', '2022-03-22', 'jane.smith', 'Employee';
EXEC InsertEmployeeInfo 3, 'Alice', 'Johnson', 'Software Developer', '2022-06-10', 'alice.johnson', 'Employee';
EXEC InsertEmployeeInfo 4, 'Bob', 'Williams', 'Software Developer', '2022-08-05', 'bob.williams', 'Employee';
EXEC InsertEmployeeInfo 5, 'Charlie', 'Brown', 'Software Developer', '2022-10-18', 'charlie.brown', 'Employee';
EXEC InsertEmployeeInfo 6, 'David', 'Jones', 'Software Developer', '2023-02-14', 'david.jones', 'Employee';
EXEC InsertEmployeeInfo 7, 'Eva', 'Miller', 'Software Developer', '2023-04-30', 'eva.miller', 'Employee';
EXEC InsertEmployeeInfo 8, 'Frank', 'Davis', 'Software Developer', '2023-07-21', 'frank.davis', 'Employee';

-- TEAM B
EXEC InsertEmployeeInfo 9, 'Grace', 'Hall', 'Tech Lead', '2023-09-12', 'grace.hall', 'Manager';
EXEC InsertEmployeeInfo 10, 'Hannah', 'Moore', 'Software Developer', '2023-09-12', 'hannah.moore', 'Employee';
EXEC InsertEmployeeInfo 11, 'Isaac', 'Clark', 'Software Developer', '2024-01-15', 'isaac.clark', 'Employee';
EXEC InsertEmployeeInfo 12, 'Jack', 'Lewis', 'Software Developer', '2024-01-20', 'jack.lewis', 'Employee';

-- HR
EXEC InsertEmployeeInfo 13, 'Kate', 'Martin', 'HR Lead', '2023-05-09', 'kate.martin', 'HR';
EXEC InsertEmployeeInfo 14, 'Liam', 'Walker', 'HR', '2023-07-22', 'liam.walker', 'HR';

-- SUPPORT
EXEC InsertEmployeeInfo 15, 'Mia', 'Harris', 'Support Team Lead', '2023-06-13', 'mia.harris', 'Manager';
EXEC InsertEmployeeInfo 16, 'Noah', 'Clarkson', 'Support Team', '2023-08-25', 'noah.clarkson', 'Employee';

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
EXEC InsertPerformanceReview 1, '2023-01-15', 85, 'Good leadership skills';
EXEC InsertPerformanceReview 2, '2023-03-22', 78, 'Consistent performance';
EXEC InsertPerformanceReview 3, '2023-06-10', 82, 'Great improvement';
EXEC InsertPerformanceReview 4, '2023-08-05', 74, 'Needs improvement in code quality';
EXEC InsertPerformanceReview 5, '2023-10-18', 80, 'Solid performance';
EXEC InsertPerformanceReview 6, '2023-12-14', 54, 'Average performance';
EXEC InsertPerformanceReview 6, '2024-03-03', 24, 'Needs improving';
EXEC InsertPerformanceReview 7, '2024-04-30', 88, 'Outstanding contribution';
EXEC InsertPerformanceReview 8, '2024-07-21', 81, 'Good team player';

--TEAM B
EXEC InsertPerformanceReview 9, '2024-09-12', 89, 'Excellent leadership';
EXEC InsertPerformanceReview 10, '2024-09-12', 77, 'Good coding skills';
EXEC InsertPerformanceReview 11, '2024-01-15', 83, 'Very reliable';
EXEC InsertPerformanceReview 12, '2024-01-20', 79, 'Consistent worker';

--HR
EXEC InsertPerformanceReview 13, '2024-05-09', 87, 'Strong management';
EXEC InsertPerformanceReview 14, '2024-07-22', 75, 'Dependable';

--SUPPORT
EXEC InsertPerformanceReview 15, '2024-06-13', 86, 'Great customer service';
EXEC InsertPerformanceReview 16, '2024-08-25', 78, 'Good problem-solving skills';

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
