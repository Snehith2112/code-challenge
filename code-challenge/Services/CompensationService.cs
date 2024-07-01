using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using challenge.Repositories;

namespace challenge.Services
{
    public class CompensationService : ICompensationService
    {
        private readonly ICompensationRepository _compensationRepository;
        private readonly IEmployeeRepository _employeeRepository;

        private readonly ILogger<CompensationService> _logger;

        public CompensationService(ILogger<CompensationService> logger, ICompensationRepository compensationRepository, IEmployeeRepository employeeRepository)
        {
            _compensationRepository = compensationRepository;
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public Compensation Create(Compensation compensation)
        {
            if (compensation == null)
            {
                return null;
            }
           var created = _compensationRepository.Add(compensation);
            _compensationRepository.SaveAsync().Wait();
            return created;
        }

        public Compensation GetById(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return null;
            }
             var compensation = _compensationRepository.GetById(id);
             if (compensation == null)
            {
                return null;
            }
             compensation.Employee =  _employeeRepository.GetById(compensation.EmployeeId);
                return compensation;
        }
    }
}