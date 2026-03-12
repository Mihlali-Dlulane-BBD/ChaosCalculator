using ChaosCalculator.Core.Parser.Interface;
using System.Globalization;
using System.Text;

namespace ChaosCalculator.Core.Parser
{
    public class Tokenizer
    {
        private readonly OperatorRegistry _registry;

        public Tokenizer(OperatorRegistry registry)
        {
            _registry = registry;
        }

        public List<Token> Tokenize(string expression)
        {
            var tokens = new List<Token>();
            int i = 0;

            bool expectOperand = true;

            while (i < expression.Length)
            {
                char c = expression[i];

                if (char.IsWhiteSpace(c))
                {
                    i++;
                    continue;
                }

                
                if (char.IsDigit(c) || c == '.')
                {
                    StringBuilder numBuilder = new StringBuilder();
                    while (i < expression.Length && (char.IsDigit(expression[i]) || expression[i] == '.'))
                    {
                        numBuilder.Append(expression[i]);
                        i++;
                    }

                    double value = double.Parse(numBuilder.ToString(), CultureInfo.InvariantCulture);
                    tokens.Add(new NumberToken(value));

                    expectOperand = false; 
                    continue;
                }

               
                if (c == '(')
                {
                    tokens.Add(new ParenthesisToken(c));
                    expectOperand = true; 
                    i++;
                    continue;
                }

                if (c == ')')
                {
                    tokens.Add(new ParenthesisToken(c));
                    expectOperand = false; 
                    i++;
                    continue;
                }

            
                if (expectOperand)
                {
                    if (c == '-')
                    {
                        tokens.Add(new NumberToken(-1));

                        if (_registry.TryGetOperator('*', out IOperator multOp))
                        {
                            tokens.Add(new OperatorToken(multOp));
                        }
                        else
                        {
                            throw new Exception("Multiplication operator missing from registry. Required for unary minus.");
                        }

                        i++;
                        continue; 
                    }

                    if (c == '+')
                    {
                        
                        i++;
                        continue;
                    }
                }

                if (_registry.TryGetOperator(c, out IOperator op))
                {
                    tokens.Add(new OperatorToken(op));
                    expectOperand = true; 
                    i++;
                    continue;
                }

                throw new Exception($"Unexpected character: {c}");
            }

            return tokens;
        }
    }
}
