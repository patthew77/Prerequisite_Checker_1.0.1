using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Web.Services;
//Opening and closing SQL connection per request is recommended daw

namespace Prerequisite_Checker_1._0._1
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        static SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
        string curTableName = "account_types";
        string curUsername = "11578799";    //let login person handle session thingy

        // can be used by login person
        public void setCurrentUser(string username)
        {
            curUsername = username;
        }

        // can be used by login person. Also used by other functions here.
        public string getCurrentUsername()
        {
            return curUsername;
        }

        /////////////////////////////////////////////////////////////
        //note: "lblErr1" and etc is just for debuggging purposes

        // -1 returned if error  (under construction)
        /// <summary>
        /// 1. getSubjStatus
        /// </summary>
        /// <param name="Subj"></param>
        /// <returns>
        ///     int, based on sql
        ///     -1 if error
        /// </returns>
        public int getSubjStatus(string Subj)
        {
            return -1;
            try
            {
                conn.Open();
                SqlCommand dataGet = new SqlCommand("SELECT @col_name FROM Student_Info WHERE EntryID = @id", conn);
                //dataGet.Parameters.AddWithValue("@id", IDTB.Text);

                SqlDataReader answers = dataGet.ExecuteReader();
                if (answers.Read())
                {
                    //NameTB.Text = answers.GetValue(0).ToString();
                    //AddresssTB.Text = answers.GetValue(1).ToString();

                    //Label7.Text = "Success.";
                }
                else
                {
                    //Label7.Text = "ERROR: ID not found.";
                }

                ///////////////////////////////////////////
                lblErr1.Text = "\"SUCCESS\" from getSubjStatus.";
                lblErr2.Text = "-";
                lblCurTable.Text = tbTableName.Text;
                curTableName = tbTableName.Text;
            }
            catch (SqlException exception)
            {
                lblErr1.Text = "latest error from getSubjStatus:";
                lblErr2.Text = exception.ToString();
                //MessageBox.Show(exception.ToString());

                return -1;  //error
            }
            finally
            {
                conn.Close();
                //Label5.Text = "conn_closed";
            }
        }


        public string[] getSubjReq(string Subj)
        {
            return new string[] { "one", "two", "three" };
        }


        public DataTable GetStudCur()
        {
            try
            {
                conn.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter("select TABLE_NAME from information_schema.tables", conn);
                DataTable datatable = new DataTable();
                dataAdapter.Fill(datatable);
                return datatable;
                gvTableList.DataSource = datatable;
                gvTableList.DataBind();


                SqlDataAdapter dataAdapter2 = new SqlDataAdapter("select * from " + curTableName, conn);
                DataTable datatable2 = new DataTable();
                dataAdapter2.Fill(datatable2);
                gv1.DataSource = datatable2;
                gv1.DataBind();
                lblCurTable.Text = curTableName;
            }
            finally
            {
                conn.Close();
            }
        }


        public string[][] GetStudCurArr()
        {
            return new string[][] {new string[] { "1-1", "1-2" }, new string[] { "2-1", "2-2" } };
        }


        // returns -1 if error (username is not integer)
        /// <summary>
        /// 4. getStudNo
        /// </summary>
        /// <returns>
        ///     int: the student no
        ///     -1 if error (username is not integer)
        /// </returns>
        public int getStudNo()
        {
            int i = 0;
            if (!int.TryParse(getCurrentUsername(), out i))
            {
                i = -1;
            }
            return i;
        }

        // simply gets username
        /// <summary>
        /// 4b. getStudNoString
        /// </summary>
        /// <returns>
        ///     string: username (w/c is student id)
        /// </returns>
        public string getStudNoString()
        {
            return getCurrentUsername();
        }



        // returns "-" if error (sql)
        /// <summary>
        /// 5. getStudName
        /// </summary>
        /// <returns>
        ///     string: the student name
        ///     returns "-" if error (sql)
        /// </returns>
        public string getStudName()
        {
            try
            {
                conn.Open();
                SqlCommand dataGet = new SqlCommand("SELECT [Student Name] FROM [login] WHERE [Username]= @uname", conn);
                dataGet.Parameters.AddWithValue("@uname", getCurrentUsername());

                SqlDataReader answers = dataGet.ExecuteReader();
                if (answers.Read())
                {
                    string name = answers.GetValue(0).ToString();
                    ///////////////////////////////////////////
                    lblErr1.Text = "\"SUCCESS\" from getStudName.";
                    lblErr2.Text = "-";
                    ///////////////////////////////////////////
                    return name;
                }
                else
                {
                    lblErr1.Text = "latest error from getSubjStatus:";
                    lblErr2.Text = "ERROR: Username not found.";
                    //error
                    return "-";
                }
            }
            catch (SqlException exception)
            {
                lblErr1.Text = "latest error from getSubjStatus:";
                lblErr2.Text = exception.ToString();
                //error
                return "-";
            }
            finally
            {
                conn.Close();
            }
        }














        // /////////////////////////////////////////////////////////////////////////////////////////////////////////

        protected void Page_Load(object sender, EventArgs e)
        {
            lblCurUser.Text = curUsername;
            try
            {
                conn.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter("select TABLE_NAME from information_schema.tables", conn);
                DataTable datatable = new DataTable();
                dataAdapter.Fill(datatable);
                gvTableList.DataSource = datatable;
                gvTableList.DataBind();


                SqlDataAdapter dataAdapter2 = new SqlDataAdapter("select * from " + curTableName, conn);
                DataTable datatable2 = new DataTable();
                dataAdapter2.Fill(datatable2);
                gv1.DataSource = datatable2;
                gv1.DataBind();
                lblCurTable.Text = curTableName;
            }
            finally
            {
                conn.Close();
            }
        }

        protected void btnGetTable_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM " + tbTableName.Text, conn);
                DataTable datatable = new DataTable();
                dataAdapter.Fill(datatable);
                gv1.DataSource = datatable;
                gv1.DataBind();

                lblErr1.Text = "\"SUCCESS\" from btnGetTable_Click.";
                lblErr2.Text = "-";
                lblCurTable.Text = tbTableName.Text;
                curTableName = tbTableName.Text;
            }
            catch (SqlException exception)
            {
                lblErr1.Text = "latest error from btnGetTable_Click:";
                lblErr2.Text = exception.ToString();
                //MessageBox.Show(exception.ToString());
            }
            finally
            {
                conn.Close();
                //Label5.Text = "conn_closed";
            }
        }

        protected void btnChangeUser_Click(object sender, EventArgs e)
        {
            setCurrentUser(tbChangeUser.Text);
        }

        protected void btn1Exec_Click(object sender, EventArgs e)
        {
            lbl1Return.Text = getSubjStatus(tb1Subject.Text).ToString();
            //lbl1Meaning.Text = "text here"
        }

        protected void btn2Exec_Click(object sender, EventArgs e)
        {
            string[] output = getSubjReq(tb2Subj.Text);
            StringBuilder outStr = new StringBuilder();
            outStr.Append("{");
            foreach (string item in output)
            {
                outStr.Append(item).Append(",");
            }
            outStr.Length--;    //remove last character, by changing index position
            outStr.Append("}");
            lbl2RetVal.Text = outStr.ToString();
            lbl2ArraySize.Text = output.Length.ToString();
        }

        protected void btn3Exec_Click(object sender, EventArgs e)
        {
            gv3Table.DataSource = GetStudCur();
            gv3Table.DataBind();
        }

        protected void btn3bExec_Click(object sender, EventArgs e)
        {
            string[][] output = GetStudCurArr();
            StringBuilder outStr = new StringBuilder();
            outStr.Append("{{");
            foreach (string[] dimension in output)
            {
                foreach (string item in dimension)
                {
                    outStr.Append(item).Append(",");
                }
                outStr.Length--;
                outStr.Append("},{");
            }
            outStr.Length = outStr.Length - 2;    //remove last character, by changing index position
            outStr.Append("}");
            lbl2RetVal.Text = outStr.ToString();
            lbl3bArrSize.Text = "["+output.Length.ToString()+","+output[0].Length.ToString()+"]";
        }

        protected void btn4Exec_Click(object sender, EventArgs e)
        {
            lbl4StudNo.Text = getStudNo().ToString();
            lbl4StudNoStr.Text = getStudNoString();
        }

        protected void btn5Exec_Click(object sender, EventArgs e)
        {
            lbl5StudName.Text = getStudName();
        }
    }
}