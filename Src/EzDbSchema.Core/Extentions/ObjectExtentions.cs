﻿using EzDbSchema.Core.Interfaces;
using EzDbSchema.Core.Objects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("EzDbSchema.MsSql")]

namespace EzDbSchema.Core.Extentions.Objects
{
    internal static class ObjectExtensions
    {

        internal static Dictionary<int, object> RefObjectXref = new Dictionary<int, object>();
        internal static List<IEzObject> DelayedRefResolutionList = new List<IEzObject>();
        //this dictionary should use weak key references
        static Dictionary<object, int> d = new Dictionary<object, int>();
        static int gid = 0;
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        internal static int GetId(this object o)
        {
            if (d.ContainsKey(o)) return d[o];
            return d[o] = gid++;
        }

        /// <summary>
        /// Gets an identifier for an object
        /// </summary>
        /// <param name="o">EZObject- if the object already exists,  use its ide</param>
        /// <param name="valToCheck">Value to check.  If it is greater than 0,  then return it</param>
        /// <returns></returns>
        internal static int GetId(this IEzObject o, ref int valToCheck)
        {
            if (d.ContainsKey(o)) return d[o];
            if (valToCheck > 0) return valToCheck;
            return d[o] = gid++;
        }

        /// <summary>
        /// Gets an identifier for an object
        /// </summary>
        /// <param name="o">EZObject- if the object already exists,  use its ide</param>
        /// <param name="valToCheck">Value to check.  If it is greater than 0,  then return it</param>
        /// <returns></returns>
        internal static int SetRefId(this IEzObject o)
        {
            if (!RefObjectXref.ContainsKey(o._id)) RefObjectXref.Add(o._id, o);
            if (!d.ContainsKey(o)) d.Add(o, o._id);
            return o._id;
            
        }

        internal static void ClearRef()
        {
            RefObjectXref.Clear();
            DelayedRefResolutionList.Clear();
            d.Clear();
        }
        /// <summary>
        /// Will search an object array and safely return a string.  If the item doesn't exist, this will return 
        /// </summary>
        /// <returns>The string.</returns>
        /// <param name="objectArray">Object array that has the item to return</param>
        /// <param name="index">IIndex to return</param>
        internal static string AsString(this object[] objectArray, int index)
        {
            if (objectArray.Count() >= index)
            {
                if (objectArray[index] != null)
                {
                    return objectArray[index].ToString();
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// This will return a string, but if the variable is null, it will return a zero length string.
        /// </summary>
        /// <returns>The safe string.</returns>
        /// <param name="word">Word.</param>
        internal static string ToSafeString(this object word)
        {
            return (word == null) ? "" : word.ToString();
        }
        /// <summary>
        /// Ases the nullable boolean.
        /// </summary>
        /// <returns>The nullable boolean.</returns>
        /// <param name="obj">Object.</param>
        internal static bool? AsNullableBoolean(this object obj)
        {
            bool? ret = null;
            if (obj != null)
            {
                var objAsStr = obj.ToString();
                ret = (objAsStr == "1") || (objAsStr.ToUpper().StartsWith("T")) || (objAsStr.ToUpper().StartsWith("Y"));
            }
            return ret;
        }
        /// <summary>
        /// Ases the boolean.
        /// </summary>
        /// <returns><c>true</c>, if boolean was ased, <c>false</c> otherwise.</returns>
        /// <param name="obj">Object.</param>
        internal static bool AsBoolean(this object obj)
        {
            bool ret = false;
            if (obj != null)
            {
                var objAsStr = obj.ToString();
                ret = (objAsStr == "1") || (objAsStr.ToUpper().StartsWith("T")) || (objAsStr.ToUpper().StartsWith("Y"));
            }
            return ret;
        }
        /// <summary>
        /// Returns an object as an Int
        /// </summary>
        /// <returns>The int.</returns>
        /// <param name="obj">Object.</param>
        /// <param name="ValueIfNull">Value if null.</param>
        internal static int AsInt(this object obj, int ValueIfNull)
        {
            int ret = ValueIfNull;
            if (obj != null)
            {
                if (obj.ToString().Contains("."))
                {
                    Double temp;
                    Boolean isOk = Double.TryParse(obj.ToString(), out temp);
                    ret = isOk ? (int)temp : 0;
                }
                else
                {
                    int.TryParse(obj.ToString(), out ret);
                }
            }
            return ret;
        }
        /// <summary>
        /// Ases the int nullable.
        /// </summary>
        /// <returns>The int nullable.</returns>
        /// <param name="obj">Object.</param>
        /// <param name="ValueIfNull">Value if null.</param>
        internal static int? AsIntNullable(this object obj, int? ValueIfNull)
        {
            int? ret = ValueIfNull;
            if (obj != null)
            {
                if (obj.ToString().Contains("."))
                {
                    Double temp;
                    Boolean isOk = Double.TryParse(obj.ToString(), out temp);
                    ret = isOk ? (int?)temp : 0;
                }
                else
                {
                    int i = 0;
                    int.TryParse(obj.ToString(), out i);
                    ret = (int?)i;
                }
            }
            return ret;
        }
        /// <summary>
        /// Will return the variable as a DateTime
        /// </summary>
        /// <returns>The date time.</returns>
        /// <param name="obj">Object.</param>
        /// <param name="ValueIfNull">Value if null.</param>
        internal static DateTime AsDateTime(this object obj, DateTime ValueIfNull)
        {
            DateTime ret = ValueIfNull;
            if (obj != null)
            {
                DateTime temp;
                Boolean isOk = DateTime.TryParse(obj.ToString(), out temp);
                ret = isOk ? temp : ValueIfNull;

            }
            return ret;
        }

        /// <summary>
        /// Return the object as a AsDateTimeNullable
        /// </summary>
        /// <returns>The date time nullable.</returns>
        /// <param name="obj">Object.</param>
        /// <param name="ValueIfNull">Value if null.</param>
        internal static DateTime? AsDateTimeNullable(this object obj, DateTime? ValueIfNull)
        {
            DateTime? ret = ValueIfNull;
            if (obj != null)
            {
                DateTime temp;
                Boolean isOk = DateTime.TryParse(obj.ToString(), out temp);
                ret = isOk ? temp : ValueIfNull;

            }
            return ret;
        }
        /// <summary>
        /// This will return the variable as a Double 
        /// </summary>
        /// <returns>The double.</returns>
        /// <param name="obj">Object.</param>
        /// <param name="ValueIfNull">Value if null.</param>
        internal static double AsDouble(this object obj, double ValueIfNull)
        {
            double ret = ValueIfNull;
            if (obj != null)
            {
                Double temp;
                Boolean isOk = Double.TryParse(obj.ToString(), out temp);
                ret = isOk ? (double)temp : 0;
            }
            return ret;
        }
    }
}