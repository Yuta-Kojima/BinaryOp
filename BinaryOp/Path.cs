using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryOp
{
    public class Path
    {
        private List<string> PathItems { get; set; }

        public Path(string path = "") {
            PathItems = path.Replace('/', '\\').Split('\\').ToList();
        }

        public void Up()
        {
            if (PathItems.Count == 0) { return; }
            PathItems.RemoveAt(PathItems.Count-1);
        }

        public bool Add(string path, string extend = "")
        {
            PathItems.Add(extend.Length != 0 ? $"{path}.{extend}" : path);
            if (File.Exists(Get()))
            {
                return true;
            }
            return false;
        }

        public string Get()
        {
            StringBuilder fullPath = new();
            int index = 0;
            PathItems.ForEach(item => { 
                fullPath.Append(item);
                if (PathItems.Count-1 != index) { fullPath.Append('/'); }
                
                index++;
            });

            return fullPath.ToString();
        }
    }
}
