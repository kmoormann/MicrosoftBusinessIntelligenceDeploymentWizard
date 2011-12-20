using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pariveda.BI.Deployment.Library.Enums;

namespace Pariveda.BI.Deployment.Library
{
    public class SQLItem : BusinessIntelligenceItem
    {
        public ServerTypes DeploymentServer {get; set;}

        public SQLItem(string fileName)
            : base(fileName)
        {
            SetItemType();
            InitializeDeploymentServer();
            
        }

        private void SetItemType()
        {
            _biItemType = BusinessIntelligenceItemType.SQLFile;
        }

        private void InitializeDeploymentServer()
        {
            DeploymentServer = ServerTypes.Unknown;
        }

        public SQLItem(string fileName, bool shouldDeploy)
            : base(fileName, shouldDeploy)
        {
            SetItemType();
            InitializeDeploymentServer();
        }
    }
}
