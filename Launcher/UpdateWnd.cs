using SFML.Graphics;
using SFML.Window;

namespace Launcher;

internal class UpdateWnd
{
    public string DisplayText { get; set; }

    public event EventHandler InstallPortable;
    public event EventHandler InstallAppData;

    protected readonly static Font fnt;

    static UpdateWnd()
    {
        string fontsfolder = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);
        var font = Path.Combine(fontsfolder, "arial.ttf");
        fnt = new Font(font);
    }

    public void Run()
    {
        var portableBtn = new Btn(10, 10, 250, 50, "Install Portable");
        portableBtn.Click += (s, e) => InstallPortable?.Invoke(this, e);

        var appdataBtn = new Btn(270, 10, 250, 50, "Install Appdata");
        appdataBtn.Click += (s, e) => InstallAppData?.Invoke(this, e);

        var handCursor = new Cursor(Cursor.CursorType.Hand);
        var arrowCursor = new Cursor(Cursor.CursorType.Arrow);

        int x = 0, y = 0;
        var w = new RenderWindow(new VideoMode(1000, 400), "Launcher for LTestApp");
        w.Closed += (s, e) => w.Close();
        w.MouseMoved += (s, e) => { x = e.X; y = e.Y; };
        w.MouseButtonPressed += (s, e) => { portableBtn.HandleMouseClick(e.X, e.Y); appdataBtn.HandleMouseClick(e.X, e.Y); };
        w.SetFramerateLimit(10);

        while (w.IsOpen)
        {
            w.DispatchEvents();
            w.Clear(Color.White);

            if (String.IsNullOrEmpty(DisplayText))
            {
                portableBtn.Render(w);
                appdataBtn.Render(w);
                if (portableBtn.HitTest(x, y) || appdataBtn.HitTest(x, y)) w.SetMouseCursor(handCursor);
                else w.SetMouseCursor(arrowCursor);
            }
            else
            {
                using var dtext = new Text(DisplayText, fnt);
                dtext.FillColor = Color.Black;
                dtext.CharacterSize = 16;
                w.Draw(dtext);
            }

            w.Display();
        }
    }

    class Btn
    {
        private readonly int posx;
        private readonly int posy;
        private readonly int w;
        private readonly int h;
        private readonly string txt;

        public event EventHandler Click;

        public Btn(int posx, int posy, int w, int h, string txt)
        {
            this.posx = posx;
            this.posy = posy;
            this.w = w;
            this.h = h;
            this.txt = txt;
        }

        public void Render(RenderWindow r)
        {
            using var dtext = new Text(txt, fnt);
            dtext.FillColor = Color.Black;
            dtext.Position = new SFML.System.Vector2f(posx + 5, posy + 5);

            using var border = new RectangleShape();
            border.Position = new SFML.System.Vector2f(posx, posy);
            border.FillColor = new Color(206, 242, 216);
            border.OutlineColor = new Color(52, 235, 100);
            border.OutlineThickness = 2;
            border.Size = new SFML.System.Vector2f(w, h);

            r.Draw(border);
            r.Draw(dtext);
        }

        public bool HitTest(int x, int y)
        {
            return x >= posx && y >= posy && x < posx + w && y < posy + h;
        }

        public void HandleMouseClick(int x, int y)
        {
            if (!HitTest(x, y)) return;
            Click?.Invoke(this, new EventArgs());
        }
    }
}
