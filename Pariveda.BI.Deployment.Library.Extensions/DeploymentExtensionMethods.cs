using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Pariveda.BI.Deployment.Library.Extensions
{
    public static class DeploymentExtensionMethods
    {
        public static DirectoryInfo CopyFilesWithExtension(this DirectoryInfo source, string extension, DirectoryInfo destination, Boolean overwrite)
        {
            if (!extension.StartsWith(".")) extension = string.Format(".{0}", extension);
            foreach (FileInfo fInfo in source.
                                        GetFiles().
                                        Where(f => f.Extension.EndsWith(extension)))
            {
                fInfo.CopyTo(string.Format("{0}\\{1}", destination.FullName, fInfo.Name), overwrite);
            }
            return source;
        }
    }
}
