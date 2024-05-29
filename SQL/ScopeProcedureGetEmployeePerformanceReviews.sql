CREATE PROCEDURE GetEmployeePerformanceReview
	@EmployeeID INT
AS
BEGIN
    SET NOCOUNT ON;

	SELECT * from Employee.PerformanceReview
	WHERE employee_id = @EmployeeID; 
END;