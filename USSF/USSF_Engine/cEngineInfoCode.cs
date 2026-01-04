namespace USSF.Engine
{
    public class cEngineInfoCode
    {
        public string lang_project = "none"; //cplusplus, csharp
        public string type_project = "none"; //exe, dll, driver
        public int for_type_mersenne = 0;
        public class CPlusPlus
        {
            public string[] public_information_for_search_class =
            {
                "//ussf_start_class=", //ussf_start_class=TestMethod:all | ussf_start_class=TestMethod:<strstr>,<rint>
                "//ussf_end_class="
            };
            public string[] public_information_for_search_methods =
            {
                "//ussf_start_method=",
                "//ussf_end_method="
            };
            public string[] type_for_method_methods =
            {
                "void" ,
                "int" ,
                "int*" ,
                "float" ,
                "float*" ,
                "string" ,
                "string*" ,
                "const char" ,
                "const char*" ,
                "char" ,
                "char*" ,
                "__int32" ,
                "__int32*" ,
                "__int64" ,
                "__int64*" ,
                "__int8" ,
                "__int8*"
            };
            public string[] parametrs_for_method_methods =
            {
                "int" ,//0
                "int*" ,//1
                "float" ,//2
                "string" ,//3
                "string*" ,//4
                "const char" ,//5
                "const char*" ,//6
                "char" ,//7
                "char*" ,//8
                "__int32" ,//9
                "__int32*" ,//10
                "__int64" ,//11
                "__int64*" ,//12
                "__int8" ,//13
                "__int8*",//14
                "byte",//15
            };
            public string[] type_for_variables =
            {
                "int" ,//0
                "int*" ,//1
                "float" ,//2
                "string" ,//3
                "string*" ,//4
                "const char*" ,//5
                "char*" ,//6
                "__int32" ,//7
                "__int32*" ,//8
                "__int64" ,//9
                "__int64*" ,//10
                "__int8" ,//11
                "__int8*",//12
                "byte",//13
                "BYTE"//14
            };

            public string[] type_for_function_default =
            {
                "rand",
                "strstr",
                "rint",
                "mersenne",
                "rint"
            };
            public string[] type_for_function_driver =
            {
                "DbgPrint", //DbgPrint(__VA_ARGS__)
                "strstr"
            };

            public string[] others_for_method_methods =
            {
                ";",
                "{",
                "}",
                "(",
                ")"
            };
            public string[] extension =
            {
                ".c",
                ".h",
                ".cpp",
                ".hpp"
            };
            public string Sleep = "Sleep";
        }
        public string[] vRandom_All =
        {
            "0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
            "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P", "A", "S", "D", "F", "G", "H", "J", "K", "L", "Z", "X", "C", "V", "B", "N", "M",
            "q", "w", "e", "r", "t", "y", "u", "i", "o", "p", "a", "s", "d", "f", "g", "h", "j", "k", "l", "z", "x", "c", "v", "b", "n", "m"
        };
        public string[] vRandom_Int =
            { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        public string[] vRandom_ABC =
            { "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P", "A", "S", "D", "F", "G", "H", "J", "K", "L", "Z", "X", "C", "V", "B", "N", "M" };
        public string[] vRandom_abc =
            { "q", "w", "e", "r", "t", "y", "u", "i", "o", "p", "a", "s", "d", "f", "g", "h", "j", "k", "l", "z", "x", "c", "v", "b", "n", "m" };
        public string[] vRandom_Abc =
        {
            "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P", "A", "S", "D", "F", "G", "H", "J", "K", "L", "Z", "X", "C", "V", "B", "N", "M",
            "q", "w", "e", "r", "t", "y", "u", "i", "o", "p", "a", "s", "d", "f", "g", "h", "j", "k", "l", "z", "x", "c", "v", "b", "n", "m"
        };
        public string[] vRandom_Simvols =
        {
            "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "-", "+", "="
        };

        /// <summary>
        /// name=FUNC_1:type=method,start=9,end=19,prms=<strstr><rand>;
        /// </summary>
        public struct InfoMethod
        {
            public string Name;
            public string Type;
            public int Start;
            public int End;
            public string Parameters;
            public InfoMethod(string name, string type, int start, int end, string prms)
            {
                Name = name;
                Type = type;
                Start = start;
                End = end;
                Parameters = prms;
            }
        }
    }
}
