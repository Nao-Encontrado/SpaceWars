using Raylib_cs;
using System.Numerics;

namespace SpaceWars;

public class Fase
{
    public string Background;
    public string SpriteSoldado;
    public string SpriteChefao;
    public int HPChefao;
    public int HPSoldado;
    public int IntervaloSpawn; // frames entre spawns de soldado

    public Fase(string background, string spriteSoldado, string spriteChefao,
                int hpChefao, int hpSoldado, int intervaloSpawn)
    {
        Background = background;
        SpriteSoldado = spriteSoldado;
        SpriteChefao = spriteChefao;
        HPChefao = hpChefao;
        HPSoldado = hpSoldado;
        IntervaloSpawn = intervaloSpawn;
    }
}

public class GerenciadorFases
{
    public int FaseAtual { get; private set; } = 0;
    public bool JogoFinalizado { get; private set; } = false;

    private List<Fase> _fases = new List<Fase>
    {
        new Fase("./assets/fundo1.png", "./assets/naveSoldado1.png", "./assets/naveInimigo1.png",  100, 40,  180),
        new Fase("./assets/fundo2.png", "./assets/naveSoldado2.png", "./assets/naveInimigo2.png", 150, 60,  150),
        new Fase("./assets/fundo3.png", "./assets/naveSoldado3.png", "./assets/naveInimigo3.png", 200, 80,  120),
        new Fase("./assets/fundo4.png", "./assets/naveSoldado4.png", "./assets/naveInimigo4.png", 250, 100, 90),
    };

    private Texture2D _textureFundo;
    private string _fundoAtual = "";

    public Fase Config => _fases[FaseAtual];

    public void CarregarFundo()
    {
        string caminho = _fases[FaseAtual].Background;
        if (_fundoAtual != caminho)
        {
            if (_fundoAtual != "") Raylib.UnloadTexture(_textureFundo);
            _textureFundo = Raylib.LoadTexture(caminho);
            _fundoAtual = caminho;
        }
    }

    public void DesenharFundo()
    {
        int w = Raylib.GetScreenWidth();
        int h = Raylib.GetScreenHeight();
        Rectangle source = new Rectangle(0, 0, _textureFundo.Width, _textureFundo.Height);
        Rectangle dest   = new Rectangle(0, 0, w, h);
        Raylib.DrawTexturePro(_textureFundo, source, dest, new Vector2(0, 0), 0, Color.White);
    }

    // Chamado quando o chefão morre — avança a fase
    public bool AvancarFase()
    {
        if (FaseAtual < _fases.Count - 1)
        {
            FaseAtual++;
            CarregarFundo();
            return true;
        }
        else
        {
            JogoFinalizado = true;
            return false;
        }
    }

    public void Iniciar()
    {
        CarregarFundo();
    }
}
