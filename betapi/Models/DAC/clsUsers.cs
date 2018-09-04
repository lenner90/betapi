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
public class clsUsers : clsConnection
{
    usersObject usersItem = new usersObject();
    List<usersObject> usersList = new List<usersObject>();
    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

    public clsUsers()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public bool verifyUser(usersObject usersObject)
    {
        bool verify = false;
        onCon();

        try
        {
            //writeLog("verifyUser", "");
            //SqlCommand cmd = new SqlCommand("SELECT * FROM TBUSER U INNER JOIN TBCOMPANYMASTER C ON C.COMPANYCODE=U.COMPANYCODE AND C.STATUS='1' AND U.USERNAME=@USERNAME AND U.COMPANYCODE=@COMPANYCODE AND U.STATUS='1'", con);
            SqlCommand cmd = new SqlCommand("SELECT * FROM users WHERE USERNAME=@USERNAME AND STATUS='1'", con);
            cmd.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = usersObject.username;
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                usersObject.hashValue = (byte[])dr["password"];
                usersObject.salt = (byte[])dr["salt"];
                usersObject.iteration = int.Parse(dr["iteration"].ToString());
            }
            else
                return false;

            byte[] hashValue;
            hashValue = getHashValue(usersObject.password, usersObject.salt, usersObject.iteration);
            if (usersObject.hashValue.SequenceEqual(hashValue))
                verify = true;
            else
                verify = false;

        }
        catch (Exception ex)
        {
            writeErrorLog("clsUsers(verifyUser)", ex.Message.ToString());
        }


