CREATE ROLE HR;
CREATE ROLE Manager;
CREATE ROLE Employee;
go

GRANT SELECT, INSERT, UPDATE, DELETE ON Employee.EmployeeInfo TO HR;
GRANT SELECT, INSERT, UPDATE, DELETE ON Employee.PerformanceReview TO HR;
GRANT SELECT, INSERT, UPDATE, DELETE ON Department.EmployeeDepartment TO HR;

GRANT SELECT, INSERT, UPDATE ON Employee.EmployeeInfo TO Manager;
GRANT SELECT, INSERT, UPDATE ON  Employee.PerformanceReview TO Manager;

GRANT SELECT ON Employee.EmployeeInfo TO Employee;
go

