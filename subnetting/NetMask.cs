using System;

namespace subnetting
{
    public class NetMask
    {
        public int Bytes {get; set;}
        public int[] Data {get; set;} = new int[32];

        public int FirstHostOctet 
        {
            get 
            {
                for(int i = 0; i < 32; i ++)
                    if(Data[i] == 0)
                        if(i >= 24)      return 3;
                        else if(i >= 16) return 2;
                        else if(i >= 8)  return 1;
                        else             return 0;
                return 0;
            }
        }

        public int IndexOfLastNetworkBit
        {
            get 
            {
                for(int i = 0; i < 32; i ++)
                    if(Data[i] == 0) return i - 1;
                return 0;
            }
        }
        public NetMask()
        {
            
        }

        public NetMask(int bytes)
        {
            for(int i = 0; i < bytes; i++) Data[i] = 1;
            Bytes = bytes;
        }
        
        public override string ToString()
        {
           var mask = "";
            for(var i = 0; i < 32; i++){
                if(i % 8 == 0 && i != 0) mask += ".";
                mask += Data[i];
            }
            return mask;
        }

        public NetMask AddBytes(int bytes)
        {
            var newBytes = Bytes + bytes;
            if(newBytes > 32) return this;
            //Increas bytes
            Bytes += bytes;
            // Update mask
            for(int i = 0; i < Bytes; i++) Data[i] = 1;

            return this;
        }
        public IP ToIP()
        {
            var octets = new int[4];
            for(int i = 0; i < 4; i ++)
            {
                string octet = "";
                for(int x = 0; x < 8; x ++) octet += Data[i * 8 + x];
                octets[i] = Convert.ToInt32(octet, 2);
            }
            return new IP { Octets = octets };
        }
        public static NetMask CopyFrom(NetMask netMask)
        {
            return new NetMask(netMask.Bytes);
        }

    }

}