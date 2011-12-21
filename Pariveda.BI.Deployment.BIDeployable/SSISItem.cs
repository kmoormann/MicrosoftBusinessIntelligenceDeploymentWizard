using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pariveda.BI.Deployment.Library.Enums;

namespace Pariveda.BI.Deployment.Library
{


    public class SSISItem : BusinessIntelligenceItem
    {
        private string _path = string.Empty;
        
        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
            }
        }

        public SSISItem(string fileName)
            : base(fileName)
        {
            SetItemType();
        }

        public SSISItem(string fileName, bool shouldDeploy)
            : base(fileName, shouldDeploy)
        {
            SetItemType();
        }

        private void SetItemType()
        {
            _biItemType = BusinessIntelligenceItemType.SSISPackage;
        }


    }
}
