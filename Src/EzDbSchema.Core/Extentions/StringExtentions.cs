﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("EzDbSchema.MsSql")]

//Thanks https://www.codeproject.com/tips/1081932/tosingular-toplural-string-extensions
namespace EzDbSchema.Core.Extentions.Strings
{
    internal static class StringExtensions
    {
        private static Dictionary<string, string> sQLDataTypeToDotNetDataType = new Dictionary<string, string>();
        private static Dictionary<string, string> sQLDataTypeToJsDataType = new Dictionary<string, string>();

        static StringExtensions()
        {
            if (sQLDataTypeToJsDataType.Count == 0)
            {
                sQLDataTypeToJsDataType.Add("bigint", "number");
                sQLDataTypeToJsDataType.Add("binary", "object");
                sQLDataTypeToJsDataType.Add("bit", "boolean");
                sQLDataTypeToJsDataType.Add("char", "string");
                sQLDataTypeToJsDataType.Add("date", "Date");
                sQLDataTypeToJsDataType.Add("datetime", "Date");
                sQLDataTypeToJsDataType.Add("datetime2", "Date");
                sQLDataTypeToJsDataType.Add("datetimeoffset", "Date");
                sQLDataTypeToJsDataType.Add("decimal", "number");
                sQLDataTypeToJsDataType.Add("varbinary", "object");
                sQLDataTypeToJsDataType.Add("float", "number");
                sQLDataTypeToJsDataType.Add("image", "object");
                sQLDataTypeToJsDataType.Add("int", "number");
                sQLDataTypeToJsDataType.Add("money", "number");
                sQLDataTypeToJsDataType.Add("nchar", "string");
                sQLDataTypeToJsDataType.Add("ntext", "string");
                sQLDataTypeToJsDataType.Add("numeric", "number");
                sQLDataTypeToJsDataType.Add("nvarchar", "string");
                sQLDataTypeToJsDataType.Add("real", "number");
                sQLDataTypeToJsDataType.Add("rowversion", "object");
                sQLDataTypeToJsDataType.Add("smalldatetime", "Date");
                sQLDataTypeToJsDataType.Add("smallint", "number");
                sQLDataTypeToJsDataType.Add("smallmoney", "number");
                sQLDataTypeToJsDataType.Add("sql_variant", "object");
                sQLDataTypeToJsDataType.Add("text", "string");
                sQLDataTypeToJsDataType.Add("time", "Date");
                sQLDataTypeToJsDataType.Add("timestamp", "Date");
                sQLDataTypeToJsDataType.Add("tinyint", "number");
                sQLDataTypeToJsDataType.Add("uniqueidentifier", "string");
                sQLDataTypeToJsDataType.Add("varchar", "string");
                sQLDataTypeToJsDataType.Add("varchar(max)", "string");
                sQLDataTypeToJsDataType.Add("nvarchar(max)", "string");
                sQLDataTypeToJsDataType.Add("varbinary(max)", "object");
                sQLDataTypeToJsDataType.Add("xml", "string");
                sQLDataTypeToJsDataType.Add("geometry", "object");
                sQLDataTypeToJsDataType.Add("geography", "object");
            }
            if (sQLDataTypeToDotNetDataType.Count == 0)
            {
                sQLDataTypeToDotNetDataType.Add("bigint", "System.Int64");
                sQLDataTypeToDotNetDataType.Add("binary", "Byte[]");
                sQLDataTypeToDotNetDataType.Add("bit", "bool");
                sQLDataTypeToDotNetDataType.Add("char", "string");
                sQLDataTypeToDotNetDataType.Add("date", "DateTime");
                sQLDataTypeToDotNetDataType.Add("datetime", "DateTime");
                sQLDataTypeToDotNetDataType.Add("datetime2", "DateTime");
                sQLDataTypeToDotNetDataType.Add("datetimeoffset", "DateTimeOffset");
                sQLDataTypeToDotNetDataType.Add("decimal", "decimal");
                sQLDataTypeToDotNetDataType.Add("varbinary", "Byte[]");
                sQLDataTypeToDotNetDataType.Add("float", "double");
                sQLDataTypeToDotNetDataType.Add("image", "Byte[]");
                sQLDataTypeToDotNetDataType.Add("int", "int");
                sQLDataTypeToDotNetDataType.Add("money", "decimal");
                sQLDataTypeToDotNetDataType.Add("nchar", "string");
                sQLDataTypeToDotNetDataType.Add("ntext", "string");
                sQLDataTypeToDotNetDataType.Add("numeric", "decimal");
                sQLDataTypeToDotNetDataType.Add("nvarchar", "string");
                sQLDataTypeToDotNetDataType.Add("real", "double");
                sQLDataTypeToDotNetDataType.Add("rowversion", "Byte[]");
                sQLDataTypeToDotNetDataType.Add("smalldatetime", "DateTime");
                sQLDataTypeToDotNetDataType.Add("smallint", "short");
                sQLDataTypeToDotNetDataType.Add("smallmoney", "decimal");
                sQLDataTypeToDotNetDataType.Add("sql_variant", "object");
                sQLDataTypeToDotNetDataType.Add("text", "string");
                sQLDataTypeToDotNetDataType.Add("time", "TimeSpan");
                sQLDataTypeToDotNetDataType.Add("timestamp", "Byte[]");
                sQLDataTypeToDotNetDataType.Add("tinyint", "Byte");
                sQLDataTypeToDotNetDataType.Add("uniqueidentifier", "Guid");
                sQLDataTypeToDotNetDataType.Add("varchar", "string");
                sQLDataTypeToDotNetDataType.Add("varchar(max)", "string");
                sQLDataTypeToDotNetDataType.Add("nvarchar(max)", "string");
                sQLDataTypeToDotNetDataType.Add("varbinary(max)", "Byte[]");
                sQLDataTypeToDotNetDataType.Add("xml", "Xml");
                sQLDataTypeToDotNetDataType.Add("geometry", "DbGeometry");
                sQLDataTypeToDotNetDataType.Add("geography", "DbGeography");
            }

        }

