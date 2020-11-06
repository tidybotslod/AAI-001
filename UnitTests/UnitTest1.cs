using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using AAI;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        private static KeySentiments analyzer;
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            analyzer = new KeySentiments();
        }

        [TestMethod]
        public void CheckAzureObject()
        {
            Assert.AreNotEqual(null, analyzer.AzureTextAnalyticsService);
        }

        class TestCase
        {
            internal string input;
            internal string[] answer;
        }

        private static TestCase[] keyWordTests = {
           new TestCase {
               input = "The quick brown fox jumps over the lazy dog",
               answer = new string[] {"quick brown fox jumps", "lazy dog" } },
           new TestCase {
               input = "We love this trail and make the trip every year. The views are breathtaking and well worth the hike!",
               answer = new string[] {"year", "trail", "trip", "views", "hike"} }
        };

        [DataRow(0, DisplayName = "CompleteAlphabet")]
        [DataRow(1, DisplayName = "Microsoft Documentation Example")]
        [TestMethod]
        public void performKeyWordTest(int testnumber)
        {
            TestCase testCase = keyWordTests[testnumber];

            List<String> keyWords = analyzer.KeyWords(testCase.input);
            Assert.AreNotEqual(null, keyWords);
            int i;
            for (i = 0; i < keyWords.Count; i++)
            {
                Assert.AreEqual(testCase.answer[i], keyWords[i]);
            }
            Assert.AreEqual(testCase.answer.Length, i);
        }

        private static TestCase[] SentimentTestCase = {
           new TestCase {
               input = "The quick brown fox jumps over the lazy dog",
               answer = new string[] { "Negative, 0.00, 0.99, 0.01, \"quick brown fox jumps\", \"lazy dog\"" }
           }
        };

        [TestMethod]
        public void performSentimentTest()
        {
            using (System.IO.MemoryStream memory = new System.IO.MemoryStream())
            {
                System.IO.StreamWriter writer = new System.IO.StreamWriter(memory, Console.OutputEncoding);
                analyzer.Sentiment(SentimentTestCase[0].input, writer);
                memory.Seek(0, System.IO.SeekOrigin.Begin);
                System.IO.StreamReader reader = new System.IO.StreamReader(memory, Console.InputEncoding);
                string result = reader.ReadLine();
                Assert.AreEqual(SentimentTestCase[0].answer[0], result);
            }
        }
    }
}
