using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using challenge.Data;
using System.Threading;

namespace challenge.Repositories
{
    public class EmployeeRespository : IEmployeeRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<IEmployeeRepository> _logger;
        private object employee;

        public EmployeeRespository(ILogger<IEmployeeRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Employee Add(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid().ToString();
            _employeeContext.Employees.Add(employee);
            return employee;
        }

        public Employee GetById(string id)
        {
            var employee = _employeeContext.Employees.Include(e => e.DirectReports).SingleOrDefault(e => e.EmployeeId == id);
            if (employee == null)
             return null;
            LoadDirectReports(employee);
            return employee;
        }

        private void LoadDirectReports(Employee employee)
        {
            _employeeContext.Entry(employee).Collection(e => e.DirectReports).Load();

            foreach (var directReport in employee.DirectReports)
            {
                LoadDirectReports(directReport);
            }
        }
        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        public Employee Remove(Employee employee)
        {
            return _employeeContext.Remove(employee).Entity;
        }

        
    }
}
