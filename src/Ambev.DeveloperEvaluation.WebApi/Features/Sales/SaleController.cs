using Ambev.DeveloperEvaluation.WebApi.Common;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales
{
    public class SaleController : BaseController
    {
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
