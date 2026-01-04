using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace USSF.Engine
{
    internal class cEngineTransformation
    {
        public List<string> result_code_ussf_one_file = new List<string>();
        public void Transformation_Start()
        {
            try
            {
                //if (Program.pEngineSearch.list_info_classes.Count > 0) Classes();
                if (Program.pSearch.list_info_methods.Count > 0) Methods();

                if (Program.pSave.SaveFile(result_code_ussf_one_file, Program.pPath_Get.patch_one_file))
                {
                    result_code_ussf_one_file.Clear();
                    object_method_2 = 0;
                }
                else Program.LOG("### ОШИБКА НА УРОВНЕ Transformation_Start ПРИ ПРОВЕРКЕ МЕТОДА СОХРАНЕНИЯ ФАЙЛА!");
            }
            catch (Exception er)
            {
                Program.LOG("[Transformation_Start] Ошибка: \n" + er.ToString());
            }
        }

        private int object_method_2 = 0;
        private void Methods()
        {
            List<string> list_original_file = new List<string>();

            //Program.LOG(Program.pEngineSearch.list_info_methods.Count.ToString());
            //Program.LOG(Program.pEngineSearch.file_code.Count.ToString());

            List<int> list_map_random;
            List<string> list_test_result;
            List<object> list_code_in_new_file = new List<object>();

            //объекты[номер_объекта] = информация(name=method_num_1:name=method,start=18,end=22 (пример))
            //объект из файла (метод\класс)
            for (int object_method = 0; object_method < Program.pSearch.list_info_methods.Count; object_method++)
            {
                //получаем начало и конец кода метода
                Program.DEBUG_LOG("получаем начало и конец кода метода");
                int num_start = Program.pSearch.list_info_methods[object_method].Start;
                int num_end = Program.pSearch.list_info_methods[object_method].End;

                //получаем параметры
                Program.DEBUG_LOG("получаем параметры");
                string prms = Program.pSearch.list_info_methods[object_method].Parameters;
                Program.DEBUG_LOG($"prms: {prms}");

                //заполняем массив оригинальным кодом
                for (int num_lines_code = num_start; num_lines_code <= num_end; num_lines_code++)
                {
                    list_original_file.Add(Program.pSearch.file_code[num_lines_code].ToString());
                }

                //заполняем массив картой мусорного кода
                list_map_random = Program.GetRandom.Positions(list_original_file.Count, 0, 5); //(0 - оригинал.код, 1 - мусор.код)
                
                Program.CWL("-> Карта мусорного кода: ");
                for (int i = 0; i < list_map_random.Count; i++)
                {
                    if (list_map_random[i] == 0) Console.ForegroundColor = ConsoleColor.Green;
                    else if (list_map_random[i] == 1) Console.ForegroundColor = ConsoleColor.Red;

                    Console.Write($"{list_map_random[i]} ");
                }
                Console.Write("\n");
                
                Console.ForegroundColor = ConsoleColor.Blue;

                //вывод оригинального кода
                //for (int test = 0; test < list_original_file.Count; test++) Program.CWL(list_original_file[test].ToString());

                //создание массива для показа результата
                list_test_result = new List<string>();

                //создание нового кода с мусором
                Program.DEBUG_LOG("создание нового кода с мусором");
                int or_code = 0;
                for (int i = 0; i < list_map_random.Count; i++)
                {
                    if (list_map_random[i] == 0) //оригинал код
                    {
                        list_test_result.Add(list_original_file[or_code].ToString());
                        or_code++;
                    }
                    else if (list_map_random[i] == 1) //мусорный код
                    {
                        list_test_result.Add(
                            "\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t" + 
                            Program.pTransform_Add.Function(prms) + 
                            " //USSF");
                    }
                }

                //Program.CWL("===========================================");

                list_original_file.Clear();
                list_map_random.Clear();
                //в массиве объектов - объект мусорного кода
                list_code_in_new_file.Add(list_test_result);
            }

            Console.ForegroundColor = ConsoleColor.Yellow;

            int num_start2 = 0;
            int num_end2 = 0;

            for (int line = 0; line < Program.pSearch.file_code.Count; line++)
            {
                //Thread.Sleep(1);
                if (object_method_2 < Program.pSearch.list_info_methods.Count)
                {
                    //получаем начало и конец кода метода
                    //num_start2 = Convert.ToInt32(new Regex("start=(.*),end").Match(Program.pSearch.list_info_methods[object_method_2]).Groups[1].Value);
                    //num_end2 = Convert.ToInt32(new Regex("end=(.*),").Match(Program.pSearch.list_info_methods[object_method_2]).Groups[1].Value);
                    num_start2 = Program.pSearch.list_info_methods[object_method_2].Start;
                    num_end2 = Program.pSearch.list_info_methods[object_method_2].End;
                }
                if (line == num_start2)
                {
                    // Program.LOG("--------------------TRUE");
                    if (list_code_in_new_file.Count >= object_method_2)//>=
                    {
                        List<string> object_list_string = (List<string>)list_code_in_new_file[object_method_2];
                        for (int a = 0; a < object_list_string.Count; a++)
                        {
                            result_code_ussf_one_file.Add(object_list_string[a]);
                        }
                        line += (num_end2 - num_start2);
                        if (object_method_2 < Program.pSearch.list_info_methods.Count &&
                            object_method_2 < list_code_in_new_file.Count)//<
                        {
                            object_method_2++;
                        }
                    }
                }
                else result_code_ussf_one_file.Add(Program.pSearch.file_code[line]);
            }

            list_code_in_new_file.Clear();
        }

        public string ChangeSleep(string line)
        {
            MatchCollection matches = new Regex(@"Sleep\((.*)\);").Matches(line);
            if (matches.Count <= 0) return line;

            foreach (Match match in matches)
            {
                float tmp = float.Parse(match.Value.Replace("Sleep(", string.Empty).Replace(");", string.Empty));

                int min = 0, max = 0;
                float one_procent = tmp / 100;
                if (tmp <= 1000 && tmp >= 100)
                {
                    min = (int)(tmp - (one_procent * 30));
                    max = (int)(tmp + (one_procent * 30));
                }
                else if (tmp < 100 && tmp >= 10)
                {
                    min = (int)(tmp - (one_procent * 40));
                    max = (int)(tmp + (one_procent * 40));
                }
                else if (tmp < 10)
                {
                    min = (int)tmp;
                    max = (int)(tmp + tmp * 2);
                }
                else if (tmp > 1000)
                {
                    min = (int)(tmp - (one_procent * 10));
                    max = (int)(tmp + (one_procent * 10));
                }

                line = line.Replace(match.Value, $"Sleep({Program.GetRandom.Int(min, max)});");
            }

            return line + " /*USSF_CHANGE_SLEEP*/";
        }

        public class Add
        {
            public string Method_Jump(bool paramters = false)
            {
                string ParametrsMethod(int max)
                {
                    string massiv_parametrs = "";
                    for (int i = 0; i < max; i++)
                    {
                        /*
                        if (i == max - 1)
                            massiv_parametrs = massiv_parametrs + $"{Program.CPlusPlus.parametrs_for_method_methods[Program.GetRandom.Int(0, Program.CPlusPlus.parametrs_for_method_methods.Length)]} " +
                                $"{Program.GetRandom.Name(4, 8)}";
                        else
                            massiv_parametrs = massiv_parametrs + $"{Program.CPlusPlus.parametrs_for_method_methods[Program.GetRandom.Int(0, Program.CPlusPlus.parametrs_for_method_methods.Length)]} " +
                                $"{Program.GetRandom.Name(4, 8)}, ";
                        */
                        string tmp = Program.GetRandom.Name(4, 8);
                        if (i != max - 1) tmp += ", ";

                        massiv_parametrs += $"{Program.pInfoCode_CPlusPlus.parametrs_for_method_methods[Program.GetRandom.Int(0, Program.pInfoCode_CPlusPlus.parametrs_for_method_methods.Length)]} {tmp}";
                    }

                    return massiv_parametrs;
                }
                try
                {
                    if (paramters)
                        return $"{Program.pInfoCode_CPlusPlus.type_for_method_methods[Program.GetRandom.Int(0, Program.pInfoCode_CPlusPlus.type_for_method_methods.Length)]} " +
                            $"{Program.GetRandom.Name(15, 30)}" +
                            $"({ParametrsMethod(Program.GetRandom.Int(3, 8))});";
                    else
                        return $"{Program.pInfoCode_CPlusPlus.type_for_method_methods[Program.GetRandom.Int(0, Program.pInfoCode_CPlusPlus.type_for_method_methods.Length)]} " +
                            $"{Program.GetRandom.Name(15, 30)}" +
                            $"();";
                }
                catch (Exception er)
                {
                    return ">ERROR: " + er.ToString();
                }
            } //НЕДОДЕЛАННОЕ
            public string Function(string prms)
            {
                string type = string.Empty;
                try
                {
                    if (Program.pInfoCode.type_project == "exe" || Program.pInfoCode.type_project == "dll")
                    {
                        GET_TYPE: type = Program.pInfoCode_CPlusPlus.type_for_function_default[Program.GetRandom.Int(0, Program.pInfoCode_CPlusPlus.type_for_function_default.Length)].ToString();
                        if (prms != "all" && !string.IsNullOrEmpty(prms) && prms.Contains("<") && !prms.Contains($"<{type}>"))
                        {
                            Program.DEBUG_LOG("goto GET_TYPE");
                            goto GET_TYPE;
                        }

                        if (type == "strstr") return $"strstr(\"{Program.GetRandom.All(1, 2)}\", \"{Program.GetRandom.All(1, 4)}\");";
                        else if (type == "rand") return $"srand(time(0)); rand() % {Program.GetRandom.Int(10, 3000)};";
                        else if (type == "rint") return $"rint({Program.GetRandom.Float().ToString().Replace(",", ".")});";
                        else if (type == "mersenne")
                        {
                            Program.pInfoCode.for_type_mersenne++;
                            return $"random_device rd_{Program.pInfoCode.for_type_mersenne}; mt19937 mersenne_{Program.pInfoCode.for_type_mersenne}(rd_{Program.pInfoCode.for_type_mersenne}());";
                        }
                    }
                    else if (Program.pInfoCode.type_project == "driver")
                    {
                        GET_TYPE: type = Program.pInfoCode_CPlusPlus.type_for_function_driver[Program.GetRandom.Int(0, Program.pInfoCode_CPlusPlus.type_for_function_driver.Length)].ToString();
                        if (prms != "all" && !string.IsNullOrEmpty(prms) && prms.Contains("<") && !prms.Contains($"<{type}>"))
                        {
                            Program.DEBUG_LOG("goto GET_TYPE");
                            goto GET_TYPE;
                        }
                        int type_num = Program.GetRandom.Int(0, 2);

                        if (type == "strstr") return $"strstr(\"{Program.GetRandom.All(1, 2)}\", \"{Program.GetRandom.All(1, 4)}\");";
                        else if (type == "DbgPrint")
                        {
                            if (type_num == 0) return $"DbgPrint(\"{Program.GetRandom.All(0, 5)} %s\", \"{Program.GetRandom.All(1, 4)}\");";
                            else if (type_num == 1) return $"DbgPrint(\"{Program.GetRandom.All(0, 5)} %i\", {Program.GetRandom.Int(10, 3000)});";
                            else if (type_num == 2) return $"DbgPrint(\"{Program.GetRandom.All(0, 5)} %f\", \"{Program.GetRandom.Float().ToString().Replace(",", ".")}\");";
                        }
                    }
                }
                catch (Exception er) { return $"> ERROR: {er}"; }
                return "//USSF_ERROR_GET_METHOD";
            }
        }
    }
}
