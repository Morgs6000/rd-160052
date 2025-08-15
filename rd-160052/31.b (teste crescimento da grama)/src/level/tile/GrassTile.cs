namespace RubyDung;

public class GrassTile : Tile {
    public GrassTile(int id) : base(id) {
        tex = 3;
    }

    protected override int GetTexture(int face) {
        if(face == 3) {
            return 0;
        }
        else {
            return face == 2 ? 2 : 3;
        }
    }

    public override void Tick(Level level, int x, int y, int z, Random random) {
        if(!level.IsLit(x, y, z)) {
            level.SetTile(x, y, z, Tile.dirt.id);
        }
        else {
            for(int i = 0; i < 4; ++i) {
                int xt = x + random.Next(3) - 1;
                int yt = y + random.Next(5) - 3;
                int zt = z + random.Next(3) - 1;
                
                if(level.GetTile(xt, yt, zt) == Tile.dirt.id && level.IsLit(xt, yt, zt)) {
                    level.SetTile(xt, yt, zt, Tile.grass.id);
                }
            }
        }
    }
}
