using System;
using WordClassLibrary;

//Program will accept a File as parameter and will read the file
// 20 most frequently used words in the file in  will be printed as the output

public class WordCount
{
	private static readonly int frequencynumber = 20;

	static void Main(string[] args)
	{
		if (args.Length == 0)
		{
			Console.WriteLine("File Argument is missing...");
			return;
		}

		try
		{
			string path = args[0];
			//Crate the file processing utility class
			var fileutility = new FileUtility();

			//read the given File
			var frequentlyusedwords = fileutility.ProcessFrequentlyUsedWords(path, frequencynumber);

			//Print the Top 20 Frequent word counts
			if (frequentlyusedwords != null)

			{
				//Display the Word frequency counts
				foreach (var word in frequentlyusedwords)
				{
					Console.WriteLine("{0} {1}", word.Key, word.Value);
				}
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine(string.Format("Exception Occured:{0}", ex.ToString()));
		}
	}
}