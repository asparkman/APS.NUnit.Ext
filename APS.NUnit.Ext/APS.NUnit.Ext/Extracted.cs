using APS.NUnit.Ext;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace APS.NUnit.Ext
{
    /// <summary>
    /// Stores information about the result of the extraction of an embedded 
    /// resource from an <c>Assembly</c>.
    /// </summary>
    public class Extracted : IDisposable
    {
        /// <summary>
        /// Makes it so this has to be declared inside this assembly.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        internal Extracted(bool deleteOnDispose)
        {
            DeleteOnDispose = deleteOnDispose;
        }
        
        /// <summary>
        /// Indicates whether the file should be deleted when the Dispose 
        /// method is called or not.
        /// </summary>
        public virtual bool DeleteOnDispose { get; set; }

        /// <summary>
        /// Indicates the <c>FileInfo</c> of the extracted file.
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Indicates the source <c>Assembly</c> where the <c>File</c> was 
        /// extracted.
        /// </summary>
        public virtual Assembly Assembly { get; set; }

        /// <summary>
        /// Indicates the namespace of the extracted <c>File</c>.
        /// </summary>
        public virtual string Namespace { get; set; }

        /// <summary>
        /// Calculates a hash code for this object.
        /// </summary>
        /// <returns>The calculated hash code.</returns>
        public override int GetHashCode()
        {
            return
                File.GetHashCode()
                + Assembly.GetHashCode()
                + Namespace.GetHashCode();
        }

        /// <summary>
        /// Indicates whether the object is equal to the other on the <c>
        /// Extracted</c> level.
        /// </summary>
        /// <param name="obj">The object to compare this one to.</param>
        /// <returns>Whether the two are equal.</returns>
        public override bool Equals(object obj)
        {
            bool result = false;
            var conv = obj as Extracted;
            if (conv != null)
            {
                result = this.File.Equals(conv.File)
                    && this.Assembly.Equals(conv.Assembly)
                    && this.Namespace.Equals(conv.Namespace);
            }
            else
            {
                result = base.Equals(obj);
            }
            return result;
        }

        /// <summary>
        /// Conditionally deletes the extracted file based on the value of 
        /// DeleteOnDispose.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Conditionally deletes the extracted file based on the value of 
        /// DeleteOnDispose.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (DeleteOnDispose && File.Exists)
            {
                try
                {
                    File.Delete();
                }
                catch (IOException)
                {
                }
                catch (SecurityException)
                {
                }
                catch (UnauthorizedAccessException)
                {
                }
            }
        }
    }
}
