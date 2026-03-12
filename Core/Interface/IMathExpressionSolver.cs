namespace ChaosCalculator.Core.Interface
{
    public interface IMathExpressionSolver
    {
        /// <summary>
        /// Evaluates a string expression and returns the result, or null if it fails.
        /// </summary>
        double? Solve(string expression);
    }
}
