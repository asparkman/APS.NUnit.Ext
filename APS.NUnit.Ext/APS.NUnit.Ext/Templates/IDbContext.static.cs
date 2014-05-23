using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.NUnit.Ext.Templates
{
    /// <summary>
    /// An interface that mimics a generated, or existing DbContext class.
    /// 
    /// Holds the non-auto-generated part of the IDbContext run-time text 
    /// template class.
    /// </summary>
    public partial class IDbContext
    {
        public IDbContext(T4Params context)
            : base(context)
        {
        }
    }
}
