using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProMapGen.models.guessing
{
    public class Transformation
    {
        public MethodDeclarationSyntax MethodSyntax { get; set; }
        public string Comment { get; set; }
        public string MethodName { get; set; }
        public string InputType { get; set; }
        public string OutputType { get; set; }
    }
}
