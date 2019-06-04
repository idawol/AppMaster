using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using Mono.Data.Sqlite;
using System;
using System.IO;

public class SQLiteController : MonoBehaviour {
    private String Path = "masterDB.db";

    private void Start()
    {
        LoadDB();
        if (!File.Exists(Application.persistentDataPath + Path))
        {
            LoadDB();
        }
    }

    public void LoadDB()
    {
        WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/" + Path);
        while (!loadDB.isDone) { }
        File.WriteAllBytes(Application.persistentDataPath + Path, loadDB.bytes);
    }

    public bool GetUser(string username, out int gender, out int id)
    {
        string cs = "URI=file:" + Application.persistentDataPath + Path;
        IDbConnection dbcon = new SqliteConnection(cs);
        dbcon.Open();

        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT * FROM users WHERE username = '"+username.ToLower()+"';";
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();

        while (reader.Read())
        {
            gender = (int)reader[2];
            id = (int)reader[0];
            dbcon.Close();
            return true;
        }
        dbcon.Close();
        gender = 0;
        id = 0;
        return false;
    }

    public bool GetUser(int id, out string username, out int gender)
    {
        string cs = "URI=file:" + Application.persistentDataPath + Path;
        IDbConnection dbcon = new SqliteConnection(cs);
        dbcon.Open();

        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT * FROM users WHERE id = "+id+";";
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();

        while (reader.Read())
        {
            gender = (int)reader[2];
            username = reader[1].ToString();
            dbcon.Close();
            return true;
        }
        dbcon.Close();
        gender = 0;
        username = null;
        return false;
    }

    public bool CreateUser(int id, string username, int gender)
    {
        string cs = "URI=file:" + Application.persistentDataPath + Path;
        IDbConnection dbcon = new SqliteConnection(cs);
        dbcon.Open();

        IDbCommand cmnd = dbcon.CreateCommand();
        cmnd.CommandText = "INSERT INTO users (id, username, gender) VALUES ("+id+", '"+username+"',"+gender+");";
        try
        {
            cmnd.ExecuteNonQuery();
            dbcon.Close();
            return true;
        }
        catch (Exception ex)
        {
            Debug.Log("db unable to update. " + ex.ToString());
            dbcon.Close();
            return false;
        }
    }

    public bool SaveUser(int id, string username, int gender)
    {
        string cs = "URI=file:" + Application.persistentDataPath + Path;
        IDbConnection dbcon = new SqliteConnection(cs);
        dbcon.Open();

        IDbCommand cmnd = dbcon.CreateCommand();
        cmnd.CommandText = "UPDATE users SET username ='" + username.ToLower() + "', gender =" + gender + " WHERE id="+id+";";
        try
        {
            cmnd.ExecuteNonQuery();
            dbcon.Close();
            return true;
        }
        catch (Exception ex)
        {
            Debug.Log("db unable to update. " + ex.ToString());
            dbcon.Close();
            return false;
        }
    }

    public bool GetTextAndImageScreen(int id, out List<string> images, out string largeImage, out string sound, out string text)
    {
        string cs = "URI=file:" + Application.persistentDataPath + Path;
        IDbConnection dbcon = new SqliteConnection(cs);
        dbcon.Open();

        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT text, pictureLarge, sound, picture1, picture2 FROM textAndImage WHERE id = "+id+";";
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();
        while (reader.Read())
        {
            text = reader[0].ToString();
            largeImage = reader[1].ToString();
            sound = reader[2].ToString();
            images = new List<string>();

            for (int i = 0; i < 2; i++)
            {
                if (!reader[i+3].ToString().Equals(""))
                {
                    images.Add(reader[i + 3].ToString());
                }
            }
            dbcon.Close();
            return true;
        }
        dbcon.Close();
        text = null;
        images = null;
        largeImage = null;
        sound = null;
        return false;
    }

    public bool GetTextScreen(int id, out List<string> images, out string sound, out string text)
    {
        string cs = "URI=file:" + Application.persistentDataPath + Path;
        IDbConnection dbcon = new SqliteConnection(cs);
        dbcon.Open();

        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT text, sound, picture1, picture2, picture3 FROM text WHERE id = " + id + ";";
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();
        while (reader.Read())
        {
            text = reader[0].ToString();
            sound = reader[1].ToString();
            images = new List<string>();

            for (int i = 0; i < 3; i++)
            {
                if (!reader[i + 2].ToString().Equals(""))
                {
                    images.Add(reader[i + 2].ToString());
                }
            }
            dbcon.Close();
            return true;
        }
        dbcon.Close();
        text = null;
        images = null;
        sound = null;
        return false;
    }

    public bool GetQuestionScreen(int id, out List<string> choices, out int correctAnswer, out string text, out List<string> images, out string sound)
    {
        string cs = "URI=file:" + Application.persistentDataPath + Path;
        IDbConnection dbcon = new SqliteConnection(cs);
        dbcon.Open();

        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT question, correct, choice1, choice2, choice3, image1, image2, image3, sound FROM question WHERE id = " + id + ";";
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();
        while (reader.Read())
        {
            text = reader[0].ToString();
            correctAnswer = (int)reader[1];
            choices = new List<string>();

            for (int i = 0; i < 3; i++)
            {
                if (!reader[i + 2].ToString().Equals(""))
                {
                    choices.Add(reader[i + 2].ToString());
                }
            }

            images = new List<string>();
            for (int i = 0; i < choices.Count; i++)
            {
                string image = reader[i + 5].ToString();
                if (!reader[i + 5].ToString().Equals(""))
                {
                    
                    images.Add(image);
                }
            }
            sound = reader[8].ToString();
            dbcon.Close();
            return true;
        }
        dbcon.Close();
        text = null;
        choices = null;
        correctAnswer = 0;
        images = null;
        sound = null;
        return false;
    }

