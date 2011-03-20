using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SukhotinsAlgorithm
{
    /// <summary>
    /// An implementation of Sukhotin's Algorithm for vowel identification.
    /// Based on the psuedo-code article by Matia Giovannini:
    /// http://alaska-kamtchatka.blogspot.com/2010/07/sukhotins-algorithm.html
    ///
    /// Default English wordlist taken from: http://www.mieliestronk.com/wordlist.html
    ///
    /// Note: I assume you're using the English alphabet of 26 letters, just to make
    ///       it easier on me.
    ///
    /// Author: Wesley Tansey (wes@nashcoding.com)
    /// Date: July 7, 2010
    /// </summary>
    class Program
    {
        const string DEFAULT_WORDS_FILE = "words.txt";
        static void Main(string[] args)
        {
            //the first argument is the words file, or the default words.txt file if no arguments are given
            var wordsFile = args.Length == 0 ? DEFAULT_WORDS_FILE : args[0];

            //read in the words from file
            string[] words;
            using (TextReader reader = new StreamReader(wordsFile))
                words = reader.ReadAllLines();

            //frequency of each letter, called 'r' in the psuedo-code
            int[] freqs = new int[26];

            //symmetric digraph frequencies, called 'd' in the psuedo-code
            int[,] sdFreqs = new int[26, 26];

            //calculate frequencies
            foreach (var word in words)
                for (int i = 0; i < word.Length; i++)
                {
                    //skip words with hyphens -- just an issue with my default word dictionary
                    if (word.Contains('-'))
                        continue;

                    //get the array index of this character
                    int cur = word[i] - 'a';

                    //increment the overall frequency of this letter
                    freqs[cur]++;

                    //increment the frequency of the current character following the previous character
                    if (i > 0)
                    {
                        int prev = word[i - 1] - 'a';
                        sdFreqs[cur, prev]++;

                        //we're generating a symmetric matrix, so we need to increment the other
                        //side unless we're on the diagnol
                        if (cur != prev)
                            sdFreqs[prev, cur]++;
                    }
                }

            //calculate the letter with maximum frequency, called 'r.m' in the psuedo-code
            int maxFreq = 0;
            for (int i = 0; i < freqs.Length; i++)
                if (freqs[maxFreq] < freqs[i])
                    maxFreq = i;

            //The stack of vowels to find
            Stack<int> v = new Stack<int>();

            //calculate the vowels
            while (freqs[maxFreq] > 0)
            {
                //the highest frequency letter is a vowel
                v.Push(maxFreq);

                //the next maximum frequency to find, called 'j' in the psuedo-code
                int nextMaxFreq = 0;

                //remove all occurences of the current vowel
                for (int i = 0; i < freqs.Length; i++)
                {
                    if (i != maxFreq)
                        freqs[i] -= 2 * sdFreqs[i, maxFreq];
                    else
                        freqs[i] = 0;

                    if (freqs[nextMaxFreq] < freqs[i])
                        nextMaxFreq = i;
                }

                maxFreq = nextMaxFreq;
            }

            //print out the results
            Console.WriteLine("Vowels found are:");
            foreach (var i in v)
                Console.WriteLine((char)('a' + i));
        }
    }
}
