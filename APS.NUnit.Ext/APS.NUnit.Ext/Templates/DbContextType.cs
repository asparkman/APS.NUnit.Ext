using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace APS.NUnit.Ext.Templates
{
    /// <summary>
    /// Represents the needed information regarding a DbContext object found 
    /// in another assembly.
    /// </summary>
    public class DbContextType
    {
        /// <summary>
        /// Initializes all the properties from the type.
        /// </summary>
        /// <param name="type">A type that inherits from the DbContext class.</param>
        public DbContextType(Type type)
        {
            Type = type;
            ContainingAssembly = type.Assembly;

            DbSetProperties = (
                    from prop in type.GetProperties(BindingFlags.Public)
                    where prop.DeclaringType.IsGenericType
                        && prop.DeclaringType.GetGenericTypeDefinition().Equals(typeof(DbSet<>))
                    select prop
                ).ToList();

            OtherProperties = (
                    from prop in type.GetProperties(BindingFlags.Public)
                    where !(
                            prop.DeclaringType.IsGenericType
                            && prop.DeclaringType.GetGenericTypeDefinition().Equals(typeof(DbSet<>))
                        )
                    select prop
                ).ToList();

            AvailableMethods = type.GetMethods(BindingFlags.Public).ToList();

            Constructors = type.GetConstructors(BindingFlags.Public).ToList();
        }

        /// <summary>
        /// Reference to the assembly that defines this type that inherits 
        /// from DbContext.
        /// </summary>
        public virtual Assembly ContainingAssembly { get; set; }
        /// <summary>
        /// Reference to the type that inherits from DbContext.
        /// </summary>
        public virtual Type Type { get; set; }

        /// <summary>
        /// The properties that are of the DbSetlt;gt; type.
        /// </summary>
        public virtual List<PropertyInfo> DbSetProperties { get; set; }
        /// <summary>
        /// The properties that are not of the DbSetlt;gt; type.
        /// </summary>
        public virtual List<PropertyInfo> OtherProperties { get; set; }

        /// <summary>
        /// The methods are are publicly accessible.
        /// </summary>
        public virtual List<MethodInfo> AvailableMethods { get; set; }
        /// <summary>
        /// The constructors that are publicly accessible.
        /// </summary>
        public virtual List<ConstructorInfo> Constructors { get; set; }
    }
}
