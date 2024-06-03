using EmployeePerformanceApp.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace EmployeePerformanceApp.Data
{
    public class EmployeePerformanceDbContext : DbContext
    {
        public EmployeePerformanceDbContext(DbContextOptions<EmployeePerformanceDbContext> options)
            : base(options)
        {
        }

        public DbSet<EmployeeInfo> EmployeeInfos { get; set; }
        public DbSet<PerformanceReviews> PerformanceReviews { get; set; }
        public DbSet<EmployeeDepartments> EmployeeDepartments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EmployeeInfo>(entity =>
            {
                entity.ToTable("EmployeeInfo", "Employee");
                entity.HasKey(e => e.employee_id);
            });

            modelBuilder.Entity<PerformanceReviews>(entity =>
            {
                entity.ToTable("PerformanceReview", "Employee");
                entity.HasKey(e => e.review_id);
            });

            modelBuilder.Entity<EmployeeDepartments>(entity =>
            {
                entity.ToTable("EmployeeDepartment", "Department");
                entity.HasKey(e => e.connection_id);
            });
        }

        public async Task ExecuteStoredProcedureAsync(string procedureName, params SqlParameter[] parameters)
        {
            var sql = $"EXEC {procedureName}";

            if (parameters != null && parameters.Length > 0)
            {
                sql += " " + string.Join(", ", parameters.Select(p => p.ParameterName));
            }

            try
            {
                await Database.ExecuteSqlRawAsync(sql, parameters);
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException($"Error executing stored procedure {procedureName}", ex);
            }
        }
    }
}