using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.NUnit.Ext.Templates
{
    /// <summary>
    /// Houses information that the templates need in order to build the source
    /// files accordingly. 
    /// </summary>
    public class T4Params
    {
        /// <summary>
        /// Represents a DbContextType that was picked up.
        /// </summary>
        public virtual DbContextType DbContextType { get; set; }

        /// <summary>
        /// Represents the parameters passed into the Provider run-time text 
        /// template.
        /// </summary>
        public virtual T4Job ProviderJob { get; set; }

        /// <summary>
        /// Represents the parameters passed into the IDbContext run-time text 
        /// template.
        /// </summary>
        public virtual T4Job IDbContextJob { get; set; }

        /// <summary>
        /// Represents the parameters passed into the Fake run-time text 
        /// template.
        /// </summary>
        public virtual T4Job FakeJob { get; set; }
    }
}
