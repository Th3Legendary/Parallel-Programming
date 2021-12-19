using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ElevatorSim
{
    class Agent
    {
        public Clearance clearance;
        public Floors currentFloor;
        private Base area51;
        private AgentStatus status = AgentStatus.OutsideElevator;
        public string Name { get; set; }

        Random random = new Random();

        public Agent(string name, Base b)
        {
            Name = name;
            area51 = b;
            GenerateClearance();
        }

        private void GenerateClearance()
        {
            int val = random.Next(30); // default 30
            if (val < 30) clearance = Clearance.Confidential;
            if (val < 15) clearance = Clearance.Secret | Clearance.Confidential;
            if (val < 5) clearance = Clearance.TopSecret | Clearance.Secret | Clearance.Confidential;                     
        }

        private Activity GetRandomActivity()
        {
            int val = random.Next(20); // make it 20 to lower the chance of the agent leaving the base
            if (val < 5) return Activity.WalkAround;
            if (val < 10) return Activity.GetCoffee;
            if (val < 12) return Activity.GrabLunch;
            if (val < 17) return Activity.CallElevator;
            return Activity.Leave;
        }
        private Floors GetRandomFloor(Floors floor)
        {
            Floors chosenFloor;
            do
            { 
                int val = random.Next(20);
                chosenFloor = Floors.TopSecret2;
                if (val < 15) chosenFloor = Floors.TopSecret1;
                if (val < 10) chosenFloor = Floors.Secret;
                if (val < 5) chosenFloor= Floors.Ground;
            } while (chosenFloor == floor);
            return chosenFloor;
        }
        private void decisionsHandler()
        {
            while (status == AgentStatus.AwaitingElevator)
            {
                if (area51.elevator.CurrentFloor == currentFloor && area51.elevator.openDoor)
                {
                    status = AgentStatus.InElevator;
                    Floors newFloor = GetRandomFloor(currentFloor);
                    Console.WriteLine($"{Name} entered the elevator and wants to go to the {area51.FloorEnumToStringConverter(newFloor)}.");
                    area51.elevator.SwitchFloors(newFloor);

                    while (status == AgentStatus.InElevator)
                    {
                        area51.elevator.isEmpty = false;
                        if(area51.elevator.CurrentFloor == newFloor)
                        {
                            if (!area51.elevator.Leave(this))
                            {
                                newFloor = GetRandomFloor(area51.elevator.CurrentFloor);
                                area51.elevator.SwitchFloors(newFloor);
                                Console.WriteLine($"{Name} doesn't have the required security clearance for this floor so they have decided to go to the {area51.FloorEnumToStringConverter(newFloor)} instead.");
                                //continue;
                            }
                            else
                            {
                                status = AgentStatus.OutsideElevator;
                                currentFloor = newFloor;
                                Console.WriteLine($"{Name} has sufficient clearance and has left the elevator at the {area51.FloorEnumToStringConverter(currentFloor)}.");
                                area51.elevator.semaphore.Release();
                            }
                            
                        }
                        Thread.Sleep(100);
                    }
                }
                Thread.Sleep(100);
            }
        }

        public void EnterBase()
        {           
            bool atBase = true;
            currentFloor = Floors.Ground;
            Console.WriteLine($"{Name} has entered Area 21 on the {area51.FloorEnumToStringConverter(currentFloor)}.");
            Thread.Sleep(100);

            while (atBase & status == AgentStatus.OutsideElevator)
            {                
                switch (GetRandomActivity())
                {
                    case Activity.WalkAround:
                        Console.WriteLine($"{Name} has decided to stretch his legs by walking around on the {area51.FloorEnumToStringConverter(currentFloor)}.");
                        Thread.Sleep(2000);
                        break;
                    case Activity.GetCoffee:
                        Console.WriteLine($"{Name} is drowsy, so he's picking up a coffee on the {area51.FloorEnumToStringConverter(currentFloor)}.");
                        Thread.Sleep(1000);
                        break;
                    case Activity.GrabLunch:
                        Console.WriteLine($"{Name} is hungry and going to eat lunch on the {area51.FloorEnumToStringConverter(currentFloor)}.");
                        Thread.Sleep(5000);
                        break;
                    case Activity.CallElevator:
                        area51.elevator.semaphore.WaitOne();
                        area51.elevator.CallElevator(currentFloor);
                        Console.WriteLine($"{Name} has called the elevator on the {area51.FloorEnumToStringConverter(currentFloor)}.");
                        status = AgentStatus.AwaitingElevator;
                        decisionsHandler();
                        break;
                    case Activity.Leave:
                        Console.WriteLine($"{this.Name} has decided to leave the base.");
                        atBase = false;
                        // make the agent call the elevator to the floor they are on if they are not on the ground floor
                        // and transport them down to the ground floor so they can leave the base without flying
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            Console.WriteLine($"{Name} has gone back home.");
        }
    }
}
