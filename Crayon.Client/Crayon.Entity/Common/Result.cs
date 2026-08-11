using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Crayon.Entity.Common
{
    public class Result<T>
    {
        public T Response { get; set; }
        public List<Errors> Error { get; set; } = new List<Errors>();
        public bool isError => Error != null && Error.Any();
    }
}
