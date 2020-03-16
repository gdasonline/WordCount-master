using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;


namespace WordClassLibrary
{
	// Apply SOLID Principle - Interface Segregation Principle 

	// File Utility Interface To Read a File and Return most frequently used words
	public interface IFileUtilityService
	{
		//specification to read the given file andf return frequently used word counts
		Dictionary<string, int> ProcessFrequentlyUsedWords(string path, int mostfrequentlynumber);

		//specification convert file content to human readable content
		string AsHumanReadable(string content);
	}

	//Word Process Interface
	public interface IWordProcessUtilityService
	{
		//specification to convert contents to list of  words
		List<string> ContentToListOfWords(string content, char[] delimiters);

		//specification for counting words
		Dictionary<string, int> CountWords(List<string> listofwords);

		//specification for counting  repetitive words
		void AddOrUpdateWordCount(Dictionary<string, int> worddictionary);
	}

	//File Utility is used to implement  File Utility and Word process Services
	public class FileUtility : IFileUtilityService, IWordProcessUtilityService
	{
		//Read pattern 
		private readonly string readpattern = "[^a-zA-Z0-9 -]";

		//Delimiter to split the sentences 
		private readonly char[] delimiters =
		{
			' ', '.', ',', ';', '\'', '-', ':', '!', '?', '(', ')', '<', '>', '=', '*', '/', '[', ']', '{', '}', '\\',
			'"',
			'\r', '\n'
		};

		private readonly ConcurrentDictionary<string, int> _concurrentwordcount =
			new ConcurrentDictionary<string, int>();


		public Dictionary<string, int> ProcessFrequentlyUsedWords(string path, int mostfrequentlynumber)
		{
			Dictionary<string, int> requentlyusedwordcount;
			using (var fileStream = new System.IO.StreamReader(path))
			{
				string line;
				while ((line = fileStream.ReadLine()) != null)

				{
					//Convert Each file line to human readable lines
					var ashumanreadablecontent = AsHumanReadable(line);

					//Split readable Lines to List of words
					var listofwords = ContentToListOfWords(ashumanreadablecontent, delimiters);

					//Count words
					var wordcounts = CountWords(listofwords);

					//Add or Update word Counts
					AddOrUpdateWordCount(wordcounts);
				}

				//Find Top Frequently Used words
				requentlyusedwordcount = _concurrentwordcount.OrderByDescending(x => x.Value).Take(mostfrequentlynumber)
					.ToDictionary(x => x.Key, y => y.Value);

				return requentlyusedwordcount;
			}
		}


		//Implement file content to readable words
		public string AsHumanReadable(string content)
		{
			return Regex.Replace(Regex.Replace(content, readpattern, " ").Trim(), @"\s+", " ");
		}

		//Split the Content to list of words
		public List<string> ContentToListOfWords(string content, char[] delimiters)
		{
			var words = content.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
			//Convert to List of strings
			return words.ToList();
		}

		//Implement  for counting frequent words
		public void AddOrUpdateWordCount(Dictionary<string, int> worddictionary)
		{
			foreach (var word in worddictionary)
			{
				_concurrentwordcount.AddOrUpdate(word.Key, word.Value,
					(key2, currentValue2) => currentValue2 + word.Value);
			}
		}
		//Implement counting words
		public Dictionary<string, int> CountWords(List<string> listofwords)
		{
			return listofwords.GroupBy(str => str.ToLower())
				.ToDictionary(group => group.Key, group => group.Count());
		}

		
	}
}