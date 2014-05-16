using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.NUnit.Ext
{
    /// <summary>
    /// Indicates a particular strategy the extractor should follow when 
    /// extracting files.
    /// </summary>
    public enum ExtractorStrategy : byte
    {
        TruncateNamespaceFromName = 0,
        KeepFolderStructure = 1
    }
}
