
CREATE PROCEDURE sp_OrdersDetailData
 @iOrderId int =0
AS
begin
	IF  EXISTS ( select * from mCore_Buyers 
	inner join mCore_Orders  on mCore_Buyers.iBuyerId=mCore_Orders.iBuyerId   
	where mCore_Orders.iStatus=1 and  mCore_Orders.iOrderId=@iOrderId and iRoleId=1)
	begin
		print 'buyer'
		set @iOrderId=null;
	end
 
	select ORD.iOrderId,ORD.iBuyerId, ORD.iOrderStatus,OST.sStatus, ORD.sShippingAddress, --orderitems.Quantity OrderQty,
	BYR.sBuyerName, BYR.sEmailId, BYR.sPhoneNo,
	 BYR.iRoleId, ROL.sRoleName
	 from mCore_Orders ORD
	inner join mCore_Buyers BYR on BYR.iBuyerId=ORD.iBuyerId
	inner join mCore_Orderstatus OST on OST.iStatus= ORD.iOrderStatus
	inner join cSec_Roles ROL on ROL.iRoleId= BYR.iRoleId 
	where ORD.iStatus=1 and  ORD.iOrderId= ISNULL(@iOrderId, ORD.iOrderId)
end





