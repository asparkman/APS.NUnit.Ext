using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.NUnit.Ext.Templates
{
    /// <summary>
    /// Generates a factory of the BaseProvider type that optionally creates a 
    /// Fake for the IDbContext class, or the actual DbContext class. 
    /// 
    /// Holds the non-auto-generated part of the Provider template class.
    /// </summary>
    public partial class Provider
    {
        public Provider(T4Params context)
            : base(context)
        {
        }
    }
}
