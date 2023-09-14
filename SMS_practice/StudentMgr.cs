using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace pj_2
{
    class StudentMgr
    {
        private ObservableCollection<Student> Students;
        private DataGrid StudentDataGrid;

        public StudentMgr(DataGrid dataGrid) {
            Students = new ObservableCollection<Student>();
            StudentDataGrid = dataGrid;
            StudentDataGrid.ItemsSource = Students;
        }

        /* CREATE */
        // 저장버튼 -> 학번 유효 검사 -> Student 생성 후, Students에 저장
        public void AddStudent(Student student) { 
            // 학번(고유키) 중복 검사:  Students에 존재하는 학번 검사 -> 저장
            bool IsUnique = true;

            foreach (var s in Students)
            {
                if (s.Id == student.Id)
                {
                    IsUnique = false;
                    break;
                }
            }

            if (IsUnique)
            {
                Students.Add(student);
                MessageBox.Show("학생정보가 정상적으로 저장되었습니다. ");
            }
            else
            {
                MessageBox.Show($"중복된 학번입니다. : {student.Id}");
            }
        }

        /* READ */
        // 1. 기본 조회
        public ObservableCollection<Student> GetStudents()
        {
            return new ObservableCollection<Student>(Students);
        }


        // 2. 이름으로 조회
        public ObservableCollection<Student> GetStudentsByName(string name)
        {
            ObservableCollection<Student> searchedStudents = new ObservableCollection<Student> ();

            foreach (var s in Students) {
                if (s.Name == name)
                {
                    searchedStudents.Add(s);
                }
            }

            return searchedStudents;
        }

        // 3. 학년으로 조회
        public ObservableCollection<Student> GetStudentsByGrade(string grade)
        {
            ObservableCollection<Student> searchedStudents = new ObservableCollection<Student>();

            foreach (var s in Students)
            {
                string stGradeEnum = s.Grade.ToString().Substring(3); // enum 값 처리

                if (stGradeEnum == grade)
                {
                    searchedStudents.Add(s);
                }
            }

            return searchedStudents;
        }


        /* Delete */
        public void DeleteStudent(Student student)
        {
            Students.Remove(student);
        }


        // 4. Update
        // 클릭 이벤트로 처리 됨
        // 기존 조회 함수 GetStudents 사용
    }
}
