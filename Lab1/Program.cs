namespace Lab1
{
    /// <summary>
    /// Головний клас програми Lab1.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Точка входу в програму.
        /// </summary>
        /// <param name="args">Аргументи командного рядка.</param>
        static void Main(string[] args)
        {
            Hello.SayHello();
        }
    }

    /// <summary>
    /// Клас для виведення привітання.
    /// </summary>
    internal class Hello
    {
        /// <summary>
        /// Виводить привітання у консоль.
        /// </summary>
        public static void SayHello()
        {
            Console.WriteLine("Hello, World!");
        }
    }
}
