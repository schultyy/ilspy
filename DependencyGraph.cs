using System;
using System.Collections.Generic;

namespace ilspy
{
    public class DependencyGraph
    {
        public String RootNode { get; private set; }

        public List<string> Dependencies { get; private set; }

        public DependencyGraph(String rootNode)
        {
            Dependencies = new List<string>();
            RootNode = rootNode;
        }

        public void AddDependency(String dependencyTypeName)
        {
            if (dependencyTypeName == null)
                throw new ArgumentException("Expected string, got null");

            if (Dependencies.Contains(dependencyTypeName))
                return;

            Dependencies.Add(dependencyTypeName);
        }
    }
}