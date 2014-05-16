using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace APS.NUnit.Ext
{
    /// <summary>
    /// Holds methods for creating files out of embedded resources.
    /// </summary>
    public class Extractor
    {
        /// <summary>
        /// Instantiates an <c>Extractor</c> object with the strategy to use.
        /// </summary>
        /// <param name="keepFolderStructure">Whether or not the relative path 
        /// in the project should be kept, or they should be applied.</param>
        /// <param name="truncNamespace">Whether or not the base namespace for 
        /// the project should be truncated from the file name.  This may cause 
        /// files to be overwritten if <c>KeepFolderStructure</c> is also set.
        /// </param>
        public Extractor(bool keepFolderStructure = true, bool truncNamespace = true)
        {
            KeepFolderStructure = keepFolderStructure;
            TruncNamespace = truncNamespace;
        }

        /// <summary>
        /// Whether or not the relative path in the project should be kept, or 
        /// they should be applied.
        /// </summary>
        public virtual bool KeepFolderStructure { get; set; }

        /// <summary>
        /// Whether or not the base namespace for the project should be 
        /// truncated from the file name.  This may cause files to be 
        /// overwritten if <c>KeepFolderStructure</c> is not set.
        /// </summary>
        public virtual bool TruncNamespace { get; set; }

        #region SINGLE FILE METHODS
        /// <summary>
        /// Determines the embedded resource by concatenating the namespace of 
        /// the type and the filename.  It then uses this to extract the 
        /// embedded resource.
        /// </summary>
        /// <param name="type">The <c>Type</c> whose namespace is used to 
        /// determine the single embedded resource to extract. </param>
        /// <param name="filename">The name of the file that should be 
        /// extracted from the namespace.</param>
        /// <returns>The information about the embedded resource that was 
        /// extracted.</returns>
        public Extracted Single(Type type, string filename)
        {
            Extracted result = null;
            var assembly = type.Assembly;

            if(filename.StartsWith("\\"))
                filename = filename.Substring(1);

            var relativePath = "";
            if (KeepFolderStructure)
            {
                var baseNs = Extractor.BaseNs(assembly, type);
                relativePath = type.Namespace.Substring(baseNs.Length).Replace(".", "\\");
                if (relativePath.StartsWith("\\"))
                    relativePath = relativePath.Substring(1);
                Directory.CreateDirectory(".\\" + relativePath);
            }

            using(var reader = new StreamReader(assembly.GetManifestResourceStream(type, filename)))
            {
                if(!TruncNamespace)
                {
                    filename = type.Namespace + "." + filename;
                }
                var file = ".\\" + relativePath + "\\" + filename;
                using(var writer = new StreamWriter(file))
                {
                    writer.Write(reader.ReadToEnd());
                    result = new Extracted()
                    {
                        File = new FileInfo(file),
                        Namespace = type.Namespace,
                        Assembly = assembly
                    };
                }
            }

            return result;
        }

        /// <summary>
        /// Determines the embedded resource by concatenating the namespace of
        /// the type and the filename.  It then uses this to extract the 
        /// embedded resource.
        /// </summary>
        /// <param name="obj">The <c>Object</c> whose namespace is used to
        /// determine the single embedded resource to extract.</param>
        /// <param name="filename">The name of the file that should be 
        /// extracted from the namespace.</param>
        /// <returns>The information about the embedded resource that was 
        /// extracted.</returns>
        public Extracted Single(object obj, string filename)
        {
            return Single(obj.GetType(), filename);
        }

        /// <summary>
        /// Determines the embedded resource by concatenating the namespace and 
        /// the filename.  It then uses this to extract the embedded resource.
        /// </summary>
        /// <param name="assembly">The assembly where the namespace resides.
        /// </param>
        /// <param name="ns">The namespace used to name the embedded resource.
        /// </param>
        /// <param name="filename">The name of the file that should be 
        /// extracted from the namespace.</param>
        /// <returns>The information about the embedded resource that was 
        /// extracted.</returns>
        public Extracted Single(Assembly assembly, string ns, string filename)
        {
            Extracted result = null;

            if (filename.StartsWith("\\"))
                filename = filename.Substring(1);

            var relativePath = "";
            if (KeepFolderStructure)
            {
                var baseNs = Extractor.BaseNs(assembly, ns);
                relativePath = ns.Substring(baseNs.Length).Replace(".", "\\");
                if (relativePath.StartsWith("\\"))
                    relativePath = relativePath.Substring(1);
                Directory.CreateDirectory(".\\" + relativePath);
            }

            using (var reader = new StreamReader(assembly.GetManifestResourceStream(ns + "." + filename)))
            {
                if (!TruncNamespace)
                {
                    filename = ns + "." + filename;
                }
                var file = ".\\" + relativePath + "\\" + filename;
                using (var writer = new StreamWriter(file))
                {
                    writer.Write(reader.ReadToEnd());
                    result = new Extracted()
                    {
                        File = new FileInfo(file),
                        Namespace = ns,
                        Assembly = assembly
                    };
                }
            }

            return result;
        }
        #endregion

        #region MULTIPLE FILE METHODS
        /// <summary>
        /// Extracts all embedded resources according to this 
        /// <c>Extractor</c>'s configuration.
        /// </summary>
        /// <param name="assemblies">The assemblies whose embedded resources 
        /// need extracting.</param>
        /// <returns>The list of extractions that occurred.</returns>
        public List<Extracted> All(params Assembly[] assemblies)
        {
            var results = new List<Extracted>();
            foreach(var assembly in assemblies)
            {
                var resourceNames = assembly.GetManifestResourceNames();
                foreach(var resourceName in resourceNames)
                {
                    var splitResourceName = resourceName.Split('.');
                    var filename = "";
                    var ns = string.Join(".", splitResourceName.Take(splitResourceName.Length - 2).ToArray()); 

                    if (TruncNamespace)
                    {
                        filename = splitResourceName[splitResourceName.Length - 2]
                             + "."
                             + splitResourceName[splitResourceName.Length - 1];
                    }
                    else
                    {
                        filename = resourceName;
                    }

                    var relativePath = "";
                    if (KeepFolderStructure)
                    {
                        var baseNs = Extractor.BaseNs(assembly);
                        var splitBaseNs = baseNs.Split('.');
                        for (int i = splitBaseNs.Length; i < splitResourceName.Length; i++)
                        {
                            relativePath += splitResourceName[i];
                            if(i != splitResourceName.Length - 1)
                                relativePath += "\\";
                        }
                        if (relativePath.StartsWith("\\"))
                            relativePath = relativePath.Substring(1);
                        Directory.CreateDirectory(".\\" + relativePath);
                    }

                    using (var reader = new StreamReader(assembly.GetManifestResourceStream(resourceName)))
                    {
                        var file = ".\\" + relativePath + "\\" + filename;
                        using (var writer = new StreamWriter(file))
                        {
                            writer.Write(reader.ReadToEnd());
                            results.Add(new Extracted()
                            {
                                File = new FileInfo(file),
                                Namespace = ns,
                                Assembly = assembly
                            });
                        }
                    }
                }
            }

            return results;
        }

        /// <summary>
        /// Extracts all embedded resources from the assemblies of the given 
        /// <c>Type</c>s.
        /// </summary>
        /// <param name="types">The types whose assemblies need to be 
        /// extracted.</param>
        /// <returns>The result of extracting all found embedded resources.
        /// </returns>
        public List<Extracted> All(params Type[] types)
        {
            var assemblies = types.Select(x => x.Assembly).Distinct().ToArray<Assembly>();
            return All(assemblies);
        }


        /// <summary>
        /// Extracts all embedded resources from the assemblies of the 
        /// <c>Type</c>s specified by the list of object passed to this method.
        /// </summary>
        /// <param name="objects">The list of objects whose determined 
        /// assemblies will be used to extract embedded resources.</param>
        /// <returns>The result of extracting all found embedded resources.
        /// </returns>
        public List<Extracted> All(params object[] objects)
        {
            var types = objects.Select(x => x.GetType()).Distinct().ToArray<Type>();
            return All(types);
        }

        /// <summary>
        /// Extracts the embedded resources for the namespaces of the given 
        /// <c>Type</c>s.
        /// </summary>
        /// <param name="types">The types whose namespaces should be used to 
        /// extract embedded resources.</param>
        /// <returns>The result of extracting all found embedded resources.
        /// </returns>
        public List<Extracted> AllNs(params Type[] types)
        {
            var results = new List<Extracted>();
            var groupedByAssemblyThenType = (
                from type in types
                group type by type.Assembly into g
                select new
                {
                    Key = g.Key,
                    Values = (
                            from typeOfAssembly in g
                            group typeOfAssembly by typeOfAssembly.Namespace into h
                            select new
                            {
                                Key = h.Key,
                                Values = h.ToList()
                            }
                        ).ToList()
                }
            ).ToList();

            foreach(var assembly in groupedByAssemblyThenType)
            {
                var resourceNames = assembly.Key.GetManifestResourceNames();
                foreach(var ns in assembly.Values)
                {
                    var nsDotCount = ns.Key.Count(y => y.Equals('.'));
                    var matchingResourceNames = resourceNames
                        .Where(x => x.StartsWith(ns.Key)
                                && x.Count(y => y.Equals('.')) - 1 == nsDotCount
                            )
                        .ToList();

                    foreach(var matchingResource in matchingResourceNames)
                    {
                        var filename = matchingResource.Substring(ns.Key.Length);

                        var relativePath = "";
                        if (KeepFolderStructure)
                        {
                            var baseNs = Extractor.BaseNs(assembly.Key, ns.Key);
                            relativePath = ns.Key.Substring(baseNs.Length).Replace(".", "\\");
                            if (relativePath.StartsWith("\\"))
                                relativePath = relativePath.Substring(1);
                            Directory.CreateDirectory(".\\" + relativePath);
                        }

                        using(var reader = new StreamReader(assembly.Key.GetManifestResourceStream(matchingResource)))
                        {
                            if (!TruncNamespace)
                            {
                                filename = ns + "." + filename;
                            }
                            var file = ".\\" + relativePath + "\\" + filename;
                            using(var writer = new StreamWriter(filename))
                            {
                                writer.Write(reader.ReadToEnd());
                                results.Add(new Extracted()
                                        {
                                            File = new FileInfo(file),
                                            Namespace = ns.Key,
                                            Assembly = assembly.Key
                                        }
                                    );
                            }
                        }
                    }
                }
            }

            return results;
        }

        /// <summary>
        /// Extracts the embedded resources for the namespaces of the given 
        /// <c>Object</c>s.
        /// </summary>
        /// <param name="objects">The objects whose namespaces should be used 
        /// to extract embedded resources.</param>
        /// <returns>The result of extracting all found embedded resources.
        /// </returns>
        public List<Extracted> AllNs(params object[] objects)
        {
            return AllNs(objects.Select(x => x.GetType()).ToArray<Type>());
        }
        #endregion

        /// <summary>
        /// The base namespace is a Visual Studio thing.  This method tries to 
        /// find it using the namespaces found in the assembly.
        /// </summary>
        /// <param name="assembly">The assembly to search for its base 
        /// namespace.</param>
        /// <returns>The most likely candidate for the base namespace defined 
        /// in Visual Studio.</returns>
        public static string BaseNs(Assembly assembly)
        {
            var namespaces = (
                from type in assembly.GetTypes()
                select type.Namespace
            ).Distinct().ToList();

            var toRemove = new List<int>();

            for(int i = namespaces.Count - 1; i >= 0; i--)
            {
                if (namespaces.Any(x => namespaces[i].StartsWith(x) && !namespaces[i].Equals(x)))
                    toRemove.Add(i);
            }

            foreach(var removable in toRemove)
            {
                namespaces.RemoveAt(removable);
            }

            return namespaces.FirstOrDefault();
        }

        /// <summary>
        /// The base namespace is a Visual Studio thing.  This method tries to 
        /// find it using the namespaces found in the assembly.
        /// 
        /// This method should be used if the assembly contains more than one 
        /// possible base namespace.
        /// </summary>
        /// <param name="assembly">The assembly to search for its base 
        /// namespace.</param>
        /// <param name="ns">The namespace that should be searched.</param>
        /// <returns>The base namespace for the namespace of the provided type
        /// in the assembly.</returns>
        public static string BaseNs(Assembly assembly, string ns)
        {
            var namespaces = (
                from type in assembly.GetTypes()
                where ns.StartsWith(type.Namespace)
                select type.Namespace
            ).Distinct().ToList();

            var toRemove = new List<int>();

            for (int i = namespaces.Count - 1; i >= 0; i--)
            {
                if (namespaces.Any(x => namespaces[i].StartsWith(x) && !namespaces[i].Equals(x)))
                    toRemove.Add(i);
            }

            foreach (var removable in toRemove)
            {
                namespaces.RemoveAt(removable);
            }

            return namespaces.FirstOrDefault();
        }

        /// <summary>
        /// The base namespace is a Visual Studio thing.  This method tries to 
        /// find it using the namespaces found in the assembly.
        /// 
        /// This method should be used if the assembly contains more than one 
        /// possible base namespace.
        /// </summary>
        /// <param name="assembly">The assembly to search for its base 
        /// namespace.</param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string BaseNs(Assembly assembly, Type type)
        {
            return BaseNs(assembly, type.Namespace);
        }
    }
}
