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
        public BWriter(T targetObject)
        {
            TargetObject = targetObject;
        }

        public void Write()
        {
            byte[] _bytes = TypeJudge.GetBytes(TargetObject);
        }

    }
}