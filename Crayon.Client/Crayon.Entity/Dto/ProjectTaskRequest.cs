using System;
using System.Collections.Generic;
using System.Text;

namespace Crayon.Entity.Dto
{
    public class ProjectTaskRequest
    {
        public int ProjectId { get; set; }

        public string TaskName { get; set; }
    }
}
