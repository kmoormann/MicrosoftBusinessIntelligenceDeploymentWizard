using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO;
using Pariveda.BI.Deployment.Library;
using Pariveda.BI.Deployment.Library.Enums;

namespace Pariveda.BI.Deployment
{
    public class BusinessIntelligenceItem : IBIDeployable
    {
        protected BusinessIntelligenceItemType _biItemType;
        protected virtual FileInfo _file { get; set; }
        protected virtual bool _shouldDeploy { get; set; }
        protected virtual uint sortOrder { get; set; }

        public BusinessIntelligenceItemType BIItemType
        {
            get
            {
                return _biItemType;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }

        [DisplayName("Deploy")]
        public bool ShouldDeploy
        {
            get { return _shouldDeploy; }
            set
            {
                _shouldDeploy = value;
                NotifyPropertyChanged("ShouldDeploy");
            }
        }

        [DisplayName("File Name")]
        [ReadOnly(true)]
        public string FileName
        {
            get { return _file.Name; }
        }


        public string DeployInfo()
        {
            throw new NotImplementedException();
        }

        public bool Deploy(DirectoryInfo deploymentDirectory)
        {
            return _shouldDeploy
                && _file.CopyTo(string.Format("{0}\\{1}", deploymentDirectory.FullName, _file.Name)).Exists;
        }

        public BusinessIntelligenceItem(string fileName)
        {
            _file = new FileInfo(fileName);
        }

        public BusinessIntelligenceItem(string fileName,bool shouldDeploy):this(fileName)
        {
            _shouldDeploy = shouldDeploy;
        }
    }
}
