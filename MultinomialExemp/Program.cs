using System.Numerics;
using System.Text.RegularExpressions;
using MathEvaluation.Context;
using MathEvaluation.Extensions;

List<double> Logarithms = new List<double>();
static BigInteger BigAr(uint[] numbers)
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
ulong Log(params uint[] numbers)
{
    return LogAr(numbers);
}
 ulong LogAr(uint[] numbers)
{
    int maxNumber = (int)numbers.Max();

    uint numbersSum = 0;
    foreach (var number in numbers)
        numbersSum += number;

    double sum = 0;
    for (int i = 2; i <= numbersSum; i++)
    {
        if (i <= maxNumber)
        {
            if (i - 2 >= Logarithms.Count)
            {
                var log = Math.Log(i);
                Logarithms.Add(log);
            }
        }
        else
        {
            if (i - 2 < Logarithms.Count)
                sum += Logarithms[i - 2];
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
            for (int i = 2; i <= number; i++)
                sum -= Logarithms[i - 2];

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
// Метод для общего случая типа (x1+3x2+...+x3^4)^5
int MultinomialGeneralCase(int num)
{
    return num;
}
// Метод для частного случая при всех известных x
int MultinomialParticularCase(int num)
{
    return num;
}

//Парсинг x-ов, коэффициентов, степеней и степени всего 
//выражения в рамках общего случай
void GetAllParametersOfMultinomialExpress(string input,
                                          out string[] sX,
                                          out string[] sXCoefs,
                                          out double sMainPow)
{
    string inputWithoutWhiteSpaces = input.Replace(" ", "");

    Regex sMainPowRegex = new Regex(@"(?<=\)\^).{1,}");
    Regex sXRegex = new Regex(@"x\d+");
    Regex sXCoefsRegex = new Regex(@"(?:(\d+(?:\.\d+)?)\*?)?x\d+");

    MatchCollection matchSMainPow = sMainPowRegex.Matches(inputWithoutWhiteSpaces);
    MatchCollection matchSX = sXRegex.Matches(inputWithoutWhiteSpaces);
    MatchCollection matchSXCoefs = sXCoefsRegex.Matches(inputWithoutWhiteSpaces);
    if (matchSMainPow.Count > 0)
    {
        sMainPow = Convert.ToDouble(matchSMainPow[0].Value);
    }
    else
    {
        Console.WriteLine("Совпадений не найдено");
            sMainPow = 0;
    }
    if (matchSX.Count > 0)
    {
        sX = new string[matchSX.Count];
        for (int i = 0; i < matchSX.Count; i++)
        {
            string xValue = matchSX[i].Value;
            sX[i] = xValue;
        }
    }
    else
    {
        sX = [];
    }
    if (matchSXCoefs.Count > 0)
    {
        sXCoefs = new string[matchSXCoefs.Count];
        for (int i = 0; i < matchSXCoefs.Count; i++)
        {
            Match match = matchSXCoefs[i];
            var coefGroup = match.Groups[1];
            sXCoefs[i] = coefGroup.Success ? coefGroup.Value : "1";
        }
    }
    else
    {
        sXCoefs = [];
    }
}
//Проверка GetAllParametersOfMultinomialExpress
GetAllParametersOfMultinomialExpress("(x1 + 3x2+ x3^(4/3) +2x4^3)^44,5", out string[] sX, out string[] sXCoefs, out double sMainPow); //Корректно для общей степени; для все xi...xn; 

Console.WriteLine(sMainPow);
foreach (var s in sX)
    Console.WriteLine(s);
foreach (var d in sXCoefs)
    Console.WriteLine(d);
//Тест факторивалов
//TestAllMethods();
