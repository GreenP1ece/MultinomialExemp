using System.Numerics;
using System.Text.RegularExpressions;
using MathEvaluation.Context;
using MathEvaluation.Extensions;

public class MultinomialExemp
{
    static List<double> Logarithms = new List<double>();
    /// <summary>
    /// Calculates the exact multinomial coefficient based on the provided array of integers.
    /// </summary>
    /// <param name="numbers">An array of integers representing the exponents in a multinomial term.</param>
    /// <returns>The multinomial coefficient as a <see cref="BigInteger"/>.</returns>
    public static BigInteger ComputeCoefficient(ulong[] numbers)
    {
        BigInteger numbersSum = 0;
        foreach (var number in numbers)
            numbersSum += number;

        BigInteger nominator = Factorial(numbersSum);

        BigInteger denominator = 1;
        foreach (var number in numbers)
            denominator *= Factorial(new BigInteger(number));

        return nominator / denominator;
    }
    /// <summary>
    /// Calculates an approximate multinomial coefficient using Stirling's approximation.
    /// </summary>
    /// <param name="numbers">An array of integers representing the exponents in the term.</param>
    /// <returns>The approximated multinomial coefficient as a <see cref="BigInteger"/>.</returns>
    public static BigInteger BigArAprox(ulong[] numbers)
    {
        BigInteger numbersSum = 0;
        foreach (var number in numbers)
            numbersSum += number;

        BigInteger nominator = ApproximateFactorial((int)numbersSum);

        BigInteger denominator = 1;
        foreach (var number in numbers)
            denominator *= ApproximateFactorial((int)number);

        return nominator / denominator;
    }
    /// <summary>
    /// Wrapper method that calculates an approximate multinomial coefficient using logarithmic methods.
    /// </summary>
    /// <param name="numbers">A variable number of integers representing the multinomial exponents.</param>
    /// <returns>The approximated multinomial coefficient as an <see cref="ulong"/>.</returns>

    public static ulong LogCoefficient(params ulong[] numbers)
    {
        return ApproximateCoefficient(numbers);
    }
    /// <summary>
    /// Calculates an approximate multinomial coefficient using logarithms and exponentials
    /// to avoid intermediate overflow and improve performance.
    /// </summary>
    /// <param name="numbers">An array of integers representing the exponents in the multinomial.</param>
    /// <returns>The approximated coefficient as an <see cref="ulong"/>.</returns>

    public static ulong ApproximateCoefficient(ulong[] numbers)
    {
        ulong maxNumber = numbers.Max();

        ulong numbersSum = 0;
        foreach (var number in numbers)
            numbersSum += number;

        double sum = 0;
        for (ulong i = 2; i <= numbersSum; i++)
        {
            if (i <= maxNumber)
            {
                if (i - 2 >= (ulong)Logarithms.Count)
                {
                    var log = Math.Log(i);
                    Logarithms.Add(log);
                }
            }
            else
            {
                if (i - 2 < (ulong)Logarithms.Count)
                    sum += Logarithms[(int)i - 2];
                else
                {
                    var log = Math.Log(i);
                    Logarithms.Add(log);
                    sum += log;
                }
            }
        }

        var maxNumberFirst = false;
        foreach (var number in numbers)
            if (number == maxNumber && !maxNumberFirst)
                maxNumberFirst = true;
            else
                for (ulong i = 2; i <= number; i++)
                    sum -= Logarithms[(int)i - 2];

        return (ulong)Math.Round(Math.Exp(sum));
    }
    /// <summary>
    /// Computes the exact factorial of a given number using iterative multiplication.
    /// </summary>
    /// <param name="n">A non-negative integer.</param>
    /// <returns>The factorial value as a <see cref="BigInteger"/>.</returns>

    public static BigInteger Factorial(BigInteger n)
    {
        BigInteger result = 1;
        for (ulong i = 1; i <= n; i++)
            result = result * i;
        return result;
    }
    /// <summary>
    /// Approximates the factorial of a number using Stirling’s formula.
    /// </summary>
    /// <param name="num">A non-negative integer.</param>
    /// <returns>An approximate factorial value as a <see cref="BigInteger"/>.</returns>

    public static BigInteger ApproximateFactorial(int num)
    {
        //Вернуть n! ~ sqrt(2ПN)*(num/e)^num
        return (BigInteger)(Math.Sqrt(2 * Math.PI * num) * Math.Pow(num / Math.E, num));
    }

