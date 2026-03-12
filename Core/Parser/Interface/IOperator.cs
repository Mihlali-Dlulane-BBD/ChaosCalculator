namespace ChaosCalculator.Core.Parser.Interface
{
    public interface IOperator
    {
        char Symbol { get; }
        int Precedence { get; }
        double Calculate(double left, double right);
    }
}
