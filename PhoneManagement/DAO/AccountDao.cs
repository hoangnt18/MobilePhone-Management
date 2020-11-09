﻿using PhoneManagement.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneManagement.DAO
{
    public class AccountDao
    {
        private static AccountDao instance;

        public static AccountDao Instance
        {
            get
            {
                if (instance == null)
                    instance = new AccountDao();
                return AccountDao.instance;
            }
            private set
            {
                AccountDao.instance = value;
            }
        }
        private AccountDao()
        {

        }

        public bool Login(string userName, string passWord)
        {
            string query = "USP_Login @userName , @passWord";

            DataTable result = DataProvider.Instance.ExecuteQuery(query, new object[] { userName, passWord });

            return result.Rows.Count > 0;
        }

        public AccountDTO GetAccountByUserName(string userName)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery(" select * from dbo.account where userName = '" + userName + "'");
            foreach (DataRow item in data.Rows)
            {
                return new AccountDTO(item);
            }
            return null;
        }

        public bool UpdateAccount(string userName, string displayName, string passWord, string newPassword)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("exec USP_UpdateAccount  @userName , @displayName , @password , @newPassword ", new object[] { userName, displayName, passWord, newPassword });
            return result > 0; // Nếu như số dòng nó thay đổi thay đổi  >0 thì update 
        }

        public DataTable GetListAccount()
        {
            return DataProvider.Instance.ExecuteQuery("SELECT UserName, DisplayName, Type FROM dbo.Account");
        }

        public bool InsertAccount(string name, string displayName, int type)
        {
            string query = string.Format("INSERT dbo.Account ( UserName, DisplayName, Type )VALUES  ( N'{0}', N'{1}', {2})", name, displayName, type);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool UpdateAccount(string name, string displayName, int type)
        {
            string query = string.Format("UPDATE dbo.Account SET DisplayName = N'{1}', Type = {2} WHERE UserName = N'{0}'", name, displayName, type);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool DeleteAccount(string name)
        {
            string query = string.Format("Delete Account where UserName = N'{0}'", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool ResetPassword(string name)
        {
            string query = string.Format("update account set password = N'0' where UserName = N'{0}'", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

    }
}
