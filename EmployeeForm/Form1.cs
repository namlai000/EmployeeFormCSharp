using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmployeeForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        bool validateInput()
        {
            string name = txtFullName.Text;
            bool bError = false;
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
    }
}
