using System;

namespace svtrobotics {
    public class SearchRequestModel {
        // string is prolly a better option for an ID   
        public string LoadId {get;set;}

        public decimal X {get;set;}
        public decimal Y {get;set;}
    }
}