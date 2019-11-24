using System;
using System.Collections;
using System.IO;
using System.Text;

namespace T12_LfsrXor
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
        private readonly byte seed;

        public XorCipher()
        {
            var rand = new System.Random();
            this.seed = (byte)rand.Next(200);
        }
        bool LFSR(byte seed)
        {
            bool x1 = getBit(seed, 1);
            bool x2 = getBit(seed, 2);
            bool x3 = getBit(seed, 3);
            bool x5 = getBit(seed, 5);
            bool x8 = getBit(seed, 8);

            return x1 ^ x2 ^ x3 ^ x5 ^ x8;
        }

        byte nextRandomByte(byte seed)
        {
            byte nextSeed = seed;
            string stringSeed = "";
            for (int i = 0; i < 8; i++)
            {
                var newBit = LFSR(nextSeed);
                nextSeed = (byte)(nextSeed << 1);
                if (newBit)
                {
                    nextSeed += 1;
                }
            }

            return seed;
        }
        
        public static bool getBit(byte b, int bitNumber)
        {
            return (b & (1 << bitNumber-1)) != 0;
        }
        
        public string Encode(string stringToEncode)
        {
            byte[] stringToEncodeBytes = Encoding.ASCII.GetBytes(stringToEncode);
            byte[] result = new byte[stringToEncodeBytes.Length];

            var nextSeed = seed;

            for (int i = 0; i < stringToEncodeBytes.Length; i++)
            {
                var nexByte = nextRandomByte(nextSeed);
                result[i] = (byte) (stringToEncodeBytes[i] ^ nexByte);
                nextSeed = nexByte;
            }

            return Encoding.ASCII.GetString(result);
        }
        
    }
}