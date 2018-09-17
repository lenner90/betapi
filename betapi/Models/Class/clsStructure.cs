using System;
using System.Drawing;

public struct usersObject
{
    public int id;
    public string username;
    public string password;
    public byte[] salt;
    public byte[] hashValue;
    public int iteration;
    public int role_id;
    public string role_desc;
    public byte status;
    public DateTime created_at;
    public int created_by;
    public DateTime updated_at;
    public int updated_by;
}

public struct betObject
{
    public int id;
    public string batch_id;
    public int agent_id;
    public string bet_type;
    public int bet_number;
    public int up;
    public int down;
    public DateTime created_date;
    public int created_by;
    public DateTime updated_date;
    public int updated_by;
}


[Serializable]
public struct cartObject
{
    public int id;
    public int userId;
    public int itemId;
    public string itemName;
    public byte[] image;
    public int qty;
    public double unit_price;    
    public double total_price;
    public double shipping_fee;
    public DateTime created_at;
    public DateTime updated_at;
    public int status;
    public int transId;
}



