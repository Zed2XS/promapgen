using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ProMapGen.models.guessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ProMapGen
{
    /// <summary>
    /// Transform Method Walker
    /// </summary>
    public class TransformMethodWalker : CSharpSyntaxWalker
    {
        public List<Transformation> TransformationList { get; set; }

        public TransformMethodWalker() : base(SyntaxWalkerDepth.StructuredTrivia)
        {
            TransformationList = new List<Transformation>();
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

        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            // must be public and static
            var isPublic = false;
            var isStatic = false;

            foreach (var m in node.Modifiers)
            {
                if (m.Text.ToLower() == "public")
                    isPublic = true;

                if (m.Text.ToLower() == "static")
                    isStatic = true;
            }

            if (isPublic && isStatic)
            {
                var t = new Transformation
                {
                    Comment = node.GetLeadingTrivia().ToString(),
                    MethodSyntax = node,
                    MethodName = node.Identifier.Text,
                    OutputType = node.ReturnType.ToString()
                };

                if (node.ParameterList.Parameters.Count > 0)
                    t.InputType = node.ParameterList.Parameters[0].Type.ToString();

                TransformationList.Add(t);
            }

            base.VisitMethodDeclaration(node);
        }
    }
}
