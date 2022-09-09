export function intersects(a: number, b: number, c: number, d: number, p: number, q: number, r: number, s: number) {
    var det: number,  gamma: number,  lambda;
    det = (c - a) * (s - q) - (r - p) * (d - b);
    if (det === 0) {
        return false;
    } else {
        lambda = ((s - q) * (r - a) + (p - r) * (s - b)) / det;
        gamma = ((b - d) * (r - a) + (c - a) * (s - b)) / det;
        return (0 < lambda && lambda < 1) && (0 < gamma && gamma < 1);
    }
};

export function getCanvasStandardColor() {
    return '#0f0f0f';
}