using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Implementation = Pariveda.BI.Deployment.Library;

namespace Pariveda.BI.Deployment.Library.AcceptanceTests
{
    
    public class US00_BaselineSteps
    {
        public US00_BaselineSteps()
        {
            _context = new BIDeploymentContext();
        }

        protected BIDeploymentContext _context { get; set; }

        public void ClickDeployButton()
        {
            
            _context.Manifest = new Implementation.Manifest(_context.TicketNumber, _context.DeploymentFolder);
            _context.PreDeployTime = DateTime.Now;
            _context.Manifest.Deploy();
        }
    }
}
