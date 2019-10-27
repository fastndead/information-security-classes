using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace IPS
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader streamReader = new StreamReader("input.txt");
            var input = streamReader.ReadLine();
            string cyphered;

            try
            {
                Console.WriteLine("Зашифрованный текст:");
                cyphered = CesarCypher.Cypher(input, 5, 29);
                Console.WriteLine(cyphered);
                Console.WriteLine("Расшифрованный текст:");
                Console.WriteLine(CesarCypher.Cypher(cyphered,5,29,false));

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            

        }

        static class CesarCypher
        {
    
            
            static public string alphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ ,.!?1234567890";
            

            static int mod(int x, int m) {
                return (x%m + m)%m;
            }


            public static string Cypher(string input, int n, int k, bool type = true)
            {
                int m = alphabet.Length;
                char[] initial = input.ToCharArray();
                if (type)
                {
                    
                    for (var i = 0; i < initial.Length; i++)
                    {
                        int numberOfChar = alphabet.IndexOf(input[i], StringComparison.CurrentCultureIgnoreCase);
                        if (numberOfChar < 0)
                        {
                            throw new Exception("Буква '"+ input[i] +"' не принадлежит алфавиту");
                        }
                        initial[i] = alphabet[mod((numberOfChar * n + k), m)];
                    }
                }
                else
                {
                    int n_inv = 0;
                    int flag;
                    for (var j = 0; j < m; j++) 
                    { 
                        flag = mod(n * j,m); 
          
                            
                        if (flag == 1) 
                        {  
                            n_inv = j;
                            break;
                                
                        } 
                    } 
                    for (var i = 0; i < initial.Length; i++)
                    {
                        int numberOfChar = alphabet.IndexOf(input[i], StringComparison.CurrentCultureIgnoreCase);
                        if (numberOfChar < 0)
                        {
                            throw new Exception("Буква '"+ input[i] +"' не принадлежит алфавиту");
                        }
                        int neededIndex = mod((numberOfChar - k)*n_inv, m);
                        initial[i] = alphabet[neededIndex];
                    }
                }

                var result = new string(initial);

                return result;
            }
        }

        
    }
}