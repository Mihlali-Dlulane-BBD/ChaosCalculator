using ChaosCalculator.Core.Parser.Interface;

namespace ChaosCalculator.Core.Parser
{
    public class OperatorRegistry
    {
        private readonly Dictionary<char, IOperator> _operators = new();

        public OperatorRegistry(IEnumerable<IOperator> operators)
        {
            foreach (var op in operators)
            {
                _operators[op.Symbol] = op;
            }
        }

        public bool TryGetOperator(char symbol, out IOperator op)
        {
            return _operators.TryGetValue(symbol, out op);
        }
    }
}
