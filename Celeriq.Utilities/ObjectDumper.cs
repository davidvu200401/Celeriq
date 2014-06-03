using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Celeriq.Utilities
{
    using System;
    using System.IO;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary />
    public class ObjectDumper
    {
        public static void Write(object o)
        {
            Write(o, 0);
        }

        public static void Write(object o, int depth)
        {
            var dumper = new ObjectDumper(depth);
            dumper.WriteObject(null, o);
        }

        private TextWriter writer;
        private int pos;
        private int level;
        private int depth;

        private ObjectDumper(int depth)
        {
            this.writer = Console.Out;
            this.depth = depth;
        }

        private void Write(string s)
        {
            if (s != null)
            {
                writer.Write(s);
                pos += s.Length;
            }
        }

        private void WriteIndent()
        {
            for (var i = 0; i < level; i++) writer.Write("  ");
        }

        private void WriteLine()
        {
            writer.WriteLine();
            pos = 0;
        }

        private void WriteTab()
        {
            Write("  ");
            while (pos%8 != 0) Write(" ");
        }

        private void WriteObject(string prefix, object o)
        {
            if (o == null || o is ValueType || o is string)
            {
                WriteIndent();
                Write(prefix);
                WriteValue(o);
                WriteLine();
            }
            else if (o is IEnumerable)
            {
                foreach (var element in (IEnumerable) o)
                {
                    if (element is IEnumerable && !(element is string))
                    {
                        WriteIndent();
                        Write(prefix);
                        Write("...");
                        WriteLine();
                        if (level < depth)
                        {
                            level++;
                            WriteObject(prefix, element);
                            level--;
                        }
                    }
                    else
                    {
                        WriteObject(prefix, element);
                    }
                }
            }
            else
            {
                var members = o.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance);
                WriteIndent();
                Write(prefix);
                var propWritten = false;
                foreach (var m in members)
                {
                    var f = m as FieldInfo;
                    var p = m as PropertyInfo;
                    if (f != null || p != null)
                    {
                        if (propWritten)
                        {
                            WriteTab();
                        }
                        else
                        {
                            propWritten = true;
                        }
                        Write(m.Name);
                        Write("=");
                        var t = f != null ? f.FieldType : p.PropertyType;
                        if (t.IsValueType || t == typeof (string))
                        {
                            WriteValue(f != null ? f.GetValue(o) : p.GetValue(o, null));
                        }
                        else
                        {
                            if (typeof (IEnumerable).IsAssignableFrom(t))
                            {
                                Write("...");
                            }
                            else
                            {
                                Write("{ }");
                            }
                        }
                    }
                }
                if (propWritten) WriteLine();
                if (level < depth)
                {
                    foreach (var m in members)
                    {
                        var f = m as FieldInfo;
                        var p = m as PropertyInfo;
                        if (f != null || p != null)
                        {
                            var t = f != null ? f.FieldType : p.PropertyType;
                            if (!(t.IsValueType || t == typeof (string)))
                            {
                                var value = f != null ? f.GetValue(o) : p.GetValue(o, null);
                                if (value != null)
                                {
                                    level++;
                                    WriteObject(m.Name + ": ", value);
                                    level--;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void WriteValue(object o)
        {
            if (o == null)
            {
                Write("null");
            }
            else if (o is DateTime)
            {
                Write(((DateTime) o).ToShortDateString());
            }
            else if (o is ValueType || o is string)
            {
                Write(o.ToString());
            }
            else if (o is IEnumerable)
            {
                Write("...");
            }
            else
            {
                Write("{ }");
            }
        }
    }
}