        offCon();
        return verify;
    }

    private static byte[] getHashValue(string password, byte[] salt, int iteration)
    {
        byte[] hashValue;
        int DerivedKeyLength = 24;

        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iteration);
        hashValue = pbkdf2.GetBytes(DerivedKeyLength);
        return hashValue;
    }

    public List<usersObject> listing(string search_param = "")
    {
        usersList = new List<usersObject>();
        onCon();
        try
        {
            SqlCommand cmd = new SqlCommand("select u.username,r.description as 'role_desc',u.status from users u inner join role r on u.role_id = r.id WHERE u.username LIKE '%" + search_param + "%' ORDER BY username", con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                usersItem.username = dr["username"].ToString();
                usersItem.role_desc = dr["role_desc"].ToString();                
                usersItem.status = (byte)dr["status"];

                usersList.Add(usersItem);
            }
        }
        catch (Exception ex)
        {
            writeErrorLog("clsUsers(listing)", ex.Message.ToString());
        }
        offCon();
        return usersList;
    }

    public bool insert(usersObject usersItem)
    {
        bool success = false;

        string password = usersItem.password;
        Random r = new Random();
        byte[] salt = GenerateRandomSalt();
        byte[] hashValue;
        int iterationCount = r.Next(10, 20);
        var valueToHash = string.IsNullOrEmpty(password) ? string.Empty : password;
        hashValue = getHashValue(valueToHash, salt, iterationCount);

        onCon();
        try
        {
            SqlCommand cmd = new SqlCommand("INSERT INTO users (username, password, salt,iteration,role_id,status,created_at, created_by, updated_at, updated_by)  VALUES (@username, @password,@salt,@iteration,@roleId, @status, @created_at, @created_by, @updated_at, @updated_by)", con);
            cmd.Parameters.AddWithValue("@username", usersItem.username);
            cmd.Parameters.AddWithValue("@password", hashValue);
            cmd.Parameters.AddWithValue("@salt", salt);
            cmd.Parameters.AddWithValue("@iteration", iterationCount);
            cmd.Parameters.AddWithValue("@roleId", usersItem.role_id);
            cmd.Parameters.AddWithValue("@status", usersItem.status);
            cmd.Parameters.AddWithValue("@created_at", usersItem.created_at);
            cmd.Parameters.AddWithValue("@created_by", usersItem.created_by);
            cmd.Parameters.AddWithValue("@updated_at", usersItem.updated_at);
            cmd.Parameters.AddWithValue("@updated_by", usersItem.updated_by);
            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            success = true;
        }
        catch (Exception ex)
        {
            writeErrorLog("clsUsers(insert)", ex.Message.ToString());
        }
        offCon();
        return success;
    }

    private static byte[] GenerateRandomSalt()
    {
        var csprng = new RNGCryptoServiceProvider();
        var salt = new byte[20];
        csprng.GetBytes(salt);
        return salt;
    }

    public bool update(usersObject usersItem)
    {
        bool success = false;
        onCon();
        try
        {
            Random r = new Random();
            byte[] salt = GenerateRandomSalt();
            byte[] hashValue;
            int iterationCount = r.Next(10, 20);
            var valueToHash = string.IsNullOrEmpty(usersItem.password) ? string.Empty : usersItem.password;
            hashValue = getHashValue(valueToHash, salt, iterationCount);

            string command = "UPDATE users SET " + (string.IsNullOrEmpty(usersItem.password)?"":("password=@password, salt=@salt, iteration=@iteration,")) + "role_id=@role_id, status=@status, updated_at=@updated_at, updated_by=@updated_by WHERE username=@username";
            SqlCommand cmd = new SqlCommand(command, con);
            cmd.Parameters.AddWithValue("@username", usersItem.username);
            if (!string.IsNullOrEmpty(usersItem.password))
            {
                cmd.Parameters.AddWithValue("@password", hashValue);
                cmd.Parameters.AddWithValue("@salt", salt);
                cmd.Parameters.AddWithValue("@iteration", iterationCount);
            }
            cmd.Parameters.AddWithValue("@role_id", usersItem.role_id);
            cmd.Parameters.AddWithValue("@status", usersItem.status);
            cmd.Parameters.AddWithValue("@updated_at", usersItem.updated_at);
            cmd.Parameters.AddWithValue("@updated_by", usersItem.updated_by);

            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            success = true;
        }
        catch (Exception ex)
        {
            writeErrorLog("clsUsers(update)", ex.Message.ToString());
        }
        offCon();
        return success;
    }



    public usersObject getDetails(string userName)
    {
        onCon();
        try
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM users WHERE username=@userName", con);
            cmd.Parameters.AddWithValue("@userName", userName);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                usersItem.id = (int)dr["id"];
                usersItem.username = dr["username"].ToString();
                usersItem.hashValue = (byte[])dr["password"];
                usersItem.salt = (byte[])dr["salt"];
                usersItem.iteration = (int)dr["iteration"];
                usersItem.role_id = (int)dr["role_id"];
                usersItem.status = (byte)dr["status"];
                usersItem.created_at = Convert.ToDateTime(dr["created_at"]);
                usersItem.created_by = (int)dr["created_by"];
                usersItem.updated_at = Convert.ToDateTime(dr["updated_at"]);
                usersItem.updated_by = (int)dr["updated_by"];
            }
        }
        catch (Exception ex)
        {
            writeErrorLog("clsUsers(getDetails)", ex.Message.ToString());
        }
        offCon();
        return usersItem;
    }

    public List<usersObject> getUsersListing()
    {
        onCon();
        try
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM users ", con);

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                usersItem.id = (int)dr["id"];
                usersItem.username = dr["username"].ToString();
                usersItem.hashValue = (byte[])dr["password"];
                usersItem.salt = (byte[])dr["salt"];
                usersItem.iteration = (int)dr["iteration"];
                usersItem.role_id = (int)dr["role_id"];
                usersItem.status = (byte)dr["status"];
                usersItem.created_at = Convert.ToDateTime(dr["created_at"]);
                usersItem.created_by = (int)dr["created_by"];
                usersItem.updated_at = Convert.ToDateTime(dr["updated_at"]);
                usersItem.updated_by = (int)dr["updated_by"];

                usersList.Add(usersItem);
            }
        }
        catch (Exception ex)
        {
            writeErrorLog("clsUsers(getUsersListing)", ex.Message.ToString());
        }
        offCon();
        return usersList;
    }

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
}