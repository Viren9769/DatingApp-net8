﻿namespace DatingAPI.DTOs
{
    public class CreateMessageDto
    {
        public required string RecipientUsername {  get; set; }
        public required string content { get; set; }
    }
}
