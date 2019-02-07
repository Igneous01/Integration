
using Integration.Attributes.Property;
using Integration.Attributes.Type;
using Integration.Xml.Attributes.Property;
using Integration.Xml.Attributes.Type;
using Integration.Xml.Enums;
using Integration.Xml.Interfaces;
using System;
using System.Collections.Generic;

namespace SampleApp.domain
{
    public class BooleanConverter : IXmlPropertyConverter<bool>
    {
        public bool ConvertToSourceType(string source)
        {
            if ("y".Equals(source.ToLower()))
                return true;
            else
                return false;
        }

        public string ConvertToDestinationType(bool source)
        {
            if (source)
                return "Y";
            else
                return "N";
        }
    }

    public class DateTimeConverter : IXmlPropertyConverter<DateTime>
    {
        private const string DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss";

        public DateTime ConvertToSourceType(string source)
        {
            return DateTime.ParseExact(source, DATE_TIME_FORMAT, System.Globalization.CultureInfo.InvariantCulture);
        }

        public string ConvertToDestinationType(DateTime source)
        {
            return source.ToString(DATE_TIME_FORMAT);
        }
    }

    [IncludeInScan]
    public class UnMarked
    {
        public int UnmarkedFieldA { get; set; }
        public double UnmarkedFieldB { get; set; }
    }

    [XmlMapper(ParentNodeName = "Applicant", MappingOperation = XmlMappingOperation.NODE)]
    public class Applicant
    {
        [Name("name")]
        public string FirstName { get; set; }

        [Name("surname")]
        public string LastName { get; set; }
    }

    [XmlMapper(ParentNodeName = "ApplicationNode", 
        MappingOperation = XmlMappingOperation.NODE,
        IgnoreNulls = true)]
    public class Application
    {
        public int ApplicationID { get; set; }

        [XmlPropertyConverter(typeof(DateTimeConverter))]
        [Name("date")]
        public DateTime CreateDate { get; set; }

        public string Name { get; set; }

        public object AnotherNull { get; set; }
    }

    [XmlMapper(ParentNodeName = "Loan", 
        MappingOperation = XmlMappingOperation.NODE, 
        IgnoreNulls = false)]
    public class Loan
    {
        [Ignore]
        public string ProductCode { get; set; }

        [Name("name")]
        public string Name { get; set; }

        [Name("amount")]
        public int Amount { get; set; }

        [Name("joint")]
        [XmlPropertyConverter(typeof(BooleanConverter))]
        public bool IsJointAccount { get; set; }

        [Name("applicant")]
        public Applicant Applicant { get; set; }

        public Application Application { get; set; }

        [Name("CustomList")]
        [XmlList(NodeName = "ListingItem")]
        public List<string> Listing { get; set; }

        public object Null { get; set; }
    }

    [XmlMapper(ParentNodeName = "CustomLoan", 
        MappingOperation = XmlMappingOperation.ATTRIBUTE, 
        IgnoreNulls = true,
        Formatting = System.Xml.Linq.SaveOptions.None)]
    public class CustomLoan : Loan
    {
        public string CustomFieldA { get; set; }
        public string CustomFieldB { get; set; }

        public Dictionary<string, UnMarked> Dictionary { get; set; }
        public LinkedList<Applicant> JointApplicants { get; set; }
    }

}
