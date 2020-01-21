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
        }
    }
}