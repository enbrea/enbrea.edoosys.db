#region ENBREA - Copyright (C) 2020 STÜBER SYSTEMS GmbH
/*    
 *    ENBREA
 *    
 *    Copyright (C) 2020 STÜBER SYSTEMS GmbH
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

using System.Data.Common;

namespace Enbrea.Edoosys.Db
{
    /// <summary>
    /// An entity within the edoo.sys database table "asv.svp_schueler_schuljahr"
    /// </summary>
    public class StudentSchoolClassAttendance
    {
        public string SchoolClassId { get; set; }
        public string SchoolClassRootId { get; set; }
        public string StudentId { get; set; }

        public static StudentSchoolClassAttendance FromDb(DbDataReader reader)
        {
            return new StudentSchoolClassAttendance
            {
                SchoolClassId = reader.GetValue<string>("klassengruppe_id"),
                SchoolClassRootId = reader.GetValue<string>("klasse_id"),
                StudentId = reader.GetValue<string>("schueler_id"),
            };
        }
    }
}
