using System;
using System.IO;
using System.Text;
using System.Threading;
using USSF.Engine;

namespace USSF
{
    internal class Program
    {
        public static int num_file = 0;
        public static bool animate = true;
        #region ССЫЛКИ НА КРАТКИЕ МЕТОДЫ
        public static void CW(string text) { if (animate) Console.Write("  " + text); }
        public static void CWL(string text) { if (animate) Console.WriteLine("  " + text); }
        public static void LOG(string text) { if (animate) Console.WriteLine("  " + $"-> {text}"); }
        public static void DEBUG_LOG(string text) { if (true) Console.WriteLine("  " + $"[MY_LOG] $> {text}"); }
        public static void PAUSE() => Console.ReadKey();
        #endregion

        #region ССЫЛКИ НА КЛАССЫ
        public static cEngineScanPatch pScanPatch = new cEngineScanPatch();
        public static cEngineScanPatch.Get pPath_Get = new cEngineScanPatch.Get();
        public static cEngineSearch pSearch = new cEngineSearch();
        public static cEngineTransformation pTransformation = new cEngineTransformation();
        public static cEngineTransformation.Add pTransform_Add = new cEngineTransformation.Add();
        public static cEngineSave pSave = new cEngineSave();
        public static cEngineForWork pForWork = new cEngineForWork();
        public static cEngineForWork.GetRandom GetRandom = new cEngineForWork.GetRandom();
        public static cEngineForWork.Text pForWork_Text = new cEngineForWork.Text();
        public static cEngineInfoCode pInfoCode = new cEngineInfoCode();
        public static cEngineInfoCode.CPlusPlus pInfoCode_CPlusPlus = new cEngineInfoCode.CPlusPlus();
        #endregion

        private static void Main(string[] args)
        {
            args = new string[3] { @"E:\Personal\Coding Projects\ПРОЕКТЫ\WARFACE\WALIMEM\DriverInjectDll-master", "cplusplus", "driver" };
            Console.InputEncoding = Encoding.GetEncoding(1251);
            Console.OutputEncoding = Encoding.GetEncoding(1251);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Title = "USSF";
            Console.SetWindowSize(94, 28);
            Console.SetBufferSize(94, 300);

            try
            {
                if (animate)
                {
                    Console.WriteLine("");
                    CWL("===========================================================================================");
                    CWL(":::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::\n" +
                        "  :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::\n" +
                        "  ::::::::::::::::::::::::::'##::::'##::'######:::'######::'########:::::::::::::::::::::::::\n" +
                        "  :::::::::::::::::::::::::: ##:::: ##:'##... ##:'##... ##: ##.....::::::::::::::::::::::::::\n" +
                        "  :::::::::::::::::::::::::: ##:::: ##: ##:::..:: ##:::..:: ##:::::::::::::::::::::::::::::::\n" +
                        "  :::::::::::::::::::::::::: ##:::: ##:. ######::. ######:: ######:::::::::::::::::::::::::::\n" +
                        "  :::::::::::::::::::::::::: ##:::: ##::..... ##::..... ##: ##...::::::::::::::::::::::::::::\n" +
                        "  :::::::::::::::::::::::::: ##:::: ##:'##::: ##:'##::: ##: ##:::::::::::::::::::::::::::::::\n" +
                        "  ::::::::::::::::::::::::::. #######::. ######::. ######:: ##:::::::::::::::::::::::::::::::\n" +
                        "  ::::::::::::::::::::::::::::.......::::......::::......:::..:::::::::::::::::::::::::::::::\n" +
                        "  :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::\n" +
                        "  :::::::::::::::::::::::::::::::::::::::::v1.0.0.0::::::::::::::::::::::::::::::::::::::::::\n" +
                        "  :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::");
                    CWL("===========================================================================================");
                }
                if (args.Length < 1) Environment.Exit(0);
                else if (args.Length == 2)
                {
                    CW("Введите параметр [тип проекта]: ");
                    args = new string[3] { args[0], args[1], Console.ReadLine() };
                }

                if (args[2] != "exe" && args[2] != "dll" && args[2] != "driver")
                {
                    Environment.Exit(0);
                }

                pInfoCode.lang_project = args[1];
                pInfoCode.type_project = args[2];
                pPath_Get.patch = new DirectoryInfo(args[0].ToString());
                pPath_Get.Files();
                CWL("===========================================================================================");
                pForWork.static_rnd_name = GetRandom.Name(5, 15);
                pSave.CopyFilesProject();

                string file_okay_process = $@"{pSave.patch_ussf}\USSF_OKAY_BUILD.ussf";
                if (File.Exists(file_okay_process)) File.Delete(file_okay_process);

                for (num_file = 0; num_file < pPath_Get.actual_files.Count; num_file++)
                {
                    pPath_Get.patch_one_file = pPath_Get.actual_files[num_file];

                    if (pPath_Get.actual_files[num_file].Length > 0)
                    {
                        CWL("===========================================================================================");
                        pSearch.Search_Start(num_file);
                        CWL("===========================================================================================");
                        if (pSearch.list_info_classes.Count > 0 || pSearch.list_info_methods.Count > 0) pTransformation.Transformation_Start();
                    }

                    pSearch.file_code.Clear();
                    pSearch.list_info_classes.Clear();
                    pSearch.list_info_methods.Clear();
                    pSearch.num_in_code_classes.Clear();
                    pSearch.num_in_code_method.Clear();
                    pTransformation.result_code_ussf_one_file.Clear();
                    pInfoCode.for_type_mersenne = 0;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
                CWL("===========================================================================================");
                CWL("\t\t\t################### ГОТОВО ####################");
                CWL("===========================================================================================");

                LOG(file_okay_process);
                File.WriteAllText(file_okay_process, pForWork.static_rnd_name);
                PAUSE();
            }
            catch (Exception error)
            {
                CWL($"Ошибка: {error}");
                PAUSE();
            }
        }
    }
}
