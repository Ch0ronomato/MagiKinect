using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagiKinect
{
    class KinectNotFoundException : System.ApplicationException
    {
        public KinectNotFoundException() {}
        public KinectNotFoundException(string message) {}
        public KinectNotFoundException(string message, System.Exception inner) {}
 
        // Constructor needed for serialization 
        // when exception propagates from a remoting server to the client.
        protected KinectNotFoundException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) {}
    }
}
