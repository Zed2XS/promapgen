using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProMapGen.models.guessing
{
    public class ClassParts
    {
        public string NamespaceName { get; set; }
        public string ClassName { get; set; }
        public List<Property> Properties { get; set; }
        public NamespaceDeclarationSyntax NamespaceSyntax { get; set; }
        public ClassDeclarationSyntax ClassSyntax { get; set; }
    }
}
