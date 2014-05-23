using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.NUnit.Ext.Templates
{
    /// <summary>
    /// Represents the parameters of a job for one of the run-time text 
    /// templates to perform.
    /// </summary>
    public class T4Job
    {
        /// <summary>
        /// This is specifically the name of the file.  This does not include 
        /// any path elements.
        /// </summary>
        public virtual string FileName { get; set; }

        /// <summary>
        /// This is specifically the path.  This does not include any name 
        /// elements.
        /// </summary>
        public virtual string FilePath { get; set; }

        /// <summary>
        /// Path.Combine on FilePath and FileName.
        /// </summary>
        public virtual string FullPath { get { return Path.Combine(FilePath, FileName); } }

        /// <summary>
        /// This is specifically the namespace of the class that is being 
        /// created.
        /// </summary>
        public virtual string Namespace { get; set; }

        /// <summary>
        /// This represents the name of the type to be created.  Type is used 
        /// as an interface is created by the IDbContext run-time text 
        /// template.
        /// </summary>
        public virtual string TypeName { get; set; }

        /// <summary>
        /// This is specifically the project that the class is being placed 
        /// into.
        /// 
        /// WARNING:
        /// The type has not been decided yet. 
        /// </summary>
        public virtual object Project { get; set; }
    }
}
