﻿using Newtonsoft.Json;

namespace TehGM.Wolfringo.Messages
{
    public class GroupAdminMessage : IWolfMessage
    {
        public string Command => MessageCommands.GroupAdmin;

        [JsonProperty("groupId")]
        public uint GroupID { get; private set; }
        [JsonProperty("subscriberId", NullValueHandling = NullValueHandling.Ignore)]
        public uint UserID { get; private set; }
        [JsonProperty("capabilities", NullValueHandling = NullValueHandling.Ignore)]
        public WolfGroupCapabilities Capabilities { get; private set; }

        [JsonConstructor]
        private GroupAdminMessage() { }

        public GroupAdminMessage(uint userID, uint groupID, WolfGroupCapabilities newCapabilities)
        {
            this.GroupID = groupID;
            this.UserID = userID;
            this.Capabilities = newCapabilities;
        }
    }
}
