//using Agrithmics.AgriGenERP.Core.Models;
//using Agrithmics.Core.Data;
//using Dapper;
//using NLog.Web;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//namespace Agrithmics.AgriGenERP.Dapper
//{
//    public static class AccountingCustomerRepository
//    {
//        public static async Task<int> SaveAccountingCustomer(this IRepositoryAsync<AccountingCustomerModel> repo, AccountingCustomerModel model)
//        {
//            var errorLog = NLogBuilder.ConfigureNLog("nlog.config").GetLogger("ErrorLog");
//            DynamicParameters dynamicParametersCustomerDetailsr = MapToCustomerDetails(model);

//            using (var connection = repo.GetConnectionFactory().GetConnection())
//            {
//                connection.Open();
//                using (var transaction = connection.BeginTransaction())
//                {
//                    try
//                    {
//                        await connection.QueryAsync<AccountingCustomerModel>("[Accounting].[SaveAccountingCustomerDetails]",
//                        param: dynamicParametersCustomerDetailsr,
//                        transaction: transaction,
//                        commandType: CommandType.StoredProcedure);

//                        var CustomerID = dynamicParametersCustomerDetailsr.Get<int>("CustomerID");

//                        if (CustomerID < 0)
//                        {
//                            transaction.Rollback();
//                            return CustomerID;
//                        }
//                        else
//                        {

//                            foreach (var item in model.customerPaymentModel)
//                            {
//                                DynamicParameters dynamicParametersCustomerPayments = MapToFactoryItemCustomerPaymentDetails(item, CustomerID, model.CreatedBy);

//                                await connection.QueryAsync<CustomerRegistrationModel>("[Accounting].[SaveAccountingCustomerPaymentDetails]",
//                               param: dynamicParametersCustomerPayments,
//                               transaction: transaction,
//                               commandType: CommandType.StoredProcedure);

//                                var FactoryItemCustomerPaymentID = dynamicParametersCustomerPayments.Get<int>("FactoryItemCustomerPaymentID");

//                                if (FactoryItemCustomerPaymentID < 0)
//                                {
//                                    transaction.Rollback();
//                                    return -1;
//                                }
//                            }

//                        }
//                        transaction.Commit();
//                        return CustomerID;

//                    }
//                    catch (Exception ex)
//                    {
//                        errorLog.Error("Error occured when saving factory item customer and error was_" + ex);
//                        transaction.Rollback();
//                        return -1;
//                    }
//                }
//            }
//        }
//        private static DynamicParameters MapToCustomerDetails(AccountingCustomerModel model)
//        {
//            DynamicParameters dynamicParameters = new DynamicParameters();

//            dynamicParameters.Add("GroupID", model.GroupID.ToString(), DbType.Int32, ParameterDirection.Input);
//            dynamicParameters.Add("FactoryID", model.FactoryID.ToString(), DbType.Int32, ParameterDirection.Input);
//            dynamicParameters.Add("CustomerName", model.CustomerName, DbType.String, ParameterDirection.Input);
//            dynamicParameters.Add("Address1", model.Address1, DbType.String, ParameterDirection.Input);
//            dynamicParameters.Add("Address2", model.Address2, DbType.String, ParameterDirection.Input);
//            dynamicParameters.Add("Address3", model.Address3, DbType.String, ParameterDirection.Input);
//            dynamicParameters.Add("ZipCode", model.ZipCode, DbType.String, ParameterDirection.Input);
//            dynamicParameters.Add("IsActive", model.IsActive.ToString(), DbType.Boolean, ParameterDirection.Input);
//            dynamicParameters.Add("IsCreditCustomer", model.IsCreditCustomer.ToString(), DbType.Boolean, ParameterDirection.Input);
//            dynamicParameters.Add("Country", model.Country.ToString(), DbType.Int32, ParameterDirection.Input);
//            dynamicParameters.Add("OfficePhoneNumber", model.OfficePhoneNumber, DbType.String, ParameterDirection.Input);
//            dynamicParameters.Add("Mobile", model.Mobile, DbType.String, ParameterDirection.Input);
//            dynamicParameters.Add("Email", model.Email, DbType.String, ParameterDirection.Input);
//            dynamicParameters.Add("TaxNumber", model.TaxNumber, DbType.String, ParameterDirection.Input);
//            dynamicParameters.Add("NicBRNumber", model.NicBRNumber, DbType.String, ParameterDirection.Input);
//            dynamicParameters.Add("CreatedBy", model.CreatedBy.ToString(), DbType.Int32, ParameterDirection.Input);
//            dynamicParameters.Add("CreatedDate", DateTime.Now, DbType.DateTime, ParameterDirection.Input);
//            dynamicParameters.Add("CustomerID", 0, DbType.Int32, ParameterDirection.Output);

//            return dynamicParameters;
//        }

