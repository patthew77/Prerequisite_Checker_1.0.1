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



        /// <summary>
        /// N1. canTake  ****NEW parameter "sstudInfoDB added"
        /// </summary>
        /// <param name="subjName">(string) subject name</param>
        /// <param name="studName">(string) student name</param>
        /// <param name="curDbName">(string) database name of curriculum</param>
        /// <returns>2D string array  (actually string[4][?] ):
        /// output[0][] =  hard pre-reqs still needed
        /// output[1][] =  soft pre-reqs still needed
        /// output[2][] =  co-reqs still needed
        /// output[3][] =  any of the requisites not found in student info is placed here.
        /// </returns>
        string[][] canTake(string subjName, string studName, string curDbName, string studInfoDbName)
        {
            try
            {
                conn.Open();
                
                SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT [Hard Prerequisites],[Soft Prerequisites],[Co - requisites]"+
                    " FROM  ["+curDbName+ "]  WHERE [Course Code] = \'" + subjName + "\'", conn);
                DataTable datatable = new DataTable();
                dataAdapter.Fill(datatable);
                DataRowCollection dRows = datatable.Rows;
                //////
                if (dRows.Count >= 1)
                {
                    if(dRows.Count >= 2)
                    {
                        lblErr13.Text = "canTake--WARNING: "+ dRows.Count+ "entries found in SQL DB (w/c is >1).";
                    }
                    /////////////
                    List<string>[] sets = new List<string>[4];
                    sets[3] = new List<string>();   //init here, since not initialize in for loop later
                    /////
                    List<int>[] status = new List<int>[3];
                    //////////////////
                    //////////////////
                    string curTxt;
                    string[] curReqs;
                    DataTable dt;
                    lblErr13.Text = "";
                    bool isFound;
                    /////
                    for (int lsi=0; lsi<3; lsi++)
                    {
                        sets[lsi] = new List<string>(); //just to initialize object
                        status[lsi] = new List<int>(); //just to initialize object
                        ///
                        curTxt = dRows[0][lsi].ToString();      //assumes in DataRow retured:  0=hard,1=soft,2=co
                        curReqs = curTxt.Split(',');
                        /////
                        foreach (string req in curReqs)
                        {
                            isFound = false;
                            dt = new DataTable();
                            try
                            {
                                dataAdapter = new SqlDataAdapter("SELECT [" + req + "] FROM [" + studInfoDbName +
                                    "] WHERE   [Username] = " + studName, conn);
                                /////

                                dataAdapter.Fill(dt);
                                if (dt.Rows.Count >= 1)
                                {
                                    sets[lsi].Add(req);
                                    status[lsi].Add(Convert.ToInt32(dt.Rows[0][req].ToString()));
                                    // ^^ add the 1st result only
                                    isFound = true;
                                }
                            }
                            catch (SqlException sqle)
                            {
                                lblErr13.Text += "[" + req + "] not found in stud_info  |  ";
                                lblErr14.Text = sqle.ToString();
                            }
                            /////
                            if (!isFound)
                            {
                                sets[3].Add(req);
                            }
                        }
                        lblErr13.Text += " --||||-- ";
                    }
                    //TODO: status code  later. 

                    return new string[][] { sets[0].ToArray(), sets[1].ToArray(), sets[2].ToArray(), sets[3].ToArray() };
                } else
                {
                    lblErr11.Text = "canTake:  no entry found in SQL DB.";
                    lblErr12.Text = "-";
                    lblErr13.Text = "-";
                    lblErr14.Text = "-";
                    ////////////
                    return new string[][] { new string[] {}, new string[] {}, new string[] {}, new string[] { subjName } };                    
                }
                
            }
            catch (SqlException exception)
            {
                lblErr1.Text = "latest error from canTake:";
                lblErr2.Text = exception.ToString();
                return null;  //error
            }
            finally
            {
                conn.Close();
            }

            /*
            lblErr11.Text = "canTake - to be implemented later.";
            return new string[][] { new string[] { "1-1", "1-2" }, new string[] { "2-1", "2-2" }, new string[] { "3-1", "3-2" }, new string[] { "4-1", "4-2" }, new string[] { "5-1", "5-2" } };
            */
        }


        void updateDB(string studName, string curDbName, string studInfoDbName)
        {
            //code later
            lblErr11.Text = "updateDB - to be implemented later.";
        }


        /////////////////////////////////////////////////////////////////////
        // note: "lblErr1" and etc is just for debuggging purposes
        ///old batch 

        /// <summary>
        ///     returns db_name of curriculum of current student
        /// </summary>
        /// <returns>
        ///     string, the dbname itself
        /// </returns>
        public String getCurDBName()
        {
            try
            {
                conn.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT [Curriculum] FROM [login] WHERE [Username] = \'" + curUsername + "\'", conn);
                //SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM [login] WHERE [Username] = \'adminako\'", conn);
                DataTable datatable = new DataTable();
                dataAdapter.Fill(datatable);
                String cur_name = "";
                foreach (DataRow row in datatable.Rows)
                {
                    cur_name = row["Curriculum"].ToString();
                    break;
                }
                ////------------
                //2nd command
                ////------------
                dataAdapter = new SqlDataAdapter("SELECT [DB Table Name] FROM [curriculum_names] WHERE [Curriculum Name] = \'" + cur_name + "\'", conn);
                datatable = new DataTable();
                dataAdapter.Fill(datatable);
                String db_name = "";
                foreach (DataRow row in datatable.Rows)
                {
                    db_name = row["DB Table Name"].ToString();
                    break;
                }
                return db_name;
            }
            finally
            {
                conn.Close();
            }
        }


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
            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            SqlCommand load;
            DataTable dt;
            string commandText;


            try
            {
                conn.Open();
                commandText = "SELECT "+Subj+" " +
                                     "FROM samp_cur_student_info WHERE Username= \'"+ curUsername+"\'";
                load = new SqlCommand(commandText, conn);
                //////////////////load.Parameters.AddWithValue("@col_name", Subj);
                //////////////////load.Parameters.AddWithValue("@id", curUsername);


                dataAdapter.SelectCommand = load;
                dt = new DataTable();
                dataAdapter.Fill(dt);
                int output = -1;
                foreach (DataRow row in dt.Rows)
                {
                    String result = row[Subj].ToString();
                    Int32.TryParse(result, out output);
                }
                return output;
            }
            catch (SqlException exception)
            {
                lblErr1.Text = "latest error from getSubjStatus: [SQL Exception]";
                lblErr2.Text += "==========="+exception.ToString();
                
                //MessageBox.Show(exception.ToString());

                return -1;  //error
            }
            catch (Exception e)
            {
                lblErr1.Text = "latest error from getSubjStatus:";
                lblErr2.Text = e.ToString();
                //MessageBox.Show(exception.ToString());

                return -1;  //error
            }
            finally
            {
                conn.Close();
                //Label5.Text = "conn_closed";
            }
        }

        /// <summary>
        /// 2. getSubjReq
        /// </summary>
        /// <param name="Subj">
        ///     student subject
        /// </param>
        /// <returns>
        ///     A 3-element string[]. 1st is hard pre-req, 2nd is soft pre-req, 3rd is co-req.
        ///     Each subject in an element is comma separated (or "as is" as database)
        /// </returns>
        public string[] getSubjReq(string Subj)
        {
            try
            {
                String db_name = getCurDBName();
                conn.Open();
                ////------------
                //3rd command  (after getCurDBName() )
                ////------------
                SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT [Hard Prerequisites],[Soft Prerequisites],[Co-requisites] FROM " +
                    db_name + " WHERE [Course Code] = \'"+Subj+"\'", conn);
                DataTable datatable = new DataTable();
                dataAdapter.Fill(datatable);
                if (datatable.Rows.Count >= 1)
                {
                    var stringArr = datatable.Rows[0].ItemArray.Select(x => x.ToString()).ToArray();
                    return stringArr;
                } else
                {
                    lblErr1.Text = "latest error from getSubjReq:";
                    lblErr2.Text = "no data from SQL (0 rows)";
                    return null;
                }
            }
            catch (SqlException exception)
            {
                lblErr1.Text = "latest error from getSubjReq:";
                lblErr2.Text = exception.ToString();
                //MessageBox.Show(exception.ToString());

                return null;  //error
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 3. GetStudCur
        /// </summary>
        /// <returns>
        ///     DataTable of curriculum of current student
        /// </returns>
        public DataTable GetStudCur()
        {
            try
            {
                String db_name = getCurDBName();
                conn.Open();
                ////------------
                //3rd command  (after getCurDBName() )
                ////------------
                SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM "+db_name, conn);
                DataTable datatable = new DataTable();
                dataAdapter.Fill(datatable);
                return datatable;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 3b. GetStudCurArr
        /// </summary>
        /// <returns>
        ///     String[][] version (of #3) of curriculum of current student
        /// </returns>
        public string[][] GetStudCurArr()
        {
            DataTable datatable = GetStudCur();
            string[][] output = new string[datatable.Rows.Count][];
            int idx = 0;
            foreach (DataRow row in datatable.Rows)
            {
                var stringArr = datatable.Rows[idx].ItemArray.Select(x => x.ToString()).ToArray();
                output[idx] = stringArr;
                idx++;
            }
            //var stringArr = datatable.Rows[0].ItemArray.Select(x => x.ToString()).ToArray();
            return output;


            //return new string[][] { new string[] { "1-1", "1-2" }, stringArr };// new string[] { "2-1", "2-2" } };
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
            lbl3bRetVal.Text = outStr.ToString();
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
        
        /////// next batch

        protected void btnN1Exec_Click(object sender, EventArgs e)
        {
            string[][] output = canTake(tbN1Subj.Text, tbN1StudName.Text, tbN1CurDB.Text, tbN1StudInfoDbName.Text);
            if(output == null)
            {
                lblN1Rtn0.Text = "Error: NULL returned from Execute.";
                return;
            }
            /////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////
            StringBuilder outStr = new StringBuilder();
            for (int i = 0; i < output.Length; i++)
            {
                if (i < 4)
                {
                    outStr.Append("{");
                    foreach (string item in output[i])
                    {
                        outStr.Append(item).Append(",");
                    }
                    outStr.Length--;
                    outStr.Append("}");
                    switch (i)
                    {
                        case 0:
                            lblN1Rtn0.Text = outStr.ToString();
                            break;

                        case 1:
                            lblN1Rtn1.Text = outStr.ToString();
                            break;

                        case 2:
                            lblN1Rtn2.Text = outStr.ToString();
                            break;

                        case 3:
                            lblN1Rtn3.Text = outStr.ToString();
                            break;
                    }
                    outStr.Clear();
                }
                else  //i>=4
                {
                    if (i == 4)
                    {
                        outStr.Append("{{");
                    }
                    foreach (string item in output[i])
                    {
                        outStr.Append(item).Append(",");
                    }
                    outStr.Length--;
                    outStr.Append("},{");
                    ////////////
                    if(i==output.Length - 1)
                    {
                        outStr.Length = outStr.Length - 2;    //remove last character, by changing index position
                        outStr.Append("}");
                        lblN1RtnMore.Text = outStr.ToString();
                    }
                }
            }
            ////////////////////////////////////////////
            lblN1RtnArrSize.Text = "[" + output.Length.ToString() + ", (output[0]:) " + output[0].Length.ToString() + "]";
        }

        protected void btnN2Exec_Click(object sender, EventArgs e)
        {
            updateDB(tbN2StudName.Text, tbN2CurDB.Text, tbN2StudInfoDbName.Text);
            lblErr14.Text = "btnN2Exec_Click - Executed.";
        }

        protected void btnN2View_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM " + tbN2StudInfoDbName.Text, conn);
                DataTable datatable = new DataTable();
                dataAdapter.Fill(datatable);
                gvN2.DataSource = datatable;
                gvN2.DataBind();

                lblErr11.Text = "\"SUCCESS\" from btnN2View_Click.";
                lblErr12.Text = "-";
                lblCurTable.Text = tbTableName.Text;
                curTableName = tbTableName.Text;
            }
            catch (SqlException exception)
            {
                lblErr11.Text = "latest error from btnN2View_Click:";
                lblErr12.Text = exception.ToString();
                //MessageBox.Show(exception.ToString());
            }
            finally
            {
                conn.Close();
                //Label5.Text = "conn_closed";
            }
        }

        

    }
}