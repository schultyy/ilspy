using System;
using System.Linq;
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
            string className = null;
            string methodName = null;

            //Let's check if the user provided us with a class and method name to inspect
            if (args.Length > 1)
            {
                //We require the user to use a `Class.Method` notation
                className = args[1].Split(".")[0];
                methodName = args[1].Split(".")[1];
            }

            var module = ModuleDefinition.ReadModule(assemblyPath);

            //Check if we received class and method name
            if (String.IsNullOrEmpty(className) && string.IsNullOrEmpty(methodName))
            {
                //If not, we just display all types as usual
                ShowAllTypes(module);
            }
            else
            {
                //Show details for this specific type
                ShowSpecificType(module, className, methodName);
            }
        }

        private static void ShowSpecificType(ModuleDefinition module, string className, string methodName)
        {
            //We find the specific class by its name
            var classType = module.GetTypes().FirstOrDefault(t => t.Name == className);
            var methodType = classType.GetMethods().FirstOrDefault(m => m.Name == methodName);
            if (methodType != null)
            {
                var returnType = methodType.ReturnType.Name.ToLower();
                var accessModifier = GetMethodAccessModifier(methodType);
                var parameters = methodType.Parameters
                    .Select(p => p.ParameterType.Name)
                    .ToArray();
                Console.WriteLine($"{accessModifier} {returnType} {methodName} ({String.Join(',', parameters)})");

                //Print the method body's instructions
                foreach (var instruction in methodType.Body.Instructions)
                {
                    Console.WriteLine($"{instruction}");
                }
            }
            else
            {
                Console.Error.WriteLine("Couldn't find method definition");
            }
        }

        private static string GetMethodAccessModifier(MethodDefinition methodDefinition)
        {
            if (methodDefinition.IsPrivate && !methodDefinition.IsPublic)
            {
                return "private";
            }

            return "public";
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
                    var parameters = methodDefinition.Parameters.Select(p => $"{p.ParameterType.Name} {p.Name}");
                    Console.WriteLine($"{typeDefinition}.{methodDefinition.Name} ({String.Join(',', parameters)})");
                }
            }
        }
    }
}
