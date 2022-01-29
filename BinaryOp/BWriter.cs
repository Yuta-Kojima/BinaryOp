using System;
using System.Reflection;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;

namespace BinaryOp
{
    /// <summary>
    /// オブジェクトの情報をバイナリデータとして書き込むクラス
    /// </summary>
    /// <typeparam name="T">書き込む対象のクラス・オブジェクト</typeparam>
    public class BWriter<T>
    {
        public T TargetObject { get; private set; }
        public BWriter(T targetObject = default)
        {
            TargetObject = targetObject;
        }

        public void Write()
        {
            Type typeOfListString = TargetObject.GetType();
            PropertyInfo[] propertyInfos = typeOfListString.GetProperties();
            int byteSize = propertyInfos.Sum(item => {
                if (item.PropertyType.IsValueType)
                {
                    return Marshal.SizeOf(item.PropertyType);
                }
                else if(item.PropertyType == typeof(string))
                {
                    Encoding utf8 = Encoding.UTF8;
                    string s = (string)item.GetValue(TargetObject)!;
                    return utf8.GetByteCount(s);
                }else if (item.PropertyType.IsArray)
                {
                    var a = item.GetValue(TargetObject) as Array;
                    int sum = 0;
                    foreach(var i in a!)
                    {
                        Console.WriteLine(i);
                        sum += Marshal.SizeOf(i);
                    }
                    return sum;

                }
                return 0;
            });

            byte[] bytes = new byte[byteSize];
            int pointer = 0;

            foreach(PropertyInfo propertyInfo in propertyInfos)
            {
                var value = propertyInfo.GetValue(TargetObject) ?? new object();

                Console.WriteLine($"{propertyInfo.Name} = ({propertyInfo.PropertyType}) {propertyInfo.GetValue(TargetObject)}");
                Type type = propertyInfo.PropertyType; 
                var cnvertedBytes = TypeJudge.TJudge(value);
                foreach(var cnvByte in cnvertedBytes)
                {
                    bytes[pointer++] = cnvByte;
                }
            }

            Console.WriteLine(bytes);
        }

    }
}