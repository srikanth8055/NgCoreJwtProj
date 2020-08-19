using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Data
{
  public  interface IUserObj
    {
        List<User> GetUsers();

        User ValidateUserDapper(User user);

        List<User> GetUserswithDapper();

        User GetUserByIdDapper(int id);

        int DeleteUserDapper(int id);

        User InsertUserDapper(User user);

        int UpdateUserDapper(User user);
    }
}
