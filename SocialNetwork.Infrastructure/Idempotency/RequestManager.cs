﻿using SocialNetwork.Domain.Entities.Idempotency;
using SocialNetwork.Domain.Exceptions;
using SocialNetwork.Infrastructure.Persistence;
using System;
using System.Threading.Tasks;

namespace SocialNetwork.Infrastructure.Idempotency
{
    public class RequestManager : IRequestManager
    {
        private readonly ApplicationDbContext _context;

        public RequestManager(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public async Task<bool> ExistAsync(Guid id)
        {
            var request = await _context.FindAsync<ClientRequest>(id);

            return request != null;
        }

        public async Task CreateRequestForCommandAsync<T>(Guid id)
        {
            var exists = await ExistAsync(id);

            var request = exists ?
                throw new DomainException($"Request with {id} already exists") :
                new ClientRequest()
                {
                    Id = id,
                    Name = typeof(T).Name,
                    Time = DateTime.UtcNow
                };

            _context.Add(request);

            await _context.SaveChangesAsync();
        }
    }
}
