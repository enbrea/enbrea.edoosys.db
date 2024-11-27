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
using System.Threading.Tasks;

namespace Enbrea.Edoosys.Db.SmokeTest
{
    public class AppService
    {
        private readonly AppConfig _appConfig;

        public AppService(AppConfig appConfig)
        {
            _appConfig = appConfig;
        }

        public async Task StartAsync()
        {
            Console.WriteLine("edoo.sys Export Test");
            Console.WriteLine("--------------------");
            Console.WriteLine();

            Console.WriteLine("Choose an option from the following list:");
            Console.WriteLine("\t1 - Export Tenants (Mandanten/Schulen)");
            Console.WriteLine("\t2 - Export School Years (Schuljahre)");
            Console.WriteLine("\t3 - Export School Classes (Klassen)");
            Console.WriteLine("\t4 - Export Students (Schüler)");
            Console.WriteLine("\t5 - Export Student School Class Attendances (Schüler-Klassenteilnahmen)");
            Console.WriteLine("\t6 - Export Student Subjects (Schüler-Fächer)");
            Console.WriteLine("\t7 - Export Subjects (Fächer)");
            Console.WriteLine("\t8 - Export Teachers (Lehrer)");
            Console.Write("Your selection? ");

            switch (Console.ReadLine())
            {
                case "1":
                    await ExportTenants();
                    break;
                case "2":
                    await ExportSchoolYears();
                    break;
                case "3":
                    await ExportSchoolClasses();
                    break;
                case "4":
                    await ExportStudents();
                    break;
                case "5":
                    await ExportStudentSchoolClassAttendances();
                    break;
                case "6":
                    await ExportStudentSubjects();
                    break;
                case "7":
                    await ExportSubjects();
                    break;
                case "8":
                    await ExportTeachers();
                    break;
            }
        }

        private EdoosysDbReader CreateDbReader()
        {
            return new EdoosysDbReader(_appConfig.DbConnection);
        }

        private async Task ExportSchoolClasses()
        {
            var dbReader = CreateDbReader();

            Console.WriteLine();
            Console.WriteLine("School Classes:");
            Console.WriteLine("---------------");

            var count = 0;
            await foreach (var schoolClass in dbReader.SchoolClassesAsync(_appConfig.SchoolNo, _appConfig.SchoolYearCode))
            {
                Console.WriteLine(@"{0}, {1}, {2}, {3}",
                    schoolClass.Id,
                    schoolClass.Code,
                    schoolClass.RootCode,
                    schoolClass.RootName);
                count++;
            }
            Console.WriteLine();
            Console.WriteLine($"Total count: {count}");
        }

        private async Task ExportTenants()
        {
            var dbReader = CreateDbReader();

            Console.WriteLine();
            Console.WriteLine("Mandanten:");
            Console.WriteLine("----------");

            var count = 0;
            await foreach (var tenant in dbReader.TenantsAsync())
            {
                Console.WriteLine(@"{0}, {1}, {2}",
                    tenant.Id,
                    tenant.Code,
                    tenant.Name);
                count++;
            }
            Console.WriteLine();
            Console.WriteLine($"Total count: {count}");
        }

        private async Task ExportSchoolYears()
        {
            var dbReader = CreateDbReader();

            Console.WriteLine();
            Console.WriteLine("School Years:");
            Console.WriteLine("-------------");

            var count = 0;
            await foreach (var schoolYear in dbReader.SchoolYearsAsync(_appConfig.SchoolNo))
            {
                Console.WriteLine(@"{0}, {1}, {2}",
                    schoolYear.Id,
                    schoolYear.Code,
                    schoolYear.Name);
                count++;
            }
            Console.WriteLine();
            Console.WriteLine($"Total count: {count}");
        }

        private async Task ExportStudents()
        {
            var dbReader = CreateDbReader();

            Console.WriteLine();
            Console.WriteLine("Students:");
            Console.WriteLine("---------");

            var count = 0;
            await foreach (var student in dbReader.StudentsAsync(_appConfig.SchoolNo, _appConfig.SchoolYearCode, true))
            {
                Console.WriteLine(@"{0}, {1}, {2}, {3}",
                    student.Lastname,
                    student.Firstname,
                    student.Birthdate?.ToString("dd.MM.yyyy"),
                    student.Gender?.ToString());
                count++;
            }
            Console.WriteLine();
            Console.WriteLine($"Total count: {count}");
        }

        private async Task ExportStudentSchoolClassAttendances()
        {
            var dbReader = CreateDbReader();

            Console.WriteLine();
            Console.WriteLine("Student School Class Attendances:");
            Console.WriteLine("---------------------------------");

            var count = 0;
            await foreach (var attendance in dbReader.StudentSchoolClassAttendancesAsync(_appConfig.SchoolNo, _appConfig.SchoolYearCode, true))
            {
                Console.WriteLine(@"{0}, {1}, {2}",
                    attendance.StudentId,
                    attendance.SchoolClassId,
                    attendance.SchoolClassRootId);
                count++;
            }
            Console.WriteLine();
            Console.WriteLine($"Total count: {count}");
        }

        private async Task ExportStudentSubjects()
        {
            var dbReader = CreateDbReader();

            Console.WriteLine();
            Console.WriteLine("Student Subjects:");
            Console.WriteLine("-----------------");

            var count = 0;
            await foreach (var studentSubject in dbReader.StudentSubjectsAsync(_appConfig.SchoolNo, _appConfig.SchoolYearCode, true))
            {
                Console.WriteLine(@"{0}, {1}, {2}, {3}, {4}",
                    studentSubject.StudentId,
                    studentSubject.SubjectId,
                    studentSubject.SchoolClassId,
                    studentSubject.SchoolClassRootId,
                    studentSubject.TeacherId);
                count++;
            }
            Console.WriteLine();
            Console.WriteLine($"Total count: {count}");
        }

        private async Task ExportSubjects()
        {
            var dbReader = CreateDbReader();

            Console.WriteLine();
            Console.WriteLine("Subjects:");
            Console.WriteLine("---------");

            var count = 0;
            await foreach (var subject in dbReader.SubjectsAsync(_appConfig.SchoolNo, _appConfig.SchoolYearCode))
            {
                Console.WriteLine(@"{0}, {1}, {2}, {3}",
                    subject.Id,
                    subject.Code,
                    subject.Name,
                    subject.DisplayName);
                count++;
            }
            Console.WriteLine();
            Console.WriteLine($"Total count: {count}");
        }

        private async Task ExportTeachers()
        {
            var dbReader = CreateDbReader();

            Console.WriteLine();
            Console.WriteLine("Teachers:");
            Console.WriteLine("---------");

            var count = 0;
            await foreach (var teacher in dbReader.TeachersAsync(_appConfig.SchoolNo, _appConfig.SchoolYearCode))
            {
                Console.WriteLine(@"{0}, {1}, {2}, {3}", 
                    teacher.Lastname, 
                    teacher.Firstname, 
                    teacher.Birthdate?.ToString("dd.MM.yyyy"), 
                    teacher.Gender?.ToString());
                count++;
            }
            Console.WriteLine();
            Console.WriteLine($"Total count: {count}");
        }
    }
}
