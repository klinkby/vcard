using System.Collections;
using System.Reflection;

namespace Klinkby.VCard;

/// <summary>
///     Generic serialization of object properties
/// </summary>
public abstract class VSerializable
{
    /// <summary>
    ///     Default constructor
    /// </summary>
    protected VSerializable()
    {
    }

    /// <summary>
    ///     Serialize object properties
    /// </summary>
    /// <returns>string</returns>
    public override string ToString()
    {
        var t = GetType();
        IEnumerable<PropertyInfo> pi = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        var members = pi.Aggregate(
            string.Empty,
            (s, p) =>
            {
                var val = p.GetValue(this, null);
                if (val == null || (val is string stringVal && string.IsNullOrEmpty(stringVal))) return s;
                var ga = p.PropertyType.GetGenericArguments();
                var traverse = val is IEnumerable && ga.Length == 1 &&
                               ga[0].IsSubclassOf(typeof(VSerializable));
                s += traverse
                    ? ((IEnumerable)val).Cast<VSerializable>()
                    .Aggregate(string.Empty, (u, v) => u + v)
                    : val is VSerializable
                        ? val.ToString()
                        : Line(p.Name, val.ToString());

                return s;
            }
        );
        return Line("BEGIN", t.Name.ToUpperInvariant())
               + members
               + Line("END", t.Name.ToUpperInvariant());
    }

    private static string Line(string key, string value) => 
        key.ToUpperInvariant() + ":" + value.Replace(Environment.NewLine, "\\n") + Environment.NewLine;
}