using System;
using System.Collections.Generic;
using System.Text;

namespace ProMapGen.models.guessing
{
    public class ClassMap
    {
        public ClassParts ClassA { get; set; }
        public ClassParts ClassB { get; set; }
        public List<PropertyMatch> PropertyBestMatches { get; set; }
        public List<PropertyMatch> PropertyOtherMatches { get; set; }
        public List<Property> UnmmatchedClassAProperties { get; set; }
        public List<Property> UnmmatchedClassBProperties { get; set; }
    }
}
