using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pj_2
{
    internal class Student
    {
        // 프로퍼티
        public string Name { get; set; }        // Not null
        public int Id { get; set; }             // Not null
        public string College { get; set; }         
        public string Department {  get; set; }

        public Grade Grade { get; set; }            // Not null
        public Sex Sex { get; set; }
        public string Address { get; set; }
        public int PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime BrithDate {  get; set; }



        // 생성자_1 : 모든 학생 정보 입력
        public Student(string name, int id, string college, string dept, Grade grade, Sex sex, int pNum, string email, DateTime bDate)
        {
            Name = name;
            Id = id;
            College = college;
            Department = dept;
            Grade = grade;
            Sex = sex;
            Address = email;
            PhoneNumber = pNum;
            Email = email;
            BrithDate = bDate;
        }


        // 생성자_2 : 필수 정보만 입력
        public Student(string name, int id, Grade grade)
            : this (name, id, "", "", grade, pj_2.Sex.미확인, 0, "", DateTime.MinValue)
        {
        }

        // 생성자_2 : 필수 정보만 입력
        public Student(string name, int id, Grade grade, Sex sex)
            : this(name, id, "", "", grade, sex, 0, "", DateTime.MinValue)
        {
        }

    }
}
