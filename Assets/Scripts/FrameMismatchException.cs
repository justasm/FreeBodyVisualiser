
public class FrameMismatchException : System.Exception {

    public int frames1, frames2;

    public FrameMismatchException(int frames1, int frames2)
        : base(frames1 + " vs " + frames2 + " frames.")
    {
        this.frames1 = frames1;
        this.frames2 = frames2;
    }

}
