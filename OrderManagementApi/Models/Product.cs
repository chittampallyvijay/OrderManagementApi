using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderManagementApi.Models
{
    public class Product
    {
        public int iProduct { get; set; }
        public string sName { get; set; }
        public decimal fWeight { get; set; }
        public decimal fHeight { get; set; }
        public byte[] image { get; set; }

        public string sUnits { get; set; }
        public byte[] Barcode { get; set; }
        public decimal fAvailableQty { get; set; }
    }

    public class ResultObject
    {
        public int iStatus { get; set; }
        public string sMessage { get; set; }
        public object Data { get; set; }
    }
}