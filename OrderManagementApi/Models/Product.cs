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
        public decimal fTotalQty { get; set; }
        public decimal fAvailableQty { get; set; }
    }

    public class Buyers
    {
        public int iBuyerId { get; set; }
        public string sRoleName { get; set; }
        public string sBuyerName { get; set; }
        public string sEmailId { get; set; }
        public string sPhoneNo { get; set; }
    }
    public class OrderItem
    {
        //public int iOrderId { get; set; }
        public int iProduct { get; set; }
        public decimal fQuantity { get; set; }
        public Product OBJProduct { get; set; }
    }

    public class Orders
    {
        public int iOrderId { get; set; }
        public int iBuyerId { get; set; }
        public string sOrderStatus { get; set; }
        public string sShippingAddress { get; set; }
        public List<OrderItem> lstOrderItems { get; set; }
        public Buyers ObjBuyers { get; set; }
    }
    public class ResultObject
    {
        public int iStatus { get; set; }
        public string sMessage { get; set; }
        public object Data { get; set; }
    }
}