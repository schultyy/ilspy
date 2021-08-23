using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

namespace ilspy
{
    static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ilspy");
            if (args.Length < 1)
            {
                Console.Error.WriteLine("Expected Assembly path. Got: None");
                Console.Error.WriteLine("Usage: ilspy <command> <args>");
                Console.Error.WriteLine("Commands:");
                Console.Error.WriteLine("types <assembly path> [<class.method>]");
                Console.Error.WriteLine("graph <assembly path> [class]");
                return;
            }

            if (args[0] == "types")
            {
                ShowTypes(args);
            }
            else if (args[0] == "graph")
            {
                var assemblyPath = args[1];
                String[] classesToInspect = new string[]{};

                if (args.Length > 2)
                {
                    classesToInspect = args.Skip(2)
                                            .ToArray();
                }
                BuildDependencyGraph(assemblyPath, classesToInspect);
            }
        }

        private static void BuildDependencyGraph(string assemblyPath, string[] classesToInspect)
        {
            var module = ModuleDefinition.ReadModule(assemblyPath);
            TypeDefinition[] allTypes;
            if (classesToInspect.Any())
            {
                allTypes = module.GetTypes()
                    .Where(t => classesToInspect.Contains(t.Name))
                    .ToArray();
            }
            else
            {
                allTypes = module.GetTypes()
                                .ToArray();
            }

            var nodes = new List<Node>();
            int id = 0;
            for(var classDefinitionIndex = 0; classDefinitionIndex < allTypes.Count(); classDefinitionIndex++)
            {
                var classDefinition = allTypes[classDefinitionIndex];
                var node = new Node(id++, classDefinition.Name);

                foreach (var methodDefinition in classDefinition.GetMethods())
                {

                    foreach (var instruction in methodDefinition.Body.Instructions.Where(instruction => instruction.OpCode == OpCodes.Call))
                    {
                        var type = instruction.Operand.GetType();
                        var operand = instruction.Operand;
                        if (type == typeof(Mono.Cecil.MethodDefinition))
                        {
                            node.Children.Add(new Node(id, ((MethodDefinition) operand).FullName));
                        }
                        else if (type == typeof(MethodReference))
                        {
                            node.Children.Add(new Node(id, ((MethodReference) operand).FullName));
                        }
                        else if (type == typeof(GenericInstanceMethod))
                        {
                            node.Children.Add(new Node(id, ((GenericInstanceMethod) operand).FullName));
                        }
                        else
                        {
                            node.Children.Add(new Node(id, operand.ToString()));
                        }

                        id++;
                    }
                }
                nodes.Add(node);
            }

            var dataSet = new DataSet(nodes);
            new GraphPlotter(dataSet).WriteToFile("output.html");
        }

        private static void ShowTypes(string[] args)
        {
            var assemblyPath = args[1];
            string className = null;
            string methodName = null;

            //Let's check if the user provided us with a class and method name to inspect
            if (args.Length > 2)
            {
                //We require the user to use a `Class.Method` notation
                className = args[2].Split(".")[0];
                methodName = args[2].Split(".")[1];
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
                PrintMethodSignature(methodType, classType);

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
            if (!methodDefinition.IsPrivate && !methodDefinition.IsPublic)
            {
                return "protected";
            }

            return "public";
        }

        private static void ShowAllTypes(ModuleDefinition module)
        {
            //Returns a list of all classes that are present in the assembly
            var allTypes = module.GetTypes();

            foreach (var classDefinition in allTypes)
            {
                Console.WriteLine($"{classDefinition.Name}");
                //`typeDefinition.GetMethods()` returns a list of all methods for this class
                foreach (var methodDefinition in classDefinition.GetMethods())
                {
                    PrintMethodSignature(methodDefinition, classDefinition);
                }
            }
        }

        private static void PrintMethodSignature(MethodDefinition methodDefinition, TypeDefinition classDefinition)
        {
            var parameters = methodDefinition.Parameters.Select(p => $"{p.ParameterType.Name} {p.Name}");
            var methodModifier = GetMethodAccessModifier(methodDefinition);
            var staticOrInstance = methodDefinition.IsStatic ? " static " : " ";
            Console.WriteLine(
                $"{methodModifier}{staticOrInstance}{classDefinition}.{methodDefinition.Name} ({String.Join(',', parameters)})");
        }
    }
}
