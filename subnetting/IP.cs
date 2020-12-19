using System;

namespace subnetting
{
    public class IP
    {
        public int [] Octets {get; set;}  = new int[4];

        public IP()
        {
        }

        public IP(string IP)
        {
            var octets = IP.Split('.');
            for(var i = 0; i < 4; i ++)
                Octets[i] = Convert.ToInt32(octets[i]);
        }
        public IP(Int64 IP)
        {
            var binary =  Convert.ToString(IP, 2);
            
            for(var i = 0; binary.Length < 32; i++) binary = "0" + binary;

            Octets[0] = Convert.ToInt32(binary.Substring(0,  8), 2);
            Octets[1] = Convert.ToInt32(binary.Substring(8,  8), 2);
            Octets[2] = Convert.ToInt32(binary.Substring(16, 8), 2);
            Octets[3] = Convert.ToInt32(binary.Substring(24, 8), 2);
        }

        public Int64 AsInt64()
        {
            string binary = "";
            foreach(var i in Octets)
            {
                string digit = Convert.ToString(i, 2);
                for(var x = digit.Length; x < 8; x ++)
                    digit = "0" + digit;
                binary += digit;
            }
            return Convert.ToInt64(binary, 2);
        }
        public static IP FromBinaryString(string binary)
        {
            if(binary.Length < 32) return null;
            IP ip = new IP();
            ip.Octets[0] = Convert.ToInt32(binary.Substring(0,  8), 2);
            ip.Octets[1] = Convert.ToInt32(binary.Substring(8,  8), 2);
            ip.Octets[2] = Convert.ToInt32(binary.Substring(16, 8), 2);
            ip.Octets[3] = Convert.ToInt32(binary.Substring(24, 8), 2);
            return ip;
        }
        public void setByte(int position, int value)
        {
            if(position >= 0 && position < 4)
                Octets[position] = value;
        }

        public override string ToString()
        {
            var ip = "";
            for(int i = 0; i < 4; i ++)
            {    
                ip += Octets[i];
                if(i != 3)
                    ip += ".";
            }

            return ip;
        }

        public string ToBinary()
        {
            string binary = "";
            foreach(var i in Octets)
            {
                string digit = Convert.ToString(i, 2);
                for(var x = digit.Length; x < 8; x ++)
                    digit = "0" + digit;
                binary += digit;
            }
            return binary;
        }

        
    }
}