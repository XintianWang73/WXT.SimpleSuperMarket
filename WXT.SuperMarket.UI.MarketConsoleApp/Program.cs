namespace WXT.SuperMarket.UI.MarketConsoleApp
{
    using System;
    using WXT.SuperMarket.Business.Services;

    class Program
    {
        static void Main(string[] args)
        {
            MarketService marketService = new MarketService();
            bool canExit = true;
            while (canExit)
            {
                Console.WriteLine("_________________________________________");
                Console.WriteLine("-----------------------------------------");
                Console.WriteLine("Please input command:");
                Console.WriteLine("Example: addproduct name 1.23, removeproduct id, getallproduct false" + Environment.NewLine +
                                  "         addtostock id count, removefromstock id count");
                Console.Write("Command:  ");
                string command = Console.ReadLine().ToLowerInvariant();
                switch (command)
                {
                    case string c when c.StartsWith("addproduct"):
                        var commands = c.Split(' ');
                        if (commands.Length != 3)
                        {
                            Console.WriteLine("Need two parameters: name and price");
                            break;
                        }
                        try
                        {
                            string product = marketService.AddProduct(commands[1], decimal.Parse(commands[2]));
                            Console.WriteLine("Add new Product succeeded.");
                            Console.WriteLine(product);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        break;
                    case string c when c.StartsWith("removeproduct"):
                        commands = c.Split(' ');
                        if (commands.Length != 2)
                        {
                            Console.WriteLine("Need one parameter: id");
                            break;
                        }
                        try
                        {
                            marketService.RemoveProduct(int.Parse(commands[1]));
                            Console.WriteLine("Remove Product succeeded.");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        break;

                    case string c when c.StartsWith("addtostock"):
                        commands = c.Split(' ');
                        if (commands.Length != 3)
                        {
                            Console.WriteLine("Need two parameters: id and count");
                            break;
                        }
                        try
                        {
                            marketService.AddToStock(int.Parse(commands[1]), int.Parse(commands[2]));
                            Console.WriteLine("Add to stock succeeded.");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        break;

                    case string c when c.StartsWith("removefromstock"):
                        commands = c.Split(' ');
                        if (commands.Length != 3)
                        {
                            Console.WriteLine("Need two parameters: id and count");
                            break;
                        }
                        try
                        {
                            marketService.RemoveFromStock(int.Parse(commands[1]), int.Parse(commands[2]));
                            Console.WriteLine("Remove from stock succeeded.");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        break;
                    case string c when c.StartsWith("getallproduct"):
                        commands = c.Split(' ');
                        if (commands.Length != 2)
                        {
                            Console.WriteLine("Need one parameter: true for in stock only, false for all");
                            break;
                        }
                        try
                        {
                            Console.WriteLine(marketService.FindAllProduct(bool.Parse(commands[1])));
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        break;
                    case "q":
                        canExit = false;
                        break;
                    default:
                        Console.WriteLine("Invalid command.");
                        break;

                }
            }
        }
    }
}
