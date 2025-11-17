namespace Test.src.SOLID.O
{
    /// <summary>
    /// Example enumeration of shape kinds; often used in non-OCP designs
    /// that switch on type. Prefer polymorphism over switching when possible.
    /// </summary>
    public enum ShapeType
    {
        Circle,
        Square,

        Rectangle
    }
}
