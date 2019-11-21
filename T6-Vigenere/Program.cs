using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace T5_Vigenere
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputStream = new StreamReader("../../../input.txt");
            var incomingMessage = inputStream.ReadToEnd();
            var vigenereCipher = new VigenereCipher(incomingMessage, "lemon");
            vigenereCipher.PrintSquare();
            var encodedMessage = vigenereCipher.Encode(incomingMessage);
            var decodedMessage = vigenereCipher.Decode(encodedMessage);
            
            using var output = new StreamWriter("../../../output.txt");
            output.WriteLine("Encoded message:\n");
            output.WriteLine(encodedMessage);
            output.WriteLine("\nDecoded message:\n");
            output.WriteLine(decodedMessage);
            output.Close();
        }
    }
    
     class VigenereCipher
    {
        private readonly List<KeyValuePair<char, int>> shifts = new List<KeyValuePair<char, int>>();
        private readonly char[] alphabet;
        private string key;
        public VigenereCipher(string incomingMessage, string key)
        {
            this.key = key;
            incomingMessage = incomingMessage.ToLower() + key.ToLower();
            incomingMessage =  Regex.Replace(incomingMessage, @"\s+", string.Empty);
            this.alphabet = incomingMessage.ToCharArray().Distinct().ToArray();

            int shiftCounter = 0;
            foreach (var letter in alphabet)
            {
                this.shifts.Add(new KeyValuePair<char, int>(letter, shiftCounter));
                shiftCounter++;
                if (shiftCounter == alphabet.Length)
                    shiftCounter = 0;
            }
           
        }

        public void PrintSquare()
        {
            Console.Write("   ");
            foreach (var letter in this.alphabet)
            {
                Console.Write($"{letter} ");
            }

            Console.WriteLine();
            for (int i = 0; i < this.alphabet.Length + 3; i++)
            {
                Console.Write("--");
            }
            Console.WriteLine();
            string alphabetString = new string(this.alphabet);
            foreach (var (letter, i) in this.shifts)
            {
                Console.Write(letter + "| ");
                for (int j = i; j < alphabetString.Length; j++)
                {
                    Console.Write($"{alphabetString[j]} ");
                }
                for (int j = 0; j < i; j++)
                {
                    Console.Write($"{alphabetString[j]} ");
                }
                Console.WriteLine();
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
                    var currentShift = shifts.First(kvp => kvp.Key == key[keyIndex]).Value;
                    var currentIndex = Array.IndexOf(alphabet, letter);
                    if (currentIndex == -1)
                    {
                        throw new Exception($"\"{letter}\" letter wasn't found in the alphabet while encoding");
                    }
                    var resultIndex = (currentIndex + currentShift) % alphabetLength;
                    encodedString.Append(alphabet[resultIndex]);
                    
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
            StringBuilder encodedString = new StringBuilder(stringToDecode.Length);
            var alphabetLength = alphabet.Length;

            int keyIndex = 0;
            foreach (var word in words)
            {
                foreach (var letter in word)
                {
                    var currentShift = shifts.First(kvp => kvp.Key == key[keyIndex]).Value;
                    var currentIndex = Array.IndexOf(alphabet, letter);
                    if (currentIndex == -1)
                    {
                        throw new Exception($"\"{letter}\" letter wasn't found in the alphabet while decoding");
                    }
                    var resultIndex = MathMod((currentIndex - currentShift), alphabetLength);
                    encodedString.Append(alphabet[resultIndex]);
                    
                    keyIndex++;
                    if (keyIndex == key.Length)
                        keyIndex = 0;
                }

                encodedString.Append(" ");
            }
          

            return encodedString.ToString();
        }
        
        int MathMod(int a, int b) {
            return (Math.Abs(a * b) + a) % b;
        }
    }
}
