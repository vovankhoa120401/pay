using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Pay1193.Entity;
using Pay1193.Models;
using Pay1193.Services;
using Pay1193.Services.Implement;
using System.Data;
using System.Net.NetworkInformation;

namespace Pay1193.Controllers
{
    public class PaymentController: Controller
    {
        private readonly IPayService _payService;
        private readonly IEmployee _employeeService;
        private readonly ITaxService _taxService;
        private readonly INationalInsuranceService _nationalInsuranceService;
        private decimal overtimeHrs;
        private decimal contractualEarnings;
        private decimal overtimeEarnings;
        private decimal totalEarnings;
        private decimal tax;
        private decimal unionFee;
        private decimal studentLoan;
        private decimal nationalInsurance;
        private decimal totalDeduction;

        public int Id { get; private set; }
        public int Employee { get; private set; }
        public string FullName { get; private set; }
        public string NiNo { get; private set; }
        public DateTime PayDate { get; private set; }
        public string PayMonth { get; private set; }
        public int TaxYearId { get; private set; }
        public string Year { get; private set; }
        public string TaxCode { get; private set; }
        public decimal HourlyRate { get; private set; }
        public object HoursWorked { get; private set; }
        public decimal ContractualHours { get; private set; }
        public decimal OvertimeHours { get; private set; }
        public decimal OvertimeRate { get; private set; }
        public decimal ContractualEarnings { get; private set; }
        public decimal OvertimeEarnings { get; private set; }
        public decimal Tax { get; private set; }
        public decimal NIC { get; private set; }
        public decimal UnionFee { get; private set; }
        public object SCL { get; private set; }
        public decimal TotalEarnings { get; private set; }
        public object TotalDeduction { get; private set; }
        public TaxYear TaxYear { get; private set; }
        public decimal NetPayment { get; private set; }

