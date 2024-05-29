CREATE PROCEDURE GetTop5EmployeesAverageScore

AS
BEGIN
    SET NOCOUNT ON;

	SELECT TOP 5
		employee.employee_id,
		employee.first_name,
		employee.last_name,
		AVG(pr.score) AS average_score
	FROM 
		Employee.EmployeeInfo employee
	INNER JOIN 
		Employee.PerformanceReview pr ON employee.employee_id = pr.employee_id
	GROUP BY 
		employee.employee_id, employee.first_name, employee.last_name
	ORDER BY 
		average_score DESC;
END; 
