using ChaosCalculator.Core.Interface;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Data;


namespace ChaosCalculator.Components.Pages
{
    public partial class Calculator
    {
        // --- 1. MODELS & STATE ---

        [Inject]
        public IMathExpressionSolver MathExpressionSolver { get; set; } = default!;
        public enum KeyType { Number, TopOperator, RightOperator, Zero }
        private string currentTheme = "dark";
        private ElementReference calculatorWrapper;

        private class KeyInfo
        {
            public string Label { get; set; }
            public KeyType Type { get; set; }
            public KeyInfo(string label, KeyType type = KeyType.Number) { Label = label; Type = type; }
        }

        private string equationString = "";
        private string currentDisplay = "0";
        private bool isNewEntry = true;
        private bool isCalculated = false;

        // --- 2. LAYOUT & CHAOS ENGINE ---
        private readonly List<KeyInfo> DefaultLayout = new()
    {
        new("C", KeyType.TopOperator), new("(", KeyType.TopOperator), new(")", KeyType.TopOperator), new("÷", KeyType.RightOperator),
        new("7"),                      new("8"),                      new("9"),                      new("×", KeyType.RightOperator),
        new("4"),                      new("5"),                      new("6"),                      new("-", KeyType.RightOperator),
        new("1"),                      new("2"),                      new("3"),                      new("+", KeyType.RightOperator),
        new("0", KeyType.Zero),        new("."),                      new("=", KeyType.RightOperator)
    };

        private List<KeyInfo> keypadLayout = new();

        protected override void OnInitialized() => keypadLayout = new List<KeyInfo>(DefaultLayout);

        private void ShuffleKeypad()
        {
            var movableIndices = new List<int>();
            var buttonsToShuffle = new List<KeyInfo>();
            for (int i = 0; i < keypadLayout.Count; i++)
            {
     
                if (keypadLayout[i].Type != KeyType.Zero)
                {
                    movableIndices.Add(i);
                    buttonsToShuffle.Add(keypadLayout[i]);
                }
            }
            var scrambledButtons = buttonsToShuffle.OrderBy(_ => Random.Shared.Next()).ToList();

            for (int i = 0; i < movableIndices.Count; i++)
            {
                int targetIndex = movableIndices[i];
                keypadLayout[targetIndex] = scrambledButtons[i];
            }
        }

        private string GetButtonClass(KeyInfo key) => key.Type switch
        {
            KeyType.TopOperator => "text-teal",
            KeyType.RightOperator => "text-pink",
            KeyType.Zero => "btn-zero",
            _ => ""
        };

        // --- 3. THE TRAFFIC COP (Routing the key press) ---
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {

                await calculatorWrapper.FocusAsync();
            }
        }
        private void OnKeyPress(KeyInfo key)
        {
            switch (key.Label)
            {
                case "C":
                    HandleClear();
                    break;
                case "=":
                    CalculateResult();
                    break;
                case "+" or "-" or "×" or "÷":
                    HandleOperator(key.Label);
                    break;
                case "(" or ")":
                    HandleBracket(key.Label);
                    break;
                default:
                    HandleNumberOrDecimal(key.Label);
                    break;
            }
        }

        private void HandleKeyDown(KeyboardEventArgs e)
        {
            // Map physical keyboard strokes directly to our helper methods
            switch (e.Key)
            {
                case "Escape":
                case "Backspace":
                case "Delete":
                case "c":
                case "C":
                    HandleClear();
                    break;
                case "Enter":
                case "=":
                    CalculateResult();
                    break;
                case "+":
                case "-":
                    HandleOperator(e.Key);
                    break;
                case "*":
                case "x":
                case "X":
                    HandleOperator("×");
                    break;
                case "/":
                    HandleOperator("÷");
                    break;
                case "(":
                case ")":
                    HandleBracket(e.Key);
                    break;
                case "0":
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                case ".":
                    HandleNumberOrDecimal(e.Key);
                    break;
            }
        }

        // --- 4. THE HELPERS (Single Responsibility Methods) ---
        private void HandleClear()
        {
            equationString = "";
            currentDisplay = "0";
            isNewEntry = true;
            isCalculated = false;
        }

        private void HandleOperator(string operatorLabel)
        {
            if (isCalculated)
            {
                equationString = $"{currentDisplay} {operatorLabel} ";
                isCalculated = false;
            }
            else if (!isNewEntry)
            {
                equationString += $"{currentDisplay} {operatorLabel} ";
            }
            else
            {
                equationString += $"{operatorLabel} ";
            }

            isNewEntry = true;
        }

        private void HandleBracket(string bracketLabel)
        {
            if (isCalculated)
            {
                equationString = $"{bracketLabel} ";
                currentDisplay = "0";
                isCalculated = false;
            }
            else
            {
                if (bracketLabel == ")" && !isNewEntry)
                {
                    equationString += $"{currentDisplay} ";
                }
                equationString += $"{bracketLabel} ";
            }
            isNewEntry = true;
        }

        private void HandleNumberOrDecimal(string label)
        {
            if (isCalculated)
            {
                equationString = "";
                currentDisplay = label == "." ? "0." : label;
                isCalculated = false;
                isNewEntry = false;
                return;
            }

            if (isNewEntry)
            {
                currentDisplay = label == "." ? "0." : label;
                isNewEntry = false;
            }
            else
            {
                if (label == "." && currentDisplay.Contains(".")) return;
                currentDisplay += label;
            }
        }

        private void CalculateResult()
        {
            if (isCalculated) return;

            try
            {
                if (!isNewEntry) equationString += currentDisplay;

                string parseableEquation = equationString.Replace("×", "*").Replace("÷", "/");
                var result = MathExpressionSolver.Solve(parseableEquation);

                currentDisplay = result.ToString() ?? "0";
                equationString += " =";

                isCalculated = true;
                isNewEntry = true;
            }
            catch
            {
                currentDisplay = "Error";
                isCalculated = true;
                isNewEntry = true;
            }
        }

        private void ToggleTheme()
        {
            currentTheme = currentTheme == "dark" ? "light" : "dark";
        }

    }
}