    /// <summary>
    /// Parses a multinomial expression string of the form "(x1 + 2x2^3 + x3)^k"
    /// into its component terms: variables, coefficients, powers, and main exponent.
    /// </summary>
    /// <param name="input">A string representing the multinomial expression, e.g., "(x1 + 2x2^3 + x3)^4".</param>
    /// <param name="sX">An array of variable names (e.g., "x1", "x2", ...).</param>
    /// <param name="sXCoefs">An array of coefficients for each variable (e.g., 1, 2, ...).</param>
    /// <param name="sXPows">An array of powers (as strings) for each variable, allowing fractions or decimals.</param>
    /// <param name="sMainPow">The main exponent applied to the entire expression (the power after the closing parenthesis).</param>
    public static void ParseMultinomial(string input,
                                              out string[] sX,
                                              out double[] sXCoefs,
                                              out string[] sXPows,
                                              out int sMainPow)
    {
        string inputWithoutWhiteSpaces = input.Replace(" ", "");

        Regex sMainPowRegex = new Regex(@"(?<=\)\^)\d");
        Regex sXRegex = new Regex(@"(?:(\d+(?:\.\d+)?)\*?)?x(\d+)(?:\^(\d+(?:\.\d+)?|\(\d+\/\d+\)))?");
        //Regex sXPowsRegex = new Regex(@"\^(\d+(\.\d+)?|\(\d+\/\d+\))");

        Match matchSMainPow = sMainPowRegex.Match(inputWithoutWhiteSpaces);

        MatchCollection matches = sXRegex.Matches(inputWithoutWhiteSpaces);

        sMainPow = matchSMainPow.Success ? Convert.ToInt32(matchSMainPow.Value) : 0;

        sX = new string[matches.Count];
        sXCoefs = new double[matches.Count];
        sXPows = new string[matches.Count];

        for (int i = 0; i < matches.Count; i++)
        {
            Match match = matches[i];

            string varIndex = match.Groups[2].Value;
            double coef = match.Groups[1].Success ? Convert.ToDouble(match.Groups[1].Value) : 1;
            string pow = match.Groups[3].Success ? match.Groups[3].Value : "1";

            sX[i] = $"x{varIndex}";
            sXCoefs[i] = coef;
            sXPows[i] = pow.Replace("(", "").Replace(")", "");
        }
    }
    /// <summary>
    /// Generates all compositions (ordered partitions) of a number `n` into `k` elements,
    /// each ranging from <paramref name="min_elem"/> to <paramref name="max_elem"/>.
    /// </summary>
    /// <param name="n">The target sum to partition.</param>
    /// <param name="k">The number of elements in each composition.</param>
    /// <param name="min_elem">The minimum value of each element.</param>
    /// <param name="max_elem">The maximum value of each element.</param>
    /// <returns>An enumerable of integer arrays representing the compositions.</returns>