        /// <summary>
        /// Truncates string so that it is no longer than the specified number of characters.
        /// </summary>
        /// <param name="str">String to truncate.</param>
        /// <param name="length">Maximum string length.</param>
        /// <returns>Original string or a truncated one if the original was too long.</returns>
        internal static string Truncate(this string str, int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length", "Length must be >= 0");
            }

            if (str == null)
            {
                return null;
            }

            int maxLength = Math.Min(str.Length, length);
            return str.Substring(0, maxLength);
        }

        internal static string ToNetType(this string sqlType)
        {
            if (sqlType == null)
                throw new ArgumentNullException("sqlType");
            if (sqlType.Contains("varchar")) return "string";
            return (sQLDataTypeToDotNetDataType.ContainsKey(sqlType) ? sQLDataTypeToDotNetDataType[sqlType] : sqlType);
        }

        internal static string ToNetType(this string sqlType, bool isNullable)
        {
            var ret = "";
            if (sqlType == null)
                throw new ArgumentNullException("sqlType");
            if (sqlType.Contains("varchar"))
                ret = "string";
            else
                ret = (sQLDataTypeToDotNetDataType.ContainsKey(sqlType) ? sQLDataTypeToDotNetDataType[sqlType] : sqlType);
            return ret + ((isNullable && !(ret.Equals("string") || ret.Equals("DbGeometry") || ret.Equals("DbGeography") || ret.EndsWith("[]"))) ? "?" : "");
        }


        internal static string ToJsType(this string sqlType)
        {
            if (sqlType == null)
                throw new ArgumentNullException("sqlType");
            if (sqlType.Contains("varchar")) return "string";
            return (sQLDataTypeToJsDataType.ContainsKey(sqlType) ? sQLDataTypeToJsDataType[sqlType] : "object");
        }

        internal static string ToJsType(this string sqlType, bool isNullable)
        {
            var ret = "object";
            if (sqlType == null)
                throw new ArgumentNullException("sqlType");
            if (sqlType.Contains("varchar"))
                ret = "string";
            else
                ret = (sQLDataTypeToJsDataType.ContainsKey(sqlType) ? sQLDataTypeToJsDataType[sqlType] : sqlType);
            return ret + (isNullable ? " | null" : "");
        }

        internal static string AsFormattedName(this string word)
        {
            if (word == null)
                return null;

            if (word.EndsWith("ID")) word = word.Substring(0, word.Length - 2);
            if (word.EndsWith("UID")) word = word.Substring(0, word.Length - 3);
            if (word.EndsWith("Id")) word = word.Substring(0, word.Length - 2);
            return word;

        }

