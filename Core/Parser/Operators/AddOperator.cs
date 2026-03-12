using ChaosCalculator.Core.Parser.Interface;

namespace ChaosCalculator.Core.Parser.Operators
{
    public class AddOperator : IOperator
    {
        public char Symbol => '+';
        public int Precedence => 1;
        public double Calculate(double left, double right) => left + right;
    }
}
