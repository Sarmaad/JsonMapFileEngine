using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DynamicFileHelperMapping;
using FileHelpers;
using FileHelpers.Dynamic;
using Newtonsoft.Json;

namespace Engine
{
    public sealed class JsonEngine
    {
        readonly string _jsonMap;

        public JsonEngine(string jsonMap)
        {
            _jsonMap = jsonMap;
        }

        public T[] Parse<T>(string file)
        {
            var m = JsonConvert.DeserializeObject<Mapping>(_jsonMap);

            var c = new DelimitedClassBuilder(m.Name.Replace(" ", "_"))
            {
                IgnoreEmptyLines = m.IgnoreEmptyLines,
                Delimiter = m.Delimiter,
                IgnoreFirstLines = m.IgnoreFirstLines,
                IgnoreLastLines = m.IgnoreLastLines
            };

            var cols = new Dictionary<string, DelimitedFieldBuilder>();
            foreach (var field in m.Fields.Where(field => !cols.ContainsKey(field.Source)))
            {
                cols.Add(field.Source, c.AddField(field.Source, GetType(field.Type)));
            }

            foreach (var delimitedFieldBuilder in cols)
            {
                delimitedFieldBuilder.Value.FieldQuoted = true;
                delimitedFieldBuilder.Value.QuoteMode = QuoteMode.OptionalForBoth;
            }
            var stype = c.CreateRecordClass();
            var e = new FileHelperEngine(stype);
            var d = e.ReadFileAsList(file);

            var exp = Mapper.CreateMap(stype, typeof(T));
            foreach (var field in m.Fields)
            {
                exp.ForMember(field.Destination, opt => opt.MapFrom(field.Source));
            }

            return Mapper.Map<T[]>(d);
        }

        Type GetType(string typeName)
        {
            switch (typeName)
            {
                case "String":
                    return typeof(string);
                    break;
            }

            return null;
        }
    }
}
