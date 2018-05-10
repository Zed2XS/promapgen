using System;
using System.Collections.Generic;
using System.Text;

namespace PropertyMappingGenerator.models
{
    /// <summary>
    /// Map Config
    /// 
    /// Class for deserializing the configuration from JSON.
    /// </summary>
    public class MapConfig
    {
        /// <summary>
        /// Using Namespaces
        /// 
        /// A list of using statement namespaces needed by the mapper.
        /// </summary>
        public List<string> UsingNamespaces { get; set; }

        /// <summary>
        /// Namespace
        /// 
        /// The namespace to put the mapper class in.
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// Transform Methods Class Path
        /// 
        /// Path to the .cs file that contains the transform methods.
        /// </summary>
        public string TransformMethodsClassPath { get; set; }

        /// <summary>
        /// Maps
        /// 
        /// Property mappings.
        /// </summary>
        public List<MapClassConfig> Maps { get; set; }
    }

    /// <summary>
    /// Map Class Config
    /// 
    /// Class for deserializing the Maps from the configuration JSON.
    /// </summary>
    public class MapClassConfig
    {
        /// <summary>
        /// Class A Name
        /// </summary>
        public string ClassAName { get; set; }

        /// <summary>
        /// Class A File Path
        /// </summary>
        public string ClassAFilePath { get; set; }

        /// <summary>
        /// Class B Name
        /// </summary>
        public string ClassBName { get; set; }

        /// <summary>
        /// Class B File Path
        /// </summary>
        public string ClassBFilePath { get; set; }

        /// <summary>
        /// A B Property Mappings
        /// 
        /// Mapping between properties from class A to class B. 
        /// </summary>
        public List<MapProperty> ABPropertyMappings { get; set; }

        /// <summary>
        /// B A Property Mappings
        /// 
        /// Mapping between properties from class B to class A. 
        /// </summary>

        public List<MapProperty> BAPropertyMappings { get; set; }
    }

    /// <summary>
    /// Map Property
    /// </summary>
    public class MapProperty
    {
        /// <summary>
        /// Property Name Destination
        /// </summary>
        public string PropertyNameDestination { get; set; }

        /// <summary>
        /// Property Type Destination
        /// </summary>
        public string PropertyTypeDestination { get; set; }

        /// <summary>
        /// Property Name Source
        /// </summary>
        public string PropertyNameSource { get; set; }

        /// <summary>
        /// Property Type Source
        /// </summary>
        public string PropertyTypeSource { get; set; }

        /// <summary>
        /// Transform Method Name
        /// </summary>
        public string TransformMethodName { get; set; }
    }
}
