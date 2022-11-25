﻿using Pay1193.Entity;
using System.ComponentModel.DateAnnotations;
using System.Xml.Ling;

namespace Pay1193.Models
{
    public class PaymentRecordIndexViewModel
    {
        internal object TotalDeduction;

        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        [Display(Name="Name")]
        public string FullName { get; set; }
        [Display(Name = "Pay Date")]
        public DateTime PayDate { get; set; }
        [Display(Name = "Month")]
        public string PayMonth { get; set; }
        public int TaxtYearId { get; set; }
        public string Year { get; set; }
        [Display(Name = "Total Earnings")]
        public decimal TotalEarnings { get; set; }
        [Display(Name = "Total Deductions")]
        public decimal TotalDeductions { get; set; }
        [Display(Name = "Net")]
        public decimal NetPayment { get; set; }
    }
}