using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitapRepositories
{

    public enum DataReceived
    {
        ApproveRequest = 1,
        PendingRequest = 2,
        AdjustQuantity = 3,
        UpdateProducts = 4,
    }

    public enum CacheData
    {
        Customer = 1,
        Product = 2,
        ProductUpdate = 3,
    }

    public enum ApprovalStatus
    {
        Pending = 0,
        Approved = 1,
    }

    public enum UserType
    {
        Customer = 1,
        Admin = 2,
    }

}

