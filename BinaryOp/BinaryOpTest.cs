#if DEBUG
using RinTester;

namespace BinaryOp
{
    public class BinaryOpTest
    {
        static Test_1 test_1 = new();
        static Test_2 test_2 = new();
        static Test_3 test_3 = new();
        static Test_4 test_4 = new();
        static Test_5 test_5 = new();
        static Test_6 test_6 = new();

        public static void GetByteSizeTest()
        {
            Console.WriteLine("//// GetBytesSize Test ////");
            
            // int 型が 4 つなので 12 bytes
            Test.RinTest("Test 1", () => TypeJudge.GetByteSize(test_1, typeof(Test_1)), 12);

            // int 型が 1 つ(4 bytes)、stringの 1 バイト文字が 5 つ(5 bytes) 3 バイト文字が 5 つ(15 bytes)
            // 合計 24 bytes
            Test.RinTest("Test 2", () => TypeJudge.GetByteSize(test_2), 24);

            // Length を記録するために int 型 1 つ分(4 bytes)、int[] 型の要素が 4 つなので 24 bytes
            Test.RinTest("Test 3", () => TypeJudge.GetByteSize(test_3), 24);

            // string 型が 6 つの配列
            // (Length) =>  4 bytes
            // [0] A        =>  1 byte
            // [1] あ       =>  3 bytes
            // [2] a        =>  1 byte
            // [3] 亜       =>  3 bytes
            // [4] ああああ => 12 bytes
            // [5] ｱｱｱｱ     => 12 bytes
            //                 36 bytes
            Test.RinTest("Test 4", () => TypeJudge.GetByteSize(test_4), 36);

            //Test.RinTest("Test 5", () => TypeJudge.GetByteSize(test_5), 24);

            //Test.RinTest("Test 6", () => TypeJudge.GetByteSize(test_6), 24);

            Console.WriteLine("\n");
        }

        public static void GetBytesTest()
        {

        }

        
    }

    public class Test_1
    {
        public int IntValue1 { get; set; } = 200;
        public int IntValue2 { get; set; } = 300000;
        public int IntValue3 { get; set; } = -10000;
    }

    public class Test_2
    {
        public int IntValue { get; set; } = 200;
        public string StringValue { get; set; } = "ABCD あいうえお";
    }

    public class Test_3
    {
        public int[] IntArrayValue { get; set; } = new int[] { 200, 300, 400, 500, 9999 };
    }

    public class Test_4
    {
        public string[] StringArrayValue { get; set; } = new string[] { "A", "あ", "a", "亜", "ああああ", "ｱｱｱｱ" };
    }

    public class Test_5
    {
        public List<int> IntArrayValue { get; set; } = new List<int>() { 1, 2, 3, 4 };
    }

    public class Test_6
    {
        public Test_1 Test_1 { get; set; } = new Test_1();
        public Test_2 Test_2 { get; set; } = new Test_2();
        public Test_3 Test_3 { get; set; } = new Test_3();
        public Test_4 Test_4 { get; set; } = new Test_4();
        public Test_5 Test_5 { get; set; } = new Test_5();
    }
}
#endif