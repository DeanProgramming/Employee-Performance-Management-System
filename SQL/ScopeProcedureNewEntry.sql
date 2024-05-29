CREATE PROCEDURE AddNewEmployee
    @FirstName VARCHAR(255),
    @LastName VARCHAR(255),
    @Position VARCHAR(255),
    @HireDate DATE,
    @DepartmentID INT,
    @LoginName VARCHAR(255),
	@RoleName VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    -- Insert the new employee
    INSERT INTO Employee.EmployeeInfo (first_name, last_name, position, hire_date)
    VALUES (@FirstName, @LastName, @Position, @HireDate);

    -- Declare variable to hold the newly inserted employee ID
    DECLARE @NewEmployeeID INT;

    -- Retrieve the newly inserted employee ID
    SELECT @NewEmployeeID = SCOPE_IDENTITY();

    -- Insert the entry in the EmployeeDepartment table
    INSERT INTO Department.EmployeeDepartment (employee_id, department_id)
    VALUES (@NewEmployeeID, @DepartmentID);

    -- Return the newly inserted employee ID
    SELECT @NewEmployeeID AS NewEmployeeID;
	
    EXEC sp_addrolemember @RoleName, @LoginName;
END;