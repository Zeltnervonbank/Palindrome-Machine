using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Palindrome_Machine
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            const string dictionaryPath = "enres.txt";
            Regex pattern = new Regex("^[a-zA-z']*$");

            List<string> words = File.ReadAllLines(dictionaryPath).Distinct().Where(x => pattern.Match(x).Success && x.Replace("'", "").Length > 3).ToList();

            FindTwoWordPalindromes(words);
            
        }

        private static void FindThreeWordPalindromes(List<string> words)
        {
            throw new NotImplementedException();
        }

        private static void FindTwoWordPalindromes(List<string> words)
        {
            Console.WriteLine("Preprocessing candidates");
            Dictionary<string, List<string>> possibleCandidates = new Dictionary<string, List<string>>();

            foreach (string word in words)
            {
                string simplified = SimplifyString(word);
                string key = simplified.Substring(simplified.Length - 2);

                if (!possibleCandidates.ContainsKey(key))
                {
                    possibleCandidates.Add(key, new List<string>());
                }
                possibleCandidates[key].Add(word);
            }

            Console.WriteLine("Finished preprocessing");

            foreach (string word in words)
            {
                if (!possibleCandidates.ContainsKey(GetKeyFromWord(word)))
                {
                    continue;
                }

                List<string> matchingWords = possibleCandidates[GetKeyFromWord(word)].Where(secondWord => IsPalindrome(SimplifyString(word + secondWord))).ToList();

                foreach (string match in matchingWords)
                {
                    Console.WriteLine($"{word} {match}");
                    using (StreamWriter sw = File.AppendText("Two word palindromes.txt"))
                    {
                        sw.WriteLine($"{word} {match}");
                    }
                }
            }
        }

        private static string GetKeyFromWord(string word)
        {
            string simplified = SimplifyString(word);
            return string.Concat(simplified[1], simplified[0]);
        }

        private static string SimplifyString(string s)
        {
            return s.Replace("'", "").ToLowerInvariant();
        }

        private static bool IsPalindrome(string word)
        {
            if (word.Length <= 1)
            {
                return true;
            }

            if (word[0] != word[word.Length - 1])
            {
                return false;
            }

            return IsPalindrome(word.Substring(1, word.Length - 2));
        }

    }
}
