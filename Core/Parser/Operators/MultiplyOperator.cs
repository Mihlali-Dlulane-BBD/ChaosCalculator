using ChaosCalculator.Core.Parser.Interface;

namespace ChaosCalculator.Core.Parser.Operators
{
    public class MultiplyOperator : IOperator
    {
        public char Symbol => '*';
        public int Precedence => 2;
        public double Calculate(double left, double right) => left * right;
    }
}
