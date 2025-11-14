using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demoHaychongiadung
{
    // ==================== DESIGN PATTERNS ====================

    // Interface Minigame
    public interface IMinigame
    {
        void Play();
    }

    // Factory Pattern
    public class GameFactory
    {
        public static IMinigame CreateGame(string type)
        {
            type = type?.Trim() ?? "";
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentException("Tên minigame không hợp lệ!");
            }
            if (type == "Cao hay thấp")
            {
                return new HighOrLowWrapper();
            }
            if (type == "Giá nào đúng")
            {
                return new WhichIsRightWrapper();
            }
            if (type == "Sắp xếp giá tăng dần")
            {
                return new SortByPriceWrapper();
            }
            else
            {
                throw new ArgumentException($"Không tồn tại minigame: {type}");
            }
        }
    }

    // Wrapper classes for IMinigame interface
    public class HighOrLowWrapper : IMinigame
    {
        public void Play()
        {
            Console.WriteLine("Chào mừng đến với minigame: Cao hay thấp!");
        }
    }

    public class WhichIsRightWrapper : IMinigame
    {
        public void Play()
        {
            Console.WriteLine("Chào mừng đến với minigame: Giá nào đúng!");
        }
    }

    public class SortByPriceWrapper : IMinigame
    {
        public void Play()
        {
            Console.WriteLine("Chào mừng đến với minigame: Sắp xếp giá tăng dần!");
        }
    }

    // Player Manager
    public class PlayerManager
    {
        private List<Player> players = new List<Player>();

        public void AddPlayer(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("Bạn không được để trống tên");

            foreach (Player p in players)
            {
                if (p.Ten.Equals(name, StringComparison.OrdinalIgnoreCase))
                    throw new DuplicateNameException($"Tên '{name}' đã được chọn. Vui lòng nhập tên khác");
            }
            players.Add(new Player(name));
            Console.WriteLine($"✓ Thêm người chơi: {name}");
        }

        public List<Player> GetPlayers()
        {
            return players;
        }
    }

    // Game UI
    public class GameUI
    {
        public void ShowMessage(string msg)
        {
            Console.WriteLine(msg);
        }
    }

    // Facade Pattern
    public class GameShowManager
    {
        private readonly PlayerManager players = new PlayerManager();
        private readonly GameUI ui = new GameUI();

        public void StartShow()
        {
            ui.ShowMessage("Chào mừng đến với Hãy chọn giá đúng!");
        }

        public void PlayMinigame(IMinigame game, string gameType)
        {
            Console.WriteLine($"Bắt đầu chơi Minigame {gameType}");
            game.Play();
        }

        public void EndShow()
        {
            ui.ShowMessage("Chương trình kết thúc. Hẹn gặp lại!");
        }

        public PlayerManager GetPlayerManager()
        {
            return players;
        }
    }

    // Command Pattern - Interface
    public interface ICommand
    {
        void Execute();
    }

    // Game Controller (Receiver)
    public class GameController
    {
        private readonly GameShowManager manager;

        public GameController(GameShowManager manager)
        {
            this.manager = manager;
        }

        public void StartShow()
        {
            manager.StartShow();
        }

        public void PlayMinigame(string gameType)
        {
            IMinigame game = GameFactory.CreateGame(gameType);
            manager.PlayMinigame(game, gameType);
        }

        public void EndShow()
        {
            manager.EndShow();
        }
    }

    // Command: Start Show
    public class StartShowCommand : ICommand
    {
        private readonly GameController controller;

        public StartShowCommand(GameController controller)
        {
            this.controller = controller;
        }

        public void Execute()
        {
            controller.StartShow();
        }
    }

    // Command: Play Minigame
    public class PlayMinigameCommand : ICommand
    {
        private readonly GameController controller;
        private readonly string gameType;

        public PlayMinigameCommand(GameController controller, string gameType)
        {
            this.controller = controller;
            this.gameType = gameType;
        }

        public void Execute()
        {
            controller.PlayMinigame(gameType);
        }
    }

    // Command: End Show
    public class EndShowCommand : ICommand
    {
        private readonly GameController controller;

        public EndShowCommand(GameController controller)
        {
            this.controller = controller;
        }

        public void Execute()
        {
            controller.EndShow();
        }
    }

    // Invoker
    public class GameHost
    {
        private List<ICommand> history = new List<ICommand>();

        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            history.Add(command);
        }

        public List<ICommand> GetHistory()
        {
            return history;
        }
    }
}
