﻿using System;
using System.Threading.Tasks;
using TehGM.Wolfringo.Messages;
using TehGM.Wolfringo.Messages.Responses;

namespace TehGM.Wolfringo.Examples.SimplePingBot
{
    class Program
    {
        static IWolfClient _client;
        static async Task Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            _client = new WolfClient();
            _client.MessageReceived += OnMessageReceived;
            await _client.ConnectAsync();
            await Task.Delay(-1);
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e);
        }

        private static async void OnMessageReceived(IWolfMessage obj)
        {
            Console.WriteLine("Message received: " + obj.Command);
            if (obj is WelcomeMessage)
            {
                Config config = Config.Load();
                LoginResponse loginResponse = await _client.SendAsync<LoginResponse>(new LoginMessage(config.Username, config.Password));
                WolfResponse response = await _client.SendAsync(new SubscribeToPmMessage());
            }
            else if (obj is ChatMessage msg)
            {
                if (msg.IsText)
                    Console.WriteLine(msg.Text);
            }
        }
    }
}
