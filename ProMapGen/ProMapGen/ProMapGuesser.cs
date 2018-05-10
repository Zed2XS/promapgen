using Microsoft.CodeAnalysis.CSharp.Syntax;
using PropertyMappingGenerator.models.guessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PropertyMappingGenerator
{
    /// <summary>
    /// Property Mapping Guesser
    /// </summary>
    public class ProMapGuesser
    {
        public ClassPropertyMapper classPropertyMapper;

        public const string DEFAULT_NAMESPACE = "DTOMapper";
        public const string DEFAULT_CLASS_NAME = "Map";

        private string _mappingClassNamespace;
        private string _mappingClassName;
        private string _transformationClassPath;
        private List<PropertyMatch> _propertyMatches;
        private List<Transformation> _transformationList { get; set; }

        public ProMapGuesser(string transformationClassPath, string mappingClassNamespace = DEFAULT_NAMESPACE, string mappingClassName = DEFAULT_CLASS_NAME)
        {
            _transformationClassPath = transformationClassPath;
            _mappingClassNamespace = mappingClassNamespace;
            _mappingClassName = mappingClassName;
            classPropertyMapper = new ClassPropertyMapper();

            if(!File.Exists(_transformationClassPath))
            {
                if (string.IsNullOrWhiteSpace(_transformationClassPath))
                    throw new ArgumentException("The transformationClassPath must not be empty.");
                else
                    throw new ArgumentException($"The file referenced by the transformtionClassPath parameter \"{_transformationClassPath}\" does not exist.");
            }

            var transformWalker = new TransformMethodWalker();
            transformWalker.ParseCSharpFile(_transformationClassPath);
            _transformationList = transformWalker.TransformationList;
        }

        /// <summary>
        /// Parse File
        /// 
        /// Parse a C# formatted code file.
        /// </summary>
        /// <param name="filePath">Location of C# File</param>
        /// <returns>Class and Using Related Directives and Details</returns>
        public void ParseCSharpFile(string filePath)
        {
            classPropertyMapper.ParseCSharpFile(filePath);
        }

        /// <summary>
        /// Parse Text
        /// 
        /// Parse a string copy of C# formatted code.
        /// </summary>
        /// <param name="codeText">Code Text</param>
        /// <returns>Class and Using Related Directives and Details</returns>
        public void ParseCSharpText(string codeText)
        {
            classPropertyMapper.ParseCSharpText(codeText);
        }

        /// <summary>
        /// Get ClassParts List
        /// </summary>
        /// <returns>List of ClassPart Objects</returns>
        public List<ClassParts> GetClassPartsList()
        {
            return classPropertyMapper.classPartsList;
        }

        /// <summary>
        /// Get Using Syntaxes
        /// </summary>
        /// <returns>Dictionary of Using Syntaxes</returns>
        public Dictionary<string, UsingDirectiveSyntax> GetUsingSyntaxes()
        {
            return classPropertyMapper.usingSyntaxes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="classA"></param>
        /// <param name="classB"></param>
        /// <returns></returns>
        public ClassMap GuessMapping(ClassParts classA, ClassParts classB)
        {
            var map = new ClassMap()
            {
                ClassA = classA,
                ClassB = classB,
                PropertyBestMatches = new List<PropertyMatch>(),
                PropertyOtherMatches = new List<PropertyMatch>(),
                UnmmatchedClassAProperties = new List<Property>(),
                UnmmatchedClassBProperties = new List<Property>()
            };

            _propertyMatches = new List<PropertyMatch>();

            // get class details
            if (string.IsNullOrWhiteSpace(classA.ClassName) || string.IsNullOrWhiteSpace(classB.ClassName))
                throw new Exception("The provided class names must not be empty.");

            if (string.IsNullOrWhiteSpace(classA.NamespaceName) || string.IsNullOrWhiteSpace(classB.NamespaceName))
                throw new Exception("The provided class namespaces name must not be empty.");

            if (
                (classA.ClassName.Trim().ToUpper() == classB.ClassName.Trim().ToUpper())
                &&
                (classA.NamespaceName.Trim().ToUpper() == classB.NamespaceName.Trim().ToUpper())
               )
                throw new Exception("You can not map a class to itself");


            // make list of all matches
            var tmpMatches = new List<PropertyMatch>();
            foreach (var pA in classA.Properties)
            {
                foreach (var pB in classB.Properties)
                {
                    tmpMatches.Add(new PropertyMatch()
                    {
                        MatchScore = Score(pA, pB),
                        ClassAProperty = pA,
                        ClassBProperty = pB
                    });
                }
            }

            // sort matches
            _propertyMatches = tmpMatches.OrderByDescending(o => o.MatchScore).ToList();

            // move matches to map
            foreach (var m in _propertyMatches.OrderByDescending(o => o.MatchScore))
            {
                if (m.MatchScore > 0)
                {
                    var mapMatch = map.PropertyBestMatches.Where(w => w.Key == m.Key).SingleOrDefault();
                    var varMatchA = map.PropertyBestMatches.Where(w => w.ClassAProperty.Key == m.ClassAProperty.Key).SingleOrDefault();
                    var varMatchB = map.PropertyBestMatches.Where(w => w.ClassBProperty.Key == m.ClassBProperty.Key).SingleOrDefault();

                    if ((mapMatch == null) && (varMatchA == null) & (varMatchB == null))
                    {
                        // get transforms
                        var transformations = _transformationList.Where(w => w.OutputType == m.ClassAProperty.PropertyType && w.InputType == m.ClassBProperty.PropertyType).ToList();

                        if (transformations != null && transformations.Count > 0)
                        {
                            // when the intput and output type are the same then any transforms that match are always optional
                            if (transformations.Count == 1 && m.ClassAProperty.PropertyType != m.ClassBProperty.PropertyType)
                                m.Transform = transformations[0]; 
                            else
                                m.PotentialTransforms = transformations;
                        }

                        map.PropertyBestMatches.Add(m);
                    }
                    else
                    {
                        map.PropertyOtherMatches.Add(m);
                    }
                }
            }

            // put unmatched variables in map
            foreach (var pA in classA.Properties)
            {
                var varMatch = map.PropertyBestMatches.Where(w => w.ClassAProperty.Key == pA.Key).SingleOrDefault();

                if (varMatch == null)
                    map.UnmmatchedClassAProperties.Add(pA);
            }

            foreach (var pB in classB.Properties)
            {
                var varMatch = map.PropertyBestMatches.Where(w => w.ClassBProperty.Key == pB.Key).SingleOrDefault();

                if (varMatch == null)
                    map.UnmmatchedClassBProperties.Add(pB);
            }

            return map;
        }

        /// <summary>
        /// 
        /// </summary>
        private void Generate()
        {
            //if (!string.IsNullOrWhiteSpace(_mappingClassNamespace))
            //    _mappingClassNamespace = DEFAULT_NAMESPACE;

            //if (!string.IsNullOrWhiteSpace(_mappingClassName))
            //    _mappingClassName = DEFAULT_CLASS_NAME;

            //var @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(_mappingClassNamespace.Trim())).NormalizeWhitespace();

            //foreach (var u in _details.UsingDirectives)
            //    @namespace = @namespace.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(u.Value.Text)));

            //var @class = SyntaxFactory.ClassDeclaration(_mappingClassName);

            //@class = @class.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        private int Score(Property p1, Property p2)
        {
            var score = 0;

            if ((p1.PropertyName == p2.PropertyName) && (p1.PropertyType == p2.PropertyType))
            {
                // perfect match!
                score = 100;
            }
            else if (p1.PropertyName == p2.PropertyName)
            {
                // name is a perfect match, but type is different
                score = 90;
            }
            else
            {
                // split on case changes and non-alpha characters and compare parts
                score = CalculateVariableNamePartsScore(p1.PropertyName, p2.PropertyName);
            }

            return score;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        private static int CalculateVariableNamePartsScore(string val1, string val2)
        {
            var s1 = CamelCaseSplitter(val1);
            var s2 = CamelCaseSplitter(val2);

            var score = 0.0;
            var maxScore = s1.Length > s2.Length ? (double)s1.Length : (double)s2.Length;

            foreach (var a in s1)
            {
                foreach (var b in s2)
                {
                    if (a == b)
                        score = score + 1;
                    else if (a.ToUpper() == b.ToUpper())
                        score = score + 0.75;
                    else if (a.ToUpper().Contains(b.ToUpper()) || b.ToUpper().Contains(a.ToUpper()))
                        score = score + 0.5;
                }
            }

            return (int)Math.Round((score / maxScore) * (double)100);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="variableName"></param>
        /// <returns></returns>
        private static string[] CamelCaseSplitter(String variableName)
        {
            if (!String.IsNullOrEmpty(variableName))
            {
                //var strRegex = @"(?<=[a-z])([A-Z])|(?<=[A-Z])([A-Z][a-z])";
                var strRegex = @"(?=\p{Lu}\p{Ll})|(?<=\p{Ll})(?=\p{Lu})";
                var myRegex = new Regex(strRegex, RegexOptions.None);
                var strTargetString = variableName;
                //var strReplace = @" $1$2";
                var strReplace = " ";

                var results = myRegex.Replace(strTargetString, strReplace).Split(null);

                // remove empty strings
                var list = new List<string>();

                foreach (var i in results)
                    if (!string.IsNullOrWhiteSpace(i))
                    {
                        Regex rgx = new Regex("[^a-zA-Z0-9]");
                        list.Add(rgx.Replace(i, ""));
                    }

                return list.ToArray();
            }
            else
            {
                return null;
            }
        }
    }

    // https://carlos.mendible.com/2017/03/02/create-a-class-with-net-core-and-roslyn/
}
