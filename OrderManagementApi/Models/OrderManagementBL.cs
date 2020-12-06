using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using System.Web;
using OrderManagementApi.Models;

namespace OrderManagementApi.Models
{
    public class OrderManagementBL
    {
        SqlConnection Sqlconnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());

        string strQuery = string.Empty;

        public ResultObject DeleteOrders(int iOrderId)
        {
            ResultObject OBJResult = new ResultObject();
            SqlTransaction transaction;
            SqlCommand SQLcmd = null;
            Sqlconnection.Open();

            transaction = Sqlconnection.BeginTransaction();
            try
            {
                int iResult = 0;
                strQuery = string.Format($@"UPDATE mCore_Orders SET iStatus=5 WHERE iOrderId={iOrderId}");
                SQLcmd = new SqlCommand(strQuery, Sqlconnection, transaction);
                iResult = SQLcmd.ExecuteNonQuery();

                if (iResult > 0)
                {
                    OBJResult.Data = iOrderId;
                    OBJResult.iStatus = 1;
                    OBJResult.sMessage = "Successfully Deleted Data.";
                    transaction.Commit();
                    Sqlconnection.Close();
                }
                else
                {
                    OBJResult.Data = iOrderId;
                    OBJResult.iStatus = 2;
                    OBJResult.sMessage = "Successfully Not Deleted.";
                    transaction.Rollback();
                    Sqlconnection.Close();
                }
            }
            catch (Exception ex)
            {
                OBJResult.Data = null;
                OBJResult.iStatus = 2;
                OBJResult.sMessage = ex.Message;
                transaction.Rollback();
                Sqlconnection.Close();
            }
            return OBJResult;
        }
        public ResultObject SaveOrders(Orders OBJOrders)
        {
            ResultObject OBJResult = new ResultObject();
            SqlTransaction transaction;
            SqlCommand SQLcmd = null;
            Sqlconnection.Open();

            transaction = Sqlconnection.BeginTransaction();
            try
            {
                int iOrderStatus = 0; bool bSendEmail = false;

                OBJOrders.ObjBuyers = SaveBuyers(OBJOrders.ObjBuyers, transaction, SQLcmd);

                strQuery = string.Format($@"SELECT iStatus from mCore_Orderstatus (readuncommitted) WHERE sStatus='{OBJOrders.sOrderStatus}'");
                SQLcmd = new SqlCommand(strQuery, Sqlconnection, transaction);
                iOrderStatus = Convert.ToInt32(SQLcmd.ExecuteScalar());
                if (OBJOrders.iOrderId > 0)
                {
                    strQuery = string.Format($@"UPDATE mCore_Orders SET iBuyerId={OBJOrders.iBuyerId},iOrderStatus={iOrderStatus},
                sShippingAddress='{OBJOrders.sShippingAddress}',iStatus=1 WHERE iOrderId={OBJOrders.iOrderId}");
                    SQLcmd = new SqlCommand(strQuery, Sqlconnection, transaction);
                    SQLcmd.ExecuteNonQuery();
                }
                else
                {
                    strQuery = string.Format($@"INSERT INTO mCore_Orders(iBuyerId,iOrderStatus,sShippingAddress,iStatus) VALUES({OBJOrders.ObjBuyers.iBuyerId},{iOrderStatus},'{OBJOrders.sShippingAddress}',1);
                                    select @@identity");
                    SQLcmd = new SqlCommand(strQuery, Sqlconnection, transaction);
                    OBJOrders.iOrderId = Convert.ToInt32(SQLcmd.ExecuteScalar());
                    bSendEmail = true;
                }
                int iResult = 0;
                if (OBJOrders.iOrderId > 0)
                {
                    iResult = SaveOrderItems(OBJOrders.lstOrderItems, OBJOrders.iOrderId, transaction, SQLcmd);
                }
                if (iResult > 0)
                {
                    if (bSendEmail)
                        SendEmail(OBJOrders);
                    OBJResult.Data = OBJOrders.iOrderId;
                    OBJResult.iStatus = 1;
                    OBJResult.sMessage = "Successfully Saved Data.";
                    transaction.Commit();
                    Sqlconnection.Close();

                }
                else
                {
                    OBJResult.Data = iResult;
                    OBJResult.iStatus = 2;
                    OBJResult.sMessage = "Successfully Not Saved.";
                    transaction.Rollback();
                    Sqlconnection.Close();
                }
            }
            catch (Exception ex)
            {
                OBJResult.Data = null;
                OBJResult.iStatus = 2;
                OBJResult.sMessage = ex.Message;
                transaction.Rollback();
                Sqlconnection.Close();
            }
            return OBJResult;
        }
        private void SendEmail(Orders OBJOrders)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(OBJOrders.ObjBuyers.sEmailId);
            SmtpSection section = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            mail.From = new MailAddress(section.From);
            mail.Subject = "Placed Order NO:" + OBJOrders.iOrderId;
            StringBuilder mailBody = new StringBuilder();
            mailBody.AppendFormat("Dear {0},", OBJOrders.ObjBuyers.sBuyerName);
            mailBody.AppendFormat("<br />");
            mailBody.AppendFormat("<p>Your Order has been placed</p>");
            mail.Body = mailBody.ToString();
            mail.IsBodyHtml = true;
            using (SmtpClient smtp = new SmtpClient(section.Network.Host, section.Network.Port))
            {
                smtp.Credentials = new NetworkCredential(section.Network.UserName, section.Network.Password);
                smtp.EnableSsl = section.Network.EnableSsl;
                smtp.Send(mail);
            }
        }
        private int SaveOrderItems(List<OrderItem> lstOrderItem, int iOrderId, SqlTransaction trans, SqlCommand SQLcmd)
        {
            strQuery = string.Empty;
            int iResult = 0;
            SQLcmd = new SqlCommand($@"DELETE FROM mCore_OrderItem WHERE iOrderId={iOrderId} ", Sqlconnection, trans);
            SQLcmd.ExecuteNonQuery();

            strQuery = string.Empty;
            for (int i = 0; i < lstOrderItem.Count; i++)
            {
                strQuery = strQuery + string.Format($@"INSERT INTO mCore_OrderItem(iOrderId,Quantity,iProduct) VALUES({iOrderId},{lstOrderItem[i].fQuantity },{lstOrderItem[i].iProduct});");
            }
            SQLcmd = new SqlCommand(strQuery, Sqlconnection, trans);
            iResult = SQLcmd.ExecuteNonQuery();
            return iResult;

        }
        private Buyers SaveBuyers(Buyers OBJBuyers, SqlTransaction trans, SqlCommand SQLcmd)
        {

            int iRoleId = 0;
            strQuery = string.Format($@"Select iRoleId from cSec_Roles (readuncommitted) where iStatus=1 and  sRoleName like '{OBJBuyers.sRoleName}%' ");
            SQLcmd = new SqlCommand(strQuery, Sqlconnection, trans);
            iRoleId = Convert.ToInt32(SQLcmd.ExecuteScalar());

            if (OBJBuyers.iBuyerId > 0)
            {
                strQuery = string.Format($@"UPDATE  mCore_Buyers SET iRoleId={iRoleId},sBuyerName='{OBJBuyers.sBuyerName}',
                sEmailId='{OBJBuyers.sEmailId}',sPhoneNo='{OBJBuyers.sPhoneNo}' WHERE iBuyerId={OBJBuyers.iBuyerId}");
                SQLcmd = new SqlCommand(strQuery, Sqlconnection, trans);
                SQLcmd.ExecuteNonQuery();
            }
            else
            {
                strQuery = string.Format($@"INSERT INTO mCore_Buyers(iRoleId,sBuyerName,sEmailId,sPhoneNo) VALUES({iRoleId},'{OBJBuyers.sBuyerName}','{OBJBuyers.sEmailId}','{OBJBuyers.sPhoneNo}');
                                    select @@identity");
                SQLcmd = new SqlCommand(strQuery, Sqlconnection, trans);
                OBJBuyers.iBuyerId = Convert.ToInt32(SQLcmd.ExecuteScalar());
            }
            return OBJBuyers;
        }

        private List<OrderItem> GetOrderItems(int iOrderId)
        {
            IDataReader IDR = null;
            List<OrderItem> lstItems = new List<OrderItem>();
            OrderItem OBJItems = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_OrdersItemDetailData", Sqlconnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@iOrderId", iOrderId);
                    //Sqlconnection.Open();
                    IDR = cmd.ExecuteReader();
                }
                while (IDR.Read())
                {
                    OBJItems = new OrderItem();
                    //OBJItems.iOrderId = Convert.ToInt32(IDR["iOrderId"]);
                    OBJItems.iProduct = Convert.ToInt32(IDR["iProduct"]);
                    OBJItems.fQuantity = Convert.ToDecimal(IDR["OrderQty"]);

                    OBJItems.OBJProduct = new Product();
                    OBJItems.OBJProduct.iProduct = Convert.ToInt32(IDR["iProduct"]);
                    OBJItems.OBJProduct.sName = IDR["ProductName"].ToString();
                    OBJItems.OBJProduct.fWeight = Convert.ToDecimal(IDR["ProductWeight"]);
                    OBJItems.OBJProduct.fHeight = Convert.ToDecimal(IDR["Height"]);
                    OBJItems.OBJProduct.sUnits = IDR["Stoke  Keeping Units"].ToString();
                    OBJItems.OBJProduct.fAvailableQty = Convert.ToDecimal(IDR["AvailableQty"]);
                    OBJItems.OBJProduct.fTotalQty = Convert.ToDecimal(IDR["TotalQty"]);


                    lstItems.Add(OBJItems);
                }
                IDR.Close();
            }
            catch (Exception ex)
            {
                lstItems = new List<OrderItem>();
            }
            return lstItems;

        }
        public ResultObject GetOrders(int iOrderId)
        {
            IDataReader reader = null;
            ResultObject OBJResult = new ResultObject();
            List<Orders> lstOrders = new List<Orders>();
            Orders OBJOrders = null;
            using (SqlCommand cmd = new SqlCommand("sp_OrdersDetailData", Sqlconnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@iOrderId", iOrderId);
                Sqlconnection.Open();
                reader = cmd.ExecuteReader();
            }
            while (reader.Read())
            {
                OBJOrders = new Orders();
                OBJOrders.iOrderId = Convert.ToInt32(reader["iOrderId"]);
                OBJOrders.iBuyerId = Convert.ToInt32(reader["iBuyerId"]);
                OBJOrders.sOrderStatus = reader["sStatus"].ToString();
                OBJOrders.sShippingAddress = reader["sShippingAddress"].ToString();

                OBJOrders.ObjBuyers = new Buyers();
                OBJOrders.ObjBuyers.iBuyerId = OBJOrders.iBuyerId;
                OBJOrders.ObjBuyers.sBuyerName = reader["sBuyerName"].ToString();
                OBJOrders.ObjBuyers.sEmailId = reader["sEmailId"].ToString();
                OBJOrders.ObjBuyers.sPhoneNo = reader["sPhoneNo"].ToString();

                OBJOrders.ObjBuyers.sRoleName = reader["sRoleName"].ToString();
                lstOrders.Add(OBJOrders);
            }
            reader.Close();

            for (int i = 0; i < lstOrders.Count; i++)
            {
                lstOrders[i].lstOrderItems = GetOrderItems(OBJOrders.iOrderId);
            }
            Sqlconnection.Close();
            if (lstOrders.Count == 0)
            {
                OBJResult.sMessage = "No Data";
            }
            OBJResult.Data = lstOrders;
            OBJResult.iStatus = 1;
            return OBJResult;
        }
    }
}