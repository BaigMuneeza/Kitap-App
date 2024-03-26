using KitapRepositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.SerializationMananger;

namespace KitapFileManager
{
    public class SignupData
    {
        string filePath = @"C:\DataFiles";

        public void ProcessSignUpData(object receivedData, int enumKey)
        {
            string filePath_ApprovedSignup = Path.Combine(filePath, "CustomerInformation.xml");
            string filePath_PendingSignup = Path.Combine(filePath, "PendingApprovalSignupInformation.xml");
            List<CustomerModel> customers = new List<CustomerModel>();

            try
            {
                switch (enumKey)
                {
                    case (int)ApprovalStatus.Pending:
                        if (File.Exists(filePath_PendingSignup))
                        {
                            customers = SerializationBase.DeserializeData<CustomerModel>(filePath_PendingSignup);
                            if (receivedData is CustomerModel customer)
                            {
                                customers.Add(customer);
                            }
                        }
                        SerializationBase.SerializeData(filePath_PendingSignup, customers);
                        Console.WriteLine("Employee data saved to PENDING file.");

                        break;

                    case (int)ApprovalStatus.Approved:
                        if (File.Exists(filePath_ApprovedSignup))
                        {
                            customers = SerializationBase.DeserializeData<CustomerModel>(filePath_ApprovedSignup);
                            if (receivedData is CustomerModel customer)
                            {
                                customers.Add(customer);
                            }
                        }
                        SerializationBase.SerializeData(filePath_ApprovedSignup, customers);
                        Console.WriteLine("Employee data saved to APPROVED file.");

                        break;

                    default:
                        Console.WriteLine("incorrect key");
                        break;
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
