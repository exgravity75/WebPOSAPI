using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPOSAPI.ViewModels
{
    public class OrderViewModel : Models.Order
    {
        public string DeletedOrderItemIDs { get; set; }
    }
}