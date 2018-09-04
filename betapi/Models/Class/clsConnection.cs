using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

/// <summary>
/// Summary description for clsConnection
/// </summary>
public class clsConnection
{
    //string connectionString = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

    public clsConnection()
    {
        
    }

    //public SqlConnection getConn()
    //{
    //    con = new SqlConnection(connectionString);

    //    return con;
    //}

    public void onCon()
    {
        if (con.State == ConnectionState.Closed)
            con.Open();
    }

    public void offCon()
    {
        if (con.State == ConnectionState.Open)
            con.Close();
    }

    public void executeQuery(string query)
    {
        SqlCommand cmd = new SqlCommand(query, con);
        cmd.ExecuteNonQuery();
    }

    public void writeLog(string subject, string content, int user)
    {
        onCon();
        SqlCommand cmd = new SqlCommand("INSERT INTO audit_log (name,description,user_id,created_at) VALUES (@name,@description,@user_id, GETDATE())", con);
        cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = subject;
        cmd.Parameters.Add("@description", SqlDbType.VarChar).Value = content;
        cmd.Parameters.Add("@user_id", SqlDbType.VarChar).Value = user;
        cmd.ExecuteNonQuery();
        cmd.Parameters.Clear();
        offCon();
    }

    public void writeErrorLog(string subject, string content)
    {
        onCon();
        SqlCommand cmd = new SqlCommand("INSERT INTO audit_error_log (name,description,created_at) VALUES (@name,@description,GETDATE())", con);
        cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = subject;
        cmd.Parameters.Add("@description", SqlDbType.VarChar).Value = content;
        cmd.ExecuteNonQuery();
        cmd.Parameters.Clear();
        offCon();
    }
}