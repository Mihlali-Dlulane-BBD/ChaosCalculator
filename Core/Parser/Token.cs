using ChaosCalculator.Core.Parser.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ChaosCalculator.Core.Parser
{
    public abstract record Token;

    public record NumberToken(double Value) : Token;
    public record OperatorToken(IOperator Operator) : Token;
    public record ParenthesisToken(char Symbol) : Token; // '(' or ')'
}
