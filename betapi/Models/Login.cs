using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;
using System.Security.Cryptography;

/// <summary>
/// Summary description for clsMenu
/// </summary>
public class Login 
{
    usersObject usersItem = new usersObject();
    List<usersObject> usersList = new List<usersObject>();
    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

    public Login()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public int loginUser(string userName, string password)
    {
        bool isValid = true;
        int userId = 0;
        try
        {
            clsUsers userDAC = new clsUsers();
            usersObject usersObject = new usersObject();

            usersObject.username = userName;
            usersObject.password = password;

            if (userDAC.verifyUser(usersObject) == false)
                isValid = false;
            else
            {

                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandText = "SELECT ID FROM users WHERE USERNAME=@USERNAME AND STATUS='1'";
                sqlCmd.Connection = con;
                sqlCmd.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = usersObject.username;
                SqlDataReader dr = sqlCmd.ExecuteReader();
                if (dr.Read())
                {
                    userId = (int)dr["id"];
                }

                if (con.State == ConnectionState.Open)
                    con.Close();

            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
        return userId;
    }

}