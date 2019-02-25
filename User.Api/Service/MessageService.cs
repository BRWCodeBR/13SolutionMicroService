﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Api.Message;
using User.Api.Models;

namespace User.Api.Service
{
    public static class MessageService
    {
        //private readonly UserStaticContext _db;

        //public MessageService()
        //{
        //    _db = new UserStaticContext();
        //}

        private static UserMessage _iUMsg = new UserMessage(FacialService.Configuration);
        public static async void SendNewIdMessage(string faceGuid)
        {
            var persistedUser = UserStaticContext.UserFace.First(x => x.faceId == faceGuid);

            var msg = new Microsoft.Azure.ServiceBus.Message()
            {
                MessageId = Guid.NewGuid().ToString(),
                Body = Encoding.ASCII.GetBytes("{\"AreaRestrictionsSet\": false, \"UserId\":" + persistedUser.codUserFace + "}")
            };

            _iUMsg.SendMessagesAsync(msg);
        }

        public static async void SendPersistedIdMessage(string faceId)
        {
            var persistedUser = UserStaticContext.UserFace.First(x => x.faceId == faceId);

            var msg = new Microsoft.Azure.ServiceBus.Message()
            {
                MessageId = Guid.NewGuid().ToString(),
                Body = Encoding.ASCII.GetBytes("Cadastro Persistido: " + persistedUser.codUserFace)
            };

            _iUMsg.SendMessagesAsync(msg);
        }

        /// <summary>
        /// Envia a mensagem com status de processing:true
        /// </summary>
        public static async void SendProcessingMessage(string processingGuid)
        {
            var msg = new Microsoft.Azure.ServiceBus.Message()
            {
                MessageId = Guid.NewGuid().ToString(),
                Body = Encoding.ASCII.GetBytes("{\"Processing\":true,\"UserId\":}")
            };

            _iUMsg.SendMessagesAsync(msg);
        }
    }
}
