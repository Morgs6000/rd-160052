namespace RubyDung;

/* ..:: Axis-Aligned Bounding Box ::.. */
public class AABB {
    private float epsilon = 0.0f;

    public float x0;
    public float y0;
    public float z0;

    public float x1;
    public float y1;
    public float z1;

    public AABB(float x0, float y0, float z0, float x1, float y1, float z1) {
        this.x0 = x0;
        this.y0 = y0;
        this.z0 = z0;

        this.x1 = x1;
        this.y1 = y1;
        this.z1 = z1;
    }

    public AABB Expand(float xa, float ya, float za) {
        float _x0 = x0;
        float _y0 = y0;
        float _z0 = z0;
        float _x1 = x1;
        float _y1 = y1;
        float _z1 = z1;

        if(xa < 0.0f) {
            _x0 += xa;
        }
        if(xa > 0.0f) {
            _x1 += xa;
        }

        if(ya < 0.0f) {
            _y0 += ya;
        }
        if(ya > 0.0f) {
            _y1 += ya;
        }

        if(za < 0.0f) {
            _z0 += za;
        }
        if(za > 0.0f) {
            _z1 += za;
        }

        return new AABB(_x0, _y0, _z0, _x1, _y1, _z1);
    }

    public float ClipXCollide(AABB c, float xa) {
        if(!(c.y1 <= y0) && !(c.y0 >= y1)) {
            if(!(c.z1 <= z0) && !(c.z0 >= z1)) {
                float max;
                if(xa > 0.0f && c.x1 <= x0) {
                    max = x0 - c.x1 - epsilon;
                    if(max < xa) {
                        xa = max;
                    }
                }

                if(xa < 0.0f && c.x0 >= x1) {
                    max = x1 - c.x0 + epsilon;
                    if(max > xa) {
                        xa = max;
                    }
                }

                return xa;
            }
            else {
                return xa;
            }
        }
        else {
            return xa;
        }
    }

    public float ClipYCollide(AABB c, float ya) {
        if(!(c.x1 <= x0) && !(c.x0 >= x1)) {
            if(!(c.z1 <= z0) && !(c.z0 >= z1)) {
                float max;
                if(ya > 0.0f && c.y1 <= y0) {
                    max = y0 - c.y1 - epsilon;
                    if(max < ya) {
                        ya = max;
                    }
                }

                if(ya < 0.0f && c.y0 >= y1) {
                    max = y1 - c.y0 + epsilon;
                    if(max > ya) {
                        ya = max;
                    }
                }

                return ya;
            }
            else {
                return ya;
            }
        }
        else {
            return ya;
        }
    }

    public float ClipZCollide(AABB c, float za) {
        if(!(c.x1 <= x0) && !(c.x0 >= x1)) {
            if(!(c.y1 <= y0) && !(c.y0 >= y1)) {
                float max;
                if(za > 0.0f && c.z1 <= z0) {
                    max = z0 - c.z1 - epsilon;
                    if(max < za) {
                        za = max;
                    }
                }

                if(za < 0.0f && c.z0 >= z1) {
                    max = z1 - c.z0 + epsilon;
                    if(max > za) {
                        za = max;
                    }
                }

                return za;
            }
            else {
                return za;
            }
        }
        else {
            return za;
        }
    }

    public void Move(float xa, float ya, float za) {
        x0 += xa;
        y0 += ya;
        z0 += za;
        x1 += xa;
        y1 += ya;
        z1 += za;
    }
}
