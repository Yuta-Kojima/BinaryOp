using System.Collections;
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

        public static int GetByteSize<T>(T t, Type type = null)
        {
            int byteSize = 0;
            if (type == null)
            {
                type = t?.GetType() ?? typeof(T);
            }

            if (type.IsPrimitive)
            {
                byteSize += Marshal.SizeOf(type);
                Console.WriteLine($"P:{type} => [ {byteSize} ]");
            }
            else if (type == typeof(string))
            {
                byteSize += Encoding.UTF8.GetByteCount(t as string ?? string.Empty);
                Console.WriteLine($"S:string => [ {byteSize} ]");
            }
            else if (type.IsArray)
            {
                // 配列サイズを記録するために Int32 型のバイトサイズを加算
                byteSize += Marshal.SizeOf<int>();
                Console.WriteLine($"SizeData => {Marshal.SizeOf<int>()}");
                var array = t as IEnumerable;
                int i = 0;
                foreach (var item in array)
                {
                    Console.Write($"A:{t.GetType()}[{i++}] => ");
                    byteSize += GetByteSize(item);
                }
            }
            else
            {
                PropertyInfo[] propertyInfos = type.GetProperties();
                for (int i = 0; i < propertyInfos.Length; i++)
                {
                    byteSize += GetByteSize(propertyInfos[i].GetValue(t), propertyInfos[i].PropertyType);
                }
            }
            return byteSize;
        }


        /// <summary>
        /// オブジェクトの型ごとに判定してバイト配列を返す。
        /// プリミティブ型が冗長になってる気がするから今後改善される可能性あり。
        /// BitConverter.GetBytesのオーバーロードがオブジェクト受け付けてないので仕方ない？
        /// プリミティブ型の種類については以下の URL の Remarks を参照。(ポインターは対象外)
        /// https://docs.microsoft.com/en-us/dotnet/api/system.type.isprimitive?view=net-6.0
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static byte[] GetBytes<T>(T t, Type type = null)
        {
            if (type == null)
            {
                type = t?.GetType() ?? typeof(T);
            }
            int byteSize = GetByteSize(t);
            byte[] bytes = new byte[byteSize];
            int index = 0;

            // プリミティブ型の処理
            if (type.IsPrimitive)
            {
                switch (t)
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
                var array = t as Array ?? Array.Empty<byte>();

                foreach (var item in array)
                {
                    foreach (var x in GetBytes(item))
                    {
                        bytes[index++] = x;
                    }
                }
                return bytes;
            }
            // string 型処理
            else if (type == typeof(string))
            {
                return Encoding.UTF8.GetBytes(t as string ?? string.Empty);
            }
            // その他オブジェクト
            else
            {
                PropertyInfo[] propertyInfos = type.GetProperties();
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    foreach (var x in GetBytes(propertyInfo.GetValue(t), propertyInfo.PropertyType))
                    {
                        bytes[index++] = x;
                    }
                }
                return bytes;
            }
        }
    }
}
