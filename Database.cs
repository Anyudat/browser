using CefSharp.DevTools.Audits;
using CefSharp.Wpf;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace browser
{
    internal class Database
    {
        string conn_str;
        MySqlConnection connection;
        MySqlCommand command;

        public Database(string Server, string DB, string UID, string PWD)
        {
            conn_str = $"server={Server};database={DB};user={UID};password={PWD}";
        }

        public void insertData(string username, string hash)
        {
            using (connection = new MySqlConnection(conn_str))
            {
                connection.Open();
                using (command = new MySqlCommand($"INSERT INTO users(username, hashkey) SELECT @user, @hash WHERE NOT EXISTS(SELECT 1 FROM users WHERE username = @user);", connection))
                {
                    command.Parameters.AddWithValue("@user", username);
                    command.Parameters.AddWithValue("@hash", hash);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public bool userExists(string username, string hash)
        {
            bool exists = false;
            using (connection = new MySqlConnection(conn_str))
            {
                connection.Open();
                using (command = new MySqlCommand($"SELECT IF(EXISTS(SELECT username FROM users WHERE username = @user and hashkey LIKE @hash),1,0);", connection))
                {
                    command.Parameters.AddWithValue("@user", username);
                    command.Parameters.AddWithValue("@hash", hash);
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        exists = reader[0].ToString() == "1";
                    }
                }
                connection.Close();
            }
            return exists;
        }

        public void insertFavoriteTab(string userID, string url, string icon_url, string title)
        {
            using (connection = new MySqlConnection(conn_str))
            {
                connection.Open();
                using (command = new MySqlCommand($"insert into favourites(user_id, url, icon_url, title) values(@userID, @url, @icon_url, @title);", connection))
                {
                    command.Parameters.AddWithValue("@userID", userID);
                    command.Parameters.AddWithValue("@url", url);
                    command.Parameters.AddWithValue("@icon_url", icon_url);
                    command.Parameters.AddWithValue("@title", title);
                    command.ExecuteNonQuery();
                }
                connection.Close();

            }
        }

        public void deleteFavoriteTab(string userID, string url)
        {
            using (connection = new MySqlConnection(conn_str))
            {
                connection.Open();
                using (command = new MySqlCommand($"DELETE from favourites where user_id = @userId and url = @url", connection))
                {
                    command.Parameters.AddWithValue("@userId", userID);
                    command.Parameters.AddWithValue("@url", url);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public string getUserID(string username)
        {
            string userID="";
            using (connection = new MySqlConnection(conn_str))
            {
                connection.Open();
                using (command = new MySqlCommand($"select id from users where username = @user", connection))
                {
                    command.Parameters.AddWithValue("@user", username);
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        userID = reader.GetString("id");

                    }
                }
                connection.Close();
            }
            return userID;
        }

        public List<Dictionary<string,string>> selectUsersFavourites(string userID)
        {
            List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();
            using (connection = new MySqlConnection(conn_str))
            {
                connection.Open();
                using (command = new MySqlCommand($"select url, icon_url, title from favourites where user_id = @userID;", connection))
                {
                    command.Parameters.AddWithValue("@userID", userID);
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Dictionary<string,string> dict = new Dictionary<string, string>();
                        dict["url"] = reader.GetString("url");
                        dict["icon_url"] = reader.GetString("icon_url");
                        dict["title"] = reader.GetString("title");
                        data.Add(dict);


                    }
                }
                connection.Close();
            }
            return data;
        }


        public bool openConn()
        {
            try
            {
                connection = new MySqlConnection(conn_str);
                connection.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool closeConn()
        {
            try
            {
                if (connection is IDisposable)
                {
                    connection.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
