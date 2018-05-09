using System;
using System.Collections.Generic;
using System.Text;

namespace ProMapGen.models.guessing
{
    public class PropertyMatch
    {
        public Property ClassAProperty { get; set; }
        public Property ClassBProperty { get; set; }
        public Transformation Transform { get; set; }
        public List<Transformation> PotentialTransforms { get; set; }
        public int MatchScore { get; set; }
        public string Key
        {
            get
            {
                return ClassAProperty.Key + "=" + ClassBProperty.Key;
            }

            set
            {
                // do nothing
            }
        }
    }
}
