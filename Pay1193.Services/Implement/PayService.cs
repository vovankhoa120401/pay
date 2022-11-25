﻿using Pay1193.Entity;
using Pay1193.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Pay1193.Services.Implement
{
    public class PayService : IPayService
    {
        private decimal overTimeHours;
        private decimal contractualEarnings;
        private readonly ApplicationDbContext _context;
        public PayService(ApplicationDbContext context)
        {
            _context = context;
        }
        public decimal ContractualEarning(decimal contractualHours, decimal hoursWorked, decimal hourlyRate)
        {
            if(hoursWorked < contractualHours)
            {
                contractualEarnings = hoursWorked * hourlyRate;

            }
            else
            {
                contractualEarnings = contractualHours * hourlyRate;
            }
            return contractualEarnings;
        }

        public async Task CreateAsync(PaymentRecord paymentRecord)
        {
            await _context.PaymentRecords.AddAsync(paymentRecord);
            await _context.SaveChangesAsync();
           
        }
        public IEnumerable<PaymentRecord> GetAll()=> _context.PaymentRecords.OrderBy(p => p.EmployeeId).ToList();
        public IEnumerable<SelectListItem> GetAllTaxYear()
        {
            var allTaxYear=_context.TaxYears.Select(taxYears => new SelectListItem {
                
                Text=taxYears.YearOfTax,
                Value=taxYears.Id.ToString()
            });
            return allTaxYear;
        }

        public PaymentRecord GetById(int id)=> _context.PaymentRecords.Where(pay => pay.Id==id).FirstOrDefault();

    

        public decimal NetPay(decimal totalEarnings, decimal totalDeduction)
        {
            return totalEarnings - totalDeduction;
        }

        public decimal OvertimeEarnings(decimal overtimeRate, decimal overTimeHours)
        {
            return overTimeHours * overtimeRate;
        }
       
        public decimal OverTimeHours(decimal hoursWorked, decimal contractualHours)
        {
            if(hoursWorked <= contractualHours)
            {
                overTimeHours = 0.00m;
            }
            else
            {
                overTimeHours = hoursWorked - contractualHours;
            }
            return overTimeHours;
        }

        public decimal OvertimeRate(decimal hourlyRate)=> hourlyRate *1.5m;
        

        public decimal TotalDeduction(decimal tax, decimal nic, decimal studentLoanRepayment, decimal unionFees)
        {
            return tax + nic + studentLoanRepayment + unionFees;
        }
        public decimal TotalEarnings(decimal overtimeEarnings, decimal contractualEarnings)
        => overtimeEarnings + contractualEarnings;

        public TaxYear GetTaxYearById(int id)
        => _context.TaxYears.Where(year => year.Id==id).FirstOrDefault();

        public decimal OvertimeHours(object hoursWorked, decimal contractualHours)
        {
            throw new NotImplementedException();
        }

        public decimal ContratualEarnings(decimal contractualHours, object hoursWorked, decimal hourlyRate)
        {
            throw new NotImplementedException();
        }

        public dynamic GetAllTaxYears()
        {
            throw new NotImplementedException();
        }
    }
}
