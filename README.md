# About *CalcDiv*

*README en español disponible dentro del repositorio.*

CLI application written in C# to calculate divisibility rules.

This is a project I've been developing since 2021, initially in C++ and later Java. My motivation is to improve my coding skills and find out more about divisibility rules along the way.

It's honestly a bit weird that [divisibility-rules](https://github.com/topics/divisibility-rules) only contains three repositories right now, mine included. Even if this is a very niche field I want to contribute something decent.

Even though the idea to begin *CalcDiv* was my own, and I did not look for other similar projects, 
I recommend checking out [@lemiorhan](https://github.com/lemiorhan)'s [grand-unified-divisibility-rule](https://github.com/lemiorhan/grand-unified-divisibility-rule), one of the other two repositories mentioned,
he explains many concepts that are also used in *CalcDiv* in great detail.

However, this project aims to find more divisibility rules, for every integer base and with an arbitrary amount of coefficients used per iteration.
As well as finding rules that do not use coefficients.

---

## How to use and interpret *CalcDiv*

We will need to understand a few concepts to use *CalcDiv*.

### Terminology and important concepts

`Divisibility rule` - A series of calculations that allow the user to know if a number is a multiple of another without performing the division.

Note that a divisibility rule depends on the representation of the number used and what number we (don't, as we are using a divisibility rule,) want to divide it by.
There are no rules for numbers in unary because of this.

We will refer to divisibility rules as rules for brevity.

`Dividend` - The number we want to divide, this number does not affect the rule, only the final result.

`Divisor` - The number we divide by, this is one of the values that affect the rule.

`Base` - The base of the positional representation of the dividend, usually it is ten, as we use the decimal numeral system, this is the second value that affects rules.

*CalcDiv* will only work for integer bases greater than 1.

A rule obtained for a certain divisor in a certain will be equivalent to another one with a different base, giving the same results.

For example, if we want to test the divisibility of 754 by 12, the dividend would be 754, the divisor would be 12, and the base would be 10, unless otherwise specified.

*CalcDiv* currently does not, and most likely never will, offer rules for non-positional systems, like roman numerals.

The other types of rules are explained during runtime, after obtaining them.

`Coefficient` - A number that is to be multiplied by another number, used in coefficient rules.

`Coefficient rules` - Rules that use a certain amount of coefficients to determine whether a dividend is divisible by a divisor.

The coefficients are a repeating sequence, different in every rule.
These coefficients can be modified to obtain an unlimited amount of rules, while still being valid.

`Remainder` - The amount left over after dividing a dividend by the divisor. A dividend that is divisible by the divisor should have a remainder of zero.

`Prime factors` - They represent an integer as the prime numbers, that when multiplied, can be used to obtained said integer. Used by non-coefficient rules.

`First digits` - The digits of a number that are closest to the units digit.

For example, the three first digits of 82490 are 4, 9 and 0 because 0 is the units digit.

The prime factors of 82490 are 2, 5, 73 and 113.

These definitions should be enough to understand the rest of the document and the application.

### Usage of *CalcDiv* and applying coefficient rules

To learn how to use *CalcDiv* and apply rules, we will use an example:

We will test 12735, in base ten, for divisibility with 7, using 2 coefficients.

We could run CalcDivCLI and enter these arguments during dialog, or run it with the `--direct-output` option to avoid this dialog, also `-d`.

We will need to run the following command, assuming the name of the executable is `CalcDivCLI.exe` and is not changed:
`CalcDivCLI.exe --direct-output 7 10 2`, or use `-d` instead.

7 is the divisor, 10 is the base, and 2 is the amount of coefficients.

We will be given a sequence of coefficients in direct output mode. In this case, `-2,-3`.

Let's apply this rule to 12735:

1. We have to separate the dividend in two parts, the first digits and the rest. The amount of digits taken is the amount of coefficients used.
	- In this example, the result is 127 and 35.
2. We multiply each of the lower digits to the coefficient in the same position in the sequence. Then we add the results.
	- In this example, since the coefficients are -2 and -3, and the lower digits are 3 and 5.
	- The products are -6 and -15, when added we obtain -21.
3. We add the leftover digits and the result of the sum. The sum of these numbers must be divisible by the divisor if, and only if, the original dividend is divisible by it too.
	- In this example, the sum is 106.

If the number we obtained can't be trivially assumed to be divisible or not, we recursively apply the same rule, using each result as the next dividend until we can.

We would need to use the rule for 106 this time, we repeat the same process:
1. After separating the digits we have 1 and 06.
2. After multiplying the first digits and the coefficients and adding them we obtain -18.
3. The sum of 1 and -18 is -17.

Trivially, -17, in base 10, is not divisible by 7.
Therefore, neither is 106 nor 12735, the original dividend.

---

## Something about *CalcDiv*'s development

There are two collaborators in this repository: [inbaluma](https://github.com/inbaluma) y [JonniThorpe](https://github.com/JonniThorpe).

I have implemented the business logic of the application on my own. However, we used this project for a college assignment, for which we made the tests in the repository.
For this I finished version 0.1, this gave me the motivation to continue developing it.

We got a 7 out of 10, so I guess it went well.

There are many options like `-d` that can be used to obtain different results or change its representation, see the in-app help for more details.

## About the future of *CalcDiv*

Since version 0.1 I have refactored many parts of the code, making it easier to maintain and develop.

My plans for future features are smaller than those of version 0.3, and I will continue to refactor the application since there are still many problems that should have been addressed already.

I want to make a GUI application from the CLI version, similar to what I did in Java, however that version has stopped working for some reason.