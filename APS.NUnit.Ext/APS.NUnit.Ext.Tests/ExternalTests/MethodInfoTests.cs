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
    public class MethodInfoTests
    {
        public readonly Type TYPE = typeof(DerivedClass);

        [Test]
        public void MethodInfo_Blank()
        {
            var methods = TYPE.GetMethods();
            var targets = GetMethodInfo_StartsWith("Blank");
            Assert.IsTrue(targets.Any());

            //targets[0].Attributes; // enum
            //targets[0].CallingConvention; // enum
            Assert.IsFalse(targets[0].ContainsGenericParameters);
            Assert.IsFalse(targets[0].CustomAttributes.Any());
            Assert.IsNotNull(targets[0].DeclaringType);
            Assert.IsFalse(targets[0].IsAbstract);
            Assert.IsFalse(targets[0].IsAssembly);
            Assert.IsFalse(targets[0].IsConstructor);
            Assert.IsFalse(targets[0].IsFamily);
            Assert.IsFalse(targets[0].IsFamilyAndAssembly);
            Assert.IsFalse(targets[0].IsFamilyOrAssembly);
            Assert.IsFalse(targets[0].IsFinal);
            Assert.IsFalse(targets[0].IsGenericMethod);
            Assert.IsFalse(targets[0].IsGenericMethodDefinition);
            Assert.IsTrue(targets[0].IsHideBySig);
            Assert.IsFalse(targets[0].IsPrivate);
            Assert.IsTrue(targets[0].IsPublic);
            Assert.IsTrue(targets[0].IsSecurityCritical);
            Assert.IsFalse(targets[0].IsSecuritySafeCritical);
            Assert.IsFalse(targets[0].IsSecurityTransparent);
            Assert.IsFalse(targets[0].IsSpecialName);
            Assert.IsFalse(targets[0].IsStatic);
            Assert.IsFalse(targets[0].IsVirtual);
            Assert.IsNotNull(targets[0].MemberType);
            //targets[0].MetadataToken; // Not sure what to do with this.
            Assert.IsNotNull(targets[0].MethodHandle);
            Assert.IsNotNull(targets[0].MethodImplementationFlags);
            Assert.IsNotNull(targets[0].Module);
            Assert.IsNotNull(targets[0].Name);
            Assert.IsNotNull(targets[0].ReflectedType);
            Assert.IsNotNull(targets[0].ReturnParameter); // Returns System.Void for void which can't be compared.
            Assert.IsNotNull(targets[0].ReturnType); // Returns System.Void for void which can't be compared.
            Assert.IsFalse(targets[0].ReturnTypeCustomAttributes.GetCustomAttributes(true).Any());
            
            Assert.IsTrue(true);
        }

        [Test]
        public void MethodInfo_Generics()
        {
            var methods = TYPE.GetMethods();
            var targets = GetMethodInfo_StartsWith("Generic_");
            Assert.IsTrue(targets.Any());

            for (int index = 0; index < targets.Count; index++)
            {
                //targets[index].Attributes; // enum
                //targets[index].CallingConvention; // enum
                Assert.IsTrue(targets[index].ContainsGenericParameters);
                Assert.IsFalse(targets[index].CustomAttributes.Any());
                Assert.IsNotNull(targets[index].DeclaringType);
                Assert.IsFalse(targets[index].IsAbstract);
                Assert.IsFalse(targets[index].IsAssembly);
                Assert.IsFalse(targets[index].IsConstructor);
                Assert.IsFalse(targets[index].IsFamily);
                Assert.IsFalse(targets[index].IsFamilyAndAssembly);
                Assert.IsFalse(targets[index].IsFamilyOrAssembly);
                Assert.IsFalse(targets[index].IsFinal);
                Assert.IsTrue(targets[index].IsGenericMethod);
                Assert.IsTrue(targets[index].IsGenericMethodDefinition);
                Assert.IsTrue(targets[index].IsHideBySig);
                Assert.IsFalse(targets[index].IsPrivate);
                Assert.IsTrue(targets[index].IsPublic);
                Assert.IsTrue(targets[index].IsSecurityCritical);
                Assert.IsFalse(targets[index].IsSecuritySafeCritical);
                Assert.IsFalse(targets[index].IsSecurityTransparent);
                Assert.IsFalse(targets[index].IsSpecialName);
                Assert.IsFalse(targets[index].IsStatic);
                Assert.IsFalse(targets[index].IsVirtual);
                Assert.IsNotNull(targets[index].MemberType);
                //targets[index].MetadataToken; // Not sure what to do with this.
                Assert.IsNotNull(targets[index].MethodHandle);
                Assert.IsNotNull(targets[index].MethodImplementationFlags);
                Assert.IsNotNull(targets[index].Module);
                Assert.IsNotNull(targets[index].Name);
                Assert.IsNotNull(targets[index].ReflectedType);
                Assert.IsNotNull(targets[index].ReturnParameter); // Returns System.Void for void which can't be compared.
                Assert.IsNotNull(targets[index].ReturnType); // Returns System.Void for void which can't be compared.
                Assert.IsFalse(targets[index].ReturnTypeCustomAttributes.GetCustomAttributes(true).Any());

                if(index == 0)
                {
                    targets[index].GetGenericArguments()
                }
            }

            Assert.IsTrue(true);
        }

        [Test]
        public void MethodInfo_Parameters()
        {
            var methods = TYPE.GetMethods();
            var targets = GetMethodInfo_StartsWith("ParameterModifier_");
            Assert.IsTrue(targets.Any());

            Assert.IsTrue(true);
        }

        [Test]
        public void MethodInfo_Default()
        {
            var methods = TYPE.GetMethods();
            var targets = GetMethodInfo_StartsWith("Default_");
            Assert.IsTrue(targets.Any());

            Assert.IsTrue(true);
        }

        [Test]
        public void MethodInfo_ReturnType()
        {
            var methods = TYPE.GetMethods();
            var targets = GetMethodInfo_StartsWith("ReturnType_");
            Assert.IsTrue(targets.Any());

            Assert.IsTrue(true);
        }

        [Test]
        public void MethodInfo_Access()
        {
            var methods = TYPE.GetMethods();
            var targets = GetMethodInfo_StartsWith("AccessModifier_");
            Assert.IsTrue(targets.Any());

            Assert.IsTrue(true);
        }

        [Test]
        public void MethodInfo_Other()
        {
            var methods = TYPE.GetMethods();
            var targets = GetMethodInfo_StartsWith("OtherModifier_");
            Assert.IsTrue(targets.Any());

            Assert.IsTrue(true);
        }

        [Test]
        public void MethodInfo_Attributes()
        {
            var methods = TYPE.GetMethods();
            var targets = GetMethodInfo_StartsWith("Attribute_");
            Assert.IsTrue(targets.Any());

            Assert.IsTrue(true);
        }

        protected MethodInfo GetMethodInfo(string name)
        {
            return (
                    from method in TYPE.GetMethods()
                    where method.Name.Equals(name)
                    select method
                ).FirstOrDefault();
        }

        protected List<MethodInfo> GetMethodInfo_StartsWith(string name)
        {
            return (
                    from method in TYPE.GetMethods()
                    where method.Name.StartsWith(name)
                    select method
                ).ToList();
        }
    }

    public abstract class BaseClass
    {
        #region
        public void Blank() { }
        #endregion

        #region GENERICS
        public void Generic_Parameter<T>(T a) { }
        public T Generic_Return<T>() { return default(T); }
        public T Generic_Constraints<T>() where T : class { return default(T); }
        #endregion

        #region PARAMETER MODIFIERS
        public void ParameterModifier_Ref(ref int a) { }
        public void ParameterModifier_Out(out int a) { a = 3; }
        public void ParameterModifier_Params(params int[] a) { }
        #endregion

        #region DEFAULT VALUES
        public void Default_1(string a = "a") { }
        #endregion

        #region RETURN TYPES
        public DayOfWeek ReturnType_DayOfWeek() { return DayOfWeek.Friday; }
        public int ReturnType_Int() { return 1; }
        public FileInfo ReturnType_FileInfo() { return new FileInfo(".\\"); }
        #endregion

        #region ACCESS MODIFIERS
        public void AccessModifier_Public() { }
        protected void AccessModifier_Protected() { }
        void AccessModifier_None() { }
        internal void AccessModifier_Internal() { }
        private void AccessModifier_Private() { }
        #endregion

        #region OTHER MODIFIERS
        public virtual void OtherModifier_Virtual() { }
        public static void OtherModifier_Static() { }
        public virtual void OtherModifier_Override() { }
        public virtual void OtherModifier_New() { }
        public virtual void OtherModifier_Override_Sealed() { }
        public abstract void OtherModifier_Abstract();
        //public extern void OtherModifier_7(); // Not sure how to do this.
        #endregion

        #region ATTRIBUTES
        [Method("a", 1)] public int Attribute_Method<T>(int val) { return val; }
        [return: RetVal()] public int Attribute_ReturnValue<T>(int val) { return val; }
        public int Attribute_Parameter<T>([Param(Str = "a", Int = 1)] int val) { return val; }
        public int Attribute_GenericParameter<[GenericParam(Str = "a", Int = 1)] T>(int val) { return val; }
        #endregion
    }

    public class DerivedClass : BaseClass
    {
        #region OTHER MODIFIERS
        public override void OtherModifier_Override() { }
        public new void OtherModifier_New() { }
        public override sealed void OtherModifier_Override_Sealed() { }
        public override void OtherModifier_Abstract() { }
        #endregion
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class MethodAttribute : Attribute
    {
        public MethodAttribute()
        { 
        }

        public MethodAttribute(string str, int i)
        {
            Str = str;
            Int = i;
        }

        public virtual string Str { get; set; }
        public virtual int Int { get; set; }
    }


    [AttributeUsage(AttributeTargets.ReturnValue)]
    public class RetValAttribute : Attribute
    {
        public RetValAttribute()
        { 
        }

        public RetValAttribute(string str, int i)
        {
            Str = str;
            Int = i;
        }

        public virtual string Str { get; set; }
        public virtual int Int { get; set; }
    }


    [AttributeUsage(AttributeTargets.Parameter)]
    public class ParamAttribute : Attribute
    {
        public ParamAttribute()
        { 
        }

        public ParamAttribute(string str, int i)
        {
            Str = str;
            Int = i;
        }

        public virtual string Str { get; set; }
        public virtual int Int { get; set; }
    }

    [AttributeUsage(AttributeTargets.GenericParameter)]
    public class GenericParamAttribute : Attribute
    {
        public GenericParamAttribute()
        { 
        }

        public GenericParamAttribute(string str, int i)
        {
            Str = str;
            Int = i;
        }

        public virtual string Str { get; set; }
        public virtual int Int { get; set; }
    }
}
