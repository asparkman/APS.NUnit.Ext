using APS.NUnit.Ext.Tests.ExternalTests.Files.Sub;
using APS.NUnit.Ext.Tests.ExternalTests.Files;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace APS.NUnit.Ext.Tests.ExternalTests
{
    [TestFixture]
    public class EmbeddedResourceCreationTests
    {
        [Test]
        public void CreateFiles_All()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceNames = assembly.GetManifestResourceNames();

            Assert.IsTrue(resourceNames.Any());
            var hitOne = false;
            var hitTwo = false;
            foreach(var resourceName in resourceNames)
            {
                Console.WriteLine("resourceName = {0}", resourceName);
                if(resourceName.Contains("TextFile1.txt") && resourceName.Contains("Sub"))
                {
                    WriteFile(assembly, resourceName);
                    TestFile(resourceName);
                    hitOne = true;
                }
                else if (resourceName.Contains("TextFile1.txt") && !resourceName.Contains("Sub"))
                {
                    WriteFile(assembly, resourceName);
                    TestFile(resourceName);
                    hitTwo = true;
                }
            }

            Assert.IsTrue(hitOne);
            Assert.IsTrue(hitTwo);
        }

        [Test]
        public void GetManifestResourceStream()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceNames = assembly.GetManifestResourceNames();

            Assert.IsTrue(resourceNames.Any());
            var hitOne = false;
            var hitTwo = false;
            foreach (var resourceName in resourceNames.Reverse())
            {
                Console.WriteLine("resourceName = {0}", resourceName);
                Stream stream = null;
                Stream stream2 = null;
                if (resourceName.Contains("SubFile1.txt") && resourceName.Contains("Sub"))
                {
                    stream = assembly.GetManifestResourceStream(new SubNamespace().GetType(), "SubFile1.txt");
                    stream2 = assembly.GetManifestResourceStream(new FilesNamespace().GetType(), "SubFile1.txt");
                    Assert.IsNotNull(stream);
                    Assert.IsNull(stream2);
                    hitOne = true;
                }
                else if (resourceName.Contains("Files2.txt") && !resourceName.Contains("Sub"))
                {
                    stream = assembly.GetManifestResourceStream(new FilesNamespace().GetType(), "Files2.txt");
                    stream2 = assembly.GetManifestResourceStream(new SubNamespace().GetType(), "Files2.txt");
                    Assert.IsNotNull(stream);
                    Assert.IsNull(stream2);
                    hitTwo = true;
                }
            }

            Assert.IsTrue(hitOne);
            Assert.IsTrue(hitTwo);
        }

        protected void WriteFile(Assembly assembly, string resourceName)
        {
            using(var reader = new StreamReader(assembly.GetManifestResourceStream(resourceName)))
            {
                using(var writer = new StreamWriter(resourceName))
                {
                    writer.Write(reader.ReadToEnd());
                }
            }
        }

        protected void TestFile(string resourceName)
        {
            using(var reader = new StreamReader(resourceName))
            {
                Console.WriteLine("resourceName = {0}, contains = {1}", resourceName, reader.ReadToEnd());
            }
        }
    }
}
