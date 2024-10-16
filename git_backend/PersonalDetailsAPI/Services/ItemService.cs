// ItemService.cs
using System;
using PersonalDetailsAPI.DAL;
using PersonalDetailsAPI.Models;
using System.Threading.Tasks;

public class ItemService : IItemService
{
    private readonly PersonalDetailsDAL _dal;

    public ItemService(PersonalDetailsDAL dal)
    {
        _dal = dal;
    }

    public async Task<ItemDto> CreateItem(ItemDto itemDto)
    {
        // Validate the item before creating
        if (itemDto == null)
        {
            throw new ArgumentNullException(nameof(itemDto), "Item cannot be null.");
        }

        // Perform business logic here if necessary
        var item = new PersonalDetails
        {
            FirstName = itemDto.FirstName,
            LastName = itemDto.LastName,
            Email = itemDto.Email,
            Phone = itemDto.Phone,
            Address = itemDto.Address,
            City = itemDto.City,
            State = itemDto.State,
            PostalCode = itemDto.PostalCode
        };

        // Insert item into the database
        _dal.InsertPersonalDetails(item);

        // Map back to DTO for return
        itemDto.Id = item.Id;
        return await Task.FromResult(itemDto);
    }

    public async Task<ItemDto> GetItem(int id)
    {
        var personalDetails = _dal.GetPersonalDetailsById(id);
        if (personalDetails == null)
        {
            throw new Exception($"Item with ID {id} not found.");
        }

        // Map to DTO
        var itemDto = new ItemDto
        {
            Id = personalDetails.Id,
            FirstName = personalDetails.FirstName,
            LastName = personalDetails.LastName,
            Email = personalDetails.Email,
            Phone = personalDetails.Phone,
            Address = personalDetails.Address,
            City = personalDetails.City,
            State = personalDetails.State,
            PostalCode = personalDetails.PostalCode
        };

        return await Task.FromResult(itemDto);
    }

    public async Task UpdateItem(ItemDto itemDto)
    {
        if (itemDto == null)
        {
            throw new ArgumentNullException(nameof(itemDto), "Item cannot be null.");
        }

        var existingItem = _dal.GetPersonalDetailsById(itemDto.Id);
        if (existingItem == null)
        {
            throw new Exception($"Item with ID {itemDto.Id} not found.");
        }

        // Update the item
        existingItem.FirstName = itemDto.FirstName;
        existingItem.LastName = itemDto.LastName;
        existingItem.Email = itemDto.Email;
        existingItem.Phone = itemDto.Phone;
        existingItem.Address = itemDto.Address;
        existingItem.City = itemDto.City;
        existingItem.State = itemDto.State;
        existingItem.PostalCode = itemDto.PostalCode;

        _dal.UpdatePersonalDetails(existingItem);

        await Task.CompletedTask;
    }

    public async Task DeleteItem(int id)
    {
        var existingItem = _dal.GetPersonalDetailsById(id);
        if (existingItem == null)
        {
            throw new Exception($"Item with ID {id} not found.");
        }

        _dal.DeletePersonalDetails(id);

        await Task.CompletedTask;
    }
}
