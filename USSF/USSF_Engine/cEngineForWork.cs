using System;
using System.Collections.Generic;
using System.Threading;

namespace USSF.Engine
{
    internal class cEngineForWork
    {
        public string static_rnd_name = "ZERO";
        public class Text
        {
            private bool CheckText(string text)
            {
                if (text.Length < 0)
                    return false;
                else
                    return true;
            }
        }
        public class GetRandom
        {
            private readonly Random random = new Random(Environment.TickCount);
            #region ССЫЛКИ НА КЛАССЫ
            public static cEngineInfoCode pEngineInfoCode = new cEngineInfoCode();
            #endregion

            public int num_next = 0;
            public List<int> Positions(int size_original_code, int min_value_rnd_code = 4, int max_value_rnd_code = 13)
            {
                List<int> List_Positions = new List<int>();
                //0 - позиция оригинального кода
                //1 - позиция мусора

                for (int line = 0; line < size_original_code; line++)
                {
                    for (int value_random_code = 0; value_random_code < Int(min_value_rnd_code, max_value_rnd_code); value_random_code++)
                    {
                        List_Positions.Add(1);
                    }
                    List_Positions.Add(0);
                }

                return List_Positions;
            }
            public string Name(int min_max, int max_size, bool ussf_str = false)
            {
                string rndname = Abc(1, 1);
                for (int i = 0; i < Int(min_max, max_size); i++)
                {
                    rndname += pEngineInfoCode.vRandom_All[random.Next(0, pEngineInfoCode.vRandom_All.Length)];
                }
                if (ussf_str) rndname = "ussf_" + rndname;

                return rndname;
            }
            public int Int(int min, int max)
            {
                return random.Next(min, max);
            }
            public float Float()
            {
                return (float)(random.NextDouble() * 100);
            }
            public byte Byte()
            {
                byte[] massiv = new byte[1];
                random.NextBytes(massiv);

                return massiv[0];
            }
            public byte[] Bytes(int size)
            {
                byte[] massiv = new byte[size];
                random.NextBytes(massiv);

                return massiv;
            }
            public string All(int min_size, int max_size)
            {
                string rndname = Abc(1, 1);
                for (int i = 0; i < Int(min_size, max_size); i++)
                {
                    rndname += pEngineInfoCode.vRandom_All[random.Next(0, pEngineInfoCode.vRandom_All.Length)];
                }

                return rndname;
            }
            public string ABC(int min_size, int max_size)
            {
                string rndname = "";
                for (int i = 0; i < Int(min_size, max_size); i++)
                {
                    rndname += pEngineInfoCode.vRandom_ABC[random.Next(0, pEngineInfoCode.vRandom_ABC.Length)];
                }
                return rndname;
            }
            public string abc(int min_size, int max_size)
            {
                string rndname = "";
                for (int i = 0; i < Int(min_size, max_size); i++)
                {
                    rndname += pEngineInfoCode.vRandom_abc[random.Next(0, pEngineInfoCode.vRandom_abc.Length)];
                }
                return rndname;
            }
            public string Abc(int min_size, int max_size)
            {
                string rndname = "";
                for (int i = 0; i < Int(min_size, max_size); i++)
                {
                    rndname += pEngineInfoCode.vRandom_Abc[random.Next(0, pEngineInfoCode.vRandom_Abc.Length)];
                }
                return rndname;
            }
            public string Simvol(int min_size, int max_size)
            {
                string rndname = "";
                for (int i = 0; i < Int(min_size, max_size); i++)
                {
                    rndname += pEngineInfoCode.vRandom_Simvols[random.Next(0, pEngineInfoCode.vRandom_Simvols.Length)];
                }
                return rndname;
            }
        }

        static public string GetBlockCode(List<string> code_in_function)
        {
            string result = "start: none|none\nend: none|none";
            int start_code_a = -1;
            int start_code_b = -1;
            int end_code_a = -1;
            int end_code_b = -1;
            try
            {
                //Получаем данные о начале блока
                for (int i = 0; i < code_in_function.Count; i++) //Получаем строки
                {
                    if (code_in_function[i].Contains("{"))
                    {
                        for (int j = 0; j < code_in_function[i].Length; j++) //Получаем символы
                        {
                            if (code_in_function[i][j] == '{')
                            {
                                start_code_a = i;
                                start_code_b = j;
                                break;
                            }
                        }
                    }
                    if (start_code_a != -1) break;
                }
                Program.DEBUG_LOG($"Начало блока: {start_code_a}:{start_code_b} -> [{code_in_function[start_code_a]}]");

                //Получаем данные о количестве {
                string tmp2 = "";
                for (int i = start_code_a; i < code_in_function.Count; i++) //Получаем строки после нахождения первого '{'
                {
                    if (i == start_code_a)
                    {
                        for (int j = i == start_code_a ? start_code_b + 1 : 0; j < code_in_function[i].Length; j++) //Получаем символы
                        {
                            tmp2 += code_in_function[i][j].ToString();
                        }
                    }
                    else
                    {
                        tmp2 += code_in_function[i];
                    }
                }
                int counts_start_symbol = CountSymbols(tmp2, "{");
                Program.DEBUG_LOG($"Всего '{{' символов с [{start_code_a}:{start_code_b}]: {counts_start_symbol}");

                int counts_end_symbol = CountSymbols(tmp2, "}");
                Program.DEBUG_LOG($"Всего '}}' символов с [{start_code_a}:{start_code_b}]: {counts_end_symbol}");

                //Реализовать узнавалку пары '}' к '{'.

                return ""; //del
                int tmp1 = 0;
                for (int i = start_code_a; i < code_in_function.Count; i++) //Получаем строки после нахождения первого '{'
                {
                    if (code_in_function[i].Contains("}"))
                    {
                        for (int j = i == start_code_a ? start_code_b : 0; j < code_in_function[i].Length; j++) //Получаем символы
                        {
                            for (int z = 0; z < 5; z++) {//
                                if (z == 3)
                                {
                                    //
                                }
                            }

                            //end_code_a = i;
                            //end_code_b = j;
                            //break;
                        }
                    }
                    if (end_code_a != -1) break;
                }
                Program.DEBUG_LOG($"Конец блока: {end_code_a}:{end_code_b} -> [{code_in_function[end_code_a]}]");
            }
            catch (Exception er)
            {
                Program.DEBUG_LOG(er.ToString());
            }
            return result;
        }
        static private int CountSymbols(string text, string symbol)
        {
            int i = 0, count = 0;
            while ((i = text.IndexOf(symbol, i)) != -1) 
            { 
                ++count; 
                i += symbol.Length; 
            }
            return count;
        }
    }
}
