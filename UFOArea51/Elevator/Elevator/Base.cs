using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ElevatorSim
{
    class Base
    {
        public Elevator elevator;
        public bool agentsInside = false;
        public Base()
        {
            elevator = new Elevator(this);
        }

        public void Open(int agentCount)
        {
            var elevatorThread = new Thread(elevator.StartElevator);
            agentsInside = true;
            elevatorThread.Start();

            List<Thread> threads = new List<Thread>();

            for (int i = 0; i < agentCount; i++)
            {
                Agent agent = new Agent(i.ToString(), this);
                var thread = new Thread(agent.EnterBase);
                thread.Start();
                threads.Add(thread);
            }
            
            foreach (var t in threads) t.Join();
            agentsInside = false;
            Console.WriteLine("--------- All agents have gone home, closing Area 51.---------");
        }
        public string FloorEnumToStringConverter(Floors floor)
        {
            switch (floor)
            {
                case Floors.Ground:
                    return "ground floor";
                case Floors.Secret:
                    return "secret floor";
                case Floors.TopSecret1:
                    return "top secret floor";
                case Floors.TopSecret2:
                    return "top² secret floor";
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
