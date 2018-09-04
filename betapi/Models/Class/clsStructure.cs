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

public struct categoryObject
{
    public int id;
    public string name;
    public string description;
    public int status;
    public byte[] image;
}

public struct productObject
{
    public int id;
    public int parentId;
    public string name;
    public string description;
    public int status;
    public byte[] image;
    public Image image2;
    public double price;
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