    static IEnumerable<int[]> GenerateCompositions(int n, int k, int min_elem, int max_elem)
    {
        var allowed = Enumerable.Range(min_elem, max_elem - min_elem + 1)
                             .Reverse();
        IEnumerable<int[]> helper(int n, int k, List<int> t)
        {
            if (k == 0)
            {
                if (n == 0)
                    yield return t.ToArray();
                yield break;
            }
            else if (k == 1)
            {
                if (allowed.Contains(n))
                {
                    t.Add(n);
                    yield return t.ToArray();
                }
            }
            else if (min_elem * k <= n && n <= max_elem * k)
            {
                foreach (var v in allowed)
                {
                    var next = new List<int>(t) { v };
                    foreach (var result in helper(n - v, k - 1, next))
                        yield return result;
                }
            }
        }
        return helper(n, k, new List<int>());
    }
    /// <summary>
    /// Expands a symbolic multinomial expression using approximate multinomial coefficients for better performance.
    /// </summary>
    /// <param name="GenMultCoefsCount">The number of generated multinomial coefficients (unused internally).</param>
    /// <param name="terms">An array of variable names (e.g., "x1", "x2").</param>
    /// <param name="termsCoefs">An array of coefficients corresponding to each term.</param>
    /// <param name="termsPows">An array of powers (as strings) for each variable.</param>
    /// <param name="compositions">List of exponent combinations (compositions) for the multinomial expansion.</param>
    /// <returns>The approximated expanded multinomial expression as a string.</returns>
    public static string ExpandMultinomialFast(int GenMultCoefsCount, string[] terms, double[] termsCoefs, string[] termsPows, List<ulong[]> compositions)
    {
        string result = "";
        List<string[]> termsGroups = [];

        Regex commonFractions = new Regex(@"d+\/d+");
        Regex commonFractionDenominatior = new Regex(@"\/\d+");

        for (int i = 0; i < compositions.Count; i++)
        {
            ulong[] comp = compositions[i];
            List<string> termParts = new();
            ulong multiCoef = LogCoefficient(comp);
            result += $"{multiCoef} * ";
            for (int j = 0; j < terms.Length; j++)
            {
                if (comp[j] == 0)
                    continue;
                double termDivider = 0;
                string termDenominator = "";

                Match matchCommonFraction = commonFractions.Match(termsPows[j]);
                termDivider = matchCommonFraction.Success ?
                Convert.ToDouble(matchCommonFraction.Value.Substring(0, matchCommonFraction.Value.IndexOf('/')))
                : Convert.ToDouble(termsPows[j].Replace(".", ","));

                termDivider *= comp[j];

                matchCommonFraction = commonFractionDenominatior.Match(termsPows[j]);
                termDenominator = matchCommonFraction.Success ?
                matchCommonFraction.Value.Substring(matchCommonFraction.Value.IndexOf('\\'),
                matchCommonFraction.Value.Length)
                : "";

                string sTermDivider = termDivider.ToString("0.00");
                string sTermCoefs = termsCoefs[j].ToString() == "1" ? "" : termsCoefs[j].ToString();
                if (termDivider == 1 && termDenominator != "")
                {
                    termParts.Add($"{termsCoefs[j]}{terms[j]})");
                }
                else if (termDivider != 0 && termDenominator != "")
                {
                    termParts.Add($"{termsCoefs[j]}{terms[j]}^({sTermDivider}/{termDenominator}");
                }
                else if (termDivider != 0 && termDenominator == "")
                {
                    termParts.Add($"{termsCoefs[j]}{terms[j]}^{sTermDivider}");
                }
                else if (termDivider == 0)
                {
                    continue;
                }
            }
            result += string.Join("*", termParts) + " + ";
        }
        return result.Remove(result.LastIndexOf(" + "));
    }
    /// <summary>
    /// Expands a symbolic multinomial expression using exact multinomial coefficients.
    /// </summary>
    /// <param name="GenMultCoefsCount">The number of generated multinomial coefficients (unused internally).</param>
    /// <param name="terms">An array of variable names (e.g., "x1", "x2").</param>
    /// <param name="termsCoefs">An array of coefficients corresponding to each term.</param>
    /// <param name="termsPows">An array of powers (as strings) for each variable.</param>
    /// <param name="compositions">List of exponent combinations (compositions) for the multinomial expansion.</param>
    /// <returns>The fully expanded multinomial expression as a string.</returns>

    public static string ExpandMultinomial(int GenMultCoefsCount, string[] terms, double[] termsCoefs, string[] termsPows, List<ulong[]> compositions)
    {
        string result = "";
        List<string[]> termsGroups = [];

        Regex commonFractions = new Regex(@"d+\/d+");
        Regex commonFractionDenominatior = new Regex(@"\/\d+");

        for (int i = 0; i < compositions.Count; i++)
        {
            ulong[] comp = compositions[i];
            List<string> termParts = new();
            BigInteger multiCoef = ComputeCoefficient(comp);
            result += $"{multiCoef} * ";
            for (int j = 0; j < terms.Length; j++)
            {
                if (comp[j] == 0)
                    continue;
                double termDivider = 0;
                string termDenominator = "";

                Match matchCommonFraction = commonFractions.Match(termsPows[j]);
                termDivider = matchCommonFraction.Success ?
                Convert.ToDouble(matchCommonFraction.Value.Substring(0, matchCommonFraction.Value.IndexOf('/')))
                : Convert.ToDouble(termsPows[j].Replace(".", ","));

                termDivider *= comp[j];

                matchCommonFraction = commonFractionDenominatior.Match(termsPows[j]);
                termDenominator = matchCommonFraction.Success ?
                matchCommonFraction.Value.Substring(matchCommonFraction.Value.IndexOf('\\'),
                matchCommonFraction.Value.Length)
                : "";

                string sTermDivider = termDivider.ToString("0.00");
                string sTermCoefs = termsCoefs[j].ToString() == "1" ? "" : termsCoefs[j].ToString();
                if (termDivider == 1 && termDenominator != "")
                {
                    termParts.Add($"{termsCoefs[j]}{terms[j]})");
                }
                else if (termDivider != 0 && termDenominator != "")
                {
                    termParts.Add($"{termsCoefs[j]}{terms[j]}^({sTermDivider}/{termDenominator}");
                }
                else if (termDivider != 0 && termDenominator == "")
                {
                    termParts.Add($"{termsCoefs[j]}{terms[j]}^{sTermDivider}");
                }
                else if (termDivider == 0)
                {
                    continue;
                }
            }
            result += string.Join("*", termParts) + " + ";
        }
        return result.Remove(result.LastIndexOf(" + "));
    }
}
