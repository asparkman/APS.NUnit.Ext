using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.NUnit.Ext.Templates
{
    /// <summary>
    /// This class is intended to generate a Fake for the IDbContext that is 
    /// generated.
    /// 
    /// Holds the non-auto-generated part of the Fake run-time text template 
    /// class.
    /// </summary>
    public partial class Fake
    {
        public Fake(T4Params prms)
            : base(prms)
        {
        }
    }
}
