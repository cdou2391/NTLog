using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace NetLog
{
    public partial class frmHome : Form
    {
      
        public frmHome()
        {
            InitializeComponent();
        }
        
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void logOutToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            frmLogin LogIn = new frmLogin();
            this.Close();
        }


        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnExitRecord_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnExitLocate_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExitUpdate_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void btnClear_Click(object sender, EventArgs e)
        {
            regTxtName.Clear();
            regTxtSurname.Clear();
            regTxtEmail.Clear();
            regTxtPhoneNo.Clear();
            regTxtAddress.Clear();
            regTxtBuilding.Clear();
            regTxtFloorNo.Clear();
            regTxtSerialNo.Clear();
            regTxtModel.Clear();
            regTxtOrgEmail.Clear();
            regTxtDeviveName.Clear();
            regTxtModel.Clear();
            comboOrganisation.SelectedIndex = -1;
            txtNewCompany.Clear();
            regTxtType.Clear();
        }
       

        Validation valid = new Validation();
        
        private void btnSave_Click(object sender, EventArgs e)
        {
            string name = regTxtName.Text;
            string surname = regTxtSurname.Text;
            string email = regTxtEmail.Text;
            string number = regTxtPhoneNo.Text;
            string address = regTxtAddress.Text;
            string floorNo = regTxtFloorNo.Text;
            string buildingName = regTxtBuilding.Text;
            string compType = regTxtType.Text;
            string make = regTxtDeviveName.Text;
            string model = regTxtModel.Text;
            string serialNum = regTxtSerialNo.Text;
            string gender = "M";
            string orgName;
            string orgID;

            GenerateClientNumber gen = new GenerateClientNumber();
            string clientNumber = gen.getClientNumber();

            bool message = valid.validation(name,surname,email, number,address, floorNo, buildingName, compType,make,model,
                                        serialNum);
            
                if (message == true)
                {   
                //Sending new client info to database
                    if(comboOrganisation.Text=="New Organisation")
                    {
                        orgName = txtNewCompany.Text;
                    }
                    else
                    {
                        orgName = comboOrganisation.Text;
                    }
                    if (regRadMale.Checked == true)
                    {
                        gender = "M";

                    }
                    else
                    {
                        gender = "F";
                    }

                    using (SqlConnection conn = new SqlConnection(Database.connectionStr))
                    {
                        conn.Open();
                    
                        using (SqlCommand cmd2 = new SqlCommand("SELECT* FROM organisation where name='" + orgName + "'", conn))
                        {
                            SqlDataReader reader = cmd2.ExecuteReader();
                            reader.Read();
                            orgID = reader["orgID"].ToString();
                        reader.Close();
                        using (SqlCommand cmd = new SqlCommand("INSERT INTO client(clientNumber,name, surname,gender,email,contactNumber,organisationID,address,building,floorNo) VALUES(@clientNumber,@name,@surname,@gender,@email,@contactNumber,@organisationID, @address, @building, @floor) ", conn))

                        {
                            cmd.Parameters.AddWithValue("@clientNumber", clientNumber);
                            cmd.Parameters.AddWithValue("@name", regTxtName.Text);
                            cmd.Parameters.AddWithValue("@surname", regTxtSurname.Text);
                            cmd.Parameters.AddWithValue("@gender", gender);
                            cmd.Parameters.AddWithValue("@email", regTxtEmail.Text);
                            cmd.Parameters.AddWithValue("@contactNumber", regTxtPhoneNo.Text);
                            cmd.Parameters.AddWithValue("@address", address);
                            cmd.Parameters.AddWithValue("@building", buildingName);
                            cmd.Parameters.AddWithValue("@floor", floorNo);
                            cmd.Parameters.AddWithValue("@organisationID", orgID);

                            cmd.ExecuteNonQuery();
                        }
                    }
                        
                        using (SqlCommand cmd = new SqlCommand("INSERT INTO devices(deviceID,deviceName,type,model,serialNumber) VALUES(@deviceID,@name,@type,@model,@serialNumber)", conn))
                        {
                            cmd.Parameters.AddWithValue("@deviceID",Convert.ToInt32(clientNumber));
                            cmd.Parameters.AddWithValue("@name", regTxtDeviveName.Text);
                            cmd.Parameters.AddWithValue("@type", regTxtType.Text);
                            cmd.Parameters.AddWithValue("@model", regTxtModel.Text);
                            cmd.Parameters.AddWithValue("@serialNumber", regTxtSerialNo.Text);
                            cmd.ExecuteNonQuery();

                        }
                        LoadClient();


                        MessageBox.Show("New client succesfully registered!");

                    }
                }


                else
                {
                    MessageBox.Show("Please fill in all the fields with the correct information");
                }
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            recTxtName.Clear();
            recTxtSurname.Clear();
            recTxtEmail.Clear();
            recTxtContactNo.Clear();
            recTxtAddress.Clear();
            recTxtBuilding.Clear();
            recTxtFloorNo.Clear();
            recTxtCallDescription.Clear();
            recComboUnikNo.Items.Clear();
            recComboUnikNo.SelectedIndex = -1;
            recComboAssignedTo.Text = "Choose";
            recComboPriority.Text = "Choose";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            updateTxtName.Clear();
            updateTxtSurname.Clear();
            updateTxtEmail.Clear();
            updateTxtContactNo.Clear();
            updateTxtAddress.Clear();
            updateTxtBuilding.Clear();
            updateTxtFloorNo.Clear();
            updateTxtSerialNumber.Clear();
            updateTxtModel.Clear();
            updateTxtDeviceName.Clear();
            updateTxtMake.Clear();
            UpdatecomboUnikNo.SelectedIndex = -1;
            txtNewCompany.Clear();
            updateTxtDeviceName.Clear();
        }

       
        private void button9_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string refNum= recComboUnikNo.Text;
            int i;
            if ((refNum.ToString().Length > 2 ))
            {
                if (int.TryParse(refNum, out i))
                {
                    int id = Convert.ToInt32(refNum);
                    using (SqlConnection conn = new SqlConnection(Database.connectionStr))
                    {
                        conn.Open();
                        try {
                            using (SqlCommand cmd = new SqlCommand("SELECT * from client where clientNumber = '" + id + "'", conn))
                            {
                                SqlDataReader reader = cmd.ExecuteReader();
                                reader.Read();
                                recTxtName.Text = reader["name"].ToString();
                                recTxtSurname.Text = reader["surname"].ToString();
                                recTxtEmail.Text = reader["email"].ToString();
                                recTxtContactNo.Text = reader["contactNumber"].ToString();
                                recTxtAddress.Text = reader["address"].ToString();
                                recTxtBuilding.Text = reader["building"].ToString();
                                recTxtFloorNo.Text = reader["floorNo"].ToString();

                                reader.Close();
                            }
                        }
                        catch (InvalidOperationException)
                        {
                            MessageBox.Show("The number you entered does not exit in the database.Try again");
                        }

                    }
                }
                else
                {
                    string clientName = refNum;
                    string inputName = clientName.Substring(clientName.IndexOf(' ') + 1);
                    using (SqlConnection conn = new SqlConnection(Database.connectionStr))
                    {
                        conn.Open();
                        try {
                            using (SqlCommand cmd = new SqlCommand("SELECT * from client where name = '" + inputName + "'", conn))
                            {
                                SqlDataReader reader = cmd.ExecuteReader();
                                reader.Read();
                                recTxtName.Text = reader["name"].ToString();
                                recTxtSurname.Text = reader["surname"].ToString();
                                recTxtEmail.Text = reader["email"].ToString();
                                recTxtContactNo.Text = reader["contactNumber"].ToString();
                                recTxtAddress.Text = reader["address"].ToString();
                                recTxtBuilding.Text = reader["building"].ToString();
                                recTxtFloorNo.Text = reader["floorNo"].ToString();

                                reader.Close();

                            }
                        }
                        catch (InvalidOperationException)
                        {
                            MessageBox.Show("The name you entered does not exit in the database.Please Try again");
                        }

                    }
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid reference number");
            }
            
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            string refNum = locComboUnikNo.Text;
            if (refNum.ToString().Length > 2)
            {
                DataTable table = new DataTable();
                try
                {
                    using (SqlConnection conn = new SqlConnection(Database.connectionStr))
                    {
                        conn.Open();

                        using (conn)
                        {
                            SqlDataAdapter reader = new SqlDataAdapter("SELECT * from calls where referenceNumber = '" + refNum + "'", conn);
                            DataSet ds = new DataSet();
                            reader.Fill(ds);
                            dataGridView1.VirtualMode = false;
                            dataGridView1.Columns.Clear();
                            dataGridView1.AutoGenerateColumns = true;
                            dataGridView1.DataSource = ds.Tables[0];
                            dataGridView1.Refresh();

                            try
                            {

                                for (int rows = 0; rows < dataGridView1.Rows.Count; rows++)
                                {
                                    string status = dataGridView1.Rows[rows].Cells[6].Value.ToString();
                                    if (status == "Closed")
                                    {
                                        dataGridView1.Rows[rows].DefaultCellStyle.BackColor = Color.Yellow;
                                    }
                                    else if (status == "Escalated")
                                    {
                                        dataGridView1.Rows[rows].DefaultCellStyle.BackColor = Color.Red;
                                    }
                                    else if (status == "Opened")
                                    {
                                        dataGridView1.Rows[rows].DefaultCellStyle.BackColor = Color.Green;
                                    }
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                 }
                catch (InvalidOperationException)
                {
                    MessageBox.Show("The number you entered does not exit in the database.Please Try again");
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid reference number");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string refNum = closeComboUnikNo.Text;
            if (refNum.ToString().Length > 2)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(Database.connectionStr))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand("SELECT * from calls where referenceNumber='" + refNum + "'", conn))
                        {
                            SqlDataReader reader = cmd.ExecuteReader();
                            reader.Read();
                            closeTxtType.Text = reader["type"].ToString();
                            closeTxtDescription.Text = reader["description"].ToString();
                            closeTxtAssignedTo.Text = reader["staffEmail"].ToString();
                            closeTxtAssignedBy.Text = reader["assignedBy"].ToString();

                            reader.Close();
                        }
                    }
                }
                catch (InvalidOperationException)
                {
                    MessageBox.Show("The number you entered does not exit in the database.Please Try again");
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid reference number");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            bool message = valid.validation(updateTxtName.Text,updateTxtSurname.Text,updateTxtEmail.Text,updateTxtContactNo.Text,
                                        updateTxtAddress.Text,updateTxtBuilding.Text,updateTxtFloorNo.Text,updateTxtDeviceName.Text,
                                        updateTxtMake.Text, updateTxtModel.Text,updateTxtSerialNumber.Text);
            string refNum = UpdatecomboUnikNo.Text;
            
            if (message == true)
            {
                string gender;
                string clientName = refNum;
                string inputName = clientName.Substring(clientName.IndexOf(' ') + 1);
                //Sending updated info to database
                if (updateRadFemale.Checked == true)
                {
                    gender = "F";

                }
                else
                {
                    gender = "M";
                }
                

                using (SqlConnection conn = new SqlConnection(Database.connectionStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("UPDATE client SET name=@name,surname=@surname,gender=@gender,email=@email,contactNumber=@contactNumber,address=@address,building=@building,floorNo=@floorNo WHERE clientNumber = '" + refNum + "' or name ='"+inputName+"'", conn))

                    {
                       
                        cmd.Parameters.AddWithValue("@name", updateTxtName.Text);
                        cmd.Parameters.AddWithValue("@surname", updateTxtSurname.Text);
                        cmd.Parameters.AddWithValue("@gender", gender);
                        cmd.Parameters.AddWithValue("@email", updateTxtEmail.Text);
                        cmd.Parameters.AddWithValue("@contactNumber", updateTxtContactNo.Text);
                        cmd.Parameters.AddWithValue("@address", updateTxtAddress.Text);
                        cmd.Parameters.AddWithValue("@building", updateTxtBuilding.Text);
                        cmd.Parameters.AddWithValue("@floorNo", updateTxtFloorNo.Text);
                        
                        cmd.ExecuteNonQuery();
                    }
                }
                LoadClient();
                MessageBox.Show("Information updated");
            }
            else
            {
                MessageBox.Show("Please fill in all the fields with the correct information");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string callType;
            string callPriority = recComboPriority.Text;
            string callStatus = "Opened";
            string callDescription = recTxtCallDescription.Text;
            string callAssignedTo = recComboAssignedTo.Text;
            string callDateLogged = DateTime.Now.ToString();
            string callDateResolved = "Still in progress";
            string callAssignedBy = staffNames.Text;
            string refNum;

            GenerateReferenceNumber genRef = new GenerateReferenceNumber();
            refNum = genRef.GetUniqueKey();

            if (recRadIncident.Checked == true)
            {
                callType = "Incident";
                refNum = "I" + genRef.GetUniqueKey();
            }
            else {
                callType = "Request";
                refNum = "R" + genRef.GetUniqueKey();
            }
            if ((recTxtCallDescription.Text == "") || (recComboUnikNo.Text == "") || (recComboAssignedTo.Text == "Choose") || (recComboPriority.Text == "Choose")
                || (recComboAssignedTo.Text == "") || (recComboPriority.Text == ""))
            {
                MessageBox.Show("Please fill in all the fields with the correct information");
            }
            else
            {
                using (SqlConnection conn = new SqlConnection(Database.connectionStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO calls(referenceNumber,type,staffEmail,assignedBy,priority,description,dateLogged,status,dateResolved) VALUES(@referenceNumber,@type,@staffEmail,@assignedBy,@priority,@description,@dateLogged,@status,@dateResolved) ", conn))
                    {
                        cmd.Parameters.AddWithValue("@referenceNumber", refNum);
                        cmd.Parameters.AddWithValue("@type", callType);
                        cmd.Parameters.AddWithValue("@staffEmail", callAssignedTo);
                        cmd.Parameters.AddWithValue("@assignedBy", callAssignedBy);
                        cmd.Parameters.AddWithValue("@priority", callPriority);
                        cmd.Parameters.AddWithValue("@description", callDescription);
                        cmd.Parameters.AddWithValue("@dateLogged", callDateLogged);
                        cmd.Parameters.AddWithValue("@status", callStatus);
                        cmd.Parameters.AddWithValue("@dateResolved", callDateResolved);
                        
                        cmd.ExecuteNonQuery();
                    }
                }
                LoadCalls();
                string techEmail = recComboAssignedTo.Text;
                string clientEmail = recTxtEmail.Text;
                string clientNames = recTxtSurname.Text + " " + recTxtName.Text;
                string callDesc = recTxtCallDescription.Text;

                SendEmail sendE = new SendEmail();
                string message = sendE.sendEmail(Globals.Staff.Email, techEmail, clientEmail, clientNames, callDesc,refNum);
              
                MessageBox.Show("Call succesfully recorded!");
                
            }
        }

        private void logOutToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            frmLogin LogIn = new frmLogin();
            this.Close();
        }

        private void LoadOrganisation()
        {
            comboOrganisation.Items.Clear();
            comboOrganisation.Items.Add("New Organisation");
            staffNames.Text = Globals.Staff.Name + " " + Globals.Staff.Surname;

            using (SqlConnection conn = new SqlConnection(Database.connectionStr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM organisation", conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ComboboxItem organisation = new ComboboxItem();
                        organisation.ID = (int)reader["orgID"];
                        organisation.Text = reader["name"].ToString();
                        comboOrganisation.Items.Add(organisation);
                    }
                    reader.Close();
                }
            }
        }
        private void LoadClient()
        {
            recComboUnikNo.Items.Clear();
            UpdatecomboUnikNo.Items.Clear();
            using (SqlConnection conn = new SqlConnection(Database.connectionStr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM client", conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ComboboxItem clientNo = new ComboboxItem();
                        clientNo.Text = reader["clientNumber"].ToString();
                        clientNo.ID = Convert.ToInt32(reader["organisationID"]);
                        recComboUnikNo.Items.Add(clientNo);
                        UpdatecomboUnikNo.Items.Add(clientNo);
                        ComboboxItem clientName = new ComboboxItem();
                        ComboboxItem clientSurName = new ComboboxItem();
                        clientName.ID = Convert.ToInt32(reader["organisationID"]);
                        clientName.Text = reader["name"].ToString();
                        clientSurName.Text = reader["surname"].ToString();
                        string fullName = clientSurName.Text + " " + clientName.Text;
                        recComboUnikNo.Items.Add(fullName);
                        UpdatecomboUnikNo.Items.Add(fullName);
                    }
                    reader.Close();
                }
            }
        }
        private void LoadCalls()
        {
            locComboUnikNo.Items.Clear();
            closeComboUnikNo.Items.Clear();
            using (SqlConnection conn = new SqlConnection(Database.connectionStr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM calls", conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ComboboxItem refNo = new ComboboxItem();
                        refNo.Text = reader["referenceNumber"].ToString();
                        locComboUnikNo.Items.Add(refNo);
                        closeComboUnikNo.Items.Add(refNo);
                    }
                    reader.Close();
                }
            }
        }
        private void LoadStaffUser()
        {
            using (SqlConnection conn = new SqlConnection(Database.connectionStr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM staffUser", conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ComboboxItem staffEmail = new ComboboxItem();
                        staffEmail.Text = reader["email"].ToString();
                        recComboAssignedTo.Items.Add(staffEmail);
                    }
                    reader.Close();
                }
            }
        }
        private void frmHome_Load(object sender, EventArgs e)
        {
            LoadOrganisation();
            LoadClient();
            LoadCalls();
            LoadStaffUser();
            regBtnClear.Enabled = false;
            regBtnSave.Enabled = false;
            regTxtAddress.Enabled = false;
            regTxtBuilding.Enabled = false;
            regTxtDeviveName.Enabled = false;
            regTxtEmail.Enabled = false;
            regTxtFloorNo.Enabled = false;
            regTxtModel.Enabled = false;
            regTxtName.Enabled = false;
            regTxtPhoneNo.Enabled = false;
            regTxtSerialNo.Enabled = false;
            regTxtSurname.Enabled = false;
            regTxtType.Enabled = false;
            regRadMale.Enabled = false;
            regRadFemale.Enabled = false;
            regTxtOrgEmail.Enabled = false;
        }
        
        private void button6_Click(object sender, EventArgs e)
        {
            string name = txtNewCompany.Text;
            string email = regTxtOrgEmail.Text;
            string password = txtNewCompany.Text + "123";

            if ((txtNewCompany.Text != "") || (regTxtOrgEmail.Text != ""))
            {
               
                    using (SqlConnection conn = new SqlConnection(Database.connectionStr))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand("INSERT INTO organisation(name,email,password) VALUES(@name,@email,@password) ", conn))

                        {
                            cmd.Parameters.AddWithValue("@name", name);
                            cmd.Parameters.AddWithValue("@email", email);
                            cmd.Parameters.AddWithValue("@password", password);
                            comboOrganisation.Text = txtNewCompany.Text;
                            cmd.ExecuteNonQuery();
                        }
                        conn.Close();
                    }

                    LoadOrganisation();
                    comboOrganisation.Text = txtNewCompany.Text;
                    MessageBox.Show("New Organisation succesfully saved!");
            }
            else
            {
                MessageBox.Show("Please fill in all the fields with the correct information");
            }
        }

        private void comboOrganisation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if((comboOrganisation.SelectedIndex == 0))
            {
                btnSaveNewCompany.Enabled = true;
                txtNewCompany.Enabled = true;
                regBtnClear.Enabled = true;
                regBtnSave.Enabled = true;
                regTxtAddress.Enabled = true;
                regTxtBuilding.Enabled = true;
                regTxtDeviveName.Enabled = true;
                regTxtEmail.Enabled = true;
                regTxtFloorNo.Enabled = true;
                regTxtModel.Enabled = true;
                regTxtName.Enabled = true;
                regTxtPhoneNo.Enabled = true;
                regTxtSerialNo.Enabled = true;
                regTxtSurname.Enabled = true;
                regTxtType.Enabled = true;
                regRadMale.Enabled = true;
                regRadFemale.Enabled = true;
                regTxtOrgEmail.Enabled = true;

            }
            else if((comboOrganisation.SelectedIndex != 0))
            {
                btnSaveNewCompany.Enabled =false;
                txtNewCompany.Enabled = false;
                regTxtOrgEmail.Enabled = false;
                regBtnClear.Enabled = true;
                regBtnSave.Enabled = true;
                regTxtAddress.Enabled = true;
                regTxtBuilding.Enabled = true;
                regTxtDeviveName.Enabled = true;
                regTxtEmail.Enabled = true;
                regTxtFloorNo.Enabled = true;
                regTxtModel.Enabled = true;
                regTxtName.Enabled = true;
                regTxtPhoneNo.Enabled = true;
                regTxtSerialNo.Enabled = true;
                regTxtSurname.Enabled = true;
                regTxtType.Enabled = true;
                regRadMale.Enabled = true;
                regRadFemale.Enabled = true;
            }
        }

        private void closeBtnSave_Click(object sender, EventArgs e)
        {
            string status;
            if(closeTxtMessage.Text=="")
            {
                MessageBox.Show("Please provide a message");
            }
            else
            {
                if (closeRadClose.Checked == true)
                {
                    status = "Closed";
                }
                else
                {
                    status = "Escalated";
                }
                string callStatus = status;
                string statusDescription = closeTxtMessage.Text;
                string callDateResolved;
                string refNum = closeComboUnikNo.Text;
                if (status=="Closed")
                {
                    callDateResolved = DateTime.Now.ToString();
                }
                else
                {
                    callDateResolved = "Still in progress";
                }
                using (SqlConnection conn = new SqlConnection(Database.connectionStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("update calls set status= @callStatus ,description=@statusDescription, dateResolved= @dateResolved where referenceNumber = '" + refNum + "'", conn))
                    {
                        
                        cmd.Parameters.AddWithValue("@callStatus", callStatus);
                        cmd.Parameters.AddWithValue("@dateResolved", callDateResolved);
                        cmd.Parameters.AddWithValue("@statusDescription", statusDescription);


                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Call sucessfully " + status + "!");
            }
        }

        private void updateBtnSearch_Click(object sender, EventArgs e)
        {
            string refNum = UpdatecomboUnikNo.Text;
            int i;
            if ((refNum.ToString().Length > 2))
            {
                if (int.TryParse(refNum, out i))
                {
                    int id = Convert.ToInt32(refNum);
                    using (SqlConnection conn = new SqlConnection(Database.connectionStr))
                    {
                        conn.Open();
                        try {
                            using (SqlCommand cmd = new SqlCommand("SELECT * from client where clientNumber = '" + id + "'", conn))
                            {
                                SqlDataReader reader = cmd.ExecuteReader();
                                reader.Read();
                                updateTxtName.Text = reader["name"].ToString();
                                updateTxtSurname.Text = reader["surname"].ToString();
                                updateTxtEmail.Text = reader["email"].ToString();
                                updateTxtContactNo.Text = reader["contactNumber"].ToString();
                                updateTxtAddress.Text = reader["address"].ToString();
                                updateTxtBuilding.Text = reader["building"].ToString();
                                updateTxtFloorNo.Text = reader["floorNo"].ToString();
                                string gender= reader["gender"].ToString();
                                if (gender == "M")
                                {
                                    updateRadMale.Checked = true;
                                }
                                else if(gender == "F")
                                {
                                    updateRadFemale.Checked = true;
                                }
                                reader.Close();
                            }
                            using (SqlCommand cmd2 = new SqlCommand("SELECT * from devices where deviceID ='" + id + "'", conn))
                            {
                                SqlDataReader reader2 = cmd2.ExecuteReader();
                                reader2.Read();
                                updateTxtDeviceName.Text = reader2["deviceName"].ToString();
                                updateTxtMake.Text = reader2["type"].ToString();
                                updateTxtModel.Text = reader2["model"].ToString();
                                updateTxtSerialNumber.Text = reader2["serialNumber"].ToString();

                                reader2.Close();
                            }
                        }
                        catch (InvalidOperationException)
                        {
                            MessageBox.Show("The number you entered does not exit in the database.Please Try again");
                        }
                    }
                }
                else
                {
                    string clientName = refNum;
                    string inputName = clientName.Substring(clientName.IndexOf(' ') + 1);
                    string userID = "";
                    using (SqlConnection conn = new SqlConnection(Database.connectionStr))
                    {
                        conn.Open();
                        try {
                            using (SqlCommand cmd = new SqlCommand("SELECT * from client where name = '" + inputName + "'", conn))
                            {
                                SqlDataReader reader = cmd.ExecuteReader();
                                reader.Read();
                                userID= reader["clientNumber"].ToString();
                                updateTxtName.Text = reader["name"].ToString();
                                updateTxtSurname.Text = reader["surname"].ToString();
                                updateTxtEmail.Text = reader["email"].ToString();
                                updateTxtContactNo.Text = reader["contactNumber"].ToString();
                                updateTxtAddress.Text = reader["address"].ToString();
                                updateTxtBuilding.Text = reader["building"].ToString();
                                updateTxtFloorNo.Text = reader["floorNo"].ToString();
                                string gender = reader["gender"].ToString();
                                if (gender == "M")
                                {
                                    updateRadMale.Checked = true;
                                }
                                else if (gender == "F")
                                {
                                    updateRadFemale.Checked = true;
                                }

                                reader.Close();

                            }
                            using (SqlCommand cmd2 = new SqlCommand("SELECT * from devices where deviceID ='" + userID + "'", conn))
                            {
                                SqlDataReader reader2 = cmd2.ExecuteReader();
                                reader2.Read();
                                updateTxtDeviceName.Text = reader2["deviceName"].ToString();
                                updateTxtMake.Text = reader2["type"].ToString();
                                updateTxtModel.Text = reader2["model"].ToString();
                                updateTxtSerialNumber.Text = reader2["serialNumber"].ToString();

                                reader2.Close();
                            }

                        }
                        catch (InvalidOperationException)
                        {
                            MessageBox.Show("The name you entered does not exit in the database.Please Try again");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid name or unique number!", "Error");
            }
        }

        private void locBtnShowAll_Click(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            using (SqlConnection conn = new SqlConnection(Database.connectionStr))
            {
                conn.Open();
                using (conn)
                {
                    SqlDataAdapter reader = new SqlDataAdapter("SELECT * from calls", conn);
                    DataSet ds = new DataSet();
                    reader.Fill(ds);
                    dataGridView1.VirtualMode = false;
                    dataGridView1.Columns.Clear();
                    dataGridView1.AutoGenerateColumns = true;
                    dataGridView1.DataSource = ds.Tables[0];
                    dataGridView1.Refresh();
                }
                try{

                    for (int rows = 0; rows < dataGridView1.Rows.Count; rows++)
                    {
                       string status=dataGridView1.Rows[rows].Cells[6].Value.ToString();
                        if (status == "Closed")
                        {
                            dataGridView1.Rows[rows].DefaultCellStyle.BackColor = Color.Yellow;
                        }
                        else if (status == "Escalated")
                        {
                            dataGridView1.Rows[rows].DefaultCellStyle.BackColor = Color.Red;
                        }
                        else if (status == "Opened")
                        {
                            dataGridView1.Rows[rows].DefaultCellStyle.BackColor = Color.Green;
                        }
                    }
                }
                   catch (Exception)
                {
                }
            }
        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
