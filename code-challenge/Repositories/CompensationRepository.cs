using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using challenge.Data;

namespace challenge.Repositories
{
    public class CompensationRespository : ICompensationRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<ICompensationRepository> _logger;

        public CompensationRespository(ILogger<ICompensationRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Compensation Add(Compensation compensation)
        { 
            var employeeExists =_employeeContext.Employees.SingleOrDefault(e => e.EmployeeId == compensation.EmployeeId) != null;
            return employeeExists ?_employeeContext.Add(compensation).Entity : null;
        }

        public Compensation GetById(string id) => _employeeContext.Compensations.SingleOrDefault(c => c.EmployeeId == id);
        
        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        public Compensation Remove(Compensation compensation)
        {
            return _employeeContext.Remove(compensation).Entity;
        }


    }
}