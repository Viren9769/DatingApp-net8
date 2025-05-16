using AutoMapper;
using DatingAPI.DTOs;
using DatingAPI.Entities;
using DatingAPI.Extensions;
using DatingAPI.Helpers;
using DatingAPI.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace DatingAPI.Controllers
{
    [Authorize]
    public class MessagesController (IMessageRepository messageRepository, IUserRepository userRepository, IMapper mapper) : BaseApiController
    {
        public async Task<ActionResult<MessageDto>> CreateMessages(CreateMessageDto messageDto)
        {
            var username = User.GetUsername();
            if(username == messageDto.RecipientUsername.ToLower()) { 
            return BadRequest("You cannot mesage yourself");}

            var sender = await userRepository.GetUserByNameAsync(username);
            var recipient = await userRepository.GetUserByNameAsync(messageDto.RecipientUsername);

            if (recipient == null || sender == null || sender.UserName == null || recipient.UserName == null) return BadRequest("Cannot send message at this time");

            var messages = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = messageDto.content
            };
            messageRepository.AddMessage(messages);
            if(await messageRepository.SaveAllAsync()) return Ok(mapper.Map<MessageDto>(messages));
            return BadRequest("Failed to save messages");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageForUser([FromQuery] MessageParams messageParams)
        {
            messageParams.Username = User.GetUsername();
            var messages = await messageRepository.GetMessagesForUser(messageParams);
            Response.AddPaginationHeader(messages);
            return messages;
        }


        [HttpGet("thread/{username}")]
        
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string username)
        {
            var currentUsername = User.GetUsername();
            return Ok(await messageRepository.GetMessageThread(currentUsername, username));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var username = User.GetUsername();
            var message = await messageRepository.GetMessage(id);
            if (message == null) return BadRequest("Cannot delete this message");
            if (message.SenderUsername != username && message.RecipientUsername != username)
                return Forbid();
            if (message.SenderUsername == username) message.SenderDeleted = true;
            if(message.RecipientUsername == username) message.RecipientDeleted = true;

            if(message is { SenderDeleted: true, RecipientDeleted: true })
            {
                messageRepository.DeleteMessage(message);
            }

            if (await messageRepository.SaveAllAsync()) return Ok();
            return BadRequest("Problem in deleting the messages");

        }
    }


}
