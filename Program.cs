using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace IntProj1
{
    class Program
    {
        static void Main(string[] args)
        {
           var fileText = File.ReadAllText(@"D:\wordlist.txt");
            string[] words=Regex.Split(fileText,"\r\n");
            var sortedWords = from arrElement in words
                              orderby arrElement.Length descending 
                                 select arrElement.ToArray();
          //  The longest word in the file that can be constructed by concatenating copies of shorter words also found in the file. 
            string longest = FindLongestWords(words);
            //The program should then go on to report the 2nd longest word found
            string secondLongestWord = FindSecondLongestWords(words);
            //Total count of how many of the words in the list can be constructed of other words in the list
            int totalNoOfConcWords = GetAllConctWords(words);

            MessageBox.Show("LongestWord:"+longest+"\r\n second Longest Word:"+secondLongestWord+"\r\n Total No Of ConcatWords:"+totalNoOfConcWords);
        }
        
        //Method to find out the longest word
        public static string FindLongestWords(string[] listOfWords)
        {
            if (listOfWords == null) throw new ArgumentException("listOfWords");
            
            //Order in descending order to speed up the process of finding longest word
            var sortedWords = listOfWords.OrderByDescending(word => word.Length).ToList();
            
            //creating a hashset for the sorted words
            var dict = new HashSet<String>(sortedWords);
            foreach (var word in sortedWords)
            {
                //Checking wether word is made up of the words in the hashset or not 
                if (isMadeOfWords(word, dict))
                {
                    return word;
                }
            }
            return null;
        }

        //Method to Check wether word is made up of the words in the hashset or not 
        private static bool isMadeOfWords(string word, HashSet<string> dict)
        {
            if (String.IsNullOrEmpty(word)) return false;
            if (word.Length == 1)
            {
                if (dict.Contains(word)) return true;
                else return false;
            }
            //Generate possible combinations of sub-words in word
            foreach (var pair in generatePairs(word))
            {
                //check wether the strings in the pair is part of the hashset
                if (dict.Contains(pair.Item1))
                {
                    if (dict.Contains(pair.Item2))
                    {
                        return true;
                    }
                    else
                    {
                        return isMadeOfWords(pair.Item2, dict);
                    }
                }
            }
            return false;
        }

        //Split word length wise to create possible combination of sub-words and return as pair of strings
        private static List<Tuple<string, string>> generatePairs(string word)
        {
            var output = new List<Tuple<string, string>>();
            for (int i = 1; i < word.Length; i++)
            {
                output.Add(Tuple.Create(word.Substring(0, i), word.Substring(i)));
            }
            return output;
        }
        
        
//Sam logic as FindLongestWords,included a counter to skip first longest word
        public static string FindSecondLongestWords(string[] listOfWords)
        {
            int concatinedWordCount = 0;
            if (listOfWords == null) throw new ArgumentException("listOfWords");
            var sortedWords = listOfWords.OrderByDescending(word => word.Length).ToList();
            var dict = new HashSet<String>(sortedWords);
            foreach (var word in sortedWords)
            {
                if (isMadeOfWords(word, dict))
                {
                    concatinedWordCount++;
                    if(concatinedWordCount==2)
                    return word;
                }
            }
            return null;
        }
        
        //Sam logic as FindLongestWords,included a counter to keep track of concatinated words
        public static int GetAllConctWords(string[] listOfWords)
        {
            int concatinedWordCount = 0;
            if (listOfWords == null) throw new ArgumentException("listOfWords");
            var sortedWords = listOfWords.OrderByDescending(word => word.Length).ToList();
            var dict = new HashSet<String>(sortedWords);
            foreach (var word in sortedWords)
            {
                if (isMadeOfWords(word, dict))
                {
                    concatinedWordCount++;
                       
                }
            }
            return concatinedWordCount;
        }
    }
}
