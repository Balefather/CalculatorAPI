using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CalculatorAPI.Controllers
{
    [EnableCors("AllowAllOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class CalculatorController : Controller
    {
        //Default endpoint
        [HttpGet("Addition")]
        public ActionResult<double> Addition(double a, double b)
        {
            double result = a + b;
            return Ok(result);
        }

        //Shortform endpoint by using lambda expression
        [HttpGet("Subtraction")]
        public ActionResult<double> Subtraction(double a, double b) => Ok(a - b);


        [HttpGet("Multiplication")]
        public ActionResult<double> Multiplication(double a, double b)
        {
            double result = a * b;
            if (double.IsInfinity(result)) // Return proper error on enormous number instead of error 500
            {
                return BadRequest("Result exceeds the range of representable values.");
            }
            return Ok(result);
        }

        //If we'd want to implement scientific notation
        [HttpGet("MultiplicationScientific")]
        public ActionResult<string> MultiplicationScientific(double a, double b)
        {
            double result = a * b;
            string resultString = result.ToString("0.###E+0");
            return Ok(resultString);
        }

        [HttpGet("Division")]
        public ActionResult<double> Division(double a, double b)
        {
            if (a == 0 || b == 0) //Mandatory check for divide by zero errors, or the known universe might implode
            {
                return BadRequest("Unable to divide by 0");
            }
            double result = a / b;
            return Ok(result);
        }

        [HttpGet]
        [Route("HandleExpression")]
        public ActionResult CalculateExpression(string expression)
        {
            try
            {
                // Using DataTable to compute expression
                DataTable dt = new DataTable();
                var result = dt.Compute(expression, "");

                return Ok(result.ToString().Replace(',', '.'));
            }
            catch (Exception ex)
            {
                return BadRequest("Invalid expression. " + ex.Message);
            }
        }
    }
}
