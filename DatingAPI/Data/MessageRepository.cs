﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingAPI.DTOs;
using DatingAPI.Entities;
using DatingAPI.Helpers;
using DatingAPI.Interface;
using Microsoft.EntityFrameworkCore;



namespace DatingAPI.Data
{
    public class MessageRepository(DataContext context, IMapper mapper) : IMessageRepository
    {
        public void AddMessage(Message message)
        {
            context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            context.Messages.Remove(message);
        }

        public async Task<Message?> GetMessage(int id)
        {
            return await context.Messages.FindAsync(id);
        }

        public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
        {
            var query = context.Messages
                .OrderByDescending(x => x.MessageSent)
                .AsQueryable();

            query = messageParams.Container switch
            {
                "Inbox" => query.Where(x => x.Recipient.UserName == messageParams.Username.ToLower() && x.RecipientDeleted == false),
                "Outbox" => query.Where(x => x.Sender.UserName == messageParams.Username.ToLower() && x.SenderDeleted == false),
                _ => query.Where(x => x.Recipient.UserName == messageParams.Username && x.DateRead == null && x.RecipientDeleted == false)
            };
            var messages = query.ProjectTo<MessageDto>(mapper.ConfigurationProvider);
            return await PagedList<MessageDto>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string recipientUsername)
        {
            var message = await context.Messages
                .Include(x => x.Sender).ThenInclude(x => x.Photos)
                .Include(x => x.Recipient).ThenInclude(x => x.Photos)
                .Where(x =>
                x.RecipientUsername == currentUsername && x.RecipientDeleted == false && x.SenderUsername == recipientUsername ||
                x.SenderUsername == currentUsername && x.SenderDeleted == false && x.RecipientUsername == recipientUsername
                )
                .OrderBy(x => x.MessageSent)
                .ToListAsync();

            var unreadMessages = message.Where(x => x.DateRead == null &&
            x.RecipientUsername == currentUsername).ToList();

            if(unreadMessages.Count != 0)
            {
                unreadMessages.ForEach(x => x.DateRead = DateTime.Now);
                await context.SaveChangesAsync();
            }

            return mapper.Map<IEnumerable<MessageDto>>(message);       
        }

        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}