//        private static DynamicParameters MapToFactoryItemCustomerPaymentDetails(CustomerPaymentModel paymentModel, int customerID, int createdBy)
//        {
//            DynamicParameters dynamicParameters = new DynamicParameters();

//            dynamicParameters.Add("PaymentTypeID", paymentModel.PaymentTypeID.ToString(), DbType.Int32, ParameterDirection.Input);
//            dynamicParameters.Add("BankID", paymentModel.BankID == 0 ? null : paymentModel.BankID.ToString(), DbType.Int32, ParameterDirection.Input);
//            dynamicParameters.Add("BranchID", paymentModel.BranchID == 0 ? null : paymentModel.BranchID.ToString(), DbType.Int32, ParameterDirection.Input);
//            dynamicParameters.Add("AccountNumber", paymentModel.AccountNumber == "" ? null : paymentModel.AccountNumber, DbType.String, ParameterDirection.Input);
//            dynamicParameters.Add("CustomerID", customerID.ToString(), DbType.Int32, ParameterDirection.Input);
//            dynamicParameters.Add("IsActive", 1, DbType.Boolean, ParameterDirection.Input);
//            dynamicParameters.Add("CreatedBy", createdBy.ToString(), DbType.Int32, ParameterDirection.Input);
//            dynamicParameters.Add("CreatedDate", DateTime.Now, DbType.DateTime, ParameterDirection.Input);
//            dynamicParameters.Add("AccountHolderName", paymentModel.AccountHolderName == "" ? null : paymentModel.AccountHolderName, DbType.String, ParameterDirection.Input);
//            dynamicParameters.Add("FactoryItemCustomerPaymentID", 0, DbType.Int32, ParameterDirection.Output);

//            return dynamicParameters;
//        }

//        public static async Task<int> UpdateFactoryItemCustomer(this IRepositoryAsync<AccountingCustomerUpdateModel> repo, AccountingCustomerUpdateModel model)
//        {
//            var errorLog = NLogBuilder.ConfigureNLog("nlog.config").GetLogger("ErrorLog");
//            DynamicParameters dynamicParametersCustomerDetails = MapToCustomerUpdateDetails(model);

//            using (var connection = repo.GetConnectionFactory().GetConnection())
//            {
//                connection.Open();
//                using (var transaction = connection.BeginTransaction())
//                {
//                    try
//                    {
//                        await connection.QueryAsync<AccountingCustomerModel>("[Accounting].[UpdateAccountingCustomerDetails]",
//                        param: dynamicParametersCustomerDetails,
//                        transaction: transaction,
//                        commandType: CommandType.StoredProcedure);

//                        var CustomerID = dynamicParametersCustomerDetails.Get<int>("Result");

//                        if (CustomerID < 0)
//                        {
//                            transaction.Rollback();
//                            return CustomerID;
//                        }
//                        else
//                        {

//                            foreach (var item in model.customerPaymentModel)
//                            {
//                                if (item.FactoryItemCustomerPaymentID == 0)
//                                {
//                                    DynamicParameters dynamicParametersCustomerPayments = MapToFactoryItemCustomerPaymentDetails(item, model.CustomerID, model.CreatedBy);

//                                    await connection.QueryAsync<CustomerRegistrationModel>("[Accounting].[SaveAccountingCustomerPaymentDetails]",
//                                       param: dynamicParametersCustomerPayments,
//                                       transaction: transaction,
//                                       commandType: CommandType.StoredProcedure);

//                                    var FactoryItemCustomerPaymentID = dynamicParametersCustomerPayments.Get<int>("FactoryItemCustomerPaymentID");

//                                    if (FactoryItemCustomerPaymentID < 0)
//                                    {
//                                        transaction.Rollback();
//                                        return -1;
//                                    }
//                                }
//                            }
//                        }

//                        transaction.Commit();
//                        return 1;

//                    }
//                    catch (Exception ex)
//                    {
//                        errorLog.Error("Error occured when saving factory item customer and error was_" + ex);
//                        transaction.Rollback();
//                        return -1;
//                    }
//                }
//            }
//        }
//        private static DynamicParameters MapToCustomerUpdateDetails(AccountingCustomerUpdateModel model)
//        {
//            DynamicParameters dynamicParameters = new DynamicParameters();

