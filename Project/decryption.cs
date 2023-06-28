using encription;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace IDEA
{

    class decryption
    {


        public static string longToBinary(long num)
        {
            Dictionary<long, string> dec = new Dictionary<long, string>();
            dec[1] = "0001"; dec[2] = "0010"; dec[3] = "0011";
            dec[4] = "0100"; dec[5] = "0101"; dec[6] = "0110";
            dec[7] = "0111"; dec[8] = "1000"; dec[9] = "1001";
            dec[10] = "1010"; dec[11] = "1011"; dec[12] = "1100";
            dec[13] = "1101"; dec[14] = "1110"; dec[15] = "1111";
            dec[16] = "0000"; dec[0] = "0000";


            return dec[num];
        }






        static string moduloMultiplication(long a, long b)
        {
            if (a == 0) a = 16;
            if (b == 0) b = 16;
            long res = (a * b) % (17);

            return longToBinary(res);
        }



        static string moduloAddition(long a, long b)
        {


            long x = (a + b) % (16);


            return longToBinary(x);
        }
        static long ConvertTooDecmial(string num, int flag = 0)
        {
            Dictionary<string, long> dec = new Dictionary<string, long>();
            dec["0001"] = 1; dec["0010"] = 2; dec["0011"] = 3;
            dec["0100"] = 4; dec["0101"] = 5; dec["0110"] = 6;
            dec["0111"] = 7; dec["1000"] = 8; dec["1001"] = 9;
            dec["1010"] = 10; dec["1011"] = 11; dec["1100"] = 12;
            dec["1101"] = 13; dec["1110"] = 14; dec["1111"] = 15;
            if (flag == 0)
                dec["0000"] = 0;
            else
                dec["0000"] = 16;

            return dec[num];
        }


        static string xor(long a, long b)
        {

            return longToBinary((a ^ b));
        }

        static string readfile(string filename)
        {
            // Give the path of the text file
            string filePath = filename;

            // Reading text file
            string fileContent = File.ReadAllText(filePath);

            return fileContent;
        }
        public static string function(string s, string t)
        {

            string Key = t; // 32 bit 
            string TextPlain = readfile(s); // 16  bit 
            var SpiltText = TextPlain.Spilt(16);   // spilt all text to 16 bit only 


            var BinaryText = new List<List<string>>();

            int cnt = 0;
            foreach (var text in SpiltText)
            {

                cnt++;
                string CompleteBinaryPartitionText = text.PadRight(16);

                BinaryText.Add(CompleteBinaryPartitionText.Spilt(4));


            }
            // Console.WriteLine(cnt);


            var List_Key_28 = Key.GenerateAllKeys();
            List_Key_28 = List_Key_28.GenerateInverseKeys();



            List<List<string>> Encriptionlist = new List<List<string>>();




            List<string> List_key_28 = new List<string>();
            for (int i = 0; i < 28; i++)
            {
                List_key_28.Add(List_Key_28[i]);
            }

            for (int j = 0; j < cnt; j++)
            {

                for (int i = 0; i < 28; i++)
                {
                    List_Key_28[i] = List_key_28[i];
                }

                for (int i = 0; i < 4; i++)
                {
                    List<long> blocksText = new List<long>();
                    List<long> blocksKey = new List<long>();
                    for (int p = 0; p < 4; p++)
                    {
                        //Console.WriteLine(BinaryText[j][p]);

                        long pp = ConvertTooDecmial(BinaryText[j][p]);

                        blocksText.Add(pp);
                    }
                    for (int k = 0; k < 6; k++)
                    {
                        //Console.WriteLine(BinaryText[j][p]);
                        long kk = ConvertTooDecmial(List_Key_28[k + i * 6]);

                        blocksKey.Add(kk);
                    }


                    string step1 = moduloMultiplication(blocksText[0], blocksKey[0]);

                    string step2 = moduloAddition(blocksText[1], blocksKey[1]);
                    string step3 = moduloAddition(blocksText[2], blocksKey[2]);
                    string step4 = moduloMultiplication(blocksText[3], blocksKey[3]);


                    string step5 = xor(ConvertTooDecmial(step1), ConvertTooDecmial(step3));
                    string step6 = xor(ConvertTooDecmial(step2), ConvertTooDecmial(step4));


                    string step7 = moduloMultiplication(ConvertTooDecmial(step5, 1), blocksKey[4]);

                    string step8 = moduloAddition(ConvertTooDecmial(step6), ConvertTooDecmial(step7));

                    string step9 = moduloMultiplication(ConvertTooDecmial(step8, 1), blocksKey[5]);

                    string step10 = moduloAddition(ConvertTooDecmial(step7), ConvertTooDecmial(step9));

                    // final result 
                    string step11 = xor(ConvertTooDecmial(step1), ConvertTooDecmial(step9));
                    string step12 = xor(ConvertTooDecmial(step3), ConvertTooDecmial(step9));
                    string step13 = xor(ConvertTooDecmial(step2), ConvertTooDecmial(step10));
                    string step14 = xor(ConvertTooDecmial(step4), ConvertTooDecmial(step10));


                    if (i == 2)
                        Console.WriteLine(moduloMultiplication(blocksText[3], blocksKey[3]));


                    BinaryText[j][0] = step11;
                    BinaryText[j][1] = step13;
                    BinaryText[j][2] = step12;
                    BinaryText[j][3] = step14;


                }

                string step15 = moduloMultiplication(ConvertTooDecmial(BinaryText[j][0], 1), ConvertTooDecmial(List_Key_28[24], 1));
                string step16 = moduloAddition(ConvertTooDecmial(BinaryText[j][1]), ConvertTooDecmial(List_Key_28[25]));
                string step17 = moduloAddition(ConvertTooDecmial(BinaryText[j][2]), ConvertTooDecmial(List_Key_28[26]));
                string step18 = moduloMultiplication(ConvertTooDecmial(BinaryText[j][3], 1), ConvertTooDecmial(List_Key_28[27], 1));
                List<string> list = new List<string>();
                list.Add(step15);
                list.Add(step16);
                list.Add(step17);
                list.Add(step18);

                Encriptionlist.Add(list);



            }

            string ans = "";
            foreach (var item in Encriptionlist)
            {

                foreach (var p in item)
                {
                    ans = string.Join(p, item);
                }

            }
            File.WriteAllText(@"D:\file.txt", ans);
            return ans;



        }


    }
}