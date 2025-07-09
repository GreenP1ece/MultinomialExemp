using System.Numerics;
using System.Text.RegularExpressions;
using MathEvaluation.Context;
using MathEvaluation.Extensions;

List<double> Logarithms = new List<double>();
static BigInteger BigAr(ulong[] numbers)
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

static BigInteger Factorial(BigInteger n)
{
    BigInteger result = 1;
    for (ulong i = 1; i <= n; i++)
        result = result * i;
    return result;
}
ulong Log(params ulong[] numbers)
{
    return LogAr(numbers);
}
ulong LogAr(ulong[] numbers)
{
    ulong maxNumber = (ulong)numbers.Max();

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
double factorialAprox(int num)
{
    int result = 1;
    //Вернуть n! ~ sqrt(2ПN)*(num/e)^num
    return (Math.Sqrt(2 * Math.PI * num) * Math.Pow(num / Math.E, num));
}
double factorial(int num)
{
    int result = 1;
    try
    {
        if (num < 0)
        {
            throw new Exception("Попытка взятия факториала от отрицательного числа");
        }
        else if (num == 0 || num == 1)
        {
            return 1;
        }
        {
            for (int i = 2; i <= num; i++)
            {
                result = result * i;
            }
        }
    }
    catch (Exception e)
    {
        Console.WriteLine($"Ошибка: {e.Message}");
    }
    return result;
}
//тест всех методов:
void TestAllMethods()
{
    //факториалы
    int fact1 = -9;
    int fact2 = 31;
    int fact3 = 29;
    BigInteger factBigInt = 36;


    Console.WriteLine("Факториалы");
    //Console.WriteLine($"Отрицательное число double factorial(int num): {factorial(fact1)}");
    //Console.WriteLine($"double factorial(int num): {factorial(fact2)}");
    Console.WriteLine($"double factorialAprox(int num): {factorialAprox(fact2)}");
    //Console.WriteLine($"double factorial(int num): {factorial(fact3)}");
    Console.WriteLine($"double factorialAprox(int num): {factorialAprox(fact3)}");
    Console.WriteLine($"BigInteger Factorial(int num): {Factorial(fact2)}");
    Console.WriteLine($"BigInteger Factorial(int num): {Factorial(fact3)}");

}

//Парсинг x-ов, коэффициентов, степеней и степени всего 
//выражения в рамках общего случая
void GetAllParametersOfMultinomialExpress(string input,
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
void GetAllParametersOfMultinomualExpressPartucularCase (string input,
                                                        out double[] terms,
                                                        out int MainPow2
                                                        )
{
    string inputWithoutWhiteSpaces = input.Replace(" ", "");

    Regex sMainPowRegex = new Regex(@"(?<=\)\^)\d");
    Regex termsRegex = new Regex(@"\d+[^\^\)\+]");

    Match matchMainPow2 = sMainPowRegex.Match(inputWithoutWhiteSpaces);
    MatchCollection matches = termsRegex.Matches(inputWithoutWhiteSpaces);
    
    MainPow2 = matchMainPow2.Success ? Convert.ToInt32(matchMainPow2.Value) : 0;
    terms = new double[matches.Count];

    for (int i = 0; i < matches.Count; i++)
    {
        Match match = matches[i];

        terms[i] = Convert.ToDouble(match.Value);
    }
}
//Проверка GetAllParametersOfMultinomialExpress
GetAllParametersOfMultinomialExpress("(x1 + 3x2^1/6+ x3^5.2 +2x4^3+x5)^4", out string[] sX, out double[] sXCoefs, out string[] sXPows, out int sMainPow); //Корректно для общей степени; для все xi...xn; 
GetAllParametersOfMultinomualExpressPartucularCase("(25+ 19 +28 + 2 + 18)^6", out double[] terms, out int MainPow2);

string[] xTerms = sX;
double[] xCoefs = sXCoefs;
string[] xPows = sXPows; // строка, потому что степень может быть представлена обыкновенной или десятичной дробью
int MainPow = sMainPow;

/*тесты для GetAllParametersOfMultinomialExpress
Console.WriteLine(sMainPow);
foreach (var s in sX)
    Console.WriteLine(s);
foreach (var d in sXCoefs)
    Console.WriteLine(d);

*/
double[] ParticTerms = terms;
Console.WriteLine("Частный метод. Иксы");
foreach (var i in ParticTerms)
{
    Console.WriteLine(i);
}
Console.WriteLine("Частный метод. Конец.");
foreach (var p in sXPows)
    Console.WriteLine(p);

//генерация всех векторов для сочетаний
IEnumerable<uint[]> GenerateCompositions(uint n, int k)
{
    uint[] result = new uint[k];
    IEnumerable<uint[]> Recurse(int index, uint remaining)
    {
        if (index == k - 1)
        {
            result[index] = remaining;
            yield return (uint[])result.Clone();
        }
        else
        {
            for (uint i = 0; i <= remaining; i++)
            {
                result[index] = i;
                foreach (uint[] _ in Recurse(index + 1, remaining - i))
                    yield return _;
            }
        }
    }

    foreach (uint[] _ in Recurse(0, n))
        yield return  _;
}
IEnumerable<int> Range(int start, int stop, int step)
{
    for (int i = start; i <= stop; i += step)
        yield return i;
}
IEnumerable<int[]> MyGenerateCompositions(int n, int k, int min_elem, int max_elem)
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
        else if (min_elem * k <= n && n <=  max_elem * k) 
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

List<ulong[]> GeneratedCompositions = [];
List<ulong> GeneratedMultinomialCoefficients = [];

foreach (var partition in MyGenerateCompositions(MainPow, xTerms.Count(), 0, xTerms.Count()))
{
    ulong[] uPartition = partition.Select(x => (ulong)x).ToArray();
    GeneratedCompositions.Add(uPartition);
}
//Console.WriteLine($"Число композиций: {GeneratedCompositions.Count()}");
foreach (ulong[] partition in GeneratedCompositions)
{
    GeneratedMultinomialCoefficients.Add(LogAr(partition));
    Console.WriteLine(string.Join(", ", partition));
}
foreach (ulong part in GeneratedMultinomialCoefficients)
{
    Console.WriteLine(string.Join(", ", part));
}
Console.WriteLine(GeneratedMultinomialCoefficients.Count());
//Общий случай
string MultinomialTheoremGeneral(int GenMultCoefsCount, string[] terms, double[] termsCoefs, string[] termsPows, List<ulong[]> compositions)
{
    string result = "";
    List<string[]> termsGroups = [];

    Regex commonFractions = new Regex(@"d+\/d+");
    Regex commonFractionDenominatior = new Regex(@"\/\d+");

    for (int i = 0; i < compositions.Count; i++)
    {
        ulong[] comp = compositions[i];
        List<string> termParts = new();
        BigInteger multiCoef = BigAr(comp);
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
            string sTermCoefs = termsCoefs[j].ToString() == "1" ? "" : termsCoefs[j].ToString() ;
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
    /*foreach (var item in terms)
    {
        result += item;
    }
    string start = result;
    for(int i = 0; i <= GenMultCoefsCount; i++)
    {
        result += " + " + start ;
    }
    */
    return result.Remove(result.LastIndexOf(" + "));

}
//Console.WriteLine(MultinomialTheoremGeneral(GeneratedMultinomialCoefficients.Count, xTerms, xCoefs, xPows, GeneratedCompositions));


//foreach (var partition in MyGenerateCompositions(5, 3, 1, 3))
//{
//    Console.WriteLine(string.Join(", ", partition));
//}

// тесты IEnumerable<int[]> GenerateCompositions(int n, int k) (успешно)
/*foreach (var comp in GenerateCompositions(4, 4))
{
    Console.WriteLine(string.Join(" ", comp));
}*/
//foreach (var comp in GenerateCompositions(MainPow, xTerms.Count()))
//{
//    Console.WriteLine(string.Join(" ", comp));
//}


//Тест факторивалов
//TestAllMethods();
