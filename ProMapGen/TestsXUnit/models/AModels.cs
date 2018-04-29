using System;
using System.Collections.Generic;
using System.Text;

namespace TestsXUnit.models
{
    public class ModelA1
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
    }

    public class ModelA2
    {
        public int InternalId { get; set; }
        public Guid Id { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }
}
