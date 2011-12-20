using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Pariveda.BI.Deployment.Library
{

    public interface IBIDeployable
    {
        string DeployInfo();
        bool Deploy(DirectoryInfo deploymentDirectory);
    }
}
