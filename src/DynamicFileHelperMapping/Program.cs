using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FileHelpers;
using FileHelpers.Dynamic;
using Newtonsoft.Json;

namespace DynamicFileHelperMapping
{
    class Program
    {
        static void Main(string[] args)
        {
            var m = JsonConvert.DeserializeObject<Mapping>(File.ReadAllText("mapping.json"));

            var c = new DelimitedClassBuilder(m.Name.Replace(" ", "_"));
            c.IgnoreEmptyLines = m.IgnoreEmptyLines;
            c.Delimiter = m.Delimiter;
            c.IgnoreFirstLines = m.IgnoreFirstLines;
            c.IgnoreLastLines = m.IgnoreLastLines;

            var cols = new Dictionary<string,DelimitedFieldBuilder>();
            foreach (var field in m.Fields.Where(field => !cols.ContainsKey(field.Source)))
            {
                cols.Add(field.Source, c.AddField(field.Source, GetType(field.Type)));
            }
            
            foreach (var delimitedFieldBuilder in cols)
            {
                delimitedFieldBuilder.Value.FieldQuoted =true;
                delimitedFieldBuilder.Value.QuoteMode= QuoteMode.OptionalForBoth;
            }
            var stype = c.CreateRecordClass();
            var e = new FileHelperEngine(stype);
            var d = e.ReadFileAsList("Code-Review-Interview-Data-File.csv");

            var exp = Mapper.CreateMap(stype, typeof (MemberDetail));
            foreach (var field in m.Fields)
            {
                exp.ForMember(field.Destination, opt => opt.MapFrom(field.Source));
            }

            var mdetails = Mapper.Map<MemberDetail[]>(d);

        }

        static Type GetType(string typeName)
        {
            switch (typeName)
            {
                case "String":
                    return typeof (string);
                    break;
            }

            return null;
        }
    }
}
