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
    /// An entity within the edoo.sys database table "asv.svp_schule_stamm"
    /// </summary>
    public class Tenant
    {
        public string Code { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string SchoolNo { get; set; }

        public static Tenant FromDb(DbDataReader reader)
        {
            return new Tenant
            {
                Id = reader.GetValue<string>("id"),
                Code = reader.GetValue<string>("kurzname"),
                SchoolNo = reader.GetValue<string>("schulnummer"),
                Name = reader.GetValue<string>("dienststellenname")
            };
        }
    }
}
