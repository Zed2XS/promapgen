using Newtonsoft.Json;
using PropertyMappingGenerator.models;
using PropertyMappingGenerator.models.guessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PropertyMappingGenerator
{
    /// <summary>
    /// Property Mapping Generator
    /// </summary>
    public class ProMapGen
    {
        private TransformMethodWalker _transformMethodWalker;
        public MapConfig ClassMapConfig { get; set; }
        public List<Transformation> TransformationList { get; set; }

        /// <summary>
        /// ProMapGen
        /// 
        /// The class file is the C# .cs source file.
        /// </summary>
        /// <param name="transformationsClassFilePath">Transformation Class File Path</param>
        public ProMapGen(string transformationsClassFilePath, string configurationJsonPath)
        {
            _transformMethodWalker = new TransformMethodWalker();
            _transformMethodWalker.ParseCSharpFile(transformationsClassFilePath);
            TransformationList = _transformMethodWalker.TransformationList;

            LoadJsonConfiguration(configurationJsonPath);
        }

        /// <summary>
        /// Load JSON Configuration
        /// 
        /// Load a JSON class map configuration file and deserialize it into ClassMapConfig.
        /// </summary>
        /// <param name="path"></param>        
        /// <returns>True if Deserializing Successed</returns>
        private bool LoadJsonConfiguration(string path)
        {
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                _classMapConfig = JsonConvert.DeserializeObject<MapConfig>(json);
                return true;
            }
            else
            {
                throw new FileNotFoundException($"{path} does not exist.");
            }
        }
    }
}
