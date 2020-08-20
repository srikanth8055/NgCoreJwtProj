using Dapper;
using DataLayer.Entity;
using LoggerFactory;
using Microsoft.Extensions.Configuration;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;

namespace DataLayer.Data
{

    public class UserData : IUserObj
    {
        public UserData() { }
        private readonly MyDbContext _dbcontext;
        public UserData(IConfiguration configuration, MyDbContext dbcontext)
        {
            Configuration = configuration;
            _dbcontext = dbcontext;
        }
        public IConfiguration Configuration { get; }
        public User GetUser(User user)
        {

            user = _dbcontext.tblUser.SingleOrDefault(x => x.name == user.name && x.password == user.password);
            if (user != null)
            {
                user.password = null;
            }
            else
            {
                user = null;
            }
            return user;

        }

        public User GetUser(int id)
        {
            try
            {
                return _dbcontext.tblUser.SingleOrDefault(x => x.id == id);
            }
            catch (Exception ex)
            {
                LoggerFactory.Log.LogException(ex.StackTrace);
                return null;
            }

        }

        public List<User> GetUsers()
        {
            try
            {
                return _dbcontext.tblUser.ToList();
            }
            catch (Exception ex)
            {
                LoggerFactory.Log.LogException(ex.StackTrace);
                return null;
            }
        }

        public User SaveUser(User user)
        {
            try
            {
                _dbcontext.tblUser.Add(user);
                _dbcontext.SaveChanges();
                return user;
            }
            catch (Exception ex)
            {
               Log.LogException(ex.StackTrace);
                return null;
            }
               
            
        }

        public void DeleteUser (User user)
        {
            try
            {
                _dbcontext.tblUser.Remove(user);
                _dbcontext.SaveChanges();
            }
            catch
            {

            }
        }
        public void UpdateUser(User user)
        {
            try
            {

                var data=  _dbcontext.tblUser.Update(user);


            }
            catch (Exception ex)
            {

                throw;
            }
        }
        //dapper code
        #region dapper

        public User ValidateUserDapper(User user)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("MyDbConnection")))
                {
                    var result = con.Query<User>("select id,name,role,mailid,isDeleted from tbluser where name=@name and password=@password", new { @name = user.name, @password = user.password }).FirstOrDefault();
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<User> GetUserswithDapper()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("MyDbConnection")))
                {
                    var data = con.QueryMultiple("GetUsers", commandType: CommandType.StoredProcedure);
                    List<User> user = new List<User>();
                    user = (List<User>)data.Read<User>();
                    return user;
                }
            }
            catch (Exception ex)
            {


                throw;
            }
        }

        public User GetUserByIdDapper(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("MyDbConnection")))
                {
                    var data = con.Query<User>("GetUserById", new { @id = id }, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    return data;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public int DeleteUserDapper(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("MyDbConnection")))
                {
                    var data = con.Execute("DeleteUserById", new { @id = id }, commandType: CommandType.StoredProcedure);

                    return data;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public User InsertUserDapper(User user)
        {
            try
            {

                using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("MyDbConnection")))
                {
                    var param = new DynamicParameters();
                    param.Add("@name", user.name);
                    param.Add("@role", user.role);
                    param.Add("@password", user.password);
                    param.Add("@mailId", user.mailId);
                    param.Add("@isDeleted", user.isDeleted);
                    var result = con.Query("InsertUser", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    var id = Convert.ToInt32(result.Identity);
                    user.id = id;
                    return user;

                }

            }
            catch (Exception ex)
            {
                Log.LogException(Convert.ToString(ex));
                throw;
            }
        }

        public int UpdateUserDapper(User user)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("MyDbConnection")))
                {
                    var param = new DynamicParameters();
                    param.Add("@id", user.id);
                    param.Add("@name", user.name);
                    param.Add("@password", user.password);
                    param.Add("@mailId", user.mailId);

                    return con.Execute("UpdateUser", param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

    }
}
