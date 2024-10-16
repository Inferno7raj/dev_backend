// IItemService.cs
using System.Threading.Tasks;

public interface IItemService
{
    Task<ItemDto> CreateItem(ItemDto itemDto);
    Task<ItemDto> GetItem(int id);
    Task UpdateItem(ItemDto itemDto);
    Task DeleteItem(int id);
}
