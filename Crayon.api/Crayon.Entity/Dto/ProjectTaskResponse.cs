using System;
using System.Collections.Generic;
using System.Text;

namespace Crayon.Entity.Dto
{
    public class ProjectTaskResponse
    {
        public int ProjectTaskId { get; set; }

        public int ProjectId { get; set; }

        public string ProjectName { get; set; }

        public string TaskName { get; set; }
    }
}
