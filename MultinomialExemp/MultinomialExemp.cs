using System.Numerics;
using System.Text.RegularExpressions;
using MathEvaluation.Context;
using MathEvaluation.Extensions;

public class MultinomialExemp
{
    static List<double> Logarithms = new List<double>();
    ///<summary>
    /// Gets an array of numbers, calculating the multinomial coefficient from them.
    ///</summary>
    ///<param name="numbers">A set of integers</param>
    ///<returns>A high precision integer of type BigInteger</returns>>
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

    public static ulong LogCoefficient(params ulong[] numbers)
    {
        return ApproximateCoefficient(numbers);
    }

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

    public static BigInteger Factorial(BigInteger n)
    {
        BigInteger result = 1;
        for (ulong i = 1; i <= n; i++)
            result = result * i;
        return result;
    }

    public static BigInteger ApproximateFactorial(int num)
    {
        //Вернуть n! ~ sqrt(2ПN)*(num/e)^num
        return (BigInteger)(Math.Sqrt(2 * Math.PI * num) * Math.Pow(num / Math.E, num));
    }

    //Парсинг x-ов, коэффициентов, степеней и степени всего 
    //выражения в рамках общего случая
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
    /// <summary>
    /// There are two <see href="https://bing.com">params</see>.
    /// <list type="number">
    /// <item><param name="id">The user <em>id</em></param></item>
    /// <item><param name="username">The user <em>name</em></param></item>
    /// </list>
    /// </summary>
    /// <returns>The <strong>username</strong>.</returns>
    public static string GetUserName(int id)
        {
            return "username";
        }
    }
