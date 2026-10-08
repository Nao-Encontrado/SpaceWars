using System.Numerics;
using Raylib_cs;

namespace SpaceWars;
/// Desenha o HUD dos personagens na parte inferior da tela.
/// Cada card mostra: nome, HP, barra de cooldown e indicador de ativo.
public static class HUD
{
    private const int CardW = 200;
    private const int CardH = 90;
    private const int Margem = 12;

    public static void Desenhar(GerenciadorPersonagens gp, NaveInimigo inimigo)
    {
        int screenW = Raylib.GetScreenWidth();
        int screenH = Raylib.GetScreenHeight();

        int totalW = gp.Personagens.Count * CardW + (gp.Personagens.Count - 1) * Margem;
        int startX = (screenW - totalW) / 2;
        int startY = screenH - CardH - 20;

        for (int i = 0; i < gp.Personagens.Count; i++)
        {
            var p = gp.Personagens[i];
            int cx = startX + i * (CardW + Margem);

            // Fundo do card
            Color fundoCor = p.EstaVivo
                ? new Color(20, 20, 30, 220)
                : new Color(10, 10, 10, 180);

            Raylib.DrawRectangle(cx, startY, CardW, CardH, fundoCor);

            // Borda — destaca o ativo
            Color borda = (i == gp.IndiceAtual) ? p.Cor : new Color(60, 60, 80, 255);
            int espessura = (i == gp.IndiceAtual) ? 3 : 1;
            Raylib.DrawRectangleLinesEx(new Rectangle(cx, startY, CardW, CardH), espessura, borda);

            if (!p.EstaVivo)
            {
                Raylib.DrawText("FORA", cx + CardW / 2 - 20, startY + CardH / 2 - 10, 20, Color.Gray);
                continue;
            }

            // Nome + tecla
            string label = $"[{i + 1}] {p.Nome}";
            Raylib.DrawText(label, cx + 8, startY + 8, 18, p.Cor);

            // Barra de HP
            float pctHP = (float)p.HP / p.HPMax;
            Raylib.DrawRectangle(cx + 8, startY + 32, CardW - 16, 12, new Color(60, 20, 20, 255));
            Raylib.DrawRectangle(cx + 8, startY + 32, (int)((CardW - 16) * pctHP), 12, Color.Red);
            Raylib.DrawText($"HP {p.HP}/{p.HPMax}", cx + 8, startY + 47, 14, Color.White);

            // Barra de cooldown da habilidade
            float pctCd = p.ProgressoHabilidade;
            Color corCd = p.HabilidadePronta ? Color.Yellow : new Color(80, 80, 40, 255);
            Raylib.DrawRectangle(cx + 8, startY + 68, CardW - 16, 10, new Color(40, 40, 20, 255));
            Raylib.DrawRectangle(cx + 8, startY + 68, (int)((CardW - 16) * pctCd), 10, corCd);

            string nomeHab = p.Habilidade switch
            {
                TipoHabilidade.TiroRapido     => "[Q] Rajada",
                TipoHabilidade.Escudo         => "[Q] Escudo",
                TipoHabilidade.TiroPerfurante => "[Q] Perfura",
                TipoHabilidade.CuraAliados    => "[Q] Cura",
                _                             => "[Q]"
            };
            Color corTextoHab = p.HabilidadePronta ? Color.Yellow : Color.Gray;
            Raylib.DrawText(nomeHab, cx + 8, startY + 80, 12, corTextoHab);
        }

        // HP do inimigo no topo
        Raylib.DrawText($"HP Inimigo: {inimigo.HP}", 20, 10, 22, Color.Red);

        // Indicador de escudo/perfurante ativos
        var ativo = gp.Atual;
        if (ativo.EscudoAtivo)
            Raylib.DrawText("ESCUDO ATIVO", screenW / 2 - 80, 15, 24, Color.SkyBlue);
        if (ativo.TiroPerfuranteAtivo)
            Raylib.DrawText("TIRO PERFURANTE", screenW / 2 - 100, 15, 24, Color.Purple);
    }
}
