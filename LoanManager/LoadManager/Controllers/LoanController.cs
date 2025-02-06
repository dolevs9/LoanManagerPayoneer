using LoadManager.ClientModels;
using LoadManager.Logic;
using LoadManager.Repositories;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace LoadManager.Controllers
{
    [ApiController]
    [Route("api/loan")]
    [EnableCors("AllowReact")]
    public class LoanController : ControllerBase
    {
        private readonly ILogger<LoanController> _logger;
        ILoanCalculator _loanCalculator;
        ILoansRepository _loanRepository;
        IDataContext _dataContext;

        public LoanController(ILogger<LoanController> logger, 
            ILoanCalculator loanCalculator, 
            ILoansRepository loanRepository,
            IDataContext dataContext)
        {
            _logger = logger;
            _loanCalculator = loanCalculator;
            _loanRepository = loanRepository;
            _dataContext = dataContext;
        }

        
        
        /// <summary>
        /// Dont take a loan, just calculate interest
        /// </summary>
        /// <param name="loanData"></param>
        /// <returns>Interest result calculation for the loan</returns>
        [HttpPost("CalculateLoanRefund")]
        public double CalculateLoanRefund([FromBody] LoanRequestData loanData)
        {
            var result = _loanCalculator.CalculateLoanInterest(loanData.id, loanData.dept, loanData.months);
            return result;
        }

        /// <summary>
        /// Sets a loan taken be the user
        /// </summary>
        /// <param name="loanData"></param>
        /// <returns>Interest value calculation</returns>
        [HttpPost("SetLoan")]
        public double SetLoan([FromBody] LoanRequestData loanData)
        {
            var result = _loanCalculator.CalculateLoanInterest(loanData.id, loanData.dept, loanData.months);
            _loanRepository.Insert(new Models.Loan()
            {
                DurationInMonths = loanData.months,
                BaseLoanDebt = loanData.dept,
                UserId = loanData.id,
                LoanStartDate = DateTime.Today,
                RefundDeptPrice = result,
                LoanId = Guid.NewGuid()
            });

            _dataContext.SaveChanges();
            return result;
        }
    }
}
