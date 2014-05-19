using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.NUnit.Ext
{
    /// <summary>
    /// Holds methods for determining what concrete class of a 
    /// connection-oriented abstract class or interface.
    /// </summary>
    public abstract class BaseProvider<TInterface>
    {
        /// <summary>
        /// The AppSettings value of TestWithConnections.
        /// </summary>
        private static readonly string TEST_WITH_CONNECTIONS = ConfigurationManager.AppSettings["TestWithConnections"];
        
        /// <summary>
        /// Returns the TestWithConnections app setting value.
        /// </summary>
        public static bool TestWithConnections
        {
            get
            {
                bool result = false;
                if (!string.IsNullOrEmpty(TEST_WITH_CONNECTIONS))
                {
                    result = TEST_WITH_CONNECTIONS.Equals(true.ToString(), StringComparison.CurrentCultureIgnoreCase);
                }

                return result;
            }
        }

        /// <summary>
        /// Creates and returns a new fake or mock of the interface being 
        /// tested.
        /// 
        /// This needs to be overridden.
        /// </summary>
        /// <returns>An instance of the fake.</returns>
        public abstract TInterface NewFake();

        /// <summary>
        /// Creates and returns a new real implementation of the interface 
        /// being tested.
        /// 
        /// This needs to be overridden.
        /// </summary>
        /// <returns>An instance of the real implmentation.</returns>
        public abstract TInterface NewReal();

        /// <summary>
        /// Returns an instance for testing depending on the test configuration 
        /// setting.
        /// </summary>
        /// <returns>An instance for testing given the configuration 
        /// settings.</returns>
        public virtual TInterface New()
        {
            TInterface result = default(TInterface);
            if (TestWithConnections)
                result = NewReal();
            else
                result = NewFake();

            return result;
        }
    }
}
