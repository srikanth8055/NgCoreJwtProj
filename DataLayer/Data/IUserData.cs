using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Data
{
    public interface IUserData
    {

        //dapper code
        #region dapper

        User ValidateUserDapper(User user);

        List<User> GetUserswithDapper();

        User GetUserByIdDapper(int id);

        int DeleteUserDapper(int id);

        User InsertUserDapper(User user);

        int UpdateUserDapper(User user);


        #endregion

    }
}
