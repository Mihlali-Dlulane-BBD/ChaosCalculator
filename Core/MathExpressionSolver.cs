using ChaosCalculator.Core.Interface;
using ChaosCalculator.Core.Parser;

namespace ChaosCalculator.Core
{
    public class MathExpressionSolver : IMathExpressionSolver
    {
        private readonly Tokenizer _tokenizer;
        private readonly Evaluator _evaluator;

        public MathExpressionSolver(Tokenizer tokenizer, Evaluator evaluator)
        {
            _tokenizer = tokenizer;
            _evaluator = evaluator;
        }

        public double? Solve(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
            {
                return null;
            }

            try
            {
                var tokens = _tokenizer.Tokenize(expression);
                return _evaluator.Evaluate(tokens);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
