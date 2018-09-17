using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;
using System.Security.Cryptography;
using System.Drawing;
using System.IO;
using System.Globalization;

/// <summary>
/// Summary description for clsMenu
/// </summary>
public class clsBet : clsConnection
{
    betObject betItem = new betObject();
    List<betObject> betList = new List<betObject>();
    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

    public clsBet()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public betObject getBetListById(int id)
    {
        bool verify = false;
        onCon();

        betObject betItem = new betObject();

        try
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM bet where id = @id ", con);
            cmd.Parameters.AddWithValue("@id", id);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                betItem.id = (int)dr["id"];
                betItem.agent_id = (int)dr["agent_id"];
                betItem.bet_type = dr["bet_type"].ToString();
                betItem.bet_number = (int)dr["bet_number"];
                betItem.up = (int)dr["up"];
                betItem.down = (int)dr["down"];
                //betItem.created_date = DateTime.ParseExact(dr["created_date"].ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                //betItem.created_by = (int)dr["created_by"];
                //betItem.updated_date = DateTime.ParseExact(dr["updated_date"].ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                //betItem.updated_by = (int)dr["updated_by"];
            }
        }
        catch (Exception ex)
        {
            writeErrorLog("clsUsers(verifyUser)", ex.Message.ToString());
        }


        offCon();
        return betItem;
    }

    public List<betObject> getBetList(string type,int id)
    {
        bool verify = false;
        onCon();

        betObject betItem = new betObject();

        try
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM bet where bet_type = @type and agent_id = @id order by id desc", con);
            cmd.Parameters.AddWithValue("@type", type);
            cmd.Parameters.AddWithValue("@id", id);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                betItem.id = (int)dr["id"];
                betItem.agent_id = (int)dr["agent_id"];
                betItem.bet_type = dr["bet_type"].ToString();
                betItem.bet_number = (int)dr["bet_number"];
                betItem.up = (int)dr["up"];
                betItem.down = (int)dr["down"];
                //betItem.created_date = DateTime.ParseExact(dr["created_date"].ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                //betItem.created_by = (int)dr["created_by"];
                //betItem.updated_date = DateTime.ParseExact(dr["updated_date"].ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                //betItem.updated_by = (int)dr["updated_by"];

                betList.Add(betItem);
            }
        }
        catch (Exception ex)
        {
            writeErrorLog("clsUsers(verifyUser)", ex.Message.ToString());
        }


        offCon();
        return betList;
    }

    public List<betObject> GetBetListByBatch(string batchId)
    {
        bool verify = false;
        onCon();

        betObject betItem = new betObject();

        try
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM bet where batch_id = @batch_id order by id desc", con);
            cmd.Parameters.AddWithValue("@batch_id", batchId);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                betItem.id = (int)dr["id"];
                betItem.agent_id = (int)dr["agent_id"];
                betItem.bet_type = dr["bet_type"].ToString();
                betItem.bet_number = (int)dr["bet_number"];
                betItem.up = (int)dr["up"];
                betItem.down = (int)dr["down"];
                //betItem.created_date = DateTime.ParseExact(dr["created_date"].ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                //betItem.created_by = (int)dr["created_by"];
                //betItem.updated_date = DateTime.ParseExact(dr["updated_date"].ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                //betItem.updated_by = (int)dr["updated_by"];

                betList.Add(betItem);
            }
        }
        catch (Exception ex)
        {
            writeErrorLog("clsUsers(verifyUser)", ex.Message.ToString());
        }


        offCon();
        return betList;
    }

    public string getUid()
    {
        bool verify = false;
        onCon();
        string uid = string.Empty;

        try
        {


            SqlCommand cmd = new SqlCommand("SELECT NEWID() as uid", con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                uid = dr["uid"].ToString();
            }
        }
        catch (Exception ex)
        {
            writeErrorLog("clsUsers(verifyUser)", ex.Message.ToString());
        }


        offCon();
        return uid;
    }

    public bool insert(betObject bet)
    {
        bool success = false;
        onCon();
        try
        {

            SqlCommand cmd = new SqlCommand("INSERT INTO bet (batch_id,agent_id,bet_type,bet_number,up,down,created_date,created_by,updated_date,updated_by) VALUES" +
                                              " (@batch_id,@agent_id,@bet_type,@bet_number,@up,@down,@created_date,@created_by,@updated_date,@updated_by)", con);
            cmd.Parameters.AddWithValue("@batch_id", bet.batch_id);
            cmd.Parameters.AddWithValue("@agent_id", bet.agent_id);
            cmd.Parameters.AddWithValue("@bet_type", bet.bet_type);
            cmd.Parameters.AddWithValue("@bet_number", bet.bet_number);
            cmd.Parameters.AddWithValue("@up", bet.up);
            cmd.Parameters.AddWithValue("@down", bet.down);
            cmd.Parameters.AddWithValue("@created_date", bet.created_date);
            cmd.Parameters.AddWithValue("@created_by", bet.created_by);
            cmd.Parameters.AddWithValue("@updated_date", bet.updated_date);
            cmd.Parameters.AddWithValue("@updated_by", bet.updated_by);
            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            success = true;
        }
        catch (Exception ex)
        {
            writeErrorLog("clsCart(insert)", ex.Message.ToString());
        }
        offCon();
        return success;
    }

    public bool update(int id, int num, int up , int down)
    {
        bool success = false;
        onCon();
        try
        {
            SqlCommand cmd = new SqlCommand("UPDATE bet SET bet_number=@bet_number,up=@up, down=@down,updated_date=@updated_date WHERE id=@id", con);
            cmd.Parameters.AddWithValue("@bet_number", num);
            cmd.Parameters.AddWithValue("@up", up);
            cmd.Parameters.AddWithValue("@down", down);
            cmd.Parameters.AddWithValue("@updated_date", DateTime.Now);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            success = true;
        }
        catch (Exception ex)
        {
            writeErrorLog("editPaymentMethod", ex.Message.ToString());
        }
        offCon();
        return success;
    }

    public bool delete(int id)
    {
        bool success = false;
        onCon();
        try
        {

            SqlCommand cmd = new SqlCommand("Delete from bet where id =@id", con);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            success = true;
        }
        catch (Exception ex)
        {
            writeErrorLog("clsCart(insert)", ex.Message.ToString());
        }
        offCon();
        return success;
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