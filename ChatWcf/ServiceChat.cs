using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace ChatWcf
{
   // All clients connected to the host will have only one (silgle) service 
   [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServiceChat : IServiceChat
    {
        List<ServerUser> users = new List<ServerUser>();
        int nextId = 1; // ID generator

        // Create a new user
        public int Connect(string name)
        {
            ServerUser user = new ServerUser()
            {
                ID = nextId, Name = name, operationContext = OperationContext.Current
            };
            nextId++; // increment ID number

            SendMessage($": {user.Name} joint chat!", 0); 

            // Add user to the list of users
            users.Add(user);

            return user.ID;
        }

        public void Disconnect(int id)
        {
            // Looking for a user by ID
            var user = users.FirstOrDefault(i => i.ID == id);

            // If user exists, desconnect the user
            if (user != null)
            {
                users.Remove(user);
                SendMessage($": {user.Name} left chat!", 0);
            }
        }

        public void SendMessage(string msg, int id)
        {
            foreach (var item in users)
            {
                string answer = DateTime.Now.ToShortTimeString();

                var user = users.FirstOrDefault(i => i.ID == id);
                if (user != null)
                {
                    answer += $": {user.Name}: ";
                }

                answer += msg;

                item.operationContext.GetCallbackChannel<IServerChatCallback>().MessageCallBack(answer);
            }
        }
    }
}
