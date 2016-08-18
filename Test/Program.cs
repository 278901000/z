using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.Foundation.Data;
using System.Configuration;
using PanGu;
using Lucene.Net.Analysis.PanGu;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            PanGu.Segment.Init(@"D:\project_about\file_server\pangu\PanGu.xml");

            string strKeyword = GetKeyWordsSplitBySpace("荷兰牛栏奶粉1+段800kg", new PanGuTokenizer());

            
            Console.Read();
        }

        static string GetKeyWordsSplitBySpace(string keywords, PanGuTokenizer ktTokenizer)
        {
            StringBuilder result = new StringBuilder();
            ICollection<WordInfo> words = ktTokenizer.SegmentToWordInfos(keywords);
            foreach (WordInfo word in words)
            {
                if (word == null)
                {
                    continue;
                }
                result.AppendFormat("{0} ", word.Word);
            }
            return result.ToString().Trim();
        }
    }
}
