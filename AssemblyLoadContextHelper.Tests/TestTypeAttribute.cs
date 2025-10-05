using System;

namespace AssemblyLoadContextHelper.Tests;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum)]
internal class TestTypeAttribute : Attribute;
