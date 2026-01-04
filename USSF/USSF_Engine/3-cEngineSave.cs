using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace USSF.Engine
{
    internal class cEngineSave
    {
        public string patch_ussf = @"USSF_result\";

        public bool SaveFile(List<string> result_code_ussf_one_file, string patch_file)
        {
            if (result_code_ussf_one_file.Count > 0)
            {
                FileInfo new_file = new FileInfo(patch_file);

                //получаем все файлы в категории результатов ussf
                foreach (string tmp_file in Directory.GetFiles(patch_ussf, "*.*", SearchOption.AllDirectories))
                {
                    FileInfo file = new FileInfo(tmp_file);
                    string temp_patch_file = file.FullName.ToString();
                    //если нашли наш файл, то
                    Program.LOG(
                        $"temp_patch_file: {temp_patch_file}|{BitConverter.ToInt32(MD5.Create().ComputeHash(File.ReadAllBytes(temp_patch_file)), 10)} " +
                        $"=? " +
                        $"patch_file: {new_file.FullName}|{BitConverter.ToInt32(MD5.Create().ComputeHash(File.ReadAllBytes(new_file.FullName)), 10)}");
                    if (BitConverter.ToInt32(MD5.Create().ComputeHash(File.ReadAllBytes(temp_patch_file)), 10)
                        ==
                        BitConverter.ToInt32(MD5.Create().ComputeHash(File.ReadAllBytes(new_file.FullName)), 10))
                    {
                        //Program.LOG($"Удаление файла: '{temp_patch_file}'");
                        File.Delete(temp_patch_file);
                        //Program.LOG($"Создание файла: '{temp_patch_file}'");

                        File.WriteAllLines(temp_patch_file, result_code_ussf_one_file, Encoding.GetEncoding(1251));
                    }
                }

                return true;
            }
            else return false;
        }

        public void CopyFilesProject()
        {
            Program.LOG("Начинаю копирование файлов проекта..");
            Program.LOG($"Патч: {Program.pPath_Get.patch}, патч ussf: {patch_ussf}");
            if (Directory.Exists(patch_ussf)) Directory.Delete(patch_ussf, true);
            patch_ussf += $@"\{Program.pPath_Get.patch.Name}\";
            Directory.CreateDirectory(patch_ussf);

            Copy(Program.pPath_Get.patch.FullName, patch_ussf);
        }

        private void Copy(string sourceDirectory, string targetDirectory)
        {
            CopyAll(new DirectoryInfo(sourceDirectory), new DirectoryInfo(targetDirectory));
        }

        private void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            //Копируем все поддиректории
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                //Создаем новую поддиректорию в директории
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);

                CopyAll(diSourceSubDir, nextTargetSubDir);
            }

            //Копируем все файлы в новую директорию
            foreach (FileInfo file in source.GetFiles())
            {
                //if (CheckFilesInPatch(file))
                file.CopyTo(Path.Combine(target.ToString(), file.Name), true);
            }
        }
    }
}
