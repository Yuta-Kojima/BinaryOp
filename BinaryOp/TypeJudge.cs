using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace BinaryOp
{
    /// <summary>
    /// オブジェクトの型に関する判定を行うメソッドをまとめたクラス
    /// (インスタンス化して使うことはたぶんない)
    /// </summary>
    internal class TypeJudge
    {
        public static TypeJudge Instance { get; private set; } = new TypeJudge();
        private TypeJudge()
        {

        }

        public static byte[] TJudge(object o)
        {

            switch (o)
            {
                case int v:
                    return BitConverter.GetBytes(v);
                case long v:
                    return BitConverter.GetBytes(v);
                case short v:
                    return BitConverter.GetBytes(v);
                case string v:
                    return Encoding.UTF8.GetBytes(v);
                default:
                    if (o.GetType().IsArray)
                    {
                        var arr = o as Array;
                        int l = arr.Length;
                        int byteSize = Marshal.SizeOf(arr.GetValue(0).GetType());
                        byte[] b = new byte[l * Marshal.SizeOf(arr.GetValue(0).GetType())];
                        int pointer = 0;
                        foreach(var a in arr)
                        {
                            foreach(var x in TJudge(a))
                            {
                                b[pointer++] = x;
                            }
                        }
                        return b;
                    }
                    return null;
                    break;
            }
        }
    }
}
