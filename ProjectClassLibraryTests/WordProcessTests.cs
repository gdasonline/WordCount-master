using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WordClassLibrary.Tests
{
	[TestClass()]
	public class WordProcessTests
	{
		[TestMethod()]
		public void ProcessWordCountTest()
		{
			FileUtility _fileutility = new FileUtility();
			string path = @"..\..\Resources\\mobydick.txt";
			int frequencynumber = 20;
			char[] delimiters =
			{
				' ', '.', ',', ';', '\'', '-', ':', '!', '?', '(', ')', '<', '>', '=', '*', '/', '[', ']', '{', '}',
				'\\', '"',
				'\r', '\n'
			};
			string line = "**The Project Gutenberg Etext of Moby Dick, by Herman Melville**";

			var readablecontent = _fileutility.AsHumanReadable(line);
			var listofwords = _fileutility.ContentToListOfWords(readablecontent, delimiters);
			var wordcounts = _fileutility.CountWords(listofwords);

			Assert.IsNotNull(readablecontent);
			Assert.AreEqual(listofwords.Count, 10);
			Assert.AreEqual(wordcounts["the"], 1);


			//read the Test File
			var frequentlyusedwords = _fileutility.ProcessFrequentlyUsedWords(path, frequencynumber);
			//Assert frequently used words contains values
			Assert.IsNotNull(frequentlyusedwords);
			//Check The frequent word is of 4284 counts
			Assert.AreEqual(frequentlyusedwords["the"], 4284);
			//Check frequent and word is of 2192 counts
			Assert.AreEqual(frequentlyusedwords["and"], 2192);
		}
	}
}