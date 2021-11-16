using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWASM_CRUD_B2C.Service
{
    public class ServiceResponse<T>
    {
        public T Response { get; set; }

        public bool Success { get; set; } = true;
        public string Message { get; set; }
    }
}
