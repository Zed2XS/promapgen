using PropertyMappingGenerator;
using System;
using Xunit;

namespace TestsXUnit
{
    public class UnitTests
    {
        public const string AMODELS_PATH = @"models/AModels.cs";
        public const string BMODELS_PATH = @"models/BModels.cs";
        public const string CONFIG_PATH = @"configs/SampleMapConfig.json";

        [Fact]
        public void DeserializeJSONConfig()
        {
            var pmg = new ProMapGen();

            Assert.True(pmg.LoadJsonConfiguration(CONFIG_PATH));

            Assert.NotNull(pmg.ClassMapConfig);

            Assert.Equal("sample.mapping.namespace", pmg.ClassMapConfig.Namespace);
            Assert.Equal("..\\..\\Transformations.cs", pmg.ClassMapConfig.TransformMethodsClassPath);

            Assert.Equal(3, pmg.ClassMapConfig.UsingNamespaces.Count);
            Assert.Equal("System", pmg.ClassMapConfig.UsingNamespaces[0]);
            Assert.Equal("System.Collections.Generic", pmg.ClassMapConfig.UsingNamespaces[1]);
            Assert.Equal("System.Text", pmg.ClassMapConfig.UsingNamespaces[2]);
            
            Assert.NotNull(pmg.ClassMapConfig.Maps);
            Assert.Single(pmg.ClassMapConfig.Maps);

            Assert.Equal("ClassA", pmg.ClassMapConfig.Maps[0].ClassAName);
            Assert.Equal("models\\ClassA.cs", pmg.ClassMapConfig.Maps[0].ClassAFilePath);
            Assert.Equal("ClassB", pmg.ClassMapConfig.Maps[0].ClassBName);
            Assert.Equal("models\\ClassB.cs", pmg.ClassMapConfig.Maps[0].ClassBFilePath);

            Assert.NotNull(pmg.ClassMapConfig.Maps[0].ABPropertyMappings);
            Assert.Equal(2, pmg.ClassMapConfig.Maps[0].ABPropertyMappings.Count);
            Assert.Equal("aVar1", pmg.ClassMapConfig.Maps[0].ABPropertyMappings[0].PropertyNameSource);
            Assert.Equal("float", pmg.ClassMapConfig.Maps[0].ABPropertyMappings[0].PropertyTypeSource);
            Assert.Equal("bVar1", pmg.ClassMapConfig.Maps[0].ABPropertyMappings[0].PropertyNameDestination);
            Assert.Equal("double", pmg.ClassMapConfig.Maps[0].ABPropertyMappings[0].PropertyTypeDestination);
            Assert.Equal("FromFloatToDouble", pmg.ClassMapConfig.Maps[0].ABPropertyMappings[0].TransformMethodName);

            Assert.Equal("aVar2", pmg.ClassMapConfig.Maps[0].ABPropertyMappings[1].PropertyNameSource);
            Assert.Equal("string", pmg.ClassMapConfig.Maps[0].ABPropertyMappings[1].PropertyTypeSource);
            Assert.Equal("bVar2", pmg.ClassMapConfig.Maps[0].ABPropertyMappings[1].PropertyNameDestination);
            Assert.Equal("string", pmg.ClassMapConfig.Maps[0].ABPropertyMappings[1].PropertyTypeDestination);
            Assert.Null(pmg.ClassMapConfig.Maps[0].ABPropertyMappings[1].TransformMethodName);

            Assert.NotNull(pmg.ClassMapConfig.Maps[0].BAPropertyMappings);
            Assert.Equal(2, pmg.ClassMapConfig.Maps[0].BAPropertyMappings.Count);
            Assert.Equal("bVar1", pmg.ClassMapConfig.Maps[0].BAPropertyMappings[0].PropertyNameSource);
            Assert.Equal("double", pmg.ClassMapConfig.Maps[0].BAPropertyMappings[0].PropertyTypeSource);
            Assert.Equal("aVar1", pmg.ClassMapConfig.Maps[0].BAPropertyMappings[0].PropertyNameDestination);
            Assert.Equal("float", pmg.ClassMapConfig.Maps[0].BAPropertyMappings[0].PropertyTypeDestination);
            Assert.Equal("FromDoubleToFloat", pmg.ClassMapConfig.Maps[0].BAPropertyMappings[0].TransformMethodName);

            Assert.Equal("bVar2", pmg.ClassMapConfig.Maps[0].BAPropertyMappings[1].PropertyNameSource);
            Assert.Equal("string", pmg.ClassMapConfig.Maps[0].BAPropertyMappings[1].PropertyTypeSource);
            Assert.Equal("aVar2", pmg.ClassMapConfig.Maps[0].BAPropertyMappings[1].PropertyNameDestination);
            Assert.Equal("string", pmg.ClassMapConfig.Maps[0].BAPropertyMappings[1].PropertyTypeDestination);
            Assert.Null(pmg.ClassMapConfig.Maps[0].BAPropertyMappings[1].TransformMethodName);
        }
    }
}
