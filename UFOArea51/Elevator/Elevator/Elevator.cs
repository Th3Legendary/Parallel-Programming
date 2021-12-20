using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ElevatorSim
{
    class Elevator
    {
        private int Speed { get; } = 1000; // 1000ms = 1s
        public Floors CurrentFloor = Floors.Ground;
        public Floors NextFloor = Floors.Ground;
        public Semaphore agentQueue = new Semaphore(1, 1);
        private Base area51;
        public volatile bool openDoor = true;
        public bool isEmpty = true;
        public ManualResetEvent mre = new ManualResetEvent(false);
        public Semaphore elevatorQueue = new Semaphore(0, 1);

        public Elevator (Base b)
        {
            area51 = b;
        }

        public void StartElevator()
        {
            while (area51.agentsInside)
            {
                elevatorQueue.WaitOne();
                if (CurrentFloor != NextFloor)
                {                   
                    openDoor = false;
                    Console.WriteLine("The elevator door has closed.");

                    int floorDifferential= Math.Abs((int)CurrentFloor - (int)NextFloor); 
                    if (floorDifferential > 0) Console.WriteLine($"Elevator starts moving. Floors to travel: {floorDifferential}");

                    for (int i = 0; i < floorDifferential; i++)
                    {
                        Thread.Sleep(Speed);
                        Console.WriteLine("Traveled one floor.");
                    }

                    CurrentFloor = NextFloor;
                    openDoor = isEmpty;
                    Console.WriteLine($"The elevator has reached the {area51.FloorEnumToStringConverter(CurrentFloor)}.");
                    elevatorQueue.Release();
                }
                else
                {
                    elevatorQueue.Release();
                }
            }
        }

        public void CallElevator(Floors floor) 
        {
            NextFloor = floor;
            isEmpty = true;       
            elevatorQueue.Release();                     
        }

        public void SwitchFloors(Floors floor)
        {
            NextFloor = floor;
            elevatorQueue.Release();
        }

        public bool Leave(Agent agent)
        {
            if ((int)agent.clearance >= (int)CurrentFloor) //checks agent clearance against floor requirement
            {
                openDoor = true;
                isEmpty = true;
            }
            return openDoor;
        }
    }
}
