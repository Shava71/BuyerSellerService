using MediatR;
using ShopService.Application.Dtos;
using ShopService.Application.Services.Interfaces;
using ShopService.Domain.Entities;
using ShopService.Domain.Events;
using ShopService.Domain.Repositories;

namespace ShopService.Application.Services.Implementations;

public class SellerService : ISellerService
{
    private readonly IItemsRepository _itemsRepo;
    private readonly IMediator _mediator;

    public SellerService(IItemsRepository itemsRepo, IMediator mediator)
    {
        _itemsRepo = itemsRepo;
        _mediator = mediator;
    }

    public Task<List<Item>> GetAllAsync(CancellationToken ct = default) => _itemsRepo.GetAllAsync(ct);

    public Task<Item?> GetByIdAsync(Guid id, CancellationToken ct = default) => _itemsRepo.GetByIdAsync(id, ct);

    public async Task<Item> CreateAsync(CreateItemDto dto, CancellationToken ct = default)
    {
        Item item = new Item(Guid.NewGuid(), dto.Name, dto.Category, dto.Price);
        await _itemsRepo.AddAsync(item, ct);

        await _mediator.Publish(new ItemAddedEvent(item.Id, item.Category, item.Price, DateTime.UtcNow), ct);

        return item;
    }

    public async Task UpdateAsync(UpdateItemDto dto, CancellationToken ct = default)
    {
        Item item = await _itemsRepo.GetByIdAsync(dto.Id, ct) ?? throw new KeyNotFoundException();
        item.Update(dto.Name, dto.Category, dto.Price);
        await _itemsRepo.UpdateAsync(item, ct);
    }

    public Task DeleteAsync(Guid id, CancellationToken ct = default) => _itemsRepo.DeleteAsync(id, ct);
}