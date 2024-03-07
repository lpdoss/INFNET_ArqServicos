using PaymentService.Domain;
using PaymentService.Repositories;
using PaymentService.Services.Dto;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shared.RabbitMQ;
using Shared.RabbitMQ.Events;

namespace PaymentService.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IMapper _mapper;
    private readonly IEventPublisher _eventPublisher;

    public OrderService(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, IMapper mapper, IEventPublisher eventPublisher)
    {
        _orderRepository = orderRepository;
        _orderItemRepository = orderItemRepository;
        _mapper = mapper;
        _eventPublisher = eventPublisher;
    }
    public async Task<List<OrderDto>> GetAll()
    {
        var query = _orderRepository.GetQueryable();
        query = query.Include(a => a.PaymentMethod);
        query = query.Include(a => a.Status);
        query = query.Include(a => a.OrderItems);

        var result = query.AsEnumerable();
        return _mapper.Map<List<OrderDto>>(result);
    }
    public async Task<OrderDto> Get(int id)
    {
        var query = _orderRepository.GetQueryable();
        query = query.Include(a => a.PaymentMethod);
        query = query.Include(a => a.Status);
        query = query.Include(a => a.OrderItems);

        var result = query.Where(a => a.Id == id).FirstOrDefault();
        if (result != null)
            return _mapper.Map<OrderDto>(result);
        return null;
    }
    public async Task<OrderDto> GetByUser(int userId)
    {
        var query = _orderRepository.GetQueryable();
        query = query.Include(a => a.PaymentMethod);
        query = query.Include(a => a.Status);
        query = query.Include(a => a.OrderItems);

        var result = query.Where(a => a.UserId == userId).FirstOrDefault();
        if (result != null)
            return _mapper.Map<OrderDto>(result);
        return null;
    }
    public async Task Save(OrderDto orderDto)
    {
        if (orderDto.Id > 0)
            throw new ArgumentException("Order already has id.");
        var order = _mapper.Map<Order>(orderDto);
        order.StatusId = 1; // Created
        await _orderRepository.Save(order);

        var orderItems = await _orderItemRepository.GetAllByCriteria(a => a.OrderId == order.Id);
        var confirmStockEvent = new ConfirmStockEvent(order.Id, orderItems.Select(a => new OrderStockItem(a.ProductId, a.Amount)).ToList());
        await _eventPublisher.PublishAsync(confirmStockEvent, "confirmStock");
    }

    public async Task Update(OrderDto orderDto)
    {
        if (orderDto.Id <= 0)
            throw new ArgumentException("Order has no id.");
        var order = _mapper.Map<Order>(orderDto);
        await _orderRepository.Update(order);
    }
    public async Task Delete(OrderDto orderDto)
    {
        if (orderDto.Id <= 0)
            throw new ArgumentException("Order is invalid.");
        var order = _mapper.Map<Order>(orderDto);
        await _orderRepository.Delete(order);
    }
}
