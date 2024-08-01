using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LinqToSql_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LinqToSqlDataClassesDataContext dataContext;
        public MainWindow()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["LinqToSql_WPF.Properties.Settings.FirstDBConnectionString"].ConnectionString;
            dataContext = new LinqToSqlDataClassesDataContext(connectionString);

            //InsertUniversities();
            //InsertStudent();
            //InsertLectures();
            //InsertStudentLectureAssociation();
            //GetUniversityOfKumi();
            //GetKumisLectures();
            //GetAllStudentsFromCU();
            //GetAllUniversitiesWithFemale();
            //LecturesAtBtu();
            //UpdateKumi();
            DeleteLucy();
        }

        public void InsertUniversities()
        {
            dataContext.ExecuteCommand("delete from University");

            University cu = new University();
            cu.Name = "CU";
            dataContext.Universities.InsertOnSubmit(cu);

            University btu = new University();
            btu.Name = "BTU";
            dataContext.Universities.InsertOnSubmit(btu);

            dataContext.SubmitChanges();

            MainDataGrid.ItemsSource = dataContext.Universities;
        }

        public void InsertStudent()
        {
            University cu = dataContext.Universities.First(uni => uni.Name.Equals("CU"));
            University btu = dataContext.Universities.First(uni => uni.Name.Equals("BTU"));

            List<Student> students = new List<Student>();

            students.Add(new Student() { Name = "El Kumi", Gender = "Kacuri kaci", UniversityId = cu.Id});
            students.Add(new Student() { Name = "Tony", Gender = "Male", University = cu });
            students.Add(new Student() { Name = "Mari", Gender = "Female", University = cu });
            students.Add(new Student() { Name = "Lucy", Gender = "Female", University = btu });

            dataContext.Students.InsertAllOnSubmit(students);
            dataContext.SubmitChanges();

            MainDataGrid.ItemsSource = dataContext.Students;

        }

        public void InsertLectures()
        {
            dataContext.Lectures.InsertOnSubmit(new Lecture() { Name = "C++"});
            dataContext.Lectures.InsertOnSubmit(new Lecture() { Name = "C#"});
            dataContext.Lectures.InsertOnSubmit(new Lecture() { Name = "Python"});

            dataContext.SubmitChanges();
            MainDataGrid.ItemsSource = dataContext.Lectures;
        }

        public void InsertStudentLectureAssociation()
        {
            Student kumi = dataContext.Students.First(s => s.Name.Equals("El Kumi"));
            Student tony = dataContext.Students.First(s => s.Name.Equals("Tony"));
            Student mari = dataContext.Students.First(s => s.Name.Equals("Mari"));
            Student lucy = dataContext.Students.First(s => s.Name.Equals("Lucy"));

            Lecture cpp = dataContext.Lectures.First(l => l.Name.Equals("C++"));
            Lecture cs = dataContext.Lectures.First(l => l.Name.Equals("C#"));
            Lecture python = dataContext.Lectures.First(l => l.Name.Equals("Python"));

            dataContext.StudentLectures.InsertOnSubmit(new StudentLecture() { Student = kumi, Lecture = cpp});
            dataContext.StudentLectures.InsertOnSubmit(new StudentLecture() { Student = mari, Lecture = python});
            dataContext.StudentLectures.InsertOnSubmit(new StudentLecture() { Student = kumi, Lecture = cs});
            dataContext.StudentLectures.InsertOnSubmit(new StudentLecture() { Student = lucy, Lecture = python});

            dataContext.SubmitChanges();
            MainDataGrid.ItemsSource = dataContext.StudentLectures;
        }

        public void GetUniversityOfKumi()
        {
            Student kumi = dataContext.Students.First(s => s.Name.Equals("El Kumi"));
            University kumisUniversity = kumi.University;

            List<University> universities = new List<University>();
            universities.Add(kumisUniversity);

            MainDataGrid.ItemsSource = universities;//datagrid requires ienumerable
        }

        public void GetKumisLectures()
        {
            Student kumi = dataContext.Students.First(k => k.Name.Equals("El Kumi"));

            var kumisLectures = from lecture in kumi.StudentLectures
                                select lecture.Lecture;

            MainDataGrid.ItemsSource = kumisLectures;
        }

        public void GetAllStudentsFromCU()
        {
            var studentsFromCU = from student in dataContext.Students
                                 where student.University.Name.Equals("CU")
                                 select student;

            MainDataGrid.ItemsSource = studentsFromCU;
        }

        public void GetAllUniversitiesWithFemale()
        {
            var universitiesWithFemaleGender = from student in dataContext.Students
                                               join university in dataContext.Universities
                                               on student.University equals university
                                               where student.Gender == "Female"
                                               select university;

            MainDataGrid.ItemsSource = universitiesWithFemaleGender;
        }

        public void LecturesAtBtu()
        {
            var lecturesBtu = from sl in dataContext.StudentLectures
                              join student in dataContext.Students
                              on sl.StudentId equals student.Id
                              where student.University.Name == "BTU"
                              select sl.Lecture;

            MainDataGrid.ItemsSource = lecturesBtu;
        }

        public void UpdateKumi()
        {
            Student kumi = dataContext.Students.FirstOrDefault(k => k.Name.Equals("El Kumi"));
            kumi.Name = "kumi el fuego";
            dataContext.SubmitChanges();

            MainDataGrid.ItemsSource = dataContext.Students;
        }

        public void DeleteLucy()
        {

            Student lucy = dataContext.Students.FirstOrDefault(l => l.Name.Equals("Lucy"));
            dataContext.Students.DeleteOnSubmit(lucy);
            dataContext.SubmitChanges();


            // in order to delete from db execute the following code :)
            string connectionString = ConfigurationManager.ConnectionStrings["LinqToSql_WPF.Properties.Settings.FirstDBConnectionString"].ConnectionString;
            LinqToSqlDataClassesDataContext db = new LinqToSqlDataClassesDataContext(connectionString);

            MainDataGrid.ItemsSource = dataContext.Students;
        }
    }
}
