#region Enbrea - Copyright (C) STÜBER SYSTEMS GmbH
/*    
 *    Enbrea
 *    
 *    Copyright (C) STÜBER SYSTEMS GmbH
 *
 *    This program is free software: you can redistribute it and/or modify
 *    it under the terms of the GNU Affero General Public License, version 3,
 *    as published by the Free Software Foundation.
 *
 *    This program is distributed in the hope that it will be useful,
 *    but WITHOUT ANY WARRANTY; without even the implied warranty of
 *    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 *    GNU Affero General Public License for more details.
 *
 *    You should have received a copy of the GNU Affero General Public License
 *    along with this program. If not, see <http://www.gnu.org/licenses/>.
 *
 */
#endregion

using System;
using System.Data.Common;

namespace Enbrea.Edoosys.Db;

/// <summary>
/// Extensions for <see cref="DbDataReader"/>
/// </summary>
public static class DbDataReaderExtensions
{
    /// <summary>
    /// Gets the value of a column as <see cref="Gender"/> from a <see cref="DbDataReader"/>
    /// </summary>
    /// <param name="dbDataReader">The data reader</param>
    /// <param name="name">The name of the column</param>
    /// <returns>The value of the column as <see cref="Gender"/></returns>
    public static Gender? GetGenderValue(this DbDataReader dbDataReader, string name)
    {
        var value = GetValue<string>(dbDataReader, name);
        
        if (value != null)
        {
            return value switch
            {
                "1" => Gender.Male,
                "2" => Gender.Female,
                "3" => Gender.Diverse,
                _ =>   Gender.Unknown
            };
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Gets the value of a column from a <see cref="DbDataReader"/>
    /// </summary>
    /// <typeparam name="TValue">The type of the value to get</typeparam>
    /// <param name="dbDataReader">The data reader</param>
    /// <param name="name">The name of the column</param>
    /// <returns>The value of the column</returns>
    public static TValue GetValue<TValue>(this DbDataReader dbDataReader, string name)
    {
        var value = dbDataReader[name];

        if (value != null)

        {
            var t = value.GetType();

            if (t == typeof(DBNull))
            {
                return default;
            }
            else
            {
                var convertionType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
                return (TValue)Convert.ChangeType(value, convertionType);
            }
        }
        else
        {
            return default;
        }
    }

}