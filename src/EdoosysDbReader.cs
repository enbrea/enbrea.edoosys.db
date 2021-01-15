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

using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Enbrea.Edoosys.Db
{
    public class EdoosysDbReader
    {
        private readonly string _dbConnectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="EdoosysDbReader<T>"/> class.
        /// </summary>
        public EdoosysDbReader(string dbConnectionString)
        {
            _dbConnectionString = dbConnectionString;
        }

        /// <summary>
        /// Returns back all school classes (Klassen) for a given school/tenant and a given school year
        /// </summary>
        /// <param name="schoolNo">Official school number</param>
        /// <param name="schoolYearCode">School Year shortname</param>
        /// <returns>An async enumerator of school classes</returns>
        public async IAsyncEnumerable<SchoolClass> SchoolClassesAsync(string schoolNo, string schoolYearCode)
        {
            await foreach (var entity in EntitiesAsync(command => SetQuery(command), reader => SchoolClass.FromDb(reader)))
            {
                yield return entity;
            }

            void SetQuery(DbCommand dbCommand)
            {
                dbCommand.CommandText =
                    $"select " +
                    $"  svp_klassengruppe.id as klassengruppe_id, " +
                    $"  svp_klassengruppe.kennung as klassengruppe_kennung, " +
                    $"  svp_klasse.id as klasse_id, " +
                    $"  svp_klasse.klassenname, " +
                    $"  svp_klasse.klassenname_statistik, " +
                    $"  svp_klasse.klassenname_lang " +
                    $"from asv.svp_klasse " +
                    $"left join asv.svp_klassengruppe on svp_klassengruppe.klasse_id = svp_klasse.id " +
                    $"join asv.svp_schule_schuljahr on svp_schule_schuljahr.id = svp_klasse.schule_schuljahr_id " +
                    $"join asv.svp_schule_stamm on svp_schule_stamm.id = svp_schule_schuljahr.schule_stamm_id " +
                    $"join asv.svp_wl_schuljahr on svp_wl_schuljahr.id = svp_schule_schuljahr.schuljahr_id " +
                    $"where svp_wl_schuljahr.kurzform = @schoolYearCode and svp_schule_stamm.schulnummer = @schoolNo";

                var dbParameter1 = dbCommand.CreateParameter();
                dbParameter1.ParameterName = "schoolNo";
                dbParameter1.Value = schoolNo;
                var dbParameter2 = dbCommand.CreateParameter();
                dbParameter2.ParameterName = "schoolYearCode";
                dbParameter2.Value = schoolYearCode;

                dbCommand.Parameters.Add(dbParameter1);
                dbCommand.Parameters.Add(dbParameter2);
            }
        }

        /// <summary>
        /// Returns back all school years (Schuljahre) for a given school/tenant
        /// </summary>
        /// <param name="schoolNo">Official school number</param>
        /// <returns>An async enumerator of school years</returns>
        public async IAsyncEnumerable<SchoolYear> SchoolYearsAsync(string schoolNo)
        {
            await foreach (var entity in EntitiesAsync(command => SetQuery(command), reader => SchoolYear.FromDb(reader)))
            {
                yield return entity;
            }

            void SetQuery(DbCommand dbCommand)
            {
                dbCommand.CommandText =
                    $"select " +
                    $"  svp_schule_schuljahr.id, " +
                    $"	svp_wl_schuljahr.kurzform, " +
                    $"	svp_wl_schuljahr.langform, " +
                    $"	svp_wl_schuljahr.anzeigeform, " +
                    $"	svp_wl_schuljahr.beginn, " +
                    $"	svp_wl_schuljahr.ende " +
                    $"from asv.svp_schule_schuljahr " +
                    $"join asv.svp_schule_stamm on svp_schule_stamm.id = svp_schule_schuljahr.schule_stamm_id " +
                    $"join asv.svp_wl_schuljahr on svp_wl_schuljahr.id = svp_schule_schuljahr.schuljahr_id " +
                    $"where svp_schule_stamm.schulnummer = @schoolNo";

                var dbParameter1 = dbCommand.CreateParameter();
                dbParameter1.ParameterName = "schoolNo";
                dbParameter1.Value = schoolNo;

                dbCommand.Parameters.Add(dbParameter1);
            }
        }

        /// <summary>
        /// Returns back all students (Schüler) for a given school/tenant and a given school year
        /// </summary>
        /// <param name="schoolNo">Official school number</param>
        /// <param name="schoolYearCode">School Year shortname</param>
        /// <param name="activeStudentsOnly">Select only active students</param>
        /// <returns>An async enumerator of students</returns>
        public async IAsyncEnumerable<Student> StudentsAsync(string schoolNo, string schoolYearCode, bool activeStudentsOnly)
        {
            await foreach (var entity in EntitiesAsync(command => SetQuery(command), reader => Student.FromDb(reader)))
            {
                yield return entity;
            }

            void SetQuery(DbCommand dbCommand)
            {
                dbCommand.CommandText =
                    $"select distinct" +
                    $"  svp_schueler_stamm.id, " +
                    $"  svp_schueler_stamm.familienname, " +
                    $"  svp_schueler_stamm.vornamen, " +
                    $"  svp_schueler_stamm.geburtsdatum, " +
                    $"  svp_schueler_stamm.rufname, " +
                    $"  svp_schueler_stamm.namensbestandteil_vorangest, " +
                    $" 	svp_schueler_stamm.namensbestandteil_nachgest, " +
                    $"  svp_schueler_stamm.eintrittsdatum, " +
                    $"  svp_schueler_stamm.austrittsdatum, " +
                    $"  svp_wl_wert.schluessel as geschlecht " +
                    $"from asv.svp_schueler_schuljahr " +
                    $"join asv.svp_schueler_stamm on svp_schueler_stamm.id = svp_schueler_schuljahr.schueler_stamm_id " +
                    $"join asv.svp_schule_schuljahr on svp_schule_schuljahr.schuljahr_id = svp_schueler_schuljahr.schuljahr_id " +
                    $"join asv.svp_schule_stamm on svp_schule_stamm.id = svp_schule_schuljahr.schule_stamm_id " +
                    $"join asv.svp_wl_schuljahr on svp_wl_schuljahr.id = svp_schule_schuljahr.schuljahr_id " +
                    $"left join asv.svp_wl_wert on svp_wl_wert.id = svp_schueler_stamm.wl_geschlecht_id " +
                    $"where svp_wl_schuljahr.kurzform = @schoolYearCode and svp_schule_stamm.schulnummer = @schoolNo and " +
                    $"((not @activeStudentsOnly) or (svp_schueler_stamm.austrittsdatum is null))";

                var dbParameter1 = dbCommand.CreateParameter();
                dbParameter1.ParameterName = "schoolNo";
                dbParameter1.Value = schoolNo;
                var dbParameter2 = dbCommand.CreateParameter();
                dbParameter2.ParameterName = "schoolYearCode";
                dbParameter2.Value = schoolYearCode;
                var dbParameter3 = dbCommand.CreateParameter();
                dbParameter3.ParameterName = "activeStudentsOnly";
                dbParameter3.Value = activeStudentsOnly;

                dbCommand.Parameters.Add(dbParameter1);
                dbCommand.Parameters.Add(dbParameter2);
                dbCommand.Parameters.Add(dbParameter3);
            }
        }

        /// <summary>
        /// Returns back the school class attendances (Klassenzugehörigkeiten) of all students for a given school/tenant and a given school year
        /// </summary>
        /// <param name="schoolNo">Official school number</param>
        /// <param name="schoolYearCode">School Year shortname</param>
        /// <param name="activeStudentsOnly">Select only active students</param>
        /// <returns>An async enumerator of school class attendances</returns>
        public async IAsyncEnumerable<StudentSchoolClassAttendance> StudentSchoolClassAttendancesAsync(string schoolNo, string schoolYearCode, bool activeStudentsOnly)
        {
            await foreach (var entity in EntitiesAsync(command => SetQuery(command), reader => StudentSchoolClassAttendance.FromDb(reader)))
            {
                yield return entity;
            }

            void SetQuery(DbCommand dbCommand)
            {
                dbCommand.CommandText =
                    $"select distinct" +
                    $"  svp_schueler_stamm.id as schueler_id, " +
                    $"  svp_schueler_schuljahr.klassengruppe_id as klassengruppe_id, " +
                    $"  svp_klassengruppe.klasse_id " +
                    $"from asv.svp_schueler_schuljahr " +
                    $"join asv.svp_schueler_stamm on svp_schueler_stamm.id = svp_schueler_schuljahr.schueler_stamm_id " +
                    $"join asv.svp_klassengruppe on svp_klassengruppe.id = svp_schueler_schuljahr.klassengruppe_id " +
                    $"join asv.svp_schule_schuljahr on svp_schule_schuljahr.schuljahr_id = svp_schueler_schuljahr.schuljahr_id " +
                    $"join asv.svp_schule_stamm on svp_schule_stamm.id = svp_schule_schuljahr.schule_stamm_id " +
                    $"join asv.svp_wl_schuljahr on svp_wl_schuljahr.id = svp_schule_schuljahr.schuljahr_id " +
                    $"where svp_wl_schuljahr.kurzform = @schoolYearCode and svp_schule_stamm.schulnummer = @schoolNo and " +
                    $"((not @activeStudentsOnly) or (svp_schueler_stamm.austrittsdatum is null))";

                var dbParameter1 = dbCommand.CreateParameter();
                dbParameter1.ParameterName = "schoolNo";
                dbParameter1.Value = schoolNo;
                var dbParameter2 = dbCommand.CreateParameter();
                dbParameter2.ParameterName = "schoolYearCode";
                dbParameter2.Value = schoolYearCode;
                var dbParameter3 = dbCommand.CreateParameter();
                dbParameter3.ParameterName = "activeStudentsOnly";
                dbParameter3.Value = activeStudentsOnly;

                dbCommand.Parameters.Add(dbParameter1);
                dbCommand.Parameters.Add(dbParameter2);
                dbCommand.Parameters.Add(dbParameter3);
            }
        }

        /// <summary>
        /// Returns back the subjects of all students for a given school/tenant and a given school year
        /// </summary>
        /// <param name="schoolNo">Official school number</param>
        /// <param name="schoolYearCode">School Year shortname</param>
        /// <param name="activeStudentsOnly">Select only active students</param>
        /// <returns>An async enumerator of subjects</returns>
        public async IAsyncEnumerable<StudentSubject> StudentSubjectsAsync(string schoolNo, string schoolYearCode, bool activeStudentsOnly)
        {
            await foreach (var entity in EntitiesAsync(command => SetQuery(command), reader => StudentSubject.FromDb(reader)))
            {
                yield return entity;
            }

            void SetQuery(DbCommand dbCommand)
            {
                dbCommand.CommandText =
                    $"select distinct" +
                    $"	svp_schueler_stamm.id as schueler_id, " +
                    $"	svp_unterrichtselement.klassengruppe_id as klassengruppe_id, " +
                    $"  svp_klassengruppe.klasse_id as klasse_id, " +
                    $"	svp_fachgruppe.schuelerfach_id as fach_id, " +
                    $"	svp_lehrer_stamm.id as lehrer_id " +
                    $"from asv.svp_unterrichtselement " +
                    $"join asv.svp_klassengruppe on svp_klassengruppe.id = svp_unterrichtselement.klassengruppe_id " +
                    $"join asv.svp_besuchtes_fach_u_element on svp_besuchtes_fach_u_element.unterrichtselement_id = svp_unterrichtselement.id " +
                    $"join asv.svp_besuchtes_fach on svp_besuchtes_fach.id = svp_besuchtes_fach_u_element.besuchtes_fach_id " +
                    $"join asv.svp_fachgruppe on svp_fachgruppe.id = svp_unterrichtselement.fachgruppe_id " +
                    $"join asv.svp_schuelerfach on svp_schuelerfach.id = svp_fachgruppe.schuelerfach_id " +
                    $"join asv.svp_schule_schuljahr on svp_schule_schuljahr.id = svp_schuelerfach.schule_schuljahr_id " +
                    $"join asv.svp_schule_stamm on svp_schule_stamm.id = svp_schule_schuljahr.schule_stamm_id " +
                    $"join asv.svp_wl_schuljahr on svp_wl_schuljahr.id = svp_schule_schuljahr.schuljahr_id " +
                    $"left join asv.svp_schueler_schuljahr on svp_schueler_schuljahr.Id = svp_besuchtes_fach.schueler_schuljahr_id " +
                    $"left join asv.svp_schueler_stamm on svp_schueler_stamm.Id = svp_schueler_schuljahr.schueler_stamm_id " +
                    $"left join asv.svp_lehrer_schuljahr_schule on svp_lehrer_schuljahr_schule.id = svp_unterrichtselement.lehrer_schuljahr_schule_id " +
                    $"left join asv.svp_lehrer_schuljahr lsj on lsj.Id = svp_lehrer_schuljahr_schule.lehrer_schuljahr_id " +
                    $"left join asv.svp_lehrer_stamm on svp_lehrer_stamm.Id = lsj.lehrer_stamm_id " +
                    $"where svp_wl_schuljahr.kurzform = @schoolYearCode and svp_schule_stamm.schulnummer = @schoolNo and " +
                    $"(not svp_fachgruppe.schuelerfach_id is null) and (svp_besuchtes_fach.schulverzeichnis_id is null) and " +
                    $"((not @activeStudentsOnly) or (svp_schueler_stamm.austrittsdatum is null))";

                var dbParameter1 = dbCommand.CreateParameter();
                dbParameter1.ParameterName = "schoolNo";
                dbParameter1.Value = schoolNo;
                var dbParameter2 = dbCommand.CreateParameter();
                dbParameter2.ParameterName = "schoolYearCode";
                dbParameter2.Value = schoolYearCode;
                var dbParameter3 = dbCommand.CreateParameter();
                dbParameter3.ParameterName = "activeStudentsOnly";
                dbParameter3.Value = activeStudentsOnly;

                dbCommand.Parameters.Add(dbParameter1);
                dbCommand.Parameters.Add(dbParameter2);
                dbCommand.Parameters.Add(dbParameter3);
            }
        }

        /// <summary>
        /// Returns back all subjects (Fächer) for a given school/tenant and a given school year
        /// </summary>
        /// <param name="schoolNo">Official school number</param>
        /// <param name="schoolYearCode">School Year shortname</param>
        /// <returns>An async enumerator of subjects</returns>
        public async IAsyncEnumerable<Subject> SubjectsAsync(string schoolNo, string schoolYearCode)
        {
            await foreach (var entity in EntitiesAsync(command => SetQuery(command), reader => Subject.FromDb(reader)))
            {
                yield return entity;
            }

            void SetQuery(DbCommand dbCommand)
            {
                dbCommand.CommandText =
                    $"select " +
                    $"  svp_schuelerfach.id, " +
                    $"  svp_schuelerfach.kurzform, " +
                    $"  svp_schuelerfach.anzeigeform, " +
                    $"  svp_schuelerfach.langform " +
                    $"from asv.svp_schuelerfach " +
                    $"join asv.svp_wl_unterrichtsfach on svp_schuelerfach.unterrichtsfach_id = svp_wl_unterrichtsfach.id " +
                    $"join asv.svp_schule_schuljahr on svp_schule_schuljahr.id = svp_schuelerfach.schule_schuljahr_id " +
                    $"join asv.svp_schule_stamm on svp_schule_stamm.id = svp_schule_schuljahr.schule_stamm_id " +
                    $"join asv.svp_wl_schuljahr on svp_wl_schuljahr.id = svp_schule_schuljahr.schuljahr_id " +
                    $"where svp_wl_schuljahr.kurzform = @schoolYearCode and svp_schule_stamm.schulnummer = @schoolNo";

                var dbParameter1 = dbCommand.CreateParameter();
                dbParameter1.ParameterName = "schoolNo";
                dbParameter1.Value = schoolNo;
                var dbParameter2 = dbCommand.CreateParameter();
                dbParameter2.ParameterName = "schoolYearCode";
                dbParameter2.Value = schoolYearCode;

                dbCommand.Parameters.Add(dbParameter1);
                dbCommand.Parameters.Add(dbParameter2);
            }
        }

        /// <summary>
        /// Returns back all teachers (Lehrer) for a given school/tenant and a given school year
        /// </summary>
        /// <param name="schoolNo">Official school number</param>
        /// <param name="schoolYearCode">School Year shortname</param>
        /// <returns>An async enumerator of teachers</returns>
        public async IAsyncEnumerable<Teacher> TeachersAsync(string schoolNo, string schoolYearCode)
        {
            await foreach (var entity in EntitiesAsync(command => SetQuery(command), reader => Teacher.FromDb(reader)))
            {
                yield return entity;
            }

            void SetQuery(DbCommand dbCommand)
            {
                dbCommand.CommandText =
                    $"select " +
                    $"  svp_lehrer_stamm.id, " +
                    $"  svp_lehrer_schuljahr_schule.namenskuerzel, " +
                    $"  svp_lehrer_stamm.familienname, " +
                    $"  svp_lehrer_stamm.vornamen, " +
                    $"  svp_lehrer_stamm.geburtsdatum, " +
                    $"  svp_lehrer_stamm.rufname, " +
                    $"  svp_lehrer_stamm.namensbestandteil_vorangest, " +
                    $" 	svp_lehrer_stamm.namensbestandteil_nachgest, " +
                    $"  svp_lehrer_schuljahr.zugang_datum, " +
                    $"  svp_lehrer_schuljahr.abgang_datum, " +
                    $"  wl1.schluessel as geschlecht, " +
                    $"  wl2.schluessel as lehrerart " +
                    $"from asv.svp_lehrer_stamm " +
                    $"join asv.svp_lehrer_schuljahr on svp_lehrer_schuljahr.lehrer_stamm_id = asv.svp_lehrer_stamm.id " +
                    $"join asv.svp_lehrer_schuljahr_schule on svp_lehrer_schuljahr.Id = svp_lehrer_schuljahr_schule.lehrer_schuljahr_id " +
                    $"join asv.svp_schule_schuljahr on svp_schule_schuljahr.id = svp_lehrer_schuljahr_schule.schule_schuljahr_id " +
                    $"join asv.svp_schule_stamm on svp_schule_stamm.id = svp_schule_schuljahr.schule_stamm_id " +
                    $"join asv.svp_wl_schuljahr on svp_wl_schuljahr.id = svp_schule_schuljahr.schuljahr_id " +
                    $"left join asv.svp_wl_wert as wl1 on wl1.id = svp_lehrer_stamm.wl_geschlecht_id " +
                    $"left join asv.svp_wl_wert as wl2 on wl2.id = svp_lehrer_stamm.wl_lehrerart_id " +
                    $"where svp_wl_schuljahr.kurzform = @schoolYearCode and svp_schule_stamm.schulnummer = @schoolNo";

                var dbParameter1 = dbCommand.CreateParameter();
                dbParameter1.ParameterName = "schoolNo";
                dbParameter1.Value = schoolNo;
                var dbParameter2 = dbCommand.CreateParameter();
                dbParameter2.ParameterName = "schoolYearCode";
                dbParameter2.Value = schoolYearCode;

                dbCommand.Parameters.Add(dbParameter1);
                dbCommand.Parameters.Add(dbParameter2);
            }
        }

        /// <summary>
        /// Returns back all schools/tenants
        /// </summary>
        /// <returns>An async enumerator of tenants</returns>
        public async IAsyncEnumerable<Tenant> TenantsAsync()
        {
            await foreach (var entity in EntitiesAsync(command => SetQuery(command), reader => Tenant.FromDb(reader)))
            {
                yield return entity;
            }

            void SetQuery(DbCommand dbCommand)
            {
                dbCommand.CommandText =
                    $"select id, schulnummer, kurzname, dienststellenname " +
                    $"from asv.svp_schule_stamm";
            }
        }
        /// <summary>
        /// Creates either a MS SQL Server or a MySQL database connection
        /// </summary>
        /// <returns>The newly created database connection</returns>
        private DbConnection CreateConnection()
        {
            return new NpgsqlConnection(_dbConnectionString);
        }

        /// <summary>
        /// Opens the internal database connection, executes an SQL query and iterates over the result set.
        /// </summary>
        /// <typeparam name="TEntity">Enttiy type to be created</typeparam>
        /// <param name="setCommand">Action for initializing the sql command</param>
        /// <param name="createEntity">Action for creating a new TEntity instance</param>
        /// <returns>An async enumerator of TEntity instances</returns>
        private async IAsyncEnumerable<TEntity> EntitiesAsync<TEntity>(Action<DbCommand> setCommand, Func<DbDataReader, TEntity> createEntity)
        {
            using DbConnection dbConnection = CreateConnection();

            await dbConnection.OpenAsync();
            try
            {
                using var dbTransaction = dbConnection.BeginTransaction();
                using var dbCommand = dbConnection.CreateCommand();

                dbCommand.Transaction = dbTransaction;
                setCommand(dbCommand);

                using var reader = await dbCommand.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    yield return createEntity(reader);
                }
            }
            finally
            {
                await dbConnection.CloseAsync();
            }
        }
    }
}
