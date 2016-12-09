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
//using hk.papago.Entity.PaPaGoDB;
using System.Diagnostics;
using NHibernate.Context;
using z.Foundation;
using System.Web;
using System.IO;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //test zengen
            //hello world

            //zengen

            //PanGu.Segment.Init(@"D:\project_about\file_server\pangu\PanGu.xml");


            //PanGu.Segment.Init(@"D:\project_about\file_server\pangu\PanGu.xml");

            //PanGu.Segment.Init(@"D:\project_about\file_server\pangu\PanGu.xml");

            //PanGu.Segment.Init(@"D:\project_about\file_server\pangu\PanGu.xml");
            //PanGu.Segment.Init(@"D:\project_about\file_server\pangu\PanGu.xml");
            //PanGu.Segment.Init(@"D:\project_about\file_server\pangu\PanGu.xml");
            //PanGu.Segment.Init(@"D:\project_about\file_server\pangu\PanGu.xml");
            //PanGu.Segment.Init(@"D:\project_about\file_server\pangu\PanGu.xml");
            //PanGu.Segment.Init(@"D:\project_about\file_server\pangu\PanGu.xml");
            //PanGu.Segment.Init(@"D:\project_about\file_server\pangu\PanGu.xml");

            //PanGu.Segment.Init(@"D:\project_about\file_server\pangu\PanGu.xml");

            //Segment segment = new Segment();
            //PanGu.Match.MatchOptions options = new PanGu.Match.MatchOptions();
            //PanGu.Setting.PanGuSettings.Config.MatchOptions.CopyTo(options);
            //options.MultiDimensionality = false;

            //StringBuilder result = new StringBuilder();

            //ICollection<WordInfo> words = segment.DoSegment("广东省深圳市罗罗区", options);
            //foreach (WordInfo word in words)
            //{
            //    if (word == null)
            //    {
            //        continue;
            //    }
            //    result.AppendFormat("{0} ", word.Word);
            //}

            //string str = result.ToString();

            //result = new StringBuilder();
            //words = segment.DoSegment("广东省深圳市罗罗区", options);
            //foreach (WordInfo word in words)
            //{
            //    if (word == null)
            //    {
            //        continue;
            //    }
            //    result.AppendFormat("{0} ", word.Word);
            //}

            // str = result.ToString();


            //PanGu.Segment.Init(@"D:\project_about\file_server\pangu\PanGu.xml");
            //PanGu.Segment.Init(@"D:\project_about\file_server\pangu\PanGu.xml");
            //PanGu.Segment.ReInit();
            //PanGu.Segment.Init(@"D:\project_about\file_server\pangu\PanGu.xml");





            //words = segment.DoSegment("广东省深圳市罗湖区", options);
            //foreach (WordInfo word in words)
            //{
            //    if (word == null)
            //    {
            //        continue;
            //    }
            //    result.AppendFormat("{0} ", word.Word);
            //}

            //string strTemp = result.ToString();

            //string strOriginAddress = "广东省深圳市罗湖区";

            //strOriginAddress = strOriginAddress.Replace(" ", "");//替换地址中的空格
            //strOriginAddress = strOriginAddress.IsMatch("^中国") ? strOriginAddress.Substring(2, strOriginAddress.Length - 2) : strOriginAddress;//替换地址中的中国字样
            //strOriginAddress = strOriginAddress.RegexReplace(@"\[|\]", "");//替换地址中特殊字符

            ////对初步处理后的原始地址进行盘古分词
            //string strKeyword = GetKeyWordsSplitBySpace(strOriginAddress, new PanGuTokenizer());
            //List<string> addressList = strKeyword.Split(' ').ToList();

            ////根据是否是直辖市来判断需要循环取省、市、区的次数
            //int loop = 3;
            //bool bMunicipality = false;
            //if (strOriginAddress.IsMatch("^北京|上海|重庆|天津"))
            //{
            //    if (addressList.Count >= 2 && (addressList[1].IndexOf(addressList[0]) > -1 || addressList[0].IndexOf(addressList[1]) > -1))
            //    {
            //        loop = 3;
            //    }
            //    else
            //    {
            //        loop = 2;
            //    }

            //    bMunicipality = true;
            //}

            ////分词后的地址在长度上必须满足省、市、区、详细地址基本长度要求
            //if (addressList.Count >= loop + 1)
            //{
            //    string strPartOfAddress = "";
            //    List<string> partOfAddressList = new List<string>();

            //    for (int i = 0; i < loop; i++)
            //    {
            //        strPartOfAddress += addressList[i];

            //        //省被单独分隔成一个字时，代表该省实为自治区或直辖市
            //        if (addressList[i] == "省")
            //        {
            //            loop++;
            //            if (bMunicipality)
            //            {
            //                loop++;
            //            }
            //            continue;
            //        }

            //        partOfAddressList.Add(addressList[i]);
            //    }

            //    string strProvince = "";
            //    string strCity = "";
            //    string strArea = "";
            //    string strDetailAddress = "";

            //    //省、市、区处理
            //    if (bMunicipality && partOfAddressList.Count < 3)
            //    {
            //        strProvince = strCity = partOfAddressList[0];
            //        strArea = partOfAddressList[1];
            //    }
            //    else
            //    {
            //        strProvince = partOfAddressList[0];
            //        strCity = partOfAddressList[1];
            //        strArea = partOfAddressList[2];
            //    }

            //    //详细地址处理
            //    strDetailAddress = strOriginAddress.Replace(strPartOfAddress, "");
            //    strDetailAddress = strDetailAddress.Replace(strProvince, "");
            //    strDetailAddress = strDetailAddress.Replace(strCity, "");
            //    strDetailAddress = strDetailAddress.Replace(strArea, "");
            //    if (strArea.IsMatch(@"^.+(?<!市)$") && strDetailAddress.IsMatch(@"^市.+"))
            //    {
            //        strDetailAddress = strDetailAddress.RegexReplace(@"^市", "");
            //    }
            //    if (strArea.IsMatch(@"^.+(?<!县)$") && strDetailAddress.IsMatch(@"^县.+"))
            //    {
            //        strDetailAddress = strDetailAddress.RegexReplace(@"^县", "");
            //    }
            //    if (strArea.IsMatch(@"^.+(?<!区)$") && strDetailAddress.IsMatch(@"^区.+"))
            //    {
            //        strDetailAddress = strDetailAddress.RegexReplace(@"^区", "");
            //    }
            //    if (strArea.IsMatch(@"^.+(?<!镇)$") && strDetailAddress.IsMatch(@"^镇.+"))
            //    {
            //        strDetailAddress = strDetailAddress.RegexReplace(@"^镇", "");
            //    }

            //    //标准化省、市、区
            //    //TO DO




            //    Console.WriteLine(strProvince);
            //    Console.WriteLine(strCity);
            //    Console.WriteLine(strArea);
            //    Console.WriteLine(strDetailAddress);
            //}





            //PanGu.Segment.Init(@"D:\project_about\file_server\pangu\PanGu.xml");
            //string str = GetKeyWordsSplitBySpace("爱他美A2+段", new PanGuTokenizer());

            //Console.WriteLine(str);

            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine(i);

                string strSource = "";
                string strSerializeTempFile = Utility.GetConfigValue("FileServer") + "\\source.txt";
                using (StreamReader sr = new StreamReader(strSerializeTempFile, Encoding.UTF8))
                {
                    strSource = sr.ReadToEnd();
                }

                //string base64string = strSource.XmlSerialize(Encoding.UTF8);

                //string a = base64string.XmlDeserialize<string>(Encoding.UTF8);


                string base64string = strSource.Serialize();

                string a = base64string.Deserialize<string>();
                //a = null;
                ////base64string = null;
                //GC.Collect();

                //string base64string = strSource.JsonSerialize();

                //string a = base64string.JsonDeserialize<string>();

                //FileOperate.Write(base64string, Utility.GetConfigValue("FileServer") + "\\base64.txt");
            }

            //for (int i = 0; i < 5; i++)
            //{
            //    string strBase64 = "";
            //    string strSerializeTempFile = Utility.GetConfigValue("FileServer") + "\\base64.txt";
            //    using (StreamReader sr = new StreamReader(strSerializeTempFile, Encoding.UTF8))
            //    {
            //        strBase64 = sr.ReadToEnd();
            //    }

            //    string strSource = strBase64.Deserialize<string>();

            //    //FileOperate.Write(strSource, Utility.GetConfigValue("FileServer") + "\\source1.txt");
            //}

            //TestClass testClass = new TestClass();
            //testClass.Name = "hello world";

            //string base64string1 = testClass.Serialize();

            //var result1 = base64string1.Deserialize<TestClass>();


            //TestClass2 testClass2 = new TestClass2();
            //testClass2.Name2 = "hello world";

            //string base64string = testClass2.Serialize();

            //var result = base64string.Deserialize<TestClass2>();


            //AAClass aaClass = new AAClass();
            //aaClass.Name = "hello world";

            //string base64string1 = aaClass.Serialize();

            //var result1 = base64string1.Deserialize<AAClass>();






            //var result = base64string.Deserialize<TestClass>();



            //Console.Write("finish");




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

    public static class Extensions
    {
        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> list, int chunkSize)
        {
            while (list.Take(1).Count() > 0)
            {
                yield return list.Take(chunkSize);
                list = list.Skip(chunkSize);
            }
        }
    }

    [Serializable]
    public class TestClass
    {
        public string Name
        {
            get; set;
        }
    }

    [Serializable]
    public class AAClass
    {
        public string Name
        {
            get; set;
        }
    }

    [Serializable]
    public class TestClass2
    {
        public string Name2
        {
            get; set;
        }
    }
}
