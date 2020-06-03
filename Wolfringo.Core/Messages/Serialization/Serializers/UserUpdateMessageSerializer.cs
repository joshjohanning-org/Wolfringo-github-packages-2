﻿using Newtonsoft.Json.Linq;
using TehGM.Wolfringo.Messages.Serialization.Internal;

namespace TehGM.Wolfringo.Messages.Serialization
{
    public class UserUpdateMessageSerializer : DefaultMessageSerializer<UserUpdateMessage>
    {
        public override SerializedMessageData Serialize(IWolfMessage message)
        {
            SerializedMessageData result = base.Serialize(message);
            JObject body = result.Payload["body"] as JObject;
            if (body == null)
                return result;
            // extended props
            JObject extended = new JObject();
            SerializationHelper.MovePropertyIfExists(ref body, ref extended, "name");
            SerializationHelper.MovePropertyIfExists(ref body, ref extended, "about");
            SerializationHelper.MovePropertyIfExists(ref body, ref extended, "lookingFor");
            SerializationHelper.MovePropertyIfExists(ref body, ref extended, "gender");
            SerializationHelper.MovePropertyIfExists(ref body, ref extended, "relationship");
            SerializationHelper.MovePropertyIfExists(ref body, ref extended, "language");
            SerializationHelper.MovePropertyIfExists(ref body, ref extended, "urls");
            if (extended.HasValues)
                body.Add(new JProperty("extended", extended));
            return result;
        }
    }
}
