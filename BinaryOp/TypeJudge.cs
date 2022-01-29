using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

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

        /// <summary>
        /// オブジェクトの型ごとに判定してバイト配列を返す。
        /// プリミティブ型が冗長になってる気がするから今後改善される可能性あり。
        /// BitConverter.GetBytesのオーバーロードがオブジェクト受け付けてないので仕方ない？
        /// プリミティブ型の種類については以下の URL の Remarks を参照。(ポインターは対象外)
        /// https://docs.microsoft.com/en-us/dotnet/api/system.type.isprimitive?view=net-6.0
        /// </summary>
        /// <param name="o">変換対象のオブジェクト</param>
        /// <returns>変換後の配列</returns>
        public static byte[] TJudge(object o)
        {
            Type type = o.GetType();

            // プリミティブ型の処理
            if (type.IsPrimitive)
            {
                switch (o)
                {
                    case Boolean v:
                        return BitConverter.GetBytes(v);
                    case Char v:
                        return BitConverter.GetBytes(v);
                    case Double v:
                        return BitConverter.GetBytes(v);
                    case Half v:
                        return BitConverter.GetBytes(v);
                    case Int16 v:
                        return BitConverter.GetBytes(v);
                    case Int32 v:
                        return BitConverter.GetBytes(v);
                    case Int64 v:
                        return BitConverter.GetBytes(v);
                    case Single v:
                        return BitConverter.GetBytes(v);
                    case UInt16 v:
                        return BitConverter.GetBytes(v);
                    case UInt32 v:
                        return BitConverter.GetBytes(v);
                    case UInt64 v:
                        return BitConverter.GetBytes(v);
                    case Byte v:
                        return new byte[] { v };
                    case SByte v:
                        return new byte[] { (byte)v };
                    default:
                        return Array.Empty<byte>();
                }
            }
            // 配列型の処理
            else if (type.IsArray)
            {
                var array = o as Array;
                int index = 0;
                byte[] b = new byte[array!.Length * Marshal.SizeOf(array.GetValue(0)!.GetType())];
                foreach (var item in array)
                {
                    foreach (var x in TJudge(item))
                    {
                        b[index++] = x;
                    }
                }
                return b;
            }
            // string 型処理
            else if (type == typeof(string))
            {
                return Encoding.UTF8.GetBytes((string)o);
            }
            // その他オブジェクト
            else
            {
                Type typeList = o.GetType();
                PropertyInfo[] propertyInfos = typeList.GetProperties();
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {

                }
                throw new Exception();
            }
        }

        public int GetByteSize<T>(T t)
        {
            Type type = t.GetType();
            if (type.IsPrimitive)
            {

            }
            return 0;
        }
    }
}
