using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace pj_2
{
    public partial class MainWindow : Window
    {
        private StudentMgr studentManager;

        public MainWindow()
        {
            InitializeComponent();

            DataGrid studentDataGrid = StudentData;
            studentManager = new StudentMgr(studentDataGrid);   // studengMgr 실행
        }



        /* 이벤트 설정 */

        // 저장 버튼
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Student> updatedStudents = new ObservableCollection<Student>();

            // --------- 필수 정보 ----------
            // 이름
            string stName = studentName.Text;
            string regexPatten = @"^[가-힣]+$";

            if (string.IsNullOrEmpty(stName) || !Regex.IsMatch(stName, regexPatten)) {
                MessageBox.Show("이름은 한글로만 입력 가능합니다.");
                return;
            }


            // 학번
            string stId = studentId.Text;
            if (string.IsNullOrEmpty(stId))
            {
                MessageBox.Show("학번을 입력하세요");
                return;
            }

            // 학년
            Grade stGrade = (Grade)(gradeComboBox.SelectedIndex + 1);

            if (gradeComboBox.SelectedIndex == -1) {
                MessageBox.Show("학년을 선택하세요");
                return;
            }

            // --------- 세부 정보 ------------
            // 성별
            Sex stSex = (Sex)(sexComboBox.SelectedIndex);
            // 소속대학, 전공, 주소, 이메일, 생년월일 - 추가 코드 필요 (현재 반영 X)



            try
            {
                // 저장 1. 필수 정보만 입력
                if (!string.IsNullOrEmpty(stName) && !string.IsNullOrEmpty(stId) && gradeComboBox.SelectedIndex >= 0)
                {
                    Student student = new Student(stName, int.Parse(stId), stGrade);

                    studentManager.AddStudent(student);     
                    studentId.Text = "";
                    studentName.Text = "";
                    gradeComboBox.SelectedIndex = -1;
                }
                // 저장 2. 추가 정보 [추가 수정 필요]
                else if (sexComboBox.SelectedIndex >= 0)
                {
                    Student student = new Student(stName, int.Parse(stId), stGrade, stSex);
                    MessageBox.Show($"성별:: {stSex}");

                    /* student 생성자 List에 저장 -> observableCollection으로 수정
                    그리드 뷰 데이터 바인딩 문제 */
                    studentManager.AddStudent(student);
                    studentId.Text = "";
                    studentName.Text = "";
                    gradeComboBox.SelectedIndex = -1;
                    sexComboBox.SelectedIndex = -1;
                }

                // StudentData.Items.Refresh(); // 다른 업데이트 기능 필요
                /* 저장 누르기 전에, delete나 수정 등 다른 이벤트가 있을 수 있다.
                 getStudents로 현재 학생 정보 데이터를 가져와야 함*/
                updatedStudents = studentManager.GetStudents();
                StudentData.ItemsSource = updatedStudents;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        // 조회 버튼
        private void SearchBtn_Click(object sender, RoutedEventArgs e) {
            string targetName = searchName.Text;
            string targetGrade = (searchGradeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString().Substring(0,1);

            ObservableCollection<Student> searchedStudents = new ObservableCollection<Student>();

            // 조회 케이스 나누기
            if (string.IsNullOrEmpty(targetName) && string.IsNullOrEmpty(targetGrade)) {
                searchedStudents = studentManager.GetStudents();
            }
            if (!string.IsNullOrEmpty(targetName)) // 이름만 입력
            {
                searchedStudents = studentManager.GetStudentsByName(targetName);
            }
            else if (!string.IsNullOrEmpty(targetGrade)) // 학년만 입력
            {
                searchedStudents = studentManager.GetStudentsByGrade(targetGrade);
            }

            StudentData.ItemsSource = searchedStudents;
        }


        // 삭제 버튼
        private void DeleteBtn_Clik(object sender, RoutedEventArgs e)
        {
            Student targetStudent = (Student)StudentData.SelectedItem;

            if (targetStudent != null)
            {
                studentManager.DeleteStudent(targetStudent);

                ObservableCollection<Student> students = studentManager.GetStudents();
                StudentData.ItemsSource = students;

            }
        }

        // 수정 버튼
        /* 한 헹씩(= 객체 1개)만 수정 -> 여러 행(객체들) 수정하는 것은 수정 필요
           따로 폼/디자인패턴/db 등 다른 방법을 쓰는 게 좋을 것 같다. */
        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            Student targetStudent = (Student)StudentData.SelectedItem;

            if (targetStudent != null)
            {
                foreach (DataGridColumn col in StudentData.Columns)
                {
                    string colName = col.Header.ToString();

                    if (colName == "이름")
                    {
                        TextBox textBox = GetCellContent(col, targetStudent) as TextBox;
                        if (textBox != null)
                        {
                            targetStudent.Name = textBox.Text;
                        }
                    }
                }

                ObservableCollection<Student> students = studentManager.GetStudents();
                StudentData.ItemsSource = students;
                StudentData.Items.Refresh();
            }
        }

        // cell의 내용을 가져오기 위한 사용자 정의 함수
        private object GetCellContent(DataGridColumn col, Student student)
        {
            var cellContent = col.GetCellContent(student);
            if (cellContent is TextBlock textBlock)
            {
                return textBlock.Text;
            }
            else if (cellContent is TextBox textBox)
            {
                return textBox.Text;
            }
            return null;
        }

        private void Initialize_Click(object sender, RoutedEventArgs e)
        {
            searchId.Text = "";
            searchName.Text = "";
            searchGradeComboBox.SelectedIndex = -1;
        }

        //그리드 뷰에 Row number를 보여주기 위한 함수
        private void dataGridLoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
    }
}
