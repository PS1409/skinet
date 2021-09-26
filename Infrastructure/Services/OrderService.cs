using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specificatons;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        //private readonly IGenericRepository<Order> _orderRepo;
        //private readonly IGenericRepository<DeliveryMethod> _deliverMethod;
       // private readonly IGenericRepository<Product> _productRepo;
        private readonly IBasketRepository _basketRepo;

       private readonly IUnitOfWork _unitOfWork;

        public OrderService(IBasketRepository basketRepo, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _basketRepo = basketRepo;
        }

        // public OrderService(IGenericRepository<Order> orderRepo, IGenericRepository<DeliveryMethod> deliverMethod
        //                     , IGenericRepository<Product> productRepo, IBasketRepository basketRepo)
        // {
        //     _orderRepo = orderRepo;
        //     _deliverMethod = deliverMethod;
        //     _productRepo = productRepo;
        //     _basketRepo = basketRepo;
        // }

        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
        {
            // Get the basket from basket repo
            var basket = await _basketRepo.GetBasketAsync(basketId);

            // get the items from the product repo
            var items = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                // var productItem = await _productRepo.GetByIdAsync(item.Id);
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
                var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
                items.Add(orderItem);
            }

            // get the delivery methods from repo
            //var deliverMethod = await _deliverMethod.GetByIdAsync(deliveryMethodId);
            var deliverMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            // calculate the subtotal
            var subtotal = items.Sum(s => (s.Price * s.Quantity));

            // create order
            var order = new Order(items, buyerEmail, shippingAddress, deliverMethod, subtotal);
             _unitOfWork.Repository<Order>().Add(order);

            //TODO: save to database
             var result = await _unitOfWork.Complete();
             if(result <= 0)  return null;

             // delete the basket
             await _basketRepo.DeleteBasketAsync(basketId);

            // return the order
            return order;
        }

        public  async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int orderId, string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(orderId, buyerEmail);
            return await _unitOfWork.Repository<Order>().GetEntityWithSpecifications(spec);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(buyerEmail);
            return await _unitOfWork.Repository<Order>().ListAsync(spec);
        }
    }
}