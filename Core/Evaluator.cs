using ChaosCalculator.Core.Parser;
using ChaosCalculator.Core.Parser.Interface;

namespace ChaosCalculator.Core
{
    public class Evaluator
    {
        private static void ApplyTopOperator(Stack<double> values, Stack<IOperator> operators)
        {
            IOperator op = operators.Pop();
            double right = values.Pop();
            double left = values.Pop();

            values.Push(op.Calculate(left, right));
        }

        public double Evaluate(List<Token> tokens)
        {
            var values = new Stack<double>();
            var operators = new Stack<IOperator>();

            foreach (var token in tokens)
            {
                switch (token)
                {
                    case NumberToken num:
                        values.Push(num.Value);
                        break;

                    case ParenthesisToken paren when paren.Symbol == '(':
                        operators.Push(new ParenthesisBoundary());
                        break;

                    case ParenthesisToken paren when paren.Symbol == ')':
                        while (operators.Count > 0 && operators.Peek() is not ParenthesisBoundary)
                        {
                            ApplyTopOperator(values, operators);
                        }
                        operators.Pop(); // Remove the boundary
                        break;

                    case OperatorToken opToken:
                        while (operators.Count > 0 &&
                               operators.Peek() is not ParenthesisBoundary &&
                               operators.Peek().Precedence >= opToken.Operator.Precedence)
                        {
                            ApplyTopOperator(values, operators);
                        }
                        operators.Push(opToken.Operator);
                        break;
                }
            }

            while (operators.Count > 0)
            {
                ApplyTopOperator(values, operators);
            }

            return values.Pop();
        }

        // A simple boundary marker for the stack
        private class ParenthesisBoundary : IOperator
        {
            public char Symbol => '(';
            public int Precedence => -1; 
            public double Calculate(double left, double right) => throw new NotImplementedException();
        }
    }
}
