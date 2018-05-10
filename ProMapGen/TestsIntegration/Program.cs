using PropertyMappingGenerator;
using System;

namespace TestsIntegration
{
    class Program
    {
        public const string AMODELS_PATH = @"../TestsXUnit/models/AModels.cs";
        public const string BMODELS_PATH = @"../TestsXunit/models/BModels.cs";
        public const string TRANSFORMATION_CLASS_FILE_PATH = @"../TestsXunit/transforms/Transforms.cs";

        static void Main(string[] args)
        {
            var mapper = new PropertyMappingGenerator.ProMapGuesser(TRANSFORMATION_CLASS_FILE_PATH);
            mapper.ParseCSharpFile(AMODELS_PATH);
            mapper.ParseCSharpFile(BMODELS_PATH);

            foreach (var key in mapper.GetUsingSyntaxes().Keys)
            {
                Console.WriteLine(mapper.GetUsingSyntaxes()[key]);
            }

            Console.WriteLine();

            for (var i = 0; i < mapper.GetClassPartsList().Count; i++)
            {
                var c1 = mapper.GetClassPartsList()[i];

                for (var j = 0; j < mapper.GetClassPartsList().Count; j++)
                {
                    if (i != j)
                    {
                        var c2 = mapper.GetClassPartsList()[j];

                        var classMap = mapper.GuessMapping(c1, c2);

                        Console.WriteLine("====================================================================================");

                        Console.WriteLine(c1.ClassSyntax.GetText());
                        Console.WriteLine(c2.ClassSyntax.GetText());

                        if (classMap.PropertyBestMatches != null)
                        {
                            Console.WriteLine("Mapped");

                            foreach (var c in classMap.PropertyBestMatches)
                            {
                                Console.WriteLine("\t" + c.ClassAProperty.PropertyType + " " + c.ClassAProperty.PropertyName + " = " + c.ClassBProperty.PropertyType + " " + c.ClassBProperty.PropertyName + " " + c.MatchScore);

                                if (c.Transform != null)
                                    Console.WriteLine("\t\tTransform Method: " + c.Transform.MethodName);

                                if (c.PotentialTransforms != null)
                                    foreach (var t in c.PotentialTransforms)
                                        Console.WriteLine("\t\tPotential Transform Method: " + t.MethodName);
                            }
                        }

                        if (classMap.PropertyOtherMatches != null)
                        {
                            Console.WriteLine("Other Matches");

                            foreach (var c in classMap.PropertyOtherMatches)
                            {
                                Console.WriteLine("\t" + c.ClassAProperty.PropertyType + " " + c.ClassAProperty.PropertyName + " = " + c.ClassBProperty.PropertyType + " " + c.ClassBProperty.PropertyName + " " + c.MatchScore);
                            }
                        }

                        if (classMap.UnmmatchedClassAProperties != null)
                        {
                            Console.WriteLine("Class A Unmapped");

                            foreach (var x in classMap.UnmmatchedClassAProperties)
                            {
                                Console.WriteLine("\t" + x.PropertyType + " " + x.PropertyName);
                            }
                        }

                        if (classMap.UnmmatchedClassAProperties != null)
                        {
                            Console.WriteLine("Class B Unmapped");

                            foreach (var x in classMap.UnmmatchedClassBProperties)
                            {
                                Console.WriteLine("\t" + x.PropertyType + " " + x.PropertyName);
                            }
                        }
                    }
                }

                Console.WriteLine("\n\n\n");
            }



            Console.ReadKey();
        }
    }
}
