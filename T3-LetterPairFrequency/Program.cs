using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace T3_LetterPairFrequency
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = new StreamReader("../../../input.txt");
            var inputString = input.ReadToEnd();
            var result = LetterPairFrequency(inputString);

            using ( var output = new StreamWriter("../../../output.txt"))
            {
                output.WriteLine($"Letter Pair \tFrequency");
                foreach (var pairWithFrquency in result)
                {
                    output.WriteLine($"{pairWithFrquency.Key} \t\t\t\t{pairWithFrquency.Value}");
                }
            }
            
        }

        static List<KeyValuePair<string, double>> LetterPairFrequency(string input)
        {
            
            var pairsDictionary = new Dictionary<string, double>();
            for (int i = 0; i < input.Length - 1; i ++)
            {
                var pair = input.Substring(i, 2);

                if (!Regex.IsMatch(pair, @"^[a-zA-Z]+$"))
                {
                    continue;
                }

                if (pairsDictionary.ContainsKey(pair))
                {
                    pairsDictionary[pair]++;
                }
                else
                {
                    pairsDictionary.Add(pair, 1);
                }
            }

            var tempKeys = new List<string>(pairsDictionary.Keys);
            foreach (var key in tempKeys)
            {
                pairsDictionary[key] = pairsDictionary[key] / pairsDictionary.Count;
            }

            var resultList = pairsDictionary.ToList();
            resultList.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));
            return resultList;
        }
    }
}