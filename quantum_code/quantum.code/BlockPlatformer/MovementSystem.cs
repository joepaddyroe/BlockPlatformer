namespace Quantum.BlockPlatformer;

public unsafe class MovementSystem : SystemMainThreadFilter<MovementSystem.Filter>
{
    public struct Filter
    {
        public EntityRef Entity;
        public CharacterController3D* CharacterController;
    }
    
    public override void Update(Frame f, ref Filter filter)
    {
        // note: pointer property access via -> instead of .
        filter.CharacterController->Move(f, filter.Entity, default);
    }
}

