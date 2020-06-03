﻿using System;
using System.Collections.Generic;
using TehGM.Wolfringo.Messages.Serialization.Serializers;

namespace TehGM.Wolfringo.Messages.Serialization
{
    public class DefaultMessageSerializerMap : ISerializerMap<string, IMessageSerializer>
    {
        public IMessageSerializer FallbackSerializer { get; set; }

        private IDictionary<string, IMessageSerializer> _map;

        public DefaultMessageSerializerMap(IMessageSerializer fallbackSerializer = null)
        {
            this.FallbackSerializer = fallbackSerializer ?? new DefaultMessageSerializer<IWolfMessage>();
            this._map = new Dictionary<string, IMessageSerializer>(StringComparer.OrdinalIgnoreCase)
            {
                // default ones
                { MessageCommands.Welcome, new DefaultMessageSerializer<WelcomeEvent>() },
                { MessageCommands.SecurityLogin, new DefaultMessageSerializer<LoginMessage>() },
                { MessageCommands.MessagePrivateSubscribe, new DefaultMessageSerializer<SubscribeToPmMessage>() },
                { MessageCommands.MessageGroupSubscribe, new DefaultMessageSerializer<SubscribeToGroupMessage>() },
                { MessageCommands.NotificationList, new DefaultMessageSerializer<ListNotificationsMessage>() },
                { MessageCommands.SubscriberProfile, new DefaultMessageSerializer<UserProfileMessage>() },
                { MessageCommands.SubscriberContactList, new DefaultMessageSerializer<ListContactsMessage>() },
                { MessageCommands.PresenceUpdate, new DefaultMessageSerializer<PresenceUpdateEvent>() },
                { MessageCommands.SubscriberSettingsUpdate, new DefaultMessageSerializer<OnlineStateUpdateMessage>() },
                { MessageCommands.SecurityLogout, new DefaultMessageSerializer<LogoutMessage>() },
                { MessageCommands.GroupProfile, new DefaultMessageSerializer<GroupProfileMessage>() },
                { MessageCommands.GroupAudioCountUpdate, new DefaultMessageSerializer<GroupAudioCountUpdateEvent>() },
                { MessageCommands.GroupUpdate, new DefaultMessageSerializer<GroupUpdateEvent>() },
                { MessageCommands.GroupMemberList, new DefaultMessageSerializer<ListGroupMembersMessage>() },
                { MessageCommands.MessageGroupHistoryList, new DefaultMessageSerializer<GroupChatHistoryMessage>() },
                { MessageCommands.MessagePrivateHistoryList, new DefaultMessageSerializer<PrivateChatHistoryMessage>() },
                { MessageCommands.MessageConversationList, new DefaultMessageSerializer<RecentConversationsMessage>() },
                { MessageCommands.SubscriberGroupList, new DefaultMessageSerializer<ListUserGroupsMessage>() },
                { MessageCommands.CharmList, new DefaultMessageSerializer<ListCharmsMessage>() },
                { MessageCommands.CharmSubscriberStatistics, new DefaultMessageSerializer<CharmStatisticsMessage>() },
                { MessageCommands.GroupMemberUpdate, new DefaultMessageSerializer<GroupMemberUpdateEvent>() },
                { MessageCommands.GroupAdmin, new DefaultMessageSerializer<GroupAdminMessage>() },
                { MessageCommands.SubscriberUpdate, new DefaultMessageSerializer<UserUpdateEvent>() },
                { MessageCommands.NotificationListClear, new DefaultMessageSerializer<ClearNotificationsMessage>() },
                { MessageCommands.MessageGroupUnsubscribe, new DefaultMessageSerializer<UnsubscribeFromGroupMessage>() },
                { MessageCommands.MessagePrivateUnsubscribe, new DefaultMessageSerializer<UnsubscribeFromPrivateMessage>() },
                // group join and leave
                { MessageCommands.GroupMemberAdd, new GroupJoinLeaveMessageSerializer<GroupJoinMessage>() },
                { MessageCommands.GroupMemberDelete, new GroupJoinLeaveMessageSerializer<GroupLeaveMessage>() },
                // contact add and delete
                { MessageCommands.SubscriberContactAdd, new ContactAddDeleteMessageSerializer<ContactAddMessage>() },
                { MessageCommands.SubscriberContactDelete, new ContactAddDeleteMessageSerializer<ContactDeleteMessage>() },
                // chat message
                { MessageCommands.MessageSend, new ChatMessageSerializer() },
                // entity updates
                { MessageCommands.SubscriberProfileUpdate, new UserUpdateMessageSerializer() }
            };
        }

        public DefaultMessageSerializerMap(IEnumerable<KeyValuePair<string, IMessageSerializer>> additionalMappings, 
            IMessageSerializer fallbackSerializer = null) : this(fallbackSerializer)
        {
            foreach (var pair in additionalMappings)
                this.MapSerializer(pair.Key, pair.Value);
        }

        public IMessageSerializer FindMappedSerializer(string key)
        {
            this._map.TryGetValue(key, out IMessageSerializer result);
            return result;
        }

        public void MapSerializer(string key, IMessageSerializer serializer)
            => this._map[key] = serializer;
    }
}
