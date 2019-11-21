using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            var cesarWithKeyCipher = new CesarWithKeyCipher(incomingMessage, "lemon");
            var encodedMessage = cesarWithKeyCipher.Encode(incomingMessage);
            var decodedMessage = cesarWithKeyCipher.Decode(encodedMessage);
            
            using var output = new StreamWriter("../../../output.txt");
            output.WriteLine("Encoded message:");
            output.WriteLine(encodedMessage);
            output.WriteLine("Decoded message:");
            output.WriteLine(decodedMessage);
            output.Close();
        }
    }
    
     class CesarWithKeyCipher
    {
        private readonly List<KeyValuePair<char, int>> shifts = new List<KeyValuePair<char, int>>();
        private readonly char[] alphabet;
        private readonly int[] key;
        public CesarWithKeyCipher(string incomingMessage, string key)
        {
            incomingMessage = incomingMessage.ToLower() + key.ToLower();
            incomingMessage =  Regex.Replace(incomingMessage, @"\s+", string.Empty);
            this.alphabet = incomingMessage.ToCharArray().Distinct().ToArray();
            this.key = new int[key.Length];
            for (int i = 0; i < key.Length; i++)
            {
                this.key[i] = Array.IndexOf(alphabet, key[i]);
            }
        }

        public string Encode(string stringToEncode)
        {
            stringToEncode = stringToEncode.ToLower();
            string[] words = Regex.Split(stringToEncode, @"\s+");
            StringBuilder encodedString = new StringBuilder(stringToEncode.Length);
            var alphabetLength = alphabet.Length;
            int keyIndex = 0;
            foreach (var word in words)
            {
                foreach (var letter in word)
                {
                    var encodedChar = alphabet[mathMod(Array.IndexOf(alphabet, letter) + key[keyIndex], alphabetLength)];
                    encodedString.Append(encodedChar);
                    
                    keyIndex++;
                    if (keyIndex == key.Length)
                        keyIndex = 0;
                }

                encodedString.Append(" ");
            }
          

            return encodedString.ToString();
        }

        public string Decode(string stringToDecode)
        {
            stringToDecode = stringToDecode.ToLower();
            string[] words = Regex.Split(stringToDecode, @"\s+");
            StringBuilder decodedString = new StringBuilder(stringToDecode.Length);
            var alphabetLength = alphabet.Length;
            int keyIndex = 0;
            foreach (var word in words)
            {
                foreach (var letter in word)
                {
                    var decodedChar = alphabet[mathMod(Array.IndexOf(alphabet, letter) + alphabetLength - key[keyIndex], alphabetLength)];
                    decodedString.Append(decodedChar);
                    
                    keyIndex++;
                    if (keyIndex == key.Length)
                        keyIndex = 0;
                }

                decodedString.Append(" ");
            }
          

            return decodedString.ToString();
        }
        
        int mathMod(int a, int b) {
            return (Math.Abs(a * b) + a) % b;
        }
    }
}