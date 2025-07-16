using System.Numerics;

namespace MultinomialExempTest
{
    public class Factorial
    {
        [Fact]
        public void FactorialTest()
        {
            int input = 31;
            BigInteger expect = BigInteger.Parse("8222838654177922817725562880000000");

            BigInteger actual = MultinomialExemp.Factorial(input);

            Assert.Equal(expect, actual);
        }
        [Fact]
        public void ApproximateFactorialTest()
        {
            int input = 31;
            BigInteger expect = BigInteger.Parse("8200764697241122458512884083195904");

            BigInteger actual = MultinomialExemp.ApproximateFactorial(input);

            Assert.Equal(expect, actual);
        }

    }
    public class FactorialPrecision
    {
        [Fact]
        public void HighestPrecisionFactorialTest()
        {
            int input = 31;

            BigInteger expectFactorial = BigInteger.Parse("8222838654177922817725562880000000");
            BigInteger expectAproximateFactorial = BigInteger.Parse("8200764697241122458512884083195904");

            double expect = 0.000001;

            double actual = (double)BigInteger.Abs(MultinomialExemp.Factorial(input)
                          - MultinomialExemp.ApproximateFactorial(input))
                          / (double)MultinomialExemp.Factorial(input);

            Assert.True(actual <= expect);
        }
        [Fact]
        public void StirlingApproximationPrecision()
        {
            int input = 31;

            BigInteger expectFactorial = BigInteger.Parse("8222838654177922817725562880000000");
            BigInteger expectAproximateFactorial = BigInteger.Parse("8200764697241122458512884083195904");

            double expect = 0.01;

            double actual = (double)BigInteger.Abs(MultinomialExemp.Factorial(input)
                          - MultinomialExemp.ApproximateFactorial(input))
                          / (double)MultinomialExemp.Factorial(input);

            Assert.True(actual <= expect);
        }
    }
    public class GenerateCompositionsTest()
    {
        [Fact]
        public void GenerateCompositions()
        {

        }
    }
}