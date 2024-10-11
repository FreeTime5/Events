using Events.Application.Models;
using Events.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Application.Interfaces.RepoInterfaces;

public interface ICategoryRepo
{
    public Task Add(string name);

    public Task<Result> Delete(string name);

    public Task<IEnumerable<Category>> GetAll();

    public Task<Category?> GetById(int id);

    public Task<Category?> GetByName(string name);
}
