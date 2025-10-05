using System;
using System.Collections.Generic;

namespace AssemblyLoadContextHelper.Tests;

[TestType]
internal class SimpleClass
{
    [TestMethod]
    public static string ParmeterlessMethod()
        => $"{nameof(SimpleClass)}.{nameof(ParmeterlessMethod)}";

    [TestMethod]
    public static string MultiParameterMethod(SimpleClass a, int b, List<int> c, Dictionary<SimpleClass, SimpleClass> d)
        => $"{nameof(SimpleClass)}.{nameof(MultiParameterMethod)}";

    [TestMethod]
    public static string GenericParameterlessMethod<TM1, TM2>()
        => $"{nameof(SimpleClass)}.{nameof(GenericParameterlessMethod)}<{typeof(TM1).Name},{typeof(TM2).Name}>";

    [TestMethod]
    public static string GenericMultiParameterMethod<TM1, TM2>(TM1 a, TM2 b, int c)
        => $"{nameof(SimpleClass)}.{nameof(GenericMultiParameterMethod)}<{typeof(TM1).Name},{typeof(TM2).Name}>";

    [TestMethod]
    public static string OverrideMethod()
        => $"{nameof(SimpleClass)}.{nameof(OverrideMethod)}.1";

    [TestMethod]
    public static string OverrideMethod<TM1>()
        => $"{nameof(SimpleClass)}.{nameof(OverrideMethod)}.2";

    [TestMethod]
    public static string OverrideMethod<TM1>(TM1 a)
        => $"{nameof(SimpleClass)}.{nameof(OverrideMethod)}.3";

    [TestMethod]
    public static string OverrideMethod<TM1>(TM1[] a)
        => $"{nameof(SimpleClass)}.{nameof(OverrideMethod)}.4";

    [TestMethod]
    public static string OverrideMethod(int a)
        => $"{nameof(SimpleClass)}.{nameof(OverrideMethod)}.5";

    [TestMethod]
    public static string OverrideMethod(ref int a)
        => $"{nameof(SimpleClass)}.{nameof(OverrideMethod)}.6";

    [TestMethod]
    public static string OverrideMethod(uint a)
        => $"{nameof(SimpleClass)}.{nameof(OverrideMethod)}.7";

    [TestMethod]
    public static string OverrideMethod(int[] a)
        => $"{nameof(SimpleClass)}.{nameof(OverrideMethod)}.8";

    [TestMethod]
    public static string OverrideMethod(Span<int> a)
        => $"{nameof(SimpleClass)}.{nameof(OverrideMethod)}.9";

    [TestMethod]
    public static string OverrideMethod(int a, string b)
        => $"{nameof(SimpleClass)}.{nameof(OverrideMethod)}.10";

    [TestMethod]
    public static string OverrideMethod(int a, IEnumerable<char> b)
        => $"{nameof(SimpleClass)}.{nameof(OverrideMethod)}.11";

    [TestType]
    internal class NestedClass
    {
        [TestMethod]
        public static string ParmeterlessMethod()
                => $"{nameof(NestedClass)}.{nameof(SimpleClass)}.{nameof(ParmeterlessMethod)}";

        [TestMethod]
        public static string MultiParameterMethod(SimpleClass a, int b, List<int> c, Dictionary<SimpleClass, SimpleClass> d)
                => $"{nameof(NestedClass)}.{nameof(SimpleClass)}.{nameof(MultiParameterMethod)}";

        [TestMethod]
        public static string GenericParameterlessMethod<TM1, TM2>()
                => $"{nameof(NestedClass)}.{nameof(SimpleClass)}.{nameof(GenericParameterlessMethod)}<{typeof(TM1).Name},{typeof(TM2).Name}>";

        [TestMethod]
        public static string GenericMultiParameterMethod<TM1, TM2>(TM1 a, TM2 b, int c)
                => $"{nameof(NestedClass)}.{nameof(SimpleClass)}.{nameof(GenericMultiParameterMethod)}<{typeof(TM1).Name},{typeof(TM2).Name}>";
    }
}

[TestType]
internal class SimpleClass<TC1>
{
    [TestMethod]
    public static string ParmeterlessMethod()
        => $"{nameof(SimpleClass<TC1>)}.{nameof(ParmeterlessMethod)}";

