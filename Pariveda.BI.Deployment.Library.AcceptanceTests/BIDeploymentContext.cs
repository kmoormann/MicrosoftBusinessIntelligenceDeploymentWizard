using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Implementation = Pariveda.BI.Deployment.Library;
using System.IO;

namespace Pariveda.BI.Deployment.Library.AcceptanceTests
{

    public class BIDeploymentContext
    {
        public uint TicketNumber { get; set; }
        public Implementation.Manifest Manifest { get; set;}
        public DirectoryInfo DeploymentFolder {get; set;}
        public DateTime PreDeployTime {get; set;}
        public IEnumerable<BI.Deployment.BusinessIntelligenceItem> ContextDeployables { get; set; }
        public FileInfo DeploymentVariablesPS { get; set; }
    }
}
