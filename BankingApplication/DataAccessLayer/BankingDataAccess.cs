using BusinessEntities;
using System;
using System.Data;
using System.Data.SqlClient;
using Utilities;

namespace DataAccessLayer
{
    public class BankingDataAccess : IBankingDataAccess
    {
        #region SQL queries - Constants
        public const string ADD_CUSTOMER_BANK_RECORD = "insert into CustomerData (cust_number, cust_name, cust_email, cust_mobile_number, cust_address, cust_account_number, cust_account_type, cust_account_opening_date) values (@CustomerNumber, @CustomerName, @CustomerEmailAddress, @CustomerMobileNumber, @CustomerAddress, @AccountNumber, @AccountType, @AccountOpeningDate)";
        public const string GET_CUSTOMER_BANK_RECORD = "select cust_number, cust_name, cust_email, cust_mobile_number, cust_address, cust_account_number, cust_account_type, cust_account_opening_date from CustomerData where cust_name = @CustomerName";
        #endregion

                
        public CustomerRecord GetCustomerBankDetail(string name)
        {
            bool hasMatchingRecord = true;
            CustomerRecord customerRecord = new CustomerRecord();

            try
            {
                SqlConnection sqlConn = new SqlConnection(Helper.ConnectionString);
                SqlCommand cmd = new SqlCommand
                {
                    Connection = sqlConn,
                    CommandType = CommandType.Text,
                    CommandText = GET_CUSTOMER_BANK_RECORD
                };

                using (sqlConn)
                {
                    cmd.Prepare();

                    SqlParameter customerName = new SqlParameter("@CustomerName", SqlDbType.VarChar);
                    customerName.Value = name;
                    customerName.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(customerName);

                    sqlConn.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                       
                        while (dataReader.Read())
                        {
                            customerRecord.Name = name;
                            customerRecord.CustomerNumber = Convert.ToString(dataReader.GetValue(dataReader.GetOrdinal("cust_number")));
                            customerRecord.EmailAddress = Convert.ToString(dataReader.GetValue(dataReader.GetOrdinal("cust_email")));
                            customerRecord.MobileNumber = Convert.ToInt64(dataReader.GetValue(dataReader.GetOrdinal("cust_mobile_number")));
                            customerRecord.Address = Convert.ToString(dataReader.GetValue(dataReader.GetOrdinal("cust_address")));
                            customerRecord.AccountNumber = Convert.ToInt64(dataReader.GetValue(dataReader.GetOrdinal("cust_account_number")));
                            customerRecord.AccountType = Convert.ToString(dataReader.GetValue(dataReader.GetOrdinal("cust_account_type")));
                            customerRecord.AccountOpeningDate = Convert.ToDateTime(dataReader.GetValue(dataReader.GetOrdinal("cust_account_opening_date")));
                        }
                    }

                    dataReader.Close();
                }

                if (hasMatchingRecord == true)
                    return customerRecord;
                else
                    return null;
            }
            catch(SqlException sqlEx)
            {
                throw new BankingAppException(sqlEx.Message);
            }
        }

        public bool AddCustomerBankRecord(CustomerRecord customerRecord)
        {
            try
            {
                SqlConnection sqlConn = new SqlConnection(Helper.ConnectionString);
                SqlCommand cmd = new SqlCommand
                {
                    Connection = sqlConn,
                    CommandType = CommandType.Text,
                    CommandText = ADD_CUSTOMER_BANK_RECORD
                };

                SqlParameter customerNumber = new SqlParameter("@CustomerNumber", SqlDbType.VarChar);
                SqlParameter customerName = new SqlParameter("@CustomerName", SqlDbType.VarChar);
                SqlParameter customerEmailAddress = new SqlParameter("@CustomerEmailAddress", SqlDbType.VarChar);
                SqlParameter customerMobileNumber = new SqlParameter("@CustomerMobileNumber", SqlDbType.BigInt);
                SqlParameter customerAddress = new SqlParameter("@CustomerAddress", SqlDbType.VarChar);
                SqlParameter accountNumber = new SqlParameter("@AccountNumber", SqlDbType.BigInt);
                SqlParameter accountType = new SqlParameter("@AccountType", SqlDbType.VarChar);
                SqlParameter accountOpeningDate = new SqlParameter("@AccountOpeningDate", SqlDbType.Date);

                customerNumber.Value = customerRecord.CustomerNumber;
                customerName.Value = customerRecord.Name;
                customerEmailAddress.Value = customerRecord.EmailAddress;
                customerMobileNumber.Value = customerRecord.MobileNumber;
                customerAddress.Value = customerRecord.Address;
                accountNumber.Value = customerRecord.AccountNumber;
                accountType.Value = customerRecord.AccountType;
                accountOpeningDate.Value = customerRecord.AccountOpeningDate;

                cmd.Parameters.Add(customerNumber);
                cmd.Parameters.Add(customerName);
                cmd.Parameters.Add(customerEmailAddress);
                cmd.Parameters.Add(customerMobileNumber);
                cmd.Parameters.Add(customerAddress);
                cmd.Parameters.Add(accountNumber);
                cmd.Parameters.Add(accountType);
                cmd.Parameters.Add(accountOpeningDate);

                using (sqlConn)
                {
                    sqlConn.Open();
                    int result = cmd.ExecuteNonQuery();
                }
            }
            catch(SqlException sqlEx)
            {
                throw new BankingAppException(sqlEx.Message);
            }
            return true;
        }
    }
}
