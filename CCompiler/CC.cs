using System;
using System.Collections.Generic;
using CCompiler.AbstractSyntaxTree;

namespace CCompiler
{
  public static class CC
  {

    public static Program Parse(List<Token> list)
    {
      if (list == null)
        throw new ArgumentNullException($"{nameof(list)} is Null");

      if (list.Count == 0)
      {
        return new Program();
      }

      var enumerator = list.GetEnumerator();
      enumerator.MoveNext();

      var program = new Program(ParseFunctions(enumerator));

      return program;
    }

    private static List<Function> ParseFunctions(IEnumerator<Token> enumerator)
    {
      var ret = new List<Function>();
      string name;

      var t = enumerator.Current;
      if (!Equals(t, new Token("int", TokenType.Keyword)))
      {
        throw new Exception("Return must be keyword!");
      }

      t = GetNext(enumerator);
      if (t.type == TokenType.Identifier)
      {
        name = t.text;
      }
      else
      {
        throw new Exception("Function name must be an identifier");
      }

      t = GetNext(enumerator);
      if (t.type != TokenType.OpenParen)
      {
        throw new Exception("");
      }

      t = GetNext(enumerator);
      if (t.type != TokenType.CloseParen)
      {
        throw new Exception("");
      }


      var p = ParseStatements(enumerator);
      ret.Add(new Function(name, p));

      return ret;
    }

    private static List<Statement> ParseStatements(IEnumerator<Token> enumerator)
    {
      var ret = new List<Statement>();
      var t = GetNext(enumerator);
      if (t.type != TokenType.OpenBrace)
      {
        throw new Exception("Open brace not found");
      }

      t = GetNext(enumerator);
      if (t.type == TokenType.Keyword && t.text == "return")
      {
        ret.Add(new Statement(ParseExpression(enumerator)));
      }
      else
      {
        throw new Exception("Not a recognized keyword");
      }

      if (GetNext(enumerator).type != TokenType.Semicolon)
      {
        throw new Exception("Missing semicolon at statement end");
      }

      t = GetNext(enumerator);
      if (t.type != TokenType.CloseBrace)
      {
        throw new Exception("Closing brace not found");
      }

      return ret;
    }

    private static Expression ParseExpression(IEnumerator<Token> enumerator)
    {
      var t = GetNext(enumerator);
      try
      {
        switch (t.type)
        {
          case TokenType.Integer:
            return new Constant(int.Parse(t.text));
          case TokenType.Negation:
            return UnaryOp.Negation(ParseExpression(enumerator));
          default:
            throw new Exception("Bad Expression");
        }
      }
      catch (FormatException)
      {
        throw new Exception("Bad numeric");
      }
    }

    /// Modifies the enumerator!
    private static Token GetNext(IEnumerator<Token> enumerator)
    {
      if (enumerator.MoveNext())
      {
        return enumerator.Current;
      }
      else
      {
        throw new Exception("Unexpected end of tokens");
      }
    }

    public static string Generate(Program p)
    {
      var generate = new Generater(p);
      return generate.Generate();
    }

    public static Program LexAndParse(string source)
    {
      var tokens = Lexer.LexString(source);
      return Parse(tokens);
    }
  }
}
