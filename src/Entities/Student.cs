#region ENBREA - Copyright (C) 2021 STÜBER SYSTEMS GmbH
/*    
 *    ENBREA
 *    
 *    Copyright (C) 2021 STÜBER SYSTEMS GmbH
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
    /// An entity within the edoo.sys database table "asv.svp_schueler_stamm" (+joined tables)
    /// </summary>
    public class Student
    {
        public DateTime? Birthdate { get; set; }
        public DateTime? EnrollmentDate { get; set; }
        public string Firstname { get; set; }
        public Gender? Gender { get; set; }
        public string Id { get; set; }
        public string Lastname { get; set; }
        public DateTime? LeaveDate { get; set; }
        public string NamePostfix { get; set; }
        public string NamePrefix { get; set; }
        public string Nickname { get; set; }

        public static Student FromDb(DbDataReader reader)
        {
            return new Student
            {
                Id = reader.GetValue<string>("id"),
                Lastname = reader.GetValue<string>("familienname"),
                Firstname = reader.GetValue<string>("vornamen"),
                Birthdate = reader.GetValue<DateTime?>("geburtsdatum"),
                Gender = reader.GetGenderValue("geschlecht"),
                Nickname = reader.GetValue<string>("rufname"),
                NamePrefix = reader.GetValue<string>("namensbestandteil_vorangest"),
                NamePostfix = reader.GetValue<string>("namensbestandteil_nachgest"),
                EnrollmentDate = reader.GetValue<DateTime?>("eintrittsdatum"),
                LeaveDate = reader.GetValue<DateTime?>("austrittsdatum")
            };
        }
    }
}
