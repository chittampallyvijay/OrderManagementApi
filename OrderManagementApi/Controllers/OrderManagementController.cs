﻿using OrderManagementApi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using System.Text;

namespace OrderManagementApi.Controllers
{
    public class OrderManagementController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        //public HttpResponseMessage GetView(int id)
        //{
        //    List<Product> lstProduct = new List<Product>();
        //    ResultObject ObjResult = new ResultObject();
        //    HttpResponseMessage objHttpResponseMessage = null;
        //    try
        //    {
        //        string connectionstring = ConfigurationManager.ConnectionStrings["Connection"].ToString();
        //        string strQuery = string.Format($@"select Name,Weight,Height,[Stoke  Keeping Units],AvailableQty from mCore_Products");
        //        SqlConnection con = new SqlConnection(connectionstring);
        //        con.Open();
        //        SqlCommand SQLC = new SqlCommand(strQuery, con);
        //        IDataReader reader = SQLC.ExecuteReader();
        //        Product ObjProduct = null;

        //        while (reader.Read())
        //        {
        //            ObjProduct = new Product();
        //            ObjProduct.sName = reader["Name"].ToString();
        //            ObjProduct.fWeight = Convert.ToDecimal(reader["Weight"]);
        //            ObjProduct.fHeight = Convert.ToDecimal(reader["Height"]);
        //            ObjProduct.sUnits = reader["Stoke  Keeping Units"].ToString();
        //            ObjProduct.fAvailableQty = Convert.ToDecimal(reader["AvailableQty"]);
        //            lstProduct.Add(ObjProduct);
        //        }
        //        reader.Close();
        //        con.Close();
        //        if (lstProduct.Count > 0)
        //        {
        //            ObjResult.Data = lstProduct;
        //            ObjResult.iStatus = 1;
        //            ObjResult.sMessage = "SuccessFully Loaded.";
        //        }
        //        else
        //        {
        //            ObjResult.Data = null;
        //            ObjResult.iStatus = 0;
        //            ObjResult.sMessage = "No Data.";
        //        }
        //        var jsonData = JsonConvert.SerializeObject(ObjResult);
        //        objHttpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
        //        objHttpResponseMessage.Content = new StringContent(jsonData, Encoding.UTF8, "application/json");
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    //Sucess
        //    return objHttpResponseMessage;
        //}

        // POST api/<controller>
        public HttpResponseMessage Post(Orders OBJOrders)
        {
            HttpResponseMessage objHttpResponseMessage = null;
            OrderManagementBL OBJBL = new OrderManagementBL();
            ResultObject ObjResult = OBJBL.SaveOrders(OBJOrders);
            var jsonData = JsonConvert.SerializeObject(ObjResult);
            objHttpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            objHttpResponseMessage.Content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            return objHttpResponseMessage;
        }

        // PUT api/<controller>/5
        public HttpResponseMessage Put(Orders OBJOrders)
        {
            HttpResponseMessage objHttpResponseMessage = null;
            OrderManagementBL OBJBL = new OrderManagementBL();
            ResultObject ObjResult = OBJBL.SaveOrders(OBJOrders);
            var jsonData = JsonConvert.SerializeObject(ObjResult);
            objHttpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            objHttpResponseMessage.Content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            return objHttpResponseMessage;
        }

        // DELETE api/<controller>/5
        public HttpResponseMessage Delete(int id)
        {
            HttpResponseMessage objHttpResponseMessage = null;
            OrderManagementBL OBJBL = new OrderManagementBL();
            ResultObject ObjResult = OBJBL.DeleteOrders(id);
            var jsonData = JsonConvert.SerializeObject(ObjResult);
            objHttpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            objHttpResponseMessage.Content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            return objHttpResponseMessage;
        }
        public HttpResponseMessage Get(int id)
        {
            HttpResponseMessage objHttpResponseMessage = null;
            OrderManagementBL OBJBL = new OrderManagementBL();
            ResultObject ObjResult = OBJBL.GetOrders(id);
            var jsonData = JsonConvert.SerializeObject(ObjResult);
            objHttpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            objHttpResponseMessage.Content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            return objHttpResponseMessage;
        }
    }
}