using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace USSF.Engine
{
    internal class cEngineSearch
    {
        public List<string> file_code = new List<string>();
        public List<string> list_info_classes = new List<string>();
        //public List<string> list_info_methods = new List<string>();
        public List<string> num_in_code_classes = new List<string>();
        //public List<string> num_in_code_method = new List<string>();
        public List<cEngineInfoCode.InfoMethod> num_in_code_method = new List<cEngineInfoCode.InfoMethod>();
        public List<cEngineInfoCode.InfoMethod> list_info_methods = new List<cEngineInfoCode.InfoMethod>();
        public void Search_Start(int num_file)
        {
            try
            {
                Program.LOG($"[{num_file}]: {Program.pPath_Get.actual_files[num_file]}");

                file_code = new List<string>(File.ReadAllLines(Program.pPath_Get.actual_files[num_file], Encoding.GetEncoding(1251)));
                for (int i = 0; i < file_code.Count; i++)
                {
                    file_code[i] = Program.pTransformation.ChangeSleep(file_code[i]);
                    string tmp = file_code[i];
                    if (tmp.Contains("[USSF_RAND_STR_NOSTATIC]")) file_code[i] = tmp.Replace("[USSF_RAND_STR_NOSTATIC]", Program.GetRandom.All(5, 10));
                    else if (tmp.Contains("USSF_RAND_STR_STATIC")) file_code[i] = tmp.Replace("USSF_RAND_STR_STATIC", Program.pForWork.static_rnd_name);
                }

                if (file_code.Count > 0)
                {
                    Methods(Classes());
                    OutPut(); //debug func
                    Update();
                    Program.LOG($"[Search_Start] {list_info_classes.Count}");
                    Program.LOG($"[Search_Start] {list_info_methods.Count}");

                    Program.LOG($"list_info_methods");
                    for (int i = 0; i < list_info_methods.Count; i++)
                    {
                        Program.DEBUG_LOG($"[{i}] {list_info_methods[i]}");
                    }
                }
            }
            catch (Exception er)
            {
                Program.LOG($"Ошибка: {er}");
            }
        }
        private void Update()
        {
            /*
            if (num_in_code_classes.count > 0)
            {
                //list_info_classes = num_in_code_classes;
                for (int element = 0; element < num_in_code_classes.count; element++)
                {
                    list_info_classes.add(num_in_code_classes[element]);
                }
                num_in_code_classes.clear();
            }
            */
            if (num_in_code_method.Count > 0)
            {
                //list_info_methods = num_in_code_method;
                for (int element = 0; element < num_in_code_method.Count; element++)
                {
                    list_info_methods.Add(num_in_code_method[element]);
                }
                num_in_code_method.Clear();
            }
        }
        private void OutPut()
        {
            Program.LOG("\tnum_in_code_method.Count: " + num_in_code_method.Count);
            /*
            if (num_in_code_classes.Count > 0)
            {
                for (int object_class = 0; object_class < num_in_code_classes.Count; object_class++)
                {
                    Program.LOG("CLASS-> " + num_in_code_classes[object_class]);

                    string name_class = new Regex("name=(.*),start").Match(num_in_code_classes[object_class]).Groups[1].Value;
                    for (int object_method = 0; object_method < num_in_code_method.Count; object_method++)
                    {
                        class_method = new Regex("class=(.*),name").Match(num_in_code_method[object_method]).Groups[1].Value;
                        if (class_method == name_class) //метод из класса
                            Program.LOG("\tMETHOD-> " + num_in_code_method[object_method]);
                    }
                }
            }
            */
            if (num_in_code_method.Count > 0)
            {
                for (int object_method = 0; object_method < num_in_code_method.Count; object_method++)
                {
                    Program.LOG("METHOD-> " + num_in_code_method[object_method]);
                }
            }
        }
        private void Methods(bool clasess)
        {
            /*
            if (clasess == true)
            {
                for (int num_object = 0; num_object < num_in_code_classes.Count; num_object++)
                {
                    int num_start = Convert.ToInt32(new Regex("start=(.*),").Match(num_in_code_classes[num_object]).Groups[1].Value);
                    int num_end = Convert.ToInt32(new Regex("end=(.*),").Match(num_in_code_classes[num_object]).Groups[1].Value);

                    for (int num_lines_code = num_start; num_lines_code <= num_end; num_lines_code++) //num_start < num_end
                    {
                        if (num_lines_code > num_end) break;

                        if (file_code[num_lines_code].Contains(Program.CPlusPlus.public_information_for_search_methods[0]))
                        {
                            string num_method = new Regex($"{Program.CPlusPlus.public_information_for_search_methods[0]}(.*);").Match(file_code[num_lines_code]).Groups[1].Value;

                            for (int num_end_lines_method = 0; num_end_lines_method < file_code.Count; num_end_lines_method++)
                            {
                                if (num_end_lines_method > file_code.Count) break;

                                if (file_code[num_end_lines_method].Contains(Program.CPlusPlus.public_information_for_search_methods[1]) &&
                                    new Regex($"{Program.CPlusPlus.public_information_for_search_methods[1]}(.*);").Match(file_code[num_end_lines_method]).Groups[1].Value.ToString() == num_method) //ussf_end_class=
                                {
                                    if (file_code[num_lines_code + 1].Contains("{"))
                                    {
                                        num_in_code_method.Add(
                                        $"name={num_method}:" +
                                        $"class={new Regex($"name=(.*),start").Match(num_in_code_classes[num_object]).Groups[1].Value}," +
                                        $"name=method," +
                                        $"start={num_lines_code + 2}," + //+1
                                        $"end={num_end_lines_method - 2};"); //-0
                                        num_lines_code = num_end_lines_method;
                                        break;
                                    }
                                    else if (file_code[num_lines_code + 2].Contains("{"))
                                    {
                                        num_in_code_method.Add(
                                        $"name={num_method}:" +
                                        $"class={new Regex($"name=(.*),start").Match(num_in_code_classes[num_object]).Groups[1].Value}," +
                                        $"name=method," +
                                        $"start={num_lines_code + 3}," + //+1
                                        $"end={num_end_lines_method - 2};"); //-0
                                        num_lines_code = num_end_lines_method;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            */

            Program.DEBUG_LOG("START - METHODS");
            //List<string> num_in_code_method_buffer = new List<string>();
            List<cEngineInfoCode.InfoMethod> num_in_code_method_buffer = new List<cEngineInfoCode.InfoMethod>();

            for (int num_lines_code = 0; num_lines_code < file_code.Count; num_lines_code++)
            {
                if (num_lines_code > file_code.Count) break;

                if (file_code[num_lines_code].Contains(Program.pInfoCode_CPlusPlus.public_information_for_search_methods[0]))
                {
                    string name_method = new Regex($"{Program.pInfoCode_CPlusPlus.public_information_for_search_methods[0]}(.*):").Match(file_code[num_lines_code]).Groups[1].Value; //ussf_start_method=
                    Program.DEBUG_LOG("name_method: " + name_method);
                    for (int num_end_lines_method = 0; num_end_lines_method < file_code.Count; num_end_lines_method++)
                    {
                        if (num_end_lines_method > file_code.Count) break;

                        if (file_code[num_end_lines_method].Contains(Program.pInfoCode_CPlusPlus.public_information_for_search_methods[1]) &&
                            new Regex($"{Program.pInfoCode_CPlusPlus.public_information_for_search_methods[1]}(.*);").Match(file_code[num_end_lines_method]).Groups[1].Value.ToString() == name_method) //ussf_end_method=
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            byte start_offset = 0;
                            
                            /*
                            void Method() {
                                ...
                            }
                            */
                            Program.DEBUG_LOG("file_code[num_lines_code + 1]: " + file_code[num_lines_code + 1]);
                            if (file_code[num_lines_code + 1].Contains("{")) start_offset = 1;

                            /*
                            void Method() 
                            {
                                ...
                            }
                            */
                            else if (file_code[num_lines_code + 2].Contains("{")) start_offset = 2;
                            else { Program.DEBUG_LOG("ERROR, WTF!?"); return; }
                            Program.DEBUG_LOG("file_code[num_lines_code + 2]: " + file_code[num_lines_code + 2]);

                            /*
                            int line_start_a = num_lines_code + start_offset;
                            int line_start_b = -1;
                            Program.DEBUG_LOG("file_code[line_start_a]: " + file_code[line_start_a]);
                            for (int j = 0; j < file_code[line_start_a].Length; j++)
                            {
                                Program.DEBUG_LOG($"file_code[{line_start_a}][{j}]: " + file_code[line_start_a][j]);
                                if (file_code[line_start_a][j].ToString().Contains("{"))
                                {
                                    line_start_b = j;
                                    break;
                                }
                            }
                            Program.DEBUG_LOG($"start {{ тут: {line_start_a+1}:{line_start_b+1}");

                            Program.DEBUG_LOG("\nНачало инфы с блоков:\n");
                            List<string> test_list = new List<string>();
                            Program.DEBUG_LOG($"line_start_a: {line_start_a}");
                            Program.DEBUG_LOG($"num_end_lines_method: {num_end_lines_method}");
                            for (int a = line_start_a; a < num_end_lines_method; a++)
                            {
                                Program.DEBUG_LOG("АЛО");
                                Program.DEBUG_LOG($"a: {a}");
                                Program.DEBUG_LOG($"num_end_lines_method: {num_end_lines_method}");
                                Program.DEBUG_LOG($"file_code[{a}]: {file_code[a]}");
                                test_list.Add(file_code[a]);
                            }
                            Program.DEBUG_LOG("\nКонец инфы с блоков:\n");
                            cEngineForWork.GetBlockCode(test_list);
                            */

                            /*
                            num_in_code_method_buffer.Add(
                                $"name={name_method}:" +
                                $"type=method," +
                                $"start={num_lines_code + start_offset + 1}," + //+1
                                $"end={num_end_lines_method - 2}," + //-0
                                $"prms={new Regex($"={name_method}:(.*);").Match(file_code[num_lines_code]).Groups[1].Value};");
                            */
                            var tmp_info_method = new cEngineInfoCode.InfoMethod
                            {
                                Name = name_method,
                                Type = "method",
                                Start = num_lines_code + start_offset + 1,
                                End = num_end_lines_method - 2,
                                Parameters = new Regex($"={name_method}:(.*);").Match(file_code[num_lines_code]).Groups[1].Value.ToString()
                            };
                            Program.DEBUG_LOG($"tmp_info_method.Name:       {tmp_info_method.Name}");
                            Program.DEBUG_LOG($"tmp_info_method.Type:       {tmp_info_method.Type}");
                            Program.DEBUG_LOG($"tmp_info_method.Start:      {tmp_info_method.Start}");
                            Program.DEBUG_LOG($"tmp_info_method.End:        {tmp_info_method.End}");
                            Program.DEBUG_LOG($"tmp_info_method.Parameters: {tmp_info_method.Parameters}");
                            num_in_code_method_buffer.Add(tmp_info_method);

                            num_lines_code = num_end_lines_method;
                            Console.ForegroundColor = ConsoleColor.Blue;
                            break;
                        }
                    }
                }
            }

            for (int num_object_in_buffer = 0; num_object_in_buffer < num_in_code_method_buffer.Count; num_object_in_buffer++)
            {
                if (num_in_code_method.Count > 0)
                {
                    for (int num_object = 0; num_object < num_in_code_method.Count; num_object++)
                    {
                        //  if (new Regex("name=(.*):class").Match(num_in_code_method_buffer[num_object_in_buffer]).Groups[1].Value ==
                        //      new Regex("name=(.*):class").Match(num_in_code_method[num_object]).Groups[1].Value)
                        if (num_in_code_method_buffer[num_object_in_buffer].Name == num_in_code_method[num_object].Name)
                        {
                            num_in_code_method_buffer.RemoveAt(num_object_in_buffer);
                            break;
                        }
                        else if (num_object == num_in_code_method.Count - 1)
                        {
                            if (num_in_code_method_buffer[num_object_in_buffer].Name.Length > 0)
                                num_in_code_method.Add(num_in_code_method_buffer[num_object_in_buffer]);
                            break;
                        }
                    }
                }
                else num_in_code_method.Add(num_in_code_method_buffer[num_object_in_buffer]);
            }
            num_in_code_method_buffer.Clear();
        }
        private bool Classes()
        {
            /*
            for (int num_lines_code = 0; num_lines_code < file_code.Count; num_lines_code++)
            {
                if (num_lines_code > file_code.Count) break;

                if (file_code[num_lines_code].Contains(Program.CPlusPlus.public_information_for_search_class[0]))
                {
                    string num_class = new Regex($"{Program.CPlusPlus.public_information_for_search_class[0]}(.*);").Match(file_code[num_lines_code]).Groups[1].Value;

                    for (int num_end_lines_class = 0; num_end_lines_class < file_code.Count; num_end_lines_class++)
                    {
                        if (num_end_lines_class > file_code.Count) break;

                        if (file_code[num_end_lines_class].Contains(Program.CPlusPlus.public_information_for_search_class[1]) &&
                            new Regex($"{Program.CPlusPlus.public_information_for_search_class[1]}(.*);").Match(file_code[num_end_lines_class]).Groups[1].Value.ToString() == num_class) //ussf_end_class=
                        {
                            if (file_code[num_lines_code + 1].Contains("class ") && file_code[num_lines_code + 1].Contains("{"))
                            {
                                num_in_code_classes.Add(
                                $"name={num_class}:" +
                                $"name={new Regex("class (.*)").Match(file_code[num_lines_code + 1]).Groups[1].Value.ToString()}," +
                                $"start={num_lines_code + 1}," +
                                $"end={num_end_lines_class};"); //найдем номер класса, имя, его начало и конец
                                num_lines_code = num_end_lines_class;
                                break;
                            }
                            else if (file_code[num_lines_code + 1].Contains("class ") && file_code[num_lines_code + 2].Contains("{"))
                            {
                                num_in_code_classes.Add(
                                $"name={num_class}:" +
                                $"name={new Regex("class (.*)").Match(file_code[num_lines_code + 1]).Groups[1].Value.ToString()}," +
                                $"start={num_lines_code + 2}," +
                                $"end={num_end_lines_class};"); //найдем номер класса, имя, его начало и конец
                                num_lines_code = num_end_lines_class;
                                break;
                            }
                        }
                    }
                }
            }

            if (num_in_code_classes.Count > 0)
                Methods(true);
            else
                Methods(false);
            */

            return false;
        }
    }
}
