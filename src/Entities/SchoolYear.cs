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

namespace Enbrea.Edoosys.Db
{
    /// <summary>
    /// An entity within the edoo.sys database table "asv.svp_wl_schuljahr"
    /// </summary>
    public class SchoolYear
    {
        public string Code { get; set; }
        public string DisplayName { get; set; }
        public DateTime EndDate { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }

        public static SchoolYear FromDb(DbDataReader reader)
        {
            return new SchoolYear
            {
                Id = reader.GetValue<string>("id"),
                Code = reader.GetValue<string>("kurzform"),
                Name = reader.GetValue<string>("langform"),
                DisplayName = reader.GetValue<string>("anzeigeform"),
                StartDate = reader.GetValue<DateTime>("beginn"),
                EndDate = reader.GetValue<DateTime>("ende")
            };
        }
    }
}
