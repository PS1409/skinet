using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Entities.OrderAggregate;

namespace Core.Specificatons
{
    public class OrdersWithItemsAndOrderingSpecification : BaseSpecification<Order>
    {
        public OrdersWithItemsAndOrderingSpecification(string email)
            : base(o => o.BuyerEmail == email)
        {
            AddIncludes(o=> o.OrderItems);
            AddIncludes(o => o.DeliveryMethod);
            AddOrderByDescending(o => o.OrderDate);
        }

        public OrdersWithItemsAndOrderingSpecification(int orderId, string email) 
               : base(o=>o.Id == orderId && o.BuyerEmail == email)
        {
            AddIncludes(o=> o.OrderItems);
            AddIncludes(o => o.DeliveryMethod);
        }
    }
}