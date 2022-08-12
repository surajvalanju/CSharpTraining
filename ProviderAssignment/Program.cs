using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;

namespace ProviderAssignment
{
    public class PatientInfo : DynamicObject
    {
        public string Line { get; set; }

        public string Header { get; set; }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = null;
            string property = binder.Name;
            List<string> headerList = Header.Split(',').ToList();
            int position = headerList.IndexOf(property);
            if (position >0)
            {
                List<string> columnList = Line.Split(',').ToList();
                result = columnList[position];
                return true;
            }
            return false;
        }

        public string dump()
        {
            return Line;
        }
    }

    public class DynamicCSVProvider
    {
        public string FilePath { get; set; }

        public IEnumerable<PatientInfo> GetPatientInfos() {
            List<PatientInfo> patientInfos = new List<PatientInfo>();
            System.IO.StreamReader _reader = new System.IO.StreamReader(FilePath);
            string header = _reader.ReadLine();
            while (!_reader.EndOfStream)
            {
                PatientInfo line = new PatientInfo() { Header = header, Line = _reader.ReadLine() };
                patientInfos.Add(line);
            }
            return patientInfos;
        }
    }

    public class Client
    {
        public void query()
        {
            DynamicCSVProvider cSVProvider = new DynamicCSVProvider();
            cSVProvider.FilePath = "..//..//..//Patient.csv";
            IEnumerable<PatientInfo> patients = cSVProvider.GetPatientInfos();
            IEnumerable<dynamic> result = patients.Where((dynamic p) => Int32.Parse(p.AGE) > 34);
            foreach (var item in result)
            {
                Console.WriteLine(item.dump());
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Client objclient = new Client();
            objclient.query();
            Console.Read();
        }
    }
}