        public PaymentController(IPayService payService,IEmployee employeeService, ITaxService taxService, INationalInsuranceService nationalInsuranceService)
        {
            _payService = payService;
            _employeeService = employeeService;
            _taxService = taxService;
            _nationalInsuranceService = nationalInsuranceService;
        }
        public IActionResult Index()
        {
            var payRecords=_payService.GetAll().Select(pay => new PaymentRecordIndexViewModel {
                
                Id = pay.Id,
                Employee = pay.Employee,
                FullName=pay.FullName,
                PayDate=pay.PayDate,
                PayMonth=pay.PayMonth,
                TaxtYearId=pay.TaxYearId,
                Year=_payService.GetTaxYearById(pay.TaxYearId).YearOfTax,
                TotalEarnings=pay.TotalEarnings,
                TotalDeduction=pay.TotalDeduction,
                NetPayment = pay.NetPayment,

            });
            return View(payRecords);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.employees=_employeeService.GetAllEmployeesForPayroll();
            ViewBag.taxYears=_payService.GetAll();
            var model=new PaymentRecordCreateViewModel();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PaymentRecordCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var payrecord = new PaymentRecord(){

                    Id = model.Id,
                    EmployeeId = model.EmployeeId,
                    FullName = _employeeService.GetById(model.EmployeeId).FullName,
                    NiNo = _employeeService.GetById(model.EmployeeId).NationalInsuranceNo,
                    PayDate = model.PayDate,
                    PayMonth = model.PayMonth,
                    PayYearId = model.TaxYearId,
                    TaxCode = model.TaxCode,
                    HourlyRate = model.HourlyRate,
                    HoursWorked = model.HoursWorked,
                    ContractualHours = model.ContractualHours,
                    OvertimeHours = overtimeHrs = _payService.OvertimeHours(model.HoursWorked, model.ContractualHours),
                    ContratualEarnings = contractualEarnings = _payService.ContratualEarnings(model.ContractualHours, model.HoursWorked, model.HourlyRate),
                    OvertimeEarnings = overtimeEarnings = _payService.OvertimeEarnings(_payService.OvertimeRate(model.HourlyRate), overtimeHrs),
                    TotalEarnings = totalEarnings = _payService.TotalEarnings(overtimeEarnings, contractualEarnings),
                    Tax = tax = _taxService.TaxAmount(totalEarnings),
                    UnionFee = unionFee = _employeeService.UnionFees(model.EmployeeId),
                    SLC = studentLoan = _employeeService.StudentLoanRepaymentAmount(model.EmployeeId, totalEarnings),
                    NIC = nationalInsurance = _nationalInsuranceService.NIContribution(totalEarnings),
                    TotalDeduction = totalDeduction = _payService.TotalDeduction(tax, nationalInsurance, studentLoan, unionFee),
                    NetPayment = _payService.NetPay(totalEarnings, totalDeduction),
                };
                await _payService.CreateAsync(payrecord);
                return RedirectToAction('Index');

            }
            ViewBag.employees=_employeeService.GetAllEmployeesForPayroll();
            ViewBag.taxYears=_payService.GetAllTaxYears();
            return View(model);
        }
     public IActionResult Detail(int id)
        {
            var paymentRecord=_payService.GetById(id);
            if (paymentRecord == null)
            {
                return NotFound();
            }
            var model = new PaymentRecordDetailViewModel()
            {
                Id = paymentRecord.Id,
                Employee = paymentRecord.EmployeeId,
                FullName = paymentRecord.FullName,
                NiNo = paymentRecord.NiNo,
                PayDate = paymentRecord.PayDate,
                PayMonth = paymentRecord.PayMonth,
                TaxYearId = paymentRecord.TaxYearId,
                Year = _payService.GetTaxYearById(paymentRecord.TaxYearId).YearOfTax,
                TaxCode = paymentRecord.TaxCode,
                HourlyRate = paymentRecord.HourlyRate,
                HoursWorked = paymentRecord.HoursWorked,
                ContractualHours = paymentRecord.ContractualHours,
                OvertimeHours = paymentRecord.OvertimeHours,
                OvertimeRate = _payService.OvertimeRate(paymentRecord.OvertimeRate),
                ContractualEarnings = paymentRecord.ContractualEarnings,
                OvertimeEarnings = paymentRecord.OvertimeEarnings,
                Tax = paymentRecord.Tax,
                NIC = paymentRecord.NIC,
                UnionFee = paymentRecord.UnionFee,
                SCL = paymentRecord.SCL,
                TotalEarnings = paymentRecord.TotalEarnings,
                TotalDeduction = paymentRecord.TotalDeduction,
                TaxYear = paymentRecord.TaxYear,
                NetPayment = paymentRecord.NetPayment,

            };
            return View(model);
        }
}
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Payslip(int id)
    {
        var paymentRecord = _payService.GetById(id);
        if (paymentRecord == null)
            {
                return NotFound();
            }
            var model=new PaymentRecordDetailViewModel()
            { 
                Id=paymentRecord.Id,
                Employee=paymentRecord.EmployeeId,
                FullName=paymentRecord.FullName,
                NiNo=paymentRecord.NiNo,
                PayDate=paymentRecord.PayDate,
                PayMonth=paymentRecord.PayMonth,
                TaxYearId=paymentRecord.TaxYearId,
                Year=_payService.GetTaxYearById(paymentRecord.TaxYearId).YearOfTax,
                TaxCode=paymentRecord.TaxCode,
                HourlyRate=paymentRecord.HourlyRate,
                HoursWorked=paymentRecord.HoursWorked,
                ContractualHours=paymentRecord.ContractualHours,
                OvertimeHours=paymentRecord.OvertimeHours,
                OvertimeRate=_payService.OvertimeRate(paymentRecord.OvertimeRate),
                ContractualEarnings=paymentRecord.ContractualEarnings,
                OvertimeEarnings=paymentRecord.OvertimeEarnings,
                Tax=paymentRecord.Tax,
                NIC=paymentRecord.NIC,
                UnionFee=paymentRecord.UnionFee,  
                SCL=paymentRecord.SCL,
                TotalEarnings= paymentRecord.TotalEarnings,
                TotalDeduction=paymentRecord.TotalDeduction,
                Employee=paymentRecord.Employee,
                TaxYear=paymentRecord.TaxYear,
                NetPayment=paymentRecord.NetPayment,

            };
            return View(model);
        }
}