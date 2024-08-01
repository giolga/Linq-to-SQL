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
            InsertStudent();
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
            students.Add(new Student() { Name = "Lucy", Gender = "Feale", University = btu });

            dataContext.Students.InsertAllOnSubmit(students);
            dataContext.SubmitChanges();

            MainDataGrid.ItemsSource = dataContext.Students;

        }
    }
}
