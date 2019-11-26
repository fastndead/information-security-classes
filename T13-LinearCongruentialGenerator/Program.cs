using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace T13_LinearCongruentialGenerator
{
     class Program
    {
        static void Main(string[] args)
        {
            var inputStream = new StreamReader("../../../input.txt");
            var incomingMessage = inputStream.ReadToEnd();
            var xorCipher = new XorCipher();
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
        private readonly int seed;
        private readonly int a;
        private readonly int c;
        private readonly int m;

        public XorCipher()
        {
            var rand = new System.Random();
            this.seed = 15;
            this.a = 22;
            this.c = 17;
            this.m = 37;
        }
        
        int mathMod(int a, int b) {
            return (Math.Abs(a * b) + a) % b;
        }

        int nextRandomNumber(int seed)
        {
            return mathMod(a * seed + c, m);
        }

        public string Encode(string stringToEncode)
        {
            byte[] stringToEncodeBytes = Encoding.ASCII.GetBytes(stringToEncode);
            byte[] result = new byte[stringToEncodeBytes.Length];

            var nextSeed = seed;

            for (int i = 0; i < stringToEncodeBytes.Length; i++)
            {
                var nexByte = nextRandomNumber(nextSeed);
                result[i] = (byte) (stringToEncodeBytes[i] ^ (byte)nexByte);
                nextSeed = nexByte;
            }

            return Encoding.ASCII.GetString(result);
        }
        
    }
}