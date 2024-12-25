namespace Api2.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Api2.Model;

    public class MathService
    {
        public OperationResponse Add(List<int> numbers)
        {
            if (numbers == null || numbers.Count == 0)
            {
                return new OperationResponse
                {
                    Message = "Parameters cannot be null, empty, or whitespace.",
                    StatusCode = 400
                };
            }

            return new OperationResponse
            {
                Result = numbers.Sum(),
                Message = "Success",
                StatusCode = 200
            };
        }

        public OperationResponse Subtract(List<int> numbers)
        {
            if (numbers == null || numbers.Count == 0)
                return new OperationResponse
                {
                    Message = "The numbers list cannot be null or empty.",
                    StatusCode = 400
                };

            if (numbers.Count > 5)
                return new OperationResponse
                {
                    Message = "The numbers list cannot contain more than 5 elements.",
                    StatusCode = 400
                };

            return new OperationResponse
            {
                Result = numbers.Aggregate((a, b) => a - b),
                Message = "Success",
                StatusCode = 200
            };
        }

        public OperationResponse Multiply(List<int> numbers)
        {
            if (numbers == null || numbers.Count == 0)
                return new OperationResponse
                {
                    Message = "The numbers list cannot be null or empty.",
                    StatusCode = 400
                };

            if (numbers.Count > 5)
                return new OperationResponse
                {
                    Message = "The numbers list cannot contain more than 5 elements.",
                    StatusCode = 400
                };

            if (numbers.Any(n => n < 0))
                return new OperationResponse
                {
                    Message = "The numbers list cannot contain negative values.",
                    StatusCode = 400
                };

            return new OperationResponse
            {
                Result = numbers.Aggregate((a, b) => a * b),
                Message = "Success",
                StatusCode = 200
            };
        }

        public OperationResponse Divide(List<int> numbers)
        {
            if (numbers == null || numbers.Count != 2)
            {
                return new OperationResponse
                {
                    Message = "Division requires exactly 2 parameters.",
                    StatusCode = 400
                };
            }

            if (numbers[1] == 0)
            {
                return new OperationResponse
                {
                    Message = "Division by zero is not allowed.",
                    StatusCode = 400
                };
            }

            return new OperationResponse
            {
                Result = numbers[0] / numbers[1],
                Message = "Success",
                StatusCode = 200
            };
        }

        public OperationResponse SumToN(int n)
        {
            if (n < 1)
                return new OperationResponse
                {
                    Message = "The parameter must be greater than or equal to 1.",
                    StatusCode = 400
                };

            return new OperationResponse
            {
                Result = Enumerable.Range(1, n).Sum(),
                Message = "Success",
                StatusCode = 200
            };
        }
    }
}
