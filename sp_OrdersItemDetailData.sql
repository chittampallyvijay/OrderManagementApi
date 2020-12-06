
CREATE PROCEDURE sp_OrdersItemDetailData @iOrderId int
AS
begin 
select orders.iOrderId,orders.iBuyerId, orders.sShippingAddress, orderitems.Quantity OrderQty,orderitems.iProduct,
 prod.Name ProductName, prod.Weight ProductWeight, prod.Height, prod.[Stoke  Keeping Units] , prod.AvailableQty TotalQty,  (prod.AvailableQty- ISNULL(alterQty.Quantity,0)) AvailableQty
 from mCore_Orders orders
inner join mCore_OrderItem orderitems on orderitems.iOrderId=orders.iOrderId and orders.iStatus=1
inner join mCore_Products prod on prod.iProduct=orderitems.iProduct
INNER JOIN (SELECT iProduct, SUM(Quantity) Quantity FROM mCore_OrderItem
			inner join mCore_Orders on mCore_Orders.iOrderId= mCore_OrderItem.iOrderId
			where mCore_Orders.iStatus=1 and mCore_Orders.iOrderStatus not in (3)
			 GROUP BY iProduct) alterQty on alterQty.iProduct=prod.iProduct
where orders.iStatus=1 and  orders.iOrderId= @iOrderId
end
