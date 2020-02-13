using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastWorkLight.Models
{
    public class Job
    {
        public string Entity { get; set; }
        public string Manage { get; set; }
        public string Pay { get; set; }
        public List<Job> Jobs { get; set; }
    }
}
