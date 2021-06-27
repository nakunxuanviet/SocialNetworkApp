﻿using System;
using System.Threading.Tasks;

namespace SocialNetwork.Domain.Entities.Idempotency
{
    public interface IRequestManager
    {
        Task<bool> ExistAsync(Guid id);

        Task CreateRequestForCommandAsync<T>(Guid id);
    }
}
