using Raylib_cs;

namespace SpaceWars;

public static class Input
{
    public static bool KeyDown(KeyboardKey tecla)
    {
        return Raylib.IsKeyDown(tecla);
    }

    public static bool KeyPressed(KeyboardKey tecla)
    {
        return Raylib.IsKeyPressed(tecla);
    }
}