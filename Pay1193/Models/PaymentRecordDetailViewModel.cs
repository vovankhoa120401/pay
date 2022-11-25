using Pay1193.Entity;
using System.ComponentModel.DataAnnotations;
using System.Xml.Ling;

namespace Pay1193.Models
{
    public class PaymentRecordDetailViewModel
    {
    public int Id { get; set; }
        public int Employee { get; internal set; }
        public string FullName { get; internal set; }
        public string NiNo { get; internal set; }
        public DateTime PayDate { get; internal set; }
        public string PayMonth { get; internal set; }
        public int TaxYearId { get; internal set; }
        public string Year { get; internal set; }
        public string TaxCode { get; internal set; }
        public decimal HourlyRate { get; internal set; }
        public object HoursWorked { get; internal set; }
        public decimal ContractualHours { get; internal set; }
        public decimal OvertimeHours { get; internal set; }
        public decimal OvertimeRate { get; internal set; }
        public decimal ContractualEarnings { get; internal set; }
        public decimal OvertimeEarnings { get; internal set; }
        public decimal Tax { get; internal set; }
        public decimal NIC { get; internal set; }
        public decimal UnionFee { get; internal set; }
        public object SCL { get; internal set; }
        public decimal TotalEarnings { get; internal set; }
        public object TotalDeduction { get; internal set; }
        public TaxYear TaxYear { get; internal set; }
        public decimal NetPayment { get; internal set; }

        [Display(public static implicit operator PaymentRecordDetailViewModel(PaymentRecordDetailViewModel v)
        {
            throw new NotImplementedException();
        }
    }

    namespace="Full Name")]
    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }
    public string FullName { get; set; }
    public string NiNo { get; set; }
    [DataType(DateType.Date), Display(Name = "Pay Date")]
    public DateTime PayDate { get; set; }
    [Display(Name = "Pay Month")]
    public string PayMonth { get; set; } 
    [Display(Name = "Tax Year")]
    public int TaxYearId { get; set; }
    public string Year { get; set }
    public string TaxYear { get; set; }
    [Display(Name = "Tax Code")]
    public string TaxCode { get; set; }
    [Display(Name = "Hourly Rate")]
    public decimal HourlyRate { get; set; }
    [Display(Name = "Hours Worked")]
    public decimal HoursWorked { get; set; }
    [Display(Name = "Contractual Hours")]
    public decimal ContractualHours { get; set; }
    [Display(Name = "Overtime Hours")]
    public decimal OvertimeHours { get; set; }
    [Display(Name = "Overtime Rate")]
    public decimal OvertimeRate { get; set; }
    [Display(Name = "Contractual Earnings")]
    public decimal ContractualEarnings { get; set; }
    [Display(Name = "Overtime Earnings")]
    public decimal OvertimeEarnings { get; set; }
    public decimal Tax { get; set; }
    public decimal NIC { get; set; }
    [Display(Name = "Union Fee")]
    public decimal? UnionFee { get; set; }
    public Nullable<decimal> SLC { get; set; }
    [Display(Name = "Total Earnings")]
    public decimal TotalEarnings { get; set; }
    [Display(Name = "Total Deductions")]
    public decimal TotalDeduction { get; set; }
    [Display(Name = "Net Payment")]
    public decimal NetPayment { get; set; }

    }

}
