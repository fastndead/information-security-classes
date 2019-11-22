using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

namespace T10_ColumnarTransposition
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputStream = new StreamReader("../../../input.txt");
            var incomingMessage = inputStream.ReadToEnd();
            const string key = "lemon";
            var columnarTranspositionCipher = new ColumnarTranspositionCipher(incomingMessage, key);
            var encodedMessage = columnarTranspositionCipher.Encode(incomingMessage);
            var decodedMessage = columnarTranspositionCipher.Decode(encodedMessage);
            
            using var output = new StreamWriter("../../../output.txt");
            output.WriteLine("Encoded message:");
            output.WriteLine(encodedMessage);
            output.WriteLine("Decoded message:");
            output.WriteLine(decodedMessage);
            output.Close();
        }
    }
    
     class ColumnarTranspositionCipher
    {
        private readonly char[] alphabet;
        private readonly string key;
        public ColumnarTranspositionCipher(string incomingMessage, string key)
        {
            incomingMessage = incomingMessage.ToLower() + key.ToLower();
            incomingMessage =  Regex.Replace(incomingMessage, @"\s+", string.Empty);
            this.alphabet = incomingMessage.ToCharArray().Distinct().ToArray();
            if(key.Distinct().Count() != key.Length)
                throw new Exception("Key should not contain repeated characters");
            this.key = key.ToLower();
        }

        public string Encode(string stringToEncode)
        {
            stringToEncode = stringToEncode.ToLower();
            StringBuilder encodedString = new StringBuilder(stringToEncode.Length);
            (char[][] matrix, int rowsCount, int columnCount) = getMatrixFromString(stringToEncode);

            
            KeyValuePair<int, int>[] shifts = getShifts();

            char[][] resultMatrix = new char[matrix.Length][];
            for (int i = 0; i < shifts.Length; i++)
            {
                resultMatrix[shifts[i].Value] = matrix[shifts[i].Key];
            }
            return getStringFromMatrix(resultMatrix, rowsCount, columnCount);
        }
        
        public string Decode(string stringToEncode)
        {
            stringToEncode = stringToEncode.ToLower();
            (char[][] matrix, int rowsCount, int columnCount) = getMatrixFromString(stringToEncode);
            
            KeyValuePair<int, int>[] shifts = getShifts("reverse");
            
            char[][] resultMatrix = new char[matrix.Length][];
            for (int i = 0; i < shifts.Length; i++)
            {
                resultMatrix[shifts[i].Value] = matrix[shifts[i].Key];
            }
            return getStringFromMatrix(resultMatrix, rowsCount, columnCount);
        }

        string getStringFromMatrix(char[][] matrix, int rowsCount, int columnCount)
        {
            StringBuilder resultString = new StringBuilder();

            for (int j = 0; j < rowsCount; j++)
            {
                for (int i = 0; i < columnCount; i++)
                {
                    resultString.Append(matrix[i][j]);
                }
            }
            return resultString.ToString();
        }

        Tuple<char[][], int, int> getMatrixFromString(string stringToEncode)
        {
            int rowsCount = (int)Math.Ceiling((double)stringToEncode.Length / (double)key.Length);
            int columnCount = key.Length;
            char[][] matrix = new char[columnCount][];
            for (int i = 0; i < columnCount; i++)
            {
                matrix[i] = new char[rowsCount];
            }
            var stringToEncodeEnumerator = stringToEncode.GetEnumerator();
            
            for (int j = 0; j < rowsCount; j++)
            {
                for (int i = 0; i < columnCount; i++)
                {
                    if (stringToEncodeEnumerator.MoveNext())
                    {
                        matrix[i][j] = stringToEncodeEnumerator.Current;
                    }
                    else
                    {
                        matrix[i][j] = ' ';
                    }
                }
            }
            return new Tuple<char[][], int, int>(matrix, rowsCount, columnCount);
        }

        KeyValuePair<int,int>[] getShifts(string type = "normal")
        {
            string sortedKey = String.Concat(key.OrderBy(c => c));
            KeyValuePair<int,int>[] shifts = new KeyValuePair<int, int>[key.Length];
            if (type == "normal")
            {
                for (int i = 0; i < key.Length; i++)
                {
                    shifts[i] = new KeyValuePair<int, int>(i, sortedKey.IndexOf(key[i]));
                }
            } else if (type == "reverse")
            {
                for (int i = 0; i < sortedKey.Length; i++)
                {
                    shifts[i] = new KeyValuePair<int, int>(i, key.IndexOf(sortedKey[i]));
                }
            }
            return shifts;
        }
    }
}