using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProMapGen.models
{
    public class Property
    {
        public string Key
        {
            get
            {
                if ((!string.IsNullOrWhiteSpace(PropertyName)) && (!string.IsNullOrWhiteSpace(PropertyType)))
                    return PropertyType + ":" + PropertyName;
                else if ((!string.IsNullOrWhiteSpace(PropertyName)) && string.IsNullOrWhiteSpace(PropertyType))
                    return PropertyName;
                else if (string.IsNullOrWhiteSpace(PropertyName) && (!string.IsNullOrWhiteSpace(PropertyType)))
                    return PropertyType;
                else
                    return string.Empty;
            }

            set
            {
                // do nothing
            }
        }

        public string PropertyName { get; set; }
        public string PropertyType { get; set; }
        public PropertyDeclarationSyntax PropertySyntax { get; set; }
    }
}
