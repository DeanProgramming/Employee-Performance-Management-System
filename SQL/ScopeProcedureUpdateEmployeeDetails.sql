CREATE PROCEDURE UpdateEmployeeDetails
	@EmployeeID INT,
    @FirstName VARCHAR(255),
    @LastName VARCHAR(255),
    @Position VARCHAR(255),
    @HireDate DATE,
    @DepartmentID INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Update employee details in Employee.EmployeeInfo table
        UPDATE Employee.EmployeeInfo
        SET first_name = @FirstName,
            last_name = @LastName,
            position = @Position,
            hire_date = @HireDate
        WHERE employee_id = @EmployeeID;

        -- Update department ID in Department.EmployeeDepartment table
        UPDATE Department.EmployeeDepartment
        SET department_id = @DepartmentID
        WHERE employee_id = @EmployeeID;
        
        -- Check if any rows were affected
        IF @@ROWCOUNT = 0
        BEGIN
            RAISERROR ('Employee not found or details unchanged.', 16, 1);
        END
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();

        -- Rollback the transaction if one is active
        IF @@TRANCOUNT > 0
            ROLLBACK;

        -- Raise the error
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH;
END;