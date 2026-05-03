using AutoMapper;
using Autoparts.BusinessLogic.Interfaces;
using Autoparts.DataAccess;
using Autoparts.Domains.DTOs;
using Autoparts.Domains.Entities;
using Microsoft.EntityFrameworkCore;

namespace Autoparts.BusinessLogic.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;

    public UserService(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _db.Users.ToListAsync();
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task<UserDto?> GetByIdAsync(int id)
    {
        var user = await _db.Users.FindAsync(id);
        return user is null ? null : _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> CreateAsync(CreateUserDto dto)
    {
        var user = _mapper.Map<User>(dto);
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto?> UpdateAsync(int id, UpdateUserDto dto)
    {
        var user = await _db.Users.FindAsync(id);
        if (user is null) return null;
        _mapper.Map(dto, user);
        await _db.SaveChangesAsync();
        return _mapper.Map<UserDto>(user);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user is null) return false;
        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
        return true;
    }
}
