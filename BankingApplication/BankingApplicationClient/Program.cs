using BusinessEntities;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using Utilities;

namespace BankingApplication_Complete
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Hello. Do you want to Insert the customer data? Yes/No");
                string response = Console.ReadLine();
                if (response.ToLower()=="yes")
                {
                    var returnValue = ReadAndInsertCustomerData();

                    if(returnValue == true)
                    {
                        Console.WriteLine("Successfully read and inserted Customer data in database");
                    }
                }
                else
                {
                    Console.WriteLine("Do you want to View the customer data? Yes/No");
                    response = Console.ReadLine();
                    if (response.ToLower()=="yes")
                    {
                        Console.WriteLine("Please provide the name of the customer to view");
                        response = Console.ReadLine();
                        GetCustomerData(response);
                        Console.WriteLine("Thanks!");
                    }
                    else
                    {
                        Console.WriteLine("Thanks!");
                    }
                }
                Console.ReadKey();
            }
            catch (BankingAppException bankEx)
            {
                Console.WriteLine(string.Format("An exception has occured. Please try again later. Exception message - {0}", bankEx.Message));
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("An exception has occured. Please try again later. Exception message - {0}", ex.Message));
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Method to read the input file and insert the customer data into the database
        /// </summary>
        /// <returns>Boolean value if the operation was successful or not</returns>
        private static bool ReadAndInsertCustomerData()
        {
            string filePath = ConfigurationManager.AppSettings["CustomerInputFilePath"];
            
            List<CustomerRecord> customerRecords = new List<CustomerRecord>();
            CustomerRecord customerRecord = new CustomerRecord();

            IBankingDataAccess bankingDataAccess = new BankingDataAccess();
            try
            {
                using (var reader = new StreamReader(File.OpenRead(filePath)))
                {
                    while (!reader.EndOfStream)
                    {
                        customerRecord = new CustomerRecord();
                        var inputRecordLine = reader.ReadLine();
                        customerRecord = ValidateInputData(inputRecordLine);
                        customerRecords.Add(customerRecord);
                    }
                }

                if(customerRecords.Count > 0)
                {
                    foreach(CustomerRecord record in customerRecords)
                    {
                        bankingDataAccess.AddCustomerBankRecord(record);
                    }
                }
            }
            catch
            {
                throw;
            }

            return true;
        }

        /// <summary>
        /// Method to validate and return the data as per the business entity Customer record
        /// </summary>
        /// <param name="recordData">One line of data from input file</param>
        /// <returns>Input data converted to Customer record business entity</returns>
        private static CustomerRecord ValidateInputData(string recordData)
        {
            var values = recordData.Split(',');
            if (values.Length == 9 && (
                values[0].Equals(string.Empty) || values[1].Equals(string.Empty) ||
                values[2].Equals(string.Empty) || values[3].Equals(string.Empty) ||
                values[4].Equals(string.Empty) || values[5].Equals(string.Empty) ||
                values[6].Equals(string.Empty) || values[7].Equals(string.Empty)
                ))
            {
                throw new BankingAppException("Invalid data in a record. All fields except account closure date is mandatory. Please check for such data records.");
            }
            else if (!values[0].StartsWith("8ES"))
            {
                throw new BankingAppException(string.Format("Invalid data with name - {0}. Customer account number does not start with 8ES.", values[1]));
            }
            else if (values[3].Length != 10)
            {
                throw new BankingAppException(string.Format("Invalid data with name - {0}. Customer mobile number is not 10 digit long.", values[1]));
            }
            else if (!string.IsNullOrEmpty(values[8]) && values[8].Trim() != string.Empty)
            {
                if (DateTime.Parse(values[8]) < DateTime.Parse(values[7]))
                {
                    throw new BankingAppException(string.Format("Invalid data with name - {0}. Account opening date should be lesser than the closure date.", values[1]));
                }
            }

            CustomerRecord customerRecord = new CustomerRecord();

            customerRecord.CustomerNumber = values[0];
            customerRecord.Name = values[1];
            customerRecord.EmailAddress = values[2];
            customerRecord.MobileNumber = Int64.Parse(values[3]);
            customerRecord.Address = values[4];
            customerRecord.AccountNumber = Int64.Parse(values[5]);
            customerRecord.AccountType = values[6];
            //customerRecord.AccountOpeningDate = Convert.ToDateTime(values[7]);

            var dt = DateTime.ParseExact(values[7], "M/dd/yyyy", CultureInfo.InvariantCulture);
            customerRecord.AccountOpeningDate = Convert.ToDateTime(dt.ToString("yyyy-MM-dd"));

            return customerRecord;
        }

        /// <summary>
        /// Method to get the Customer data by the Customer name
        /// </summary>
        /// <param name="name">Customer name for which the data has to be fetched</param>
        private static void GetCustomerData(string name)
        {
            IBankingDataAccess bankingDataAccess = new BankingDataAccess();
            try
            {
                CustomerRecord customerRecord = bankingDataAccess.GetCustomerBankDetail(name);

                if (customerRecord != null)
                {
                    Console.WriteLine("Customer detail:");
                    Console.WriteLine(string.Format("Name: {0}", customerRecord.Name));
                    Console.WriteLine(string.Format("Customer number: {0}", customerRecord.CustomerNumber));
                    Console.WriteLine(string.Format("Mobile number: {0}", customerRecord.MobileNumber));
                    Console.WriteLine(string.Format("Email address: {0}", customerRecord.EmailAddress));
                    Console.WriteLine(string.Format("Address: {0}", customerRecord.Address));
                    Console.WriteLine(string.Format("Account number: {0}", customerRecord.AccountNumber));
                    Console.WriteLine(string.Format("Account type: {0}", customerRecord.AccountType));
                    Console.WriteLine(string.Format("Account opening date: {0}", customerRecord.AccountOpeningDate.ToShortDateString()));
                }
                else
                {
                    Console.WriteLine("Sorry, there is no customer information as per the data that you provided.");
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
