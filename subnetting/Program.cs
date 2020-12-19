using System;

namespace subnetting
{
    class Program
    {
        static void Main(string[] args)
        {
            Program program = new Program();
         
            System.Console.WriteLine("Welcome!");


            int option = 0;
            do
            {
                Console.Clear();
                System.Console.WriteLine("-----------------------------------------------");
                System.Console.WriteLine("                     Menu");
                System.Console.WriteLine("-----------------------------------------------");
                System.Console.WriteLine("  1 -. Get network info by CIDR with class.");
                System.Console.WriteLine("  2 -. Get network info by CIDR without class.");
                System.Console.WriteLine("  3 -. Subnets by CIDR.");
                System.Console.WriteLine("  4 -. Get Netmask from CIDR");
                System.Console.WriteLine("  5 -. Exit.");
                System.Console.Write("  > ");
                option = Convert.ToInt32(Console.ReadLine());

                if(option == 5) break;

                Console.Clear();
                switch (option)
                {
                    case 1:
                        System.Console.WriteLine("-----------------------------------------------");
                        System.Console.WriteLine("          Network Info by CIDR with class");
                        System.Console.WriteLine("-----------------------------------------------");
                        GetNetworkInfoByCIDR();
                        break;
                    case 2:
                        System.Console.WriteLine("-----------------------------------------------");
                        System.Console.WriteLine("        Network Info by CIDR without class");
                        System.Console.WriteLine("-----------------------------------------------");
                        GetNetorkInfoFromCIDRWithoutClass();
                        break;
                    case 3:
                        System.Console.WriteLine("-----------------------------------------------");
                        System.Console.WriteLine("                 Subnets by CIDR");
                        System.Console.WriteLine("-----------------------------------------------");
                        SubnetsByCIDR();
                        break;
                    case 4:
                        System.Console.WriteLine("-----------------------------------------------");
                        System.Console.WriteLine("                Get Netmask from CIDR");
                        System.Console.WriteLine("-----------------------------------------------");
                        GetNetmaskFromCIDR();
                        break;
                }
                Console.ReadLine();
            }
            while(true);
        }

        private static void GetNetmaskFromCIDR ()
        {
            Program p = new Program();
            System.Console.Write("Enter CIDR    : ");
            var cidr = Console.ReadLine();

            IP ip;
            NetMask mask;
            // Get input information
            p.SplitCIDR(cidr, out ip, out mask);   
        
            System.Console.WriteLine($"Network IP   :{ip}");
            System.Console.WriteLine($"Network Mask :{mask}");
            System.Console.WriteLine($"Network Mask :{mask.ToIP()}");
        }
        private static void GetNetorkInfoFromCIDRWithoutClass()
        {
            Program p = new Program();
            System.Console.Write("Enter CIDR    : ");
            var cidr = Console.ReadLine();

            IP ip;
            NetMask mask;
            // Get input information
            p.SplitCIDR(cidr, out ip, out mask);

            var bIP = ip.ToBinary(); 
            var bNetmask = mask.ToIP().ToBinary();
            var bNetIP = p.ANDBinary(bIP, bNetmask);

            var networkIP = new IP(Convert.ToInt64(bNetIP, 2));
            var broadcast = p.CalculateBroadcastAddressWithoutClass(mask, networkIP);
            var firtIP = new IP(networkIP.AsInt64() + 1);
            var lastIP = new IP(broadcast.AsInt64() - 1);

            System.Console.WriteLine("\n");
            System.Console.WriteLine("Results");
            System.Console.WriteLine("-----------------------------------");
            System.Console.WriteLine($"Network IP   :{networkIP}");
            System.Console.WriteLine($"First IP     :{firtIP}");
            System.Console.WriteLine($"Last IP      :{lastIP}");
            System.Console.WriteLine($"Broadcast IP :{broadcast}");
        }
        private static void GetNetworkInfoByCIDR()
        {
            Program p = new Program();
            System.Console.Write("Enter CIDR    : ");
            var cidr = Console.ReadLine();

            IP ip;
            NetMask mask;
            // Get input information
            p.SplitCIDR(cidr, out ip, out mask);

            var bIP = ip.ToBinary(); 
            var bNetmask = mask.ToIP().ToBinary();
            var bNetIP = p.ANDBinary(bIP, bNetmask);

            var networkIP = new IP(Convert.ToInt64(bNetIP, 2));
            var broadcast = p.CalculateBroadcastAddressWithClass(mask, networkIP);
            var firtIP = new IP(networkIP.AsInt64() + 1);
            var lastIP = new IP(broadcast.AsInt64() - 1);

            System.Console.WriteLine("\n");
            System.Console.WriteLine("Results");
            System.Console.WriteLine("-----------------------------------");
            System.Console.WriteLine($"Network IP   : {networkIP}");
            System.Console.WriteLine($"First IP     : {firtIP}");
            System.Console.WriteLine($"Last IP      : {lastIP}");
            System.Console.WriteLine($"Broadcast IP : {broadcast}");

        }
        private IP CalculateBroadcastAddressWithoutClass(NetMask mask, IP networkIP)
        {
            var lastIndex = 1 + mask.IndexOfLastNetworkBit;
            var bNetIp = networkIP.ToBinary().ToCharArray();
            for(int i = lastIndex; i < 32; i++)
                bNetIp[i] = '1';

            var x = new string(bNetIp);

            return new IP(Convert.ToInt64(x, 2));
        }
        private IP CalculateBroadcastAddressWithClass(NetMask mask, IP networkIP)
        {
            var hostOctet = mask.FirstHostOctet;
            var bNetIp = networkIP.ToBinary().ToCharArray();

            for(int i = hostOctet * 8; i < 32; i++)
                bNetIp[i] = '1';

            var x = new string(bNetIp);

            return new IP(Convert.ToInt64(x, 2));
        }
        private string ANDBinary(string a, string b)
        {   
            if(a.Length != b.Length) return "00000000000000000000000000000000";
            string c = "";
            for(int i = 0; i < a.Length; i ++)
                c += a[i] == '1' && b[i] == '1' ? '1' : '0';

            return c;
        }
        
