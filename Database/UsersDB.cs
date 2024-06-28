using AuthorizationFormWPF.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizationFormWPF.Database
{
    public class UsersDB : ICollection<User>, IDisposable
    {
        private const string _dbName = "users.db";
        private SQLiteConnection _connection;
        private SQLiteCommand _command;

        public UsersDB()
        {
            _connection = new SQLiteConnection("data source = " + _dbName);
            _connection.Open();

            _command = _connection.CreateCommand();

            _command.CommandText =
                @"CREATE TABLE IF NOT EXISTS registration_data(
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                login TEXT NOT NULL,
                password TEXT NOT NULL,
                registered_at DATETIME NOT NULL
                ); 
                CREATE TABLE IF NOT EXISTS personal_data (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                registration_data_id INTEGER REFERENCES registration_data(id),
                real_name TEXT NOT NULL,
                email TEXT,
                age INTEGER NOT NULL);
                ";

            _command.ExecuteNonQuery();

        }
        public int Count
        {
            get
            {
                _command.CommandText =
                    @"SELECT COUNT(*) AS count FROM registration_data";
                var reader = _command.ExecuteReader();
                reader.Read();
                int count = reader.GetInt32("count");
                reader.Close();

                return count;
            }

        }

        public bool IsReadOnly => false;


 

        public User? GetUserByUsername(string username)
        {
            _command.CommandText =
                @$"SELECT * FROM registration_data
                    JOIN personal_data ON registration_data.id = personal_data.registration_data_id
                    AND login = '{username}'";

            var reader = _command.ExecuteReader();
            if (!reader.Read())
            {
                reader.Close();
                return null;
            };

            User? result = new User();

            result.Id = reader.GetInt32("id");
            result.Username = reader.GetString("login");
            result.Password = reader.GetString("password");
            result.RegisteredAt = DateTime.ParseExact(reader.GetString("registered_at"), "yyyy-MM-dd HH:mm:ss", null);
            result.RealName = reader.GetString("real_name");
            result.Email = reader.GetString("email");
            result.Age = (uint)reader.GetInt32("Age");
        

            reader.Close();

            return result;


        }


        public void Add(User item)
        {
            _command.CommandText = 
                $@"INSERT INTO registration_data(login, password, registered_at) 
                    VALUES ('{item.Username}', '{item.Password}', '{item.RegisteredAt.ToString("yyyy-MM-dd HH:mm:ss")}');

                INSERT INTO personal_data(registration_data_id, real_name, email, age)
                    VALUES (last_insert_rowid(), '{item.RealName}', '{item.Email}', '{item.Age}');";

            if (_command.ExecuteNonQuery() == 0)
                throw new Exception("User adding error. No cars inserted to database");
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(User item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(User[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(User item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<User> GetEnumerator()
        {
            _command.CommandText =
    @"SELECT * FROM registration_data
                  JOIN personal_data ON registration_data.id = personal_data.registration_data_id";

            var reader = _command.ExecuteReader();

            while (reader.Read())
            {
                var result = new User();

                result.Id = reader.GetInt32("id");
                result.Username = reader.GetString("login");
                result.Password = reader.GetString("password");
                result.RegisteredAt = DateTime.ParseExact(reader.GetString("registered_at"), "yyyy-MM-dd HH:mm:ss", null);
                result.RealName = reader.GetString("real_name");
                result.Email = reader.GetString("email");
                result.Age = (uint)reader.GetInt32("Age");



                yield return result;
            }
            reader.Close();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

