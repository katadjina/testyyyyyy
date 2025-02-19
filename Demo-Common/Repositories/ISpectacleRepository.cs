﻿using Demo_Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo_Common.Repositories
{
    public interface ISpectacleRepository<TEntity, TId> : IRepository<TEntity, TId>        where TEntity : ISpectacle
    {
    }
}
