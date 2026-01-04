using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace USSF.Engine
{
    public class cEngineScanPatch
    {
        public class Get
        {
            public DirectoryInfo patch;
            public string patch_one_file = "";
            public List<string> actual_files = new List<string>();
            public List<int> actual_files_hash = new List<int>();
            public bool Files()
            {
                Program.LOG($"ПАТЧ: {patch}");
                string[] all_files = Directory.GetFiles(patch.FullName, "*.*", SearchOption.AllDirectories);

                for (int a = 0; a < all_files.Length; a++)
                {
                    FileInfo fileInfo = new FileInfo(all_files[a]);
                    for (int b = 0; b < Program.pInfoCode_CPlusPlus.extension.Length; b++)
                    {
                        if (fileInfo.Extension == Program.pInfoCode_CPlusPlus.extension[b])
                        {
                            actual_files.Add(fileInfo.FullName);
                            actual_files_hash.Add(BitConverter.ToInt32(MD5.Create().ComputeHash(File.ReadAllBytes(fileInfo.FullName)), 10));
                        }
                    }
                }
                if (Program.animate)
                for (int element = 0; element < actual_files.Count; element++)
                {
                    if (actual_files[element].Length > 0 && actual_files_hash[element] != 0)
                    {
                        Program.LOG($"{actual_files[element]} | {Convert.ToString(actual_files_hash[element], 16).ToUpper()}");
                    }
                }

                if (actual_files.Count > 0) return true;
                return false;
            }
        }
    }
}
