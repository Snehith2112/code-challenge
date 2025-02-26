﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using challenge.Repositories;

namespace challenge.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(ILogger<EmployeeService> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public Employee Create(Employee employee)
        {
            if(employee != null)
            {
                _employeeRepository.Add(employee);
                _employeeRepository.SaveAsync().Wait();
            }

            return employee;
        }

        public Employee GetById(string id)
        {
            if(!String.IsNullOrEmpty(id))
            {
                return _employeeRepository.GetById(id);
            }

            return null;
        }

        public ReportingStructure GetReport(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                int numberOfReports = CalculateReport(id);
                var employee = _employeeRepository.GetById(id);
                if (employee != null)
                {
                    return new ReportingStructure
                    {
                        Employee = employee,
                        numberOfReports = numberOfReports
                    };

                };
            }

            return null;
        }
        private int CalculateReport(string id)
        {
            var employee = _employeeRepository.GetById(id);
            if (employee?.DirectReports.Count == null || !employee.DirectReports.Any())

            {
                return 0;
            }
            return
                employee.DirectReports.Sum(directreport => 1 + CalculateReport(directreport.EmployeeId));
        }

            public Employee Replace(Employee originalEmployee, Employee newEmployee)
        {
            if(originalEmployee != null)
            {
                _employeeRepository.Remove(originalEmployee);
                if (newEmployee != null)
                {
                    // ensure the original has been removed, otherwise EF will complain another entity w/ same id already exists
                    _employeeRepository.SaveAsync().Wait();

                    _employeeRepository.Add(newEmployee);
                    // overwrite the new id with previous employee id
                    newEmployee.EmployeeId = originalEmployee.EmployeeId;
                }
                _employeeRepository.SaveAsync().Wait();
            }

            return newEmployee;
        }
    }
}
