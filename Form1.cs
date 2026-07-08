namespace FinalTest_Hard
{
   
    public partial class Form1 : Form
    {
        private List<Enrollment> enrollments = [];
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new();
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                string[] lines = File.ReadAllLines(ofd.FileName);
                Render(lines);
            }
        }

        private void Render(string[] lines)
        {
            foreach (string line in lines.Skip(1))
            {
                string[] field = line.Split(',');

                enrollments.Add(new Enrollment
                {
                    ID = field[0],
                    Name = field[1],
                    CourseCode = field[2],
                    CodeName = field[3],
                    Grade = field[4],
                    Credit = int.Parse(field[5])
                });
            }
            List<Student> students = ProcessEnroll(enrollments);
            dataGridView1.DataSource = students;
        }

        public List<Student> ProcessEnroll(List<Enrollment> enrolls)
        {
            List<Student> students = [];
            Dictionary<string, Student> unique = [];
            Dictionary<string, List<string>> grades = [];

            enrolls.ForEach(e =>
            {
                if (unique.TryGetValue(e.ID, out Student stu )&& grades.TryGetValue(e.ID, out List<string> grds))
                {
                    stu.TotalCredits += e.Credit;
                    grds.Add(e.Grade);
                    
                } else
                {
                    unique[e.ID] = new Student
                    {
                        ID = e.ID,
                        Name = e.Name,
                        TotalCredits = e.Credit,
                        Grade = ""
                    };
                    grades[e.ID] = [e.Grade];
                }


            });

            foreach(var stu in unique)
            {
                var student = stu.Value;
                student.Grade = CalculateMedianGrade(grades[stu.Key]);
                students.Add(student);
            }

            return students;
        }

        public static string ConvertToGrade(int value)
        {
            return _ = value switch
            {
                >= 90 => "A+",
                >= 85 => "A",
                >= 80 => "A-",
                >= 75 => "B+",
                >= 70 => "B",
                >= 65 => "B-",
                >= 60 => "C+",
                >= 55 => "C",
                >= 50 => "C-",
                >= 40 => "D",
                _ => "E"
            };
        }

        public static int ConvertToValue(string grade)
        {
            return _ = grade switch
            {
                "A+" => 95,
                "A" => 90,
                "A-" => 85,
                "B+" => 80,
                "B" => 75,
                "B-" => 70,
                "C+" => 65,
                "C" => 60,
                "C-" => 55,
                "D" => 50,
                _ => 0
            };
        }

        public static string CalculateMedianGrade(List<string> grades)
        {
            int total = 0;

            foreach(var grade in grades)
            {
                total += ConvertToValue(grade);
            }

            float median = total / grades.Count();

            int parsedVal = (int)Math.Round(median);

            return ConvertToGrade(parsedVal);
            
        }
    } 

    public class Enrollment()
    {
        public required string ID { get; set; }
        public required string Name { get; set; }
        public required string CourseCode { get; set; }
        public required string CodeName { get; set; }
        public required string Grade { get; set; }
        public int Credit { get; set; }
    }

    public class Student()
    {
        public required string ID {  set; get; }
        public required string Name { get; set; }
        public int TotalCredits { get; set; }
        public required string Grade { get; set; }
    }

}
