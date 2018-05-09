using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ProMapGen.models.guessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ProMapGen
{
    /// <summary>
    /// Class Property Walker
    /// 
    /// Walk C# formatted code and return the parts needed to map them tegether. 
    /// </summary>
    public class ClassPropertyMapper : CSharpSyntaxWalker
    {
        public const string PUBLIC_KEYWORD = "public";
        public const string USING_KEYWORD = "using";
        public List<ClassMap> classPropertyMaps = new List<ClassMap>();
        public Dictionary<string, UsingDirectiveSyntax> usingSyntaxes = new Dictionary<string, UsingDirectiveSyntax>();
        public List<ClassParts> classPartsList = new List<ClassParts>();

        public ClassPropertyMapper() : base(SyntaxWalkerDepth.StructuredTrivia)
        {
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
            var codeText = File.ReadAllText(filePath);
            ParseCSharpText(codeText);
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
            Visit(CSharpSyntaxTree.ParseText(codeText).GetRoot());
        }

        /// <summary>
        /// Visit Using Directives
        /// 
        /// This method is called when a "using" directive is found.
        /// </summary>
        /// <param name="node">Using Directive</param>
        public override void VisitUsingDirective(UsingDirectiveSyntax node)
        {
            var namespaceText = node.ToString();

            if (!string.IsNullOrWhiteSpace(namespaceText))
            {
                namespaceText = Regex.Replace(namespaceText, USING_KEYWORD, "", RegexOptions.IgnoreCase).Replace(";", "").Trim();
                var key = namespaceText.ToLower();

                if (!usingSyntaxes.ContainsKey(key))
                    usingSyntaxes.Add(key, node);
            }

            base.VisitUsingDirective(node);
        }

        /// <summary>
        /// Visit Class Declaration
        /// 
        /// This method is called when the node being visited is a class.
        /// </summary>
        /// <param name="node">Class Syntax</param>
        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            // get namespace text
            string nameSpaceText = null;
            NamespaceDeclarationSyntax namespaceDeclarationSyntax = null;
            if (SyntaxNodeHelper.TryGetParentSyntax(node, out namespaceDeclarationSyntax))
                nameSpaceText = namespaceDeclarationSyntax.Name.ToString();
            else
                nameSpaceText = string.Empty;

            // get class text
            var classText = node.Identifier.ToString();

            var classParts = new ClassParts()
            {
                NamespaceName = nameSpaceText,
                NamespaceSyntax = namespaceDeclarationSyntax,
                ClassName = classText,
                ClassSyntax = node
            };

            classPartsList.Add(classParts);

            base.VisitClassDeclaration(node);
        }

        /// <summary>
        /// Visit Property Decleration
        /// 
        /// This method is called when the node being visited is a public property (a variable that has a { get; set; }"
        /// </summary>
        /// <param name="node">Property Syntax Node</param>
        public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            if (classPartsList.Count > 0)
            {
                // get last element
                var classParts = classPartsList[classPartsList.Count - 1];

                // add property to the list
                if (classParts.Properties == null)
                    classParts.Properties = new List<Property>();

                // check modifiers to see if the property is public
                foreach (var m in node.Modifiers)
                {
                    // only public properties can be mapped
                    if (m.Text.ToLower() == PUBLIC_KEYWORD)
                    {
                        // add info to list
                        classParts.Properties.Add(new Property()
                        {
                            PropertyName = node.Identifier.ToString(),
                            PropertyType = node.Type.ToString(),
                            PropertySyntax = node
                        });
                        break;
                    }
                }
            }

            base.VisitPropertyDeclaration(node);
        }
    }
}
