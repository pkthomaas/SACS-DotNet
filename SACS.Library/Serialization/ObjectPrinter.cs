using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SACS.Library.Serialization
{
    /// <summary>
    /// The helper for displaying objects as strings
    /// </summary>
    public static class ObjectPrinter
    {
        /// <summary>
        /// The number of spaces used in indentation
        /// </summary>
        private const int IndentSpaces = 2;

        /// <summary>
        /// The string the enumerable elements start with
        /// </summary>
        private const string EnumerableElementStartWith = "- ";

        /// <summary>
        /// The symbol used for empty objects
        /// </summary>
        private const string EmptyObjectSymbol = "{}";

        /// <summary>
        /// The symbol used for null objects
        /// </summary>
        private const string NullSymbol = "null";

        /// <summary>
        /// The first line of result
        /// </summary>
        private const string FirstLine = "---";

        /// <summary>
        /// The types that are printed immediately without printing their properties
        /// </summary>
        private static readonly Type[] TypesToPrint = new[] 
        { 
            typeof(string), 
            typeof(char), 
            typeof(int), 
            typeof(bool), 
            typeof(decimal), 
            typeof(DateTime), 
            typeof(double) 
        };

        /// <summary>
        /// Creates the string that represents all properties of the object (recursively)
        /// </summary>
        /// <param name="obj">The input object.</param>
        /// <returns>The string.</returns>
        public static string CreateString(object obj)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(FirstLine);
            CreateString(obj, stringBuilder, 0, string.Empty, false);
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Creates the string from object.
        /// </summary>
        /// <param name="obj">The input object.</param>
        /// <param name="stringBuilder">The string builder.</param>
        /// <param name="indentationLevel">Indentation level.</param>
        /// <param name="startWith">The string to start the element with.</param>
        /// <param name="newline">If set to <c>true</c>, then start with new line if object has properties.</param>
        private static void CreateString(object obj, StringBuilder stringBuilder, int indentationLevel, string startWith, bool newline)
        {
            string indent = GetIndent(indentationLevel, startWith);
            if (obj == null)
            {
                stringBuilder.AppendFormat("{0}{1}{2}\n", indent, startWith, NullSymbol);
            }
            else
            {
                Type type = obj.GetType();
                var properties = type.GetProperties();
                if (properties.Count() == 0)
                {
                    stringBuilder.AppendLine(EmptyObjectSymbol);
                }
                else
                {
                    if (newline)
                    {
                        stringBuilder.AppendLine();
                    }

                    PrintProperties(obj, stringBuilder, indentationLevel, startWith, indent, properties);
                }            
            }
        }

        /// <summary>
        /// Gets the indentation string (spaces).
        /// </summary>
        /// <param name="indentationLevel">The indentation level.</param>
        /// <param name="startWith">The additional string that the line will start with.</param>
        /// <returns>The indentation string.</returns>
        private static string GetIndent(int indentationLevel, string startWith)
        {
            string indent = new string(' ', (indentationLevel * IndentSpaces) - startWith.Length);
            return indent;
        }

        /// <summary>
        /// Generates a string from properties of the object.
        /// </summary>
        /// <param name="obj">The input object.</param>
        /// <param name="stringBuilder">The string builder.</param>
        /// <param name="indentationLevel">The indentation level.</param>
        /// <param name="startWith">The additional string that the first line should start with.</param>
        /// <param name="indent">The indentation string.</param>
        /// <param name="properties">The properties of the object.</param>
        private static void PrintProperties(object obj, StringBuilder stringBuilder, int indentationLevel, string startWith, string indent, PropertyInfo[] properties)
        {
            foreach (var propertyInfo in properties)
            {
                stringBuilder.AppendFormat("{0}{1}{2}: ", indent, startWith, propertyInfo.Name);
                if (startWith != string.Empty)
                {
                    startWith = string.Empty;
                    indent = new string(' ', indentationLevel * IndentSpaces);
                }

                object value = propertyInfo.GetValue(obj);               
                Type propertyType = propertyInfo.PropertyType;
                IEnumerable enumerable = value as IEnumerable;
                if (enumerable != null && !(value is string))
                {
                    stringBuilder.AppendLine();
                    Type innerType = propertyType.IsGenericType ? propertyType.GenericTypeArguments[0] : typeof(object);
                    if (TypesToPrint.Contains(innerType))
                    {
                        foreach (var element in enumerable)
                        {
                            PrintValue(stringBuilder, element, indentationLevel + 1, EnumerableElementStartWith);
                        }
                    }
                    else
                    {
                        foreach (var element in enumerable)
                        {
                            CreateString(element, stringBuilder, indentationLevel + 1, EnumerableElementStartWith, false);
                        }
                    }
                }
                else
                {
                    Type underlyingType = propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ?
                        Nullable.GetUnderlyingType(propertyType) : null;

                    if (value == null)
                    {
                        stringBuilder.AppendLine("null");
                    }
                    else if (TypesToPrint.Contains(propertyType) || (underlyingType != null && TypesToPrint.Contains(underlyingType)))
                    {
                        PrintValue(stringBuilder, value);
                    }
                    else
                    {
                        CreateString(value, stringBuilder, indentationLevel + 1, string.Empty, true);
                    }
                }
            }
        }

        /// <summary>
        /// Generates a string from a value.
        /// </summary>
        /// <param name="stringBuilder">The string builder.</param>
        /// <param name="value">The value.</param>
        /// <param name="indentationLevel">The indentation level. If set to null, then no indentation.</param>
        /// <param name="startWith">The additional string with which the string should start.</param>
        private static void PrintValue(StringBuilder stringBuilder, object value, int? indentationLevel = null, string startWith = "")
        {
            if (indentationLevel.HasValue)
            {
                stringBuilder.Append(GetIndent(indentationLevel.Value, startWith));
                stringBuilder.Append(startWith);
            }

            if (value is string)
            {
                stringBuilder.AppendFormat("\"{0}\"", value);
            }
            else if (value is char)
            {
                stringBuilder.AppendFormat("\'{0}\'", value);
            }
            else
            {
                stringBuilder.Append(value.ToString());
            }

            stringBuilder.AppendLine();
        }
    }
}
