using System;

namespace svtrobotics {
    public class SearchResponseModel {
        // string is prolly a better option here
        public string RobotId{get;set;}
        public decimal DistanceToGoal{get;set;}
        public decimal BatteryLevel{get;set;}
    }
}