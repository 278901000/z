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
using NHibernate.Context;
using z.Foundation;

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

            PanGu.Segment.Init(@"D:\project_about\file_server\pangu\PanGu.xml");

            //string strOriginAddress = "江苏省南通市海门市海门叠石桥国际家纺城三期一楼2区29号";
            string strOriginAddress = "内蒙古省巴彦淖尔盟市临河区团结路美丽洋房一单元5";
            strOriginAddress = strOriginAddress.Replace(" ", "");
            string strKeyword = GetKeyWordsSplitBySpace(strOriginAddress, new PanGuTokenizer());
            List<string> addressList = strKeyword.Split(' ').ToList();

            int loop = 3;
            bool bMunicipality = false;
            if (strOriginAddress.IsMatch("^北京|上海|重庆|天津"))
            {
                loop = 2;
                bMunicipality = true;
            }

            if (addressList.Count >= loop + 1)
            {
                string strPartOfAddress = "";
                List<string> partOfAddressList = new List<string>();
                for (int i = 0; i < loop; i++)
                {
                    strPartOfAddress += addressList[i];

                    if (addressList[i] == "省")
                    {
                        loop++;
                        if (bMunicipality)
                        {
                            loop++;
                        }
                        continue;
                    }

                    partOfAddressList.Add(addressList[i]);
                }

                string strProvince = partOfAddressList[0];
                string strCity = partOfAddressList[1];
                string strArea = "";
                if (partOfAddressList.Count > 2)
                {
                    strArea = partOfAddressList[2];
                }

                string strDetailAddress = strOriginAddress.Replace(strPartOfAddress, "");


                string a = "";
            }











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

            //ISession session1 = NHibernateHelper<admin_system>.OpenSession();
            //var a = session1.Query<admin_system>().ToList();

            //ISession session5 = NHibernateHelper<admin_system>.OpenSession();
            //var e = session5.Query<admin_system>().ToList();

            //ISession session3 = NHibernateHelper<admin_user>.OpenSession();
            //var c = session3.Query<admin_user>().ToList();

            //ISession session2 = NHibernateHelper<supplier>.OpenSession();
            //var b = session2.Query<supplier>().ToList();

            //ISession session6 = NHibernateHelper<supplier>.OpenSession();
            //var f = session6.Query<supplier>().ToList();

            //ISession session4 = NHibernateHelper<brand>.OpenSession();
            //var d = session4.Query<brand>().ToList();

            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();
            //for (int i = 0; i < 100000; i++)
            //{
            //    NHibernateHelper<admin_system>.OpenSession();
            //}

            //stopwatch.Stop();

            //Console.WriteLine(stopwatch.ElapsedMilliseconds);

            //IRepository repository = new NHibernateRepository();

            //repository.Save<admin_system>(new admin_system()
            //{
            //    Name = "aaa123",
            //    SysKey = "11223344",
            //    Code = "adfadf",
            //    URL = "http://adfadf",
            //    CallBackUrl = "http://adfadf",
            //    CreateBy = "a",
            //    CreateOn = DateTime.UtcNow
            //});

            //var adminSystem = repository.Get<admin_system>(24);
            //adminSystem.Name = "aaa-update";
            //repository.Update<admin_system>(adminSystem);

            //repository.Delete<admin_system>(adminSystem);


            //repository.Delete<admin_system>(25);



            Console.Write("finish");

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