        internal static string ToSnakeCase(this string str)
        {
            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "-" + x.ToString() : x.ToString())).ToLower();
        }


        /// <summary>
        /// Resolve the path variables
        /// </summary>
        /// <param name="PathToResolve"></param>
        /// <returns></returns>
        internal static string ResolvePathVars(this string PathToResolve)
        {
            return PathToResolve.ResolvePathVars("ez-db-schema-core");
        }
        /// <summary>
        /// Resolve the pah vatraibles
        /// </summary>
        /// <param name="PathToResolve"></param>
        /// <returns></returns>
        internal static string ResolvePathVars(this string PathToResolve, string rootFolderName)
        {
            if ((PathToResolve.Contains("{SOLUTION_PATH}")) || (PathToResolve.Contains("{ASSEMBLY_PATH}")))
            {
                var AssemblyPath = AppContext.BaseDirectory;
                AssemblyPath += (AssemblyPath.EndsWith(Path.DirectorySeparatorChar.ToString()) ? "" : Path.DirectorySeparatorChar.ToString());
                var SolutionPath = AssemblyPath;

                //File.AppendAllText(@"C:\Temp\DEBUG.txt", string.Format("   System.Diagnostics.Debugger.IsAttached={0}" + "\n", System.Diagnostics.Debugger.IsAttached));
                DirectoryInfo di = new DirectoryInfo(SolutionPath);
                while (di != null)
                {
                    if (di.Name == rootFolderName)
                    {
                        SolutionPath = di.FullName + Path.DirectorySeparatorChar.ToString();
                        break;
                    }
                    di = di.Parent;
                }
                PathToResolve = PathToResolve.Replace("{SOLUTION_PATH}", SolutionPath).Replace("{ASSEMBLY_PATH}", AssemblyPath);
            }

            return PathToResolve;
        }

        /// <summary>
        /// Pluck the specified string between the last instance of a character and the end of the string.
        /// </summary>
        /// <returns>The plucked String</returns>
        /// <param name="str">String.</param>
        /// <param name="leftString">Left string.</param>
        internal static string Pluck(this string str, string leftString)
        {
            return (str + "$#@").Pluck(leftString, "$#@");
        }

        /// <summary>
        /// Pluck the specified string between two strings.
        /// </summary>
        /// <returns>The plucked String</returns>
        /// <param name="str">String.</param>
        /// <param name="leftString">Left string.</param>
        /// <param name="rightString">Right string.</param>
        internal static string Pluck(this string str, string leftString, string rightString)
        {
            try
            {
                var rpos = str.IndexOf(rightString);
                if (rpos > 0)
                {
                    var lpos = str.LastIndexOf(leftString, rpos);
                    if ((lpos > 0) && (rpos > lpos))
                    {
                        return str.Substring(lpos + leftString.Length, (rpos - lpos) - leftString.Length);
                    }
                }
            }
            catch (Exception)
            {
                return "";
            }
            return "";
        }

        /// <summary>
        /// Pluck a string from str. The rest of the string (inclusive of delimiters) will be passed out through RemainingString
        /// </summary>
        /// <returns>The plucked string</returns>
        /// <param name="str">String to search for</param>
        /// <param name="leftString">Left string.</param>
        /// <param name="rightString">Right string.</param>
        /// <param name="RemainingString">Remaining string with the plucked string removed</param>
        internal static string Pluck(this string str, string leftString, string rightString, out string RemainingString)
        {
            RemainingString = str;
            try
            {
                var lpos = -1;
                //Find the string we are interested in first
                lpos = str.IndexOf(leftString);
                //return blank if it doesn't exist
                if (lpos < 0) return "";
                //but if we find it, start to search for the terminating string
                var rpos = str.IndexOf(rightString, lpos);
                if (rpos > 0)
                {
                    //lpos = str.LastIndexOf(leftString, rpos);
                    if ((lpos > -1) && (rpos > lpos))
                    {
                        var ret = str.Substring(lpos + leftString.Length, (rpos - lpos) - leftString.Length);
                        RemainingString = str.Substring(0, lpos + leftString.Length);
                        RemainingString += str.Substring(rpos);
                        return ret;
                    }
                }
            }
            catch (Exception)
            {
                return "";
            }
            return "";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        internal static string Unquote(this string str)
        {
            var ret = str.Trim();
            if (ret.ToUpper().Equals("NULL")) return null;
            if ((str.StartsWith("\"")) && (str.EndsWith("\"")))
            {
                ret = ret.Replace("\"\"", "\"").Substring(2, ret.Length - 3);
                if (ret.Equals("\"")) ret = "";
            }
            return ret;
        }
        /// <summary>
        /// Should capitalize the first letter of each word. Acronyms will stay uppercased.
        /// Anything after a non letter or number will keep be capitalized. 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        internal static string ToTitleCase(this string str)
        {
            var tokens = str.Split(new[] { " " }, StringSplitOptions.None);
            var stringBuilder = new StringBuilder();
            for (var ti = 0; ti < tokens.Length; ti++)
            {
                var token = tokens[ti];
                if (token == token.ToUpper())
                    stringBuilder.Append(token + " ");
                else
                {
                    var previousWasSeperator = false;
                    var previousWasNumber = false;
                    var ignoreNumber = false;
                    for (var i = 0; i < token.Length; i++)
                    {

                        if (char.IsNumber(token[i]))
                        {
                            stringBuilder.Append(token[i]);
                            previousWasNumber = true;
                        }
                        else if (!char.IsLetter(token[i]))
                        {
                            stringBuilder.Append(token[i]);
                            previousWasSeperator = true;
                        }
                        else if ((previousWasNumber && !ignoreNumber) || previousWasSeperator)
                        {
                            stringBuilder.Append(char.ToUpper(token[i]));
                            previousWasSeperator = false;
                            previousWasNumber = false;
                        }
                        else if (i == 0)
                        {
                            ignoreNumber = true;
                            stringBuilder.Append(char.ToUpper(token[i]));
                        }
                        else
                        {
                            ignoreNumber = true;
                            stringBuilder.Append(char.ToLower(token[i]));
                        }
                    }
                    stringBuilder.Append(" ");
                }
            }
            return stringBuilder.ToString().TrimEnd();
        }

        /// <summary>
        /// This algorithim will take all numbers that prefix the string and suffix them.
        /// it will also strip all  
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        internal static string ToCsObjectName(this string str)
        {
            var strAsCharArray = str.ToCharArray();
            var invalidPrefixNumbers = "";
            var charsOtherThanNumbers = 0;
            var stringBuilder = new StringBuilder();
            var previousWasSeperator = false;
            var previousWasNumber = false;
            var ignoreNumber = false;
            for (var i = 0; i < strAsCharArray.Length; i++)
            {
                charsOtherThanNumbers++;

                if (char.IsNumber(strAsCharArray[i]))
                {
                    charsOtherThanNumbers--;
                    if (charsOtherThanNumbers == 0)
                    {
                        invalidPrefixNumbers += strAsCharArray[i];
                    }
                    else
                    {
                        stringBuilder.Append(strAsCharArray[i]);
                    }
                    previousWasNumber = true;
                }
                else if (strAsCharArray[i] == '-')
                {
                    stringBuilder.Append('_');
                    previousWasSeperator = true;
                }
                else if (!char.IsLetter(strAsCharArray[i]))
                {
                    //stringBuilder.Append(token[i]);
                    previousWasSeperator = true;
                }
                else if ((previousWasNumber && !ignoreNumber) || previousWasSeperator)
                {
                    stringBuilder.Append(char.ToUpper(strAsCharArray[i]));
                    previousWasSeperator = false;
                    previousWasNumber = false;
                }
                else
                {
                    ignoreNumber = true;
                    stringBuilder.Append(strAsCharArray[i]);
                }
            }
            stringBuilder.Append(" ");
            return (stringBuilder.ToString() + invalidPrefixNumbers).TrimEnd().Replace(" ", "");
        }
        internal static string CHAR_ENCODE_PREFIX = "%";
        internal static string CHAR_ENCODE_REPLACE = "=-";
        internal static string ToSafeFileName(this string fileName)
        {
            return fileName.ToSafeFileName(false);
        }
        internal static string ToSafeFileName(this string fileName, bool PreserveSpaces)
        {
            return WebUtility.UrlEncode(fileName.Replace(" ", (PreserveSpaces ? "%20" : ""))).Replace(StringExtensions.CHAR_ENCODE_PREFIX, StringExtensions.CHAR_ENCODE_REPLACE);
        }
        internal static string FromSafeFileName(this string fileName)
        {
            return WebUtility.UrlDecode(fileName.Replace(StringExtensions.CHAR_ENCODE_REPLACE, StringExtensions.CHAR_ENCODE_PREFIX).Replace("%20", " "));
        }

        /// <summary>
        /// Changes the name of the field based on rules
        /// </summary>
        /// <param name="fieldName">Field Name</param>
        /// <param name="stringsToReplace">A string array.  but an item in the array can contain a field with an equal... ie:
        /// stringsToReplace[0] = "uuu_user_"       // will replace all instances of uuu_user_ with zero length strings (deleting them)
        /// stringsToReplace[1] = "username=login"  // will replace all instances of username with login
        /// stringsToReplace[2] = "@@LOWER"         // will convert string to lower case
        /// stringsToReplace[3] = "@@UPPER"         // will convert string to upper case
        /// </param>
        /// <returns></returns>
        internal static string AsOraFieldName(this string fieldName, params string[] stringsToReplace)
        {
            foreach (var str in stringsToReplace)
            {
                if (str == "@@LOWER")
                {
                    fieldName = fieldName.ToLower();
                }
                else
                if (str == "@@UPPER")
                {
                    fieldName = fieldName.ToUpper();
                }
                else
                {
                    if (str.Contains("="))
                    {
                        var arr = str.Split('=');
                        if (arr.Count() > 1)
                        {
                            fieldName = fieldName.Replace(arr[0], arr[1]);
                        }
                    }
                    else
                    {
                        fieldName = fieldName.Replace(str, "");
                    }
                }
            }
            return fieldName;
        }

        /// <summary>A fully managed version of the 64bit GetHashCode() that does not use any randomization and will return the same value for all future versions of .NET (as long as the behavior of int ^ char never changes).
        /// Much thanks to https://stackoverflow.com/questions/36845430/persistent-hashcode-for-strings
        /// </summary>
        /// <returns>The stable hash code.</returns>
        /// <param name="str">String.</param>
        internal static int GetStableHashCode(this string str)
        {
            unchecked
            {
                int hash1 = 5381;
                int hash2 = hash1;

                for (int i = 0; i < str.Length && str[i] != '\0'; i += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ str[i];
                    if (i == str.Length - 1 || str[i + 1] == '\0')
                        break;
                    hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
                }

                return hash1 + (hash2 * 1566083941);
            }
        }

        internal static bool IsEqualToFileContents(this string str, string FileToCompareTo)
        {
            try
            {
                if (!File.Exists(FileToCompareTo)) return false;
                string readText = File.ReadAllText(FileToCompareTo);
                return (readText.Equals(str, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal static StringBuilder PrependComma(this StringBuilder sb)
        {
            if ( !((sb.ToString().Trim().EndsWith("{")) || (sb.ToString().Trim().EndsWith("[")))) sb.Append(", ");
            return sb;
        }
        internal static StringBuilder AppendJson(this StringBuilder sb, string name, string value)
        {
            return sb.PrependComma().AppendFormat("\"{0}\": \"{1}\"", name, value);
        }
        internal static StringBuilder AppendJson(this StringBuilder sb, string name, int value)
        {
            return sb.PrependComma().AppendFormat("\"{0}\": {1}", name, value);
        }
        internal static StringBuilder AppendJson(this StringBuilder sb, string name, decimal value)
        {
            return sb.PrependComma().AppendFormat("\"{0}\": {1}", name, value);
        }
        internal static StringBuilder AppendJson(this StringBuilder sb, string name, DateTime value)
        {
            return sb.PrependComma().AppendFormat("\"{0}\": \"{1}\"", name, value.ToString("o"));
        }
        internal static StringBuilder AppendJson(this StringBuilder sb, string name, bool value)
        {
            return sb.PrependComma().AppendFormat("\"{0}\": {1}", name, (value ? "true" : "false"));
        }
        internal static StringBuilder AppendJson(this StringBuilder sb, string name, object value)
        {
            if (value.GetType().Equals(typeof(decimal)))
                return sb.AppendJson(name, value.ToString());
            if (value.GetType().Equals(typeof(DateTime)))
                return sb.AppendJson(name, DateTime.Parse(value.ToString()));
            if (value.GetType().Equals(typeof(bool)))
                return sb.AppendJson(name, (bool)value);
            else if (value.GetType().Equals(typeof(int)))
                return sb.AppendJson(name, int.Parse(value.ToString()));
            else
                return sb.AppendJson(name, value.ToString());
        }

    }
}