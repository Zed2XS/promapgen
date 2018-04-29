using System;
using System.Collections.Generic;
using System.Text;

namespace TestsXUnit.models
{
    public class ModelB1
    {
        public const char CONSTANT = 'A';
        public static char STATIC = 'B';
        public readonly char READONLY = 'C';
        public int Id { get; set; }
        public string GivenName { get; set; }
    }

    public class ModelB2
    {
        public Guid Id { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string City { get; set; }
        public string USPostalStateCode { get; set; }
        public string PostalCode { get; set; }
        public string PostalCodePlusFour { get; set; }
    }
}
