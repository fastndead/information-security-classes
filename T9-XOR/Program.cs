using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;

namespace T9_XOR
{
     class Program
    {
        static void Main(string[] args)
        {
            var inputStream = new StreamReader("../../../input.txt");
            var incomingMessage = inputStream.ReadToEnd();
            var key = "lemon";
            var xorCipher = new XorCipher(incomingMessage, key);
            var encodedMessage = xorCipher.Encode(incomingMessage);
            var decodedMessage = xorCipher.Encode(encodedMessage);
            
            using var output = new StreamWriter("../../../output.txt");
            output.WriteLine("Encoded message:");
            output.WriteLine(encodedMessage);
            output.WriteLine("Decoded message:");
            output.WriteLine(decodedMessage);
            output.Close();
        }
    }
    
     class XorCipher
    {
        private readonly char[] key;
        public XorCipher(string incomingMessage, string key)
        {
            this.key = key.ToLower().ToCharArray();

        }

        public string Encode(string stringToEncode)
        {
            byte[] keyByters = Encoding.ASCII.GetBytes(key);
            byte[] stringToEncodeBytes = Encoding.ASCII.GetBytes(stringToEncode);
            byte[] result = new byte[stringToEncode.Length];

            for (int i = 0; i < stringToEncodeBytes.Length; i++)
            {
                result[i] = (byte)(stringToEncodeBytes[i] ^ keyByters[i % key.Length]);
            }

            return Encoding.ASCII.GetString(result);
        }
    }
}