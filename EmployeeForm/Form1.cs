using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace EmployeeForm
{
    public partial class Form1 : Form
    {
        EmployeeDBEntities entity = new EmployeeDBEntities();

        public Form1()
        {
            InitializeComponent();
            loadEmployee();
        }

        bool validateInput()
        {
            string name = txtFullName.Text;
            bool bError = false;
            errorProvider1.Clear();

            if (name.Length == 0)
            {
                errorProvider1.SetError(txtFullName, "Please enter your name!");
                bError = true;
            }

            DateTime curDate = DateTime.Now;
            int curYear = curDate.Year;
            DateTime dob = dtpDate.Value;
            int birthYear = dob.Year;
            if (curYear - birthYear < 18)
            {
                errorProvider1.SetError(dtpDate, "Age must be greater than or equal to 18");
                bError = true;
            }

            if (radMale.Checked == false && radFemale.Checked == false)
            {
                errorProvider1.SetError(groupBox1, "You must choose a gender");
                bError = true;
            }

            if (cbNational.SelectedIndex < 0)
            {
                errorProvider1.SetError(cbNational, "Please select your national!");
                bError = true;
            }

            if (txtPhone.MaskCompleted == false)
            {
                errorProvider1.SetError(txtPhone, "Please enter required digit!");
                bError = true;
            }

            if (txtAddress.TextLength == 0)
            {
                errorProvider1.SetError(txtAddress, "Please enter address!");
                bError = true;
            }

            if (bError == true)
            {
                return false;
            }
            else
            {
                errorProvider1.Clear();
                return true;
            }
        }

        void addNewEmployee() 
        {
            /*Database*/
            /*
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"server=.\SQLEXPRESS2014;database=EmployeeDB;uid=sa;pwd=namlai120;";
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;

            string gender;
            if (radMale.Checked) gender = "M";
            else gender = "F";

            string salary = "NULL";
            if (txtSalary.Text.Length != 0) salary = txtSalary.Text;

            string date = dtpDate.Value.ToShortDateString();

            string sql = string.Format("INSERT INTO Employees (FullName,DateOfBirth,Gender,[National],Phone,Address,Qualification,Salary) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}',{7})", 
                txtFullName.Text, date, gender, cbNational.Text, txtPhone.Text,  txtAddress.Text, cbQualification.Text, salary);
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
            */

            /*
            DataGridViewRow r = new DataGridViewRow();
            r.CreateCells(dataGridView1);
            r.SetValues("", txtFullName.Text, dtpDate.Value.ToShortDateString(), gender, cbNational.Text, txtPhone.Text, txtAddress.Text, cbQualification.Text, txtSalary.Text);
            dataGridView1.Rows.Add(r);
            */

            /*
             * MODEL ENTITY DATABASE
             * EmployeeDBEntities dataSet = new EmployeeDBEntities();
             * Employees emp = new Employees();
             * emp.FUllName = txtFullName.Text;
             * ...
             * dataSet.AddToEmployees(emp);
             * dataSet.SaveChanges();
             */

            string gender;
            if (radMale.Checked) gender = "M";
            else gender = "F";

            Employee emp = new Employee();
            emp.FullName = txtFullName.Text;
            emp.DateOfBirth = dtpDate.Value;
            emp.Gender = gender;
            emp.National = cbNational.Text;
            emp.Phone = txtPhone.Text;
            emp.Address = txtAddress.Text;
            emp.Qualification = cbQualification.Text;
            emp.Salary = decimal.Parse(txtSalary.Text);

            entity.Employees.Add(emp);
            entity.SaveChanges();
        }

        void updateEmployee()
        {
            /*
            DataGridViewRow r = dataGridView1.SelectedRows[0];
            string gender;
            if (radMale.Checked) gender = "Male";
            else gender = "Female";
            r.SetValues("", txtFullName.Text, dtpDate.Value.ToShortDateString(), gender, cbNational.Text, txtPhone.Text, txtAddress.Text, cbQualification.Text, txtSalary.Text);
            */

            DataGridViewRow r = dataGridView1.SelectedRows[0];
            int id = int.Parse(r.Cells[0].Value.ToString());
            Employee emp = entity.Employees.First(em => em.ID == id);
            emp.FullName = txtFullName.Text;
            entity.SaveChanges();
            
        }

        void loadEmployee()
        {
            /*
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"server=.\SQLEXPRESS2014;database=EmployeeDB;uid=sa;pwd=namlai120";
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "SELECT * FROM Employees";
                SqlDataReader dr = cmd.ExecuteReader();
                dataGridView1.Rows.Clear();
                while (dr.Read())
                {
                    dataGridView1.Rows.Add(dr[0].ToString(), dr[1].ToString(), DateTime.Parse(dr[2].ToString()).ToShortDateString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString(), double.Parse(dr[8].ToString()));
                }
                dr.Close();
            }catch (Exception)
            {
                throw;
            }
            */
            
            dataGridView1.DataSource = entity.Employees.ToList();
        }

        void deleteEmployee(int id)
        {
            /*
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"server=.\SQLEXPRESS2014;database=EmployeeDB;uid=sa;pwd=namlai120;";
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;

            string sql = string.Format("DELETE FROM Employees WHERE ID={0}", id);
                
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
            */

            Employee emp = entity.Employees.First(em => em.ID == id);
            entity.Employees.Remove(emp);
            entity.SaveChanges();
        }

        private void btnAdd_click(object sender, EventArgs e)
        {
            if (validateInput() == false) return;
            try
            {
                addNewEmployee();
                MessageBox.Show("Adding Successful!");
                loadEmployee();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void dataGridView1_SelectedRow(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow r = dataGridView1.SelectedRows[0];
                lbID.Text = r.Cells[0].Value.ToString();
                txtFullName.Text = r.Cells[1].Value.ToString();

                DateTime dt;
                DateTime.TryParse(r.Cells[2].Value.ToString(), out dt);
                dtpDate.Value = dt;

                if (r.Cells[3].Value.ToString().Equals("M")) radMale.Checked = true;
                else radFemale.Checked = true;

                cbNational.Text = r.Cells[4].Value.ToString();
                txtPhone.Text = r.Cells[5].Value.ToString();
                txtAddress.Text = r.Cells[6].Value.ToString();
                cbQualification.Text = r.Cells[7].Value.ToString();
                txtSalary.Text = r.Cells[8].Value.ToString();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (validateInput() == false) return;
            try 
	        {
                updateEmployee();
                MessageBox.Show("Update Successful!");
	        }
	        catch (Exception)
	        {
		
		        throw;
	        }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                //dataGridView1.Rows.Remove(dataGridView1.SelectedRows[0]);
                string id = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                int idd = int.Parse(id);
                deleteEmployee(idd);
                loadEmployee();
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            loadEmployee();
        }
    }
}
