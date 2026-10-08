using Raylib_cs;

namespace SpaceWars;

public enum TipoHabilidade
{
    TiroRapido,      // Helius  — rafaga de 3 tiros
    Escudo,          // Hares   — absorve proximo dano
    TiroPerfurante,  // Cipher  — tiro atravessa inimigos
    CuraAliados,     // Serath  — recupera HP
}

public class Personagem
{
    public string Nome;
    public int HP;
    public int HPMax;
    public int Dano;           // dano base do tiro
    public int Speed;
    public float CadenciaSegundos; // tempo minimo entre tiros (segundos)
    public TipoHabilidade Habilidade;
    public float CooldownHabilidade;   // tempo total de recarga (segundos)
    public Color Cor;                   // cor de destaque no HUD

    // Estado interno
    public bool Ativo;
    public float _timerTiro;          // acumula delta time
    public float _timerHabilidade;    // acumula delta time
    public bool EscudoAtivo;
    public bool TiroPerfuranteAtivo;
    private float _duracaoEfeito = 3f; // duracao de efeitos temporarios
    private float _timerEfeito;
    public string CaminhoNave;

    public Personagem(
        string nome, string caminhoNave, int hp, int dano, int speed,
        float cadencia, TipoHabilidade habilidade,
        float cooldown, Color cor)
    {
        Nome = nome;
        CaminhoNave = caminhoNave;
        HP = hp;
        HPMax = hp;
        Dano = dano;
        Speed = speed;
        CadenciaSegundos = cadencia;
        Habilidade = habilidade;
        CooldownHabilidade = cooldown;
        Cor = cor;

        _timerTiro = cadencia; // começa pronto para atirar
        _timerHabilidade = 0;
    }

    public bool PodeAtirar => _timerTiro >= CadenciaSegundos;
    public bool HabilidadePronta => _timerHabilidade >= CooldownHabilidade;
    public float ProgressoHabilidade => Math.Min(_timerHabilidade / CooldownHabilidade, 1f);

    public void Atualizar(float delta)
    {
        if (_timerTiro < CadenciaSegundos)
            _timerTiro += delta;

        if (_timerHabilidade < CooldownHabilidade)
            _timerHabilidade += delta;

        // Efeitos com duração
        if (EscudoAtivo || TiroPerfuranteAtivo)
        {
            _timerEfeito -= delta;
            if (_timerEfeito <= 0)
            {
                EscudoAtivo = false;
                TiroPerfuranteAtivo = false;
            }
        }
    }

    public void RegistrarTiro()
    {
        _timerTiro = 0;
    }

    // Retorna true se a habilidade foi ativada com sucesso
    public bool UsarHabilidade()
    {
        if (!HabilidadePronta) return false;

        _timerHabilidade = 0;
        _timerEfeito = _duracaoEfeito;

        switch (Habilidade)
        {
            case TipoHabilidade.Escudo:
                EscudoAtivo = true;
                break;
            case TipoHabilidade.TiroPerfurante:
                TiroPerfuranteAtivo = true;
                break;
                // TiroRapido e CuraAliados são tratados externamente
        }

        return true;
    }

    public void ReceberDano(int valor)
    {
        if (EscudoAtivo)
        {
            EscudoAtivo = false; // escudo absorve e quebra
            return;
        }
        HP = Math.Max(0, HP - valor);
    }

    public void Curar(int valor)
    {
        HP = Math.Min(HPMax, HP + valor);
    }

    public bool EstaVivo => HP > 0;
}
