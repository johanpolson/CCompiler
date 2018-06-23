using System;
using System.Linq;
using System.Collections.Generic;

namespace CCompiler.AbstractSyntaxTree
{
  public class Program
  {
    public readonly List<Function> functionList = new List<Function>();

    public Program()
    {
    }

    public Program(Function function) =>
      this.functionList.Add(function);

    public Program(List<Function> functionList) =>
      this.functionList.AddRange(functionList);

    public override bool Equals(object obj) =>
      obj is Program program && functionList.SequenceEqual(program.functionList);

    public override int GetHashCode() =>
      HashCode.Combine(functionList);
  }
}
