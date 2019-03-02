namespace WXT.SuperMarket.UI.CustomerConsoleApp
{
    using System;
    using WXT.SuperMarket.Business.Services;

    /// <summary>
    /// Defines the <see cref="Program" />
    /// </summary>
    class Program
    {
        /// <summary>
        /// The Main
        /// </summary>
        /// <param name="args">The args<see cref="string[]"/></param>
        static void Main(string[] args)
        {
            CustomerService customerService = new CustomerService();
            MarketService marketService = new MarketService();
            bool canExit = true;
            while (canExit)
            {
                Console.WriteLine("_________________________________________");
                Console.WriteLine("-----------------------------------------");
                Console.WriteLine("Please input command:");
                Console.WriteLine("Example: addproduct name 1.23, removeproduct id, getallproduct false" + Environment.NewLine +
                                  "         addtostock id count, removefromstock id count");
                Console.WriteLine("         newcustomer name password, login name password, logout, deletecustomer, " + Environment.NewLine +
                                  "         addtocart id count, takefromcart id count, clearcart, checkout, getallproduct false");
                Console.Write("Command:  ");
                string command = Console.ReadLine().ToLowerInvariant();
                try
                {
                    switch (command)
                    {
                        case string c when c.StartsWith("addproduct"):
                            var commands = c.Split(' ');
                            if (commands.Length != 3)
                            {
                                Console.WriteLine("Need two parameters: name and price");
                                break;
                            }
                            string product = marketService.AddProduct(commands[1], decimal.Parse(commands[2]));
                            Console.WriteLine("Add new Product succeeded.");
                            Console.WriteLine(product);
                            break;

                        case string c when c.StartsWith("removeproduct"):
                            commands = c.Split(' ');
                            if (commands.Length != 2)
                            {
                                Console.WriteLine("Need one parameter: id");
                                break;
                            }
                            marketService.RemoveProduct(int.Parse(commands[1]));
                            Console.WriteLine("Remove Product succeeded.");
                            break;

                        case string c when c.StartsWith("addtostock"):
                            commands = c.Split(' ');
                            if (commands.Length != 3)
                            {
                                Console.WriteLine("Need two parameters: id and count");
                                break;
                            }
                            marketService.AddToStock(int.Parse(commands[1]), int.Parse(commands[2]));
                            Console.WriteLine("Add to stock succeeded.");
                            break;

                        case string c when c.StartsWith("removefromstock"):
                            commands = c.Split(' ');
                            if (commands.Length != 3)
                            {
                                Console.WriteLine("Need two parameters: id and count");
                                break;
                            }
                            marketService.RemoveFromStock(int.Parse(commands[1]), int.Parse(commands[2]));
                            Console.WriteLine("Remove from stock succeeded.");
                            break;

                        case string c when c.StartsWith("newcustomer"):
                            commands = c.Split(' ');
                            if (commands.Length != 3)
                            {
                                Console.WriteLine("Need two parameters: name and password");
                                break;
                            }
                            string customer = customerService.RegisterNewCustomer(commands[1], commands[2]);
                            Console.WriteLine("Add new customer succeeded.");
                            Console.WriteLine(customer);
                            break;

                        case string c when c.StartsWith("login"):
                            commands = c.Split(' ');
                            if (commands.Length != 3)
                            {
                                Console.WriteLine("Need two parameters: name and password");
                                break;
                            }
                            customerService.Login(commands[1], commands[2]);
                            Console.WriteLine("Login succeeded, you can begin to buy things.");
                            break;

                        case string c when c.StartsWith("logout"):
                            customerService.Logout();
                            Console.WriteLine("Logout succeeded. Bye-bye");
                            break;

                        case string c when c.StartsWith("deletecustomer"):
                            customerService.DeleteCustomer();
                            Console.WriteLine("Delete customer and logout succeeded. Bye-bye.");
                            break;

                        case string c when c.StartsWith("addtocart"):
                            commands = c.Split(' ');
                            if (commands.Length != 3)
                            {
                                Console.WriteLine("Need two parameters: id and count");
                                break;
                            }
                            customerService.AddtoCart(int.Parse(commands[1]), int.Parse(commands[2]));
                            Console.WriteLine("Add to cart succeeded.");
                            break;

                        case string c when c.StartsWith("takefromcart"):
                            commands = c.Split(' ');
                            if (commands.Length != 3)
                            {
                                Console.WriteLine("Need two parameters: id and count");
                                break;
                            }
                            var realCount = customerService.TakeFromCart(int.Parse(commands[1]), int.Parse(commands[2]));
                            Console.WriteLine($"Remove {realCount} unit of product {int.Parse(commands[1])} from cart.");
                            break;

                        case string c when c.StartsWith("clearcart"):
                            customerService.ClearCart();
                            Console.WriteLine("Clear cart succeeded.");
                            break;

                        case string c when c.StartsWith("checkout"):
                            var receipt = customerService.CheckOut();
                            Console.WriteLine("Checkout succeeded.");
                            Console.WriteLine(receipt);
                            break;

                        case string c when c.StartsWith("getallproduct"):
                            commands = c.Split(' ');
                            if (commands.Length != 2)
                            {
                                Console.WriteLine("Need one parameter: true for in stock only, false for all");
                                break;
                            }
                            Console.WriteLine(customerService.FindAllProduct(bool.Parse(commands[1])));
                            break;

                        case "q":
                            canExit = false;
                            break;

                        default:
                            Console.WriteLine("Invalid command.");
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
