using System.Globalization;
using System.Threading.Tasks;
using JustSaying.Tools.Commands;
using Magnum.CommandLineParser;
using Magnum.Monads.Parser;

namespace JustSaying.Tools
{
    public static class CommandParser
    {
        public static async Task<bool> ParseAndExecuteAsync(string commandText)
        {
            var anyCommandFailure = false;

            await CommandLine
                .Parse<ICommand>(commandText, InitializeCommandLineParser)
                .ForEachAsync(async option =>
                {
                    var optionSuccess = await option.ExecuteAsync().ConfigureAwait(false);
                    anyCommandFailure |= !optionSuccess;
                }).ConfigureAwait(false);

            return anyCommandFailure;
        }

        private static void InitializeCommandLineParser(ICommandLineElementParser<ICommand> x)
        {
            x.Add((from arg in x.Argument("exit")
                   select (ICommand)new ExitCommand())
                .Or(from arg in x.Argument("quit")
                    select (ICommand)new ExitCommand())
                .Or(from arg in x.Argument("help")
                    select (ICommand)new HelpCommand())

                .Or(from arg in x.Argument("move")
                    from sourceQueueName in x.Definition("from")
                    from destinationQueueName in x.Definition("to")
                    from region in x.Definition("in")
                    from count in (from d in x.Definition("count") select d).Optional("count", "1")
                    select (ICommand)new MoveCommand(sourceQueueName.Value, destinationQueueName.Value, region.Value,
                        int.Parse(count.Value, CultureInfo.InvariantCulture)))
                );
        }
    }
}
