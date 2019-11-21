using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace T4_Polybius_square
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputStream = new StreamReader("../../../input.txt");
            var incomingMessage = inputStream.ReadToEnd();
            var polybiusCipher = new PolybiusCipher(incomingMessage);
            
            var encodedString = polybiusCipher.Encode(incomingMessage);
            var decodedString = polybiusCipher.Decode(encodedString);
           
            using var output = new StreamWriter("../../../output.txt");
            output.WriteLine("Encoded message:\n");
            output.WriteLine(encodedString);
            output.WriteLine("\nDecoded message:\n");
            output.WriteLine(decodedString);
            output.Close();
        }
    }

    class PolybiusCipher
    {
        private readonly char[,] square;
        public PolybiusCipher(string incomingMessage)
        {
            incomingMessage = incomingMessage.ToLower();
            incomingMessage =  Regex.Replace(incomingMessage, @"\s+", string.Empty);
            char[] alphabet = incomingMessage.ToCharArray().Distinct().ToArray();
            int dimension = (int)Math.Ceiling(Math.Sqrt(alphabet.Length));

            square = new char[dimension, dimension];

            int i = 0;
            while(i < dimension){
                for (int j = 0; j < dimension; j++)
                {
                    if ((i * dimension + j) >= alphabet.Length)
                    {
                        break;
                    }
                    square[i, j] = alphabet[i * dimension + j];
                }

                i++;
            }
        }

        public void Print()
        {
            for (int i = 0; i < square.GetLength(0); i++)
            {
                for (int j = 0; j < square.GetLength(1); j++)
                {
                    Console.Write($"{square[i,j]} ");
                }
                Console.WriteLine();
            }
        }

        public string Encode(string stringToEncode)
        {
            stringToEncode = stringToEncode.ToLower();
            StringBuilder encodedString = new StringBuilder(stringToEncode.Length);
            
            int w = square.GetLength(0); 
            int h = square.GetLength(1);

            foreach (var letter in stringToEncode)
            {
                if (Regex.IsMatch(letter.ToString(), @"\s+"))
                {
                    encodedString.Append(" ");
                    continue;
                }

                for (int x = 0; x < w; ++x)
                {
                    for (int y = 0; y < h; ++y)
                    {
                        if (square[x, y].Equals(letter))
                            encodedString.Append($"{x}{y}");
                    }
                }
            }            
            

            return encodedString.ToString();
        }

        public string Decode(string stringToDecode)
        {
            stringToDecode = stringToDecode.ToLower();
            string[] words = Regex.Split(stringToDecode, @"\s+");
            StringBuilder decodedString = new StringBuilder(stringToDecode.Length / 2);
            
            int w = square.GetLength(0); 
            int h = square.GetLength(1);
            
            foreach (var word in words)
            {
                if (word.Length % 2 != 0)
                {
                    throw new Exception($"Provided string is incorrectly encoded.| Every word should have even number of letters. This word caused problems: \n{word}\nThis is what I have so far: {decodedString}");
                }

                for (int i = 0; i < word.Length; i += 2)
                {
                    if (!char.IsDigit(word[i]) || !char.IsDigit(word[i + 1]))
                        throw new Exception("Provided string is incorrectly encoded.| Unsupported characters in string. Every word should be an even number of digits");

                    decodedString
                        .Append(square[int.Parse(word[i].ToString()), int.Parse(word[i + 1].ToString())]);
                }

                decodedString.Append(" ");
            }
            return decodedString.ToString();
        }
    }
}