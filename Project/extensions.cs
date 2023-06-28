using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace encription
{
    internal static class extensions
    {

        public static List<string> Spilt(this string text, int stop)
        {
            var partitions = new List<string>();
            List<char> part = new List<char>();
            for (int i = 1; i <= text.Length; i++)
            {
                part.Add(text[i - 1]);
                if (i % stop == 0)
                {
                    var joinedToString = string.Join("", part);
                    partitions.Add(joinedToString);
                    part.Clear();
                }
            }

            if (text.Length % 8 != 0) partitions.Add(string.Join("", part));
            return partitions;
        }



        public static string StringToBinary(string data)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in data.ToCharArray())
            {
                sb.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
            }
            return sb.ToString();
        }

        public static string BinaryToString(string data)
        {
            List<Byte> byteList = new List<Byte>();

            for (int i = 0; i < data.Length; i += 8)
            {
                byteList.Add(Convert.ToByte(data.Substring(i, 8), 2));
            }
            return Encoding.Unicode.GetString(byteList.ToArray());
        }


        public static string ShiftLeftone(string s, int count)
        {
            char[] chars = s.ToCharArray();
            Array.Reverse(chars);
            string ss;
            ss = new string(chars);
            return s.Substring(count, s.Length - 1) + s.Substring(0, count);

        }

        public static string ShiftLeftN(string s, int count)
        {

            string partition = s;

            for (int i = 0; i < count; i++)
            {
                partition = ShiftLeftone(partition, 1);


            }
            return partition;
        }

        public static List<string> GenerateAllKeys(this string BinaryKey)
        {

            List<string> result = new List<string>();
            int index = 0;
            string NewKey = "";
            int NumKeys = 0;
            while (NumKeys < 28)
            {
                NewKey = string.Concat(NewKey + BinaryKey[index]);
                index++;
                if (NewKey.Length == 4)
                {
                    result.Add(NewKey);
                    NewKey = "";
                    NumKeys++;
                }

                if (index == BinaryKey.Length)
                {
                    index = 0;
                    BinaryKey = ShiftLeftN(BinaryKey, 6);

                }


            }
            return result;
        }

        static string InverseAddition(this string num)
        {
            Dictionary<string, string> dec = new Dictionary<string, string>();
            dec["0000"] = "0000"; dec["0101"] = "1011"; dec["1010"] = "0110";
            dec["0001"] = "1111"; dec["0110"] = "1010"; dec["1011"] = "0101";
            dec["0010"] = "1110"; dec["0111"] = "1001"; dec["1100"] = "0100";
            dec["0011"] = "1101"; dec["1000"] = "1000"; dec["1101"] = "0011";
            dec["0100"] = "1100"; dec["1001"] = "0111"; dec["1110"] = "0010";
            dec["1111"] = "0001";

            return dec[num];

        }

        static string InverseMultiplication(this string num)
        {
            Dictionary<string, string> dec = new Dictionary<string, string>();

            dec["0000"] = "0000"; dec["0101"] = "0111"; dec["1010"] = "1100";
            dec["0001"] = "0001"; dec["0110"] = "0011"; dec["1011"] = "1110";
            dec["0010"] = "1001"; dec["0111"] = "0101"; dec["1100"] = "1010";
            dec["0011"] = "0110"; dec["1000"] = "1111"; dec["1101"] = "0100";
            dec["0100"] = "1101"; dec["1001"] = "0010"; dec["1110"] = "1011";
            dec["1111"] = "1000";
            return dec[num];
        }


        public static List<string> GenerateInverseKeys(this List<string> array_keys_28)
        {
            const int MATRIX_ROWS = 5;
            const int MATRIX_COLUMNS = 6;

            string[,] matrix = new string[MATRIX_ROWS, MATRIX_COLUMNS];
            int index = 0;
            for (int i = 0; i < MATRIX_ROWS; i++)
            {
                for (int j = 0; j < MATRIX_COLUMNS; j++)
                {
                    if (index == 28) break;
                    matrix[i, j] = array_keys_28[index++];


                }
            }

            int rows = 4, columns = 0;

            List<string> result = new List<string>();
            for (int i = 0; i < 28; i++)
            {
                if (i % 6 == 0)
                    result.Add(InverseMultiplication(matrix[rows, i % 6]));
                else if (i % 6 == 1)
                    result.Add(InverseAddition(matrix[rows, i % 6]));
                else if (i % 6 == 2)
                    result.Add(InverseAddition(matrix[rows, i % 6]));
                else if (i % 6 == 3)
                    result.Add(InverseMultiplication(matrix[rows, i % 6]));
                else
                    result.Add(matrix[rows, i % 6]);

                //Console.WriteLine(matrix[rows , i%6] + " "+  rows  + " "+ i%6);  

                if (rows == 4 && i == 3)
                {
                    rows--;
                    columns++;
                }
                else if (columns == 6)
                {
                    columns = 1;
                    rows--;
                }
                else if (rows != 4) columns++;



            }
            return result;
        }
    }
}
