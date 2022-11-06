using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VillageRTS
{
    public enum DBT
    {
        Digit,
        Name,
        Description
    }

    public static partial class Program
    {

        public class DBText
        {
            public string this[Resource index] => CurrentLang.Get("Resource." + index.ToString());
            public string this[string index] => CurrentLang.Get(index);
            public string this[Action action] => CurrentLang.Get(action.Typename);
            public string this[Building building] => CurrentLang.Get(building.Typename);
            public string this[Building building, DBT dbt]
            {
                get
                {
                    if (dbt == DBT.Description) return CurrentLang.GetClear(building.Typename + ".Description");
                    if (dbt == DBT.Name) return CurrentLang.Get(building.Typename + ".Title");
                    return this[building];
                }
            }
            public string this[Action action, DBT dbt]
            {
                get
                {
                    if (dbt == DBT.Description) return CurrentLang.GetClear(action.Typename + ".Description");
                    if (dbt == DBT.Name) return CurrentLang.Get(action.Typename + ".Title");
                    return this[action];
                }
            }
        }

        public class Lang
        {
            public string Ident => Text["%LANGID%"];
            public string Title => Text["%LANGNAME%"];
            public Dictionary<string, string> Text = new Dictionary<string, string>();

            public string GetClear(string key) => Text.ContainsKey(key) ? Text[key] : "";
            public string Get(string key) => Text.ContainsKey(key) ? Text[key] : key;

            public static Lang[] Langs { get; private set; }

            static Lang()
            {
                var langs = new List<Lang>();
                var assembly = Assembly.GetExecutingAssembly();
                foreach (var x in assembly.GetManifestResourceNames().Where(x => x.Contains("Langs.lang_")))
                    using (var stream = assembly.GetManifestResourceStream(x)) langs.Add(ReadLang(stream));
                Langs = langs.ToArray();
            }

            static Lang ReadLang(Stream stream)
            {
                var lang = new Lang();
                using (var tr = new StreamReader(stream))
                {
                    while (!tr.EndOfStream)
                    {
                        var s = tr.ReadLine();
                        if (s.StartsWith("//") || string.IsNullOrWhiteSpace(s)) continue;

                        var splitter = s.IndexOf('=');
                        lang.Text[s.Substring(0, splitter)] = s.Substring(splitter + 1, s.Length - splitter - 1);
                    }
                }
                return lang;
            }
        }

        public static Lang CurrentLang { get; set; } = Lang.Langs.First();

        public static readonly DBText Text = new DBText();
    }
}
