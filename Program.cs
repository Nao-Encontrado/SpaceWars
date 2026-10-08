using Raylib_cs;
using SpaceWars;

Raylib.InitWindow(3000, 1500, "SpaceWars");
Raylib.SetTargetFPS(60);

GerenciadorPersonagens gp = new GerenciadorPersonagens();
GerenciadorFases gf = new GerenciadorFases();
gf.Iniciar();

NaveJogador naveJogador = new NaveJogador(gp);
NaveInimigo naveInimigo = new NaveInimigo(naveJogador, gf.Config.SpriteChefao, gf.Config.HPChefao);

List<Tiro> tiros = new List<Tiro>();
List<Soldado> soldados = new List<Soldado>();
int spawnCont = 0;

bool transicaoFase = false;
int timerTransicao = 0;

while (!Raylib.WindowShouldClose())
{
    float delta = Raylib.GetFrameTime();

    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.Black);
    gf.DesenharFundo();

    // ======= TELA DE VITÓRIA =======
    if (gf.JogoFinalizado)
    {
        int cx = Raylib.GetScreenWidth() / 2;
        int cy = Raylib.GetScreenHeight() / 2;
        Raylib.DrawText("VOCÊ VENCEU!", cx - 200, cy - 40, 80, Color.Yellow);
        Raylib.DrawText("Obrigado por jogar Space Wars", cx - 280, cy + 60, 40, Color.White);
        Raylib.EndDrawing();
        continue;
    }

    // ======= TRANSIÇÃO DE FASE =======
    if (transicaoFase)
    {
        timerTransicao++;
        int cx = Raylib.GetScreenWidth() / 2;
        int cy = Raylib.GetScreenHeight() / 2;
        Raylib.DrawText($"FASE {gf.FaseAtual + 1}", cx - 100, cy - 30, 80, Color.White);

        if (timerTransicao >= 180)
        {
            transicaoFase = false;
            timerTransicao = 0;
            soldados.Clear();
            tiros.Clear();
            spawnCont = 0;
            naveInimigo = new NaveInimigo(naveJogador, gf.Config.SpriteChefao, gf.Config.HPChefao);
        }

        Raylib.EndDrawing();
        continue;
    }

    // ======= ATUALIZAR =======
    gp.Atualizar(delta);
    naveJogador.Atualizar();

    int speed = gp.Atual.Speed;

    if (Input.KeyDown(KeyboardKey.Left)) naveJogador.X -= speed;
    if (Input.KeyDown(KeyboardKey.Right)) naveJogador.X += speed;
    if (Input.KeyDown(KeyboardKey.Up)) naveJogador.Y -= speed;
    if (Input.KeyDown(KeyboardKey.Down)) naveJogador.Y += speed;

    // Tiro normal — Space
    if (Input.KeyDown(KeyboardKey.Space))
    {
        var novosTiros = gp.TentarAtirar(
            naveJogador.X + naveJogador.Width,
            naveJogador.Y + naveJogador.Height / 2f,
            naveInimigo
        );
        foreach (var t in novosTiros)
        {
            if (t.X == 0) { t.X = naveJogador.X + naveJogador.Width; t.Y = naveJogador.Y + naveJogador.Height / 2f; }
            tiros.Add(t);
        }
    }

    // Habilidade — Q
    if (Input.KeyPressed(KeyboardKey.Q))
    {
        var extras = gp.UsarHabilidade(
            naveJogador.X + naveJogador.Width,
            naveJogador.Y + naveJogador.Height / 2f,
            naveInimigo
        );
        tiros.AddRange(extras);
    }

    // Spawn de soldados
    spawnCont++;
    if (spawnCont >= gf.Config.IntervaloSpawn)
    {
        soldados.Add(new Soldado(gf.Config.SpriteSoldado, gf.Config.HPSoldado));
        spawnCont = 0;
    }

    soldados = soldados.Where(s => s.Atualizar(tiros, naveJogador)).ToList();

    // Atualiza chefão
    if (naveInimigo.HP > 0)
        naveInimigo.Atualizar(tiros);

    // Verifica se chefão morreu — avança fase
    if (naveInimigo.HP <= 0)
    {
        bool temProxima = gf.AvancarFase();
        if (temProxima)
        {
            transicaoFase = true;
            timerTransicao = 0;
        }
    }

    // Atualiza tiros e colisões
    tiros = tiros.Where(t =>
    {
        if (!t.Atualizar()) return false;

        if (t.DirecaoX > 0)
        {
            if (naveInimigo.HP > 0 && t.ChecarColisao(naveInimigo)) return false;
            foreach (var s in soldados)
                if (t.ChecarColisao(s)) return false;
        }
        else
        {
            if (t.ChecarColisao(naveJogador)) return false;
        }

        return true;
    }).ToList();

    // ======= DESENHAR =======
    naveJogador.Desenhar();
    Raylib.DrawRectangleLines((int)naveJogador.X, (int)naveJogador.Y, naveJogador.Width, naveJogador.Height, Color.Green);

    if (naveInimigo.HP > 0)
    {
        naveInimigo.Desenhar();
        Raylib.DrawRectangleLines((int)naveInimigo.X, (int)naveInimigo.Y, naveInimigo.Width, naveInimigo.Height, Color.Red);
    }

    foreach (var s in soldados)
    {
        s.Desenhar();
        Raylib.DrawRectangleLines((int)s.X, (int)s.Y, s.Width, s.Height, Color.Red);
    }

    foreach (Tiro tiro in tiros)
        tiro.Desenhar();

    HUD.Desenhar(gp, naveInimigo);

    Raylib.EndDrawing();
}

Raylib.CloseWindow();