//            dynamicParameters.Add("CustomerName", model.CustomerName, DbType.String, ParameterDirection.Input);
//            dynamicParameters.Add("Address1", model.Address1, DbType.String, ParameterDirection.Input);
//            dynamicParameters.Add("Address2", model.Address2, DbType.String, ParameterDirection.Input);
//            dynamicParameters.Add("Address3", model.Address3, DbType.String, ParameterDirection.Input);
//            dynamicParameters.Add("ZipCode", model.ZipCode, DbType.String, ParameterDirection.Input);
//            dynamicParameters.Add("IsActive", model.IsActive.ToString(), DbType.Boolean, ParameterDirection.Input);
//            dynamicParameters.Add("IsCreditCustomer", model.IsCreditCustomer.ToString(), DbType.Boolean, ParameterDirection.Input);
//            dynamicParameters.Add("Country", model.Country == null ? "0" : model.Country.ToString(), DbType.Int32, ParameterDirection.Input);
//            dynamicParameters.Add("OfficePhoneNumber", model.OfficePhoneNumber, DbType.String, ParameterDirection.Input);
//            dynamicParameters.Add("Mobile", model.Mobile, DbType.String, ParameterDirection.Input);
//            dynamicParameters.Add("Email", model.Email, DbType.String, ParameterDirection.Input);
//            dynamicParameters.Add("NicBRNumber", model.NicBRNumber, DbType.String, ParameterDirection.Input);
//            dynamicParameters.Add("TaxNumber", model.TaxNumber, DbType.String, ParameterDirection.Input);
//            dynamicParameters.Add("CustomerID", model.CustomerID, DbType.Int32, ParameterDirection.Input);
//            dynamicParameters.Add("ModifiedBy", model.CreatedBy.ToString(), DbType.Int32, ParameterDirection.Input);
//            dynamicParameters.Add("ModifiedDate", DateTime.Now, DbType.DateTime, ParameterDirection.Input);
//            dynamicParameters.Add("GroupID", model.GroupID.ToString(), DbType.Int32, ParameterDirection.Input);
//            dynamicParameters.Add("FactoryID", model.FactoryID.ToString(), DbType.Int32, ParameterDirection.Input);
//            dynamicParameters.Add("Result", "-1", DbType.Int32, ParameterDirection.InputOutput);

//            return dynamicParameters;
//        }
//        private static DynamicParameters MapToUpdateFactoryItemCustomerDetails(CustomerRegistrationModel customerRegistration, int ModifiedBy, int customerID)
//        {
//            DynamicParameters dynamicParameters = new DynamicParameters();

//            dynamicParameters.Add("CustomerItem", customerRegistration.CustomerItem.ToString(), DbType.String, ParameterDirection.Input);
//            dynamicParameters.Add("IsActive", 1, DbType.Boolean, ParameterDirection.Input);
//            dynamicParameters.Add("ModifiedBy", ModifiedBy.ToString(), DbType.Int32, ParameterDirection.Input);
//            dynamicParameters.Add("ModifiedDate", DateTime.Now, DbType.DateTime, ParameterDirection.Input);
//            dynamicParameters.Add("FactoryItemCustomerID", customerRegistration.FactoryItemCustomerID.ToString(), DbType.Int32, ParameterDirection.Input);
//            dynamicParameters.Add("CustomerID", customerID.ToString(), DbType.Int32, ParameterDirection.Input);
//            dynamicParameters.Add("Result", "-1", DbType.Int32, ParameterDirection.InputOutput);

//            return dynamicParameters;
//        }
//        private static DynamicParameters MapToUpdateFactoryItemCustomerPaymentDetails(PaymentModel paymentModel, int customerID, int ModifiedBy)
//        {
//            DynamicParameters dynamicParameters = new DynamicParameters();

//            dynamicParameters.Add("PaymentTypeID", paymentModel.PaymentTypeID.ToString(), DbType.Int32, ParameterDirection.Input);
//            dynamicParameters.Add("BankID", paymentModel.BankID == null ? "0" : paymentModel.BankID.ToString(), DbType.Int32, ParameterDirection.Input);
//            dynamicParameters.Add("BranchID", paymentModel.BranchID == null ? "0" : paymentModel.BranchID.ToString(), DbType.Int32, ParameterDirection.Input);
//            dynamicParameters.Add("AccountNumber", paymentModel.AccountNumber, DbType.String, ParameterDirection.Input);
//            dynamicParameters.Add("CustomerID", customerID.ToString(), DbType.Int32, ParameterDirection.Input);
//            dynamicParameters.Add("IsActive", 1, DbType.Boolean, ParameterDirection.Input);
//            dynamicParameters.Add("ModifiedBy", ModifiedBy.ToString(), DbType.Int32, ParameterDirection.Input);
//            dynamicParameters.Add("ModifiedDate", DateTime.Now, DbType.DateTime, ParameterDirection.Input);
//            dynamicParameters.Add("ModifiedDate", DateTime.Now, DbType.DateTime, ParameterDirection.Input);
//            dynamicParameters.Add("AccountHolderName", paymentModel.AccountHolderName.ToString(), DbType.String, ParameterDirection.Input);
//            dynamicParameters.Add("Result", "-1", DbType.Int32, ParameterDirection.InputOutput);

//            return dynamicParameters;
//        }
//    }
//}
