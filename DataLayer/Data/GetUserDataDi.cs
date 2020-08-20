using Dapper;
using Microsoft.Extensions.Configuration;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using LoggerFactory;
using Newtonsoft.Json;

namespace DataLayer.Data
{
  public  class GetUserDataDi : IGetUser
    {
        public GetUserDataDi(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public List<User> GetUsers()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("ConnectionString")))
                {
                    var data = con.QueryMultiple("GetUsers", commandType: CommandType.StoredProcedure);
                    List<User> user = new List<User>();
                    user = (List<User>)data.Read<User>();
                    return user;
                }
            }
            catch (Exception ex)
            {
              Log.LogException(ex.StackTrace);

                return null;
            }
        }
    }
    public interface IGetUser {
         List<User> GetUsers();
             
    }
}
