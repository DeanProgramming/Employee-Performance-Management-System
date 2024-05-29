CREATE SCHEMA Employee;
go

CREATE SCHEMA Department;
go

-- Create EmployeeInfo Table
CREATE TABLE Employee.EmployeeInfo (
    employee_id INT IDENTITY (1,1) PRIMARY KEY,
    first_name VARCHAR(255) NOT NULL,
    last_name VARCHAR(255) NOT NULL,
    position VARCHAR(255) NOT NULL,
    login_name VARCHAR(255) NOT NULL,
    role_position VARCHAR(255) NOT NULL,
    hire_date DATE NOT NULL
);

-- Create PerformanceReview Table
CREATE TABLE Employee.PerformanceReview (
    review_id INT IDENTITY (1,1) PRIMARY KEY,
    employee_id INT NOT NULL,
    review_date DATE NOT NULL,
    score INT NOT NULL,
    comments VARCHAR(255),
    CONSTRAINT FK_Employee_PerformanceReview FOREIGN KEY (employee_id) 
        REFERENCES Employee.EmployeeInfo(employee_id) ON DELETE CASCADE
);

-- Create DepartmentInfo Table
CREATE TABLE Department.DepartmentInfo (
    department_id INT IDENTITY (1,1) PRIMARY KEY, 
    department_name VARCHAR(255) NOT NULL
);

-- Create EmployeeDepartment Table
CREATE TABLE Department.EmployeeDepartment (
    connection_id INT IDENTITY (1,1) PRIMARY KEY,
    employee_id INT NOT NULL,
    department_id INT NOT NULL,
    CONSTRAINT FK_Employee_EmployeeDepartment FOREIGN KEY (employee_id) 
        REFERENCES Employee.EmployeeInfo(employee_id) ON DELETE CASCADE,
    CONSTRAINT FK_Department_EmployeeDepartment FOREIGN KEY (department_id) 
        REFERENCES Department.DepartmentInfo(department_id) ON DELETE CASCADE
);