    public int GetScreenType(int id)
    {
        string cs = "URI=file:" + Application.persistentDataPath + Path;
        IDbConnection dbcon = new SqliteConnection(cs);
        dbcon.Open();

        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT type FROM screens WHERE id=" + id;
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();

        int type = (int)reader.GetValue(0);
        dbcon.Close();
        return type;
    }

    public bool isAvailableUsername(string username)
    {
        string cs = "URI=file:" + Application.persistentDataPath + Path;
        IDbConnection dbcon = new SqliteConnection(cs);
        dbcon.Open();

        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT * FROM users WHERE username = '"+username+"';";
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();

        if (reader.Read())
        {
            dbcon.Close();
            return false;
        }
        dbcon.Close();
        return true;
    }

    public int NextID (string table)
    {
        string cs = "URI=file:" + Application.persistentDataPath + Path;
        IDbConnection dbcon = new SqliteConnection(cs);
        dbcon.Open();

        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;
        int max = 0;
        string query = "SELECT id FROM "+table+";";
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();

        while (reader.Read())
        {
            if ((int)reader[0]>max)
            {
                max = (int)reader[0];
            }
        }

        dbcon.Close();
        return max+1;
    }

    public bool SavePicture(int id, int userId, int gameId, string picture)
    {
        string cs = "URI=file:" + Application.persistentDataPath + Path;
        IDbConnection dbcon = new SqliteConnection(cs);
        dbcon.Open();

        IDbCommand cmnd = dbcon.CreateCommand();
        cmnd.CommandText = "INSERT INTO pictures (id, userId, gameId, picture) VALUES (" + id + ", " + userId + ", " + gameId +", '"+ picture +"');";
        Debug.Log("mastergame INSERT INTO pictures(id, userId, gameId, picture) VALUES(" + id + ", " + userId + ", " + gameId + ", '" + picture + "'); ");
        try
        {
            cmnd.ExecuteNonQuery();
            dbcon.Close();
            return true;
        }
        catch (Exception ex)
        {
            Debug.Log("db unable to update. " + ex.ToString());
            dbcon.Close();
            return false;
        }
    }

    public string GetPicture(int id) //TODO
    {
        string cs = "URI=file:" + Application.persistentDataPath + Path;
        IDbConnection dbcon = new SqliteConnection(cs);
        dbcon.Open();

        IDbCommand cmnd_read = dbcon.CreateCommand();
        string query = "SELECT * FROM pictures WHERE id=" + id + ";";
        Debug.Log("load " + query);
        cmnd_read.CommandText = query;
        IDataReader reader = cmnd_read.ExecuteReader();
        while (reader.Read())
        {
            return reader[3].ToString();
        }
        return null;
    }

    public bool getMedals(int userId, out List<string> names, out List<string> images)
    {
        string cs = "URI=file:" + Application.persistentDataPath + Path;
        IDbConnection dbcon = new SqliteConnection(cs);
        dbcon.Open();
        names = new List<string>();
        images = new List<string>();

        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT name, image FROM medals WHERE userId = " + userId + ";";
        Debug.Log("mastergame " + query);
        cmnd_read.CommandText = query;
        try
        {
            reader = cmnd_read.ExecuteReader();
            Debug.Log("mastergame execute");
            while (reader.Read())
            {
                names.Add(reader[0].ToString());
                images.Add(reader[1].ToString());
            }
            Debug.Log("mastergame " + names[0] + " " + images[0]);
            if (names.Count > 0)
            {
                return true;
            }
            dbcon.Close();
        }
        catch (Exception ex)
        {
            Debug.Log("mastergame get medals " + ex);
        }
        dbcon.Close();
        return false;
    }

    public bool SaveMedal(int Id, int userId, string storyName, string image)
    {
        string cs = "URI=file:" + Application.persistentDataPath + Path;
        IDbConnection dbcon = new SqliteConnection(cs);
        dbcon.Open();

        IDbCommand cmnd = dbcon.CreateCommand();
        cmnd.CommandText = "INSERT INTO medals (id, userId, name, image) VALUES (" + Id + ", " + userId + ", '" + storyName +"', '"+ image +"');" ;
        Debug.Log("mastergame INSERT INTO medals (id, userId, name, image) VALUES (" + Id + ", " + userId + ", '" + storyName + "', '" + image + "');");
        try
        {
            cmnd.ExecuteNonQuery();
            Debug.Log("mastergame executed query");
            dbcon.Close();
            return true;
        }
        catch (Exception ex)
        {
            Debug.Log("db unable to update. " + ex.ToString());
            dbcon.Close();
            return false;
        }
    }

    public int getDirection(int id, out string picture, out double lat, out double lon)
    {
        string cs = "URI=file:" + Application.persistentDataPath + Path;
        IDbConnection dbcon = new SqliteConnection(cs);
        dbcon.Open();

        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT * FROM navigation WHERE id = " + id + ";";
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();

        while (reader.Read())
        {
            Debug.Log("MasterGame reding");
            picture = reader[2].ToString();
            int direction = (int)reader[1];
            string lats = reader[3].ToString();
            string lons = reader[4].ToString();
            lat = double.Parse(lats);
            lon = double.Parse(lons);
            dbcon.Close();
            return direction;
        }
        dbcon.Close();
        picture = null;
        lat = 0;
        lon = 0;
        return 0;
    }
}
