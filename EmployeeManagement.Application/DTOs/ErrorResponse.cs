using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Application.DTOs
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
