using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;

namespace T6_CesarWithKey
{
     class Program
    {
        static void Main(string[] args)
        {
            var inputStream = new StreamReader("../../../input.txt");
            var incomingMessage = inputStream.ReadToEnd();
            var cesarWithKeyCipher = new PairCipher(incomingMessage);
            var encodedMessage = cesarWithKeyCipher.Encode(incomingMessage);
            var decodedMessage = cesarWithKeyCipher.Encode(encodedMessage);
            
            using var output = new StreamWriter("../../../output.txt");
            output.WriteLine("Encoded message:");
            output.WriteLine(encodedMessage);
            output.WriteLine("Decoded message:");
            output.WriteLine(decodedMessage);
            output.Close();
        }
    }
    
     class PairCipher
    {
        private readonly char[] alphabet;
        private readonly char[] alphabetWithoutKeyLetters;
        private readonly char[] key;
        public PairCipher(string incomingMessage)
        {
            incomingMessage = incomingMessage.ToLower();
            incomingMessage =  Regex.Replace(incomingMessage, @"\s+", string.Empty);
            this.alphabet = incomingMessage.ToCharArray().Distinct().ToArray();
            this.key = new char[(alphabet.Length / 2)];
            var rand = new Random();
            for (int i = 0; i < this.key.Length; i++)
            {
                this.key[i] = alphabet[rand.Next(0, alphabet.Length)];
            }

            this.alphabetWithoutKeyLetters = alphabet.Where(val => Array.IndexOf(key, val) == -1).ToArray();
            Console.WriteLine(alphabetWithoutKeyLetters);
        }

        public string Encode(string stringToEncode)
        {
            stringToEncode = stringToEncode.ToLower();
            string[] words = Regex.Split(stringToEncode, @"\s+");
            StringBuilder encodedString = new StringBuilder(stringToEncode.Length);

            foreach (var word in words)
            {
                foreach (var letter in word)
                {
                    var indexOfLetterInKey = Array.IndexOf(key, letter);
                    var indexOfLetterNotInKey = Array.IndexOf(alphabetWithoutKeyLetters, letter);
                    if (indexOfLetterInKey != -1)
                    {
                        encodedString.Append(indexOfLetterInKey > alphabetWithoutKeyLetters.Length - 1
                            ? letter
                            : alphabetWithoutKeyLetters[indexOfLetterInKey]);
                    }
                    else if( indexOfLetterNotInKey != -1)
                    {
                        encodedString.Append(indexOfLetterNotInKey > key.Length - 1
                            ? letter
                            : key[indexOfLetterNotInKey]);
                    }
                    else
                    {
                        throw new Exception($"Unsupported characters in incoming message: {letter}");
                    }
                }

                encodedString.Append(" ");

            }
            return encodedString.ToString();
        }
        
    }
}