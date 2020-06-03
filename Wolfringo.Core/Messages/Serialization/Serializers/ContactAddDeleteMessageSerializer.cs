﻿using Newtonsoft.Json.Linq;
using System;

namespace TehGM.Wolfringo.Messages.Serialization.Serializers
{
    // this protocol just can't stay consistent... when you send contact add or delete, you put their
    // ID in "id" prop. And then you receive an event where their ID is "targetId", cause "id" is your own
    // not only it's inconsistent, but also makes no sense, as the recipient seems to not get the message at all
    // ... eh, I can't anymore...
    public class ContactAddDeleteMessageSerializer<T> : DefaultMessageSerializer<T> where T : IWolfMessage
    {
        private static readonly Type _addMessageType = typeof(ContactAddMessage);
        private static readonly Type _deleteMessageType = typeof(ContactDeleteMessage);

        public ContactAddDeleteMessageSerializer()
        {
            ThrowIfInvalidMessageType(typeof(T));
        }

        public override IWolfMessage Deserialize(string command, SerializedMessageData messageData)
        {
            // replace "id" with "targetId" if it exists
            if (messageData.Payload["body"]?["targetId"] == null)
                return base.Deserialize(command, messageData);
            else
            {
                JToken payload = messageData.Payload.DeepClone();
                JObject body = (JObject)payload["body"];
                JToken value = body["targetId"];
                body.Remove("targetId");
                body["id"] = value;
                //body.Add("id", value);
                return base.Deserialize(command, new SerializedMessageData(payload, messageData.BinaryMessages));
            }
        }

        private void ThrowIfInvalidMessageType(Type messageType)
        {
            if (!_addMessageType.IsAssignableFrom(messageType) && !_deleteMessageType.IsAssignableFrom(messageType))
                throw new InvalidOperationException($"{this.GetType().Name} can only support {_addMessageType.Name} and {_deleteMessageType.Name}");
        }
    }
}
