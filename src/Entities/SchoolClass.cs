#region ENBREA - Copyright (C) 2022 STÜBER SYSTEMS GmbH
/*    
 *    ENBREA
 *    
 *    Copyright (C) 2022 STÜBER SYSTEMS GmbH
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
    /// An entity within the edoo.sys database table "asv.svp_klassengruppe" (+joined tables)
    /// </summary>
    public class SchoolClass
    {
        public string Code { get; set; }
        public string Id { get; set; }
        public string RootId { get; set; }
        public string RootCode { get; set; }
        public string RootName { get; set; }
        public string RootStatisticalCode { get; set; }

        public static SchoolClass FromDb(DbDataReader reader)
        {
            return new SchoolClass
            {
                Id = reader.GetValue<string>("klassengruppe_id"),
                Code = reader.GetValue<string>("klassengruppe_kennung"),
                RootId = reader.GetValue<string>("klasse_id"),
                RootCode = reader.GetValue<string>("klassenname"),
                RootStatisticalCode = reader.GetValue<string>("klassenname_statistik"),
                RootName = reader.GetValue<string>("klassenname_lang")
            };
        }
    }
}
