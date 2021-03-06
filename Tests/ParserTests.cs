using System;
using System.Collections.Generic;
using CCompiler;
using CCompiler.AbstractSyntaxTree;
using Xunit;
using FluentAssertions;
using static CCompiler.TokenType;

namespace Tests
{
  public class ParserTests
  {
    [Fact]
    public void EmptyIsValid() =>
      Assert.Equal(CC.Parse(new List<Token>()), new Program());

    [Fact]
    public void NullThrows() =>
      Assert.Throws<ArgumentNullException>(() => CC.Parse(null));

    [Fact]
    public void MainReturn2Parse()
    {
      var expectedProgram =
        new Program(
          new Function("main", new Statement(new Constant(2))));

      var tokens = new TokenList
      {
        {"int", Keyword},
        {"main", Identifier},
        {"(", OpenParen},
        {")", CloseParen},
        {"{", OpenBrace},
        {"return", Keyword},
        {"2", Integer},
        {";", Semicolon},
        {"}", CloseBrace},
      };

      var actualProgram = CC.Parse(tokens);

      expectedProgram.Should().Be(actualProgram);
    }

    [Fact]
    public void ParseNegation()
    {
      var expectedProgram =
        new Program(
          new Function("main", new Statement(new UnaryOp(UnaryOp.Type.Negation, new Constant(2)))));

      var tokens = new TokenList
      {
        {"int", Keyword},
        {"main", Identifier},
        {"(", OpenParen},
        {")", CloseParen},
        {"{", OpenBrace},
        {"return", Keyword},
        {"-", Negation},
        {"2", Integer},
        {";", Semicolon},
        {"}", CloseBrace},
      };

      var actualProgram = CC.Parse(tokens);

      expectedProgram.Should().Be(actualProgram);

    }
  }
}
