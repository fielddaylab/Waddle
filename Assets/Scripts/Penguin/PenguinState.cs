using FieldDay.Processes;

namespace Waddle {
    public abstract class PenguinState : IProcessStateCallbacks {
        static private PenguinBrain Brain(Process p) {
            return p.Context<PenguinBrain>();
        }
    }
}