using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace T2_LetterFrequency
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader inputStream = new StreamReader("input.txt");
            StreamWriter outputStream = new StreamWriter("output.txt");
            
            var input = inputStream.ReadLine();
            var resultDic = LetterFrequency(input);
            for (int i = 0; i < resultDic.Count; i++)
            {
                var (key, value) = resultDic.ElementAt(i);
                resultDic[key] = value / input.Length;

                outputStream.Write(key +" | " + Math.Round(resultDic[key], 3) );
                Console.WriteLine(key +" | " + Math.Round(resultDic[key], 3) );
            }
        }

        static Dictionary<char,double> LetterFrequency(string input)
        {
            var frequencies = new Dictionary<char, double>();
            var alphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
            foreach (var letter in alphabet)
            {
                frequencies.Add(letter, 0);
            }

            foreach (var letter in input)
            {
                if (frequencies.ContainsKey(letter.ToString().ToUpper()[0]) )
                {
                    frequencies[letter.ToString().ToUpper()[0]]++;
                }
               
            }

            return frequencies;
        }
    }
}