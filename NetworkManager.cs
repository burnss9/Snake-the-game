﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SnakeGame
{

    public class ObjectNetID
    {
        public int ObjectID { get; set; }
        public int OwnerID { get; set; }
        public ObjectNetID(int ObjectID, int OwnerID)
        {
            this.ObjectID = ObjectID;
            this.OwnerID = OwnerID;
        }

        public override bool Equals(object obj)
        {
            if (obj is ObjectNetID)
            {
                return OwnerID == ((ObjectNetID)obj).OwnerID && ObjectID == ((ObjectNetID)obj).ObjectID;

            }
            else
            {
                return base.Equals(obj);
            }

        }

        public override int GetHashCode()
        {
            return (OwnerID * 10000).GetHashCode() + ObjectID.GetHashCode();
        }

    }

    public class NetworkManager
    {
        string hostAddress = "localhost";

        int listenPort = 7707;

        public int LocalClientID = 0;

        public Dictionary<ObjectNetID, ISerializable> NetworkObjects = new Dictionary<ObjectNetID, ISerializable>();
        public Dictionary<int, Socket> Clients = new Dictionary<int, Socket>();

        public Socket Host = new Socket(SocketType.Stream, ProtocolType.Tcp);

        public Game game;
        public bool isServer;

        int Players = 0;
        int Objects = 0;

        int TickCount = 0;
        int TickDelay = 10;


        public NetworkManager(Game game, bool isServer, int TickDelay)
        {
            this.game = game;
            this.isServer = isServer;

            if (isServer)
            {
                Console.WriteLine("");
                ServerSocket ServerSocket = new ServerSocket(this);
            }
            else
            {
                try
                {
                    Host.Connect(hostAddress, 7707);
                    Console.WriteLine("connected");
                    byte[] message = new byte[4];
                    Host.Receive(message);
                    Console.WriteLine("received");
                    LocalClientID = BitConverter.ToInt32(message, 0);

                }
                catch (Exception)
                {
                    Console.WriteLine("error");
                }

            }

        }

        public void RegisterPlayer(Socket s)
        {

            int id = Players;
            Players++;

            byte[] playerJoinInfo = BitConverter.GetBytes(id);

            ObjectNetID objectNetID = new ObjectNetID(Objects++, id);

            NetworkObjects.Add(objectNetID, game.RegisterSnake(objectNetID));

            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.SetBuffer(playerJoinInfo, 0, playerJoinInfo.Count());

            if (s != null)
            {
                Clients.Add(id, s);
                s.SendAsync(args);
            }

            foreach (var o in NetworkObjects)
            {
                o.Value.setNetworkClean(false);
            }


        }

        public void NetworkTick()
        {

            if (isServer)
            {

                foreach (KeyValuePair<int, Socket> pair in Clients)
                {
                    Socket s = pair.Value;

                    if (s.Available > 0)
                    {
                        byte[] buffer = new byte[256];

                        s.Receive(buffer);

                        string r = Encoding.ASCII.GetString(buffer);

                        string command = r.Split(':')[0];

                        switch (command)
                        {

                            case "Turn":

                                Direction d = (Direction)BitConverter.ToInt16(buffer, 5);

                                Snake snake = null;

                                foreach (var netobj in NetworkObjects)
                                {
                                    if (netobj.Key.OwnerID == pair.Key)
                                    {
                                        snake = (Snake)netobj.Value;
                                    }
                                }
                                if (snake != null)
                                {

                                    snake.SetDirection(d);
                                }
                                break;


                        }


                    }

                }

            }
            else
            {

                if (Host.Available > 0)
                {
                    int Available = Host.Available;

                    byte[] Message = new byte[2048];
                    Host.Receive(Message);

                    Console.WriteLine("Bytes: " + Message.Count());

                    int bytesRead = 0;

                    while (bytesRead < Available)
                    {
                        int objClass = BitConverter.ToInt32(Message, bytesRead);
                        bytesRead += sizeof(Int32);
                        int ownerID = BitConverter.ToInt32(Message, bytesRead);
                        bytesRead += sizeof(Int32);
                        int objectID = BitConverter.ToInt32(Message, bytesRead);
                        bytesRead += sizeof(Int32);
                        int objLength = BitConverter.ToInt32(Message, bytesRead);
                        bytesRead += sizeof(Int32);

                        byte[] obj = new byte[objLength];
                        Buffer.BlockCopy(Message, bytesRead, obj, 0, objLength);

                        bytesRead += objLength;

                        ObjectNetID objectNetID = new ObjectNetID(objectID, ownerID);
                        if (NetworkObjects.ContainsKey(objectNetID))
                        {
                            NetworkObjects[objectNetID].Deserialize(obj);
                        }
                        else
                        {
                            Console.WriteLine("adding network object: " + objectID + " " + ownerID);
                            Console.WriteLine(Encoding.ASCII.GetChars(obj));

                            Snake s = new Snake(new Point(2, 2), game.gameField);
                            game.gameField.Snakes.Add(s);
                            s.Deserialize(obj);
                            s.objectNetID = new ObjectNetID(objectID, ownerID);
                            NetworkObjects.Add(s.objectNetID, s);
                        }

                    }

                }
            }

            if (TickCount < TickDelay)
            {
                TickCount++;
            }
            else
            {
                //Console.WriteLine("Network tick server");

                TickCount = 0;
                //Network Tick
                if (isServer)
                {

                    IEnumerable<byte> Replication = new byte[0];

                    foreach (KeyValuePair<ObjectNetID, ISerializable> pair in NetworkObjects)
                    {
                        if (!pair.Value.isNetworkDirty())
                        {
                            continue;
                        }
                        pair.Value.setNetworkClean(true);

                        byte[] classID = BitConverter.GetBytes(1);

                        byte[] objectBytes = pair.Value.Serialize();

                        byte[] objLength = BitConverter.GetBytes(objectBytes.Count());

                        byte[] ownerID = BitConverter.GetBytes(pair.Key.OwnerID);

                        byte[] objectID = BitConverter.GetBytes(pair.Key.ObjectID);

                        Replication = Replication.Concat(classID).Concat(ownerID).Concat(objectID).Concat(objLength).Concat(objectBytes);

                    }

                    foreach (KeyValuePair<int, Socket> s in Clients)
                    {
                        SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                        byte[] message = Replication.ToArray();
                        args.SetBuffer(message, 0, message.Count());

                        Console.WriteLine("Sending " + message.Count() + " bytes.");

                        s.Value.SendAsync(args);
                    }


                }
            }
        }

    }

    public class ServerSocket
    {

        Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        NetworkManager manager;
        bool active = true;
        int listenPort = 7707;

        public ServerSocket(NetworkManager manager)
        {
            this.manager = manager;
            new Thread(Listen).Start();

        }

        public void Listen()
        {
            try
            {

                //IPAddress ip = Dns.GetHostEntry(IPAddress.Any.ToString()).AddressList[0];
                //socket.Bind(new IPEndPoint(ip, listenPort));

                socket.Bind(new IPEndPoint(IPAddress.Any, listenPort));
                socket.Listen(3);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }


            while (active)
            {
                Console.WriteLine("accepting...");
                manager.RegisterPlayer(socket.Accept());
                Console.WriteLine("Accepted?");

            }

        }

    }


}


