CREATE PROCEDURE GetEmployeeDueForReview
	@MonthCutOff INT
AS
BEGIN
    SET NOCOUNT ON;

	DECLARE @CutOff DATE;
	SET @CutOff = DATEADD(MONTH, -@MonthCutOff, GETDATE());

    SELECT 
        employee.employee_id,
        employee.first_name,
        employee.last_name,
        employee.position,
        employee.hire_date,
        MAX(pr.review_date) AS last_review_date
    FROM 
        Employee.EmployeeInfo employee
    LEFT JOIN 
        Employee.PerformanceReview pr
    ON 
        employee.employee_id = pr.employee_id
    GROUP BY 
        employee.employee_id, employee.first_name, employee.last_name, employee.position, employee.hire_date
    HAVING 
        MAX(pr.review_date) IS NULL OR MAX(pr.review_date) <= @CutOff
    ORDER BY 
        last_review_date ASC;
END; 