    [TestMethod]
    public static string MultiParameterMethod(SimpleClass a, int b, List<int> c, Dictionary<SimpleClass, SimpleClass> d)
        => $"{nameof(SimpleClass<TC1>)}.{nameof(MultiParameterMethod)}";

    [TestMethod]
    public static string GenericParameterlessMethod<TM1, TM2>()
        => $"{nameof(SimpleClass<TC1>)}.{nameof(GenericParameterlessMethod)}<{typeof(TM1).Name},{typeof(TM2).Name}>";

    [TestMethod]
    public static string GenericMultiParameterMethod<TM1, TM2>(TM1 a, TM2 b, int c)
        => $"{nameof(SimpleClass<TC1>)}.{nameof(GenericMultiParameterMethod)}<{typeof(TM1).Name},{typeof(TM2).Name}>";

    [TestType]
    internal class NestedClass
    {
        [TestMethod]
        public static string ParmeterlessMethod()
                => $"{nameof(SimpleClass<TC1>)}.{nameof(SimpleClass)}.{nameof(ParmeterlessMethod)}";

        [TestMethod]
        public static string MultiParameterMethod(SimpleClass a, int b, List<int> c, Dictionary<SimpleClass, SimpleClass> d)
                => $"{nameof(SimpleClass<TC1>)}.{nameof(SimpleClass)}.{nameof(MultiParameterMethod)}";

        [TestMethod]
        public static string GenericParameterlessMethod<TM1, TM2>()
                => $"{nameof(SimpleClass<TC1>)}.{nameof(SimpleClass)}.{nameof(GenericParameterlessMethod)}<{typeof(TM1).Name},{typeof(TM2).Name}>";

        [TestMethod]
        public static string GenericMultiParameterMethod<TM1, TM2>(TM1 a, TM2 b, int c)
                => $"{nameof(SimpleClass<TC1>)}.{nameof(SimpleClass)}.{nameof(GenericMultiParameterMethod)}<{typeof(TM1).Name},{typeof(TM2).Name}>";
    }
}

[TestType]
internal class SimpleClass<TC1, TC2>
{
    [TestMethod]
    public static string ParmeterlessMethod()
        => $"{nameof(SimpleClass<TC1, TC2>)}.{nameof(ParmeterlessMethod)}";

    [TestMethod]
    public static string MultiParameterMethod(SimpleClass a, int b, List<int> c, Dictionary<SimpleClass, SimpleClass> d)
        => $"{nameof(SimpleClass<TC1, TC2>)}.{nameof(MultiParameterMethod)}";

    [TestMethod]
    public static string GenericParameterlessMethod<TM1, TM2>()
        => $"{nameof(SimpleClass<TC1, TC2>)}.{nameof(GenericParameterlessMethod)}<{typeof(TM1).Name},{typeof(TM2).Name}>";

    [TestMethod]
    public static string GenericMultiParameterMethod<TM1, TM2>(TM1 a, TM2 b, int c)
        => $"{nameof(SimpleClass<TC1, TC2>)}.{nameof(GenericMultiParameterMethod)}<{typeof(TM1).Name},{typeof(TM2).Name}>";

    [TestType]
    internal class NestedClass
    {
        [TestMethod]
        public static string ParmeterlessMethod()
                => $"{nameof(SimpleClass<TC1, TC2>)}.{nameof(SimpleClass)}.{nameof(ParmeterlessMethod)}";

        [TestMethod]
        public static string MultiParameterMethod(SimpleClass a, int b, List<int> c, Dictionary<SimpleClass, SimpleClass> d)
                => $"{nameof(SimpleClass<TC1, TC2>)}.{nameof(SimpleClass)}.{nameof(MultiParameterMethod)}";

        [TestMethod]
        public static string GenericParameterlessMethod<TM1, TM2>()
                => $"{nameof(SimpleClass<TC1, TC2>)}.{nameof(SimpleClass)}.{nameof(GenericParameterlessMethod)}<{typeof(TM1).Name},{typeof(TM2).Name}>";

        [TestMethod]
        public static string GenericMultiParameterMethod<TM1, TM2>(TM1 a, TM2 b, int c)
                => $"{nameof(SimpleClass<TC1, TC2>)}.{nameof(SimpleClass)}.{nameof(GenericMultiParameterMethod)}<{typeof(TM1).Name},{typeof(TM2).Name}>";
    }
}
