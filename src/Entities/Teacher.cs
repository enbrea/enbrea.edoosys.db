﻿#region Enbrea - Copyright (C) STÜBER SYSTEMS GmbH
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

namespace Enbrea.Edoosys.Db
{
    /// <summary>
    /// An entity within the edoo.sys database table "asv.svp_lehrer_stamm" (+joined tables)
    /// </summary>
    public class Teacher
    {
        public DateTime? Birthdate { get; set; }
        public string Code { get; set; }
        public string Firstname { get; set; }
        public Gender? Gender { get; set; }
        public string Id { get; set; }
        public string Lastname { get; set; }
        public DateTime? LeaveDate { get; set; }
        public string NamePostfix { get; set; }
        public string NamePrefix { get; set; }
        public string Nickname { get; set; }
        public DateTime? StartDate { get; set; }
        public string TeacherType { get; set; }

        public static Teacher FromDb(DbDataReader reader)
        {
            return new Teacher
            {
                Id = reader.GetValue<string>("id"),
                Code = reader.GetValue<string>("namenskuerzel"),
                Lastname = reader.GetValue<string>("familienname"),
                Firstname = reader.GetValue<string>("vornamen"),
                Birthdate = reader.GetValue<DateTime?>("geburtsdatum"),
                Nickname = reader.GetValue<string>("rufname"),
                NamePrefix = reader.GetValue<string>("namensbestandteil_vorangest"),
                NamePostfix = reader.GetValue<string>("namensbestandteil_nachgest"),
                Gender = reader.GetGenderValue("geschlecht"),
                TeacherType = reader.GetValue<string>("lehrerart"),
                StartDate = reader.GetValue<DateTime?>("zugang_datum"),
                LeaveDate = reader.GetValue<DateTime?>("abgang_datum")
            };
        }
    }
}
