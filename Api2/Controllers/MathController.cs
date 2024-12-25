using Microsoft.AspNetCore.Mvc;

namespace Api2.Controllers
{
    using Api2.Model;
    using Api2.Services;
    using Api2.Utilities;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [ApiController]
    [Route("api/[controller]")]
    public class MathController : ControllerBase
    {
        private readonly MathService _mathService;
        private readonly HeaderValidator _headerValidator;

        public MathController(IConfiguration configuration)
        {
            _mathService = new MathService();
            _headerValidator = new HeaderValidator(configuration);
        }

        private IActionResult ValidateHeaders()
        {
            if (!_headerValidator.ValidateHeaders(Request.Headers, out var errorMessage))
            {
                return BadRequest(new OperationResponse
                {
                    Message = errorMessage,
                    StatusCode = 400
                });
            }

            return null;
        }

        [HttpPost("add")]
        public IActionResult Add([FromQuery] string @params)
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return BadRequest(new OperationResponse
                {
                    Message = "Authorization header is missing.",
                    StatusCode = 400,
                    User = "info"
                });
            }

            // Parametre kontrolü
            if (string.IsNullOrWhiteSpace(@params))
            {
                return BadRequest(new OperationResponse
                {
                    Message = "The params field is required.",
                    StatusCode = 400,
                    User = "info"
                });
            }

            try
            {
                // Parametre çözme ve toplama
                var numbers = ParseParams(@params);
                var result = _mathService.Add(numbers);

                // Başarılı işlem yanıtı
                return Ok(new OperationResponse
                {
                    Result = result.Result,
                    Message = "Success",
                    StatusCode = 200,
                    User = "info"
                });
            }
            catch (ArgumentException ex)
            {
                // Hatalı parametre formatı
                return BadRequest(new OperationResponse
                {
                    Message = ex.Message,
                    StatusCode = 400,
                    User = "info"
                });
            }
            catch (Exception ex)
            {
                // Beklenmeyen hata durumu
                return StatusCode(500, new OperationResponse
                {
                    Message = "An unexpected error occurred.",
                    StatusCode = 500,
                    User = "info"
                });
            }
        }



        [HttpPost("subtraction")]
        public IActionResult Subtract([FromQuery] string @params)
        {
            var validation = ValidateHeaders();
            if (validation != null) return validation;

            var numbers = ParseParams(@params);
            var result = _mathService.Subtract(numbers);

            return Ok(new OperationResponse
            {
                Result = result.Result,
                Message = result.Message,
                StatusCode = result.StatusCode,
                User = "info"
            });
        }

        [HttpPost("multiplication")]
        public IActionResult Multiply([FromQuery] string @params)
        {
            var validation = ValidateHeaders();
            if (validation != null) return validation;

            var numbers = ParseParams(@params);
            var result = _mathService.Multiply(numbers);

            if (result.StatusCode != 200)
            {
                return BadRequest(new OperationResponse
                {
                    Message = result.Message,
                    StatusCode = result.StatusCode
                });
            }
            return Ok(new OperationResponse
            {
                Result = result.Result,
                Message = result.Message,
                StatusCode = result.StatusCode,
                User = "info"
            });
        }

        [HttpPost("division")]
        public IActionResult Divide([FromQuery] string @params)
        {
            var validation = ValidateHeaders();
            if (validation != null) return validation;

            var numbers = ParseParams(@params);
            var result = _mathService.Divide(numbers);

            if (result.StatusCode != 200)
            {
                return BadRequest(new OperationResponse
                {
                    Message = result.Message,
                    StatusCode = result.StatusCode
                });
            }

            return Ok(new OperationResponse
            {
                Result = result.Result,
                Message = result.Message,
                StatusCode = result.StatusCode,
                User = "info"
            });
        }

        [HttpGet("sum")]
        public IActionResult Sum([FromQuery] int @params)
        {
            var validation = ValidateHeaders();
            if (validation != null) return validation;

            var result = _mathService.SumToN(@params);

            if (result.StatusCode != 200)
            {
                return BadRequest(new OperationResponse
                {
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    User = "null"
                });
            }

            return Ok(new OperationResponse
            {
                Result = result.Result,
                Message = result.Message,
                StatusCode = result.StatusCode,
                User = "info"
            });
        }

        private List<int> ParseParams(string @params)
        {
            if (string.IsNullOrWhiteSpace(@params))
            {
                throw new ArgumentException("Parameters cannot be null, empty, or whitespace.");
            }

            try
            {
                return @params.Split(',').Select(int.Parse).ToList();
            }
            catch (FormatException)
            {
                throw new ArgumentException("Invalid parameter format. Ensure all parameters are integers.");
            }
        }
    }
}
