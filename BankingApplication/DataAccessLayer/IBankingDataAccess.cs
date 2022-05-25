using BusinessEntities;

namespace DataAccessLayer
{
    /// <summary>
    /// Interface for all Banking transactions
    /// </summary>
    public interface IBankingDataAccess
    {
        bool AddCustomerBankRecord(CustomerRecord bankRecord);

        CustomerRecord GetCustomerBankDetail(string name);
    }
}
