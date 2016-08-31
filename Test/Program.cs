using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using PanGu;
using Lucene.Net.Analysis.PanGu;
using NHibernate;
using NHibernate.Linq;
using z.Foundation.Data;
using z.AdminCenter.Entity;
using System.Threading;
using hk.papago.Entity.PaPaGoDB;
using System.Diagnostics;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //sbyte[] numbers = { SByte.MinValue, -1, 0, 10, 100, SByte.MaxValue };
            //bool result;

            //foreach (sbyte number in numbers)
            //{
            //    result = Convert.ToBoolean(number);
            //    Console.WriteLine("{0,-5}  -->  {1}", number, result);
            //}

            //PanGu.Segment.Init(@"D:\project_about\file_server\pangu\PanGu.xml");

            //string strKeyword = GetKeyWordsSplitBySpace("荷兰牛栏奶粉1+段800kg", new PanGuTokenizer());




            //for (int i = 0; i < 1; i++)
            //{
            //    Stopwatch watch = new Stopwatch();
            //    watch.Start();
            //    ISession session = NHibernateHelper<order_info>.OpenSession();
            //    watch.Stop();
            //    Console.WriteLine(watch.ElapsedMilliseconds + "ms");

            //    Stopwatch watch1 = new Stopwatch();
            //    watch1.Start();
            //    ISession session1 = NHibernateHelper<order_info>.OpenSession();
            //    watch1.Stop();
            //    Console.WriteLine(watch1.ElapsedMilliseconds + "ms");

            //    Stopwatch watch2 = new Stopwatch();
            //    watch2.Start();
            //    ISession session2 = NHibernateHelper<supplier>.OpenSession();
            //    watch2.Stop();
            //    Console.WriteLine(watch2.ElapsedMilliseconds + "ms");

            //    Stopwatch watch3 = new Stopwatch();
            //    watch3.Start();
            //    ISession session3 = NHibernateHelper<seller>.OpenSession();
            //    watch3.Stop();
            //    Console.WriteLine(watch3.ElapsedMilliseconds + "ms");
            //}

            ISession session1 = NHibernateHelper<admin_system>.OpenSession();
            var a = session1.Query<admin_system>().ToList();

            ISession session5 = NHibernateHelper<admin_system>.OpenSession();
            var e = session5.Query<admin_system>().ToList();

            ISession session3 = NHibernateHelper<admin_user>.OpenSession();
            var c = session3.Query<admin_user>().ToList();

            ISession session2 = NHibernateHelper<supplier>.OpenSession();
            var b = session2.Query<supplier>().ToList();

            ISession session6 = NHibernateHelper<supplier>.OpenSession();
            var f = session6.Query<supplier>().ToList();

            ISession session4 = NHibernateHelper<brand>.OpenSession();
            var d = session4.Query<brand>().ToList();



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
