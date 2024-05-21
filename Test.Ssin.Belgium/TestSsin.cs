using System;
using NUnit.Framework;
using Ssin.Belgium;

namespace Test.Ssin.Belgium
{
    public class TestSsin
    {
        [Test]
        public void TestIsValid()
        {
            var ssin = new global::Ssin.Belgium.Ssin(88, 10, 11, 357, 67);
            Assert.IsTrue(ssin.IsValid());
            
            ssin = new global::Ssin.Belgium.Ssin(88, 10, 11, 358, 67);
            Assert.IsFalse(ssin.IsValid());
            
            ssin = new global::Ssin.Belgium.Ssin(42, 01, 22, 051, 81);
            Assert.IsTrue(ssin.IsValid());
            
            ssin = new global::Ssin.Belgium.Ssin(17, 10, 05, 114, 95);
            Assert.IsTrue(ssin.IsValid());
            
            ssin = new global::Ssin.Belgium.Ssin(00, 05, 24, 242, 72);
            Assert.IsTrue(ssin.IsValid());
            
            ssin = new global::Ssin.Belgium.Ssin(84, 03, 27, 057, 94);
            Assert.IsFalse(ssin.IsValid());
        }

        [Test]
        public void TestStaticIsValid()
        {
            Assert.IsTrue(global::Ssin.Belgium.Ssin.IsValid("88101135767"));
            Assert.IsTrue(global::Ssin.Belgium.Ssin.IsValid("88.10.11-357.67"));
            Assert.IsFalse(global::Ssin.Belgium.Ssin.IsValid("88.10.11-357/67"));
            Assert.IsFalse(global::Ssin.Belgium.Ssin.IsValid("881011357678"));
            Assert.IsFalse(global::Ssin.Belgium.Ssin.IsValid("88101135768"));
            Assert.IsFalse(global::Ssin.Belgium.Ssin.IsValid("84.03.27-057.94"));
            Assert.IsTrue(global::Ssin.Belgium.Ssin.IsValid("99501600009"));
        }

        [Test]
        [TestCaseSource(typeof(SsinSamplingSet), nameof(SsinSamplingSet.SsinList))]
        public void TestSamplingSet(string ssin)
        {
            Assert.IsTrue(global::Ssin.Belgium.Ssin.IsValid(ssin));
        }

        [Test]
        public void TestParse()
        {
            var expected = new global::Ssin.Belgium.Ssin(88, 10, 11, 357, 67);
            global::Ssin.Belgium.Ssin actual = default;
            Assert.DoesNotThrow(() => actual = global::Ssin.Belgium.Ssin.Parse("88101135767"));
            Assert.AreEqual(expected, actual);

            Assert.DoesNotThrow(() => actual = global::Ssin.Belgium.Ssin.Parse("88.10.11-357.67"));
            Assert.AreEqual(expected, actual);

            Assert.Throws<FormatException>(() => actual = global::Ssin.Belgium.Ssin.Parse("invalid"));
            Assert.Throws<FormatException>(() => actual = global::Ssin.Belgium.Ssin.Parse("881011357678"));
        }

        [Test]
        public void TestParseExact()
        {
            var expected = new global::Ssin.Belgium.Ssin(88, 10, 11, 357, 67);
            global::Ssin.Belgium.Ssin actual = default;
            Assert.DoesNotThrow(() => actual = global::Ssin.Belgium.Ssin.ParseExact("88101135767", SsinFormat.Raw));
            Assert.AreEqual(expected, actual);

            Assert.DoesNotThrow(() => actual = global::Ssin.Belgium.Ssin.ParseExact("88.10.11-357.67", SsinFormat.Formatted));
            Assert.AreEqual(expected, actual);

            Assert.Throws<FormatException>(() => actual = global::Ssin.Belgium.Ssin.ParseExact("88101135767", SsinFormat.Formatted));
            Assert.Throws<FormatException>(() => actual = global::Ssin.Belgium.Ssin.ParseExact("88.10.11-357.67", SsinFormat.Raw));
        }

        [Test]
        public void TestTryParse()
        {
            var expected = new global::Ssin.Belgium.Ssin(88, 10, 11, 357, 67);
            Assert.IsTrue(global::Ssin.Belgium.Ssin.TryParse("88101135767", out var actual));
            Assert.AreEqual(expected, actual);

            Assert.IsTrue(global::Ssin.Belgium.Ssin.TryParse("88.10.11-357.67", out actual));
            Assert.AreEqual(expected, actual);

            Assert.IsFalse(global::Ssin.Belgium.Ssin.TryParse("invalid", out actual));
            Assert.AreEqual(default(global::Ssin.Belgium.Ssin), actual);
        }

        [Test]
        public void TestTryParseExact()
        {
            var expected = new global::Ssin.Belgium.Ssin(88, 10, 11, 357, 67);
            Assert.IsTrue(global::Ssin.Belgium.Ssin.TryParseExact("88101135767", out var actual, SsinFormat.Raw));
            Assert.AreEqual(expected, actual);

            Assert.IsFalse(global::Ssin.Belgium.Ssin.TryParseExact("88101135767", out actual, SsinFormat.Formatted));
            Assert.AreEqual(default(global::Ssin.Belgium.Ssin), actual);
        }

        [TestCase("88.10.11-357.67", SsinGender.Male)]
        [TestCase("87.09.29-253.88", SsinGender.Male)]
        public void TestGetGender(string nationalNumber, SsinGender genderEnum)
        {
            var ssin = global::Ssin.Belgium.Ssin.Parse(nationalNumber);
            Assert.IsTrue(ssin.IsValid());
            Assert.IsTrue(ssin.GetGender() == genderEnum);
        }

        [TestCase("88.10.11-357.67", 1988, 10, 11)]
        [TestCase("87.09.29-253.88", 1987, 09 ,29)]
        [TestCase("19.04.23-273.75", 2019, 04 ,23)]
        public void TestGetBirthDate(string nationalNumber, int year, int month, int day)
        {
            var ssin = global::Ssin.Belgium.Ssin.Parse(nationalNumber);
            Assert.IsTrue(ssin.IsValid());
            Assert.IsTrue(ssin.GetBirthdate() == new DateTime(year, month, day));
        }
        
        [TestCase("19.00.23-273.75")]
        [TestCase("19.04.00-273.75")]
        public void TestGetBirthDateReturnsNull(string nationalNumber)
        {
            var ssin = global::Ssin.Belgium.Ssin.Parse(nationalNumber);
            Assert.IsNull(ssin.GetBirthdate());
        }

        [Test]
        public void TestUnknownBirthDateIsValid()
        {
            var ssin = new global::Ssin.Belgium.Ssin(00, 00, 01, 397, 87);
            Assert.IsTrue(ssin.IsValid());
        }
    }
}