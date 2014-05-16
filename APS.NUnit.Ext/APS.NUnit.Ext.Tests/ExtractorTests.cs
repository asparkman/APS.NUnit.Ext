using APS.NUnit.Ext.Tests.ExternalTests.Files;
using APS.NUnit.Ext.Tests.ExternalTests.Files.Sub;
using APS.NUnit.Ext.Tests.Files.ExtractorTests;
using Base.Namespace;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace APS.NUnit.Ext.Tests
{
    [TestFixture]
    public class ExtractorTests
    {
        [Test]
        public void Ctor_All()
        {
            var type = this.GetType();

            var targets = new List<Extractor>()
            {
                new Extractor(false, false),
                new Extractor(false, true),
                new Extractor(true, false),
                new Extractor(true, true)
            };

            var ignoreFolderStructure = ".";
            var keepFolderStructure = ".\\Files\\" + type.Name;

            var keepNamespace = typeof(ExtractorTestsAnchor).Namespace + ".Ctor_{0}.txt";
            var truncNamespace = "Ctor_{0}.txt";

            var expectations = new List<string>()
            {
                Path.Combine(ignoreFolderStructure, keepNamespace),
                Path.Combine(ignoreFolderStructure, truncNamespace),
                Path.Combine(keepFolderStructure, keepNamespace),
                Path.Combine(keepFolderStructure, truncNamespace)
            };

            Assert.AreEqual(targets.Count, expectations.Count);

            var passed = true;
            var failed = new List<KeyValuePair<string, string>>();
            for(int i = 0; i < targets.Count; i++)
            {
                var target = targets[i];
                var ns = type.Namespace + ".Files." + type.Name;
                var filename = string.Format("Ctor_" + i + ".txt");
                var expected = new FileInfo(string.Format(expectations[i], i));
                var result = target.Single(type.Assembly, ns, filename);

                if(!expected.FullName.Equals(result.File.FullName))
                {
                    failed.Add(new KeyValuePair<string, string>(expected.FullName, result.File.FullName));
                    passed = false;
                }
            }

            Assert.IsTrue(passed);
        }

        [Test]
        public void Single_Type_All()
        {
            var target = new Extractor();
            var type = this.GetType();
            var expected = new FileInfo(".\\Files\\" + type.Name + "\\Single_Type_0.txt");
            var result = target.Single(typeof(ExtractorTestsAnchor), "Single_Type_0.txt");

            Assert.AreEqual(expected.FullName, result.File.FullName);
            Assert.IsNotNull(result.Namespace);
            Assert.IsNotNull(result.Assembly);
        }

        [Test]
        public void Single_Object_All()
        {
            var target = new Extractor();
            var type = this.GetType();
            var expected = new FileInfo(".\\Files\\" + type.Name + "\\Single_Object_0.txt");
            var result = target.Single(new ExtractorTestsAnchor(), "Single_Object_0.txt");

            Assert.AreEqual(expected.FullName, result.File.FullName);
            Assert.IsNotNull(result.Namespace);
            Assert.IsNotNull(result.Assembly);
        }


        [Test]
        public void Single_Assembly_String_All()
        {
            var target = new Extractor();
            var type = this.GetType();
            var ns = typeof(ExtractorTestsAnchor).Namespace;
            var assembly = typeof(ExtractorTestsAnchor).Assembly;
            var expected = new FileInfo(".\\Files\\" + type.Name + "\\Single_Assembly_String_0.txt");
            var result = target.Single(assembly, ns, "Single_Assembly_String_0.txt");

            Assert.AreEqual(expected.FullName, result.File.FullName);
            Assert.IsNotNull(result.Namespace);
            Assert.IsNotNull(result.Assembly);
        }

        [Test]
        public void All_ListAssembly_All()
        {
            var target = new Extractor();
            var type = this.GetType();
            var assembly = Assembly.GetExecutingAssembly();
            var resourceNames = assembly.GetManifestResourceNames();
            var result = target.All(assembly);

            Assert.AreEqual(resourceNames.Length, result.Count);
            foreach(var extracted in result)
            {
                Assert.IsNotNull(extracted.File);
                Assert.IsNotNull(extracted.Namespace);
                Assert.IsNotNull(extracted.Assembly);

                var resourceName = extracted.Namespace + "." + extracted.File.Name;
                Assert.IsTrue(resourceNames.Any(x => x.Equals(resourceName, StringComparison.InvariantCultureIgnoreCase)));
            }

            var distinctExtracted = result.Distinct();
            Assert.AreEqual(distinctExtracted.Count(), result.Count);
        }

        [Test]
        public void All_ListType_All()
        {
            var target = new Extractor();
            var type = this.GetType();
            var assembly = Assembly.GetExecutingAssembly();
            var resourceNames = assembly.GetManifestResourceNames();
            var result = target.All(type);

            Assert.AreEqual(resourceNames.Length, result.Count);
            foreach (var extracted in result)
            {
                Assert.IsNotNull(extracted.File);
                Assert.IsNotNull(extracted.Namespace);
                Assert.IsNotNull(extracted.Assembly);

                var resourceName = extracted.Namespace + "." + extracted.File.Name;
                Assert.IsTrue(resourceNames.Any(x => x.Equals(resourceName, StringComparison.InvariantCultureIgnoreCase)));
            }

            var distinctExtracted = result.Distinct();
            Assert.AreEqual(distinctExtracted.Count(), result.Count);
        }

        [Test]
        public void All_ListObject_All()
        {
            var target = new Extractor();
            var type = this.GetType();
            var assembly = Assembly.GetExecutingAssembly();
            var resourceNames = assembly.GetManifestResourceNames();
            var result = target.All(this);

            Assert.AreEqual(resourceNames.Length, result.Count);
            foreach (var extracted in result)
            {
                Assert.IsNotNull(extracted.File);
                Assert.IsNotNull(extracted.Namespace);
                Assert.IsNotNull(extracted.Assembly);

                var resourceName = extracted.Namespace + "." + extracted.File.Name;
                Assert.IsTrue(resourceNames.Any(x => x.Equals(resourceName, StringComparison.InvariantCultureIgnoreCase)));
            }

            var distinctExtracted = result.Distinct();
            Assert.AreEqual(distinctExtracted.Count(), result.Count);
        }

        [Test]
        public void AllNs_ListType_All()
        {
            var target = new Extractor();
            var type = this.GetType();
            var assembly = Assembly.GetExecutingAssembly();
            var resourceNames = assembly.GetManifestResourceNames();
            var anchorType = typeof(ExtractorTestsAnchor);
            var anchorNs = anchorType.Namespace;
            var anchorNsDotCount = anchorNs.Count(x => x.Equals('.'));
            var nsResourceNames = resourceNames.Where(x => 
                    x.StartsWith(anchorNs) 
                    && x.EndsWith(".txt") 
                    && anchorNsDotCount == x.Count(y => y.Equals('.')) - 1
                ).ToList();

            var result = target.AllNs(anchorType);
            Assert.AreEqual(nsResourceNames.Count, result.Count);
            foreach (var extracted in result)
            {
                Assert.IsNotNull(extracted.File);
                Assert.IsNotNull(extracted.Namespace);
                Assert.IsNotNull(extracted.Assembly);

                var resourceName = extracted.Namespace + "." + extracted.File.Name;
                Assert.IsTrue(resourceNames.Any(x => x.Equals(resourceName, StringComparison.InvariantCultureIgnoreCase)));
            }

            var distinctExtracted = result.Distinct();
            Assert.AreEqual(distinctExtracted.Count(), result.Count);
        }

        [Test]
        public void AllNs_ListObject_All()
        {
            var target = new Extractor();
            var type = this.GetType();
            var assembly = Assembly.GetExecutingAssembly();
            var resourceNames = assembly.GetManifestResourceNames();
            var anchorType = typeof(ExtractorTestsAnchor);
            var anchorNs = anchorType.Namespace;
            var anchorNsDotCount = anchorNs.Count(x => x.Equals('.'));
            var nsResourceNames = resourceNames.Where(x =>
                    x.StartsWith(anchorNs)
                    && x.EndsWith(".txt")
                    && anchorNsDotCount == x.Count(y => y.Equals('.')) - 1
                ).ToList();

            var result = target.AllNs(new ExtractorTestsAnchor());
            Assert.AreEqual(nsResourceNames.Count, result.Count);
            foreach (var extracted in result)
            {
                Assert.IsNotNull(extracted.File);
                Assert.IsNotNull(extracted.Namespace);
                Assert.IsNotNull(extracted.Assembly);

                var resourceName = extracted.Namespace + "." + extracted.File.Name;
                Assert.IsTrue(resourceNames.Any(x => x.Equals(resourceName, StringComparison.InvariantCultureIgnoreCase)));
            }

            var distinctExtracted = result.Distinct();
            Assert.AreEqual(distinctExtracted.Count(), result.Count);
        }

        [Test]
        public void BaseNs_Assembly_All()
        {
            var expected = this.GetType().Namespace;
            var result = Extractor.BaseNs(this.GetType().Assembly);
            var result2 = Extractor.BaseNs(typeof(FilesNamespace).Assembly);
            var result3 = Extractor.BaseNs(typeof(SubNamespace).Assembly);
            var result4 = Extractor.BaseNs(typeof(BaseNamespaceAnchor).Assembly);
            Assert.AreEqual(result, result2);
            Assert.AreEqual(result2, result3);
            Assert.AreEqual(result3, result4);

            var base1 = typeof(BaseNamespaceAnchor).Namespace;
            var base2 = typeof(ExtractorTests).Namespace;

            Assert.IsTrue(base1.Equals(result) || base2.Equals(result));
        }

        [Test]
        public void BaseNs_Assembly_String_All()
        {
            var expected = this.GetType().Namespace;
            var result = Extractor.BaseNs(this.GetType().Assembly, typeof(ExtractorTests).Namespace);
            var result2 = Extractor.BaseNs(typeof(FilesNamespace).Assembly, typeof(FilesNamespace).Namespace);
            var result3 = Extractor.BaseNs(typeof(SubNamespace).Assembly, typeof(SubNamespace).Namespace);
            var result4 = Extractor.BaseNs(typeof(BaseNamespaceAnchor).Assembly, typeof(BaseNamespaceAnchor).Namespace);
            Assert.AreEqual(result, result2);
            Assert.AreEqual(result2, result3);
            Assert.AreNotEqual(result3, result4);
        }

        [Test]
        public void BaseNs_Assembly_Type_All()
        {
            var expected = this.GetType().Namespace;
            var result = Extractor.BaseNs(this.GetType().Assembly, typeof(ExtractorTests));
            var result2 = Extractor.BaseNs(typeof(FilesNamespace).Assembly, typeof(FilesNamespace));
            var result3 = Extractor.BaseNs(typeof(SubNamespace).Assembly, typeof(SubNamespace));
            var result4 = Extractor.BaseNs(typeof(BaseNamespaceAnchor).Assembly, typeof(BaseNamespaceAnchor));
            Assert.AreEqual(result, result2);
            Assert.AreEqual(result2, result3);
            Assert.AreNotEqual(result3, result4);
        }
    }
}

namespace Base.Namespace
{
    public class BaseNamespaceAnchor
    {

    }
}
