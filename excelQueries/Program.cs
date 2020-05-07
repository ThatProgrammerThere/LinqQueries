using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.IO;
using System.Xml;

namespace excelQueries
{
    //class object
   class classdata {

        public string subject;
        public int courseCode;
        public int courseNum;
        public string courseTitle;
        public string days;
        public string startTime;
        public string endTime;
        public string location;
        public int regCap;
        public string classroom;
        public string instructor;

        public static classdata getdataSet(string dataLine) {
            string[] dataSet = dataLine.Split(',');
            classdata courses = new classdata();
            courses.subject = Convert.ToString(dataSet[0]);
            courses.courseCode = Convert.ToInt32(dataSet[1]);
            courses.courseNum = Convert.ToInt32(dataSet[2]);
            courses.courseTitle = Convert.ToString(dataSet[3]);
            courses.days = Convert.ToString(dataSet[4]);
            courses.startTime = Convert.ToString(dataSet[5]);
            courses.endTime = Convert.ToString(dataSet[6]);
            courses.location = Convert.ToString(dataSet[7]);
            courses.regCap = Convert.ToInt32(dataSet[8]);
            courses.classroom = Convert.ToString(dataSet[9]);
            courses.instructor = Convert.ToString(dataSet[10]);
            return courses;
        }
    }
    //teacher object
    class teacherdata
    {
        public string Name;
        public string Office;
        public string Email;

        public static teacherdata getdataSet(string dataLine)
        {
            string[] dataSet = dataLine.Split(',');
            teacherdata teachers = new teacherdata();
            teachers.Name = Convert.ToString(dataSet[0]);
            teachers.Office = Convert.ToString(dataSet[1]);
            teachers.Email = Convert.ToString(dataSet[2]);
            return teachers;
        }

    }


   class Program {

        static void Main(string[] args) {

            //get them classes boi
            classdata[] dataTable = new classdata[231];
            teacherdata[] teachTable = new teacherdata[30];
            int entriesFound = 0;
            using (var textReader = new StreamReader("Courses.csv")) {
                string line = textReader.ReadLine();
                int skipCount = 0;
                while (line != null && skipCount < 1) {
                    line = textReader.ReadLine();
                    skipCount++;
                }
                while (line != null) {
                    // string[] columns = line.Split(',');
                    //perform your logic
                    dataTable[entriesFound] = classdata.getdataSet(line);

                    entriesFound++;
                    line = textReader.ReadLine();

                }
            }

            //get them teachers boi 
            int entries = 0;
            using (var textReader = new StreamReader("Instructors.csv"))
            {
                string line = textReader.ReadLine();
                int skipCount = 0;
                while (line != null && skipCount < 1)
                {
                    line = textReader.ReadLine();
                    skipCount++;
                }
                while (line != null)
                {
                    // string[] columns = line.Split(',');
                    //perform your logic
                    teachTable[entries] = teacherdata.getdataSet(line);

                    entries++;
                    line = textReader.ReadLine();

                }
            }

            /*

             // test that table wrote data correctly
            for(int i = 0; i < dataTable.Length; i++) {
                Console.WriteLine(dataTable[i].subject + dataTable[i].courseCode + " " + dataTable[i].courseTitle);
            }

            for(int i = 0; i < teachTable.Length; i++) {
                Console.WriteLine(teachTable[i].Name + teachTable[i].Office + " " + teachTable[i].Email);
            }
            

            */

            //Time for queries!

            //1.2a

            IEnumerable<object> IEEQuery = (
            from courses in dataTable
            where courses.courseCode >= 300 && courses.subject == "IEE"
            orderby courses.instructor ascending
            select courses
        );
            Console.WriteLine("1.2a Query Results \n\n Course Title \t  | \t instructor \n--------------------------------------------|");
            foreach (classdata courses in IEEQuery) {
            Console.WriteLine(courses.courseTitle + " | " + courses.instructor + "\n--------------------------------------------|");
            }


            //1.2b

        var groupedQuery = (
            from courses in dataTable
            group courses by courses.subject into g1
                from g2 in
                    (from courses in g1
                    group courses by courses.courseCode)
                    group g2 by g1.Key
            
        );
            Console.WriteLine("1.2b Query Results \n--------------------------------------------|");
            foreach (var g in groupedQuery) {
                Console.WriteLine("For the course subject: " + g.Key); 
                foreach(var g2 in g)
                {
                    Console.WriteLine("----------------------------\nFor the course code: " + g2.Key);
                    foreach (var element in g2)
                    {
                        if (g2.Count() > 1)
                        {
                            Console.WriteLine("\n" + element.subject + " " + element.courseCode + "\n");
                        }
                        else Console.WriteLine("No values found");

                    }
                }
            }


            //1.5

        var teacherQuery = (
            from courses in dataTable
            join teachers in teachTable on courses.instructor equals teachers.Name
            where courses.courseCode >=200 && courses.courseCode <300
            orderby courses.courseCode ascending
            select new {
               subj = courses.subject,
               code = courses.courseCode,
               email = teachers.Email
            }
        );
            Console.WriteLine("1.5 Query Results \n--------------------------------------------");
            foreach (var tuple in teacherQuery)
            {
                Console.WriteLine(tuple.subj +"\t|" + tuple.code + "\t|" + tuple.email);
            }



        }
    }
}
