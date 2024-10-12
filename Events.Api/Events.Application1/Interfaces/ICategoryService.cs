﻿using Events.Application.Models;
using Events.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Application.Interfaces;

public interface ICategoryService
{
    public Task AddCategory(string name, User user);

    public Task DeleteCategory(string name, User user);

    public Task<IEnumerable<Category>> GetAllCategories();
}