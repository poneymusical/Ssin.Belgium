# Ssin.Belgium

[![Build Status](https://guhke.visualstudio.com/Ssin.Belgium/_apis/build/status/poneymusical.Ssin.Belgium?branchName=master)](https://guhke.visualstudio.com/Ssin.Belgium/_build/latest?definitionId=11&branchName=master)

This library consists in a `Ssin` type that can be used to represent belgian SSIN numbers. The struct contains parsing methods (`Parse` and `TryParse`, just like in base types) and validation methods.
The library can handle SSIN for residents ("numéro de registre national") and non-residents ("numéro BIS").

For more information on the structure of a belgian SSIN, please refer to [Wikipedia](https://fr.wikipedia.org/wiki/Num%C3%A9ro_de_registre_national) or the [belgian Social Security website](https://www.socialsecurity.be/site/v2/dimona/fr/dimona/scenario/fields/action_insz.html).

**A word of caution:** as the validation process of a SSIN is somewhat complex and involves all its members, constructor and parsing methods only account for the global structure. This means that you can build or parse a value that looks like a SSIN (it has all the necessary members and does not contain invalid characters), but is not a valid SSIN (e.g. the control part does not match the rest of the SSIN). So be sure to use the validation methods as well!


## Compatibility

The library targets .NET Standard 2.0. It has no external dependencies as it relies solely on basic stuff.

## Feedback

Feedbacks are very much appreciated. If you suspect the library is bugged, please raise an issue on the GitHub repository, including the SSIN value that causes concern. I'll try to answer as quickly as I can!

## Usage

Simply add the nuget package to your project. The package is available [here](https://www.nuget.org/packages/Ssin.Belgium/).

### Formatting

An enum allows you to tell what is the formatting of your SSIN. Two formats are available:
* `SsinFormat.Raw`: the SSIN looks like 1234578901; 11 digits, no other char allowed.
* `SsinFormat.Formatted`: the SSIN looks like 12.34.56-789.01. This is the way a SSIN is printed on a Belgian ID card.

### Building a SSIN

You can use the `Ssin` type to instantiate values, e.g.:

```csharp
Ssin ssin = new Ssin(90, 10, 01, 123, 45);
```

You can then print the SSIN to a string:

```csharp
Console.WriteLine(ssin.ToString()); //"90100112345"
Console.WriteLine(ssin.ToString(SsinFormat.Raw)); //"90100112345"
Console.WriteLine(ssin.ToString(SsinFormat.Formatted)); //"90.10.01-123.45"
```

## Parsing

Several static parsing methods are available:
* `Ssin Ssin.Parse(string source)`
* `Ssin Ssin.ParseExact(string source, SsinFormat format)`
* `bool Ssin.TryParse(string source, out Ssin parsed)`
* `bool Ssin.TryParseExact(string source, out Ssin parsed, SsinFormat format)`

## Validation methods

Two validation methods are available:
* `bool IsValid()`. 
* Static `bool Ssin.IsValid(string ssin)` => this method allows you to quickly check a string SSIN. Handy!

## Getting birth date
You can get the user's birth date for the provided SSIN :
* `DateTime GetBirthDate()`

The method returns `null` if the date is unknown (no day or month or year).

## Getting the user gender

You may get the birth gender from the provided SSIN :
* `SsinGender GetGender()`

The method returns `null` if the gender is unknown, only if BIS number with +40 increment. Information is available [here](https://housinganywhere.com/Belgium/belgian-national-number/).

> Note that this information is not 100% reliable because gender may have change but not the SSIN.
