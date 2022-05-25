using System;

namespace BusinessEntities
{
    /// <summary>
    /// Business entity to store Customer record detail
    /// </summary>
    public class CustomerRecord
    {
        /// <summary>
        /// Customer number
        /// </summary>
        public string CustomerNumber { get; set; }
        /// <summary>
        /// Customer name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Customer email address
        /// </summary>
        public string EmailAddress { get; set; }
        /// <summary>
        /// Customer mobile number
        /// </summary>
        public Int64 MobileNumber { get; set; }
        /// <summary>
        /// Customer address
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Customer account number
        /// </summary>
        public Int64 AccountNumber { get; set; }
        /// <summary>
        /// Customer Account type
        /// </summary>
        public string AccountType { get; set; }
        /// <summary>
        /// Account opening date
        /// </summary>
        public DateTime AccountOpeningDate { get; set; }
        /// <summary>
        /// Account closing date
        /// </summary>
        public DateTime AccountClosingDate { get; set; }
    }
}
