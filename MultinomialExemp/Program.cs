using System.Numerics;
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
    return Math.Sqrt(2 * Math.PI * num) * Math.Pow(num / Math.E, num);
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
int MultinomialCoefficient(int n, params int[] numbers)
{
    try
    {
        if (n < 0)
        { 
            throw new Exception("Мультиномиальный коэффициент неопределен"); 
        }
            
        int numbersSum = 0;

        foreach (int i in numbers) 
        {
            if (i > n || i < 0)
            {
                throw new Exception("Мультиномиальный коэффициент неопределен");
            }
            numbersSum = numbersSum + i;
        }
    }
    catch(Exception e)
    {
        Console.WriteLine($"Ошибка: {e.Message}");
    }
    int result = 0;
    int denominator = 1;
    foreach (int number in numbers)
    {
        denominator = denominator * (int)factorial(number);
    }
    return n / denominator;
}
// Метод для общего случая типа (x1+3x2+...+x3^4)^5
int MultinomialGeneralCase(int num)
{
    return num;
}
Console.WriteLine(factorial(5));


//var context = new MathContext();
//context.BindFunction(Math.Sqrt);
//context.BindFunction(d => Math.Log(d), "ln");

//Console.WriteLine("ln(1/-x1 + Math.Sqrt(1/(x2*x2) + 1))"
//    .Evaluate(new { x1 = 0.5, x2 = -0.5 }, context));
