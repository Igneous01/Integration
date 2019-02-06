using Integration.Cache;
using Integration.Xml.Attributes.Type;
using Integration.Xml.Mapper;
using SampleApp.domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;

namespace SampleApp
{
    class Program
    {
        public static string ToXML<T>(T Instance)
        {
            using (var stringwriter = new System.IO.StringWriter())
            {
                var serializer = new XmlSerializer(Instance.GetType());
                serializer.Serialize(stringwriter, Instance);
                return stringwriter.ToString();
            };
        }

        static void Main(string[] args)
        {
            List<CustomLoan> loans = new List<CustomLoan>();
            for (int i = 0; i < 1000; i++)
            {
                loans.Add(new CustomLoan()
                    {
                        Name = "Revolving Loan",
                        Amount = 25000,
                        IsJointAccount = true,
                        ProductCode = "RLP",
                        Applicant = new Applicant()
                        {
                            FirstName = "George",
                            LastName = "Bush"
                        },
                        Application = new Application()
                        {
                            ApplicationID = 1,
                            CreateDate = new DateTime(2018, 5, 22),
                            Name = "New Application"
                        },
                        Listing = new List<string>() { "string", "another", "text" },
                        CustomFieldA = "Custom Data For Client Here",
                        CustomFieldB = "Custom Other Data For Client Here",
                        //Dictionary = new Dictionary<string, UnMarked>()
                        //{
                        //    { "KeyA", new UnMarked(){ UnmarkedFieldA = 20, UnmarkedFieldB = 3.14564 } },
                        //    { "KeyB", new UnMarked(){ UnmarkedFieldA = 77, UnmarkedFieldB = 16.981357 } },
                        //    { "KeyC", new UnMarked(){ UnmarkedFieldA = 86486, UnmarkedFieldB = -1.1384 } }
                        //}
                    });
            }


            var xmlMapper = new XmlMapper<CustomLoan>();
            string values = "";

            Stopwatch watch = Stopwatch.StartNew();

            foreach (CustomLoan loan in loans)
                values = xmlMapper.MapToXmlString(loan);    

            watch.Stop();

            Console.WriteLine(watch.ElapsedMilliseconds);
            Console.WriteLine(values);

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(values);

            CustomLoan mappedBackObject = xmlMapper.MapToObject(xml);

            //if (mappedBackObject.Equals(loans[0]))
            //{
            //    Console.WriteLine("HALELUAH");
            //}

            Console.WriteLine("Serialization");
            Console.WriteLine(ToXML(loans[0]));

            Console.ReadKey();
            return;
        }
    }
}