        private static void SubnetsByCIDR(){
            Program program = new Program();
            System.Console.Write("Enter CIDR Notation : ");
            var cidr = Console.ReadLine();
            System.Console.Write("Number of subnets   : ");
            var numberOfNetworks = Convert.ToInt32(Console.ReadLine());

            var netWorkIP   = new IP();
            var currentMask = new NetMask();
            // Get input information
            program.SplitCIDR(cidr, out netWorkIP, out currentMask);
            // Calculate required bytes for new network mask
            var requiredBytes   = program.CalculateRequiredBytes(currentMask.Bytes, numberOfNetworks); 
            // Get new network mask.
            var newMask = NetMask.CopyFrom(currentMask).AddBytes(requiredBytes);
            // Get Number of hosts
            var numberOfHosts = program.CalculateNumberOfHosts(newMask);
            // Calculate network jump
            var networkJump = program.CalculateNetworkJump(newMask);

            System.Console.WriteLine("-----------------------------------");
            System.Console.WriteLine($"Current Network Mask     : {currentMask} | {currentMask.ToIP()}");
            System.Console.WriteLine($"Required Bytes ON        : {requiredBytes}");
            System.Console.WriteLine($"New Network Mask         : {newMask} | {newMask.ToIP()}");
            System.Console.WriteLine($"Hosts per Network        : {numberOfHosts}");
            System.Console.WriteLine($"Network Jump             : {networkJump}");
            System.Console.WriteLine();
            program.PrintSubnets(netWorkIP, numberOfNetworks, numberOfHosts);
        }

        

        public int CalculateRequiredBytes(int networkBytes, int subnets)
        {
            if(subnets <= 1)        return 0;
            if(subnets <= 2)        return 1;
            if(subnets <= 4)        return 2;
            if(subnets <= 8)        return 3;
            if(subnets <= 16)       return 4;
            if(subnets <= 32)       return 5;
            if(subnets <= 64)       return 6;
            if(subnets <= 128)      return 7;
            return 0;
        }

        public int CalculateNetworkJump(NetMask netMask)
        {
            var index = 0;
            var ip = netMask.ToIP();
            for(index = 0;  index < 4; index++)
                if(ip.Octets[index] != 255) break;

            return 256 - ip.Octets[index];
        }

        //Number for hosts  = 2 ^ M - 2;
        public int CalculateNumberOfHosts(NetMask netMask)
        {
            var m = 32 - netMask.Bytes;
            return (int) (Math.Pow(2.0f, m) - 2);
        }

        public void SplitCIDR(string cidr, out IP ip, out NetMask netMask)
        {
            var x = cidr.Split("/");
            ip = new IP(x[0]);
            netMask = new NetMask(Convert.ToInt32(x[1]));
        }
        public void PrintSubnets(IP networkIp, int subnetsCount, int hostCount)
        {
            IP ip = networkIp;
            var format = "{0, -3}  {1, -15}  {2, -15}  {3, -15}  {4, -15}";
            System.Console.WriteLine(format, "#", "Network IP", "First IP", "Last Ip", "Broadcast");
            System.Console.WriteLine(format, "-", "----------", "--------", "-------", "---------");
            for(var subnet = 0; subnet < subnetsCount; subnet ++)
            {
                var firstIp = new IP(ip.AsInt64() + 1);
                var lastIp = new IP(ip.AsInt64() + hostCount);
                var broadcast = new IP(ip.AsInt64() + hostCount + 1);
                System.Console.WriteLine(format, subnet + 1, ip, firstIp, lastIp, broadcast);

                ip = new IP(ip.AsInt64() + hostCount + 2);
            }
        }
        
    }
}
