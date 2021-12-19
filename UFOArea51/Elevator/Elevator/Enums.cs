using System;
using System.Collections.Generic;
using System.Text;

namespace ElevatorSim
{
    [Flags]
    public enum Clearance { None = 0, Confidential = 1, Secret = 2, TopSecret = 4 }

    public enum Floors { None = 0, Ground = 1, Secret = 2, TopSecret1 = 3, TopSecret2 = 4 }

    public enum Activity { WalkAround, GrabLunch, GetCoffee, CallElevator, Leave }

    public enum AgentStatus { AwaitingElevator, InElevator, OutsideElevator }
}
