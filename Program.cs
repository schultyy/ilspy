using System;
using Mono.Cecil;
using Mono.Cecil.Rocks;

namespace ilspy
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ilspy");
            if (args.Length < 1)
            {
                Console.Error.WriteLine("Expected Assembly path. Got: None");
                Console.Error.WriteLine("Usage: ilspy <assembly path> [<class.method>]");
                return;
            }

            var assemblyPath = args[0];

            var module = ModuleDefinition.ReadModule(assemblyPath);
            ShowAllTypes(module);
        }

        private static void ShowAllTypes(ModuleDefinition module)
        {
            //Returns a list of all classes that are present in the assembly
            var allTypes = module.GetTypes();

            foreach (var typeDefinition in allTypes)
            {
                Console.WriteLine($"{typeDefinition.Name}");
                //`typeDefinition.GetMethods()` returns a list of all methods for this class
                foreach (var methodDefinition in typeDefinition.GetMethods())
                {
                    Console.WriteLine($"{typeDefinition}.{methodDefinition.Name}");
                }
            }
        }
    }
}
