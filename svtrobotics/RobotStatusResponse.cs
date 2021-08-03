using System;

namespace svtrobotics {
    public class RobotStatusResponse {
        public string RobotId {get;set;}
        public decimal BatteryLevel {get;set;}
        public decimal X {get;set;}
        public decimal Y {get;set;}
    }
}