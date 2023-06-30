using Microsoft.Data.Sqlite;
using System.Data;
using System.Reflection;
using System.Xml.Linq;

namespace GcseOptions.Data
{
    public class GcseOptionsDb
    {
        private string _connectionString;

        public GcseOptionsDb(string connectionString)
        {
            _connectionString = connectionString;
        }

        
        public List<string> DisplayClasses(int CourseID)
        {
            var classes = new List<string>();
            using (SqliteConnection connection = new SqliteConnection())
            {
                connection.ConnectionString = _connectionString;
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                string instruction = $"Select ClassID from Class where CourseID = {CourseID}";
                
                command.CommandText = instruction;
                var dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    var class1 = dataReader.GetString(1);
                    classes.Add(class1);
                }
            }
            return classes;
        }
        public List<string> DisplayClassmates(int ClassID)
        {
            var students = new List<string>();
            using (SqliteConnection connection = new SqliteConnection())
            {
                connection.ConnectionString = _connectionString;
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                string instruction = $"Select s.StudentFirstName, s.StudentLast from Students s join ClassAttendance c on s.StudentID = c.StudentID where c.ClassID = {ClassID}";
                command.CommandText = instruction;
                var dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    var name = dataReader.GetString(1) + dataReader.GetString(2);
                    students.Add(name);
                }
            }
            return students;
        }

        public void AddStudent(string name1, string name2, int year, int month, int day)
        {
            using (SqliteConnection connection = new SqliteConnection())
            {
                connection.ConnectionString = _connectionString;
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = $"insert into Students(Studentfirstname,studentlastname,birthyear,birthmonth,birthday) VALUES ({name1},{name2},{year},{month},{day})";
                
                command.ExecuteNonQuery();
            }
        }
        public void AddTeacher(string name1, string name2, string title, string qualification)
        {
            using (SqliteConnection connection = new SqliteConnection())
            {
                connection.ConnectionString = _connectionString;
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = $"insert into Teachers(teacherfirstname,teacherlastname,qualification,title) VALUES ({name1},{name2},{qualification},{title})";

                command.ExecuteNonQuery();
            }
        }
        public List<string> ShowChoices(int StudentID)
        {
            var gcses = new List<string>();
            using (SqliteConnection connection = new SqliteConnection())
            {
                connection.ConnectionString = _connectionString;
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                string instruction = $"Select c.coursename from courses c join classattendace a on c.courseID = a.courseID where a.studentID = {StudentID}";
                command.CommandText = instruction;
                var dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    var subject = dataReader.GetString(1);
                    gcses.Add(subject);
                }
            }
            return gcses;
        }
        public bool ValidateClass(int ClassID)
        {
            List<string> class1 = DisplayClassmates(ClassID);
            if(class1.Count() <= 15 && class1.Count() >= 5)
            {
                return true;

            }

            return false;
        }
        public bool validateGcses(int StudentID)
        {
            int humanityCounter = 0;
            int languageCOunter = 0;
            int creativeCounter = 0;
            int gcseCounter = 0;
            using (SqliteConnection connection = new SqliteConnection())
            {
                
                connection.ConnectionString = _connectionString;
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                string instruction = $"Select co.category from courses co join classattendance ca join class cl on ca.classid = cl.classid on cl.courseid = co.courseid where ca.studentid = {StudentID}";
                command.CommandText = instruction;
                var dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    var category = dataReader.GetString(1);
                    gcseCounter += 1;
                    if (category == "Humanity")
                    {
                        humanityCounter += 1;
                    }
                    else if (category == "Language")
                    {
                        languageCOunter += 1;
                    }
                    else if (category == "Creative")
                    {
                        creativeCounter += 1;
                    }
                }
            }
            if (humanityCounter < 1 || languageCOunter < 1 || creativeCounter < 1 || gcseCounter != 5)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public void AddChoice(int StudentID, int CouseID)
        {
            using (SqliteConnection connection = new SqliteConnection())
            {
                connection.ConnectionString = _connectionString;
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = $"insert into classattendance ";

                command.ExecuteNonQuery();
            }
        }
        public void Menu()
        {
            Console.WriteLine("Please enter:");
            Console.WriteLine("1 to display the classes for a course subject");
            Console.WriteLine("2 to display all students in one class");
            Console.WriteLine("3 to display a student's gcse options");
            Console.WriteLine("4 to verify a student's options");
            Console.WriteLine("5 to add a new teacher");
            Console.WriteLine("6 to add a new student");
            string input = Console.ReadLine();
            if (input == "1")
            {
                Console.WriteLine("What course would you like to see the classes for? Enter the course ID.");
                int course = Convert.ToInt32(Console.ReadLine());
                DisplayClasses(course);
            }
            if (input == "2")
            {
                Console.WriteLine("What class would you like to see the students for? Enter the class ID.");
                int class1 = Convert.ToInt32(Console.ReadLine());
                DisplayClassmates(class1);
            }
            if(input == "3")
            {
                Console.WriteLine("Enter the Student ID of the Student for which you wish to see the choices for.");
                int student1 = Convert.ToInt32(Console.ReadLine());
                ShowChoices(student1);
            }
            if (input == "4")
            {
                Console.WriteLine("Enter the Student ID of the Student for which you wish to validate their choices.");
                int student1 = Convert.ToInt32(Console.ReadLine());
                validateGcses(student1);
            }
            if (input == "5")
            {
                Console.WriteLine("What is their first name?");
                string firstName = Console.ReadLine();
                Console.WriteLine("What is their last name?");
                string lastName = Console.ReadLine();
                Console.WriteLine("What is their title?");
                string title = Console.ReadLine();
                Console.WriteLine("What is their qualification?");
                string qualification = Console.ReadLine();
                AddTeacher(firstName, lastName, title, qualification);

            }
            if (input == "6")
            {
                Console.WriteLine("What is their first name?");
                string firstName = Console.ReadLine();
                Console.WriteLine("What is their last name?");
                string lastName = Console.ReadLine();
                Console.WriteLine("What is their birthyear?");
                int year = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("What is their birthmonth?");
                int month = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("What is their birthday?");
                int day = Convert.ToInt32(Console.ReadLine());
                AddStudent(firstName, lastName, year, month, day);

            }

        }

    }
}