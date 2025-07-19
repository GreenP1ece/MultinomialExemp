# Multinomial Expansion & Coefficient Calculator

A C# library that parses symbolic and numeric multinomial expressions of the form:

(x1 + 3x2^1/6 + x3^5.2 + 2x4^3 + x5)^4

It then computes the **multinomial coefficients**, expands the expression using both **exact** and **approximate** methods, and supports symbolic and purely numeric cases.

---

## ✨ Features

- ✅ Parses symbolic multinomial expressions
- ✅ Extracts coefficients, variables, powers, and main exponent
- ✅ Calculates exact multinomial coefficients (`BigInteger`)
- ✅ Fast approximate calculation using Stirling’s formula and logarithms
- ✅ Supports fractional and decimal powers
- ✅ Composition generator for term expansion
- ✅ Outputs full expansion as a readable expression

---

## 📦 Example Input

```csharp
string input = "(x1 + 3x2^1/6 + x3^5.2 + 2x4^3 + x5)^4";

ParseMultinomial(input, out var variables, out var coefficients, out var powers, out var exponent);

// Compute compositions and coefficients
var compositions = GenerateCompositions(exponent, variables.Length, 0, variables.Length).ToList();
var expansion = ExpandMultinomial(compositions.Count, variables, coefficients, powers, compositions);

Console.WriteLine(expansion);
```
### 🧠 How It Works
Parsing
The expression is parsed using regular expressions to extract:

Variable names (x1, x2, …)

Coefficients (3, 2, …)

Powers (1/6, 5.2, …)

Main exponent (^4)

Composition Generation
Generates all integer compositions of the main exponent into k terms.

Coefficient Calculation

Exact: via factorials using BigInteger

Approximate: via Stirling’s formula or log-exp optimization

Expression Expansion
Each term is formatted into a human-readable polynomial expression.

### 📏 Accuracy
Exact multinomial coefficients calculated via BigInteger

Approximate factorial and coefficient methods maintain:

≤ 0.1% error for n > 20

≤ 1% error for smaller n with simple Stirling formula

### 🔍 TODO / Roadmap
 Better support for rational exponents in symbolic output

 LINQ-based composition generator

 Add CLI or Web UI frontend

 NuGet packaging
### 🙌 Acknowledgements
Inspired by combinatorics and symbolic algebra packages such as Mathematica and SymPy.

📬 Contact
Created by GreenP1ece.
Feel free to contribute, open issues, or suggest improvements.
