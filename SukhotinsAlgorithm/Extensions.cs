using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SukhotinsAlgorithm
{
    public static class Extensions
    {
        public static string[] ReadAllLines(this TextReader reader)
        {
            List<string> lines = new List<string>();
            string line = null;
            while ((line = reader.ReadLine()) != null)
                lines.Add(line);
            return lines.ToArray();
        }
    }
}
